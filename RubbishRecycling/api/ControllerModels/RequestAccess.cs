using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RubbishRecyclingAU.ControllerModels
{
    public class RequestAccess
    {
        public enum SessionPrivilegeLevel
        {
            /// <summary>The user can't do anything with the session except MFA.</summary>
            Underprivileged,

            /// <summary>The user has not been MFAed in this session but also doesn't need to be for
            /// normal app usage.</summary>
            Neutral,

            /// <summary>The user has been MFAed and so can access privileged operations.</summary>
            Privileged
        }
    }
}
