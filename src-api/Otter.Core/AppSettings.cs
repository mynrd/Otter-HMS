using System;
using System.Collections.Generic;
using System.Text;

namespace Otter.Core
{
    public class AppSettings
    {
        public string SecretKey { get; set; }
        public int TokenExpirationMinutes { get; set; }
    }
}