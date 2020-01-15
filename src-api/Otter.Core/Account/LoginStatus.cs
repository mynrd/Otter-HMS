using System;
using System.Collections.Generic;
using System.Text;

namespace Otter.Core.Account
{
    public enum LoginStatus
    {
        None,
        Locked,
        ResetPassword,
        Success,
        NotFound,
        InvalidPassword,
    }
}