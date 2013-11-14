using System;
using System.Collections.Generic;
using umbraco;
using umbraco.cms.businesslogic.macro;
using Umbraco.Core.Models;
using umbraco.presentation.install;
using Umbraco.Web.Macros;

namespace Zbu.DataTypes.RepeatableFragment.FragmentRendering
{
    class FragmentRenderer
    {
        public string Render(string razor, string contentTypeAlias, IDictionary<string, object> dataValues)
        {
            // fixme - what shall we initialize?
            var macro = new MacroModel();
            macro.ScriptName = "NOSCRIPT";
            macro.ScriptCode = @"
@inherits UmbracoViewPage<IPublishedContent>
<div>
    <span>zz</span>
    <span>@Model.GetPropertyValue(""title"")</span>
</div>
";

            // fixme - ispreviewing?
            var publishedContent = new PublishedFragment(contentTypeAlias, dataValues, true);

            // fixme - handle exceptions?!
            var result = LoadPartialViewMacro(macro, publishedContent);
            return result.Result;
        }

        // copied and adapted from Umbraco's code to execute partial view macros
        private static ScriptingMacroResult LoadPartialViewMacro(MacroModel macro, IPublishedContent content)
        {
            var result = new ScriptingMacroResult();

            var engine = MacroEngineFactory.GetEngine(PartialViewMacroEngine.EngineName) as PartialViewMacroEngine;
            if (engine == null) throw new Exception("Oops.");
            result.Result = engine.Execute(macro, content);

            // don't bother - only the legacy razor engine seems to implement that interface
            //var reportingEngine = engine as IMacroEngineResultStatus;
            //if (reportingEngine != null)
            //{
            //    if (reportingEngine.Success == false)
            //        result.ResultException = reportingEngine.ResultException;
            //}

            return result;
        }
    }
}
