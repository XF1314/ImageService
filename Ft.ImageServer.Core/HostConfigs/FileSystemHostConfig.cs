using System;
using System.Collections.Generic;
using System.Text;

namespace Ft.ImageServer.Core.HostConfigs
{
    public class FileSystemHostConfig : IHostConfig
    {
        /// <summary>
        /// Host名称
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// Host类型
        /// </summary>
        public HostType HostType => HostType.FileSystem;

        /// <summary>
        /// Fallback Image
        /// </summary>
        public string FallbackImageUri { get; set; }

        /// <summary>
        /// 后端图片服务器地址
        /// </summary>
        public string Path { get; set; }
    }
}
