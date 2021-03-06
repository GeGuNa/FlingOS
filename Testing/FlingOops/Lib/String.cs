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

namespace FlingOops
{
    /// <summary>
    /// Replacement class for methods, properties and fields usually found on standard System.String type.
    /// Also contains utility methods for low-level string manipulation.
    /// </summary>
    [Drivers.Compiler.Attributes.StringClass]
    public sealed class String : Object
    {
        /* If you add more fields here, remember to update the compiler and all the ASM files that depend on the string
           class structure ( i.e. do all the hard work! ;) )
         */

        /// <summary>
        /// The size of the fields in an string object that come before the actual string data.
        /// </summary>
        public const uint FieldsBytesSize = 8;

        /// <summary>
        /// The length of the string.
        /// </summary>
        public int length;

        /*   ----------- DO NOT CREATE A CONSTRUCTOR FOR THIS CLASS - IT WILL NEVER BE CALLED IF YOU DO ----------- */
        
        /// <summary>
        /// Creates a new, blank FlingOops.String of specified length.
        /// IMPORTANT NOTE: You MUST assign the return value of this to a variable / local / arg / 
        /// field etc. You may not use IL or C# that results in an IL Pop op of the return value
        /// of this method as it will screw up the GC RefCount handling.
        /// </summary>
        /// <param name="length">The length of the string to create.</param>
        /// <returns>The new string.</returns>
        [Drivers.Compiler.Attributes.NoGC]
        [Drivers.Compiler.Attributes.NoDebug]
        public static unsafe FlingOops.String New(int length)
        {
            if (length < 0)
            {
                BasicConsole.WriteLine("ArgumentException! String.New, length less than zero.");
                ExceptionMethods.Throw(new Exception("Parameter \"length\" cannot be less than 0 in FlingOops.String.New(int length)."));
            }
            FlingOops.String result = (FlingOops.String)Utilities.ObjectUtilities.GetObject(GC.NewString(length));
            if (result == null)
            {
                BasicConsole.WriteLine("NullReferenceException! String.New, result is null.");
                ExceptionMethods.Throw(new Exception());
            }
            return result;
        }

        /// <summary>
        /// Concatenates two strings into one new string.
        /// </summary>
        /// <param name="str1">The first part of the new string.</param>
        /// <param name="str2">The second part of the new string.</param>
        /// <returns>The new string.</returns>
        [Drivers.Compiler.Attributes.NoGC]
        [Drivers.Compiler.Attributes.NoDebug]
        public static unsafe FlingOops.String Concat(FlingOops.String str1, FlingOops.String str2)
        {
            FlingOops.String newStr = New(str1.length + str2.length);

            for (int i = 0; i < str1.length; i++)
            {
                newStr[i] = str1[i];
            }
            for (int i = 0; i < str2.length; i++)
            {
                newStr[i + str1.length] = str2[i];
            }
            return newStr;
        }

        /// <summary>
        /// Gets the character at the specified index.
        /// </summary>
        /// <param name="index">The index of the character to get.</param>
        /// <returns>The character at the specified index.</returns>
        public unsafe char this[int index]
        {
            [Drivers.Compiler.Attributes.NoDebug]
            [Drivers.Compiler.Attributes.NoGC]
            get
            {
                byte* thisPtr = (byte*)Utilities.ObjectUtilities.GetHandle(this);
                thisPtr += 8; /*For fields inc. inherited*/
                return ((char*)thisPtr)[index];
            }
            [Drivers.Compiler.Attributes.NoDebug]
            [Drivers.Compiler.Attributes.NoGC]
            set
            {
                byte* thisPtr = (byte*)Utilities.ObjectUtilities.GetHandle(this);
                thisPtr += 8; /*For fields inc. inherited*/
                ((char*)thisPtr)[index] = value;
            }
        }
        /// <summary>
        /// Gets the character at the specified index.
        /// </summary>
        /// <param name="index">The index of the character to get.</param>
        /// <returns>The character at the specified index.</returns>
        public unsafe char this[uint index]
        {
            [Drivers.Compiler.Attributes.NoDebug]
            [Drivers.Compiler.Attributes.NoGC]
            get
            {
                byte* thisPtr = (byte*)Utilities.ObjectUtilities.GetHandle(this);
                thisPtr += 8; /*For fields inc. inherited*/
                return ((char*)thisPtr)[index];
            }
            [Drivers.Compiler.Attributes.NoDebug]
            [Drivers.Compiler.Attributes.NoGC]
            set
            {
                byte* thisPtr = (byte*)Utilities.ObjectUtilities.GetHandle(this);
                thisPtr += 8; /*For fields inc. inherited*/
                ((char*)thisPtr)[index] = value;
            }
        }

