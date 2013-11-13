using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using umbraco;
using umbraco.BasePages;
using umbraco.BusinessLogic.Actions;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Serialization;
using umbraco.presentation;

namespace Zbu.DataTypes.RepeatableFragment
{
    public class EditFragment : UmbracoEnsuredPage
    {
        protected umbraco.uicontrols.TabView TabView1;

        private umbraco.controls.FragmentControl _control;
        private IContent _content;
        private IContentType _contentType;

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);

            int id;
            if (int.TryParse(Request.QueryString["id"], out id) == false)
            {
                this.DisplayFatalError("Invalid query string (missing or invalid document ID)");
                return;
            }
            _content = ApplicationContext.Current.Services.ContentService.GetById(id);
            if (_content == null)
            {
                this.DisplayFatalError("No document with ID " + id);
                return;
            }
            //_document = new Document(content);
            //_contentId = id;

            var json = Request.QueryString["fragment"];
            Fragment fragment = null;
            if (!string.IsNullOrWhiteSpace(json))
            {
                var serializer = new JsonSerializer();
                fragment = serializer.Deserialize<Fragment>(json);
                var contentTypeAlias = fragment.FragmentTypeAlias;
                if (!string.IsNullOrWhiteSpace(contentTypeAlias))
                {
                    _contentType = ApplicationContext.Current.Services.ContentTypeService.GetContentType(contentTypeAlias);
                }
            }

            //var contentTypeAlias = Request.QueryString["ctype"];
            //if (!string.IsNullOrWhiteSpace(contentTypeAlias))
            //{
            //    _contentType = ApplicationContext.Current.Services.ContentTypeService.GetContentType(contentTypeAlias);
            //}
            if (_contentType == null || fragment == null) // redundant but pleases Resharper
            {
                this.DisplayFatalError("Invalid query string (missing or invalid fragment type alias)");
                return;
            }

            //// we need to check if there's a published version of this document
            //_documentHasPublishedVersion = _document.Content.HasPublishedVersion();

            _control = new umbraco.controls.FragmentControl(_content, _contentType, fragment.Values)
            {
                ID = "TabView1",
                Width = Unit.Pixel(666),
                Height = Unit.Pixel(666)
            };

            Container.Controls.Add(_control);

            _control.Save += Save;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (_content == null)
            //    return;

            //if (!CheckUserValidation())
            //    return;

            // else ...
        }

        protected void Save(object sender, EventArgs e)
        {
            DoSave();
        }

        private void DoSave()
        {
            // error handling test
            if (!Page.IsValid)
            {
                foreach (umbraco.uicontrols.TabPage tp in _control.GetPanels())
                {
                    tp.ErrorControl.Visible = true;
                    tp.ErrorHeader = ui.Text("errorHandling", "errorButDataWasSaved");
                    tp.CloseCaption = ui.Text("close");
                }
            }
            else if (Page.IsPostBack)
            {
                // hide validation summaries
                foreach (umbraco.uicontrols.TabPage tp in _control.GetPanels())
                {
                    tp.ErrorControl.Visible = false;
                }
            }

            var serializer = new JsonSerializer();
            var fragment = new Fragment
            {
                FragmentTypeAlias = _contentType.Alias,
                Values = _control.Values
            };
            var json = serializer.Serialize(fragment).Replace("\r", "").Replace("\n", " ");

            PostBackJs.Text = string.Format("<script type=\"text/javascript\">UmbClientMgr.closeModalWindow('{0}');</script>",
                json.Replace("'", "\\'"));
        }

        private void ShowUserValidationError(string message)
        {
            Controls.Clear();
            Controls.Add(new LiteralControl(String.Format("<link rel='stylesheet' type='text/css' href='../../umbraco_client/ui/default.css'><link rel='stylesheet' type='text/css' href='../../umbraco_client/tabview/style.css'><link rel='stylesheet' type='text/css' href='../../umbraco_client/propertypane/style.css'><div id='body_dashboardTabs' style='height: auto; width: auto;'><div class='header'><ul><li id='body_dashboardTabs_tab01' class='tabOn'><a id='body_dashboardTabs_tab01a' href='#' onclick='setActiveTab('body_dashboardTabs','body_dashboardTabs_tab01',body_dashboardTabs_tabs); return false;'><span><nobr>Access denied</nobr></span></a></li></ul></div><div id='' class='tabpagecontainer'><div id='body_dashboardTabs_tab01layer' class='tabpage' style='display: block;'><div class='menubar'></div><div class='tabpagescrollinglayer' id='body_dashboardTabs_tab01layer_contentlayer' style='width: auto; height: auto;'><div class='tabpageContent' style='padding:0 10px;'><div class='propertypane' style=''><div><div class='propertyItem' style=''><div class='dashboardWrapper'><h2>Access denied</h2><img src='./dashboard/images/access-denied.png' alt='Access denied' class='dashboardIcon'>{0}</div></div></div></div></div></div></div></div><div class='footer'><div class='status'><h2></h2></div></div></div>", message)));
        }

        private bool CheckUserValidation()
        {
            if (!ValidateUserApp(Constants.Applications.Content))
            {
                ShowUserValidationError("<h3>The current user doesn't have access to this application</h3><p>Please contact the system administrator if you think that you should have access.</p>");
                return false;
            }

            if (!ValidateUserNodeTreePermissions(_content.Path, ActionBrowse.Instance.Letter.ToString(CultureInfo.InvariantCulture)))
            {
                ShowUserValidationError(
                    "<h3>The current user doesn't have permissions to browse this document</h3><p>Please contact the system administrator if you think that you should have access.</p>");
                return false;
            }

            if (!ValidateUserNodeTreePermissions(_content.Path, ActionUpdate.Instance.Letter.ToString(CultureInfo.InvariantCulture)))
            {
                ShowUserValidationError("<h3>The current user doesn't have permissions to edit this document</h3><p>Please contact the system administrator if you think that you should have access.</p>");
                return false;
            }

            return true;
        }

        protected Literal PostBackJs;
        protected ClientDependency.Core.Controls.JsInclude JsInclude1;
        protected ClientDependency.Core.Controls.JsInclude JsInclude2;
        protected ClientDependency.Core.Controls.JsInclude JsInclude3;
        protected PlaceHolder Container;
    }
}
