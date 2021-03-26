﻿using System;
using System.Diagnostics;
using System.Linq;

namespace CPU
{
    public class Cpu6502
    {
        public ushort PC { get; internal set; } // Program Counter
        public byte SP { get; internal set; } // Stack Pointer

        public Register A { get; internal set; } // Accumulator Register
        public Register X { get; internal set; } // Index Register X
        public Register Y { get; internal set; } // Index Register Y

        public ProcessorStatus PS { get; internal set; }
        internal byte[] _memory;

        public Cpu6502(byte[] memory)
        {
            A = new(this);
            X = new(this);
            Y = new(this);
            _memory = memory.ToArray();
            PS = new();
        }

        public void Reset()
        {
            PC = GetResetAddress();
            while (true)
                ReadCurrentInstruction();
        }

        void ReadCurrentInstruction()
        {
            byte instruction = _memory[PC++];

            switch (instruction)
            {
                case Cpu6502ByteCodesInstructions.LDA_IMMEDIATE:
                case Cpu6502ByteCodesInstructions.LDA_ZEROPAGE:
                case Cpu6502ByteCodesInstructions.LDA_ZEROPAGE_X:
                case Cpu6502ByteCodesInstructions.LDA_ABSOLUTE:
                case Cpu6502ByteCodesInstructions.LDA_ABSOLUTE_X:
                case Cpu6502ByteCodesInstructions.LDA_ABSOLUTE_Y:
                case Cpu6502ByteCodesInstructions.LDA_INDIRECT_X:
                case Cpu6502ByteCodesInstructions.LDA_INDIRECT_Y:
                    LDA(instruction);
                    break;
                case Cpu6502ByteCodesInstructions.LDX_IMMEDIATE:
                case Cpu6502ByteCodesInstructions.LDX_ZEROPAGE:
                case Cpu6502ByteCodesInstructions.LDX_ZEROPAGE_Y:
                case Cpu6502ByteCodesInstructions.LDX_ABSOLUTE:
                case Cpu6502ByteCodesInstructions.LDX_ABSOLUTE_Y:
                    LDX(instruction);           
                    break;
                case Cpu6502ByteCodesInstructions.LDY_IMMEDIATE:
                case Cpu6502ByteCodesInstructions.LDY_ZEROPAGE:
                case Cpu6502ByteCodesInstructions.LDY_ZEROPAGE_X:
                case Cpu6502ByteCodesInstructions.LDY_ABSOLUTE:
                case Cpu6502ByteCodesInstructions.LDY_ABSOLUTE_X:
                    LDY(instruction);
                    break;
                case Cpu6502ByteCodesInstructions.STA_ZEROPAGE:
                case Cpu6502ByteCodesInstructions.STA_ZEROPAGE_X:
                case Cpu6502ByteCodesInstructions.STA_ABSOLUTE:
                case Cpu6502ByteCodesInstructions.STA_ABSOLUTE_X:
                case Cpu6502ByteCodesInstructions.STA_ABSOLUTE_Y:
                case Cpu6502ByteCodesInstructions.STA_INDIRECT_X:
                case Cpu6502ByteCodesInstructions.STA_INDIRECT_Y:
                    STA(instruction);
                    break;
                case Cpu6502ByteCodesInstructions.STX_ZEROPAGE:
                case Cpu6502ByteCodesInstructions.STX_ZEROPAGE_Y:
                case Cpu6502ByteCodesInstructions.STX_ABSOLUTE:
                    STX(instruction);
                    break;
                case Cpu6502ByteCodesInstructions.STY_ZEROPAGE:
                case Cpu6502ByteCodesInstructions.STY_ZEROPAGE_X:
                case Cpu6502ByteCodesInstructions.STY_ABSOLUTE:
                    STY(instruction);
                    break;
                case Cpu6502ByteCodesInstructions.TAX:
                    TAX();
                    break;
                case Cpu6502ByteCodesInstructions.TAY:
                    TAY();
                    break;
                case Cpu6502ByteCodesInstructions.TXA:
                    TXA();
                    break;
                case Cpu6502ByteCodesInstructions.TYA:
                    TYA();
                    break;
                case Cpu6502ByteCodesInstructions.TSX:
                    TSX();
                    break;
                case Cpu6502ByteCodesInstructions.TXS:
                    TXS();
                    break;
                case Cpu6502ByteCodesInstructions.PHA:
                    PHA();
                    break;
                case Cpu6502ByteCodesInstructions.PHP:
                    PHP();
                    break;
                case Cpu6502ByteCodesInstructions.PLA:
                    PLA();
                    break;
                case Cpu6502ByteCodesInstructions.PLP:
                    PLP();
                    break;
                case Cpu6502ByteCodesInstructions.AND_IMMEDIATE:
                case Cpu6502ByteCodesInstructions.AND_ZEROPAGE:
                case Cpu6502ByteCodesInstructions.AND_ZEROPAGE_X:
                case Cpu6502ByteCodesInstructions.AND_ABSOLUTE:
                case Cpu6502ByteCodesInstructions.AND_ABSOLUTE_X:
                case Cpu6502ByteCodesInstructions.AND_ABSOLUTE_Y:
                case Cpu6502ByteCodesInstructions.AND_INDIRECT_X:
                case Cpu6502ByteCodesInstructions.AND_INDIRECT_Y:
                    AND(instruction);
                    break;
                case Cpu6502ByteCodesInstructions.EOR_IMMEDIATE:
                case Cpu6502ByteCodesInstructions.EOR_ZEROPAGE:
                case Cpu6502ByteCodesInstructions.EOR_ZEROPAGE_X:
                case Cpu6502ByteCodesInstructions.EOR_ABSOLUTE:
                case Cpu6502ByteCodesInstructions.EOR_ABSOLUTE_X:
                case Cpu6502ByteCodesInstructions.EOR_ABSOLUTE_Y:
                case Cpu6502ByteCodesInstructions.EOR_INDIRECT_X:
                case Cpu6502ByteCodesInstructions.EOR_INDIRECT_Y:
                    EOR(instruction);
                    break;
                case Cpu6502ByteCodesInstructions.ORA_IMMEDIATE:
                case Cpu6502ByteCodesInstructions.ORA_ZEROPAGE:
                case Cpu6502ByteCodesInstructions.ORA_ZEROPAGE_X:
                case Cpu6502ByteCodesInstructions.ORA_ABSOLUTE:
                case Cpu6502ByteCodesInstructions.ORA_ABSOLUTE_X:
                case Cpu6502ByteCodesInstructions.ORA_ABSOLUTE_Y:
                case Cpu6502ByteCodesInstructions.ORA_INDIRECT_X:
                case Cpu6502ByteCodesInstructions.ORA_INDIRECT_Y:
                    ORA(instruction);
                    break;
                case Cpu6502ByteCodesInstructions.BIT_ABSOLUTE:
                case Cpu6502ByteCodesInstructions.BIT_ZEROPAGE:
                    BIT(instruction);
                    break;
                case Cpu6502ByteCodesInstructions.ADC_IMMEDIATE:
                case Cpu6502ByteCodesInstructions.ADC_ZEROPAGE:
                case Cpu6502ByteCodesInstructions.ADC_ZEROPAGE_X:
                case Cpu6502ByteCodesInstructions.ADC_ABSOLUTE:  
                case Cpu6502ByteCodesInstructions.ADC_ABSOLUTE_X:
                case Cpu6502ByteCodesInstructions.ADC_ABSOLUTE_Y:
                case Cpu6502ByteCodesInstructions.ADC_INDIRECT_X:
                case Cpu6502ByteCodesInstructions.ADC_INDIRECT_Y:
                    ADC(instruction);
                    break;
                case Cpu6502ByteCodesInstructions.SBC_IMMEDIATE:
                case Cpu6502ByteCodesInstructions.SBC_ZEROPAGE:
                case Cpu6502ByteCodesInstructions.SBC_ZEROPAGE_X:
                case Cpu6502ByteCodesInstructions.SBC_ABSOLUTE:
                case Cpu6502ByteCodesInstructions.SBC_ABSOLUTE_X:
                case Cpu6502ByteCodesInstructions.SBC_ABSOLUTE_Y:
                case Cpu6502ByteCodesInstructions.SBC_INDIRECT_X:
                case Cpu6502ByteCodesInstructions.SBC_INDIRECT_Y:
                    SBC(instruction);
                    break;
                case Cpu6502ByteCodesInstructions.CMP_IMMEDIATE:
                case Cpu6502ByteCodesInstructions.CMP_ZEROPAGE:
                case Cpu6502ByteCodesInstructions.CMP_ZEROPAGE_X:
                case Cpu6502ByteCodesInstructions.CMP_ABSOLUTE:
                case Cpu6502ByteCodesInstructions.CMP_ABSOLUTE_X:
                case Cpu6502ByteCodesInstructions.CMP_ABSOLUTE_Y:
                case Cpu6502ByteCodesInstructions.CMP_INDIRECT_X:
                case Cpu6502ByteCodesInstructions.CMP_INDIRECT_Y:
                    CMP(instruction);
                    break;
                case Cpu6502ByteCodesInstructions.CPX_IMMEDIATE:
                case Cpu6502ByteCodesInstructions.CPX_ZEROPAGE:
                case Cpu6502ByteCodesInstructions.CPX_ABSOLUTE:
                    CPX(instruction);
                    break;
                case Cpu6502ByteCodesInstructions.CPY_IMMEDIATE:
                case Cpu6502ByteCodesInstructions.CPY_ZEROPAGE:
                case Cpu6502ByteCodesInstructions.CPY_ABSOLUTE:
                    CPY(instruction);
                    break;
                case Cpu6502ByteCodesInstructions.INC_ABSOLUTE:
                case Cpu6502ByteCodesInstructions.INC_ABSOLUTE_X:
                case Cpu6502ByteCodesInstructions.INC_ZEROPAGE:
                case Cpu6502ByteCodesInstructions.INC_ZEROPAGE_X:
                    INC(instruction);
                    break;
                case Cpu6502ByteCodesInstructions.INX:
                    INX();
                    break;
                case Cpu6502ByteCodesInstructions.INY:
                    INY();
                    break;
                case Cpu6502ByteCodesInstructions.DEX:
                    DEX();
                    break;
                case Cpu6502ByteCodesInstructions.DEY:
                    DEY();
                    break;
                case Cpu6502ByteCodesInstructions.DEC_ABSOLUTE:
                case Cpu6502ByteCodesInstructions.DEC_ABSOLUTE_X:
                case Cpu6502ByteCodesInstructions.DEC_ZEROPAGE:
                case Cpu6502ByteCodesInstructions.DEC_ZEROPAGE_X:
                    DEC(instruction);
                    break;
                case Cpu6502ByteCodesInstructions.ASL_ACCUMULATOR:
                case Cpu6502ByteCodesInstructions.ASL_ABSOLUTE:
                case Cpu6502ByteCodesInstructions.ASL_ABSOLUTE_X:
                case Cpu6502ByteCodesInstructions.ASL_ZEROPAGE:
                case Cpu6502ByteCodesInstructions.ASL_ZEROPAGE_X:
                    ASL(instruction);
                    break;
                case Cpu6502ByteCodesInstructions.LSR_ACCUMULATOR:
                case Cpu6502ByteCodesInstructions.LSR_ABSOLUTE:
                case Cpu6502ByteCodesInstructions.LSR_ABSOLUTE_X:
                case Cpu6502ByteCodesInstructions.LSR_ZEROPAGE:
                case Cpu6502ByteCodesInstructions.LSR_ZEROPAGE_X:
                    LSR(instruction);
                    break;
                case Cpu6502ByteCodesInstructions.ROL_ACCUMULATOR:
                case Cpu6502ByteCodesInstructions.ROL_ABSOLUTE:
                case Cpu6502ByteCodesInstructions.ROL_ABSOLUTE_X:
                case Cpu6502ByteCodesInstructions.ROL_ZEROPAGE:
                case Cpu6502ByteCodesInstructions.ROL_ZEROPAGE_X:
                    ROL(instruction);
                    break;
                case Cpu6502ByteCodesInstructions.ROR_ACCUMULATOR:
                case Cpu6502ByteCodesInstructions.ROR_ABSOLUTE:
                case Cpu6502ByteCodesInstructions.ROR_ABSOLUTE_X:
                case Cpu6502ByteCodesInstructions.ROR_ZEROPAGE:
                case Cpu6502ByteCodesInstructions.ROR_ZEROPAGE_X:
                    ROR(instruction);
                    break;
                case Cpu6502ByteCodesInstructions.JMP_ABSOLUTE:
                case Cpu6502ByteCodesInstructions.JMP_INDIRECT:
                    JMP(instruction);
                    break;
                case Cpu6502ByteCodesInstructions.JSR_ABSOLUTE:
                    JSR();
                    break;
                case Cpu6502ByteCodesInstructions.RTS:
                    RTS();
                    break;
                case Cpu6502ByteCodesInstructions.CLC:
                    CLC();
                    break;
                case Cpu6502ByteCodesInstructions.CLD:
                    CLD();
                    break;
                case Cpu6502ByteCodesInstructions.CLI:
                    CLI();
                    break;
                case Cpu6502ByteCodesInstructions.CLV:
                    CLV();
                    break;
                case Cpu6502ByteCodesInstructions.SEC:
                    SEC();
                    break;
                case Cpu6502ByteCodesInstructions.SED:
                    SED();
                    break;
                case Cpu6502ByteCodesInstructions.SEI:
                    SEI();
                    break;
                case Cpu6502ByteCodesInstructions.BRK:
                    BRK();
                    break;
                case Cpu6502ByteCodesInstructions.NOP:
                    NOP();
                    break;
                case Cpu6502ByteCodesInstructions.RTI:
                    RTI();
                    break;
                default:
                    throw new NotImplementedException($"Instruction not implemented: 0x{instruction:X2} ({instruction})");
            };
        }

