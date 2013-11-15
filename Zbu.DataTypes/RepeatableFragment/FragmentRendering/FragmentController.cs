using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web.Mvc;

namespace Zbu.DataTypes.RepeatableFragment.FragmentRendering
{
    [MergeParentContextViewData]
    class FragmentController : Controller
    {
        private readonly string _viewName;
        private readonly IPublishedContent _content;

        public FragmentController(string viewName, IPublishedContent content)
        {
            _viewName = viewName;
            _content = content;
        }

        [ChildActionOnly]
        public PartialViewResult Index()
        {
            return PartialView(FragmentVirtualPathProvider.VirtualDir + _viewName + ".cshtml", _content);
        }
    }
}
