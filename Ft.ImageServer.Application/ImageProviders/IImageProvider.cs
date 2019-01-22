using Ft.ImageServer.Core.HostConfigs;
using Ft.ImageServer.Core.Result;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ft.ImageServer.Service.ImageProviders
{
    public interface IImageProvider
    {
        /// <summary>
        /// 获取Image
        /// </summary>
        /// <param name="hostName">host name</param>
        /// <param name="fileName">文件名称</param>
        /// <returns></returns>
        Task<Result<byte[]>> GetImageAsync(string hostName, string fileName);

    }
}