        #region Load Operations
        void LDA(byte instruction) // Load Accumulator
        {
            ushort addr = instruction switch
            {
                Cpu6502ByteCodesInstructions.LDA_IMMEDIATE  => PC++,
                Cpu6502ByteCodesInstructions.LDA_ZEROPAGE   => ReadZeroPageAddrFromMemory(),
                Cpu6502ByteCodesInstructions.LDA_ZEROPAGE_X => ReadZeroPageAddrFromMemory(offset: X.Value),
                Cpu6502ByteCodesInstructions.LDA_ABSOLUTE   => ReadAddrFromMemory(),
                Cpu6502ByteCodesInstructions.LDA_ABSOLUTE_X => ReadAddrFromMemory(offset: X.Value),
                Cpu6502ByteCodesInstructions.LDA_ABSOLUTE_Y => ReadAddrFromMemory(offset: Y.Value),
                Cpu6502ByteCodesInstructions.LDA_INDIRECT_X => _memory[ReadZeroPageAddrFromMemory(offset: X.Value) % 0x0100],
                Cpu6502ByteCodesInstructions.LDA_INDIRECT_Y => _memory[ReadZeroPageAddrFromMemory() + Y.Value],
                _ => throw new NotImplementedException($"Instruction not implemented: {instruction:X2} ({instruction})")
            };
            A.LoadValueFromAddress(addr);
        }
        void LDX(byte instruction) // Load X Register
        {
            ushort addr = instruction switch
            {
                Cpu6502ByteCodesInstructions.LDX_IMMEDIATE  => PC++,
                Cpu6502ByteCodesInstructions.LDX_ZEROPAGE   => ReadZeroPageAddrFromMemory(),
                Cpu6502ByteCodesInstructions.LDX_ZEROPAGE_Y => ReadZeroPageAddrFromMemory(offset: Y.Value),
                Cpu6502ByteCodesInstructions.LDX_ABSOLUTE   => ReadAddrFromMemory(),
                Cpu6502ByteCodesInstructions.LDX_ABSOLUTE_Y => ReadAddrFromMemory(offset: Y.Value),
                _ => throw new NotImplementedException($"Instruction not implemented: {instruction:X2} ({instruction})")
            };
            X.LoadValueFromAddress(addr);
        }
        void LDY(byte instruction) // Load Y Register
        {
            ushort addr = instruction switch
            {
                Cpu6502ByteCodesInstructions.LDY_IMMEDIATE  => PC++,
                Cpu6502ByteCodesInstructions.LDY_ZEROPAGE   => ReadZeroPageAddrFromMemory(),
                Cpu6502ByteCodesInstructions.LDY_ZEROPAGE_X => ReadZeroPageAddrFromMemory(offset: X.Value),
                Cpu6502ByteCodesInstructions.LDY_ABSOLUTE   => ReadAddrFromMemory(),
                Cpu6502ByteCodesInstructions.LDY_ABSOLUTE_X => ReadAddrFromMemory(offset: X.Value),
                _ => throw new NotImplementedException($"Instruction not implemented: {instruction:X2} ({instruction})")
            };
            Y.LoadValueFromAddress(addr);
        }
        #endregion

