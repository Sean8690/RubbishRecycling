using System;

#pragma warning disable 1591

namespace InfoTrack.Cdd.Application.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class DimensionsAttribute : Attribute
    {
        public DimensionsAttribute(double width, double height)
        {
            Width = width;
            Height = height;
        }

        public double Width { get; }

        public double Height { get; }
    }
}
