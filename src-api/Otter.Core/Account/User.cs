using System;
using System.Collections.Generic;
using System.Text;

namespace Otter.Core.Account
{
    public class User
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public LoginStatus Status { get; set; }
        public string StatusStr => Status.ToString();
    }
}
