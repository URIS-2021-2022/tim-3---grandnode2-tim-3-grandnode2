using System;

namespace Grand.Web.Common.Components
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.Delegate)]
    public class BaseViewComponentAttribute : Attribute
    {
        public bool AdminAccess { get; set; }
    }
}
