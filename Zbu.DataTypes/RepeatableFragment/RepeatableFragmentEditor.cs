using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Umbraco.Core.IO;
using umbraco.interfaces;
using umbraco.editorControls;
using Zbu.DataTypes.RepeatableFragment.FragmentRendering;

[assembly: WebResource("Zbu.DataTypes.RepeatableFragment.RepeatableFragment.css", "text/css")]
[assembly: WebResource("Zbu.DataTypes.RepeatableFragment.RepeatableFragment.js", "application/x-javascript")]

namespace Zbu.DataTypes.RepeatableFragment
{
    class RepeatableFragmentEditor : PlaceHolder, IDataEditor
    {
        private readonly RepeatableFragmentDataType _datatype;

        private HiddenField _formIds;
        private Panel _panel;

        public RepeatableFragmentEditor(RepeatableFragmentDataType datatype)
        {
            _datatype = datatype;            
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // create the panel
            _panel = new Panel
            {
                ID = ID + "_edit",
                CssClass = "zbu-repeatable-fragment"
            };
            Controls.Add(_panel);

            // create the hidden field containing the fragment ids
            // the actual value will be initialized by JavaScript
            _formIds = new HiddenField
            {
                ID = ID + "_idx",
                Value = ","
            };
            _panel.Controls.Add(_formIds);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // register our resources
            this.RegisterEmbeddedClientResource("Zbu.DataTypes.RepeatableFragment.RepeatableFragment.css",
                umbraco.cms.businesslogic.datatype.ClientDependencyType.Css);
            this.RegisterEmbeddedClientResource("Zbu.DataTypes.RepeatableFragment.RepeatableFragment.js",
                umbraco.cms.businesslogic.datatype.ClientDependencyType.Javascript);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            const string html = @"
<div class=""zbu-fragments""></div>
<a href=""#"" class=""zbu-fragments-add"">add</a>
<script type=""text/javascript"">
    $('#{0}_edit').zbu('repeatable-fragment', {{
        umbraco: '{1}',
        contentId: {2},
        fragment: '{3}',
        fragments: {4},
        fraghtml: [ {5} ]
    }});
</script>
";

            // get values
            var contentId = Page.Request.QueryString["id"];

            var data = _datatype.Data.Value as string;
            if (string.IsNullOrWhiteSpace(data)) data = "[]";

            var serializer = new JsonSerializer();
            var fragments = serializer.Deserialize<Fragment[]>(data);
            var fraghtml = new StringBuilder();
            foreach (var fragment in fragments)
            {
                if (fraghtml.Length > 0)
                    fraghtml.Append(", ");
                var s = RenderFragment(fragment);
                s = s.Replace("\r", "").Replace("\n", " ").Replace("'", "\\'");
                fraghtml.AppendFormat("'{0}'", s);
            }

            // build the editor's html
            _panel.Controls.Add(new LiteralControl(string.Format(html,
                ClientID,
                IOHelper.ResolveUrl(SystemDirectories.Umbraco).Replace("'", "\\'"),
                contentId,
                serializer.Serialize(new Fragment { FragmentTypeAlias = _datatype.Config.FragmentTypeAlias }).Replace("'", "\\'"),
                data,
                fraghtml)));

            // reset the fragment ids (on postbacks)
            // the actual value will be initialized by JavaScript
            _formIds.Value = ",";
        }

        private string RenderFragment(Fragment fragment)
        {
            var engine = new FragmentRenderer2();
            var viewName = string.Format("FV{0}", _datatype.DataTypeDefinitionId);
            FragmentVirtualPathProvider.SetFragmentView(viewName, _datatype.Config.FragmentViewCode);
            return engine.Render(fragment.FragmentTypeAlias, fragment.Values, viewName);
        }

        public Control Editor
        {
            get { return this; }
        }

        public void Save()
        {
            var idx = _formIds.Value.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var data = new StringBuilder();
            foreach (var id in idx)
            {
                if (data.Length > 0) data.Append(", ");
                data.Append(Page.Request.Form[ClientID + "_" + id]);
            }
            _datatype.Data.Value = "[ " + data + " ]";
        }

        public bool ShowLabel
        {
            get { return true; }
        }

        public bool TreatAsRichTextEditor
        {
            get { return false; }
        }
    }
}
