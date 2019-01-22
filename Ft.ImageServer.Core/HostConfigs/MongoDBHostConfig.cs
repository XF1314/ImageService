using System;
using System.Collections.Generic;
using System.Text;

namespace Ft.ImageServer.Core.HostConfigs
{
    public class MongoDBHostConfig : IHostConfig
    {
        /// <summary>
        /// Host名称
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// Host类型
        /// </summary>
        public HostType HostType => HostType.GridFs;

        /// <summary>
        /// Fallback Image
        /// </summary>
        public string FallbackImageUri { get; set; }

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DatabaseName { get; set; }
    }
}
