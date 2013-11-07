using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using umbraco.cms.businesslogic.datatype;
using umbraco.presentation.webservices;

namespace Zbu.DataTypes.RepeatableFragment
{
    class RepeatableFragmentData : DefaultData
    {
        public RepeatableFragmentData(BaseDataType dataType)
            : base(dataType)
        {
        }

        // { "fragmentTypeAlias": "myFragment", "fragments": [
        //   { ... }
        // ]}

        // NOOOOO get the fragment type in the fragment itself! straight from the JSON from the FragmentControl!!

        // FIXME but then data is just a STRING and we don't need no custom data?

        public override object Value
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