        #region Store Operations
        void STA(byte instruction) // Store Accumulator
        {
            ushort addr = instruction switch
            {
                Cpu6502ByteCodesInstructions.STA_ZEROPAGE   => ReadZeroPageAddrFromMemory(),
                Cpu6502ByteCodesInstructions.STA_ZEROPAGE_X => ReadZeroPageAddrFromMemory(offset: X.Value),
                Cpu6502ByteCodesInstructions.STA_ABSOLUTE   => ReadAddrFromMemory(),
                Cpu6502ByteCodesInstructions.STA_ABSOLUTE_X => ReadAddrFromMemory(offset: X.Value),
                Cpu6502ByteCodesInstructions.STA_ABSOLUTE_Y => ReadAddrFromMemory(offset: Y.Value),
                Cpu6502ByteCodesInstructions.STA_INDIRECT_X => _memory[ReadZeroPageAddrFromMemory(offset: X.Value) % 0x0100],
                Cpu6502ByteCodesInstructions.STA_INDIRECT_Y => _memory[ReadZeroPageAddrFromMemory() + Y.Value],
                _ => throw new NotImplementedException($"Instruction not implemented: {instruction:X2} ({instruction})")
            };
            A.WriteValueToAddress(addr);
        }
        void STX(byte instruction) // Store X Register
        {
            ushort addr = instruction switch
            {
                Cpu6502ByteCodesInstructions.STX_ZEROPAGE   => ReadZeroPageAddrFromMemory(),
                Cpu6502ByteCodesInstructions.STX_ZEROPAGE_Y => ReadZeroPageAddrFromMemory(offset: Y.Value),
                Cpu6502ByteCodesInstructions.STX_ABSOLUTE   => ReadAddrFromMemory(),
                _ => throw new NotImplementedException($"Instruction not implemented: {instruction:X2} ({instruction})")
            };
            X.WriteValueToAddress(addr);
        }
        void STY(byte instruction) // Store Y Register
        {
            ushort addr = instruction switch
            {
                Cpu6502ByteCodesInstructions.STY_ZEROPAGE   => ReadZeroPageAddrFromMemory(),
                Cpu6502ByteCodesInstructions.STY_ZEROPAGE_X => ReadZeroPageAddrFromMemory(offset: X.Value),
                Cpu6502ByteCodesInstructions.STY_ABSOLUTE   => ReadAddrFromMemory(),
                _ => throw new NotImplementedException($"Instruction not implemented: {instruction:X2} ({instruction})")
            };
            Y.WriteValueToAddress(addr);
        }
        #endregion

