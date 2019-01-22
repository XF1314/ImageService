using Ft.ImageServer.Core.HostConfigs;
using Ft.ImageServer.Core.Result;
using Ft.ImageServer.Service.Exceptions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ft.ImageServer.Service.ImageProviders
{
    public class FileSystemImageProvider : IImageProvider
    {
        private readonly List<FileSystemHostConfig> _fileSystemHostConfig;

        public FileSystemImageProvider(IOptions<List<FileSystemHostConfig>> fileSystemHostConfig)
        {
            _fileSystemHostConfig = fileSystemHostConfig.Value;
        }

        public async Task<Result<byte[]>> GetImageAsync(string hostName, string fileName)
        {
            var hostConfig = _fileSystemHostConfig.FirstOrDefault(x => x.HostName.Equals(hostName, StringComparison.OrdinalIgnoreCase));
            if (hostConfig == null)
                return Result.FromError<byte[]>($"HostName:{hostName}无可匹配的HostConfig项");
            else
            {
                try
                {
                    var filePath = $"{hostConfig.Path}/{fileName}";
                    var fileBytes = await File.ReadAllBytesAsync(filePath);
                    return Result.FromData(fileBytes);
                }
                catch (FileNotFoundException exception)
                {
                    if (string.IsNullOrWhiteSpace(hostConfig.FallbackImageUri))
                        throw;
                    else if (fileName == hostConfig.FallbackImageUri)
                        return Result.FromError<byte[]>("没能找到fallback image");
                    else
                    {
                        throw new RedirectToFallbackException(hostConfig.FallbackImageUri, "图片未找到, 重定向到fallback image");
                    }
                }
            }
        }
    }
}
