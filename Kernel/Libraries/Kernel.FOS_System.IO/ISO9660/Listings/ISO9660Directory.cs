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
    
#define ISO9660DIR_TRACE
#undef ISO9660DIR_TRACE

using System;
using Kernel.FOS_System.Collections;

namespace Kernel.FOS_System.IO.ISO9660
{
    public class ISO9660Directory : Directory
    {
        internal Disk.ISO9660.DirectoryRecord TheDirectoryRecord;

        private ISO9660File _theFile;
        private Streams.ISO9660.ISO9660FileStream _fileStream;

        public ISO9660Directory(ISO9660FileSystem fileSystem, ISO9660Directory parent, Disk.ISO9660.DirectoryRecord record)
            : base(fileSystem, parent, record.FileIdentifier.length > 0 ? (FOS_System.String)record.FileIdentifier.Split(';')[0] : "")
        {
            TheDirectoryRecord = record;

            _theFile = new ISO9660File(fileSystem, parent, record) { IsDirectoryFile = true };
        }

        public override void AddListing(Base aListing)
        {
            ExceptionMethods.Throw(new Exceptions.NotSupportedException("Cannot modify contents of ISO9660 disc (yet)! (Directory)"));
        }
        public override bool Delete()
        {
            ExceptionMethods.Throw(new Exceptions.NotSupportedException("Cannot modify contents of ISO9660 disc (yet)! (Directory)"));
            return false;
        }
        public override String GetFullPath()
        {
            if (TheDirectoryRecord.IsRootDirectory)
            {
                return this.TheFileSystem.TheMapping.Prefix;
            }
            return base.GetFullPath();
        }
        public override Base GetListing(Collections.List nameParts)
        {
            return TheFileSystem.GetListingFromListings(nameParts, Parent, GetListings());
        }
        private List _cachedlistings;
        public override List GetListings()
        {
            if (_cachedlistings == null)
            {
                Get_FileStream();
                byte[] data = new byte[(uint)_theFile.Size];
                _fileStream.Position = 0;
                int actuallyRead = _fileStream.Read(data, 0, (int)data.Length);
                _cachedlistings = new List(10);

                uint position = 0;
                Disk.ISO9660.DirectoryRecord newRecord;
                do
                {
                    newRecord = new Disk.ISO9660.DirectoryRecord(data, position);
#if ISO9660DIR_TRACE
                    BasicConsole.WriteLine(newRecord.ConvertToString());
#endif
                    if (newRecord.RecordLength > 0)
                    {
                        if ((newRecord.TheFileFlags & Disk.ISO9660.DirectoryRecord.FileFlags.Directory) != 0)
                        {
                            // Directory
                            _cachedlistings.Add(new ISO9660Directory((ISO9660FileSystem)TheFileSystem, this, newRecord));
                        }
                        else
                        {
                            // File
                            _cachedlistings.Add(new ISO9660File((ISO9660FileSystem)TheFileSystem, this, newRecord));
                        }

                        position += newRecord.RecordLength;
                    }
                }
                while (position < data.Length && newRecord.RecordLength > 0);
            }
            return _cachedlistings;
        }
        private void Get_FileStream()
        {
            if (_fileStream == null)
            {
                _fileStream = (Streams.ISO9660.ISO9660FileStream)_theFile.GetStream();
            }
        }
        public override void RemoveListing(Base aListing)
        {
            ExceptionMethods.Throw(new Exceptions.NotSupportedException("Cannot modify contents of ISO9660 disc (yet)! (Directory)"));
        }
        public override void WriteListings()
        {
            ExceptionMethods.Throw(new Exceptions.NotSupportedException("Cannot modify contents of ISO9660 disc (yet)! (Directory)"));
        }
    }
}
