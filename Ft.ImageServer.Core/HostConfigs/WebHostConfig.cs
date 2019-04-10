using System;
using System.Collections.Generic;
using System.Text;

namespace Ft.ImageServer.Core.HostConfigs
{
    /// <summary>
    /// 基于Web的Host配置
    /// </summary>
    public class WebHostConfig : IHostConfig
    {
        /// <summary>
        /// Host名称
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// Host类型
        /// </summary>
        public HostType HostType => HostType.Web;

        /// <summary>
        /// Fallback Image
        /// </summary>
        public string FallbackImageUri { get; set; }

        /// <summary>
        /// 后端图片服务器地址
        /// </summary>
        public string Backend { get; set; }
    }
}
