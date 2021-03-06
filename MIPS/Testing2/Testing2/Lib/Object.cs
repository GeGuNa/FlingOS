using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testing2
{
    /// <summary>
    /// All objects (that are GC managed) should derive from this type.
    /// </summary>
    public class Object : ObjectWithType
    {
        [Drivers.Compiler.Attributes.NoGC]
        [Drivers.Compiler.Attributes.NoDebug]
        public Object()
        {
        }
    }
    /// <summary>
    /// Represents an object with a type. You should use the <see cref="Kernel.FOS_System.Object"/> class.
    /// </summary>
    /// <remarks>
    /// We implement it like this so that _Type field is always the first
    /// field in memory of all objects.
    /// </remarks>
    public class ObjectWithType
    {
        /// <summary>
        /// The underlying, specific type of the object specified when it was created.
        /// </summary>
        public Type _Type;

        [Drivers.Compiler.Attributes.NoGC]
        [Drivers.Compiler.Attributes.NoDebug]
        public ObjectWithType()
        {
        }
    }
}
