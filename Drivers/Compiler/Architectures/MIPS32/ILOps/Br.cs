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
using Drivers.Compiler.IL;

namespace Drivers.Compiler.Architectures.MIPS32
{
    /// <summary>
    /// See base class documentation.
    /// </summary>
    public class Br : IL.ILOps.Br
    {
        public override void PerformStackOperations(ILPreprocessState conversionState, ILOp theOp)
        {
            ASMOps.BranchOp branchOp = ASMOps.BranchOp.Branch;
            
            switch ((OpCodes)theOp.opCode.Value)
            {
                case OpCodes.Br:
                case OpCodes.Br_S:
                    break;
                case OpCodes.Brtrue:
                    branchOp = ASMOps.BranchOp.BranchNotZero;
                    break;
                case OpCodes.Brtrue_S:
                    branchOp = ASMOps.BranchOp.BranchNotZero;
                    break;
                case OpCodes.Brfalse:
                    branchOp = ASMOps.BranchOp.BranchZero;
                    break;
                case OpCodes.Brfalse_S:
                    branchOp = ASMOps.BranchOp.BranchZero;
                    break;
                case OpCodes.Beq:
                    branchOp = ASMOps.BranchOp.BranchEqual;
                    break;
                case OpCodes.Beq_S:
                    branchOp = ASMOps.BranchOp.BranchEqual;
                    break;
                case OpCodes.Bne_Un:
                    branchOp = ASMOps.BranchOp.BranchNotEqual;
                    break;
                case OpCodes.Bne_Un_S:
                    branchOp = ASMOps.BranchOp.BranchNotEqual;
                    break;
                case OpCodes.Bge:
                    branchOp = ASMOps.BranchOp.BranchGreaterThanEqual;
                    break;
                case OpCodes.Bge_S:
                    branchOp = ASMOps.BranchOp.BranchGreaterThanEqual;
                    break;
                case OpCodes.Bge_Un:
                    branchOp = ASMOps.BranchOp.BranchGreaterThanEqual;
                    break;
                case OpCodes.Bge_Un_S:
                    branchOp = ASMOps.BranchOp.BranchGreaterThanEqual;
                    break;
                case OpCodes.Ble:
                    branchOp = ASMOps.BranchOp.BranchLessThanEqual;
                    break;
                case OpCodes.Ble_S:
                    branchOp = ASMOps.BranchOp.BranchLessThanEqual;
                    break;
                case OpCodes.Ble_Un:
                    branchOp = ASMOps.BranchOp.BranchLessThanEqual;
                    break;
                case OpCodes.Ble_Un_S:
                    branchOp = ASMOps.BranchOp.BranchLessThanEqual;
                    break;
                case OpCodes.Blt:
                    branchOp = ASMOps.BranchOp.BranchLessThan;
                    break;
                case OpCodes.Blt_S:
                    branchOp = ASMOps.BranchOp.BranchLessThan;
                    break;
                case OpCodes.Blt_Un:
                    branchOp = ASMOps.BranchOp.BranchLessThan;
                    break;
                case OpCodes.Blt_Un_S:
                    branchOp = ASMOps.BranchOp.BranchLessThan;
                    break;
                case OpCodes.Bgt:
                    branchOp = ASMOps.BranchOp.BranchGreaterThan;
                    break;
                case OpCodes.Bgt_S:
                    branchOp = ASMOps.BranchOp.BranchGreaterThan;
                    break;
                case OpCodes.Bgt_Un:
                    branchOp = ASMOps.BranchOp.BranchGreaterThan;
                    break;
                case OpCodes.Bgt_Un_S:
                    branchOp = ASMOps.BranchOp.BranchGreaterThan;
                    break;
            }

            if (branchOp == ASMOps.BranchOp.BranchZero || branchOp == ASMOps.BranchOp.BranchNotZero)
            {
                //Pop from our stack the test item to use in the condition
                StackItem testItem = conversionState.CurrentStackFrame.Stack.Pop();
            }
            else if (branchOp == ASMOps.BranchOp.BranchEqual || branchOp == ASMOps.BranchOp.BranchNotEqual ||
                         branchOp == ASMOps.BranchOp.BranchGreaterThanEqual ||
                         branchOp == ASMOps.BranchOp.BranchLessThanEqual ||
                         branchOp == ASMOps.BranchOp.BranchLessThan ||
                         branchOp == ASMOps.BranchOp.BranchGreaterThan)
            {
                //Pop from our stack the test items to use in the condition
                StackItem itemB = conversionState.CurrentStackFrame.Stack.Pop();
                StackItem itemA = conversionState.CurrentStackFrame.Stack.Pop();
            }
        }

