using System;
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
        readonly internal MemoryManager _memoryManager;

        public Cpu6502(MemoryManager memoryManager)
        {
            A = new(this);
            X = new(this);
            Y = new(this);
            _memoryManager = memoryManager;
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
            byte instruction = _memoryManager.ReadAt(PC++);

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
                case Cpu6502ByteCodesInstructions.BCC_ABSOLUTE:
                    BCC();
                    break;
                case Cpu6502ByteCodesInstructions.BCS_ABSOLUTE:
                    BCS();
                    break;
                case Cpu6502ByteCodesInstructions.BEQ_ABSOLUTE:
                    BEQ();
                    break;
                case Cpu6502ByteCodesInstructions.BMI_ABSOLUTE:
                    BMI();
                    break;
                case Cpu6502ByteCodesInstructions.BNE_ABSOLUTE:
                    BNE();
                    break;
                case Cpu6502ByteCodesInstructions.BPL_ABSOLUTE:
                    BPL();
                    break;
                case Cpu6502ByteCodesInstructions.BVC_ABSOLUTE:
                    BVC();
                    break;
                case Cpu6502ByteCodesInstructions.BVS_ABSOLUTE:
                    BVS();
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
        /// <summary>
        /// Load Accumulator
        /// </summary>
        /// <param name="instruction">Byte code instruction</param>
        void LDA(byte instruction)
        {
            ushort addr = instruction switch
            {
                Cpu6502ByteCodesInstructions.LDA_IMMEDIATE => PC++,
                Cpu6502ByteCodesInstructions.LDA_ZEROPAGE => ReadZeroPageAddrFromMemory(),
                Cpu6502ByteCodesInstructions.LDA_ZEROPAGE_X => ReadZeroPageAddrFromMemory(offset: X.Value),
                Cpu6502ByteCodesInstructions.LDA_ABSOLUTE => ReadAddrFromMemory(),
                Cpu6502ByteCodesInstructions.LDA_ABSOLUTE_X => ReadAddrFromMemory(offset: X.Value),
                Cpu6502ByteCodesInstructions.LDA_ABSOLUTE_Y => ReadAddrFromMemory(offset: Y.Value),
                Cpu6502ByteCodesInstructions.LDA_INDIRECT_X => _memoryManager.ReadAt(ReadZeroPageAddrFromMemory(offset: X.Value) & 0xFF),
                Cpu6502ByteCodesInstructions.LDA_INDIRECT_Y => _memoryManager.ReadAt(ReadZeroPageAddrFromMemory() + Y.Value),
                _ => throw new NotImplementedException($"Instruction not implemented: {instruction:X2} ({instruction})")
            };
            A.LoadValueFromAddress(addr);
        }
        /// <summary>
        /// Load X Register
        /// </summary>
        /// <param name="instruction">Byte code instruction</param>
        void LDX(byte instruction)
        {
            ushort addr = instruction switch
            {
                Cpu6502ByteCodesInstructions.LDX_IMMEDIATE => PC++,
                Cpu6502ByteCodesInstructions.LDX_ZEROPAGE => ReadZeroPageAddrFromMemory(),
                Cpu6502ByteCodesInstructions.LDX_ZEROPAGE_Y => ReadZeroPageAddrFromMemory(offset: Y.Value),
                Cpu6502ByteCodesInstructions.LDX_ABSOLUTE => ReadAddrFromMemory(),
                Cpu6502ByteCodesInstructions.LDX_ABSOLUTE_Y => ReadAddrFromMemory(offset: Y.Value),
                _ => throw new NotImplementedException($"Instruction not implemented: {instruction:X2} ({instruction})")
            };
            X.LoadValueFromAddress(addr);
        }
        /// <summary>
        /// Load Y Register
        /// </summary>
        /// <param name="instruction">Byte code instruction</param>
        void LDY(byte instruction)
        {
            ushort addr = instruction switch
            {
                Cpu6502ByteCodesInstructions.LDY_IMMEDIATE => PC++,
                Cpu6502ByteCodesInstructions.LDY_ZEROPAGE => ReadZeroPageAddrFromMemory(),
                Cpu6502ByteCodesInstructions.LDY_ZEROPAGE_X => ReadZeroPageAddrFromMemory(offset: X.Value),
                Cpu6502ByteCodesInstructions.LDY_ABSOLUTE => ReadAddrFromMemory(),
                Cpu6502ByteCodesInstructions.LDY_ABSOLUTE_X => ReadAddrFromMemory(offset: X.Value),
                _ => throw new NotImplementedException($"Instruction not implemented: {instruction:X2} ({instruction})")
            };
            Y.LoadValueFromAddress(addr);
        }
        #endregion

        #region Store Operations
        /// <summary>
        /// Store Accumulator
        /// </summary>
        /// <param name="instruction">Byte code instruction</param>
        void STA(byte instruction)
        {
            ushort addr = instruction switch
            {
                Cpu6502ByteCodesInstructions.STA_ZEROPAGE => ReadZeroPageAddrFromMemory(),
                Cpu6502ByteCodesInstructions.STA_ZEROPAGE_X => ReadZeroPageAddrFromMemory(offset: X.Value),
                Cpu6502ByteCodesInstructions.STA_ABSOLUTE => ReadAddrFromMemory(),
                Cpu6502ByteCodesInstructions.STA_ABSOLUTE_X => ReadAddrFromMemory(offset: X.Value),
                Cpu6502ByteCodesInstructions.STA_ABSOLUTE_Y => ReadAddrFromMemory(offset: Y.Value),
                Cpu6502ByteCodesInstructions.STA_INDIRECT_X => _memoryManager.ReadAt(ReadZeroPageAddrFromMemory(offset: X.Value) & 0xFF),
                Cpu6502ByteCodesInstructions.STA_INDIRECT_Y => _memoryManager.ReadAt(ReadZeroPageAddrFromMemory() + Y.Value),
                _ => throw new NotImplementedException($"Instruction not implemented: {instruction:X2} ({instruction})")
            };
            A.WriteValueToAddress(addr);
        }
        /// <summary>
        /// Store X Register
        /// </summary>
        /// <param name="instruction">Byte code instruction</param>
        void STX(byte instruction)
        {
            ushort addr = instruction switch
            {
                Cpu6502ByteCodesInstructions.STX_ZEROPAGE => ReadZeroPageAddrFromMemory(),
                Cpu6502ByteCodesInstructions.STX_ZEROPAGE_Y => ReadZeroPageAddrFromMemory(offset: Y.Value),
                Cpu6502ByteCodesInstructions.STX_ABSOLUTE => ReadAddrFromMemory(),
                _ => throw new NotImplementedException($"Instruction not implemented: {instruction:X2} ({instruction})")
            };
            X.WriteValueToAddress(addr);
        }
        /// <summary>
        /// Store Y Register
        /// </summary>
        /// <param name="instruction">Byte code instruction</param>
        void STY(byte instruction)
        {
            ushort addr = instruction switch
            {
                Cpu6502ByteCodesInstructions.STY_ZEROPAGE => ReadZeroPageAddrFromMemory(),
                Cpu6502ByteCodesInstructions.STY_ZEROPAGE_X => ReadZeroPageAddrFromMemory(offset: X.Value),
                Cpu6502ByteCodesInstructions.STY_ABSOLUTE => ReadAddrFromMemory(),
                _ => throw new NotImplementedException($"Instruction not implemented: {instruction:X2} ({instruction})")
            };
            Y.WriteValueToAddress(addr);
        }
        #endregion

        #region Register Transfers
        /// <summary>
        /// Transfer Accumulator To X
        /// </summary>
        void TAX() => A.TransferValueToRegister(X);
        /// <summary>
        /// Transfer Accumulator To Y
        /// </summary>
        void TAY() => A.TransferValueToRegister(Y);
        /// <summary>
        /// Transfer X To Accumulator
        /// </summary>
        void TXA() => X.TransferValueToRegister(A);
        /// <summary>
        /// Transfer Y To Accumulator
        /// </summary>
        void TYA() => Y.TransferValueToRegister(A);
        #endregion

        #region Stack Operations
        /// <summary>
        /// Transfer Stack Pointer to X
        /// </summary>
        void TSX()
        {
            X.Value = SP;
            PS = PS with { Z = X.Value == 0, N = (X.Value >> 7 & 1) == 1 };
        }
        /// <summary>
        /// Transfer X to Stack Pointer
        /// </summary>
        void TXS() => SP = X.Value;
        /// <summary>
        /// Push Accumulator on Stack
        /// </summary>
        void PHA() => _memoryManager.WriteAt(0x0100 | SP--, A.Value);
        /// <summary>
        /// Push Processor Status on Stack
        /// </summary>
        void PHP() => _memoryManager.WriteAt(0x0100 | SP--, PS);
        /// <summary>
        /// Pull Accumulator from Stack
        /// </summary>
        void PLA() => A.Value = _memoryManager.ReadAt(0x0100 | SP++);
        /// <summary>
        /// Pull Processor Status from Stack
        /// </summary>
        void PLP() => PS = _memoryManager.ReadAt(0x0100 | SP++);
        #endregion

        #region Logical
        /// <summary>
        /// Logical AND 
        /// </summary>
        /// <param name="instruction">Byte code instruction</param>
        void AND(byte instruction)
        {
            ushort addr = instruction switch
            {
                Cpu6502ByteCodesInstructions.AND_IMMEDIATE => PC++,
                Cpu6502ByteCodesInstructions.AND_ZEROPAGE => ReadZeroPageAddrFromMemory(),
                Cpu6502ByteCodesInstructions.AND_ZEROPAGE_X => ReadZeroPageAddrFromMemory(offset: X.Value),
                Cpu6502ByteCodesInstructions.AND_ABSOLUTE => ReadAddrFromMemory(),
                Cpu6502ByteCodesInstructions.AND_ABSOLUTE_X => ReadAddrFromMemory(offset: X.Value),
                Cpu6502ByteCodesInstructions.AND_ABSOLUTE_Y => ReadAddrFromMemory(offset: Y.Value),
                Cpu6502ByteCodesInstructions.AND_INDIRECT_X => _memoryManager.ReadAt(ReadZeroPageAddrFromMemory(offset: X.Value) & 0xFF),
                Cpu6502ByteCodesInstructions.AND_INDIRECT_Y => _memoryManager.ReadAt(ReadZeroPageAddrFromMemory() + Y.Value),
                _ => throw new NotImplementedException()
            };
            var b = _memoryManager.ReadAt(addr);
            A.Value = (byte)(A.Value & b);
            PS = PS with { Z = A.Value == 0, N = (A.Value >> 7 & 1) == 1 };
        }
        /// <summary>
        /// Exclusive OR
        /// </summary>
        /// <param name="instruction">Byte code instruction</param>
        void EOR(byte instruction)
        {
            ushort addr = instruction switch
            {
                Cpu6502ByteCodesInstructions.EOR_IMMEDIATE => PC++,
                Cpu6502ByteCodesInstructions.EOR_ZEROPAGE => ReadZeroPageAddrFromMemory(),
                Cpu6502ByteCodesInstructions.EOR_ZEROPAGE_X => ReadZeroPageAddrFromMemory(offset: X.Value),
                Cpu6502ByteCodesInstructions.EOR_ABSOLUTE => ReadAddrFromMemory(),
                Cpu6502ByteCodesInstructions.EOR_ABSOLUTE_X => ReadAddrFromMemory(offset: X.Value),
                Cpu6502ByteCodesInstructions.EOR_ABSOLUTE_Y => ReadAddrFromMemory(offset: Y.Value),
                Cpu6502ByteCodesInstructions.EOR_INDIRECT_X => _memoryManager.ReadAt(ReadZeroPageAddrFromMemory(offset: X.Value) & 0xFF),
                Cpu6502ByteCodesInstructions.EOR_INDIRECT_Y => _memoryManager.ReadAt(ReadZeroPageAddrFromMemory() + Y.Value),
                _ => throw new NotImplementedException()
            };
            var b = _memoryManager.ReadAt(addr);
            A.Value = (byte)(A.Value ^ b);
            PS = PS with { Z = A.Value == 0, N = (A.Value >> 7 & 1) == 1 };
        }
        /// <summary>
        /// Logical Inclusive OR
        /// </summary>
        /// <param name="instruction">Byte code instruction</param>
        void ORA(byte instruction)
        {
            ushort addr = instruction switch
            {
                Cpu6502ByteCodesInstructions.ORA_IMMEDIATE => PC++,
                Cpu6502ByteCodesInstructions.ORA_ZEROPAGE => ReadZeroPageAddrFromMemory(),
                Cpu6502ByteCodesInstructions.ORA_ZEROPAGE_X => ReadZeroPageAddrFromMemory(offset: X.Value),
                Cpu6502ByteCodesInstructions.ORA_ABSOLUTE => ReadAddrFromMemory(),
                Cpu6502ByteCodesInstructions.ORA_ABSOLUTE_X => ReadAddrFromMemory(offset: X.Value),
                Cpu6502ByteCodesInstructions.ORA_ABSOLUTE_Y => ReadAddrFromMemory(offset: Y.Value),
                Cpu6502ByteCodesInstructions.ORA_INDIRECT_X => _memoryManager.ReadAt(ReadZeroPageAddrFromMemory(offset: X.Value) & 0xFF),
                Cpu6502ByteCodesInstructions.ORA_INDIRECT_Y => _memoryManager.ReadAt(ReadZeroPageAddrFromMemory() + Y.Value),
                _ => throw new NotImplementedException()
            };
            var b = _memoryManager.ReadAt(addr);
            A.Value = (byte)(A.Value | b);
            PS = PS with { Z = A.Value == 0, N = (A.Value >> 7 & 1) == 1 };
        }
        /// <summary>
        /// Bit Test
        /// </summary>
        /// <param name="instruction">Byte code instruction</param>
        void BIT(byte instruction)
        {
            ushort addr = instruction switch
            {
                Cpu6502ByteCodesInstructions.BIT_ZEROPAGE => ReadZeroPageAddrFromMemory(),
                Cpu6502ByteCodesInstructions.BIT_ABSOLUTE => ReadAddrFromMemory(),
                _ => throw new NotImplementedException()
            };
            var b = _memoryManager.ReadAt(addr);
            var r = b & A.Value;
            PS = PS with { Z = r == 0, V = (b >> 6 & 1) == 1, N = (b >> 7 & 1) == 1 };
        }
        #endregion

        #region Arithmetic
        /// <summary>
        /// Add with Carry
        /// </summary>
        /// <param name="instruction">Byte code instruction</param>
        void ADC(byte instruction)
        {
            if (PS.D)
                throw new NotImplementedException("Decimal Mode not implemented yet.");
            ushort addr = instruction switch
            {
                Cpu6502ByteCodesInstructions.ADC_IMMEDIATE => PC++,
                Cpu6502ByteCodesInstructions.ADC_ZEROPAGE => ReadZeroPageAddrFromMemory(),
                Cpu6502ByteCodesInstructions.ADC_ZEROPAGE_X => ReadZeroPageAddrFromMemory(offset: X.Value),
                Cpu6502ByteCodesInstructions.ADC_ABSOLUTE => ReadAddrFromMemory(),
                Cpu6502ByteCodesInstructions.ADC_ABSOLUTE_X => ReadAddrFromMemory(offset: X.Value),
                Cpu6502ByteCodesInstructions.ADC_ABSOLUTE_Y => ReadAddrFromMemory(offset: Y.Value),
                Cpu6502ByteCodesInstructions.ADC_INDIRECT_X => _memoryManager.ReadAt(ReadZeroPageAddrFromMemory(offset: X.Value) & 0xFF),
                Cpu6502ByteCodesInstructions.ADC_INDIRECT_Y => _memoryManager.ReadAt(ReadZeroPageAddrFromMemory() + Y.Value),
                _ => throw new NotImplementedException()
            };
            var b = _memoryManager.ReadAt(addr);
            var res = A.Value + b + (PS.C ? 1 : 0);
            PS = PS with { C = res > 0xFF, V = (A.Value ^ b >> 7 & 1) == 0 && (A.Value ^ res >> 7 & 1) == 1 };
            
            A.Value = (byte)(res & 0xFF);
            PS = PS with { Z = A.Value == 0, N = (A.Value >> 7 & 1) == 1 };
        }
        /// <summary>
        /// Substract with Carry
        /// </summary>
        /// <param name="instruction">Byte code instruction</param>
        void SBC(byte instruction)
        {
            if (PS.D)
                throw new NotImplementedException("Decimal Mode not implemented yet.");
            ushort addr = instruction switch
            {
                Cpu6502ByteCodesInstructions.SBC_IMMEDIATE => PC++,
                Cpu6502ByteCodesInstructions.SBC_ZEROPAGE => ReadZeroPageAddrFromMemory(),
                Cpu6502ByteCodesInstructions.SBC_ZEROPAGE_X => ReadZeroPageAddrFromMemory(offset: X.Value),
                Cpu6502ByteCodesInstructions.SBC_ABSOLUTE => ReadAddrFromMemory(),
                Cpu6502ByteCodesInstructions.SBC_ABSOLUTE_X => ReadAddrFromMemory(offset: X.Value),
                Cpu6502ByteCodesInstructions.SBC_ABSOLUTE_Y => ReadAddrFromMemory(offset: Y.Value),
                Cpu6502ByteCodesInstructions.SBC_INDIRECT_X => _memoryManager.ReadAt(ReadZeroPageAddrFromMemory(offset: X.Value) & 0xFF),
                Cpu6502ByteCodesInstructions.SBC_INDIRECT_Y => _memoryManager.ReadAt(ReadZeroPageAddrFromMemory() + Y.Value),
                _ => throw new NotImplementedException()
            };
            var b = _memoryManager.ReadAt(addr);
            var res = A.Value - b - (PS.C ? 0 : 1);
            PS = PS with { C = res <= 0xFF, V = (A.Value ^ b >> 7 & 1) == 1 && (A.Value ^ res >> 7 & 1) == 1 };

            A.Value = (byte)(res & 0xFF);
            PS = PS with { Z = A.Value == 0, N = (A.Value >> 7 & 1) == 1 };
        }
        /// <summary>
        /// Compare Accumulator
        /// </summary>
        /// <param name="instruction">Byte code instruction</param>
        void CMP(byte instruction)
        {
            ushort addr = instruction switch
            {
                Cpu6502ByteCodesInstructions.CMP_IMMEDIATE => PC++,
                Cpu6502ByteCodesInstructions.CMP_ZEROPAGE => ReadZeroPageAddrFromMemory(),
                Cpu6502ByteCodesInstructions.CMP_ZEROPAGE_X => ReadZeroPageAddrFromMemory(offset: X.Value),
                Cpu6502ByteCodesInstructions.CMP_ABSOLUTE => ReadAddrFromMemory(),
                Cpu6502ByteCodesInstructions.CMP_ABSOLUTE_X => ReadAddrFromMemory(offset: X.Value),
                Cpu6502ByteCodesInstructions.CMP_ABSOLUTE_Y => ReadAddrFromMemory(offset: Y.Value),
                Cpu6502ByteCodesInstructions.CMP_INDIRECT_X => _memoryManager.ReadAt(ReadZeroPageAddrFromMemory(offset: X.Value) & 0xFF),
                Cpu6502ByteCodesInstructions.CMP_INDIRECT_Y => _memoryManager.ReadAt(ReadZeroPageAddrFromMemory() + Y.Value),
                _ => throw new NotImplementedException()
            };
            var b = _memoryManager.ReadAt(addr);
            var res = A.Value - b;

            PS = PS with { Z = res == 0, N = (res >> 7 & 1) == 1, C = res <= 0 };
        }
        /// <summary>
        /// Compare X Register
        /// </summary>
        /// <param name="instruction">Byte code instruction</param>
        void CPX(byte instruction)
        {
            ushort addr = instruction switch
            {
                Cpu6502ByteCodesInstructions.CPX_IMMEDIATE => PC++,
                Cpu6502ByteCodesInstructions.CPX_ZEROPAGE => ReadZeroPageAddrFromMemory(),
                Cpu6502ByteCodesInstructions.CPX_ABSOLUTE => ReadAddrFromMemory(),
                _ => throw new NotImplementedException()
            };
            var b = _memoryManager.ReadAt(addr);
            var res = X.Value - b;

            PS = PS with { Z = res == 0, N = (res >> 7 & 1) == 1, C = res <= 0 };
        }
        /// <summary>
        /// Compare Y Register
        /// </summary>
        /// <param name="instruction">Byte code instruction</param>
        void CPY(byte instruction)
        {
            ushort addr = instruction switch
            {
                Cpu6502ByteCodesInstructions.CPY_IMMEDIATE => PC++,
                Cpu6502ByteCodesInstructions.CPY_ZEROPAGE => ReadZeroPageAddrFromMemory(),
                Cpu6502ByteCodesInstructions.CPY_ABSOLUTE => ReadAddrFromMemory(),
                _ => throw new NotImplementedException()
            };
            var b = _memoryManager.ReadAt(addr);
            var res = Y.Value - b;

            PS = PS with { Z = res == 0, N = (res >> 7 & 1) == 1, C = res <= 0 };
        }
        #endregion

        #region Increments & Decrements
        /// <summary>
        /// Increment a Memory Location
        /// </summary>
        /// <param name="instruction">Byte code instruction</param>
        void INC(byte instruction)
        {
            ushort addr = instruction switch
            {
                Cpu6502ByteCodesInstructions.INC_ZEROPAGE => ReadZeroPageAddrFromMemory(),
                Cpu6502ByteCodesInstructions.INC_ZEROPAGE_X => ReadZeroPageAddrFromMemory(offset: X.Value),
                Cpu6502ByteCodesInstructions.INC_ABSOLUTE => ReadAddrFromMemory(),
                Cpu6502ByteCodesInstructions.INC_ABSOLUTE_X => ReadAddrFromMemory(offset: X.Value),
                _ => throw new NotImplementedException($"Instruction not implemented: {instruction:X2} ({instruction})")
            };
            var b = _memoryManager.ReadAt(addr) + 1;
            _memoryManager.WriteAt(addr, (byte)b);
            PS = PS with { Z = b == 0, N = (b >> 7 & 1) == 1 };
        }
        /// <summary>
        /// Increment the X Register
        /// </summary>
        void INX() => X.Increment();
        /// <summary>
        /// Increment the Y Register
        /// </summary>
        void INY() => Y.Increment();
        /// <summary>
        /// Decrement a Memory Location
        /// </summary>
        /// <param name="instruction">Byte code instruction</param>
        void DEC(byte instruction)
        {
            ushort addr = instruction switch
            {
                Cpu6502ByteCodesInstructions.DEC_ZEROPAGE => ReadZeroPageAddrFromMemory(),
                Cpu6502ByteCodesInstructions.DEC_ZEROPAGE_X => ReadZeroPageAddrFromMemory(offset: X.Value),
                Cpu6502ByteCodesInstructions.DEC_ABSOLUTE => ReadAddrFromMemory(),
                Cpu6502ByteCodesInstructions.DEC_ABSOLUTE_X => ReadAddrFromMemory(offset: X.Value),
                _ => throw new NotImplementedException($"Instruction not implemented: {instruction:X2} ({instruction})")
            };
            var b = _memoryManager.ReadAt(addr) - 1;
            _memoryManager.WriteAt(addr, (byte)b);
            PS = PS with { Z = b == 0, N = (b >> 7 & 1) == 1 };
        }
        /// <summary>
        /// /// Decrement the X Register
        /// </summary>
        void DEX() => X.Decrement();
        /// <summary>
        /// Decrement the Y Register
        /// </summary>
        void DEY() => Y.Decrement();
        #endregion

        #region Shifts
        /// <summary>
        /// Arithmetic Shift Left
        /// </summary>
        /// <param name="instruction">Byte code instruction</param>
        void ASL(byte instruction)
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
                    Cpu6502ByteCodesInstructions.ASL_ZEROPAGE => ReadZeroPageAddrFromMemory(),
                    Cpu6502ByteCodesInstructions.ASL_ZEROPAGE_X => ReadZeroPageAddrFromMemory(offset: X.Value),
                    Cpu6502ByteCodesInstructions.ASL_ABSOLUTE => ReadAddrFromMemory(),
                    Cpu6502ByteCodesInstructions.ASL_ABSOLUTE_X => ReadAddrFromMemory(offset: X.Value),
                    _ => throw new NotImplementedException()
                };
                b = _memoryManager.ReadAt(addr.Value);
            }

            res = b << 1;
            b = (byte)(res % 0x0100);
            if (instruction == Cpu6502ByteCodesInstructions.ASL_ACCUMULATOR)
                A.Value = b;
            else
            {
                _ = addr ?? throw new NullReferenceException();
                _memoryManager.WriteAt(addr.Value, b);
            }
            PS = PS with { C = (res >> 8 & 1) == 1, Z = b == 0, V = (b >> 7 & 1) == 1 };
        }
        /// <summary>
        /// Logical Shift Right
        /// </summary>
        /// <param name="instruction">Byte code instruction</param>
        void LSR(byte instruction)
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
                    Cpu6502ByteCodesInstructions.LSR_ZEROPAGE => ReadZeroPageAddrFromMemory(),
                    Cpu6502ByteCodesInstructions.LSR_ZEROPAGE_X => ReadZeroPageAddrFromMemory(offset: X.Value),
                    Cpu6502ByteCodesInstructions.LSR_ABSOLUTE => ReadAddrFromMemory(),
                    Cpu6502ByteCodesInstructions.LSR_ABSOLUTE_X => ReadAddrFromMemory(offset: X.Value),
                    _ => throw new NotImplementedException()
                };
                b = _memoryManager.ReadAt(addr.Value);
            }

            PS = PS with { C = (b & 1) == 1 };
            res = b >> 1;
            b = (byte)(res % 0x0100);
            if (instruction == Cpu6502ByteCodesInstructions.LSR_ACCUMULATOR)
                A.Value = b;
            else
            {
                _ = addr ?? throw new NullReferenceException();
                _memoryManager.WriteAt(addr.Value, b);
            }
            PS = PS with { Z = b == 0, V = (b >> 7 & 1) == 1 };
        }
        /// <summary>
        /// Rotate Left
        /// </summary>
        /// <param name="instruction">Byte code instruction</param>
        void ROL(byte instruction)
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
                    Cpu6502ByteCodesInstructions.ROL_ZEROPAGE => ReadZeroPageAddrFromMemory(),
                    Cpu6502ByteCodesInstructions.ROL_ZEROPAGE_X => ReadZeroPageAddrFromMemory(offset: X.Value),
                    Cpu6502ByteCodesInstructions.ROL_ABSOLUTE => ReadAddrFromMemory(),
                    Cpu6502ByteCodesInstructions.ROL_ABSOLUTE_X => ReadAddrFromMemory(offset: X.Value),
                    _ => throw new NotImplementedException()
                };
                b = _memoryManager.ReadAt(addr.Value);
            }

            res = (b << 1) + (PS.C ? 1 : 0);
            b = (byte)(res % 0x0100);
            if (instruction == Cpu6502ByteCodesInstructions.ROL_ACCUMULATOR)
                A.Value = b;
            else
            {
                _ = addr ?? throw new NullReferenceException();
                _memoryManager.WriteAt(addr.Value, b);
            }
            PS = PS with { C = (res >> 8 & 1) == 1, Z = b == 0, V = (b >> 7 & 1) == 1 };
        }
        /// <summary>
        /// Rotate Right
        /// </summary>
        /// <param name="instruction">Byte code instruction</param>
        void ROR(byte instruction)
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
                    Cpu6502ByteCodesInstructions.ROR_ZEROPAGE => ReadZeroPageAddrFromMemory(),
                    Cpu6502ByteCodesInstructions.ROR_ZEROPAGE_X => ReadZeroPageAddrFromMemory(offset: X.Value),
                    Cpu6502ByteCodesInstructions.ROR_ABSOLUTE => ReadAddrFromMemory(),
                    Cpu6502ByteCodesInstructions.ROR_ABSOLUTE_X => ReadAddrFromMemory(offset: X.Value),
                    _ => throw new NotImplementedException()
                };
                b = _memoryManager.ReadAt(addr.Value);
            }

            PS = PS with { C = (b & 1) == 1 };
            res = (b >> 1) + (PS.C ? 1 << 7 : 0);
            b = (byte)(res & 0xFF);
            if (instruction == Cpu6502ByteCodesInstructions.ROR_ACCUMULATOR)
                A.Value = b;
            else
            {
                _ = addr ?? throw new NullReferenceException();
                _memoryManager.WriteAt(addr.Value, b);
            }
            PS = PS with { Z = b == 0, V = (b >> 7 & 1) == 1 };
        }
        #endregion

        #region Jumps & Calls
        /// <summary>
        /// Jump to Another Location
        /// </summary>
        /// <param name="instruction">Byte code instruction</param>
        void JMP(byte instruction)
        {
            ushort addr = instruction switch
            {
                Cpu6502ByteCodesInstructions.JMP_ABSOLUTE => ReadAddrFromMemory(),
                Cpu6502ByteCodesInstructions.JMP_INDIRECT => _memoryManager.ReadAt(ReadAddrFromMemory()),
                _ => throw new NotImplementedException()
            };
            PC = addr;
        }
        /// <summary>
        /// Jump to a Subroutine
        /// </summary>
        void JSR()
        {
            ushort addr = ReadAddrFromMemory();

            var currentAddr = PC - 1;
            _memoryManager.WriteAt(0x100 | SP--, (byte)(currentAddr & 0xFF));
            currentAddr = (byte)(currentAddr >> 8);
            _memoryManager.WriteAt(0x100 | SP--, (byte)(currentAddr & 0xFF));

            PC = addr;
        }
        /// <summary>
        /// Return from Subroutine
        /// </summary>
        void RTS()
        {
            byte upper = _memoryManager.ReadAt(SP++);
            byte lower = _memoryManager.ReadAt(SP++);

            ushort addr = ComputeAddrFromUL(lower, upper, offset: 1);
            PC = addr;
        }
        #endregion

        #region Branches
        /// <summary>
        /// Branch if Carry Flag Clear
        /// </summary>
        void BCC()
        {
            ushort addr = ReadAddrFromMemory();
            if (PS.C == false)
                PC = addr;
        }
        /// <summary>
        /// Branch if Carry Flag Set
        /// </summary>
        void BCS()
        {
            ushort addr = ReadAddrFromMemory();
            if (PS.C == true)
                PC = addr;
        }
        /// <summary>
        /// Branch if Zero Flag Set
        /// </summary>
        void BEQ()
        {
            ushort addr = ReadAddrFromMemory();
            if (PS.Z == true)
                PC = addr;
        }
        /// <summary>
        /// Branch if Negative Flag Set
        /// </summary>
        void BMI()
        {
            ushort addr = ReadAddrFromMemory();
            if (PS.N == true)
                PC = addr;
        }
        /// <summary>
        /// Branch if Zero Flag Clear
        /// </summary>
        void BNE()
        {
            ushort addr = ReadAddrFromMemory();
            if (PS.Z == false)
                PC = addr;
        }
        /// <summary>    
        /// Branch if Negative Flag Clear
        /// </summary>
        void BPL()
        {
            ushort addr = ReadAddrFromMemory();
            if (PS.N == false)
                PC = addr;
        }
        /// <summary>
        /// Branch if Overflow Flag Clear
        /// </summary>
        void BVC()
        {
            ushort addr = ReadAddrFromMemory();
            if (PS.V == false)
                PC = addr;
        }
        /// <summary>
        /// Branch if Overflow Flag Set
        /// </summary>
        void BVS()
        {
            ushort addr = ReadAddrFromMemory();
            if (PS.V == true)
                PC = addr;
        }
        #endregion

        #region Status Flag Changes
        /// <summary>
        /// Clear Carry Flag
        /// </summary>
        void CLC() => PS = PS with { C = false };
        /// <summary>
        /// Clear Decimal Mode Flag
        /// </summary>
        void CLD() => PS = PS with { D = false };
        /// <summary>
        /// Clear Interrupt Disable Flag
        /// </summary>
        void CLI() => PS = PS with { I = false };
        /// <summary>
        /// Clear Overflow Flag
        /// </summary>
        void CLV() => PS = PS with { V = false };
        /// <summary>
        /// Set Carry Flag
        /// </summary>
        void SEC() => PS = PS with { C = true };
        /// <summary>
        /// Set Decimal Mode Flag
        /// </summary>
        void SED() => PS = PS with { D = true };
        /// <summary>
        /// Set Interrupt Disable Flag
        /// </summary>
        void SEI() => PS = PS with { I = true };
        #endregion

        #region System Functions
        /// <summary>
        /// Force an Interrupt
        /// </summary>
        void BRK()
        {
            PHP();

            var currentAddr = PC;
            _memoryManager.WriteAt(0x100 | SP--, (byte)(currentAddr & 0xFF));
            currentAddr = (byte)(currentAddr >> 8);
            _memoryManager.WriteAt(0x100 | SP--, (byte)(currentAddr & 0xFF));

            ushort brkAddr = GetBrkAddress();
            PC = brkAddr;
            PS = PS with { B = true };
        }

        /// <summary>
        /// No Operation
        /// </summary>
        void NOP()
        { }
        /// <summary>
        /// Return from Interrupt
        /// </summary>
        void RTI()
        {
            byte upper = _memoryManager.ReadAt(SP++);
            byte lower = _memoryManager.ReadAt(SP++);

            ushort addr = ComputeAddrFromUL(lower, upper, offset: 1);
            PC = addr;

            PLP();
        }
        #endregion

        ushort ReadZeroPageAddrFromMemory(byte offset = 0) => ComputeAddrFromUL(_memoryManager.ReadAt(PC++), 0, offset);
        ushort ReadAddrFromMemory(byte offset = 0) => ComputeAddrFromUL(_memoryManager.ReadAt(PC++), _memoryManager.ReadAt(PC++), offset);

        static ushort ComputeAddrFromUL(byte lower, byte upper, byte offset = 0) => (ushort)(((upper << 8) | lower) + offset);
        ushort GetNmiAddress() => ComputeAddrFromUL(_memoryManager.ReadAt(Cpu6502Consts.NMI_ADDRESS_L), _memoryManager.ReadAt(Cpu6502Consts.NMI_ADDRESS_U));
        ushort GetResetAddress() => ComputeAddrFromUL(_memoryManager.ReadAt(Cpu6502Consts.RESET_ADDRESS_L), _memoryManager.ReadAt(Cpu6502Consts.RESET_ADDRESS_U));
        ushort GetBrkAddress() => ComputeAddrFromUL(_memoryManager.ReadAt(Cpu6502Consts.BRK_ADDRESS_L), _memoryManager.ReadAt(Cpu6502Consts.BRK_ADDRESS_U));
    }

    public record ProcessorStatus
    {
        /// <summary>
        /// Carry Flag
        /// </summary>
        public bool C { get; init; }
        /// <summary>
        /// Zero Flag
        /// </summary>
        public bool Z { get; init; }
        /// <summary>
        /// Interrupt Disable
        /// </summary>
        public bool I { get; init; }
        /// <summary>
        /// Decimal Mode Flag
        /// </summary>
        public bool D { get; init; }
        /// <summary>
        /// Break Command
        /// </summary>
        public bool B { get; init; }
        /// <summary>
        /// Overflow Flag
        /// </summary>
        public bool V { get; init; }
        /// <summary>
        /// Negative Flag
        /// </summary>
        public bool N { get; init; }

        public static implicit operator byte(ProcessorStatus ps) => (byte)((ps.C ? 64 : 0) + (ps.Z ? 32 : 0) + (ps.I ? 16 : 0) + (ps.D ? 8 : 0) + (ps.B ? 4 : 0) + (ps.V ? 2 : 0) + (ps.N ? 1 : 0));
        public static implicit operator ProcessorStatus(byte b) => new()
        {
            N = (b >> 0 & 1) == 1,
            V = (b >> 1 & 1) == 1,
            B = (b >> 2 & 1) == 1,
            D = (b >> 3 & 1) == 1,
            I = (b >> 4 & 1) == 1,
            Z = (b >> 5 & 1) == 1,
            C = (b >> 6 & 1) == 1,
        };
    }

    [DebuggerDisplay("{Value}, {HexValue, nq}, {BinValue, nq}, {CharValue}")]
    public class Register
    {
        readonly WeakReference<Cpu6502> _cpu;
        Cpu6502 CPU => _cpu.TryGetTarget(out var cpu) ? cpu : throw new ArgumentNullException();
        public byte Value { get; internal set; }
        private string HexValue => "0x" + Value.ToString("X2");
        private string BinValue => "0b" + Convert.ToString(Value, 2).PadLeft(8, '0');
        private char CharValue => (char)Value;
        
        public Register(Cpu6502 cpu)
        {
            _cpu = new(cpu);
            Value = 0;
        }

        public void LoadValueFromAddress(ushort address)
        {
            Value = CPU._memoryManager.ReadAt(address);
            CPU.PS = CPU.PS with { Z = Value == 0, N = (Value >> 7 & 1) == 1 };
        }
        public void WriteValueToAddress(ushort address) => CPU._memoryManager.WriteAt(address, Value);
        public void TransferValueToRegister(Register r)
        {
            r.Value = Value;
            CPU.PS = CPU.PS with { Z = r.Value == 0, N = (r.Value >> 7 & 1) == 1 };
        }
        public void Increment()
        {
            Value++;
            CPU.PS = CPU.PS with { Z = Value == 0, N = (Value >> 7 & 1) == 1 };
        }
        public void Decrement()
        {
            Value--;
            CPU.PS = CPU.PS with { Z = Value == 0, N = (Value >> 7 & 1) == 1 };
        }
    }
}
