<%@ Page Title="Edit content" Language="c#" MasterPageFile="masterpages/umbracoPage.Master"
    CodeBehind="EditFragment.aspx.cs" ValidateRequest="false" AutoEventWireup="True"
    Inherits="Zbu.DataTypes.RepeatableFragment.EditFragment" Trace="false" %>

<%@ Register TagPrefix="umb" Namespace="ClientDependency.Core.Controls" Assembly="ClientDependency.Core" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .tabpagecontainer { border-top: 1px solid #CAC9C9; }
        #body_TabView1 > div.header { display: none; }
        .mceToolbarExternal { left: 24px; }
    </style>
    <asp:Literal runat="server" ID="PostBackJs"></asp:Literal>
</asp:Content>

<asp:Content ContentPlaceHolderID="body" runat="server">
    <umb:JsInclude ID="JsInclude1" runat="server" FilePath="js/umbracoCheckKeys.js" PathNameAlias="UmbracoRoot" />
    <umb:JsInclude ID="JsInclude2" runat="server" FilePath="ui/jquery.js" PathNameAlias="UmbracoClient" Priority="0" />
    <umb:JsInclude ID="JsInclude3" runat="server" FilePath="ui/jqueryui.js" PathNameAlias="UmbracoClient" Priority="1" />
    <table style="height: 38px; width: 371px; border: none 0px;" cellspacing="0" cellpadding="0" id="__controls">
        <tr valign="top">
            <td height="20"></td>
            <td><asp:PlaceHolder ID="Container" runat="server"></asp:PlaceHolder></td>
        </tr>
    </table>
    <script type="text/javascript">
		// Save handlers for IDataFields		
		var saveHandlers = new Array();
		
		// For short-cut keys
		var isDialog = true;
		var functionsFrame = this;
		var disableEnterSubmit = true;
		
		function addSaveHandler(handler) {
			saveHandlers[saveHandlers.length] = handler;
		}		
		
		function invokeSaveHandlers() {
			for (var i=0;i<saveHandlers.length;i++) {
				eval(saveHandlers[i]);
			}
		}
		
		jQuery(function() {
		    var inputs = $('#__controls input:not(.editorIcon), #__controls textarea');
		    //Sys.Debug.traceDump(inputs);
		    inputs.change(function() {
		        UmbClientMgr.set_isDirty(true);
		    });
		    jQuery('input.editorIcon').click(function() { UmbClientMgr.set_isDirty(false); });
		});

        jQuery(document).ready(function () {
            UmbClientMgr.appActions().bindSaveShortCut();
        });
    </script>
</asp:Content>
