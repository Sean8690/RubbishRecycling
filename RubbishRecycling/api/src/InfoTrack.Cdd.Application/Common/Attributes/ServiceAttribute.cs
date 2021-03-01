using System;

#pragma warning disable 1591

namespace InfoTrack.Cdd.Application.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class ServiceAttribute : Attribute
    {
        public string Description { get; set; }
    }
}
