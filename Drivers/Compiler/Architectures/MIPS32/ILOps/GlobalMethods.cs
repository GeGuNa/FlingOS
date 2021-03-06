#region LICENSE
// ---------------------------------- LICENSE ---------------------------------- //
//
//    Fling OS - The educational operating system
//    Copyright (C) 2015 Edward Nutting
//
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 2 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
//  Project owner: 
//		Email: edwardnutting@outlook.com
//		For paper mail address, please contact via email for details.
//
// ------------------------------------------------------------------------------ //
#endregion
    
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Drivers.Compiler.IL;

namespace Drivers.Compiler.Architectures.MIPS32
{
    public static class GlobalMethods
    {
        public static void LoadData(ILConversionState conversionState, ILOp theOp,
            string addressReg, string valueReg, int offset, int size, bool SignExtend = false)
        {
            if (size == 1)
            {
                conversionState.Append(new ASMOps.Mov() { Src = offset + "(" + addressReg + ")", Dest = "$t6", MoveType = ASMOps.Mov.MoveTypes.SrcMemoryToDestReg, Size = ASMOps.OperandSize.Byte, SignExtend = SignExtend });
            }
            else
            {
                conversionState.Append(new ASMOps.Xor() { Src1 = "$t6", Src2 = "$t6", Dest = "$t6" });
                int shiftBits = 0;
                if (offset % 2 == 1)
                {
                    conversionState.Append(new ASMOps.Mov() { Src = offset + "(" + addressReg + ")", Dest = "$t7", MoveType = ASMOps.Mov.MoveTypes.SrcMemoryToDestReg, Size = ASMOps.OperandSize.Byte });
                    conversionState.Append(new ASMOps.Or() { Src1 = "$t7", Src2 = "$t6", Dest = "$t6" });

                    size -= 1;
                    offset += 1;
                    shiftBits += 8;
                }

                while (size > 1)
                {
                    conversionState.Append(new ASMOps.Mov() { Src = offset + "(" + addressReg + ")", Dest = "$t7", MoveType = ASMOps.Mov.MoveTypes.SrcMemoryToDestReg, Size = ASMOps.OperandSize.Halfword, SignExtend = (SignExtend && size == 2) });
                    if (shiftBits > 0)
                    {
                        conversionState.Append(new ASMOps.Sll() { Src = "$t7", Dest = "$t7", Bits = shiftBits });
                    }
                    conversionState.Append(new ASMOps.Or() { Src1 = "$t7", Src2 = "$t6", Dest = "$t6" });

                    size -= 2;
                    offset += 2;
                    shiftBits += 16;
                }

                if (size == 1)
                {
                    conversionState.Append(new ASMOps.Mov() { Src = offset + "(" + addressReg + ")", Dest = "$t7", MoveType = ASMOps.Mov.MoveTypes.SrcMemoryToDestReg, Size = ASMOps.OperandSize.Byte, SignExtend = SignExtend });
                    if (shiftBits > 0)
                    {
                        conversionState.Append(new ASMOps.Sll() { Src = "$t7", Dest = "$t7", Bits = shiftBits });
                    }
                    conversionState.Append(new ASMOps.Or() { Src1 = "$t7", Src2 = "$t6", Dest = "$t6" });

                    size -= 1;
                    offset += 1;
                    shiftBits += 8;
                }
            }

            conversionState.Append(new ASMOps.Mov() { Src = "$t6", Dest = valueReg, Size = ASMOps.OperandSize.Word, MoveType = ASMOps.Mov.MoveTypes.RegToReg });
        }
        public static void StoreData(ILConversionState conversionState, ILOp theOp,
            string addressReg, string valueReg, int offset, int size)
        {
            if (size == 1)
            {
                conversionState.Append(new ASMOps.Mov() { Src = valueReg, Dest = offset + "(" + addressReg + ")", MoveType = ASMOps.Mov.MoveTypes.SrcRegToDestMemory, Size = ASMOps.OperandSize.Byte });
            }
            else
            {
                conversionState.Append(new ASMOps.Mov() { Dest = "$t6", Src = valueReg, Size = ASMOps.OperandSize.Word, MoveType = ASMOps.Mov.MoveTypes.RegToReg });
                
                if (offset % 2 == 1)
                {
                    conversionState.Append(new ASMOps.Mov() { Dest = offset + "(" + addressReg + ")", Src = "$t6", MoveType = ASMOps.Mov.MoveTypes.SrcRegToDestMemory, Size = ASMOps.OperandSize.Byte });
                    conversionState.Append(new ASMOps.Srl() { Src = "$t6", Dest = "$t6", Bits = 8 });

                    size -= 1;
                    offset += 1;
                }

                while (size > 1)
                {
                    conversionState.Append(new ASMOps.Mov() { Dest = offset + "(" + addressReg + ")", Src = "$t6", MoveType = ASMOps.Mov.MoveTypes.SrcRegToDestMemory, Size = ASMOps.OperandSize.Halfword });
                    conversionState.Append(new ASMOps.Srl() { Src = "$t6", Dest = "$t6", Bits = 16 });
                    
                    size -= 2;
                    offset += 2;
                }

                if(size == 1)
                {
                    conversionState.Append(new ASMOps.Mov() { Dest = offset + "(" + addressReg + ")", Src = "$t6", MoveType = ASMOps.Mov.MoveTypes.SrcRegToDestMemory, Size = ASMOps.OperandSize.Byte });
                    conversionState.Append(new ASMOps.Srl() { Src = "$t6", Dest = "$t6", Bits = 8 });

                    size -= 1;
                    offset += 1;
                }
            }
        }
    }
}