        #region Register Transfers
        void TAX() => A.TransferValueToRegister(X); // Transfer Accumulator To X
        void TAY() => A.TransferValueToRegister(Y); // Transfer Accumulator To Y
        void TXA() => X.TransferValueToRegister(A); // Transfer X To Accumulator
        void TYA() => Y.TransferValueToRegister(A); // Transfer Y To Accumulator
        #endregion

        #region Stack Operations
        void TSX() // Transfer Stack Pointer to X
        {
            X.Value = SP;
            PS = PS with { Z = X.Value == 0, N = (X.Value & (1 << 7)) == (1 << 7) };
        }
        void TXS() => SP = X.Value; // Transfer X to Stack Pointer
        void PHA() => _memory[0x0100 + SP--] = A.Value; // Push Accumulator on Stack
        void PHP() => _memory[0x0100 + SP--] = PS; // Push Processor Status on Stack
        void PLA() => A.Value = _memory[0x0100 + SP++]; // Pull Accumulator from Stack
        void PLP() => PS = _memory[0x0100 + SP++]; // Pull Processor Status from Stack
        #endregion

        #region Logical
        void AND(byte instruction) // Logical AND
        {
            ushort addr = instruction switch
            {
                Cpu6502ByteCodesInstructions.AND_IMMEDIATE  => PC++,
                Cpu6502ByteCodesInstructions.AND_ZEROPAGE   => ReadZeroPageAddrFromMemory(),
                Cpu6502ByteCodesInstructions.AND_ZEROPAGE_X => ReadZeroPageAddrFromMemory(offset: X.Value),
                Cpu6502ByteCodesInstructions.AND_ABSOLUTE   => ReadAddrFromMemory(),
                Cpu6502ByteCodesInstructions.AND_ABSOLUTE_X => ReadAddrFromMemory(offset: X.Value),
                Cpu6502ByteCodesInstructions.AND_ABSOLUTE_Y => ReadAddrFromMemory(offset: Y.Value),
                Cpu6502ByteCodesInstructions.AND_INDIRECT_X => _memory[ReadZeroPageAddrFromMemory(offset: X.Value) % 0x0100],
                Cpu6502ByteCodesInstructions.AND_INDIRECT_Y => _memory[ReadZeroPageAddrFromMemory() + Y.Value],
                _ => throw new NotImplementedException()
            };
            var b = _memory[addr];
            A.Value = (byte)(A.Value & b);
            PS = PS with { Z = A.Value == 0, N = (A.Value & (1 << 7)) == (1 << 7) };
        } 
        void EOR(byte instruction) // Exclusive OR
        {
            ushort addr = instruction switch
            {
                Cpu6502ByteCodesInstructions.EOR_IMMEDIATE  => PC++,
                Cpu6502ByteCodesInstructions.EOR_ZEROPAGE   => ReadZeroPageAddrFromMemory(),
                Cpu6502ByteCodesInstructions.EOR_ZEROPAGE_X => ReadZeroPageAddrFromMemory(offset: X.Value),
                Cpu6502ByteCodesInstructions.EOR_ABSOLUTE   => ReadAddrFromMemory(),
                Cpu6502ByteCodesInstructions.EOR_ABSOLUTE_X => ReadAddrFromMemory(offset: X.Value),
                Cpu6502ByteCodesInstructions.EOR_ABSOLUTE_Y => ReadAddrFromMemory(offset: Y.Value),
                Cpu6502ByteCodesInstructions.EOR_INDIRECT_X => _memory[ReadZeroPageAddrFromMemory(offset: X.Value) % 0x0100],
                Cpu6502ByteCodesInstructions.EOR_INDIRECT_Y => _memory[ReadZeroPageAddrFromMemory() + Y.Value],
                _ => throw new NotImplementedException()
            };
            var b = _memory[addr];
            A.Value = (byte)(A.Value ^ b);
            PS = PS with { Z = A.Value == 0, N = (A.Value & (1 << 7)) == (1 << 7) };
        } 
        void ORA(byte instruction) // Logical Inclusive OR
        {
            ushort addr = instruction switch
            {
                Cpu6502ByteCodesInstructions.ORA_IMMEDIATE  => PC++,
                Cpu6502ByteCodesInstructions.ORA_ZEROPAGE   => ReadZeroPageAddrFromMemory(),
                Cpu6502ByteCodesInstructions.ORA_ZEROPAGE_X => ReadZeroPageAddrFromMemory(offset: X.Value),
                Cpu6502ByteCodesInstructions.ORA_ABSOLUTE   => ReadAddrFromMemory(),
                Cpu6502ByteCodesInstructions.ORA_ABSOLUTE_X => ReadAddrFromMemory(offset: X.Value),
                Cpu6502ByteCodesInstructions.ORA_ABSOLUTE_Y => ReadAddrFromMemory(offset: Y.Value),
                Cpu6502ByteCodesInstructions.ORA_INDIRECT_X => _memory[ReadZeroPageAddrFromMemory(offset: X.Value) % 0x0100],
                Cpu6502ByteCodesInstructions.ORA_INDIRECT_Y => _memory[ReadZeroPageAddrFromMemory() + Y.Value],
                _ => throw new NotImplementedException()
            };
            var b = _memory[addr];
            A.Value = (byte)(A.Value | b);
            PS = PS with { Z = A.Value == 0, N = (A.Value & (1 << 7)) == (1 << 7) };
        } 
        void BIT(byte instruction) // Bit Test
        {
            ushort addr = instruction switch
            {
                Cpu6502ByteCodesInstructions.BIT_ZEROPAGE => ReadZeroPageAddrFromMemory(),
                Cpu6502ByteCodesInstructions.BIT_ABSOLUTE => ReadAddrFromMemory(),
                _ => throw new NotImplementedException()
            };
            var b = _memory[addr];
            var r = b & A.Value;
            PS = PS with { Z = r == 0, V = (b & (1 << 6)) == (1 << 6), N = (b & (1 << 7)) == (1 << 7) };
        } 
        #endregion