        public override void Preprocess(ILPreprocessState preprocessState, ILOp theOp)
        {
            //This will store the offset from the current next op's position
            //to the IL op to jump to.
            int ILOffset = 0;
            
            switch ((OpCodes)theOp.opCode.Value)
            {
                case OpCodes.Br:
                    //Load the IL offset as signed Int 32 from the value bytes.
                    ILOffset = Utilities.ReadInt32(theOp.ValueBytes, 0);
                    break;
                case OpCodes.Br_S:
                    //Load the IL offset as signed Int 8 from the value bytes.
                    ILOffset = (int)(sbyte)theOp.ValueBytes[0];
                    break;
                case OpCodes.Brtrue:
                    //See above.
                    ILOffset = Utilities.ReadInt32(theOp.ValueBytes, 0);
                    break;
                case OpCodes.Brtrue_S:
                    //See above
                    ILOffset = (int)(sbyte)theOp.ValueBytes[0];
                    break;
                case OpCodes.Brfalse:
                    //See above
                    ILOffset = Utilities.ReadInt32(theOp.ValueBytes, 0);
                    break;
                case OpCodes.Brfalse_S:
                    //See above
                    ILOffset = (int)(sbyte)theOp.ValueBytes[0];
                    break;

                case OpCodes.Beq:
                    //See above
                    ILOffset = Utilities.ReadInt32(theOp.ValueBytes, 0);
                    break;
                case OpCodes.Beq_S:
                    //See above
                    ILOffset = (int)(sbyte)theOp.ValueBytes[0];
                    break;

                case OpCodes.Bne_Un:
                    //See above
                    ILOffset = Utilities.ReadInt32(theOp.ValueBytes, 0);
                    break;
                case OpCodes.Bne_Un_S:
                    //See above
                    ILOffset = (int)(sbyte)theOp.ValueBytes[0];
                    break;

                case OpCodes.Bge:
                    //See above
                    ILOffset = Utilities.ReadInt32(theOp.ValueBytes, 0);
                    break;
                case OpCodes.Bge_S:
                    //See above
                    ILOffset = (int)(sbyte)theOp.ValueBytes[0];
                    break;
                case OpCodes.Bge_Un:
                    //See above : This is unsigned variant
                    ILOffset = Utilities.ReadInt32(theOp.ValueBytes, 0);
                    break;
                case OpCodes.Bge_Un_S:
                    //See above
                    ILOffset = (int)(sbyte)theOp.ValueBytes[0];
                    break;

                case OpCodes.Ble:
                    //See above
                    ILOffset = Utilities.ReadInt32(theOp.ValueBytes, 0);
                    break;
                case OpCodes.Ble_S:
                    //See above
                    ILOffset = (int)(sbyte)theOp.ValueBytes[0];
                    break;
                case OpCodes.Ble_Un:
                    //See above : This is unsigned variant
                    ILOffset = Utilities.ReadInt32(theOp.ValueBytes, 0);
                    break;
                case OpCodes.Ble_Un_S:
                    //See above
                    ILOffset = (int)(sbyte)theOp.ValueBytes[0];
                    break;

                case OpCodes.Blt:
                    //See above
                    ILOffset = Utilities.ReadInt32(theOp.ValueBytes, 0);
                    break;
                case OpCodes.Blt_S:
                    //See above
                    ILOffset = (int)(sbyte)theOp.ValueBytes[0];
                    break;
                case OpCodes.Blt_Un:
                    //See above : This is unsigned variant
                    ILOffset = Utilities.ReadInt32(theOp.ValueBytes, 0);
                    break;
                case OpCodes.Blt_Un_S:
                    //See above
                    ILOffset = (int)(sbyte)theOp.ValueBytes[0];
                    break;

                case OpCodes.Bgt:
                    //See above
                    ILOffset = Utilities.ReadInt32(theOp.ValueBytes, 0);
                    break;
                case OpCodes.Bgt_S:
                    //See above
                    ILOffset = (int)(sbyte)theOp.ValueBytes[0];
                    break;
                case OpCodes.Bgt_Un:
                    //See above : This is unsigned variant
                    ILOffset = Utilities.ReadInt32(theOp.ValueBytes, 0);
                    break;
                case OpCodes.Bgt_Un_S:
                    //See above
                    ILOffset = (int)(sbyte)theOp.ValueBytes[0];
                    break;
            }

            if (ILOffset != 0)
            {
                //Get the IL number of the next op
                int startILNum = theOp.NextOffset;
                //Add the offset to get the IL op num to jump to
                int ILNumToGoTo = startILNum + ILOffset;

                //Find the IL op to jump to 
                ILOp opToGoTo = preprocessState.Input.At(ILNumToGoTo);
                
                //Mark it as requiring a label
                opToGoTo.LabelRequired = true;
            }
        }

