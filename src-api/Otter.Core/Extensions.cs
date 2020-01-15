using System;
using System.Collections.Generic;
using System.Text;

namespace Otter.Core
{
    public static class Extensions
    {
        public static int ToInt(this string value)
        {
            return int.Parse(value);
        }
    }
}