        #region Arithmetic
        void ADC(byte instruction) // Add with Carry
        {
            ushort addr = instruction switch
            {
                Cpu6502ByteCodesInstructions.ADC_IMMEDIATE  => PC++,
                Cpu6502ByteCodesInstructions.ADC_ZEROPAGE   => ReadZeroPageAddrFromMemory(),
                Cpu6502ByteCodesInstructions.ADC_ZEROPAGE_X => ReadZeroPageAddrFromMemory(offset: X.Value),
                Cpu6502ByteCodesInstructions.ADC_ABSOLUTE   => ReadAddrFromMemory(),
                Cpu6502ByteCodesInstructions.ADC_ABSOLUTE_X => ReadAddrFromMemory(offset: X.Value),
                Cpu6502ByteCodesInstructions.ADC_ABSOLUTE_Y => ReadAddrFromMemory(offset: Y.Value),
                Cpu6502ByteCodesInstructions.ADC_INDIRECT_X => _memory[ReadZeroPageAddrFromMemory(offset: X.Value) % 0x0100],
                Cpu6502ByteCodesInstructions.ADC_INDIRECT_Y => _memory[ReadZeroPageAddrFromMemory() + Y.Value],
                _ => throw new NotImplementedException()
            };
            var b = _memory[addr];
            var res = A.Value + b;
            A.Value = (byte)(res % 0x100);

            PS = PS with { Z = A.Value == 0, N = (A.Value & (1 << 7)) == (1 << 7), V = byte.MaxValue < res };
        }
        void SBC(byte instruction) // Substract with Carry
        {
            ushort addr = instruction switch
            {
                Cpu6502ByteCodesInstructions.SBC_IMMEDIATE  => PC++,
                Cpu6502ByteCodesInstructions.SBC_ZEROPAGE   => ReadZeroPageAddrFromMemory(),
                Cpu6502ByteCodesInstructions.SBC_ZEROPAGE_X => ReadZeroPageAddrFromMemory(offset: X.Value),
                Cpu6502ByteCodesInstructions.SBC_ABSOLUTE   => ReadAddrFromMemory(),
                Cpu6502ByteCodesInstructions.SBC_ABSOLUTE_X => ReadAddrFromMemory(offset: X.Value),
                Cpu6502ByteCodesInstructions.SBC_ABSOLUTE_Y => ReadAddrFromMemory(offset: Y.Value),
                Cpu6502ByteCodesInstructions.SBC_INDIRECT_X => _memory[ReadZeroPageAddrFromMemory(offset: X.Value) % 0x0100],
                Cpu6502ByteCodesInstructions.SBC_INDIRECT_Y => _memory[ReadZeroPageAddrFromMemory() + Y.Value],
                _ => throw new NotImplementedException()
            };
            var b = _memory[addr];
            var carry = (PS.C ? 1 : 0) << 8;
            var res = carry + A.Value - b;
            A.Value = (byte)(Math.Abs(res) % 0x100);

            PS = PS with { Z = A.Value == 0, N = (A.Value & (1 << 7)) == (1 << 7), V = res <= byte.MaxValue };
            throw new NotImplementedException();
        }
        void CMP(byte instruction) // Compare Accumulator
        {
            ushort addr = instruction switch
            {
                Cpu6502ByteCodesInstructions.CMP_IMMEDIATE  => PC++,
                Cpu6502ByteCodesInstructions.CMP_ZEROPAGE   => ReadZeroPageAddrFromMemory(),
                Cpu6502ByteCodesInstructions.CMP_ZEROPAGE_X => ReadZeroPageAddrFromMemory(offset: X.Value),
                Cpu6502ByteCodesInstructions.CMP_ABSOLUTE   => ReadAddrFromMemory(),
                Cpu6502ByteCodesInstructions.CMP_ABSOLUTE_X => ReadAddrFromMemory(offset: X.Value),
                Cpu6502ByteCodesInstructions.CMP_ABSOLUTE_Y => ReadAddrFromMemory(offset: Y.Value),
                Cpu6502ByteCodesInstructions.CMP_INDIRECT_X => _memory[ReadZeroPageAddrFromMemory(offset: X.Value) % 0x0100],
                Cpu6502ByteCodesInstructions.CMP_INDIRECT_Y => _memory[ReadZeroPageAddrFromMemory() + Y.Value],
                _ => throw new NotImplementedException()
            };
            var b = _memory[addr];
            var res = A.Value - b;

            PS = PS with { Z = res == 0, N = (res & (1 << 7)) == (1 << 7), C = res <= 0 };
        }
        void CPX(byte instruction) // Compare X Register
        {
            ushort addr = instruction switch
            {
                Cpu6502ByteCodesInstructions.CPX_IMMEDIATE => PC++,
                Cpu6502ByteCodesInstructions.CPX_ZEROPAGE  => ReadZeroPageAddrFromMemory(),
                Cpu6502ByteCodesInstructions.CPX_ABSOLUTE  => ReadAddrFromMemory(),
                _ => throw new NotImplementedException()
            };
            var b = _memory[addr];
            var res = X.Value - b;

            PS = PS with { Z = res == 0, N = (res & (1 << 7)) == (1 << 7), C = res <= 0 };
        }
        void CPY(byte instruction) // Compare Y Register
        {
            ushort addr = instruction switch
            {
                Cpu6502ByteCodesInstructions.CPY_IMMEDIATE => PC++,
                Cpu6502ByteCodesInstructions.CPY_ZEROPAGE  => ReadZeroPageAddrFromMemory(),
                Cpu6502ByteCodesInstructions.CPY_ABSOLUTE  => ReadAddrFromMemory(),
                _ => throw new NotImplementedException()
            };
            var b = _memory[addr];
            var res = Y.Value - b;

            PS = PS with { Z = res == 0, N = (res & (1 << 7)) == (1 << 7), C = res <= 0 };
        }
        #endregion