        /// <summary>
        /// See base class documentation.
        /// </summary>
        /// <param name="theOp">See base class documentation.</param>
        /// <param name="conversionState">See base class documentation.</param>
        /// <returns>See base class documentation.</returns>
        /// <exception cref="System.NotSupportedException">
        /// Thrown if branch test operand value is a floating point value.
        /// </exception>
        public override void Convert(ILConversionState conversionState, ILOp theOp)
        {
            //This will store the offset from the current next op's position
            //to the IL op to branch to.
            int ILOffset = 0;
            //This will store the branch operation - by default, the basic branch op
            ASMOps.BranchOp branchOp = ASMOps.BranchOp.Branch;
            bool UnsignedTest = false;

            ASMOps.BranchOp inverseBranchOp = ASMOps.BranchOp.None;
            bool isNegativeTest = false;

            //The value for the branch op to test against - currently always "0" since on jnz and jz ops are used.
            string testVal = "0";
            
            switch ((OpCodes)theOp.opCode.Value)
            {
                case OpCodes.Br:
                    //Load the IL offset as signed Int 32 from the value bytes.
                    ILOffset = Utilities.ReadInt32(theOp.ValueBytes, 0);
                    break;
                case OpCodes.Br_S:
                    //Load the IL offset as signed Int 8 from the value bytes.
                    ILOffset = (int)(sbyte)theOp.ValueBytes[0];
                    break;
                case OpCodes.Brtrue:
                    //See above.
                    ILOffset = Utilities.ReadInt32(theOp.ValueBytes, 0);
                    //Branch-if-true means we want to do the branch if the operand is not equal to 0
                    //i.e. not false
                    //Set the branch op to jnz - Branch if Not Zero
                    branchOp = ASMOps.BranchOp.BranchNotZero;
                    break;
                case OpCodes.Brtrue_S:
                    //See above
                    ILOffset = (int)(sbyte)theOp.ValueBytes[0];
                    //See above
                    branchOp = ASMOps.BranchOp.BranchNotZero;
                    break;
                case OpCodes.Brfalse:
                    //See above
                    ILOffset = Utilities.ReadInt32(theOp.ValueBytes, 0);
                    //Branch-if-true means we want to do the branch if the operand is equal to 0
                    //i.e. is false
                    //Set the branch op to jz - Branch if Zero
                    branchOp = ASMOps.BranchOp.BranchZero;
                    break;
                case OpCodes.Brfalse_S:
                    //See above
                    ILOffset = (int)(sbyte)theOp.ValueBytes[0];
                    //See above
                    branchOp = ASMOps.BranchOp.BranchZero;
                    break;

                case OpCodes.Beq:
                    //See above
                    ILOffset = Utilities.ReadInt32(theOp.ValueBytes, 0);
                    //Branch-if-equal means we want to do the branch if the operand is equal to the other operand
                    //i.e. A == B
                    //Set the branch op to je - Branch if equal
                    branchOp = ASMOps.BranchOp.BranchEqual;
                    inverseBranchOp = ASMOps.BranchOp.BranchNotEqual;
                    break;
                case OpCodes.Beq_S:
                    //See above
                    ILOffset = (int)(sbyte)theOp.ValueBytes[0];
                    //See above
                    branchOp = ASMOps.BranchOp.BranchEqual;
                    inverseBranchOp = ASMOps.BranchOp.BranchNotEqual;
                    break;

                case OpCodes.Bne_Un:
                    //See above
                    ILOffset = Utilities.ReadInt32(theOp.ValueBytes, 0);
                    //Branch-if-equal means we want to do the branch if the operand is equal to the other operand
                    //i.e. A == B
                    //Set the branch op to je - Branch if not equal
                    branchOp = ASMOps.BranchOp.BranchNotEqual;
                    isNegativeTest = true;
                    break;
                case OpCodes.Bne_Un_S:
                    //See above
                    ILOffset = (int)(sbyte)theOp.ValueBytes[0];
                    //See above
                    branchOp = ASMOps.BranchOp.BranchNotEqual;
                    isNegativeTest = true;
                    break;

                case OpCodes.Bge:
                    //See above
                    ILOffset = Utilities.ReadInt32(theOp.ValueBytes, 0);
                    //Branch-if-greater-than-or-equal means we want to do the branch if the operand is greater than equal to 
                    //the other operand
                    //i.e. A >= B
                    //Set the branch op to jge - Branch if greater than or equal
                    branchOp = ASMOps.BranchOp.BranchGreaterThanEqual;
                    inverseBranchOp = ASMOps.BranchOp.BranchLessThan;
                    break;
                case OpCodes.Bge_S:
                    //See above
                    ILOffset = (int)(sbyte)theOp.ValueBytes[0];
                    //See above
                    branchOp = ASMOps.BranchOp.BranchGreaterThanEqual;
                    inverseBranchOp = ASMOps.BranchOp.BranchLessThan;
                    break;
                case OpCodes.Bge_Un:
                    //See above : This is unsigned variant
                    ILOffset = Utilities.ReadInt32(theOp.ValueBytes, 0);
                    UnsignedTest = true;
                    //See above
                    branchOp = ASMOps.BranchOp.BranchGreaterThanEqual;
                    inverseBranchOp = ASMOps.BranchOp.BranchLessThan;
                    break;
                case OpCodes.Bge_Un_S:
                    //See above
                    ILOffset = (int)(sbyte)theOp.ValueBytes[0];
                    UnsignedTest = true;
                    //See above
                    branchOp = ASMOps.BranchOp.BranchGreaterThanEqual;
                    inverseBranchOp = ASMOps.BranchOp.BranchLessThan;
                    break;

                case OpCodes.Ble:
                    //See above
                    ILOffset = Utilities.ReadInt32(theOp.ValueBytes, 0);
                    //Branch-if-less-than-or-equal means we want to do the branch if the operand is less than equal to 
                    //the other operand
                    //i.e. A <= B
                    //Set the branch op to jle - Branch if less than or equal
                    branchOp = ASMOps.BranchOp.BranchLessThanEqual;
                    inverseBranchOp = ASMOps.BranchOp.BranchGreaterThan;
                    break;
                case OpCodes.Ble_S:
                    //See above
                    ILOffset = (int)(sbyte)theOp.ValueBytes[0];
                    //See above
                    branchOp = ASMOps.BranchOp.BranchLessThanEqual;
                    inverseBranchOp = ASMOps.BranchOp.BranchGreaterThan;
                    break;
                case OpCodes.Ble_Un:
                    //See above : This is unsigned variant
                    ILOffset = Utilities.ReadInt32(theOp.ValueBytes, 0);
                    UnsignedTest = true;
                    //See above
                    branchOp = ASMOps.BranchOp.BranchLessThanEqual;
                    inverseBranchOp = ASMOps.BranchOp.BranchGreaterThan;
                    break;
                case OpCodes.Ble_Un_S:
                    //See above
                    ILOffset = (int)(sbyte)theOp.ValueBytes[0];
                    UnsignedTest = true;
                    //See above
                    branchOp = ASMOps.BranchOp.BranchLessThanEqual;
                    inverseBranchOp = ASMOps.BranchOp.BranchGreaterThan;
                    break;

                case OpCodes.Blt:
                    //See above
                    ILOffset = Utilities.ReadInt32(theOp.ValueBytes, 0);
                    //Branch-if-less-than means we want to do the branch if the operand is less than
                    //the other operand
                    //i.e. A < B
                    //Set the branch op to jl - Branch if less than
                    branchOp = ASMOps.BranchOp.BranchLessThan;
                    isNegativeTest = true;
                    break;
                case OpCodes.Blt_S:
                    //See above
                    ILOffset = (int)(sbyte)theOp.ValueBytes[0];
                    //See above
                    branchOp = ASMOps.BranchOp.BranchLessThan;
                    isNegativeTest = true;
                    break;
                case OpCodes.Blt_Un:
                    //See above : This is unsigned variant
                    ILOffset = Utilities.ReadInt32(theOp.ValueBytes, 0);
                    UnsignedTest = true;
                    //See above
                    branchOp = ASMOps.BranchOp.BranchLessThan;
                    isNegativeTest = true;
                    break;
                case OpCodes.Blt_Un_S:
                    //See above
                    ILOffset = (int)(sbyte)theOp.ValueBytes[0];
                    UnsignedTest = true;
                    //See above
                    branchOp = ASMOps.BranchOp.BranchLessThan;
                    isNegativeTest = true;
                    break;

                case OpCodes.Bgt:
                    //See above
                    ILOffset = Utilities.ReadInt32(theOp.ValueBytes, 0);
                    //Branch-if-greater-than means we want to do the branch if the operand is greater than
                    //the other operand
                    //i.e. A > B
                    //Set the branch op to jg - Branch if greater than
                    branchOp = ASMOps.BranchOp.BranchGreaterThan;
                    isNegativeTest = true;
                    break;
                case OpCodes.Bgt_S:
                    //See above
                    ILOffset = (int)(sbyte)theOp.ValueBytes[0];
                    //See above
                    branchOp = ASMOps.BranchOp.BranchGreaterThan;
                    isNegativeTest = true;
                    break;
                case OpCodes.Bgt_Un:
                    //See above : This is unsigned variant
                    ILOffset = Utilities.ReadInt32(theOp.ValueBytes, 0);
                    UnsignedTest = true;
                    //See above
                    branchOp = ASMOps.BranchOp.BranchGreaterThan;
                    isNegativeTest = true;
                    break;
                case OpCodes.Bgt_Un_S:
                    //See above
                    ILOffset = (int)(sbyte)theOp.ValueBytes[0];
                    UnsignedTest = true;
                    //See above
                    branchOp = ASMOps.BranchOp.BranchGreaterThan;
                    isNegativeTest = true;
                    break;

            }

            if (ILOffset == 0)
            {
                //Err..why bother jumping at all if the offset is 0?
                conversionState.Append(new ASMOps.Comment("No jump insert - pointless 0 distance jump"));
            }
            else
            {
                //Get the IL number of the next op
                int startILNum = theOp.NextOffset;
                //Add the offset to get the IL op num to jump to
                int ILNumToGoTo = startILNum + ILOffset;
                
                //Find the IL op to jump to 
                ILOp opToGoTo = conversionState.Input.At(ILNumToGoTo);
                int opToGoToPosition = conversionState.PositionOf(opToGoTo);
                int currOpPosition = conversionState.PositionOf(theOp);

                //If the jump op is not a straightforward jump i.e. has one or more conditions
                if (branchOp == ASMOps.BranchOp.BranchZero || branchOp == ASMOps.BranchOp.BranchNotZero)
                {
                    //Pop from our stack the test item to use in the condition
                    StackItem testItem = conversionState.CurrentStackFrame.Stack.Pop();
                    
                    if (testItem.isFloat)
                    {
                        //SUPPORT - floats
                        throw new NotSupportedException("Branch test based on float not supported!");
                    }
                    //If the test item is Int64 or UInt64
                    else if (testItem.sizeOnStackInBytes == 8)
                    {
                        //Compare first 32 bits (low bits)
                        //Then (if necessary) compare second 32 bits (high bits)
                        
                        //Pop the low bits
                        conversionState.Append(new ASMOps.Pop() { Size = ASMOps.OperandSize.Word, Dest = "$t0" });
                        //Pop the high bits
                        conversionState.Append(new ASMOps.Pop() { Size = ASMOps.OperandSize.Word, Dest = "$t1" });
                            
                        //If we are testing for not equal to zero:
                        if (branchOp == ASMOps.BranchOp.BranchNotZero)
                        {
                            //If the low bits are not zero, do the jump
                            conversionState.Append(new ASMOps.Branch() { BranchType = branchOp, Src2 = testVal, Src1 = "$t0", DestILPosition = opToGoToPosition });
                            //If the high bits are not zero, do the jump
                            conversionState.Append(new ASMOps.Branch() { BranchType = branchOp, Src1 = "$t1", Src2 = testVal, DestILPosition = opToGoToPosition });
                        }
                        //If we are testing for equal to zero:
                        else if (branchOp == ASMOps.BranchOp.BranchZero)
                        {
                            //If the low bits are not zero, jump to the end of these tests as condition has not been met
                            conversionState.Append(new ASMOps.Branch() { BranchType = ASMOps.BranchOp.BranchNotZero, Src1 = "$t0", DestILPosition = opToGoToPosition, Extension = "End" });
                            //If the high bits are zero, do the jump
                            conversionState.Append(new ASMOps.Branch() { BranchType = branchOp, Src1 = "$t1", DestILPosition = opToGoToPosition });
                            //Insert the end label to be jumped to if condition is not met (see above)
                            conversionState.Append(new ASMOps.Label() { ILPosition = currOpPosition, Extension = "End" });
                        }
                        else
                        {
                            //Check that no extra ops have been added in the switch block above!
                            throw new NotSupportedException("Unsupported 64-bit branch op!");
                        }
                    }
                    //If the test item is Int32 or UInt32
                    else if (testItem.sizeOnStackInBytes == 4)
                    {
                        //Pop the test item
                        conversionState.Append(new ASMOps.Pop() { Size = ASMOps.OperandSize.Word, Dest = "$t0" });
                        //Do the specified jump
                        conversionState.Append(new ASMOps.Branch() { BranchType = branchOp, Src1 = "$t0", Src2 = testVal, DestILPosition = opToGoToPosition });
                    }
                    else
                    {
                        throw new InvalidOperationException("Invalid stack operand sizes!");
                    }
                }
                else if (branchOp == ASMOps.BranchOp.BranchEqual || branchOp == ASMOps.BranchOp.BranchNotEqual ||
                         branchOp == ASMOps.BranchOp.BranchGreaterThanEqual ||
                         branchOp == ASMOps.BranchOp.BranchLessThanEqual ||
                         branchOp == ASMOps.BranchOp.BranchLessThan ||
                         branchOp == ASMOps.BranchOp.BranchGreaterThan)
                {
                    //Pop from our stack the test items to use in the condition
                    StackItem itemB = conversionState.CurrentStackFrame.Stack.Pop();
                    StackItem itemA = conversionState.CurrentStackFrame.Stack.Pop();

                    if (itemA.isFloat || itemB.isFloat)
                    {
                        //SUPPORT - floats
                        throw new NotSupportedException("Branch test based on float not supported!");
                    }
                    else if(itemA.sizeOnStackInBytes != itemB.sizeOnStackInBytes)
                    {
                        throw new InvalidOperationException("Branch test operands must be same size!");
                    }
                    else if (itemA.sizeOnStackInBytes == 8)
                    {
                        //Pop the test item B
                        conversionState.Append(new ASMOps.Pop() { Size = ASMOps.OperandSize.Word, Dest = "$t1" });
                        conversionState.Append(new ASMOps.Pop() { Size = ASMOps.OperandSize.Word, Dest = "$t2" });
                        //Pop the test item A
                        conversionState.Append(new ASMOps.Pop() { Size = ASMOps.OperandSize.Word, Dest = "$t0" });
                        conversionState.Append(new ASMOps.Pop() { Size = ASMOps.OperandSize.Word, Dest = "$t3" });

                        if(!isNegativeTest)
                        {
                            //Compare test item A high bits to test item B high bits
                            //If they are not equal, abort the testing
                            conversionState.Append(new ASMOps.Branch() { BranchType = inverseBranchOp, Src1 = "$t3", Src2 = "$t2", DestILPosition = currOpPosition, Extension = "End", UnsignedTest = UnsignedTest });

                            //Else the igh bits are equal so test low bits
                            //Compare test item A low bits to test item B low bits
                            //Do the specified jump
                            conversionState.Append(new ASMOps.Branch() { BranchType = branchOp, Src1 = "$t1", Src2 = "$t0", DestILPosition = opToGoToPosition, UnsignedTest = UnsignedTest });

                            //Insert the end label to be jumped to if condition is not met (see above)
                            conversionState.Append(new ASMOps.Label() { ILPosition = currOpPosition, Extension = "End" });
                        }
                        else
                        {
                            //Compare test item A high bits to test item B high bits
                            //Do the specified jump
                            conversionState.Append(new ASMOps.Branch() { BranchType = branchOp, Src1 = "$t3", Src2 = "$t2", DestILPosition = opToGoToPosition, UnsignedTest = UnsignedTest });
                            //Compare test item A low bits to test item B low bits
                            //Do the specified jump
                            conversionState.Append(new ASMOps.Branch() { BranchType = branchOp, Src1 = "$t1", Src2 = "$t0", DestILPosition = opToGoToPosition, UnsignedTest = UnsignedTest });
                        }
                    }
                    else if (itemA.sizeOnStackInBytes == 4)
                    {
                        //Pop the test item B
                        conversionState.Append(new ASMOps.Pop() { Size = ASMOps.OperandSize.Word, Dest = "$t1" });
                        //Pop the test item A
                        conversionState.Append(new ASMOps.Pop() { Size = ASMOps.OperandSize.Word, Dest = "$t0" });
                        //Compare test item A to test item B
                        //Do the specified jump
                        conversionState.Append(new ASMOps.Branch() { BranchType = branchOp, Src1 = "$t0", Src2 = "$t1", DestILPosition = opToGoToPosition, UnsignedTest = UnsignedTest });
                    }
                    else
                    {
                        throw new NotSupportedException("Branch test based on supplied stack item sizes not supported!");
                    }
                }
                else
                {
                    //Do the straightforward jump...
                    conversionState.Append(new ASMOps.Branch() { BranchType = branchOp, DestILPosition = opToGoToPosition });
                }
            }
        }
    }
}
