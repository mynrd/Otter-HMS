using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ExcludeToUpdateAttribute : Attribute
    {
    }
}
