using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ft.ImageServer.Core.Entities;
using Ft.ImageServer.Core.Result;
using Ft.ImageServer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp.AspNetCore.Mvc;

namespace Ft.ImageServer.Host.Controllers
{
    /// <summary>
    /// 图片Metadata
    /// </summary>
    [AllowAnonymous, ApiController]
    public class ImageMetadataController : AbpController
    {
        private readonly IImageService _imageService;
        private readonly ILogger<ImageMetadataController> _logger;

        /// <inheritdoc/>
        public ImageMetadataController(IImageService imageService,ILogger<ImageMetadataController> logger)
        {
            _logger = logger;
            _imageService = imageService;
        }


        /// <summary>
        /// 获取图片Metadata（只对MongoDB存储方式有效）
        /// </summary>
        /// <param name="id">图片Id</param>
        /// <returns></returns>
        [HttpGet("/metadata/{id:gridfs}")]
        public async Task<Result<ImageMetadata>> GetImageMetadataAsync( string id)
        {
            return await _imageService.GetImageMetadataAsync(id);
        }
    }
}
