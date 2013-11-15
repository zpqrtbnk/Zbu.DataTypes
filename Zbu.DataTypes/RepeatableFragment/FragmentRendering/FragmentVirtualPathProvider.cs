using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;

namespace Zbu.DataTypes.RepeatableFragment.FragmentRendering
{
    class FragmentVirtualPathProvider : VirtualPathProvider
    {
        public static string VirtualDir = "~/Views/Zbu.Fragments/";

        private static readonly ConcurrentDictionary<string, string> FragmentViews = new ConcurrentDictionary<string, string>();

        public static void SetFragmentView(string viewKey, string code)
        {
            FragmentViews[viewKey] = code;
        }

        public static void ClearFragmentView(string viewKey)
        {
            string code;
            FragmentViews.TryRemove(viewKey, out code);
        }

        public static void ClearFragmentViews()
        {
            FragmentViews.Clear();
        }

        private static bool IsFragmentViewPath(string virtualPath)
        {
            var checkPath = VirtualPathUtility.ToAppRelative(virtualPath);
            return checkPath.StartsWith(VirtualDir, StringComparison.InvariantCultureIgnoreCase);
        }

        private static string GetViewKey(string virtualPath)
        {
            var checkPath = VirtualPathUtility.ToAppRelative(virtualPath);
            var viewKey = checkPath.Substring(VirtualDir.Length, checkPath.Length - VirtualDir.Length - ".cshtml".Length);
            return viewKey;
        }

        public override bool FileExists(string virtualPath)
        {
            // file already exist even if we have no corresponding view code
            return IsFragmentViewPath(virtualPath) || base.FileExists(virtualPath);
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            if (!IsFragmentViewPath(virtualPath))
                return base.GetFile(virtualPath);

            // always return something - even an empty file
            var viewKey = GetViewKey(virtualPath);
            string viewCode;
            return FragmentViews.TryGetValue(viewKey, out viewCode) 
                ? new FragmentVirtualFile(virtualPath, viewCode)
                : new FragmentVirtualFile(virtualPath, string.Empty);
        }

        // fixme - what should we depend on?!
        public override CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            if (IsFragmentViewPath(virtualPath))
            {
                var key = VirtualPathUtility.ToAppRelative(virtualPath);

                key = key.Replace(VirtualDir, "").Replace(".cshtml", "");

                var cacheKey = String.Format("Zbu.Fragment.{0}", key);

                var dependencyKey = new String[1];
                dependencyKey[0] = string.Format(cacheKey);

                return new CacheDependency(null, dependencyKey);
            }

            return Previous.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
        }

        public override string GetFileHash(string virtualPath, IEnumerable virtualPathDependencies)
        {
            return IsFragmentViewPath(virtualPath)
                ? virtualPath 
                : base.GetFileHash(virtualPath, virtualPathDependencies);
        }

        public static void RegisterWithHostingEnvironment()
        {
            HostingEnvironment.RegisterVirtualPathProvider(new FragmentVirtualPathProvider());
        }
    }
}
