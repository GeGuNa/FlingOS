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

namespace Drivers.Compiler.Architectures.x86.ASMOps
{
    public class Shr : ASM.ASMOp
    {
        public string Src;
        public string Dest;
        /// <summary>
        /// Optional. Cannot be used with Signed.
        /// Set to use Extended Shift (see http://en.wikibooks.org/wiki/X86_Assembly/Shift_and_Rotate#Extended_Shift_Instructions)
        /// </summary>
        public string Count;
        /// <summary>
        /// Optional. Cannot be used with Count.
        /// </summary>
        public bool Signed = false;
        
        public override string Convert(ASM.ASMBlock theBlock)
        {
            if (!string.IsNullOrWhiteSpace(Count))
            {
                return "shrd " + Dest + ", " + Src + ", " + Count;
            }
            else if (Signed)
            {
                return "sar " + Dest + ", " + Src;
            }
            else
            {
                return "shr " + Dest + ", " + Src;
            }
        }
    }
}