        #region Increments & Decrements
        void INC(byte instruction) // Increment a Memory Location
        {
            ushort addr = instruction switch
            {
                Cpu6502ByteCodesInstructions.INC_ZEROPAGE   => ReadZeroPageAddrFromMemory(),
                Cpu6502ByteCodesInstructions.INC_ZEROPAGE_X => ReadZeroPageAddrFromMemory(offset: X.Value),
                Cpu6502ByteCodesInstructions.INC_ABSOLUTE   => ReadAddrFromMemory(),
                Cpu6502ByteCodesInstructions.INC_ABSOLUTE_X => ReadAddrFromMemory(offset: X.Value),
                _ => throw new NotImplementedException($"Instruction not implemented: {instruction:X2} ({instruction})")
            };
            var b = ++_memory[addr];
            PS = PS with { Z = b == 0, N = (b & (1 << 7)) == (1 << 7) };
        }
        void INX() => X.Increment(); // Increment the X Register
        void INY() => Y.Increment(); // Increment the Y Register
        void DEC(byte instruction) // Decrement a Memory Location
        {
            ushort addr = instruction switch
            {
                Cpu6502ByteCodesInstructions.DEC_ZEROPAGE   => ReadZeroPageAddrFromMemory(),
                Cpu6502ByteCodesInstructions.DEC_ZEROPAGE_X => ReadZeroPageAddrFromMemory(offset: X.Value),
                Cpu6502ByteCodesInstructions.DEC_ABSOLUTE   => ReadAddrFromMemory(),
                Cpu6502ByteCodesInstructions.DEC_ABSOLUTE_X => ReadAddrFromMemory(offset: X.Value),
                _ => throw new NotImplementedException($"Instruction not implemented: {instruction:X2} ({instruction})")
            };
            var b = --_memory[addr];
            PS = PS with { Z = b == 0, N = (b & (1 << 7)) == (1 << 7) };
        }
        void DEX() => X.Decrement(); // Decrement the X Register
        void DEY() => Y.Decrement(); // Decrement the Y Register
        #endregion

