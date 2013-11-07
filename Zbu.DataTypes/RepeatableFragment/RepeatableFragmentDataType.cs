using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using umbraco.cms.businesslogic.datatype;
using Umbraco.Core;
using umbraco.interfaces;

namespace Zbu.DataTypes.RepeatableFragment
{
    public class RepeatableFragmentDataType : BaseDataType, IDataType
    {
        public static readonly Guid RepeatableFragmentGuid = new Guid("{595F84A8-22EB-4D75-9757-65738F4FFED8}");

        private IData _data;
        private IDataEditor _editor;
        private IDataPrevalue _prevalue;
        private RepeatableFragmentPrevalueEditor.ConfigData _config;

        private RepeatableFragmentPrevalueEditor.ConfigData Config
        {
            get { return _config ?? (_config = ((RepeatableFragmentPrevalueEditor)PrevalueEditor).Config); }
        }

        public override IData Data
        {
            get { return _data ?? (_data = new DefaultData(this)); }
        }

        public override IDataEditor DataEditor
        {
            get { return _editor ?? (_editor = new RepeatableFragmentEditor(Data, Config)); }
        }

        public override string DataTypeName
        {
            get { return "Repeatable Fragment"; }
        }

        public override Guid Id
        {
            get { return RepeatableFragmentGuid; }
        }

        public override IDataPrevalue PrevalueEditor
        {
            get { return _prevalue ?? (_prevalue = new RepeatableFragmentPrevalueEditor(this)); }
        }
    }
}
