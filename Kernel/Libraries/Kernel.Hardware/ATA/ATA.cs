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

namespace Kernel.Hardware.ATA
{
    /// <summary>
    /// Represents an ATA device.
    /// </summary>
    public abstract class ATA : Devices.DiskDevice
    {
        /// <summary>
        /// The ATA controller IDs.
        /// </summary>
        public enum ControllerID
        { 
            /// <summary>
            /// Primary ATA controller.
            /// </summary>
            Primary,
            /// <summary>
            /// Secondary ATA controller.
            /// </summary>
            Secondary 
        }
        /// <summary>
        /// The ATA bus positions.
        /// </summary>
        public enum BusPosition
        { 
            /// <summary>
            /// Master device.
            /// </summary>
            Master,
            /// <summary>
            /// Slave device.
            /// </summary>
            Slave 
        }

        /// <summary>
        /// The device's controller ID.
        /// </summary>
        public ControllerID controllerId;
        /// <summary>
        /// The device's bus position.
        /// </summary>
        public BusPosition busPosition;

        /// <summary>
        /// Initialises a new ATA device with block size 512.
        /// </summary>
        internal ATA()
        {
            blockSize = 512;
        }
    }
}