        #region Shifts
        void ASL(byte instruction) // Arithmetic Shift Left
        {
            ushort? addr = null;
            byte b;
            int res;
            if (instruction == Cpu6502ByteCodesInstructions.ASL_ACCUMULATOR)
                b = A.Value;
            else
            {
                addr = instruction switch
                {
                    Cpu6502ByteCodesInstructions.ASL_ZEROPAGE   => ReadZeroPageAddrFromMemory(),
                    Cpu6502ByteCodesInstructions.ASL_ZEROPAGE_X => ReadZeroPageAddrFromMemory(offset: X.Value),
                    Cpu6502ByteCodesInstructions.ASL_ABSOLUTE   => ReadAddrFromMemory(),
                    Cpu6502ByteCodesInstructions.ASL_ABSOLUTE_X => ReadAddrFromMemory(offset: X.Value),
                    _ => throw new NotImplementedException()
                };
                b = _memory[addr.Value];
            }

            res = b << 1;
            b = (byte)(res % 0x0100);
            if (instruction == Cpu6502ByteCodesInstructions.ASL_ACCUMULATOR)
                A.Value = b;
            else
            {
                _ = addr ?? throw new NullReferenceException();
                _memory[addr.Value] = b;
            }
            PS = PS with { C = (res & (1 << 8)) == 1 << 8, Z = b == 0, V = (b & (1 << 7)) == 1 << 7 };
        }
        void LSR(byte instruction) // Logical Shift Right
        {
            ushort? addr = null;
            byte b;
            int res;
            if (instruction == Cpu6502ByteCodesInstructions.LSR_ACCUMULATOR)
                b = A.Value;
            else
            {
                addr = instruction switch
                {
                    Cpu6502ByteCodesInstructions.LSR_ZEROPAGE   => ReadZeroPageAddrFromMemory(),
                    Cpu6502ByteCodesInstructions.LSR_ZEROPAGE_X => ReadZeroPageAddrFromMemory(offset: X.Value),
                    Cpu6502ByteCodesInstructions.LSR_ABSOLUTE   => ReadAddrFromMemory(),
                    Cpu6502ByteCodesInstructions.LSR_ABSOLUTE_X => ReadAddrFromMemory(offset: X.Value),
                    _ => throw new NotImplementedException()
                };
                b = _memory[addr.Value];
            }

            PS = PS with { C = (b & 1) == 1 };
            res = b >> 1;
            b = (byte)(res % 0x0100);
            if (instruction == Cpu6502ByteCodesInstructions.LSR_ACCUMULATOR)
                A.Value = b;
            else
            {
                _ = addr ?? throw new NullReferenceException();
                _memory[addr.Value] = b;
            }
            PS = PS with { Z = b == 0, V = (b & (1 << 7)) == 1 << 7 };
        } 
        void ROL(byte instruction) // Rotate Left
        {
            ushort? addr = null;
            byte b;
            int res;
            if (instruction == Cpu6502ByteCodesInstructions.ROL_ACCUMULATOR)
                b = A.Value;
            else
            {
                addr = instruction switch
                {
                    Cpu6502ByteCodesInstructions.ROL_ZEROPAGE   => ReadZeroPageAddrFromMemory(),
                    Cpu6502ByteCodesInstructions.ROL_ZEROPAGE_X => ReadZeroPageAddrFromMemory(offset: X.Value),
                    Cpu6502ByteCodesInstructions.ROL_ABSOLUTE   => ReadAddrFromMemory(),
                    Cpu6502ByteCodesInstructions.ROL_ABSOLUTE_X => ReadAddrFromMemory(offset: X.Value),
                    _ => throw new NotImplementedException()
                };
                b = _memory[addr.Value];
            }

            res = (b << 1) + (PS.C ? 1 : 0);
            b = (byte)(res % 0x0100);
            if (instruction == Cpu6502ByteCodesInstructions.ROL_ACCUMULATOR)
                A.Value = b;
            else
            {
                _ = addr ?? throw new NullReferenceException();
                _memory[addr.Value] = b;
            }
            PS = PS with { C = (res & (1 << 8)) == 1 << 8, Z = b == 0, V = (b & (1 << 7)) == 1 << 7 };
        } 
        void ROR(byte instruction) // Rotate Right
        {
            ushort? addr = null;
            byte b;
            int res;
            if (instruction == Cpu6502ByteCodesInstructions.ROR_ACCUMULATOR)
                b = A.Value;
            else
            {
                addr = instruction switch
                {
                    Cpu6502ByteCodesInstructions.ROR_ZEROPAGE   => ReadZeroPageAddrFromMemory(),
                    Cpu6502ByteCodesInstructions.ROR_ZEROPAGE_X => ReadZeroPageAddrFromMemory(offset: X.Value),
                    Cpu6502ByteCodesInstructions.ROR_ABSOLUTE   => ReadAddrFromMemory(),
                    Cpu6502ByteCodesInstructions.ROR_ABSOLUTE_X => ReadAddrFromMemory(offset: X.Value),
                    _ => throw new NotImplementedException()
                };
                b = _memory[addr.Value];
            }

            PS = PS with { C = (b & 1) == 1 };
            res = (b >> 1) + (PS.C ? 1 << 7 : 0);
            b = (byte)(res % 0x0100);
            if (instruction == Cpu6502ByteCodesInstructions.ROR_ACCUMULATOR)
                A.Value = b;
            else
            {
                _ = addr ?? throw new NullReferenceException();
                _memory[addr.Value] = b;
            }
            PS = PS with { Z = b == 0, V = (b & (1 << 7)) == 1 << 7 };
        } 
        #endregion

