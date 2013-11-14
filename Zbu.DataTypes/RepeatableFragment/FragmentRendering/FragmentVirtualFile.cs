using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Hosting;

namespace Zbu.DataTypes.RepeatableFragment.FragmentRendering
{
    class FragmentVirtualFile : VirtualFile
    {
        private readonly string _virtualPath;

        public FragmentVirtualFile(string virtualPath)
            : base(virtualPath)
        {
            _virtualPath = VirtualPathUtility.ToAppRelative(virtualPath);
        }

        private static string LoadResource(string resourceKey)
        {
            // Load from your database respository or whatever here...
            // Note that the caching is disabled for this content in the virtual path
            // provider, so you must cache this yourself in your repository.

            //var contentRepository = FrameworkHelper.Resolve<IContentRepository>();
            //var resource = contentRepository.GetContent(resourceKey);

            var resource = @"
<div>HELLO</div>
";

            if (String.IsNullOrWhiteSpace(resource))
                resource = String.Empty;

            return resource;
        }

        public override Stream Open()
        {
            // Always in format: "~/CMS/{0}.cshtml"
            var key = _virtualPath.Replace("~/Zbu.Fragment/", "").Replace(".cshtml", "");

            var resource = LoadResource(key);

            // this automatically appends the inherit and default using statements 
            // ... add any others here you like or append them to your resource.
            //resource = String.Format("{0}{1}", "@inherits System.Web.Mvc.WebViewPage<dynamic>\r\n" +
            //                                   "@using System.Web.Mvc\r\n" +
            //                                   "@using System.Web.Mvc.Html\r\n", resource);

            var bytes = Encoding.UTF8.GetBytes(resource);
            var ms = new MemoryStream(bytes);
            return ms;
        }    
    }
}
