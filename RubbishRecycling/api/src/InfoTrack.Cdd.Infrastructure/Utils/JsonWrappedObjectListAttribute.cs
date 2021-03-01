using System;

namespace InfoTrack.Cdd.Infrastructure.Utils
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class JsonWrappedObjectListAttribute : Attribute
    {
        public string PropertyName { get; }

        public JsonWrappedObjectListAttribute()
        {
        }

        public JsonWrappedObjectListAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }
    }
}