        #region Jumps & Calls
        void JMP(byte instruction) // Jump to Another Location
        {
            ushort addr = instruction switch
            {
                Cpu6502ByteCodesInstructions.JMP_ABSOLUTE => ReadAddrFromMemory(),
                Cpu6502ByteCodesInstructions.JMP_INDIRECT => _memory[ReadAddrFromMemory()],
                _ => throw new NotImplementedException()
            };
            PC = addr;
        }
        void JSR() // Jump to a Subroutine
        {
            ushort addr = ReadAddrFromMemory();

            var currentAddr = PC - 1;
            _memory[PS--] = (byte)(currentAddr % 0x0100);
            currentAddr = (byte)(currentAddr >> 8);
            _memory[PS--] = (byte)(currentAddr % 0x0100);

            PC = addr;
        }
        void RTS() // Return from Subroutine
        {
            byte upper = _memory[SP++];
            byte lower = _memory[SP++];

            ushort addr = ComputeAddrFromUL(lower, upper, offset: 1);
            PC = addr;
        }
        #endregion

        #region Branches
        void BCC() // Branch if Carry Flag Clear
        { }
        void BCS() // Branch if Carry Flag Set
        { }
        void BEQ() // Branch if Zero Flag Set
        { }
        void BMI() // Branch if Negative Flag Set
        { }
        void BNE() // Branch if Zero Flag Clear
        { }
        void BPL() // Branch if Negative Flag Clear
        { }
        void BVC() // Branch if Overflow Flag Clear
        { }
        void BVS() // Branch if Overflow Flag Set
        { }
        #endregion

        #region Status Flag Changes
        void CLC() => PS = PS with { C = false }; // Clear Carry Flag
        void CLD() => PS = PS with { D = false }; // Clear Decimal Mode Flag
        void CLI() => PS = PS with { I = false }; // Clear Interrupt Disable Flag
        void CLV() => PS = PS with { V = false }; // Clear Overflow Flag
        void SEC() => PS = PS with { C = true }; // Set Carry Flag
        void SED() => PS = PS with { D = true }; // Set Decimal Mode Flag
        void SEI() => PS = PS with { I = true }; // Set Interrupt Disable Flag
        #endregion

        #region System Functions
        void BRK() // Force an Interrupt
        { } 
        void NOP() // No Operation
        { } 
        void RTI() // Return from Interrupt
        { } 
        #endregion

        ushort ReadZeroPageAddrFromMemory(byte offset = 0) => ComputeAddrFromUL(_memory[PC++], 0, offset);
        ushort ReadAddrFromMemory(byte offset = 0) => ComputeAddrFromUL(_memory[PC++], _memory[PC++], offset);

        static ushort ComputeAddrFromUL(byte lower, byte upper, byte offset = 0) => (ushort)(upper * (byte.MaxValue + 1) + lower + offset);
        ushort GetNmiAddress() => ComputeAddrFromUL(_memory[Cpu6502Consts.NMI_ADDRESS_L], _memory[Cpu6502Consts.NMI_ADDRESS_U]);
        ushort GetResetAddress() => ComputeAddrFromUL(_memory[Cpu6502Consts.RESET_ADDRESS_L], _memory[Cpu6502Consts.RESET_ADDRESS_U]);
        ushort GetBrkAddress() => ComputeAddrFromUL(_memory[Cpu6502Consts.BRK_ADDRESS_L], _memory[Cpu6502Consts.BRK_ADDRESS_U]);
    }

    public record ProcessorStatus
    {
        public bool C { get; init; } // Carry Flag
        public bool Z { get; init; } // Zero Flag
        public bool I { get; init; } // Interrupt Disable
        public bool D { get; init; } // Decimal Mode Flag
        public bool B { get; init; } // Break Command
        public bool V { get; init; } // Overflow Flag
        public bool N { get; init; } // Negative Flag

        public static implicit operator byte(ProcessorStatus ps) => (byte)((ps.C ? 64 : 0) + (ps.Z ? 32 : 0) + (ps.I ? 16 : 0) + (ps.D ? 8 : 0) + (ps.B ? 4 : 0) + (ps.V ? 2 : 0) + (ps.N ? 1 : 0));
        public static implicit operator ProcessorStatus(byte b) => new()
        {
            N = (b & (1 << 0)) == (1 << 0),
            V = (b & (1 << 1)) == (1 << 1),
            B = (b & (1 << 2)) == (1 << 2),
            D = (b & (1 << 3)) == (1 << 3),
            I = (b & (1 << 4)) == (1 << 4),
            Z = (b & (1 << 5)) == (1 << 5),
            C = (b & (1 << 6)) == (1 << 6),
        };
    }

    [DebuggerDisplay("{Value}, {HexValue, nq}, {BinValue, nq}")]
    public class Register
    {
        readonly WeakReference<Cpu6502> _cpu;
        Cpu6502 CPU => _cpu.TryGetTarget(out var cpu) ? cpu : throw new ArgumentNullException();
        public byte Value { get; internal set; }
        private string HexValue => "0x" + Value.ToString("X2");
        private string BinValue => "0b" + Convert.ToString(Value, 2).PadLeft(8, '0');

        public Register(Cpu6502 cpu)
        {
            _cpu = new(cpu);
            Value = 0;
        }

        public void LoadValueFromAddress(ushort address)
        {
            Value = CPU._memory[address];
            CPU.PS = CPU.PS with { Z = Value == 0, N = (Value & (1 << 7)) == (1 << 7) };
        }
        public void WriteValueToAddress(ushort address) => CPU._memory[address] = Value;
        public void TransferValueToRegister(Register r)
        {
            r.Value = Value;
            CPU.PS = CPU.PS with { Z = r.Value == 0, N = (r.Value & (1 << 7)) == (1 << 7) };
        }
        public void Increment()
        {
            Value++;
            CPU.PS = CPU.PS with { Z = Value == 0, N = (Value & (1 << 7)) == (1 << 7) };
        }
        public void Decrement()
        {
            Value--;
            CPU.PS = CPU.PS with { Z = Value == 0, N = (Value & (1 << 7)) == (1 << 7) };
        }
    }
}
