using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using umbraco.cms.businesslogic.datatype;
using Umbraco.Core;
using Umbraco.Core.Models;
using umbraco.interfaces;

namespace Zbu.DataTypes.RepeatableFragment
{
    class RepeatableFragmentPrevalueEditor : PlaceHolder, IDataPrevalue
    {
        private readonly BaseDataType _datatype;
        private ConfigData _config;

        private TextBox _alias;

        public RepeatableFragmentPrevalueEditor(BaseDataType dataType)
        {
            _datatype = dataType;
        }

        public Control Editor
        {
            get { return this; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            _alias = new TextBox
            {
                Text = Config.FragmentTypeAlias
            };

            Controls.Add(_alias);
        }

        // this is somewhat ugly yet this is how it's done in most built-in editors
        protected override void Render(HtmlTextWriter writer)
        {
            writer.WriteLine("<table>");
            writer.WriteLine("<tr><th>Fragment type alias</th><td>");
            _alias.RenderControl(writer);
            writer.Write("</td></tr>");
            writer.Write("</table>");
        }

        public void Save()
        {
            _datatype.DBType = DBTypes.Ntext;

            var config = Config;
            config.FragmentTypeAlias = _alias.Text;
            Config = config;
        }

        public class ConfigData
        {
            public string FragmentTypeAlias;
        }

        public ConfigData Config
        {
            get
            {
                if (_config == null)
                {
                    var id = _datatype.DataTypeDefinitionId;
                    var prevalues = ApplicationContext.Current.Services.DataTypeService
                        .GetPreValuesCollectionByDataTypeId(id)
                        .PreValuesAsDictionary;

                    Umbraco.Core.Models.PreValue prevalue;
                    var fragmentTypeAlias = prevalues.TryGetValue("FragmentTypeAlias", out prevalue)
                        ? prevalue.Value
                        : "";

                    _config = new ConfigData
                    {
                        FragmentTypeAlias = fragmentTypeAlias
                    };
                }
                return _config;
            }
            set
            {
                var id = _datatype.DataTypeDefinitionId;

                var prevalues = new Dictionary<string, Umbraco.Core.Models.PreValue>();
                prevalues["FragmentTypeAlias"] = new Umbraco.Core.Models.PreValue(id, value.FragmentTypeAlias);

                ApplicationContext.Current.Services.DataTypeService.SavePreValues(id, prevalues);
            }
        }
    }
}