        [Drivers.Compiler.Attributes.NoDebug]
        [Drivers.Compiler.Attributes.NoGC]
        public unsafe char* GetCharPointer()
        {
            return (char*)(((byte*)Utilities.ObjectUtilities.GetHandle(this)) + FieldsBytesSize);
        }

        /// <summary>
        /// Creates a new string and pads the left side of the string with the specified character until the 
        /// whole string is of the specified length or returns the original string if it is longer.
        /// </summary>
        /// <param name="totalLength">The final length of the whole string.</param>
        /// <param name="padChar">The character to pad with.</param>
        /// <returns>The new, padded string.</returns>
        [Drivers.Compiler.Attributes.NoDebug]
        [Drivers.Compiler.Attributes.NoGC]
        public FlingOops.String PadLeft(int totalLength, char padChar)
        {
            FlingOops.String result = New(totalLength);

            if (this.length >= totalLength)
            {
                for (int i = 0; i < result.length; i++)
                {
                    result[i] = this[i];
                }
                return result;
            }

            int offset = totalLength - this.length;
            for (int i = 0; i < this.length; i++)
            {
                result[i + offset] = this[i];
            }
            for (int i = 0; i < offset; i++)
            {
                result[i] = padChar;
            }
            return result;
        }

        /// <summary>
        /// Concatenates two strings using "+" operator.
        /// </summary>
        /// <param name="x">The first string.</param>
        /// <param name="y">The second string.</param>
        /// <returns>The new contenated string.</returns>
        [Drivers.Compiler.Attributes.NoDebug]
        [Drivers.Compiler.Attributes.NoGC]
        public static FlingOops.String operator +(FlingOops.String x, FlingOops.String y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    return null;
                }
                else
                {
                    return y;
                }
            }
            else if (y == null)
            {
                return x;
            }

