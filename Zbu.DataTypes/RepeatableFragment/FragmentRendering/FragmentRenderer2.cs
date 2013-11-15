using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Umbraco.Web;

namespace Zbu.DataTypes.RepeatableFragment.FragmentRendering
{
    class FragmentRenderer2
    {
        private readonly HttpContextBase _httpContext;
        private readonly UmbracoContext _umbracoContext;

        public FragmentRenderer2()
        {
            if (HttpContext.Current == null)
                throw new Exception("Missing HttpContext.Current.");
            _httpContext = new HttpContextWrapper(HttpContext.Current);

            if (UmbracoContext.Current == null)
                throw new Exception("Missing UmbracoContext.Current.");
            _umbracoContext = UmbracoContext.Current;
        }

        public string Render(string contentTypeAlias, IDictionary<string, object> dataValues, string viewName)
        {
            var content = new PublishedFragment(contentTypeAlias, dataValues, true);

            var routeVals = new RouteData();
            routeVals.Values.Add("controller", "Fragment");
            routeVals.Values.Add("action", "Index");
            routeVals.DataTokens.Add("umbraco-context", _umbracoContext); //required for UmbracoViewPage

            var request = new RequestContext(_httpContext, routeVals);
            string output;
            using (var controller = new FragmentController(viewName, content))
            {
                controller.ControllerContext = new ControllerContext(request, controller);
                var result = controller.Index();

                using (var sw = new StringWriter())
                {
                    EnsureViewObjectDataOnResult(controller, result);
                    var viewContext2 = new ViewContext(controller.ControllerContext, result.View, result.ViewData, result.TempData, sw);
                    result.View.Render(viewContext2, sw);
                    foreach (var v in result.ViewEngineCollection)
                        v.ReleaseView(controller.ControllerContext, result.View);
                    output = sw.ToString().Trim();
                }
            }

            return output;
        }

        // fixme - copied from Umbraco.Web.Mvc.ControllerExtensions
        static void EnsureViewObjectDataOnResult(ControllerBase controller, ViewResultBase result)
        {
            //when merging we'll create a new dictionary, otherwise you might run into an enumeration error
            // caused from ModelStateDictionary
            result.ViewData.ModelState.Merge(new ModelStateDictionary(controller.ViewData.ModelState));

            // Temporarily copy the dictionary to avoid enumerator-modification errors
            var newViewDataDict = new ViewDataDictionary(controller.ViewData);
            foreach (var d in newViewDataDict)
                result.ViewData[d.Key] = d.Value;

            result.TempData = controller.TempData;

            if (result.View != null) return;

            if (string.IsNullOrEmpty(result.ViewName))
                result.ViewName = controller.ControllerContext.RouteData.GetRequiredString("action");

            if (result.View != null) return;

            if (result is PartialViewResult)
            {
                var viewEngineResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, result.ViewName);

                if (viewEngineResult.View == null)
                {
                    throw new InvalidOperationException("Could not find the view " + result.ViewName + ", the following locations were searched: " + Environment.NewLine + string.Join(Environment.NewLine, viewEngineResult.SearchedLocations));
                }

                result.View = viewEngineResult.View;
            }
            else if (result is ViewResult)
            {
                var vr = (ViewResult)result;
                var viewEngineResult = ViewEngines.Engines.FindView(controller.ControllerContext, vr.ViewName, vr.MasterName);

                if (viewEngineResult.View == null)
                {
                    throw new InvalidOperationException("Could not find the view " + vr.ViewName + ", the following locations were searched: " + Environment.NewLine + string.Join(Environment.NewLine, viewEngineResult.SearchedLocations));
                }

                result.View = viewEngineResult.View;
            }
        }
    }
}
