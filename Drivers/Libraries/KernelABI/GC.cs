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

namespace KernelABI
{
    public static class GC
    {
        [Drivers.Compiler.Attributes.NoDebug]
        [Drivers.Compiler.Attributes.NoGC]
        [Drivers.Compiler.Attributes.IncrementRefCountMethod]
        public static void IncrementRefCount(Drivers.Framework.Object anObj)
        {
        }
        [Drivers.Compiler.Attributes.NoDebug]
        [Drivers.Compiler.Attributes.NoGC]
        [Drivers.Compiler.Attributes.DecrementRefCountMethod]
        public static void DecrementRefCount(Drivers.Framework.Object anObj)
        {
        }
    }
}