            return FlingOops.String.Concat(x, y);
        }
        /// <summary>
        /// Tests whether all the characters of two strings are equal.
        /// </summary>
        /// <param name="x">The first string.</param>
        /// <param name="y">The second string.</param>
        /// <returns>Whether the two strings are identical or not.</returns>
        [Drivers.Compiler.Attributes.NoDebug]
        [Drivers.Compiler.Attributes.NoGC]
        public static unsafe bool operator ==(FlingOops.String x, FlingOops.String y)
        {
            bool equal = true;

            //Prevent recursive calls to this "==" implicit method!
            if (Utilities.ObjectUtilities.GetHandle(x) == null ||
                Utilities.ObjectUtilities.GetHandle(y) == null)
            {
                if (Utilities.ObjectUtilities.GetHandle(x) == null &&
                    Utilities.ObjectUtilities.GetHandle(y) == null)
                {
                    return true;
                }
                return false;
            }

            if (x.length != y.length)
            {
                equal = false;
            }
            else
            {
                for (int i = 0; i < x.length; i++)
                {
                    if (x[i] != y[i])
                    {
                        equal = false;
                        break;
                    }
                }
            }

            return equal;
        }
        /// <summary>
        /// Tests whether any of the characters of two strings are not equal.
        /// </summary>
        /// <param name="x">The first string.</param>
        /// <param name="y">The second string.</param>
        /// <returns>Whether the two strings mismatch in any place.</returns>
        [Drivers.Compiler.Attributes.NoDebug]
        [Drivers.Compiler.Attributes.NoGC]
        public static bool operator !=(FlingOops.String x, FlingOops.String y)
        {
            return !(x == y);
        }

        /// <summary>
        /// Implicitly converts the specified value to an FlingOops.String.
        /// </summary>
        /// <param name="x">The value to convert.</param>
        /// <returns>The FlingOops.String value.</returns>
        [Drivers.Compiler.Attributes.NoDebug]
        [Drivers.Compiler.Attributes.NoGC]
        public static implicit operator FlingOops.String(bool x)
        {
            return x ? "True" : "False";
        }
        /// <summary>
        /// Implicitly converts the specified value to an FlingOops.String.
        /// </summary>
        /// <param name="x">The value to convert.</param>
        /// <returns>The FlingOops.String value.</returns>
        [Drivers.Compiler.Attributes.NoDebug]
        [Drivers.Compiler.Attributes.NoGC]
        public static implicit operator FlingOops.String(string x)
        {
            return (FlingOops.String)(object)x;
        }
        /// <summary>
        /// Implicitly converts the specified FlingOops.String to a System.String.
        /// </summary>
        /// <param name="x">The value to convert.</param>
        /// <returns>The System.String.</returns>
        [Drivers.Compiler.Attributes.NoDebug]
        [Drivers.Compiler.Attributes.NoGC]
        public static explicit operator string(FlingOops.String x)
        {
            return (string)(object)x;
        }
        /// <summary>
        /// Implicitly converts the specified value to a hex FlingOops.String.
        /// </summary>
        /// <param name="x">The value to convert.</param>
        /// <returns>The FlingOops.String value.</returns>
        [Drivers.Compiler.Attributes.NoDebug]
        [Drivers.Compiler.Attributes.NoGC]
        public static implicit operator FlingOops.String(byte x)
        {
            FlingOops.String result = "";
            uint y = x;
            while (y > 0)
            {
                uint rem = y % 16u;
                switch (rem)
                {
                    case 0:
                        result = "0" + result;
                        break;
                    case 1:
                        result = "1" + result;
                        break;
                    case 2:
                        result = "2" + result;
                        break;
                    case 3:
                        result = "3" + result;
                        break;
                    case 4:
                        result = "4" + result;
                        break;
                    case 5:
                        result = "5" + result;
                        break;
                    case 6:
                        result = "6" + result;
                        break;
                    case 7:
                        result = "7" + result;
                        break;
                    case 8:
                        result = "8" + result;
                        break;
                    case 9:
                        result = "9" + result;
                        break;
                    case 10:
                        result = "A" + result;
                        break;
                    case 11:
                        result = "B" + result;
                        break;
                    case 12:
                        result = "C" + result;
                        break;
                    case 13:
                        result = "D" + result;
                        break;
                    case 14:
                        result = "E" + result;
                        break;
                    case 15:
                        result = "F" + result;
                        break;
                }
                y = y / 16u;
            }
            result = "0x" + result.PadLeft(2, '0');
            return result;
        }
        /// <summary>
        /// Implicitly converts the specified value to a hex FlingOops.String.
        /// </summary>
        /// <param name="x">The value to convert.</param>
        /// <returns>The FlingOops.String value.</returns>
        [Drivers.Compiler.Attributes.NoDebug]
        [Drivers.Compiler.Attributes.NoGC]
        public static implicit operator FlingOops.String(UInt16 x)
        {
            FlingOops.String result = "";
            uint y = x;
            while (y > 0)
            {
                uint rem = y & 0xFu;
                switch (rem)
                {
                    case 0:
                        result = "0" + result;
                        break;
                    case 1:
                        result = "1" + result;
                        break;
                    case 2:
                        result = "2" + result;
                        break;
                    case 3:
                        result = "3" + result;
                        break;
                    case 4:
                        result = "4" + result;
                        break;
                    case 5:
                        result = "5" + result;
                        break;
                    case 6:
                        result = "6" + result;
                        break;
                    case 7:
                        result = "7" + result;
                        break;
                    case 8:
                        result = "8" + result;
                        break;
                    case 9:
                        result = "9" + result;
                        break;
                    case 10:
                        result = "A" + result;
                        break;
                    case 11:
                        result = "B" + result;
                        break;
                    case 12:
                        result = "C" + result;
                        break;
                    case 13:
                        result = "D" + result;
                        break;
                    case 14:
                        result = "E" + result;
                        break;
                    case 15:
                        result = "F" + result;
                        break;
                }
                y >>= 4;
            }
            return "0x" + result.PadLeft(4, '0');
        }
        /// <summary>
        /// Implicitly converts the specified value to an FlingOops.String.
        /// </summary>
        /// <param name="x">The value to convert.</param>
        /// <returns>The FlingOops.String value.</returns>
        [Drivers.Compiler.Attributes.NoDebug]
        [Drivers.Compiler.Attributes.NoGC]
        public static implicit operator FlingOops.String(char x)
        {
            FlingOops.String result = FlingOops.String.New(1);
            result[0] = x;
            return result;
        }
        /// <summary>
        /// Implicitly converts the specified value to a hex FlingOops.String.
        /// </summary>
        /// <param name="x">The value to convert.</param>
        /// <returns>The FlingOops.String value.</returns>
        [Drivers.Compiler.Attributes.NoDebug]
        [Drivers.Compiler.Attributes.NoGC]
        public static implicit operator FlingOops.String(Int16 x)
        {
            return (UInt16)x;
        }
        /// <summary>
        /// Implicitly converts the specified value to a hex FlingOops.String.
        /// </summary>
        /// <param name="x">The value to convert.</param>
        /// <returns>The FlingOops.String value.</returns>
        [Drivers.Compiler.Attributes.NoDebug]
        [Drivers.Compiler.Attributes.NoGC]
        public static implicit operator FlingOops.String(uint x)
        {
            FlingOops.String result = "";
            uint y = x;
            while (y > 0)
            {
                uint rem = y & 0xFu;
                switch (rem)
                {
                    case 0:
                        result = "0" + result;
                        break;
                    case 1:
                        result = "1" + result;
                        break;
                    case 2:
                        result = "2" + result;
                        break;
                    case 3:
                        result = "3" + result;
                        break;
                    case 4:
                        result = "4" + result;
                        break;
                    case 5:
                        result = "5" + result;
                        break;
                    case 6:
                        result = "6" + result;
                        break;
                    case 7:
                        result = "7" + result;
                        break;
                    case 8:
                        result = "8" + result;
                        break;
                    case 9:
                        result = "9" + result;
                        break;
                    case 10:
                        result = "A" + result;
                        break;
                    case 11:
                        result = "B" + result;
                        break;
                    case 12:
                        result = "C" + result;
                        break;
                    case 13:
                        result = "D" + result;
                        break;
                    case 14:
                        result = "E" + result;
                        break;
                    case 15:
                        result = "F" + result;
                        break;
                }
                y >>= 4;
            }
            return "0x" + result.PadLeft(8, '0');
        }
        /// <summary>
        /// Implicitly converts the specified value to a hex FlingOops.String.
        /// </summary>
        /// <param name="x">The value to convert.</param>
        /// <returns>The FlingOops.String value.</returns>
        [Drivers.Compiler.Attributes.NoDebug]
        [Drivers.Compiler.Attributes.NoGC]
        public static implicit operator FlingOops.String(int x)
        {
            return (uint)x;
        }
        /// <summary>
        /// Implicitly converts the specified value to a hex FlingOops.String.
        /// </summary>
        /// <param name="x">The value to convert.</param>
        /// <returns>The FlingOops.String value.</returns>
        [Drivers.Compiler.Attributes.NoDebug]
        [Drivers.Compiler.Attributes.NoGC]
        public static implicit operator FlingOops.String(ulong x)
        {
            uint part1 = (uint)x;
            uint part2 = (uint)(x >> 16 >> 16);
            return ((FlingOops.String)part2) + " " + ((FlingOops.String)part1);
        }
        /// <summary>
        /// Implicitly converts the specified value to a hex FlingOops.String.
        /// </summary>
        /// <param name="x">The value to convert.</param>
        /// <returns>The FlingOops.String value.</returns>
        [Drivers.Compiler.Attributes.NoDebug]
        [Drivers.Compiler.Attributes.NoGC]
        public static implicit operator FlingOops.String(long x)
        {
            return (ulong)x;
        }
    }
}
