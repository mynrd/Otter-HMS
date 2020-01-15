using System;
using System.Collections.Generic;
using System.Text;

namespace Otter.Core.Account
{
    public interface ICurrentUser
    {
        string UserId { get; }
        string Email { get; }
        bool IsAuthenticated { get; }
    }
}
