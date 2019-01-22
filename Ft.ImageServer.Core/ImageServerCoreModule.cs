using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Ft.ImageServer.Core
{
    [DependsOn(typeof(AbpDddDomainModule))]
    public class ImageServerCoreModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {

        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {

        }

        public override void PostConfigureServices(ServiceConfigurationContext context)
        {

        }
    }
}
