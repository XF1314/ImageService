using Ft.ImageServer.Core.HostConfigs;
using Ft.ImageServer.Core.Result;
using Ft.ImageServer.Service.Exceptions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ft.ImageServer.Service.ImageProviders
{
    public class WebImageProvider : IImageProvider
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly List<WebHostConfig> _webHostConfigs;

        public WebImageProvider(IHttpClientFactory clientFactory, IOptions<List<WebHostConfig>> webHostConfigs)
        {
            _clientFactory = clientFactory;
            _webHostConfigs = webHostConfigs.Value;
        }

        public async Task<Result<byte[]>> GetImageAsync(string hostName, string fileName)
        {
            var hostConfig = _webHostConfigs.FirstOrDefault(x => x.HostName.Equals(hostName, StringComparison.OrdinalIgnoreCase));
            if (hostConfig == null)
                return Result.FromError<byte[]>($"HostName:{hostName}无可匹配的HostConfig项");
            else
            {
                var uri = $"{hostConfig.Backend}/{fileName}";
                var httpClient = _clientFactory.CreateClient();
                using (var stream = new MemoryStream())
                {
                    using (var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri))
                    using (var contentStream = await (await httpClient.SendAsync(httpRequestMessage)).Content.ReadAsStreamAsync())
                    {
                        await contentStream.CopyToAsync(stream);
                    }

                    var imageBytes = stream.TryGetBuffer(out ArraySegment<byte> data) ? data.Array : null;
                    if (imageBytes == null && !string.IsNullOrWhiteSpace(hostConfig.FallbackImageUri))
                        throw new RedirectToFallbackException(hostConfig.FallbackImageUri, "图片未找到, 重定向到fallback image");
                    else
                    {
                        return Result.FromData(imageBytes);
                    }
                }
            }
        }
    }
}
