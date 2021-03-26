using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU
{
    public static class Cpu6502Consts
    {
        public const ushort NMI_ADDRESS_L = 0xFFFA;
        public const ushort NMI_ADDRESS_U = 0xFFFB;
        public const ushort RESET_ADDRESS_L = 0xFFFC;
        public const ushort RESET_ADDRESS_U = 0xFFFD;
        public const ushort BRK_ADDRESS_L = 0xFFFE;
        public const ushort BRK_ADDRESS_U = 0xFFFF;
    }

    public static class Cpu6502ByteCodesInstructions
    {

        #region Load Operations
        /// <summary>
        /// Load Accumulator
        /// </summary>
        public const byte LDA_IMMEDIATE = 0xA9;
        /// <summary>
        /// Load Accumulator
        /// </summary>
        public const byte LDA_ZEROPAGE = 0xA5;
        /// <summary>
        /// Load Accumulator
        /// </summary>
        public const byte LDA_ZEROPAGE_X = 0xB5;
        /// <summary>
        /// Load Accumulator
        /// </summary>
        public const byte LDA_ABSOLUTE = 0xAD;
        /// <summary>
        /// Load Accumulator
        /// </summary>
        public const byte LDA_ABSOLUTE_X = 0xBD;
        /// <summary>
        /// Load Accumulator
        /// </summary>
        public const byte LDA_ABSOLUTE_Y = 0xB9;
        /// <summary>
        /// Load Accumulator
        /// </summary>
        public const byte LDA_INDIRECT_X = 0xA1;
        /// <summary>
        /// Load Accumulator
        /// </summary>
        public const byte LDA_INDIRECT_Y = 0xB1;

        /// <summary>
        /// Load X Register
        /// </summary>
        public const byte LDX_IMMEDIATE = 0xA2;
        /// <summary>
        /// Load X Register
        /// </summary>
        public const byte LDX_ZEROPAGE = 0xA6;
        /// <summary>
        /// Load X Register
        /// </summary>
        public const byte LDX_ZEROPAGE_Y = 0xB6;
        /// <summary>
        /// Load X Register
        /// </summary>
        public const byte LDX_ABSOLUTE = 0xAE;
        /// <summary>
        /// Load X Register
        /// </summary>
        public const byte LDX_ABSOLUTE_Y = 0xBE;

        /// <summary>
        /// Load Y Register
        /// </summary>
        public const byte LDY_IMMEDIATE = 0xA0;
        /// <summary>
        /// Load Y Register
        /// </summary>
        public const byte LDY_ZEROPAGE = 0xA4;
        /// <summary>
        /// Load Y Register
        /// </summary>
        public const byte LDY_ZEROPAGE_X = 0xB4;
        /// <summary>
        /// Load Y Register
        /// </summary>
        public const byte LDY_ABSOLUTE = 0xAC;
        /// <summary>
        /// Load Y Register
        /// </summary>
        public const byte LDY_ABSOLUTE_X = 0xBC;
        #endregion

        #region Store Operations
        /// <summary>
        /// Store Accumulator
        /// </summary>
        public const byte STA_ZEROPAGE = 0x85;
        /// <summary>
        /// Store Accumulator
        /// </summary>
        public const byte STA_ZEROPAGE_X = 0x95;
        /// <summary>
        /// Store Accumulator
        /// </summary>
        public const byte STA_ABSOLUTE = 0x8D;
        /// <summary>
        /// Store Accumulator
        /// </summary>
        public const byte STA_ABSOLUTE_X = 0x9D;
        /// <summary>
        /// Store Accumulator
        /// </summary>
        public const byte STA_ABSOLUTE_Y = 0x99;
        /// <summary>
        /// Store Accumulator
        /// </summary>
        public const byte STA_INDIRECT_X = 0x81;
        /// <summary>
        /// Store Accumulator
        /// </summary>
        public const byte STA_INDIRECT_Y = 0x91;

        /// <summary>
        /// Store X Register
        /// </summary>
        public const byte STX_ZEROPAGE = 0x86;
        /// <summary>
        /// Store X Register
        /// </summary>
        public const byte STX_ZEROPAGE_Y = 0x96;
        /// <summary>
        /// Store X Register
        /// </summary>
        public const byte STX_ABSOLUTE = 0x8E;

        /// <summary>
        /// Store Y Register
        /// </summary>
        public const byte STY_ZEROPAGE = 0x84;
        /// <summary>
        /// Store Y Register
        /// </summary>
        public const byte STY_ZEROPAGE_X = 0x94;
        /// <summary>
        /// Store Y Register
        /// </summary>
        public const byte STY_ABSOLUTE = 0x8C;
        #endregion

        #region Register Transfers
        /// <summary>
        /// Transfer Accumulator To X
        /// </summary>
        public const byte TAX = 0xAA;
        /// <summary>
        /// Transfer Accumulator To Y
        /// </summary>
        public const byte TAY = 0xA8;
        /// <summary>
        /// Transfer X To Accumulator
        /// </summary>
        public const byte TXA = 0x8A;
        /// <summary>
        /// Transfer Y To Accumulator
        /// </summary>
        public const byte TYA = 0x98;
        #endregion

        #region Stack Operations        
        /// <summary>
        /// Transfer Stack Pointer to X
        /// </summary>
        public const byte TSX = 0xBA;
        /// <summary>
        /// Transfer X to Stack Pointer
        /// </summary>
        public const byte TXS = 0x9A;
        /// <summary>
        /// Push Accumulator on Stack
        /// </summary>
        public const byte PHA = 0x48;
        /// <summary>
        /// Push Processor Status on Stack
        /// </summary>
        public const byte PHP = 0x08;
        /// <summary>
        /// Pull Accumulator from Stack
        /// </summary>
        public const byte PLA = 0x68;
        /// <summary>
        /// Pull Processor Status from Stack
        /// </summary>
        public const byte PLP = 0x28;
        #endregion

        #region Logical
        /// <summary>
        /// Logical AND
        /// </summary>
        public const byte AND_IMMEDIATE = 0x29;
        /// <summary>
        /// Logical AND
        /// </summary>
        public const byte AND_ZEROPAGE = 0x25;
        /// <summary>
        /// Logical AND
        /// </summary>
        public const byte AND_ZEROPAGE_X = 0x35;
        /// <summary>
        /// Logical AND
        /// </summary>
        public const byte AND_ABSOLUTE = 0x2D;
        /// <summary>
        /// Logical AND
        /// </summary>
        public const byte AND_ABSOLUTE_X = 0x3D;
        /// <summary>
        /// Logical AND
        /// </summary>
        public const byte AND_ABSOLUTE_Y = 0x39;
        /// <summary>
        /// Logical AND
        /// </summary>
        public const byte AND_INDIRECT_X = 0x21;
        /// <summary>
        /// Logical AND
        /// </summary>
        public const byte AND_INDIRECT_Y = 0x31;

        /// <summary>
        /// Exclusive OR
        /// </summary>
        public const byte EOR_IMMEDIATE = 0x49;
        /// <summary>
        /// Exclusive OR
        /// </summary>
        public const byte EOR_ZEROPAGE = 0x45;
        /// <summary>
        /// Exclusive OR
        /// </summary>
        public const byte EOR_ZEROPAGE_X = 0x55;
        /// <summary>
        /// Exclusive OR
        /// </summary>
        public const byte EOR_ABSOLUTE = 0x4D;
        /// <summary>
        /// Exclusive OR
        /// </summary>
        public const byte EOR_ABSOLUTE_X = 0x5D;
        /// <summary>
        /// Exclusive OR
        /// </summary>
        public const byte EOR_ABSOLUTE_Y = 0x59;
        /// <summary>
        /// Exclusive OR
        /// </summary>
        public const byte EOR_INDIRECT_X = 0x41;
        /// <summary>
        /// Exclusive OR
        /// </summary>
        public const byte EOR_INDIRECT_Y = 0x51;

        /// <summary>
        /// Logical Inclusive OR
        /// </summary>
        public const byte ORA_IMMEDIATE = 0x09;
        /// <summary>
        /// Logical Inclusive OR
        /// </summary>
        public const byte ORA_ZEROPAGE = 0x05;
        /// <summary>
        /// Logical Inclusive OR
        /// </summary>
        public const byte ORA_ZEROPAGE_X = 0x15;
        /// <summary>
        /// Logical Inclusive OR
        /// </summary>
        public const byte ORA_ABSOLUTE = 0x0D;
        /// <summary>
        /// Logical Inclusive OR
        /// </summary>
        public const byte ORA_ABSOLUTE_X = 0x1D;
        /// <summary>
        /// Logical Inclusive OR
        /// </summary>
        public const byte ORA_ABSOLUTE_Y = 0x19;
        /// <summary>
        /// Logical Inclusive OR
        /// </summary>
        public const byte ORA_INDIRECT_X = 0x01;
        /// <summary>
        /// Logical Inclusive OR
        /// </summary>
        public const byte ORA_INDIRECT_Y = 0x11;

        /// <summary>
        /// Bit Test
        /// </summary>
        public const byte BIT_ZEROPAGE = 0x24;
        /// <summary>
        /// Bit Test
        /// </summary>
        public const byte BIT_ABSOLUTE = 0x2C;
        #endregion

        #region Arithmetic
        /// <summary>
        /// Add with Carry
        /// </summary>
        public const byte ADC_IMMEDIATE = 0x69;
        /// <summary>
        /// Add with Carry
        /// </summary>
        public const byte ADC_ZEROPAGE = 0x65;
        /// <summary>
        /// Add with Carry
        /// </summary>
        public const byte ADC_ZEROPAGE_X = 0x75;
        /// <summary>
        /// Add with Carry
        /// </summary>
        public const byte ADC_ABSOLUTE = 0x6D;
        /// <summary>
        /// Add with Carry
        /// </summary>
        public const byte ADC_ABSOLUTE_X = 0x7D;
        /// <summary>
        /// Add with Carry
        /// </summary>
        public const byte ADC_ABSOLUTE_Y = 0x79;
        /// <summary>
        /// Add with Carry
        /// </summary>
        public const byte ADC_INDIRECT_X = 0x61;
        /// <summary>
        /// Add with Carry
        /// </summary>
        public const byte ADC_INDIRECT_Y = 0x71;

        /// <summary>
        /// Substract with Carry
        /// </summary>
        public const byte SBC_IMMEDIATE = 0xE9;
        /// <summary>
        /// Substract with Carry
        /// </summary>
        public const byte SBC_ZEROPAGE = 0xE5;
        /// <summary>
        /// Substract with Carry
        /// </summary>
        public const byte SBC_ZEROPAGE_X = 0xF5;
        /// <summary>
        /// Substract with Carry
        /// </summary>
        public const byte SBC_ABSOLUTE = 0xED;
        /// <summary>
        /// Substract with Carry
        /// </summary>
        public const byte SBC_ABSOLUTE_X = 0xFD;
        /// <summary>
        /// Substract with Carry
        /// </summary>
        public const byte SBC_ABSOLUTE_Y = 0xF9;
        /// <summary>
        /// Substract with Carry
        /// </summary>
        public const byte SBC_INDIRECT_X = 0xE1;
        /// <summary>
        /// Substract with Carry
        /// </summary>
        public const byte SBC_INDIRECT_Y = 0xF1;

        /// <summary>
        /// Compare Accumulator
        /// </summary>
        public const byte CMP_IMMEDIATE = 0xC9;
        /// <summary>
        /// Compare Accumulator
        /// </summary>
        public const byte CMP_ZEROPAGE = 0xC5;
        /// <summary>
        /// Compare Accumulator
        /// </summary>
        public const byte CMP_ZEROPAGE_X = 0xD5;
        /// <summary>
        /// Compare Accumulator
        /// </summary>
        public const byte CMP_ABSOLUTE = 0xCD;
        /// <summary>
        /// Compare Accumulator
        /// </summary>
        public const byte CMP_ABSOLUTE_X = 0xDD;
        /// <summary>
        /// Compare Accumulator
        /// </summary>
        public const byte CMP_ABSOLUTE_Y = 0xD9;
        /// <summary>
        /// Compare Accumulator
        /// </summary>
        public const byte CMP_INDIRECT_X = 0xC1;
        /// <summary>
        /// Compare Accumulator
        /// </summary>
        public const byte CMP_INDIRECT_Y = 0xD1;

        /// <summary>
        /// Compare X Register
        /// </summary>
        public const byte CPX_IMMEDIATE = 0xE0;
        /// <summary>
        /// Compare X Register
        /// </summary>
        public const byte CPX_ZEROPAGE = 0xE4;
        /// <summary>
        /// Compare X Register
        /// </summary>
        public const byte CPX_ABSOLUTE = 0xEC;

        /// <summary>
        /// Compare Y Register
        /// </summary>
        public const byte CPY_IMMEDIATE = 0xC0;
        /// <summary>
        /// Compare Y Register
        /// </summary>
        public const byte CPY_ZEROPAGE = 0xC4;
        /// <summary>
        /// Compare Y Register
        /// </summary>
        public const byte CPY_ABSOLUTE = 0xCC;
        #endregion

        #region Increments & Decrements
        /// <summary>
        /// Increment a Memory Location
        /// </summary>
        public const byte INC_ZEROPAGE = 0xE6;
        /// <summary>
        /// Increment a Memory Location
        /// </summary>
        public const byte INC_ZEROPAGE_X = 0xF6;
        /// <summary>
        /// Increment a Memory Location
        /// </summary>
        public const byte INC_ABSOLUTE = 0xEE;
        /// <summary>
        /// Increment a Memory Location
        /// </summary>
        public const byte INC_ABSOLUTE_X = 0xFE;

        /// <summary>
        /// Increment the X Register
        /// </summary>
        public const byte INX = 0xE8;
        /// <summary>
        /// Increment the Y Register
        /// </summary>
        public const byte INY = 0xC8;

        /// <summary>
        /// Decrement a Memory Location
        /// </summary>
        public const byte DEC_ZEROPAGE = 0xC6;
        /// <summary>
        /// Decrement a Memory Location
        /// </summary>
        public const byte DEC_ZEROPAGE_X = 0xD6;
        /// <summary>
        /// Decrement a Memory Location
        /// </summary>
        public const byte DEC_ABSOLUTE = 0xCE;
        /// <summary>
        /// Decrement a Memory Location
        /// </summary>
        public const byte DEC_ABSOLUTE_X = 0xDE;

        /// <summary>
        /// Decrement the X Register
        /// </summary>
        public const byte DEX = 0xCA;
        /// <summary>
        /// Decrement the Y Register
        /// </summary>
        public const byte DEY = 0x88;
        #endregion

        #region Shifts        
        /// <summary>
        /// Arithmetic Shift Left
        /// </summary>
        public const byte ASL_ACCUMULATOR = 0x0A;
        /// <summary>
        /// Arithmetic Shift Left
        /// </summary>
        public const byte ASL_ZEROPAGE = 0x06;
        /// <summary>
        /// Arithmetic Shift Left
        /// </summary>
        public const byte ASL_ZEROPAGE_X = 0x16;
        /// <summary>
        /// Arithmetic Shift Left
        /// </summary>
        public const byte ASL_ABSOLUTE = 0x0E;
        /// <summary>
        /// Arithmetic Shift Left
        /// </summary>
        public const byte ASL_ABSOLUTE_X = 0x1E;

        /// <summary>
        /// Logical Shift Right
        /// </summary>
        public const byte LSR_ACCUMULATOR = 0x4A;
        /// <summary>
        /// Logical Shift Right
        /// </summary>
        public const byte LSR_ZEROPAGE = 0x46;
        /// <summary>
        /// Logical Shift Right
        /// </summary>
        public const byte LSR_ZEROPAGE_X = 0x56;
        /// <summary>
        /// Logical Shift Right
        /// </summary>
        public const byte LSR_ABSOLUTE = 0x4E;
        /// <summary>
        /// Logical Shift Right
        /// </summary>
        public const byte LSR_ABSOLUTE_X = 0x5E;

        /// <summary>
        /// Rotate Left
        /// </summary>
        public const byte ROL_ACCUMULATOR = 0x2A;
        /// <summary>
        /// Rotate Left
        /// </summary>
        public const byte ROL_ZEROPAGE = 0x26;
        /// <summary>
        /// Rotate Left
        /// </summary>
        public const byte ROL_ZEROPAGE_X = 0x36;
        /// <summary>
        /// Rotate Left
        /// </summary>
        public const byte ROL_ABSOLUTE = 0x2E;
        /// <summary>
        /// Rotate Left
        /// </summary>
        public const byte ROL_ABSOLUTE_X = 0x3E;

        /// <summary>
        /// Rotate Right
        /// </summary>
        public const byte ROR_ACCUMULATOR = 0x6A;
        /// <summary>
        /// Rotate Right
        /// </summary>
        public const byte ROR_ZEROPAGE = 0x66;
        /// <summary>
        /// Rotate Right
        /// </summary>
        public const byte ROR_ZEROPAGE_X = 0x76;
        /// <summary>
        /// Rotate Right
        /// </summary>
        public const byte ROR_ABSOLUTE = 0x6E;
        /// <summary>
        /// Rotate Right
        /// </summary>
        public const byte ROR_ABSOLUTE_X = 0x7E;
        #endregion

        #region Jumps & Calls        
        /// <summary>
        /// Jump to Another Location
        /// </summary>
        public const byte JMP_ABSOLUTE = 0x4C;
        /// <summary>
        /// Jump to Another Location
        /// </summary>
        public const byte JMP_INDIRECT = 0x6C;

        /// <summary>
        /// Jump to a Subroutine
        /// </summary>
        public const byte JSR_ABSOLUTE = 0x20;
        /// <summary>
        /// Return from Subroutine
        /// </summary>
        public const byte RTS = 0x60;
        #endregion

        #region Branches
        /// <summary>
        /// Branch if Carry Flag Clear
        /// </summary>
        public const byte BCC_ABSOLUTE = 0x90;
        /// <summary>
        /// Branch if Carry Flag Set
        /// </summary>
        public const byte BCS_ABSOLUTE = 0xB0;
        /// <summary>
        /// Branch if Zero Flag Set
        /// </summary>
        public const byte BEQ_ABSOLUTE = 0xF0;
        /// <summary>
        /// Branch if Negative Flag Set
        /// </summary>
        public const byte BMI_ABSOLUTE = 0x30;
        /// <summary>
        /// Branch if Zero Flag Clear
        /// </summary>
        public const byte BNE_ABSOLUTE = 0xD0;
        /// <summary>
        /// Branch if Negative Flag Clear
        /// </summary>
        public const byte BPL_ABSOLUTE = 0x10;
        /// <summary>
        /// Branch if Overflow Flag Clear
        public const byte BVC_ABSOLUTE = 0x50;
        /// </summary>
        /// <summary>
        /// Branch if Overflow Flag Set
        /// </summary>
        public const byte BVS_ABSOLUTE = 0x70;
        #endregion

        #region Status Flag Changes
        /// <summary>
        /// Clear Carry Flag
        /// </summary>
        public const byte CLC = 0x18;
        /// <summary>
        /// Clear Decimal Mode Flag
        /// </summary>
        public const byte CLD = 0xD8;
        /// <summary>
        /// Clear Interrupt Disable Flag
        /// </summary>
        public const byte CLI = 0x58;
        /// <summary>
        /// Clear Overflow Flag
        /// </summary>
        public const byte CLV = 0xB8;
        /// <summary>
        /// Set Carry Flag
        /// </summary>
        public const byte SEC = 0x38;
        /// <summary>
        /// Set Decimal Mode Flag
        /// </summary>
        public const byte SED = 0xF8;
        /// <summary>
        /// Set Interrupt Disable Flag
        /// </summary>
        public const byte SEI = 0x78;
        #endregion

        #region System Functions        
        /// <summary>
        /// Force an Interrupt
        /// </summary>
        public const byte BRK = 0x00;
        /// <summary>
        /// No Operation
        /// </summary>
        public const byte NOP = 0xEA;
        /// <summary>
        /// Return from Interrupt
        /// </summary>
        public const byte RTI = 0x40;
        #endregion
    }

    public static class Cpu6502StringInstructions
    {
        #region Load Operations
        /// <summary>
        /// Load Accumulator
        /// </summary>
        public const string LDA = "lda";
        /// <summary>
        /// Load X Register
        /// </summary>
        public const string LDX = "ldx";
        /// <summary>
        /// Load Y Register
        /// </summary>
        public const string LDY = "ldy";
        #endregion

        #region Store Operations
        /// <summary>
        /// Store Accumulator
        /// </summary>
        public const string STA = "sta";
        /// <summary>
        /// Store X Register
        /// </summary>
        public const string STX = "stx";
        /// <summary>
        /// Store Y Register
        /// </summary>
        public const string STY = "sty";
        #endregion

        #region Register Transfers
        /// <summary>
        /// Transfer Accumulator To X
        /// </summary>
        public const string TAX = "tax";
        /// <summary>
        /// Transfer Accumulator To Y
        /// </summary>
        public const string TAY = "tay";
        /// <summary>
        /// Transfer X To Accumulator
        /// </summary>
        public const string TXA = "txa";
        /// <summary>
        /// Transfer Y To Accumulator
        /// </summary>
        public const string TYA = "tya";
        #endregion

        #region Stack Operations
        /// <summary>
        /// Transfer Stack Pointer to X
        /// </summary>
        public const string TSX = "tsx";
        /// <summary>
        /// Transfer X to Stack Pointer
        /// </summary>
        public const string TXS = "txs";
        /// <summary>
        /// Push Accumulator on Stack
        /// </summary>
        public const string PHA = "pha";
        /// <summary>
        /// Push Processor Status on Stack
        /// </summary>
        public const string PHP = "php";
        /// <summary>
        /// Pull Accumulator from Stack
        /// </summary>
        public const string PLA = "pla";
        /// <summary>
        /// Pull Processor Status from Stack
        /// </summary>
        public const string PLP = "plp";
        #endregion

        #region Logical
        /// <summary>
        /// Logical AND
        /// </summary>
        public const string AND = "and";
        /// <summary>
        /// Exclusive OR
        /// </summary>
        public const string EOR = "eor";
        /// <summary>
        /// Logical Inclusive OR
        /// </summary>
        public const string ORA = "ora";
        /// <summary>
        /// Bit Test
        /// </summary>
        public const string BIT = "bit";
        #endregion

        #region Arithmetic
        /// <summary>
        /// Add with Carry
        /// </summary>
        public const string ADC = "adc";
        /// <summary>
        /// Substract with Carry
        /// </summary>
        public const string SBC = "sbc";
        /// <summary>
        /// Compare Accumulator
        /// </summary>
        public const string CMP = "cmp";
        /// <summary>
        /// Compare X Register
        /// </summary>
        public const string CPX = "cpx";
        /// <summary>
        /// Compare Y Register
        /// </summary>
        public const string CPY = "cpy";
        #endregion

        #region Increments & Decrements
        /// <summary>
        /// Increment a Memory Location
        /// </summary>
        public const string INC = "inc";
        /// <summary>
        /// Increment the X Register
        /// </summary>
        public const string INX = "inx";
        /// <summary>
        /// Increment the Y Register
        /// </summary>
        public const string INY = "iny";
        /// <summary>
        /// Decrement a Memory Location
        /// </summary>
        public const string DEC = "dec";
        /// <summary>
        /// Decrement the X Register
        /// </summary>
        public const string DEX = "dex";
        /// <summary>
        /// Decrement the Y Register
        /// </summary>
        public const string DEY = "dey";
        #endregion

        #region Shifts
        /// <summary>
        /// Arithmetic Shift Left
        /// </summary>
        public const string ASL = "asl";
        /// <summary>
        /// Logical Shift Right
        /// </summary>
        public const string LSR = "lsr";
        /// <summary>
        /// Rotate Left
        /// </summary>
        public const string ROL = "rol";
        /// <summary>
        /// Rotate Right
        /// </summary>
        public const string ROR = "ror";
        #endregion

        #region Jumps & Calls
        /// <summary>
        /// Jump to Another Location
        /// </summary>
        public const string JMP = "jmp";
        /// <summary>
        /// Jump to a Subroutine
        /// </summary>
        public const string JSR = "jsr";
        /// <summary>
        /// Return from Subroutine
        /// </summary>
        public const string RTS = "rts";
        #endregion

        #region Branches
        /// <summary>
        /// Branch if Carry Flag Clear
        /// </summary>
        public const string BCC = "bcc";
        /// <summary>
        /// Branch if Carry Flag Set
        /// </summary>
        public const string BCS = "bcs";
        /// <summary>
        /// Branch if Zero Flag Set
        /// </summary>
        public const string BEQ = "beq";
        /// <summary>
        /// Branch if Negative Flag Set
        /// </summary>
        public const string BMI = "bmi";
        /// <summary>
        /// Branch if Zero Flag Clear
        /// </summary>
        public const string BNE = "bne";
        /// <summary>
        /// Branch if Negative Flag Clear
        /// </summary>
        public const string BPL = "bpl";
        /// <summary>
        /// Branch if Overflow Flag Clear
        /// </summary>
        public const string BVC = "bvc";
        /// <summary>
        /// Branch if Overflow Flag Set
        /// </summary>
        public const string BVS = "bvs";
        #endregion

        #region Status Flag Changes
        /// <summary>
        /// Clear Carry Flag
        /// </summary>
        public const string CLC = "clc";
        /// <summary>
        /// Clear Decimal Mode Flag
        /// </summary>
        public const string CLD = "cld";
        /// <summary>
        /// Clear Interrupt Disable Flag
        /// </summary>
        public const string CLI = "cli";
        /// <summary>
        /// Clear Overflow Flag
        /// </summary>
        public const string CLV = "clv";
        /// <summary>
        /// Set Carry Flag
        /// </summary>
        public const string SEC = "sec";
        /// <summary>
        /// Set Decimal Mode Flag
        /// </summary>
        public const string SED = "sed";
        /// <summary>
        /// Set Interrupt Disable Flag
        /// </summary>
        public const string SEI = "sei";
        #endregion

        #region System Functions
        /// <summary>
        /// Force an Interrupt
        /// </summary>
        public const string BRK = "brk";
        /// <summary>
        /// No Operation
        /// </summary>
        public const string NOP = "nop";
        /// <summary>
        /// Return from Interrupt
        /// </summary>
        public const string RTI = "rti";
        #endregion
    }
}
