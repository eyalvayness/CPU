using System;
using System.Diagnostics;
using System.Linq;

namespace CPU
{
    public class Cpu6502
    {
        public static class Consts
        {
            public const ushort NMI_ADDRESS_L   = '\uFFFA';
            public const ushort NMI_ADDRESS_U   = '\uFFFB';
            public const ushort RESET_ADDRESS_L = '\uFFFC';
            public const ushort RESET_ADDRESS_U = '\uFFFD';
            public const ushort BRK_ADDRESS_L   = '\uFFFE';
            public const ushort BRK_ADDRESS_U   = '\uFFFF';
        }

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
                case Cpu6502ByteCodesIntructions.LDA_IMMEDIATE:
                case Cpu6502ByteCodesIntructions.LDA_ZEROPAGE:
                case Cpu6502ByteCodesIntructions.LDA_ZEROPAGE_X:
                case Cpu6502ByteCodesIntructions.LDA_ABSOLUTE:
                case Cpu6502ByteCodesIntructions.LDA_ABSOLUTE_X:
                case Cpu6502ByteCodesIntructions.LDA_ABSOLUTE_Y:
                case Cpu6502ByteCodesIntructions.LDA_INDERECT_X:
                case Cpu6502ByteCodesIntructions.LDA_INDERECT_Y:
                    LDA(instruction);
                    break;
                case Cpu6502ByteCodesIntructions.LDX_IMMEDIATE:
                case Cpu6502ByteCodesIntructions.LDX_ZEROPAGE:
                case Cpu6502ByteCodesIntructions.LDX_ZEROPAGE_Y:
                case Cpu6502ByteCodesIntructions.LDX_ABSOLUTE:
                case Cpu6502ByteCodesIntructions.LDX_ABSOLUTE_Y:
                    LDX(instruction);           
                    break;
                case Cpu6502ByteCodesIntructions.LDY_IMMEDIATE:
                case Cpu6502ByteCodesIntructions.LDY_ZEROPAGE:
                case Cpu6502ByteCodesIntructions.LDY_ZEROPAGE_X:
                case Cpu6502ByteCodesIntructions.LDY_ABSOLUTE:
                case Cpu6502ByteCodesIntructions.LDY_ABSOLUTE_X:
                    LDY(instruction);
                    break;
                case Cpu6502ByteCodesIntructions.STA_ZEROPAGE:
                case Cpu6502ByteCodesIntructions.STA_ZEROPAGE_X:
                case Cpu6502ByteCodesIntructions.STA_ABSOLUTE:
                case Cpu6502ByteCodesIntructions.STA_ABSOLUTE_X:
                case Cpu6502ByteCodesIntructions.STA_ABSOLUTE_Y:
                case Cpu6502ByteCodesIntructions.STA_INDERECT_X:
                case Cpu6502ByteCodesIntructions.STA_INDERECT_Y:
                    STA(instruction);
                    break;
                case Cpu6502ByteCodesIntructions.STX_ZEROPAGE:
                case Cpu6502ByteCodesIntructions.STX_ZEROPAGE_Y:
                case Cpu6502ByteCodesIntructions.STX_ABSOLUTE:
                    STX(instruction);
                    break;
                case Cpu6502ByteCodesIntructions.STY_ZEROPAGE:
                case Cpu6502ByteCodesIntructions.STY_ZEROPAGE_X:
                case Cpu6502ByteCodesIntructions.STY_ABSOLUTE:
                    STY(instruction);
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
                Cpu6502ByteCodesIntructions.LDA_IMMEDIATE => PC++,
                Cpu6502ByteCodesIntructions.LDA_ZEROPAGE => ReadZeroPageAddrFromMemory(),
                Cpu6502ByteCodesIntructions.LDA_ZEROPAGE_X => ReadZeroPageAddrFromMemory(offset: X.Value),
                Cpu6502ByteCodesIntructions.LDA_ABSOLUTE => ReadAddrFromMemory(),
                Cpu6502ByteCodesIntructions.LDA_ABSOLUTE_X => ReadAddrFromMemory(offset: X.Value),
                Cpu6502ByteCodesIntructions.LDA_ABSOLUTE_Y => ReadAddrFromMemory(offset: Y.Value),
                _ => throw new NotImplementedException($"Instruction not implemented: {instruction:X2} ({instruction})")
            };
            A.LoadValueFromAddress(addr);
        }
        void LDX(byte instruction) // Load X Register
        {
            ushort addr = instruction switch
            {
                Cpu6502ByteCodesIntructions.LDX_IMMEDIATE => PC++,
                Cpu6502ByteCodesIntructions.LDX_ZEROPAGE => ReadZeroPageAddrFromMemory(),
                Cpu6502ByteCodesIntructions.LDX_ZEROPAGE_Y => ReadZeroPageAddrFromMemory(offset: Y.Value),
                Cpu6502ByteCodesIntructions.LDX_ABSOLUTE => ReadAddrFromMemory(),
                Cpu6502ByteCodesIntructions.LDX_ABSOLUTE_Y => ReadAddrFromMemory(offset: Y.Value),
                _ => throw new NotImplementedException($"Instruction not implemented: {instruction:X2} ({instruction})")
            };
            X.LoadValueFromAddress(addr);
        }
        void LDY(byte instruction) // Load Y Register
        {
            ushort addr = instruction switch
            {
                Cpu6502ByteCodesIntructions.LDY_IMMEDIATE => PC++,
                Cpu6502ByteCodesIntructions.LDY_ZEROPAGE => ReadZeroPageAddrFromMemory(),
                Cpu6502ByteCodesIntructions.LDY_ZEROPAGE_X => ReadZeroPageAddrFromMemory(offset: X.Value),
                Cpu6502ByteCodesIntructions.LDY_ABSOLUTE => ReadAddrFromMemory(),
                Cpu6502ByteCodesIntructions.LDY_ABSOLUTE_X => ReadAddrFromMemory(offset: X.Value),
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
                Cpu6502ByteCodesIntructions.STA_ZEROPAGE => ReadZeroPageAddrFromMemory(),
                Cpu6502ByteCodesIntructions.STA_ZEROPAGE_X => ReadZeroPageAddrFromMemory(offset: X.Value),
                Cpu6502ByteCodesIntructions.STA_ABSOLUTE => ReadAddrFromMemory(),
                Cpu6502ByteCodesIntructions.STA_ABSOLUTE_X => ReadAddrFromMemory(offset: X.Value),
                Cpu6502ByteCodesIntructions.STA_ABSOLUTE_Y => ReadAddrFromMemory(offset: Y.Value),
                _ => throw new NotImplementedException($"Instruction not implemented: {instruction:X2} ({instruction})")
            };
            A.WriteValueToAddress(addr);
        }
        void STX(byte instruction) // Store X Register
        {
            ushort addr = instruction switch
            {
                Cpu6502ByteCodesIntructions.STX_ZEROPAGE => ReadZeroPageAddrFromMemory(),
                Cpu6502ByteCodesIntructions.STX_ZEROPAGE_Y => ReadZeroPageAddrFromMemory(offset: Y.Value),
                Cpu6502ByteCodesIntructions.STX_ABSOLUTE => ReadAddrFromMemory(),
                _ => throw new NotImplementedException($"Instruction not implemented: {instruction:X2} ({instruction})")
            };
            X.WriteValueToAddress(addr);
        }
        void STY(byte instruction) // Store Y Register
        {
            ushort addr = instruction switch
            {
                Cpu6502ByteCodesIntructions.STY_ZEROPAGE => ReadZeroPageAddrFromMemory(),
                Cpu6502ByteCodesIntructions.STY_ZEROPAGE_X => ReadZeroPageAddrFromMemory(offset: X.Value),
                Cpu6502ByteCodesIntructions.STY_ABSOLUTE => ReadAddrFromMemory(),
                _ => throw new NotImplementedException($"Instruction not implemented: {instruction:X2} ({instruction})")
            };
            Y.WriteValueToAddress(addr);
        }
        #endregion

        #region Register Transfers
        void TAX() // Transfer Accumulator To X
        { }
        void TAY() // Transfer Accumulator To Y
        { }
        void TXA() // Transfer X To Accumulator
        { }
        void TYA() // Transfer Y To Accumulator
        { }
        #endregion

        #region Stack Operations
        void TSX() // Transfer Stack Pointer to X
        { }
        void TXS() // Transfer X to Stack Pointer
        { }
        void PHA() // Push Accumulator on Stack
        { }
        void PHP() // Push Processor Status on Stack
        { }
        void PLA() // Pull Accumulator from Stack
        { }
        void PLP()  // Pull Processor Status from Stack
        { }
        #endregion

        #region Logical
        void AND() // Logical AND
        { } 
        void EOR() // Exclusive OR
        { } 
        void ORA() // Logical Inclusive OR
        { } 
        void BIT() // Bit Test
        { } 
        #endregion

        #region Arithmetic
        void ADC() // Add with Carry
        { }
        void SBC() // Substract with Carry
        { }
        void CMP() // Compare Accumulator
        { }
        void CPX() // Compare X Register
        { }
        void CPY() // Compare Y Register
        { }
        #endregion

        #region Increments & Decrements
        void INC() // Increment a Memory Location
        { }
        void INX() // Increment the X Register
        { }
        void INY() // Increment the Y Register
        { }
        void DEC() // Decrement a Memory Location
        { }
        void DEX() // Decrement the X Register
        { }
        void DEY() // Decrement the Y Register
        { }
        #endregion

        #region Shifts
        void ASL() // Arithmetic Shift Left
        { }
        void LSR() // Logical Shift Right
        { } 
        void ROL() // Rotate Left
        { } 
        void ROR() // Rotate Right
        { } 
        #endregion

        #region Jumps & Calls
        void JMP() // Jump to Another Location
        { }
        void JSR() // Jump to a Subroutine
        { }
        void RTS() // Return from Subroutine
        { }
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
        void CLC() // Clear Carry Flag
        { }
        void CLD() // Clear Decimal Mode Flag
        { }
        void CLI() // Clear Interrupt Disable Flag
        { }
        void CLV() // Clear Overflow Flag
        { }
        void SEC()  // Set Carry Flag
        { }
        void SED() // Set Decimal Mode Flag
        { }
        void SEI()  // Set Interrupt Disable Flag
        { }
        #endregion

        #region System Functions
        void BRK() // Force an Interrupt
        {
        } 
        void NOP() // No Operation
        {
        } 
        void RTI() // Return from Interrupt
        {
        } 
        #endregion

        ushort ReadZeroPageAddrFromMemory(byte offset = 0) => ComputeAddrFromUL(_memory[PC++], 0, offset);
        ushort ReadAddrFromMemory(byte offset = 0) => ComputeAddrFromUL(_memory[PC++], _memory[PC++], offset);

        ushort ComputeAddrFromUL(byte lower, byte upper, byte offset = 0) => (ushort)(upper * (byte.MaxValue + 1) + lower + offset);
        ushort GetNmiAddress() => ComputeAddrFromUL(_memory[Consts.NMI_ADDRESS_L], _memory[Consts.NMI_ADDRESS_U]);
        ushort GetResetAddress() => ComputeAddrFromUL(_memory[Consts.RESET_ADDRESS_L], _memory[Consts.RESET_ADDRESS_U]);
        ushort GetBrkAddress() => ComputeAddrFromUL(_memory[Consts.BRK_ADDRESS_L], _memory[Consts.BRK_ADDRESS_U]);
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
    }

    [DebuggerDisplay("{Value}, {HexValue, nq}, {BinValue, nq}")]
    public class Register
    {
        readonly WeakReference<Cpu6502> _cpu;
        Cpu6502 Cpu => _cpu.TryGetTarget(out var cpu) ? cpu : throw new ArgumentNullException();
        public byte Value { get; internal set; }
        public string HexValue => "0x" + Value.ToString("X2");
        public string BinValue => "0b" + Convert.ToString(Value, 2).PadLeft(8, '0');

        public Register(Cpu6502 cpu)
        {
            _cpu = new(cpu);
            Value = 0;
        }

        public void LoadValueFromAddress(ushort address)
        {
            Value = Cpu._memory[address];
            Cpu.PS = Cpu.PS with { Z = Value == 0, N = ((Value & (1 << 7)) >> 7) == 1 };
        }
        public void WriteValueToAddress(ushort address) => Cpu._memory[address] = Value;
    }
}
