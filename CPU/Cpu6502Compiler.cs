using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU
{
    public class Cpu6502Compiler : IDisposable
    {
        record FillableLink(string Name, long Position, int Size);

        bool _disposed;
        readonly MemoryStream _writer;
        readonly Reader _reader;
        readonly Dictionary<string, ushort> _links;
        readonly List<FillableLink> _fillableLinks;

        Cpu6502Compiler(string inFilepath)
        {
            _disposed = false;
            _reader = new Reader(inFilepath);
            _writer = new MemoryStream(new byte[ushort.MaxValue + 1]);
            _links = new();
            _fillableLinks = new();
        }
        ~Cpu6502Compiler() => Dispose(false);

        void CompileLines()
        {
            while (!_reader.EOS)
                CompileLine();

            foreach (var fl in _fillableLinks)
            {
                var value = _links[fl.Name];
                _writer.Seek(fl.Position, SeekOrigin.Begin);
                for (int i = 0; i < fl.Size; i++)
                {
                    byte b = (byte)(value & 0xFF);
                    _writer.WriteByte(b);
                    value = (ushort)(value >> 8);
                }
            }
        }

        void CompileLine()
        {
            var c = _reader.Peek();
            if (!(char.IsLetter(c) || c == '\t'))
            {
                _reader.ToNextLine();
                return;
            }

            if (c == '\t')
                CompileInstruction();
            else if (char.IsUpper(c))
            {
                string v = _reader.GetWord();
                var eq = _reader.GetRawString();
                if (!eq.Contains('='))
                    throw new NotImplementedException();
                var vt = _reader.GetValue(out var s, out var ln);
                var val = s ?? throw new NullReferenceException();
                _links.Add(v, val);
            }
            else
                _links.Add(_reader.GetWord(), (ushort)_writer.Position);

            _reader.ToNextLine();
        }

        void CompileInstruction()
        {
            _ = _reader.Read();
            var c = _reader.Peek();
            if (c == '.')
                CompileASMInstruction();
            else
                CompileCpuInstruction();
        }

        void CompileASMInstruction()
        {
            string instruction = _reader.GetWord();
            ushort? value;
            string? linkName;
            switch (instruction)
            {
                case "org":
                    _reader.GetUInt16(out value, out linkName);
                    var offset = value ?? throw new NullReferenceException();
                    _writer.Seek(offset, SeekOrigin.Begin);
                    break;
                case "word":
                    _reader.GetUInt16(out value, out linkName);
                    if (value == null && linkName != null)
                        _fillableLinks.Add(new(linkName, _writer.Position, 2));
                    var word = value ?? 0;
                    _writer.WriteByte((byte)(word & 0xFF));
                    word = (ushort)(word >> 8);
                    _writer.WriteByte((byte)(word & 0xFF));
                    break;
                case "asciiz":
                    var s = _reader.GetExplicitString() + (char)0;
                    var arr = s.Select(c => (byte)c).ToArray();
                    _writer.Write(arr);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        void CompileCpuInstruction()
        {
            string instruction = _reader.GetWord();
            switch (instruction)
            {
                case Cpu6502StringInstructions.LDA:
                    WriteLDA();
                    break;
                case Cpu6502StringInstructions.LDX:
                    WriteLDX();
                    break;
                case Cpu6502StringInstructions.LDY:
                    WriteLDY();
                    break;
                case Cpu6502StringInstructions.STA:
                    WriteSTA();
                    break;
                case Cpu6502StringInstructions.STX:
                    WriteSTX();
                    break;
                case Cpu6502StringInstructions.STY:
                    WriteSTY();
                    break;
                case Cpu6502StringInstructions.AND:
                    WriteAND();
                    break;
                case Cpu6502StringInstructions.EOR:
                    WriteEOR();
                    break;
                case Cpu6502StringInstructions.ORA:
                    WriteORA();
                    break;
                case Cpu6502StringInstructions.BIT:
                    WriteBIT();
                    break;
                case Cpu6502StringInstructions.ADC:
                    WriteADC();
                    break;
                case Cpu6502StringInstructions.SBC:
                    WriteSBC();
                    break;
                case Cpu6502StringInstructions.CMP:
                    WriteCMP();
                    break;
                case Cpu6502StringInstructions.CPX:
                    WriteCPX();
                    break;
                case Cpu6502StringInstructions.CPY:
                    WriteCPY();
                    break;
                case Cpu6502StringInstructions.INC:
                    WriteINC();
                    break;
                case Cpu6502StringInstructions.DEC:
                    WriteDEC();
                    break;
                case Cpu6502StringInstructions.ASL:
                    WriteASL();
                    break;
                case Cpu6502StringInstructions.LSR:
                    WriteLSR();
                    break;
                case Cpu6502StringInstructions.ROL:
                    WriteROL();
                    break;
                case Cpu6502StringInstructions.ROR:
                    WriteROR();
                    break;
                case Cpu6502StringInstructions.JMP:
                    WriteJMP();
                    break;
                case Cpu6502StringInstructions.JSR:
                    WriteJSR();
                    break;
                case Cpu6502StringInstructions.BCC:
                    WriteBCC();
                    break;
                case Cpu6502StringInstructions.BCS:
                    WriteBCS();
                    break;
                case Cpu6502StringInstructions.BEQ:
                    WriteBEQ();
                    break;
                case Cpu6502StringInstructions.BMI:
                    WriteBMI();
                    break;
                case Cpu6502StringInstructions.BNE:
                    WriteBNE();
                    break;
                case Cpu6502StringInstructions.BPL:
                    WriteBPL();
                    break;
                case Cpu6502StringInstructions.BVC:
                    WriteBVC();
                    break;
                case Cpu6502StringInstructions.BVS:
                    WriteBVS();
                    break;
                default:
                    var bc = instruction switch
                    {
                        Cpu6502StringInstructions.TAX => Cpu6502ByteCodesInstructions.TAX,
                        Cpu6502StringInstructions.TAY => Cpu6502ByteCodesInstructions.TAY,
                        Cpu6502StringInstructions.TXA => Cpu6502ByteCodesInstructions.TXA,
                        Cpu6502StringInstructions.TYA => Cpu6502ByteCodesInstructions.TYA,

                        Cpu6502StringInstructions.TSX => Cpu6502ByteCodesInstructions.TSX,
                        Cpu6502StringInstructions.TXS => Cpu6502ByteCodesInstructions.TXS,
                        Cpu6502StringInstructions.PHA => Cpu6502ByteCodesInstructions.PHA,
                        Cpu6502StringInstructions.PHP => Cpu6502ByteCodesInstructions.PHP,
                        Cpu6502StringInstructions.PLA => Cpu6502ByteCodesInstructions.PLA,
                        Cpu6502StringInstructions.PLP => Cpu6502ByteCodesInstructions.PLP,

                        Cpu6502StringInstructions.INX => Cpu6502ByteCodesInstructions.INX,
                        Cpu6502StringInstructions.INY => Cpu6502ByteCodesInstructions.INY,
                        Cpu6502StringInstructions.DEX => Cpu6502ByteCodesInstructions.DEX,
                        Cpu6502StringInstructions.DEY => Cpu6502ByteCodesInstructions.DEY,

                        Cpu6502StringInstructions.RTS => Cpu6502ByteCodesInstructions.RTS,

                        Cpu6502StringInstructions.CLC => Cpu6502ByteCodesInstructions.CLC,
                        Cpu6502StringInstructions.CLD => Cpu6502ByteCodesInstructions.CLD,
                        Cpu6502StringInstructions.CLI => Cpu6502ByteCodesInstructions.CLI,
                        Cpu6502StringInstructions.CLV => Cpu6502ByteCodesInstructions.CLV,
                        Cpu6502StringInstructions.SEC => Cpu6502ByteCodesInstructions.SEC,
                        Cpu6502StringInstructions.SED => Cpu6502ByteCodesInstructions.SED,
                        Cpu6502StringInstructions.SEI => Cpu6502ByteCodesInstructions.SEI,

                        Cpu6502StringInstructions.BRK => Cpu6502ByteCodesInstructions.BRK,
                        Cpu6502StringInstructions.NOP => Cpu6502ByteCodesInstructions.NOP,
                        Cpu6502StringInstructions.RTI => Cpu6502ByteCodesInstructions.RTI,
                        _ => throw new NotImplementedException($"Instruction not implemented: 0x{instruction:X2} ({instruction})")
                    };
                    _writer.WriteByte(bc);
                    break;
            }
        }

        #region Load Operations
        void WriteLDA() // Load Accumulator
        {
            FillableLink fl = new("", 0, 1);
            var vt = _reader.GetValue(out var nullableS, out var ln);
            if (ln != null)
                fl = fl with { Name = ln };
            var s = nullableS ?? 0;
            var bc = vt switch
            {
                Reader.ValueTypes.Immediate => Cpu6502ByteCodesInstructions.LDA_IMMEDIATE,
                Reader.ValueTypes.ZeroPage  => Cpu6502ByteCodesInstructions.LDA_ZEROPAGE,
                Reader.ValueTypes.ZeroPageX => Cpu6502ByteCodesInstructions.LDA_ZEROPAGE_X,
                Reader.ValueTypes.Absolute  => Cpu6502ByteCodesInstructions.LDA_ABSOLUTE,
                Reader.ValueTypes.AbsoluteX => Cpu6502ByteCodesInstructions.LDA_ABSOLUTE_X,
                Reader.ValueTypes.AbsoluteY => Cpu6502ByteCodesInstructions.LDA_ABSOLUTE_Y,
                Reader.ValueTypes.IndirectX => Cpu6502ByteCodesInstructions.LDA_INDIRECT_X,
                Reader.ValueTypes.IndirectY => Cpu6502ByteCodesInstructions.LDA_INDIRECT_Y,
                _ => throw new NotImplementedException(),
            };
            _writer.WriteByte(bc);

            fl = fl with { Position = _writer.Position };
            _writer.WriteByte((byte)(s & 0xFF));
            if (vt.HasFlag(Reader.ValueTypes.Absolute))
            {
                fl = fl with { Size = fl.Size + 1 };
                s = (ushort)(s >> 8);
                _writer.WriteByte((byte)(s & 0xFF));
            }
            if (nullableS == null && ln != null)
                _fillableLinks.Add(fl);
        }
        void WriteLDX() // Load X Register
        {
            FillableLink fl = new("", 0, 1);
            var vt = _reader.GetValue(out var nullableS, out var ln);
            if (ln != null)
                fl = fl with { Name = ln };
            var s = nullableS ?? 0;
            var bc = vt switch
            {
                Reader.ValueTypes.Immediate => Cpu6502ByteCodesInstructions.LDX_IMMEDIATE,
                Reader.ValueTypes.ZeroPage  => Cpu6502ByteCodesInstructions.LDX_ZEROPAGE,
                Reader.ValueTypes.ZeroPageY => Cpu6502ByteCodesInstructions.LDX_ZEROPAGE_Y,
                Reader.ValueTypes.Absolute  => Cpu6502ByteCodesInstructions.LDX_ABSOLUTE,
                Reader.ValueTypes.AbsoluteY => Cpu6502ByteCodesInstructions.LDX_ABSOLUTE_Y,
                _ => throw new NotImplementedException(),
            };
            _writer.WriteByte(bc);

            fl = fl with { Position = _writer.Position };
            _writer.WriteByte((byte)(s & 0xFF));
            if (vt.HasFlag(Reader.ValueTypes.Absolute))
            {
                fl = fl with { Size = fl.Size + 1 };
                s = (ushort)(s >> 8);
                _writer.WriteByte((byte)(s & 0xFF));
            }
            if (nullableS == null && ln != null)
                _fillableLinks.Add(fl);
        }
        void WriteLDY() // Load Y Register
        {
            FillableLink fl = new("", 0, 1);
            var vt = _reader.GetValue(out var nullableS, out var ln);
            if (ln != null)
                fl = fl with { Name = ln };
            var s = nullableS ?? 0;
            var bc = vt switch
            {
                Reader.ValueTypes.Immediate => Cpu6502ByteCodesInstructions.LDY_IMMEDIATE,
                Reader.ValueTypes.ZeroPage  => Cpu6502ByteCodesInstructions.LDY_ZEROPAGE,
                Reader.ValueTypes.ZeroPageX => Cpu6502ByteCodesInstructions.LDY_ZEROPAGE_X,
                Reader.ValueTypes.Absolute  => Cpu6502ByteCodesInstructions.LDY_ABSOLUTE,
                Reader.ValueTypes.AbsoluteX => Cpu6502ByteCodesInstructions.LDY_ABSOLUTE_X,
                _ => throw new NotImplementedException(),
            };
            _writer.WriteByte(bc);

            fl = fl with { Position = _writer.Position };
            _writer.WriteByte((byte)(s & 0xFF));
            if (vt.HasFlag(Reader.ValueTypes.Absolute))
            {
                fl = fl with { Size = fl.Size + 1 };
                s = (ushort)(s >> 8);
                _writer.WriteByte((byte)(s & 0xFF));
            }
            if (nullableS == null && ln != null)
                _fillableLinks.Add(fl);
        }
        #endregion

        #region Store Operations
        void WriteSTA() // Store Accumulator
        {
            FillableLink fl = new("", 0, 1);
            var vt = _reader.GetValue(out var nullableS, out var ln);
            if (ln != null)
                fl = fl with { Name = ln };
            var s = nullableS ?? 0;
            var bc = vt switch
            {
                Reader.ValueTypes.ZeroPage  => Cpu6502ByteCodesInstructions.STA_ZEROPAGE,
                Reader.ValueTypes.ZeroPageX => Cpu6502ByteCodesInstructions.STA_ZEROPAGE_X,
                Reader.ValueTypes.Absolute  => Cpu6502ByteCodesInstructions.STA_ABSOLUTE,
                Reader.ValueTypes.AbsoluteX => Cpu6502ByteCodesInstructions.STA_ABSOLUTE_X,
                Reader.ValueTypes.AbsoluteY => Cpu6502ByteCodesInstructions.STA_ABSOLUTE_Y,
                Reader.ValueTypes.IndirectX => Cpu6502ByteCodesInstructions.STA_INDIRECT_X,
                Reader.ValueTypes.IndirectY => Cpu6502ByteCodesInstructions.STA_INDIRECT_Y,
                _ => throw new NotImplementedException(),
            };
            _writer.WriteByte(bc);

            fl = fl with { Position = _writer.Position };
            _writer.WriteByte((byte)(s & 0xFF));
            if (vt.HasFlag(Reader.ValueTypes.Absolute))
            {
                fl = fl with { Size = fl.Size + 1 };
                s = (ushort)(s >> 8);
                _writer.WriteByte((byte)(s & 0xFF));
            }
            if (nullableS == null && ln != null)
                _fillableLinks.Add(fl);
        }
        void WriteSTX() // Store X Register
        {
            FillableLink fl = new("", 0, 1);
            var vt = _reader.GetValue(out var nullableS, out var ln);
            if (ln != null)
                fl = fl with { Name = ln };
            var s = nullableS ?? 0;
            var bc = vt switch
            {
                Reader.ValueTypes.ZeroPage  => Cpu6502ByteCodesInstructions.STX_ZEROPAGE,
                Reader.ValueTypes.ZeroPageY => Cpu6502ByteCodesInstructions.STX_ZEROPAGE_Y,
                Reader.ValueTypes.Absolute  => Cpu6502ByteCodesInstructions.STX_ABSOLUTE,
                _ => throw new NotImplementedException(),
            };
            _writer.WriteByte(bc);

            fl = fl with { Position = _writer.Position };
            _writer.WriteByte((byte)(s & 0xFF));
            if (vt.HasFlag(Reader.ValueTypes.Absolute))
            {
                fl = fl with { Size = fl.Size + 1 };
                s = (ushort)(s >> 8);
                _writer.WriteByte((byte)(s & 0xFF));
            }
            if (nullableS == null && ln != null)
                _fillableLinks.Add(fl);
        }
        void WriteSTY() // Store Y Register
        {
            FillableLink fl = new("", 0, 1);
            var vt = _reader.GetValue(out var nullableS, out var ln);
            if (ln != null)
                fl = fl with { Name = ln };
            var s = nullableS ?? 0;
            var bc = vt switch
            {
                Reader.ValueTypes.ZeroPage  => Cpu6502ByteCodesInstructions.STY_ZEROPAGE,
                Reader.ValueTypes.ZeroPageX => Cpu6502ByteCodesInstructions.STY_ZEROPAGE_X,
                Reader.ValueTypes.Absolute  => Cpu6502ByteCodesInstructions.STY_ABSOLUTE,
                _ => throw new NotImplementedException(),
            };
            _writer.WriteByte(bc);

            fl = fl with { Position = _writer.Position };
            _writer.WriteByte((byte)(s & 0xFF));
            if (vt.HasFlag(Reader.ValueTypes.Absolute))
            {
                fl = fl with { Size = fl.Size + 1 };
                s = (ushort)(s >> 8);
                _writer.WriteByte((byte)(s & 0xFF));
            }
            if (nullableS == null && ln != null)
                _fillableLinks.Add(fl);
        }
        #endregion

        #region Logical
        void WriteAND() // Logical AND
        {
            FillableLink fl = new("", 0, 1);
            var vt = _reader.GetValue(out var nullableS, out var ln);
            if (ln != null)
                fl = fl with { Name = ln };
            var s = nullableS ?? 0;
            var bc = vt switch
            {
                Reader.ValueTypes.Immediate => Cpu6502ByteCodesInstructions.AND_IMMEDIATE,
                Reader.ValueTypes.ZeroPage  => Cpu6502ByteCodesInstructions.AND_ZEROPAGE,
                Reader.ValueTypes.ZeroPageX => Cpu6502ByteCodesInstructions.AND_ZEROPAGE_X,
                Reader.ValueTypes.Absolute  => Cpu6502ByteCodesInstructions.AND_ABSOLUTE,
                Reader.ValueTypes.AbsoluteX => Cpu6502ByteCodesInstructions.AND_ABSOLUTE_X,
                Reader.ValueTypes.AbsoluteY => Cpu6502ByteCodesInstructions.AND_ABSOLUTE_Y,
                Reader.ValueTypes.IndirectX => Cpu6502ByteCodesInstructions.AND_INDIRECT_X,
                Reader.ValueTypes.IndirectY => Cpu6502ByteCodesInstructions.AND_INDIRECT_Y,
                _ => throw new NotImplementedException(),
            };
            _writer.WriteByte(bc);

            fl = fl with { Position = _writer.Position };
            _writer.WriteByte((byte)(s & 0xFF));
            if (vt.HasFlag(Reader.ValueTypes.Absolute))
            {
                fl = fl with { Size = fl.Size + 1 };
                s = (ushort)(s >> 8);
                _writer.WriteByte((byte)(s & 0xFF));
            }
            if (nullableS == null && ln != null)
                _fillableLinks.Add(fl);
        }
        void WriteEOR() // Exclusive OR
        {
            FillableLink fl = new("", 0, 1);
            var vt = _reader.GetValue(out var nullableS, out var ln);
            if (ln != null)
                fl = fl with { Name = ln };
            var s = nullableS ?? 0;
            var bc = vt switch
            {
                Reader.ValueTypes.Immediate => Cpu6502ByteCodesInstructions.EOR_IMMEDIATE,
                Reader.ValueTypes.ZeroPage  => Cpu6502ByteCodesInstructions.EOR_ZEROPAGE,
                Reader.ValueTypes.ZeroPageX => Cpu6502ByteCodesInstructions.EOR_ZEROPAGE_X,
                Reader.ValueTypes.Absolute  => Cpu6502ByteCodesInstructions.EOR_ABSOLUTE,
                Reader.ValueTypes.AbsoluteX => Cpu6502ByteCodesInstructions.EOR_ABSOLUTE_X,
                Reader.ValueTypes.AbsoluteY => Cpu6502ByteCodesInstructions.EOR_ABSOLUTE_Y,
                Reader.ValueTypes.IndirectX => Cpu6502ByteCodesInstructions.EOR_INDIRECT_X,
                Reader.ValueTypes.IndirectY => Cpu6502ByteCodesInstructions.EOR_INDIRECT_Y,
                _ => throw new NotImplementedException(),
            };
            _writer.WriteByte(bc);

            fl = fl with { Position = _writer.Position };
            _writer.WriteByte((byte)(s & 0xFF));
            if (vt.HasFlag(Reader.ValueTypes.Absolute))
            {
                fl = fl with { Size = fl.Size + 1 };
                s = (ushort)(s >> 8);
                _writer.WriteByte((byte)(s & 0xFF));
            }
            if (nullableS == null && ln != null)
                _fillableLinks.Add(fl);
        }
        void WriteORA() // Logical Inclusive OR
        {
            FillableLink fl = new("", 0, 1);
            var vt = _reader.GetValue(out var nullableS, out var ln);
            if (ln != null)
                fl = fl with { Name = ln };
            var s = nullableS ?? 0;
            var bc = vt switch
            {
                Reader.ValueTypes.Immediate => Cpu6502ByteCodesInstructions.ORA_IMMEDIATE,
                Reader.ValueTypes.ZeroPage  => Cpu6502ByteCodesInstructions.ORA_ZEROPAGE,
                Reader.ValueTypes.ZeroPageX => Cpu6502ByteCodesInstructions.ORA_ZEROPAGE_X,
                Reader.ValueTypes.Absolute  => Cpu6502ByteCodesInstructions.ORA_ABSOLUTE,
                Reader.ValueTypes.AbsoluteX => Cpu6502ByteCodesInstructions.ORA_ABSOLUTE_X,
                Reader.ValueTypes.AbsoluteY => Cpu6502ByteCodesInstructions.ORA_ABSOLUTE_Y,
                Reader.ValueTypes.IndirectX => Cpu6502ByteCodesInstructions.ORA_INDIRECT_X,
                Reader.ValueTypes.IndirectY => Cpu6502ByteCodesInstructions.ORA_INDIRECT_Y,
                _ => throw new NotImplementedException(),
            };
            _writer.WriteByte(bc);

            fl = fl with { Position = _writer.Position };
            _writer.WriteByte((byte)(s & 0xFF));
            if (vt.HasFlag(Reader.ValueTypes.Absolute))
            {
                fl = fl with { Size = fl.Size + 1 };
                s = (ushort)(s >> 8);
                _writer.WriteByte((byte)(s & 0xFF));
            }
            if (nullableS == null && ln != null)
                _fillableLinks.Add(fl);
        }
        void WriteBIT() // Bit Test
        {
            FillableLink fl = new("", 0, 1);
            var vt = _reader.GetValue(out var nullableS, out var ln);
            if (ln != null)
                fl = fl with { Name = ln };
            var s = nullableS ?? 0;
            var bc = vt switch
            {
                Reader.ValueTypes.ZeroPage  => Cpu6502ByteCodesInstructions.BIT_ZEROPAGE,
                Reader.ValueTypes.Absolute  => Cpu6502ByteCodesInstructions.BIT_ABSOLUTE,
                _ => throw new NotImplementedException(),
            };
            _writer.WriteByte(bc);

            fl = fl with { Position = _writer.Position };
            _writer.WriteByte((byte)(s & 0xFF));
            if (vt.HasFlag(Reader.ValueTypes.Absolute))
            {
                fl = fl with { Size = fl.Size + 1 };
                s = (ushort)(s >> 8);
                _writer.WriteByte((byte)(s & 0xFF));
            }
            if (nullableS == null && ln != null)
                _fillableLinks.Add(fl);
        }
        #endregion

        #region Arithmetic
        void WriteADC() // Add with Carry
        {
            FillableLink fl = new("", 0, 1);
            var vt = _reader.GetValue(out var nullableS, out var ln);
            if (ln != null)
                fl = fl with { Name = ln };
            var s = nullableS ?? 0;
            var bc = vt switch
            {
                Reader.ValueTypes.Immediate => Cpu6502ByteCodesInstructions.ADC_IMMEDIATE,
                Reader.ValueTypes.ZeroPage  => Cpu6502ByteCodesInstructions.ADC_ZEROPAGE,
                Reader.ValueTypes.ZeroPageX => Cpu6502ByteCodesInstructions.ADC_ZEROPAGE_X,
                Reader.ValueTypes.Absolute  => Cpu6502ByteCodesInstructions.ADC_ABSOLUTE,
                Reader.ValueTypes.AbsoluteX => Cpu6502ByteCodesInstructions.ADC_ABSOLUTE_X,
                Reader.ValueTypes.AbsoluteY => Cpu6502ByteCodesInstructions.ADC_ABSOLUTE_Y,
                Reader.ValueTypes.IndirectX => Cpu6502ByteCodesInstructions.ADC_INDIRECT_X,
                Reader.ValueTypes.IndirectY => Cpu6502ByteCodesInstructions.ADC_INDIRECT_Y,
                _ => throw new NotImplementedException(),
            };
            _writer.WriteByte(bc);

            fl = fl with { Position = _writer.Position };
            _writer.WriteByte((byte)(s & 0xFF));
            if (vt.HasFlag(Reader.ValueTypes.Absolute))
            {
                fl = fl with { Size = fl.Size + 1 };
                s = (ushort)(s >> 8);
                _writer.WriteByte((byte)(s & 0xFF));
            }
            if (nullableS == null && ln != null)
                _fillableLinks.Add(fl);
        }
        void WriteSBC() // Substract with Carry
        {
            FillableLink fl = new("", 0, 1);
            var vt = _reader.GetValue(out var nullableS, out var ln);
            if (ln != null)
                fl = fl with { Name = ln };
            var s = nullableS ?? 0;
            var bc = vt switch
            {
                Reader.ValueTypes.Immediate => Cpu6502ByteCodesInstructions.SBC_IMMEDIATE,
                Reader.ValueTypes.ZeroPage  => Cpu6502ByteCodesInstructions.SBC_ZEROPAGE,
                Reader.ValueTypes.ZeroPageX => Cpu6502ByteCodesInstructions.SBC_ZEROPAGE_X,
                Reader.ValueTypes.Absolute  => Cpu6502ByteCodesInstructions.SBC_ABSOLUTE,
                Reader.ValueTypes.AbsoluteX => Cpu6502ByteCodesInstructions.SBC_ABSOLUTE_X,
                Reader.ValueTypes.AbsoluteY => Cpu6502ByteCodesInstructions.SBC_ABSOLUTE_Y,
                Reader.ValueTypes.IndirectX => Cpu6502ByteCodesInstructions.SBC_INDIRECT_X,
                Reader.ValueTypes.IndirectY => Cpu6502ByteCodesInstructions.SBC_INDIRECT_Y,
                _ => throw new NotImplementedException(),
            };
            _writer.WriteByte(bc);

            fl = fl with { Position = _writer.Position };
            _writer.WriteByte((byte)(s & 0xFF));
            if (vt.HasFlag(Reader.ValueTypes.Absolute))
            {
                fl = fl with { Size = fl.Size + 1 };
                s = (ushort)(s >> 8);
                _writer.WriteByte((byte)(s & 0xFF));
            }
            if (nullableS == null && ln != null)
                _fillableLinks.Add(fl);
        }
        void WriteCMP() // Compare Accumulator
        {
            FillableLink fl = new("", 0, 1);
            var vt = _reader.GetValue(out var nullableS, out var ln);
            if (ln != null)
                fl = fl with { Name = ln };
            var s = nullableS ?? 0;
            var bc = vt switch
            {
                Reader.ValueTypes.Immediate => Cpu6502ByteCodesInstructions.CMP_IMMEDIATE,
                Reader.ValueTypes.ZeroPage  => Cpu6502ByteCodesInstructions.CMP_ZEROPAGE,
                Reader.ValueTypes.ZeroPageX => Cpu6502ByteCodesInstructions.CMP_ZEROPAGE_X,
                Reader.ValueTypes.Absolute  => Cpu6502ByteCodesInstructions.CMP_ABSOLUTE,
                Reader.ValueTypes.AbsoluteX => Cpu6502ByteCodesInstructions.CMP_ABSOLUTE_X,
                Reader.ValueTypes.AbsoluteY => Cpu6502ByteCodesInstructions.CMP_ABSOLUTE_Y,
                Reader.ValueTypes.IndirectX => Cpu6502ByteCodesInstructions.CMP_INDIRECT_X,
                Reader.ValueTypes.IndirectY => Cpu6502ByteCodesInstructions.CMP_INDIRECT_Y,
                _ => throw new NotImplementedException(),
            };
            _writer.WriteByte(bc);

            fl = fl with { Position = _writer.Position };
            _writer.WriteByte((byte)(s & 0xFF));
            if (vt.HasFlag(Reader.ValueTypes.Absolute))
            {
                fl = fl with { Size = fl.Size + 1 };
                s = (ushort)(s >> 8);
                _writer.WriteByte((byte)(s & 0xFF));
            }
            if (nullableS == null && ln != null)
                _fillableLinks.Add(fl);
        }
        void WriteCPX() // Compare X Register
        {
            FillableLink fl = new("", 0, 1);
            var vt = _reader.GetValue(out var nullableS, out var ln);
            if (ln != null)
                fl = fl with { Name = ln };
            var s = nullableS ?? 0;
            var bc = vt switch
            {
                Reader.ValueTypes.Immediate => Cpu6502ByteCodesInstructions.CPX_IMMEDIATE,
                Reader.ValueTypes.ZeroPage  => Cpu6502ByteCodesInstructions.CPX_ZEROPAGE,
                Reader.ValueTypes.Absolute  => Cpu6502ByteCodesInstructions.CPX_ABSOLUTE,
                _ => throw new NotImplementedException(),
            };
            _writer.WriteByte(bc);

            fl = fl with { Position = _writer.Position };
            _writer.WriteByte((byte)(s & 0xFF));
            if (vt.HasFlag(Reader.ValueTypes.Absolute))
            {
                fl = fl with { Size = fl.Size + 1 };
                s = (ushort)(s >> 8);
                _writer.WriteByte((byte)(s & 0xFF));
            }
            if (nullableS == null && ln != null)
                _fillableLinks.Add(fl);
        }
        void WriteCPY() // Compare Y Register
        {
            FillableLink fl = new("", 0, 1);
            var vt = _reader.GetValue(out var nullableS, out var ln);
            if (ln != null)
                fl = fl with { Name = ln };
            var s = nullableS ?? 0;
            var bc = vt switch
            {
                Reader.ValueTypes.Immediate => Cpu6502ByteCodesInstructions.CPY_IMMEDIATE,
                Reader.ValueTypes.ZeroPage  => Cpu6502ByteCodesInstructions.CPY_ZEROPAGE,
                Reader.ValueTypes.Absolute  => Cpu6502ByteCodesInstructions.CPY_ABSOLUTE,
                _ => throw new NotImplementedException(),
            };
            _writer.WriteByte(bc);

            fl = fl with { Position = _writer.Position };
            _writer.WriteByte((byte)(s & 0xFF));
            if (vt.HasFlag(Reader.ValueTypes.Absolute))
            {
                fl = fl with { Size = fl.Size + 1 };
                s = (ushort)(s >> 8);
                _writer.WriteByte((byte)(s & 0xFF));
            }
            if (nullableS == null && ln != null)
                _fillableLinks.Add(fl);
        }
        #endregion

        #region Increments & Decrements
        void WriteINC() // Increment a Memory Location
        {
            FillableLink fl = new("", 0, 1);
            var vt = _reader.GetValue(out var nullableS, out var ln);
            if (ln != null)
                fl = fl with { Name = ln };
            var s = nullableS ?? 0;
            var bc = vt switch
            {
                Reader.ValueTypes.ZeroPage  => Cpu6502ByteCodesInstructions.INC_ZEROPAGE,
                Reader.ValueTypes.ZeroPageX => Cpu6502ByteCodesInstructions.INC_ZEROPAGE_X,
                Reader.ValueTypes.Absolute  => Cpu6502ByteCodesInstructions.INC_ABSOLUTE,
                Reader.ValueTypes.AbsoluteX => Cpu6502ByteCodesInstructions.INC_ABSOLUTE_X,
                _ => throw new NotImplementedException(),
            };
            _writer.WriteByte(bc);

            fl = fl with { Position = _writer.Position };
            _writer.WriteByte((byte)(s & 0xFF));
            if (vt.HasFlag(Reader.ValueTypes.Absolute))
            {
                fl = fl with { Size = fl.Size + 1 };
                s = (ushort)(s >> 8);
                _writer.WriteByte((byte)(s & 0xFF));
            }
            if (nullableS == null && ln != null)
                _fillableLinks.Add(fl);
        }
        void WriteDEC() // Decrement a Memory Location
        {
            FillableLink fl = new("", 0, 1);
            var vt = _reader.GetValue(out var nullableS, out var ln);
            if (ln != null)
                fl = fl with { Name = ln };
            var s = nullableS ?? 0;
            var bc = vt switch
            {
                Reader.ValueTypes.ZeroPage  => Cpu6502ByteCodesInstructions.DEC_ZEROPAGE,
                Reader.ValueTypes.ZeroPageX => Cpu6502ByteCodesInstructions.DEC_ZEROPAGE_X,
                Reader.ValueTypes.Absolute  => Cpu6502ByteCodesInstructions.DEC_ABSOLUTE,
                Reader.ValueTypes.AbsoluteX => Cpu6502ByteCodesInstructions.DEC_ABSOLUTE_X,
                _ => throw new NotImplementedException(),
            };
            _writer.WriteByte(bc);

            fl = fl with { Position = _writer.Position };
            _writer.WriteByte((byte)(s & 0xFF));
            if (vt.HasFlag(Reader.ValueTypes.Absolute))
            {
                fl = fl with { Size = fl.Size + 1 };
                s = (ushort)(s >> 8);
                _writer.WriteByte((byte)(s & 0xFF));
            }
            if (nullableS == null && ln != null)
                _fillableLinks.Add(fl);
        }
        #endregion

        #region Shifts
        void WriteASL() // Arithmetic Shift Left
        {
            FillableLink fl = new("", 0, 1);
            var vt = _reader.GetValue(out var nullableS, out var ln);
            if (ln != null)
                fl = fl with { Name = ln };
            var s = nullableS ?? 0;
            var bc = vt switch
            {
                Reader.ValueTypes.A         => Cpu6502ByteCodesInstructions.ASL_ACCUMULATOR,
                Reader.ValueTypes.ZeroPage  => Cpu6502ByteCodesInstructions.ASL_ZEROPAGE,
                Reader.ValueTypes.ZeroPageX => Cpu6502ByteCodesInstructions.ASL_ZEROPAGE_X,
                Reader.ValueTypes.Absolute  => Cpu6502ByteCodesInstructions.ASL_ABSOLUTE,
                Reader.ValueTypes.AbsoluteX => Cpu6502ByteCodesInstructions.ASL_ABSOLUTE_X,
                _ => throw new NotImplementedException(),
            };
            _writer.WriteByte(bc);

            fl = fl with { Position = _writer.Position };
            _writer.WriteByte((byte)(s & 0xFF));
            if (vt.HasFlag(Reader.ValueTypes.Absolute))
            {
                fl = fl with { Size = fl.Size + 1 };
                s = (ushort)(s >> 8);
                _writer.WriteByte((byte)(s & 0xFF));
            }
            if (nullableS == null && ln != null)
                _fillableLinks.Add(fl);
        }
        void WriteLSR() // Logical Shift Right
        {
            FillableLink fl = new("", 0, 1);
            var vt = _reader.GetValue(out var nullableS, out var ln);
            if (ln != null)
                fl = fl with { Name = ln };
            var s = nullableS ?? 0;
            var bc = vt switch
            {
                Reader.ValueTypes.A         => Cpu6502ByteCodesInstructions.LSR_ACCUMULATOR,
                Reader.ValueTypes.ZeroPage  => Cpu6502ByteCodesInstructions.LSR_ZEROPAGE,
                Reader.ValueTypes.ZeroPageX => Cpu6502ByteCodesInstructions.LSR_ZEROPAGE_X,
                Reader.ValueTypes.Absolute  => Cpu6502ByteCodesInstructions.LSR_ABSOLUTE,
                Reader.ValueTypes.AbsoluteX => Cpu6502ByteCodesInstructions.LSR_ABSOLUTE_X,
                _ => throw new NotImplementedException(),
            };
            _writer.WriteByte(bc);

            fl = fl with { Position = _writer.Position };
            _writer.WriteByte((byte)(s & 0xFF));
            if (vt.HasFlag(Reader.ValueTypes.Absolute))
            {
                fl = fl with { Size = fl.Size + 1 };
                s = (ushort)(s >> 8);
                _writer.WriteByte((byte)(s & 0xFF));
            }
            if (nullableS == null && ln != null)
                _fillableLinks.Add(fl);
        }
        void WriteROL() // Rotate Left
        {
            FillableLink fl = new("", 0, 1);
            var vt = _reader.GetValue(out var nullableS, out var ln);
            if (ln != null)
                fl = fl with { Name = ln };
            var s = nullableS ?? 0;
            var bc = vt switch
            {
                Reader.ValueTypes.A         => Cpu6502ByteCodesInstructions.ROL_ACCUMULATOR,
                Reader.ValueTypes.ZeroPage  => Cpu6502ByteCodesInstructions.ROL_ZEROPAGE,
                Reader.ValueTypes.ZeroPageX => Cpu6502ByteCodesInstructions.ROL_ZEROPAGE_X,
                Reader.ValueTypes.Absolute  => Cpu6502ByteCodesInstructions.ROL_ABSOLUTE,
                Reader.ValueTypes.AbsoluteX => Cpu6502ByteCodesInstructions.ROL_ABSOLUTE_X,
                _ => throw new NotImplementedException(),
            };
            _writer.WriteByte(bc);

            fl = fl with { Position = _writer.Position };
            _writer.WriteByte((byte)(s & 0xFF));
            if (vt.HasFlag(Reader.ValueTypes.Absolute))
            {
                fl = fl with { Size = fl.Size + 1 };
                s = (ushort)(s >> 8);
                _writer.WriteByte((byte)(s & 0xFF));
            }
            if (nullableS == null && ln != null)
                _fillableLinks.Add(fl);
        }
        void WriteROR() // Rotate Right
        {
            FillableLink fl = new("", 0, 1);
            var vt = _reader.GetValue(out var nullableS, out var ln);
            if (ln != null)
                fl = fl with { Name = ln };
            var s = nullableS ?? 0;
            var bc = vt switch
            {
                Reader.ValueTypes.A         => Cpu6502ByteCodesInstructions.ROR_ACCUMULATOR,
                Reader.ValueTypes.ZeroPage  => Cpu6502ByteCodesInstructions.ROR_ZEROPAGE,
                Reader.ValueTypes.ZeroPageX => Cpu6502ByteCodesInstructions.ROR_ZEROPAGE_X,
                Reader.ValueTypes.Absolute  => Cpu6502ByteCodesInstructions.ROR_ABSOLUTE,
                Reader.ValueTypes.AbsoluteX => Cpu6502ByteCodesInstructions.ROR_ABSOLUTE_X,
                _ => throw new NotImplementedException(),
            };
            _writer.WriteByte(bc);

            fl = fl with { Position = _writer.Position };
            _writer.WriteByte((byte)(s & 0xFF));
            if (vt.HasFlag(Reader.ValueTypes.Absolute))
            {
                fl = fl with { Size = fl.Size + 1 };
                s = (ushort)(s >> 8);
                _writer.WriteByte((byte)(s & 0xFF));
            }
            if (nullableS == null && ln != null)
                _fillableLinks.Add(fl);
        }
        #endregion

        #region Jumps & Calls
        void WriteJMP() // Jump to Another Location
        {
            FillableLink fl = new("", 0, 1);
            var vt = _reader.GetValue(out var nullableS, out var ln);
            if (ln != null)
                fl = fl with { Name = ln };
            var s = nullableS ?? 0;
            var bc = vt switch
            {
                Reader.ValueTypes.Absolute => Cpu6502ByteCodesInstructions.JMP_ABSOLUTE,
                Reader.ValueTypes.Indirect => Cpu6502ByteCodesInstructions.JMP_INDIRECT,
                _ => throw new NotImplementedException(),
            };
            _writer.WriteByte(bc);

            fl = fl with { Position = _writer.Position };
            _writer.WriteByte((byte)(s & 0xFF));
            if (vt.HasFlag(Reader.ValueTypes.Absolute))
            {
                fl = fl with { Size = fl.Size + 1 };
                s = (ushort)(s >> 8);
                _writer.WriteByte((byte)(s & 0xFF));
            }
            if (nullableS == null && ln != null)
                _fillableLinks.Add(fl);
        }
        void WriteJSR() // Jump to a Subroutine
        {
            FillableLink fl = new("", 0, 1);
            var vt = _reader.GetValue(out var nullableS, out var ln);
            if (ln != null)
                fl = fl with { Name = ln };
            var s = nullableS ?? 0;
            var bc = vt switch
            {
                Reader.ValueTypes.Absolute => Cpu6502ByteCodesInstructions.JSR_ABSOLUTE,
                _ => throw new NotImplementedException(),
            };
            _writer.WriteByte(bc);

            fl = fl with { Position = _writer.Position };
            _writer.WriteByte((byte)(s & 0xFF));
            if (vt.HasFlag(Reader.ValueTypes.Absolute))
            {
                fl = fl with { Size = fl.Size + 1 };
                s = (ushort)(s >> 8);
                _writer.WriteByte((byte)(s & 0xFF));
            }
            if (nullableS == null && ln != null)
                _fillableLinks.Add(fl);
        }
        #endregion

        #region Branches
        void WriteBCC() // Branch if Carry Flag Clear
        {
            FillableLink fl = new("", 0, 1);
            var vt = _reader.GetValue(out var nullableS, out var ln);
            if (ln != null)
                fl = fl with { Name = ln };
            var s = nullableS ?? 0;
            var bc = vt switch
            {
                Reader.ValueTypes.Absolute => Cpu6502ByteCodesInstructions.BCC_ABSOLUTE,
                _ => throw new NotImplementedException(),
            };
            _writer.WriteByte(bc);

            fl = fl with { Position = _writer.Position };
            _writer.WriteByte((byte)(s & 0xFF));
            if (vt.HasFlag(Reader.ValueTypes.Absolute))
            {
                fl = fl with { Size = fl.Size + 1 };
                s = (ushort)(s >> 8);
                _writer.WriteByte((byte)(s & 0xFF));
            }
            if (nullableS == null && ln != null)
                _fillableLinks.Add(fl);
        }
        void WriteBCS() // Branch if Carry Flag Set
        {
            FillableLink fl = new("", 0, 1);
            var vt = _reader.GetValue(out var nullableS, out var ln);
            if (ln != null)
                fl = fl with { Name = ln };
            var s = nullableS ?? 0;
            var bc = vt switch
            {
                Reader.ValueTypes.Absolute => Cpu6502ByteCodesInstructions.BCS_ABSOLUTE,
                _ => throw new NotImplementedException(),
            };
            _writer.WriteByte(bc);

            fl = fl with { Position = _writer.Position };
            _writer.WriteByte((byte)(s & 0xFF));
            if (vt.HasFlag(Reader.ValueTypes.Absolute))
            {
                fl = fl with { Size = fl.Size + 1 };
                s = (ushort)(s >> 8);
                _writer.WriteByte((byte)(s & 0xFF));
            }
            if (nullableS == null && ln != null)
                _fillableLinks.Add(fl);
        }
        void WriteBEQ() // Branch if Zero Flag Set
        {
            FillableLink fl = new("", 0, 1);
            var vt = _reader.GetValue(out var nullableS, out var ln);
            if (ln != null)
                fl = fl with { Name = ln };
            var s = nullableS ?? 0;
            var bc = vt switch
            {
                Reader.ValueTypes.Absolute => Cpu6502ByteCodesInstructions.BEQ_ABSOLUTE,
                _ => throw new NotImplementedException(),
            };
            _writer.WriteByte(bc);

            fl = fl with { Position = _writer.Position };
            _writer.WriteByte((byte)(s & 0xFF));
            if (vt.HasFlag(Reader.ValueTypes.Absolute))
            {
                fl = fl with { Size = fl.Size + 1 };
                s = (ushort)(s >> 8);
                _writer.WriteByte((byte)(s & 0xFF));
            }
            if (nullableS == null && ln != null)
                _fillableLinks.Add(fl);
        }
        void WriteBMI() // Branch if Negative Flag Set
        {
            FillableLink fl = new("", 0, 1);
            var vt = _reader.GetValue(out var nullableS, out var ln);
            if (ln != null)
                fl = fl with { Name = ln };
            var s = nullableS ?? 0;
            var bc = vt switch
            {
                Reader.ValueTypes.Absolute => Cpu6502ByteCodesInstructions.BMI_ABSOLUTE,
                _ => throw new NotImplementedException(),
            };
            _writer.WriteByte(bc);

            fl = fl with { Position = _writer.Position };
            _writer.WriteByte((byte)(s & 0xFF));
            if (vt.HasFlag(Reader.ValueTypes.Absolute))
            {
                fl = fl with { Size = fl.Size + 1 };
                s = (ushort)(s >> 8);
                _writer.WriteByte((byte)(s & 0xFF));
            }
            if (nullableS == null && ln != null)
                _fillableLinks.Add(fl);
        }
        void WriteBNE() // Branch if Zero Flag Clear
        {
            FillableLink fl = new("", 0, 1);
            var vt = _reader.GetValue(out var nullableS, out var ln);
            if (ln != null)
                fl = fl with { Name = ln };
            var s = nullableS ?? 0;
            var bc = vt switch
            {
                Reader.ValueTypes.Absolute => Cpu6502ByteCodesInstructions.BNE_ABSOLUTE,
                _ => throw new NotImplementedException(),
            };
            _writer.WriteByte(bc);

            fl = fl with { Position = _writer.Position };
            _writer.WriteByte((byte)(s & 0xFF));
            if (vt.HasFlag(Reader.ValueTypes.Absolute))
            {
                fl = fl with { Size = fl.Size + 1 };
                s = (ushort)(s >> 8);
                _writer.WriteByte((byte)(s & 0xFF));
            }
            if (nullableS == null && ln != null)
                _fillableLinks.Add(fl);
        }
        void WriteBPL() // Branch if Negative Flag Clear
        {
            FillableLink fl = new("", 0, 1);
            var vt = _reader.GetValue(out var nullableS, out var ln);
            if (ln != null)
                fl = fl with { Name = ln };
            var s = nullableS ?? 0;
            var bc = vt switch
            {
                Reader.ValueTypes.Absolute => Cpu6502ByteCodesInstructions.BPL_ABSOLUTE,
                _ => throw new NotImplementedException(),
            };
            _writer.WriteByte(bc);

            fl = fl with { Position = _writer.Position };
            _writer.WriteByte((byte)(s & 0xFF));
            if (vt.HasFlag(Reader.ValueTypes.Absolute))
            {
                fl = fl with { Size = fl.Size + 1 };
                s = (ushort)(s >> 8);
                _writer.WriteByte((byte)(s & 0xFF));
            }
            if (nullableS == null && ln != null)
                _fillableLinks.Add(fl);
        }
        void WriteBVC() // Branch if Overflow Flag Clear
        {
            FillableLink fl = new("", 0, 1);
            var vt = _reader.GetValue(out var nullableS, out var ln);
            if (ln != null)
                fl = fl with { Name = ln };
            var s = nullableS ?? 0;
            var bc = vt switch
            {
                Reader.ValueTypes.Absolute => Cpu6502ByteCodesInstructions.BVC_ABSOLUTE,
                _ => throw new NotImplementedException(),
            };
            _writer.WriteByte(bc);

            fl = fl with { Position = _writer.Position };
            _writer.WriteByte((byte)(s & 0xFF));
            if (vt.HasFlag(Reader.ValueTypes.Absolute))
            {
                fl = fl with { Size = fl.Size + 1 };
                s = (ushort)(s >> 8);
                _writer.WriteByte((byte)(s & 0xFF));
            }
            if (nullableS == null && ln != null)
                _fillableLinks.Add(fl);
        }
        void WriteBVS() // Branch if Overflow Flag Set
        {
            FillableLink fl = new("", 0, 1);
            var vt = _reader.GetValue(out var nullableS, out var ln);
            if (ln != null)
                fl = fl with { Name = ln };
            var s = nullableS ?? 0;
            var bc = vt switch
            {
                Reader.ValueTypes.Absolute => Cpu6502ByteCodesInstructions.BVS_ABSOLUTE,
                _ => throw new NotImplementedException(),
            };
            _writer.WriteByte(bc);

            fl = fl with { Position = _writer.Position };
            _writer.WriteByte((byte)(s & 0xFF));
            if (vt.HasFlag(Reader.ValueTypes.Absolute))
            {
                fl = fl with { Size = fl.Size + 1 };
                s = (ushort)(s >> 8);
                _writer.WriteByte((byte)(s & 0xFF));
            }
            if (nullableS == null && ln != null)
                _fillableLinks.Add(fl);
        }
        #endregion

        byte[] GetByteCodes() => _writer.ToArray();

        public static byte[] Compile(string inFilepath)
        {
            using var compiler = new Cpu6502Compiler(inFilepath);
            compiler.CompileLines();

            return compiler.GetByteCodes();
        }
        public static void CompileAndSaveAs(string inFilepath, string outFilepath)
        {
            using var compiler = new Cpu6502Compiler(inFilepath);
            compiler.CompileLines();

            var bytes = compiler.GetByteCodes();
            File.WriteAllBytes(outFilepath, bytes);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            
            if (disposing)
            {
                _writer.Dispose();
                _reader.Dispose();
            }
            _disposed = true;
        }

        private class Reader : IDisposable
        {
            bool _disposed;
            readonly StreamReader _reader;
            public bool EOS => _reader.EndOfStream;

            public Reader(string filepath)
            {
                _reader = new(filepath);
            }
            ~Reader() => Dispose(false);

            public char Read() => (char)_reader.Read();
            public char Peek() => (char)_reader.Peek();
            public void ToNextLine() => _reader.ReadLine();
            public string GetRawString()
            {
                StringBuilder sb = new();

                char c;
                do
                {
                    c = (char)_reader.Read();
                } while (char.IsWhiteSpace(c) && !EOS);

                while (!char.IsWhiteSpace(c) && !EOS)
                {
                    sb.Append(c);
                    c = (char)_reader.Read();
                }

                return sb.ToString();
            }
            public string GetWord()
            {
                StringBuilder sb = new();

                char c;
                do
                {
                    c = (char)_reader.Read();
                } while (!char.IsLetter(c) && !EOS);

                while (char.IsLetter(c) && !EOS)
                {
                    sb.Append(c);
                    c = (char)_reader.Read();
                }

                return sb.ToString();
            }
            public string GetExplicitString()
            {
                StringBuilder sb = new();

                char c;
                do
                {
                    c = (char)_reader.Read();
                } while (c != '"' && !EOS);

                c = (char)_reader.Read();
                while (c != '"' && !EOS)
                {
                    sb.Append(c);
                    c = (char)_reader.Read();
                }

                return sb.ToString();
            }
            public void GetUInt16(out ushort? value, out string? linkName)
            {
                linkName = null;
                value = null;
                var s = GetRawString();
                int ba = 10;
                var isLink = s.All(char.IsLetter);
                if (isLink)
                {
                    linkName = s;
                    return;
                }
                if (!char.IsDigit(s[0]))
                {
                    ba = s[0] switch
                    {
                        '$' => 16,
                        '%' => 2,
                        _ => throw new NotImplementedException()
                    };
                    s = s[1..];
                }
                value = Convert.ToUInt16(s, ba);
            }
            public void GetByte(out byte? value, out string? linkName)
            {
                linkName = null;
                value = null;
                var s = GetRawString();
                int ba = 10;
                var isLink = s.All(char.IsLetter);
                if (isLink)
                {
                    linkName = s;
                    return;
                }
                if (!char.IsDigit(s[0]))
                {
                    ba = s[0] switch
                    {
                        '$' => 16,
                        '%' => 2,
                        _ => throw new NotFiniteNumberException()
                    };
                    s = s[1..];
                }
                value = Convert.ToByte(s, ba);
            }

            [Flags] public enum ValueTypes
            {
                None = 0,

                X = 1,
                Y = 2,
                A = 4,

                Immediate = 8,
                ZeroPage = 16,
                Absolute = 32,
                Indirect = 64,

                ZeroPageX = ZeroPage | X,
                ZeroPageY = ZeroPage | Y,

                AbsoluteX = Absolute | X,
                AbsoluteY = Absolute | Y,

                IndirectX = Indirect | X,
                IndirectY = Indirect | Y,
            }

            public ValueTypes GetValue(out ushort? s, out string? linkName)
            {
                ValueTypes vt = ValueTypes.None;
                s = null;
                linkName = null;
                int ba = 10;
                var str = GetRawString();
                if (str == "a")
                    return ValueTypes.A;
                if (!char.IsDigit(str[0]) && str[0] == '#')
                {
                    vt |= ValueTypes.Immediate;
                    str = str[1..];
                }
                if (str.Contains('(') && str.Contains(')'))
                {
                    str = str.Replace("(", string.Empty).Replace(")", string.Empty);
                    vt |= ValueTypes.Indirect;
                }
                if (str.Contains(','))
                {
                    var sp = str.Split(',');
                    _ = sp.Length == 2 ? true : throw new NotImplementedException();
                    str = sp[0];
                    vt |= sp[1] switch
                    {
                        "x" => ValueTypes.X,
                        "y" => ValueTypes.Y,
                        _ => throw new NotImplementedException()
                    };
                }
                bool isLink = str.All(char.IsLetter);
                if (isLink)
                {
                    linkName = str;
                }
                if (!isLink && !char.IsDigit(str[0]))
                {
                    ba = str[0] switch
                    {
                        '$' => 16,
                        '%' => 2,
                        _ => throw new NotImplementedException()
                    };
                    str = str[1..];
                }
                if (!isLink)
                    s = Convert.ToUInt16(str, ba);

                if (s <= byte.MaxValue)
                {
                    if (!vt.HasFlag(ValueTypes.Immediate))
                        vt |= ValueTypes.ZeroPage;
                }
                if (vt.HasFlag(ValueTypes.ZeroPage) == false
                    && vt.HasFlag(ValueTypes.Immediate) == false
                    && vt.HasFlag(ValueTypes.Indirect) == false)
                    vt |= ValueTypes.Absolute;
                return vt;
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            protected virtual void Dispose(bool disposing)
            {
                if (_disposed)
                    return;

                if (disposing)
                {
                    _reader.Dispose();
                }

                _disposed = true;
            }
        }
    }
}
