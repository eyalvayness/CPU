using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU
{
    public static class Cpu6502ByteCodesInstructions
    {

        #region Load Operations
        // Load Accumulator
        public const byte LDA_IMMEDIATE  = 0xA9;
        public const byte LDA_ZEROPAGE   = 0xA5;
        public const byte LDA_ZEROPAGE_X = 0xB5;
        public const byte LDA_ABSOLUTE   = 0xAD;
        public const byte LDA_ABSOLUTE_X = 0xBD;
        public const byte LDA_ABSOLUTE_Y = 0xB9;
        public const byte LDA_INDIRECT_X = 0xA1;
        public const byte LDA_INDIRECT_Y = 0xB1;

        // Load X Register
        public const byte LDX_IMMEDIATE  = 0xA2;
        public const byte LDX_ZEROPAGE   = 0xA6;
        public const byte LDX_ZEROPAGE_Y = 0xB6;
        public const byte LDX_ABSOLUTE   = 0xAE;
        public const byte LDX_ABSOLUTE_Y = 0xBE;

        // Load Y Register
        public const byte LDY_IMMEDIATE  = 0xA0;
        public const byte LDY_ZEROPAGE   = 0xA4;
        public const byte LDY_ZEROPAGE_X = 0xB4;
        public const byte LDY_ABSOLUTE   = 0xAC;
        public const byte LDY_ABSOLUTE_X = 0xBC;
        #endregion

        #region Store Operations
        // Store Accumulator
        public const byte STA_ZEROPAGE   = 0x85;
        public const byte STA_ZEROPAGE_X = 0x95;
        public const byte STA_ABSOLUTE   = 0x8D;
        public const byte STA_ABSOLUTE_X = 0x9D;
        public const byte STA_ABSOLUTE_Y = 0x99;
        public const byte STA_INDIRECT_X = 0x81;
        public const byte STA_INDIRECT_Y = 0x91;

        // Store X Register
        public const byte STX_ZEROPAGE   = 0x86;
        public const byte STX_ZEROPAGE_Y = 0x96;
        public const byte STX_ABSOLUTE   = 0x8E;

        // Store Y Register
        public const byte STY_ZEROPAGE   = 0x84;
        public const byte STY_ZEROPAGE_X = 0x94;
        public const byte STY_ABSOLUTE   = 0x8C;
        #endregion

        #region Register Transfers
        public const byte TAX = 0xAA; // Transfer Accumulator To X
        public const byte TAY = 0xA8; // Transfer Accumulator To Y
        public const byte TXA = 0x8A; // Transfer X To Accumulator
        public const byte TYA = 0x98; // Transfer Y To Accumulator
        #endregion

        #region Stack Operations
        public const byte TSX = 0xBA; // Transfer Stack Pointer to X
        public const byte TXS = 0x9A; // Transfer X to Stack Pointer
        public const byte PHA = 0x48; // Push Accumulator on Stack
        public const byte PHP = 0x08; // Push Processor Status on Stack
        public const byte PLA = 0x68; // Pull Accumulator from Stack
        public const byte PLP = 0x28; // Pull Processor Status from Stack
        #endregion

        #region Logical
        // Logical AND
        public const byte AND_IMMEDIATE  = 0x29;
        public const byte AND_ZEROPAGE   = 0x25;
        public const byte AND_ZEROPAGE_X = 0x35;
        public const byte AND_ABSOLUTE   = 0x2D;
        public const byte AND_ABSOLUTE_X = 0x3D;
        public const byte AND_ABSOLUTE_Y = 0x39;
        public const byte AND_INDIRECT_X = 0x21;
        public const byte AND_INDIRECT_Y = 0x31;

        // Exclusive OR
        public const byte EOR_IMMEDIATE  = 0x49;
        public const byte EOR_ZEROPAGE   = 0x45;
        public const byte EOR_ZEROPAGE_X = 0x55;
        public const byte EOR_ABSOLUTE   = 0x4D;
        public const byte EOR_ABSOLUTE_X = 0x5D;
        public const byte EOR_ABSOLUTE_Y = 0x59;
        public const byte EOR_INDIRECT_X = 0x41;
        public const byte EOR_INDIRECT_Y = 0x51;

        // Logical Inclusive OR
        public const byte ORA_IMMEDIATE  = 0x09;
        public const byte ORA_ZEROPAGE   = 0x05;
        public const byte ORA_ZEROPAGE_X = 0x15;
        public const byte ORA_ABSOLUTE   = 0x0D;
        public const byte ORA_ABSOLUTE_X = 0x1D;
        public const byte ORA_ABSOLUTE_Y = 0x19;
        public const byte ORA_INDIRECT_X = 0x01;
        public const byte ORA_INDIRECT_Y = 0x11;

        // Bit Test
        public const byte BIT_ZEROPAGE = 0x24;
        public const byte BIT_ABSOLUTE = 0x2C;
        #endregion

        #region Arithmetic
        // Add with Carry
        public const byte ADC_IMMEDIATE  = 0x69;
        public const byte ADC_ZEROPAGE   = 0x65;
        public const byte ADC_ZEROPAGE_X = 0x75;
        public const byte ADC_ABSOLUTE   = 0x6D;
        public const byte ADC_ABSOLUTE_X = 0x7D;
        public const byte ADC_ABSOLUTE_Y = 0x79;
        public const byte ADC_INDIRECT_X = 0x61;
        public const byte ADC_INDIRECT_Y = 0x71;

        // Substract with Carry
        public const byte SBC_IMMEDIATE  = 0xE9;
        public const byte SBC_ZEROPAGE   = 0xE5;
        public const byte SBC_ZEROPAGE_X = 0xF5;
        public const byte SBC_ABSOLUTE   = 0xED;
        public const byte SBC_ABSOLUTE_X = 0xFD;
        public const byte SBC_ABSOLUTE_Y = 0xF9;
        public const byte SBC_INDIRECT_X = 0xE1;
        public const byte SBC_INDIRECT_Y = 0xF1;

        // Compare Accumulator
        public const byte CMP_IMMEDIATE  = 0xC9;
        public const byte CMP_ZEROPAGE   = 0xC5;
        public const byte CMP_ZEROPAGE_X = 0xD5;
        public const byte CMP_ABSOLUTE   = 0xCD;
        public const byte CMP_ABSOLUTE_X = 0xDD;
        public const byte CMP_ABSOLUTE_Y = 0xD9;
        public const byte CMP_INDIRECT_X = 0xC1;
        public const byte CMP_INDIRECT_Y = 0xD1;

        // Compare X Register
        public const byte CPX_IMMEDIATE  = 0xE0;
        public const byte CPX_ZEROPAGE   = 0xE4;
        public const byte CPX_ABSOLUTE   = 0xEC;

        // Compare Y Register
        public const byte CPY_IMMEDIATE  = 0xC0;
        public const byte CPY_ZEROPAGE   = 0xC4;
        public const byte CPY_ABSOLUTE   = 0xCC;
        #endregion

        #region Increments & Decrements
        // Increment a Memory Location
        public const byte INC_ZEROPAGE   = 0xE6;
        public const byte INC_ZEROPAGE_X = 0xF6;
        public const byte INC_ABSOLUTE   = 0xEE;
        public const byte INC_ABSOLUTE_X = 0xFE;

        public const byte INX = 0xE8; // Increment the X Register
        public const byte INY = 0xC8; // Increment the Y Register

        // Decrement a Memory Location
        public const byte DEC_ZEROPAGE   = 0xC6;
        public const byte DEC_ZEROPAGE_X = 0xD6;
        public const byte DEC_ABSOLUTE   = 0xCE;
        public const byte DEC_ABSOLUTE_X = 0xDE;

        public const byte DEX = 0xCA; // Decrement the X Register
        public const byte DEY = 0x88; // Decrement the Y Register
        #endregion

        #region Shifts
        // Arithmetic Shift Left
        public const byte ASL_ACCUMULATOR = 0x0A;
        public const byte ASL_ZEROPAGE    = 0x06;
        public const byte ASL_ZEROPAGE_X  = 0x16;
        public const byte ASL_ABSOLUTE    = 0x0E;
        public const byte ASL_ABSOLUTE_X  = 0x1E;
                                            
        // Logical Shift Right              
        public const byte LSR_ACCUMULATOR = 0x4A;
        public const byte LSR_ZEROPAGE    = 0x46;
        public const byte LSR_ZEROPAGE_X  = 0x56;
        public const byte LSR_ABSOLUTE    = 0x4E;
        public const byte LSR_ABSOLUTE_X  = 0x5E;
                                            
        // Rotate Left                      
        public const byte ROL_ACCUMULATOR = 0x2A;
        public const byte ROL_ZEROPAGE    = 0x26;
        public const byte ROL_ZEROPAGE_X  = 0x36;
        public const byte ROL_ABSOLUTE    = 0x2E;
        public const byte ROL_ABSOLUTE_X  = 0x3E;
                                            
        // Rotate Right                     
        public const byte ROR_ACCUMULATOR = 0x6A;
        public const byte ROR_ZEROPAGE    = 0x66;
        public const byte ROR_ZEROPAGE_X  = 0x76;
        public const byte ROR_ABSOLUTE    = 0x6E;
        public const byte ROR_ABSOLUTE_X  = 0x7E;
        #endregion

        #region Jumps & Calls
        // Jump to Another Location
        public const byte JMP_ABSOLUTE = 0x4C;
        public const byte JMP_INDIRECT = 0x6C;

        public const byte JSR_ABSOLUTE = 0x20; // Jump to a Subroutine
        public const byte RTS = 0x60; // Return from Subroutine
        #endregion

        #region Branches
        public const byte BCC_ABSOLUTE = 0x90;// Branch if Carry Flag Clear
        public const byte BCS_ABSOLUTE = 0xB0;// Branch if Carry Flag Set
        public const byte BEQ_ABSOLUTE = 0xF0;// Branch if Zero Flag Set
        public const byte BMI_ABSOLUTE = 0x30;// Branch if Negative Flag Set
        public const byte BNE_ABSOLUTE = 0xD0;// Branch if Zero Flag Clear
        public const byte BPL_ABSOLUTE = 0x10;// Branch if Negative Flag Clear
        public const byte BVC_ABSOLUTE = 0x50;// Branch if Overflow Flag Clear
        public const byte BVS_ABSOLUTE = 0x70;// Branch if Overflow Flag Set
        #endregion

        #region Status Flag Changes
        public const byte CLC = 0x18; // Clear Carry Flag
        public const byte CLD = 0xD8; // Clear Decimal Mode Flag
        public const byte CLI = 0x58; // Clear Interrupt Disable Flag
        public const byte CLV = 0xD8; // Clear Overflow Flag
        public const byte SEC = 0x38; // Set Carry Flag
        public const byte SED = 0xF8; // Set Decimal Mode Flag
        public const byte SEI = 0x78; // Set Interrupt Disable Flag
        #endregion

        #region System Functions
        public const byte BRK = 0x00; // Force an Interrupt
        public const byte NOP = 0xEA; // No Operation
        public const byte RTI = 0x40; // Return from Interrupt
        #endregion
    }

    public static class Cpu6502StringInstructions
    {
        #region Load Operations
        public const string LDA = "lda"; // Load Accumulator
        public const string LDX = "ldx"; // Load X Register
        public const string LDY = "ldy"; // Load Y Register
        #endregion

        #region Store Operations
        public const string STA = "sta"; // Store Accumulator
        public const string STX = "stx"; // Store X Register
        public const string STY = "sty"; // Store Y Register
        #endregion

        #region Register Transfers
        public const string TAX = "tax"; // Transfer Accumulator To X
        public const string TAY = "tay"; // Transfer Accumulator To Y
        public const string TXA = "txa"; // Transfer X To Accumulator
        public const string TYA = "tya"; // Transfer Y To Accumulator
        #endregion

        #region Stack Operations
        public const string TSX = "tsx"; // Transfer Stack Pointer to X
        public const string TXS = "txs"; // Transfer X to Stack Pointer
        public const string PHA = "pha"; // Push Accumulator on Stack
        public const string PHP = "php"; // Push Processor Status on Stack
        public const string PLA = "pla"; // Pull Accumulator from Stack
        public const string PLP = "plp"; // Pull Processor Status from Stack
        #endregion

        #region Logical
        public const string AND = "and"; // Logical AND
        public const string EOR = "eor"; // Exclusive OR
        public const string ORA = "ora"; // Logical Inclusive OR
        public const string BIT = "bit"; // Bit Test
        #endregion

        #region Arithmetic
        public const string ADC = "adc"; // Add with Carry
        public const string SBC = "sbc"; // Substract with Carry
        public const string CMP = "cmp"; // Compare Accumulator
        public const string CPX = "cpx"; // Compare X Register
        public const string CPY = "cpy"; // Compare Y Register
        #endregion

        #region Increments & Decrements
        public const string INC = "inc"; // Increment a Memory Location
        public const string INX = "inx"; // Increment the X Register
        public const string INY = "iny"; // Increment the Y Register
        public const string DEC = "dec"; // Decrement a Memory Location
        public const string DEX = "dex"; // Decrement the X Register
        public const string DEY = "dey"; // Decrement the Y Register
        #endregion

        #region Shifts
        public const string ASL = "asl"; // Arithmetic Shift Left
        public const string LSR = "lsr"; // Logical Shift Right
        public const string ROL = "rol"; // Rotate Left
        public const string ROR = "ror"; // Rotate Right
        #endregion

        #region Jumps & Calls
        public const string JMP = "jmp"; // Jump to Another Location
        public const string JSR = "jsr"; // Jump to a Subroutine
        public const string RTS = "rts"; // Return from Subroutine
        #endregion

        #region Branches
        public const string BCC = "bcc"; // Branch if Carry Flag Clear
        public const string BCS = "bcs"; // Branch if Carry Flag Set
        public const string BEQ = "beq"; // Branch if Zero Flag Set
        public const string BMI = "bmi"; // Branch if Negative Flag Set
        public const string BNE = "bne"; // Branch if Zero Flag Clear
        public const string BPL = "bpl"; // Branch if Negative Flag Clear
        public const string BVC = "bvc"; // Branch if Overflow Flag Clear
        public const string BVS = "bvs"; // Branch if Overflow Flag Set
        #endregion

        #region Status Flag Changes
        public const string CLC = "clc"; // Clear Carry Flag
        public const string CLD = "cld"; // Clear Decimal Mode Flag
        public const string CLI = "cli"; // Clear Interrupt Disable Flag
        public const string CLV = "clv"; // Clear Overflow Flag
        public const string SEC = "sec"; // Set Carry Flag
        public const string SED = "sed"; // Set Decimal Mode Flag
        public const string SEI = "sei"; // Set Interrupt Disable Flag
        #endregion

        #region System Functions
        public const string BRK = "brk"; // Force an Interrupt
        public const string NOP = "nop"; // No Operation
        public const string RTI = "rti"; // Return from Interrupt
        #endregion
    }
}
