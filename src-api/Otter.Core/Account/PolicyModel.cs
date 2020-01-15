using System;
using System.Collections.Generic;
using System.Text;

namespace Otter.Core.Account
{
    public class PolicyModel
    {
        public string PolicyCode { get; set; }
        public IEnumerable<string> RoleCodes { get; set; }
    }
}
