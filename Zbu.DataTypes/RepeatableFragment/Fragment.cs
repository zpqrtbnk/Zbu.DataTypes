using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zbu.DataTypes.RepeatableFragment
{
    class Fragment
    {
        public string FragmentTypeAlias;
        public IDictionary<string, object> Values = new Dictionary<string, object>();
    }
}
