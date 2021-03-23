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
        bool _disposed;
        readonly MemoryStream _writer;
        readonly Reader _reader;
        Dictionary<string, ushort> _links;

        Cpu6502Compiler(string inFilepath)
        {
            _disposed = false;
            _reader = new Reader(inFilepath);
            _writer = new MemoryStream(new byte[ushort.MaxValue + 1]);
            _links = new();
        }
        ~Cpu6502Compiler() => Dispose(false);

        void CompileLines()
        {
            while (!_reader.EOS)
                CompileLine();
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
            switch (instruction)
            {
                case "org":
                    var offset = _reader.GetUInt16(_links);
                    _writer.Seek(offset, SeekOrigin.Begin);
                    break;
                case "word":
                    var value = _reader.GetUInt16(_links);
                    _writer.WriteByte((byte)(value & 0b11111111));
                    value = (ushort)(value >> 8);
                    _writer.WriteByte((byte)(value & 0b11111111));
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
                case Cpu6502StringIntructions.LDA:
                    WriteLDA();
                    break;
                case Cpu6502StringIntructions.LDX:
                    WriteLDX();
                    break;
                case Cpu6502StringIntructions.LDY:
                    WriteLDY();
                    break;
                default:
                    break;
            }
        }

        #region Load Operations
        // Load Accumulator
        void WriteLDA()
        {
            var vt = _reader.GetValue(_links, out var b, out var s);
            var bc = vt switch
            {
                Reader.ValueTypes.Immediate => Cpu6502ByteCodesIntructions.LDA_IMMEDIATE,
                Reader.ValueTypes.ZeroPage  => Cpu6502ByteCodesIntructions.LDA_ZEROPAGE,
                Reader.ValueTypes.ZeroPageX => Cpu6502ByteCodesIntructions.LDA_ZEROPAGE_X,
                Reader.ValueTypes.Absolute  => Cpu6502ByteCodesIntructions.LDA_ABSOLUTE,
                Reader.ValueTypes.AbsoluteX => Cpu6502ByteCodesIntructions.LDA_ABSOLUTE_X,
                Reader.ValueTypes.AbsoluteY => Cpu6502ByteCodesIntructions.LDA_ABSOLUTE_Y,
                _ => throw new NotImplementedException(),
            };
            _writer.WriteByte(bc);
            _writer.WriteByte((byte)(s & 0b11111111));
            if (vt.HasFlag(Reader.ValueTypes.Absolute))
            {
                s = (ushort)(s >> 8);
                _writer.WriteByte((byte)(s & 0b11111111));
            }
        }
        // Load X Register
        void WriteLDX()
        {
            var vt = _reader.GetValue(_links, out var b, out var s);
            var bc = vt switch
            {
                Reader.ValueTypes.Immediate => Cpu6502ByteCodesIntructions.LDX_IMMEDIATE,
                Reader.ValueTypes.ZeroPage  => Cpu6502ByteCodesIntructions.LDX_ZEROPAGE,
                Reader.ValueTypes.ZeroPageY => Cpu6502ByteCodesIntructions.LDX_ZEROPAGE_Y,
                Reader.ValueTypes.Absolute  => Cpu6502ByteCodesIntructions.LDX_ABSOLUTE,
                Reader.ValueTypes.AbsoluteY => Cpu6502ByteCodesIntructions.LDX_ABSOLUTE_Y,
                _ => throw new NotImplementedException(),
            };
            _writer.WriteByte(bc);
            _writer.WriteByte((byte)(s & 0b11111111));
            if (vt.HasFlag(Reader.ValueTypes.Absolute))
            {
                s = (ushort)(s >> 8);
                _writer.WriteByte((byte)(s & 0b11111111));
            }
        }
        // Load Y Register
        void WriteLDY()
        {
            var vt = _reader.GetValue(_links, out var b, out var s);
            var bc = vt switch
            {
                Reader.ValueTypes.Immediate => Cpu6502ByteCodesIntructions.LDY_IMMEDIATE,
                Reader.ValueTypes.ZeroPage  => Cpu6502ByteCodesIntructions.LDY_ZEROPAGE,
                Reader.ValueTypes.ZeroPageX => Cpu6502ByteCodesIntructions.LDY_ZEROPAGE_X,
                Reader.ValueTypes.Absolute  => Cpu6502ByteCodesIntructions.LDY_ABSOLUTE,
                Reader.ValueTypes.AbsoluteX => Cpu6502ByteCodesIntructions.LDY_ABSOLUTE_X,
                _ => throw new NotImplementedException(),
            };
            _writer.WriteByte(bc);
            _writer.WriteByte((byte)(s & 0b11111111));
            if (vt.HasFlag(Reader.ValueTypes.Absolute))
            {
                s = (ushort)(s >> 8);
                _writer.WriteByte((byte)(s & 0b11111111));
            }
        }
        #endregion

        #region Store Operations
        void WriteSTA() // Store Accumulator
        {
        }
        void WriteSTX() // Store X Register
        {
        }
        void WriteSTY() // Store Y Register
        {
        }
        #endregion

        #region Register Transfers
        void WriteTAX() // Transfer Accumulator To X
        { }
        void WriteTAY() // Transfer Accumulator To Y
        { }
        void WriteTXA() // Transfer X To Accumulator
        { }
        void WriteTYA() // Transfer Y To Accumulator
        { }
        #endregion

        #region Stack Operations
        void WriteTSX() // Transfer Stack Pointer to X
        { }
        void WriteTXS() // Transfer X to Stack Pointer
        { }
        void WritePHA() // Push Accumulator on Stack
        { }
        void WritePHP() // Push Processor Status on Stack
        { }
        void WritePLA() // Pull Accumulator from Stack
        { }
        void WritePLP()  // Pull Processor Status from Stack
        { }
        #endregion

        #region Logical
        void WriteAND() // Logical AND
        { }
        void WriteEOR() // Exclusive OR
        { }
        void WriteORA() // Logical Inclusive OR
        { }
        void WriteBIT() // Bit Test
        { }
        #endregion

        #region Arithmetic
        void WriteADC() // Add with Carry
        { }
        void WriteSBC() // Substract with Carry
        { }
        void WriteCMP() // Compare Accumulator
        { }
        void WriteCPX() // Compare X Register
        { }
        void WriteCPY() // Compare Y Register
        { }
        #endregion

        #region Increments & Decrements
        void WriteINC() // Increment a Memory Location
        { }
        void WriteINX() // Increment the X Register
        { }
        void WriteINY() // Increment the Y Register
        { }
        void WriteDEC() // Decrement a Memory Location
        { }
        void WriteDEX() // Decrement the X Register
        { }
        void WriteDEY() // Decrement the Y Register
        { }
        #endregion

        #region Shifts
        void WriteASL() // Arithmetic Shift Left
        { }
        void WriteLSR() // Logical Shift Right
        { }
        void WriteROL() // Rotate Left
        { }
        void WriteROR() // Rotate Right
        { }
        #endregion

        #region Jumps & Calls
        void WriteJMP() // Jump to Another Location
        { }
        void WriteJSR() // Jump to a Subroutine
        { }
        void WriteRTS() // Return from Subroutine
        { }
        #endregion

        #region Branches
        void WriteBCC() // Branch if Carry Flag Clear
        { }
        void WriteBCS() // Branch if Carry Flag Set
        { }
        void WriteBEQ() // Branch if Zero Flag Set
        { }
        void WriteBMI() // Branch if Negative Flag Set
        { }
        void WriteBNE() // Branch if Zero Flag Clear
        { }
        void WriteBPL() // Branch if Negative Flag Clear
        { }
        void WriteBVC() // Branch if Overflow Flag Clear
        { }
        void WriteBVS() // Branch if Overflow Flag Set
        { }
        #endregion

        #region Status Flag Changes
        void WriteCLC() // Clear Carry Flag
        { }
        void WriteCLD() // Clear Decimal Mode Flag
        { }
        void WriteCLI() // Clear Interrupt Disable Flag
        { }
        void WriteCLV() // Clear Overflow Flag
        { }
        void WriteSEC()  // Set Carry Flag
        { }
        void WriteSED() // Set Decimal Mode Flag
        { }
        void WriteSEI()  // Set Interrupt Disable Flag
        { }
        #endregion

        #region System Functions
        void WriteBRK() // Force an Interrupt
        {
        }
        void WriteNOP() // No Operation
        {
        }
        void WriteRTI() // Return from Interrupt
        {
        }
        #endregion

        byte[] GetByteCodes() => _writer.ToArray();

        public static byte[] Compile(string inFilepath)
        {
            using var compiler = new Cpu6502Compiler(inFilepath);
            compiler.CompileLines();

            return compiler.GetByteCodes();
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
            public ushort GetUInt16(Dictionary<string, ushort> links)
            {
                var s = GetRawString();
                if (links.ContainsKey(s))
                    return links[s];
                if (!char.IsDigit(s[0]))
                {
                    var b = s[0] switch
                    {
                        '$' => 16,
                        '%' => 2,
                        _ => throw new NotFiniteNumberException()
                    };
                    s = s[1..];
                    return Convert.ToUInt16(s, b);
                }
                return ushort.Parse(s);
            }
            public byte GetByte(Dictionary<string, ushort> links)
            {
                var s = GetRawString();
                if (links.ContainsKey(s))
                    return (byte)links[s];
                if (!char.IsDigit(s[0]))
                {
                    var b = s[0] switch
                    {
                        '$' => 16,
                        '%' => 2,
                        _ => throw new NotFiniteNumberException()
                    };
                    s = s[1..];
                    return Convert.ToByte(s, b);
                }
                return byte.Parse(s);
            }

            [Flags] public enum ValueTypes
            {
                None = 0,

                X = 1,
                Y = 2,

                Immediate = 4,
                ZeroPage = 8,
                Absolute = 16,
                Indirect = 32,

                ZeroPageX = ZeroPage | X,
                ZeroPageY = ZeroPage | Y,

                AbsoluteX = Absolute | X,
                AbsoluteY = Absolute | Y,
            }

            public ValueTypes GetValue(Dictionary<string, ushort> links, out byte b, out ushort s)
            {
                ValueTypes vt = ValueTypes.None;
                b = 0;
                s = 0;
                int ba = 10;
                var str = GetRawString();
                if (!char.IsDigit(str[0]))
                {
                    if (str[0] == '#')
                    {
                        vt |= ValueTypes.Immediate;
                        str = str[1..];
                    }
                    ba = str[0] switch
                    {
                        '$' => 16,
                        '%' => 2,
                        _ => throw new NotFiniteNumberException()
                    };
                    str = str[1..];
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
                s = Convert.ToUInt16(str, ba);

                if (s <= byte.MaxValue)
                {
                    b = (byte)s;
                    if (!vt.HasFlag(ValueTypes.Immediate))
                        vt |= ValueTypes.ZeroPage;
                }
                if (vt.HasFlag(ValueTypes.ZeroPage) == false && vt.HasFlag(ValueTypes.Immediate) == false)
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
