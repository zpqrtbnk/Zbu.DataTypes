using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;

namespace Zbu.DataTypes.RepeatableFragment.FragmentRendering
{
    public class FragmentRenderingApplication : ApplicationEventHandler
    {
        protected override void ApplicationInitialized(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            FragmentVirtualPathProvider.RegisterWithHostingEnvironment();
            base.ApplicationInitialized(umbracoApplication, applicationContext);
        }
    }
}
