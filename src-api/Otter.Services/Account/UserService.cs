using Otter.Core.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace Otter.Services.Account
{
    public class UserService : BaseService
    {
        public UserService(ICurrentUser currentUser) : base(currentUser)
        {
        }
    }
}