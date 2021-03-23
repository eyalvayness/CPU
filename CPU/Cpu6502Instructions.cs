using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU
{
    public static class Cpu6502ByteCodesIntructions
    {

        #region Load Operations
        // Load Accumulator
        public const byte LDA_IMMEDIATE  = (byte)'\u00A9';
        public const byte LDA_ZEROPAGE   = (byte)'\u00A5';
        public const byte LDA_ZEROPAGE_X = (byte)'\u00B5';
        public const byte LDA_ABSOLUTE   = (byte)'\u00AD';
        public const byte LDA_ABSOLUTE_X = (byte)'\u00BD';
        public const byte LDA_ABSOLUTE_Y = (byte)'\u00B9';
        public const byte LDA_INDERECT_X = (byte)'\u00A1';
        public const byte LDA_INDERECT_Y = (byte)'\u00B1';

        // Load X Register
        public const byte LDX_IMMEDIATE  = (byte)'\u00A2';
        public const byte LDX_ZEROPAGE   = (byte)'\u00A6';
        public const byte LDX_ZEROPAGE_Y = (byte)'\u00B6';
        public const byte LDX_ABSOLUTE   = (byte)'\u00AE';
        public const byte LDX_ABSOLUTE_Y = (byte)'\u00BE';

        // Load Y Register
        public const byte LDY_IMMEDIATE  = (byte)'\u00A0';
        public const byte LDY_ZEROPAGE   = (byte)'\u00A4';
        public const byte LDY_ZEROPAGE_X = (byte)'\u00B4';
        public const byte LDY_ABSOLUTE   = (byte)'\u00AC';
        public const byte LDY_ABSOLUTE_X = (byte)'\u00BC';
        #endregion

        #region Store Operations
        // Store Accumulator
        public const byte STA_ZEROPAGE   = (byte)'\u0085';
        public const byte STA_ZEROPAGE_X = (byte)'\u0095';
        public const byte STA_ABSOLUTE   = (byte)'\u008D';
        public const byte STA_ABSOLUTE_X = (byte)'\u009D';
        public const byte STA_ABSOLUTE_Y = (byte)'\u0099';
        public const byte STA_INDERECT_X = (byte)'\u0081';
        public const byte STA_INDERECT_Y = (byte)'\u0091';

        // Store X Register
        public const byte STX_ZEROPAGE   = (byte)'\u0086';
        public const byte STX_ZEROPAGE_Y = (byte)'\u0096';
        public const byte STX_ABSOLUTE   = (byte)'\u008E';

        // Store Y Register
        public const byte STY_ZEROPAGE   = (byte)'\u0084';
        public const byte STY_ZEROPAGE_X = (byte)'\u0094';
        public const byte STY_ABSOLUTE   = (byte)'\u008C';
        #endregion

        #region Register Transfers
        public const byte TAX = (byte)'\u00AA'; // Transfer Accumulator To X
        public const byte TAY = (byte)'\u00A8'; // Transfer Accumulator To Y
        public const byte TXA = (byte)'\u008A'; // Transfer X To Accumulator
        public const byte TYA = (byte)'\u0098'; // Transfer Y To Accumulator
        #endregion

        #region Stack Operations
        public const byte TSX = (byte)'\u00BA'; // Transfer Stack Pointer to X
        public const byte TXS = (byte)'\u009A'; // Transfer X to Stack Pointer
        public const byte PHA = (byte)'\u0048'; // Push Accumulator on Stack
        public const byte PHP = (byte)'\u0008'; // Push Processor Status on Stack
        public const byte PLA = (byte)'\u0068'; // Pull Accumulator from Stack
        public const byte PLP = (byte)'\u0028'; // Pull Processor Status from Stack
        #endregion

        #region Logical
        // Logical AND
        public const byte AND_IMMEDIATE  = (byte)'\u0029';
        public const byte AND_ZEROPAGE   = (byte)'\u0025';
        public const byte AND_ZEROPAGE_X = (byte)'\u0035';
        public const byte AND_ABSOLUTE   = (byte)'\u002D';
        public const byte AND_ABSOLUTE_X = (byte)'\u003D';
        public const byte AND_ABSOLUTE_Y = (byte)'\u0039';
        public const byte AND_INDERECT_X = (byte)'\u0021';
        public const byte AND_INDERECT_Y = (byte)'\u0031';

        // Exclusive OR
        public const byte EOR_IMMEDIATE  = (byte)'\u0049';
        public const byte EOR_ZEROPAGE   = (byte)'\u0045';
        public const byte EOR_ZEROPAGE_X = (byte)'\u0055';
        public const byte EOR_ABSOLUTE   = (byte)'\u004D';
        public const byte EOR_ABSOLUTE_X = (byte)'\u005D';
        public const byte EOR_ABSOLUTE_Y = (byte)'\u0059';
        public const byte EOR_INDERECT_X = (byte)'\u0041';
        public const byte EOR_INDERECT_Y = (byte)'\u0051';

        // Logical Inclusive OR
        public const byte ORA_IMMEDIATE  = (byte)'\u0009';
        public const byte ORA_ZEROPAGE   = (byte)'\u0005';
        public const byte ORA_ZEROPAGE_X = (byte)'\u0015';
        public const byte ORA_ABSOLUTE   = (byte)'\u000D';
        public const byte ORA_ABSOLUTE_X = (byte)'\u001D';
        public const byte ORA_ABSOLUTE_Y = (byte)'\u0019';
        public const byte ORA_INDERECT_X = (byte)'\u0001';
        public const byte ORA_INDERECT_Y = (byte)'\u0011';

        // Bit Test
        public const byte BIT_ZEROPAGE = (byte)'\u0024';
        public const byte BIT_ABSOLUTE = (byte)'\u002C';
        #endregion

        #region Arithmetic
        // Add with Carry
        public const byte ADC_IMMEDIATE  = (byte)'\u0069';
        public const byte ADC_ZEROPAGE   = (byte)'\u0065';
        public const byte ADC_ZEROPAGE_X = (byte)'\u0075';
        public const byte ADC_ABSOLUTE   = (byte)'\u006D';
        public const byte ADC_ABSOLUTE_X = (byte)'\u007D';
        public const byte ADC_ABSOLUTE_Y = (byte)'\u0079';
        public const byte ADC_INDERECT_X = (byte)'\u0061';
        public const byte ADC_INDERECT_Y = (byte)'\u0071';

        // Substract with Carry
        public const byte SBC_IMMEDIATE  = (byte)'\u00E9';
        public const byte SBC_ZEROPAGE   = (byte)'\u00E5';
        public const byte SBC_ZEROPAGE_X = (byte)'\u00F5';
        public const byte SBC_ABSOLUTE   = (byte)'\u00ED';
        public const byte SBC_ABSOLUTE_X = (byte)'\u00FD';
        public const byte SBC_ABSOLUTE_Y = (byte)'\u00F9';
        public const byte SBC_INDERECT_X = (byte)'\u00E1';
        public const byte SBC_INDERECT_Y = (byte)'\u00F1';

        // Compare Accumulator
        public const byte CMP_IMMEDIATE  = (byte)'\u00C9';
        public const byte CMP_ZEROPAGE   = (byte)'\u00C5';
        public const byte CMP_ZEROPAGE_X = (byte)'\u00D5';
        public const byte CMP_ABSOLUTE   = (byte)'\u00CD';
        public const byte CMP_ABSOLUTE_X = (byte)'\u00DD';
        public const byte CMP_ABSOLUTE_Y = (byte)'\u00D9';
        public const byte CMP_INDERECT_X = (byte)'\u00C1';
        public const byte CMP_INDERECT_Y = (byte)'\u00D1';

        // Compare X Register
        public const byte CPX_IMMEDIATE  = (byte)'\u00E0';
        public const byte CPX_ZEROPAGE   = (byte)'\u00E4';
        public const byte CPX_ABSOLUTE   = (byte)'\u00EC';

        // Compare Y Register
        public const byte CPY_IMMEDIATE  = (byte)'\u00C0';
        public const byte CPY_ZEROPAGE   = (byte)'\u00C4';
        public const byte CPY_ABSOLUTE   = (byte)'\u00CC';
        #endregion

        #region Increments & Decrements
        // Increment a Memory Location
        public const byte INC_ZEROPAGE   = (byte)'\u00E6';
        public const byte INC_ZEROPAGE_X = (byte)'\u00F6';
        public const byte INC_ABSOLUTE   = (byte)'\u00EE';
        public const byte INC_ABSOLUTE_X = (byte)'\u00FE';

        public const byte INX = (byte)'\u00E8'; // Increment the X Register
        public const byte INY = (byte)'\u00C8'; // Increment the Y Register

        // Decrement a Memory Location
        public const byte DEC_ZEROPAGE   = (byte)'\u00C6';
        public const byte DEC_ZEROPAGE_X = (byte)'\u00D6';
        public const byte DEC_ABSOLUTE   = (byte)'\u00CE';
        public const byte DEC_ABSOLUTE_X = (byte)'\u00DE';

        public const byte DEX = (byte)'\u00CA'; // Decrement the X Register
        public const byte DEY = (byte)'\u0088'; // Decrement the Y Register
        #endregion

        #region Shifts
        // Arithmetic Shift Left
        public const byte ASL_ACCUMULATOR = (byte)'\u000A';
        public const byte ASL_ZEROPAGE    = (byte)'\u0006';
        public const byte ASL_ZEROPAGE_X  = (byte)'\u0016';
        public const byte ASL_ABSOLUTE    = (byte)'\u000E';
        public const byte ASL_ABSOLUTE_X  = (byte)'\u001E';

        // Logical Shift Right
        public const byte LSR_ACCUMULATOR = (byte)'\u004A';
        public const byte LSR_ZEROPAGE    = (byte)'\u0046';
        public const byte LSR_ZEROPAGE_X  = (byte)'\u0056';
        public const byte LSR_ABSOLUTE    = (byte)'\u004E';
        public const byte LSR_ABSOLUTE_X  = (byte)'\u005E';

        // Rotate Left
        public const byte ROL_ACCUMULATOR = (byte)'\u002A';
        public const byte ROL_ZEROPAGE    = (byte)'\u0026';
        public const byte ROL_ZEROPAGE_X  = (byte)'\u0036';
        public const byte ROL_ABSOLUTE    = (byte)'\u002E';
        public const byte ROL_ABSOLUTE_X  = (byte)'\u003E';

        // Rotate Right
        public const byte ROR_ACCUMULATOR = (byte)'\u006A';
        public const byte ROR_ZEROPAGE    = (byte)'\u0066';
        public const byte ROR_ZEROPAGE_X  = (byte)'\u0076';
        public const byte ROR_ABSOLUTE    = (byte)'\u006E';
        public const byte ROR_ABSOLUTE_X  = (byte)'\u007E';
        #endregion

        #region Jumps & Calls
        // Jump to Another Location
        public const byte JMP_ABSOLUTE = (byte)'\u004C';
        public const byte JMP_INDIRECT = (byte)'\u006C';

        public const byte JSR = (byte)'\u0020'; // Jump to a Subroutine
        public const byte RSS = (byte)'\u0060'; // Return from Subroutine
        #endregion

        #region Branches
        public const byte BCC = (byte)'\u0090';// Branch if Carry Flag Clear
        public const byte BCS = (byte)'\u00B0';// Branch if Carry Flag Set
        public const byte BEQ = (byte)'\u00F0';// Branch if Zero Flag Set
        public const byte BMI = (byte)'\u0030';// Branch if Negative Flag Set
        public const byte BNE = (byte)'\u00D0';// Branch if Zero Flag Clear
        public const byte BPL = (byte)'\u0010';// Branch if Negative Flag Clear
        public const byte BVC = (byte)'\u0050';// Branch if Overflow Flag Clear
        public const byte BVS = (byte)'\u0070';// Branch if Overflow Flag Set
        #endregion

        #region Status Flag Changes
        public const byte CLC = (byte)'\u0018'; // Clear Carry Flag
        public const byte CLD = (byte)'\u00D8'; // Clear Decimal Mode Flag
        public const byte CLI = (byte)'\u0058'; // Clear Interrupt Disable Flag
        public const byte CLV = (byte)'\u00D8'; // Clear Overflow Flag
        public const byte SEC = (byte)'\u0038'; // Set Carry Flag
        public const byte SED = (byte)'\u00F8'; // Set Decimal Mode Flag
        public const byte SEI = (byte)'\u0078'; // Set Interrupt Disable Flag
        #endregion

        #region System Functions
        public const byte BRK = (byte)'\u0000'; // Force an Interrupt
        public const byte NOP = (byte)'\u00EA'; // No Operation
        public const byte RTI = (byte)'\u0040'; // Return from Interrupt
        #endregion
    }

    public static class Cpu6502StringIntructions
    {
        #region Load Operations
        public const string LDA = "LDA"; // Load Accumulator
        public const string LDX = "LDX"; // Load X Register
        public const string LDY = "LDY"; // Load Y Register
        #endregion

        #region Store Operations
        public const string STA = "STA"; // Store Accumulator
        public const string STX = "STX"; // Store X Register
        public const string STY = "STY"; // Store Y Register
        #endregion

        #region Register Transfers
        public const string TAX = "TAX"; // Transfer Accumulator To X
        public const string TAY = "TAY"; // Transfer Accumulator To Y
        public const string TXA = "TXA"; // Transfer X To Accumulator
        public const string TYA = "TYA"; // Transfer Y To Accumulator
        #endregion

        #region Stack Operations
        public const string TSX = "TSX"; // Transfer Stack Pointer to X
        public const string TXS = "TXS"; // Transfer X to Stack Pointer
        public const string PHA = "PHA"; // Push Accumulator on Stack
        public const string PHP = "PHP"; // Push Processor Status on Stack
        public const string PLA = "PLA"; // Pull Accumulator from Stack
        public const string PLP = "PLP"; // Pull Processor Status from Stack
        #endregion

        #region Logical
        public const string AND = "AND"; // Logical AND
        public const string EOR = "EOR"; // Exclusive OR
        public const string ORA = "ORA"; // Logical Inclusive OR
        public const string BIT = "BIT"; // Bit Test
        #endregion

        #region Arithmetic
        public const string ADC = "ADC"; // Add with Carry
        public const string SBC = "SBC"; // Substract with Carry
        public const string CMP = "CMP"; // Compare Accumulator
        public const string CPX = "CPX"; // Compare X Register
        public const string CPY = "CPY"; // Compare Y Register
        #endregion

        #region Increments & Decrements
        public const string INC = "INC"; // Increment a Memory Location
        public const string INX = "INX"; // Increment the X Register
        public const string INY = "INY"; // Increment the Y Register
        public const string DEC = "DEC"; // Decrement a Memory Location
        public const string DEX = "DEX"; // Decrement the X Register
        public const string DEY = "DEY"; // Decrement the Y Register
        #endregion

        #region Shifts
        public const string ASL = "ASL"; // Arithmetic Shift Left
        public const string LSR = "LSR"; // Logical Shift Right
        public const string ROL = "ROL"; // Rotate Left
        public const string ROR = "ROR"; // Rotate Right
        #endregion

        #region Jumps & Calls
        public const string JMP = "JMP"; // Jump to Another Location
        public const string JSR = "JSR"; // Jump to a Subroutine
        public const string RTS = "RTS"; // Return from Subroutine
        #endregion

        #region Branches
        public const string BCC = "BCC"; // Branch if Carry Flag Clear
        public const string BCS = "BCS"; // Branch if Carry Flag Set
        public const string BEQ = "BEQ"; // Branch if Zero Flag Set
        public const string BMI = "BMI"; // Branch if Negative Flag Set
        public const string BNE = "BNE"; // Branch if Zero Flag Clear
        public const string BPL = "BPL"; // Branch if Negative Flag Clear
        public const string BVC = "BVC"; // Branch if Overflow Flag Clear
        public const string BVS = "BVS"; // Branch if Overflow Flag Set
        #endregion

        #region Status Flag Changes
        public const string CLC = "CLC"; // Clear Carry Flag
        public const string CLD = "CLD"; // Clear Decimal Mode Flag
        public const string CLI = "CLI"; // Clear Interrupt Disable Flag
        public const string CLV = "CLV"; // Clear Overflow Flag
        public const string SEC = "SEC"; // Set Carry Flag
        public const string SED = "SED"; // Set Decimal Mode Flag
        public const string SEI = "SEI"; // Set Interrupt Disable Flag
        #endregion

        #region System Functions
        public const string BRK = "BRK"; // Force an Interrupt
        public const string NOP = "NOP"; // No Operation
        public const string RTI = "RTI"; // Return from Interrupt
        #endregion
    }
}
