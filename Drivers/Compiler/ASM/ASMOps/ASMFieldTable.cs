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

namespace Drivers.Compiler.ASM.ASMOps
{
    [ASMOpTarget(Target = OpCodes.FieldTable)]
    public abstract class ASMFieldTable : ASMOp
    {
        public string CurrentTypeId;
        public string CurrentTypeName;
        public List<Tuple<string, string, string>> AllFieldInfos;
        public List<Tuple<string, int>> TableEntryFieldInfos;

        public ASMFieldTable(string currentTypeId, string currentTypeName, List<Tuple<string, string, string>> allFieldInfos, List<Tuple<string, int>> tableEntryFieldInfos)
        {
            CurrentTypeId = currentTypeId;
            CurrentTypeName = currentTypeName;
            AllFieldInfos = allFieldInfos;
            TableEntryFieldInfos = tableEntryFieldInfos;
        }
    }
}
