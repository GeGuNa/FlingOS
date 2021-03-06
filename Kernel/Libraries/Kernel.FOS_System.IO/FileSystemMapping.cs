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

namespace Kernel.FOS_System.IO
{
    /// <summary>
    /// Represents a file system mapping. A file system 
    /// mapping maps a path prefix (e.g. A:/) to a particular
    /// file system.
    /// 
    /// </summary>
    public class FileSystemMapping : FOS_System.Object
    {
        /// <summary>
        /// The prefix to map. This must be unique.
        /// </summary>
        protected FOS_System.String prefix;
        /// <summary>
        /// The prefix to map. This must be unique.
        /// </summary>
        public FOS_System.String Prefix
        {
            get
            {
                return prefix;
            }
            set
            {
                prefix = value.ToUpper();
            }
        }

        /// <summary>
        /// The file system to map.
        /// </summary>
        protected FileSystem theFileSystem;
        /// <summary>
        /// The file system to map.
        /// </summary>
        public FileSystem TheFileSystem
        {
            get
            {
                return theFileSystem;
            }
        }

        /// <summary>
        /// Initializes a new file system mapping.
        /// </summary>
        /// <param name="aPrefix">The prefix to map.</param>
        /// <param name="aFileSystem">The file system to map.</param>
        public FileSystemMapping(FOS_System.String aPrefix, FileSystem aFileSystem)
        {
            prefix = aPrefix;
            theFileSystem = aFileSystem;
        }

        /// <summary>
        /// Determines whether the specified path starts with this
        /// mapping's prefix.
        /// </summary>
        /// <param name="aPath">The path to check.</param>
        /// <returns>
        /// Whether the specified path starts with this
        /// mapping's prefix.
        /// </returns>
        public bool PathMatchesMapping(FOS_System.String aPath)
        {
            return aPath.ToUpper().StartsWith(prefix);
        }
        /// <summary>
        /// Removes the mapping's prefix from the specified path.
        /// </summary>
        /// <param name="aPath">The path to remove the prefix from.</param>
        /// <returns>The path without the prefix.</returns>
        public FOS_System.String RemoveMappingPrefix(FOS_System.String aPath)
        {
            return aPath.Substring(prefix.length, aPath.length - prefix.length);
        }
    }
}
