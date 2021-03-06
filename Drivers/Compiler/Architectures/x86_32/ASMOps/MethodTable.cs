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
using Drivers.Compiler;

namespace Drivers.Compiler.Architectures.x86.ASMOps
{
    public class MethodTable : ASM.ASMOps.ASMMethodTable
    {
        public MethodTable(string currentTypeId, string currentTypeName, List<Tuple<string, string>> allMethodInfos, List<Tuple<string, int>> tableEntryFieldInfos)
            : base(currentTypeId, currentTypeName, allMethodInfos, tableEntryFieldInfos)
        {
        }

        public override string Convert(ASM.ASMBlock theBlock)
        {
            StringBuilder ASMResult = new StringBuilder();
            ASMResult.AppendLine("; Method Table - " + CurrentTypeName);
            ASMResult.AppendLine("GLOBAL " + CurrentTypeId + "_MethodTable:data");
            ASMResult.AppendLine(CurrentTypeId + "_MethodTable:");

            foreach (Tuple<string, string> aMethodInfo in AllMethodInfos)
            {
                string MethodID = aMethodInfo.Item1;
                string MethodIDValue = aMethodInfo.Item2;

                foreach (Tuple<string, int> anEntryFieldInfo in TableEntryFieldInfos)
                {
                    string allocString = ASMUtilities.GetAllocStringForSize(anEntryFieldInfo.Item2);
                    switch(anEntryFieldInfo.Item1)
                    {
                        case "MethodID":
                            ASMResult.AppendLine(allocString + " " + MethodIDValue);
                            break;
                        case "MethodPtr":
                            ASMResult.AppendLine(allocString + " " + MethodID);
                            break;
                    }
                }
            }

            ASMResult.AppendLine("; Method Table End - " + CurrentTypeName);
            
            return ASMResult.ToString();
        }
    }
}
