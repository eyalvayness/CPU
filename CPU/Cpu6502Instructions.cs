using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU
{
    public static class Cpu6502ByteCodesIntructions
    {
    }
    public static class Cpu6502StringIntructions
    {
        //Load Operations
        public const string LDA = "LDA"; // Load Accumulator
        public const string LDX = "LDX"; // Load X Register
        public const string LDY = "LDY"; // Load Y Register

        //Store Operations
        public const string STA = "STA"; // Store Accumulator
        public const string STX = "STX"; // Store X Register
        public const string STY = "STY"; // Store Y Register

        //Register Transfers
        public const string TAX = "TAX"; // Transfer Accumulator To X
        public const string TAY = "TAY"; // Transfer Accumulator To Y
        public const string TXA = "TXA"; // Transfer X To Accumulator
        public const string TYA = "TYA"; // Transfer Y To Accumulator

        //Stack Operations
        public const string TSX = "TSX"; // Transfer Stack Pointer to X
        public const string TXS = "TXS"; // Transfer X to Stack Pointer
        public const string PHA = "PHA"; // Push Accumulator on Stack
        public const string PHP = "PHP"; // Push Processor Status on Stack
        public const string PLA = "PLA"; // Pull Accumulator from Stack
        public const string PLP = "PLP"; // Pull Processor Status from Stack

        //Logical
        public const string AND = "AND"; // Logical AND
        public const string EOR = "EOR"; // Exclusive OR
        public const string ORA = "ORA"; // Logical Inclusive OR
        public const string BIT = "BIT"; // Bit Test

        //Arithmetic
        public const string ADC = "ADC"; // Add with Carry
        public const string SBC = "SBC"; // Substract with Carry
        public const string CMP = "CMP"; // Compare Accumulator
        public const string CPX = "CPX"; // Compare X Register
        public const string CPY = "CPY"; // Compare Y Register

        //Increments & Decrements
        public const string INC = "INC"; // Increment a Memory Location
        public const string INX = "INX"; // Increment the X Register
        public const string INY = "INY"; // Increment the Y Register
        public const string DEC = "DEC"; // Decrement a Memory Location
        public const string DEX = "DEX"; // Decrement the X Register
        public const string DEY = "DEY"; // Decrement the Y Register

        //Shifts
        public const string ASL = "ASL"; // Arithmetic Shift Left
        public const string LSR = "LSR"; // Logical Shift Right
        public const string ROL = "ROL"; // Rotate Left
        public const string ROR = "ROR"; // Rotate Right

        //Jumps & Calls
        public const string JMP = "JMP"; // Jump to Another Location
        public const string JSR = "JSR"; // Jump to a Subroutine
        public const string RTS = "RTS"; // Return from Subroutine

        //Branches
        public const string BCC = "BCC"; // Branch if Carry Flag Clear
        public const string BCS = "BCS"; // Branch if Carry Flag Set
        public const string BEQ = "BEQ"; // Branch if Zero Flag Set
        public const string BMI = "BMI"; // Branch if Negative Flag Set
        public const string BNE = "BNE"; // Branch if Zero Flag Clear
        public const string BPL = "BPL"; // Branch if Negative Flag Clear
        public const string BVC = "BVC"; // Branch if Overflow Flag Clear
        public const string BVS = "BVS"; // Branch if Overflow Flag Set

        //Status Flag Changes
        public const string CLC = "CLC"; // Clear Carry Flag
        public const string CLD = "CLD"; // Clear Decimal Mode Flag
        public const string CLI = "CLI"; // Clear Interrupt Disable Flag
        public const string CLV = "CLV"; // Clear Overflow Flag
        public const string SEC = "SEC"; // Set Carry Flag
        public const string SED = "SED"; // Set Decimal Mode Flag
        public const string SEI = "SEI"; // Set Interrupt Disable Flag

        //System Functions
        public const string BRK = "BRK"; // Force an Interrupt
        public const string NOP = "NOP"; // No Operation
        public const string RTI = "RTI"; // Return from Interrupt
    }
}
