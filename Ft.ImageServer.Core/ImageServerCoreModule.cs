using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Ft.ImageServer.Core
{
   /// <summary>
   /// 图片服务领域层
   /// </summary>
    [DependsOn(typeof(AbpDddDomainModule))]
    public class ImageServerCoreModule : AbpModule
    {
        /// <inheritdoc/>
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {

        }

        /// <inheritdoc/>
        public override void ConfigureServices(ServiceConfigurationContext context)
        {

        }

        /// <inheritdoc/>
        public override void PostConfigureServices(ServiceConfigurationContext context)
        {

        }
    }
}
