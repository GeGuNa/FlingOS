﻿#region LICENSE
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
    /// <summary>
    /// See base class documentation.
    /// </summary>
    public class Ldarg : IL.ILOps.Ldarg
    {
        public override void PerformStackOperations(ILPreprocessState conversionState, ILOp theOp)
        {
            Int16 index = 0;
            switch ((OpCodes)theOp.opCode.Value)
            {
                case OpCodes.Ldarg:
                    index = Utilities.ReadInt16(theOp.ValueBytes, 0);
                    break;
                case OpCodes.Ldarg_0:
                    index = 0;
                    break;
                case OpCodes.Ldarg_1:
                    index = 1;
                    break;
                case OpCodes.Ldarg_2:
                    index = 2;
                    break;
                case OpCodes.Ldarg_3:
                    index = 3;
                    break;
                case OpCodes.Ldarg_S:
                    index = (Int16)theOp.ValueBytes[0];
                    break;
                case OpCodes.Ldarga:
                    index = Utilities.ReadInt16(theOp.ValueBytes, 0);
                    break;
                case OpCodes.Ldarga_S:
                    index = (Int16)theOp.ValueBytes[0];
                    break;
            }

            List<Type> allParams = conversionState.Input.TheMethodInfo.UnderlyingInfo.GetParameters().Select(x => x.ParameterType).ToList();
            if (!conversionState.Input.TheMethodInfo.IsStatic)
            {
                allParams.Insert(0, conversionState.Input.TheMethodInfo.UnderlyingInfo.DeclaringType);
            }
            
            if ((OpCodes)theOp.opCode.Value == OpCodes.Ldarga ||
                (OpCodes)theOp.opCode.Value == OpCodes.Ldarga_S)
            {
                conversionState.CurrentStackFrame.Stack.Push(new StackItem()
                {
                    sizeOnStackInBytes = 4,
                    isFloat = false,
                    isGCManaged = false
                });
            }
            else
            {
                Types.TypeInfo paramTypeInfo = conversionState.TheILLibrary.GetTypeInfo(allParams[index]);
                int bytesForArg = paramTypeInfo.SizeOnStackInBytes;
                conversionState.CurrentStackFrame.Stack.Push(new StackItem()
                {
                    sizeOnStackInBytes = bytesForArg,
                    isFloat = false,
                    isGCManaged = paramTypeInfo.IsGCManaged
                });
            }
        }

        /// <summary>
        /// See base class documentation.
        /// <para>To Do's:</para>
        /// <list type="bullet">
        /// <item>
        /// <term>To do</term>
        /// <description>Implement loading of float arguments.</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="theOp">See base class documentation.</param>
        /// <param name="conversionState">See base class documentation.</param>
        /// <returns>See base class documentation.</returns>
        /// <exception cref="System.NotSupportedException">
        /// Thrown when loading a float argument is required as it currently hasn't been
        /// implemented.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// Thrown when an invalid number of bytes is specified for the argument to load.
        /// </exception>
        public override void Convert(ILConversionState conversionState, ILOp theOp)
        {
            //Get the index of the argument to load
            Int16 index = 0;
            switch ((OpCodes)theOp.opCode.Value)
            {
                case OpCodes.Ldarg:
                    index = Utilities.ReadInt16(theOp.ValueBytes, 0);
                    break;
                case OpCodes.Ldarg_0:
                    index = 0;
                    break;
                case OpCodes.Ldarg_1:
                    index = 1;
                    break;
                case OpCodes.Ldarg_2:
                    index = 2;
                    break;
                case OpCodes.Ldarg_3:
                    index = 3;
                    break;
                case OpCodes.Ldarg_S:
                    index = (Int16)theOp.ValueBytes[0];
                    break;
                case OpCodes.Ldarga:
                    index = Utilities.ReadInt16(theOp.ValueBytes, 0);
                    break;
                case OpCodes.Ldarga_S:
                    index = (Int16)theOp.ValueBytes[0];
                    break;
            }

            //Used to store the number of bytes to add to EBP to get to the arg
            int BytesOffsetFromEBP = 0;
            //Get all the params for the current method
            List<Type> allParams = conversionState.Input.TheMethodInfo.UnderlyingInfo.GetParameters().Select(x => x.ParameterType).ToList();
            if(!conversionState.Input.TheMethodInfo.IsStatic)
            {
                allParams.Insert(0, conversionState.Input.TheMethodInfo.UnderlyingInfo.DeclaringType);
            }
            //Check whether the arg we are going to load is float or not
            if (Utilities.IsFloat(allParams[index]))
            {
                //SUPPORT - floats
                throw new NotSupportedException("Float arguments not supported yet!");
            }
            //For all the parameters pushed on the stack after the param we want
            for (int i = allParams.Count - 1; i > -1 && i > index; i--)
            {
                //Add the param stack size to the EBP offset
                BytesOffsetFromEBP += conversionState.TheILLibrary.GetTypeInfo(allParams[i]).SizeOnStackInBytes;
            }

            //Add 8 for return address and value of EBP (both pushed at start of method call)
            BytesOffsetFromEBP += 8;

            //We must check the return value to see if it has a size on the stack
            //Get the return type
            Type retType = (conversionState.Input.TheMethodInfo.IsConstructor ?
                    typeof(void) : ((MethodInfo)conversionState.Input.TheMethodInfo.UnderlyingInfo).ReturnType);
            //Get the size of the return type
            int retSize = conversionState.TheILLibrary.GetTypeInfo(retType).SizeOnStackInBytes;
            //Add it to EBP offset
            BytesOffsetFromEBP += retSize;

            if ((OpCodes)theOp.opCode.Value == OpCodes.Ldarga ||
                (OpCodes)theOp.opCode.Value == OpCodes.Ldarga_S)
            {
                //Push the address of the argument onto the stack

                conversionState.Append(new ASMOps.Mov() { Size = ASMOps.OperandSize.Word, Src = "$fp", Dest = "$t2" });
                conversionState.Append(new ASMOps.Add() { Src2 = BytesOffsetFromEBP.ToString(), Src1 = "$t2", Dest = "$t2" });
                conversionState.Append(new ASMOps.Push() { Size = ASMOps.OperandSize.Word, Src = "$t2" });

                //Push the address onto our stack
                conversionState.CurrentStackFrame.Stack.Push(new StackItem()
                {
                    sizeOnStackInBytes = 4,
                    isFloat = false,
                    isGCManaged = false
                });
            }
            else
            {
                //Push the argument onto the stack
                Types.TypeInfo paramTypeInfo = conversionState.TheILLibrary.GetTypeInfo(allParams[index]);
                int bytesForArg = paramTypeInfo.SizeOnStackInBytes;
                if (bytesForArg == 4)
                {
                    conversionState.Append(new ASMOps.Mov() { Size = ASMOps.OperandSize.Word, Src = BytesOffsetFromEBP.ToString() + "($fp)", Dest = "$t0", MoveType = ASMOps.Mov.MoveTypes.SrcMemoryToDestReg });
                    conversionState.Append(new ASMOps.Push() { Size = ASMOps.OperandSize.Word, Src = "$t0" });
                }
                else if (bytesForArg == 8)
                {
                    conversionState.Append(new ASMOps.Mov() { Size = ASMOps.OperandSize.Word, Src = (BytesOffsetFromEBP + 4).ToString() + "($fp)", Dest = "$t0", MoveType = ASMOps.Mov.MoveTypes.SrcMemoryToDestReg });
                    conversionState.Append(new ASMOps.Push() { Size = ASMOps.OperandSize.Word, Src = "$t0" });
                    conversionState.Append(new ASMOps.Mov() { Size = ASMOps.OperandSize.Word, Src = BytesOffsetFromEBP.ToString() + "($fp)", Dest = "$t0", MoveType = ASMOps.Mov.MoveTypes.SrcMemoryToDestReg });
                    conversionState.Append(new ASMOps.Push() { Size = ASMOps.OperandSize.Word, Src = "$t0" });
                }
                else
                {
                    throw new ArgumentException("Cannot load arg! Don't understand byte size of the arg!");
                }

                //Push the arg onto our stack
                conversionState.CurrentStackFrame.Stack.Push(new StackItem()
                {
                    sizeOnStackInBytes = bytesForArg,
                    isFloat = false,
                    isGCManaged = paramTypeInfo.IsGCManaged
                });
            }
        }
    }
}
