using System;
using System.Collections;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;

namespace Zbu.DataTypes.RepeatableFragment.FragmentRendering
{
    class FragmentVirtualPathProvider : VirtualPathProvider
    {
        private static bool IsCustomContentPath(string virtualPath)
        {
            var checkPath = VirtualPathUtility.ToAppRelative(virtualPath);
            return checkPath.StartsWith("~/Zbu.Fragment/", StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool FileExists(string virtualPath)
        {
            return IsCustomContentPath(virtualPath) || base.FileExists(virtualPath);
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            return IsCustomContentPath(virtualPath) ? new FragmentVirtualFile(virtualPath) : base.GetFile(virtualPath);
        }

        public override CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            if (IsCustomContentPath(virtualPath))
            {
                var key = VirtualPathUtility.ToAppRelative(virtualPath);

                key = key.Replace("~/Zbu.Fragment/", "").Replace(".cshtml", "");

                var cacheKey = String.Format("Zbu.Fragment.{0}", key);

                var dependencyKey = new String[1];
                dependencyKey[0] = string.Format(cacheKey);

                return new CacheDependency(null, dependencyKey);
            }

            return Previous.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
        }

        public override string GetFileHash(string virtualPath, IEnumerable virtualPathDependencies)
        {
            if (IsCustomContentPath(virtualPath))
            {
                return virtualPath;
            }

            return base.GetFileHash(virtualPath, virtualPathDependencies);
        }

        public static void RegisterWithHostingEnvironment()
        {
            HostingEnvironment.RegisterVirtualPathProvider(new FragmentVirtualPathProvider());
        }
    }
}
