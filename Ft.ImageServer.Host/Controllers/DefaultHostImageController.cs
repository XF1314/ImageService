using Ft.ImageServer.Application.Dtos;
using Ft.ImageServer.Core.HostConfigs;
using Ft.ImageServer.Core.Result;
using Ft.ImageServer.Service;
using Ft.ImageServer.Service.ImageProviders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Uow;

namespace Ft.ImageServer.Host.Controllers
{
    /// <summary>
    /// 默认host的照片
    /// </summary>
    [AllowAnonymous, ApiController]
    public class DefaultHostImageController : AbpController
    {
        private readonly IImageService _imageService;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<HostBasedImageController> _logger;

        /// <inheritdoc/>
        public DefaultHostImageController(IImageService imageService,
            IServiceProvider serviceProvider, ILogger<HostBasedImageController> logger, IUnitOfWorkManager unitOfWorkManager)
        {
            _logger = logger;
            _imageService = imageService;
            _serviceProvider = serviceProvider;
            UnitOfWorkManager = unitOfWorkManager;
        }

        #region 获取图片
        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="id">照片Id</param>
        /// <param name="metahash">照片 meta hash</param>
        /// <param name="width">照片宽度</param>
        /// <param name="height">照片高度</param>
        /// <param name="quality">照片质量</param>
        /// <param name="options">附加选项([tgf]{1,3})</param>
        /// <returns></returns>
        [HttpGet("/image/{id:gridfs}/h-{metahash:metahash}/{width:range(0,5000)}x{height:range(0,5000)}/{quality:range(0,100)}/{options:options}")]
        public async Task<IActionResult> GetImageAsync(string id, string metahash, int width, int height, int quality, string options)
        {
            var result = await _imageService.GetImageFromMongoDBAsync(MongoDBImageProvider.DefaultHostName, id, metahash, width, height, quality, options);
            if (result.Success)
                return File(result.Data.Imagebytes, result.Data.Mime);
            else
            {
                _logger.LogInformation($"未能获取到照片信息，参数是:host={MongoDBImageProvider.DefaultHostName},id={id},metahash={metahash},width={width},height={height},quality={quality},options={options}");
                return NotFound();
            }
        }

        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="id">照片Id</param>
        /// <param name="metahash">照片 meta hash</param>
        /// <param name="quality">照片质量</param>
        /// <returns></returns>
        [HttpGet("/image/{id:gridfs}/h-{metahash:metahash}/{quality:range(0,100)}")]
        public async Task<IActionResult> GetImageWithoutOptionsAsync(string id, string metahash, int quality)
        {
            var result = await _imageService.GetImageFromMongoDBAsync(MongoDBImageProvider.DefaultHostName, id, metahash, quality);
            if (result.Success)
                return File(result.Data.Imagebytes, result.Data.Mime);
            else
            {
                _logger.LogInformation($"未能获取到照片信息，参数是:host={MongoDBImageProvider.DefaultHostName},id={id},metahash={metahash},quality={quality}");
                return NotFound();
            }
        }

        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="id">照片Id</param>
        /// <param name="metahash">照片 meta hash</param>
        /// <param name="quality">照片质量</param>
        /// <param name="options">附加选项([tgf]{1,3})</param>
        /// <returns></returns>
        [HttpGet("/image/{id:gridfs}/h-{metahash:metahash}/{quality:range(0,100)}/{options:options}")]
        public async Task<IActionResult> GetImageWithoutOptionsAsync(string id, string metahash, int quality, string options)
        {
            var result = await _imageService.GetImageFromMongoDBAsync(MongoDBImageProvider.DefaultHostName, id, metahash, quality, options);
            if (result.Success)
                return File(result.Data.Imagebytes, result.Data.Mime);
            else
            {
                _logger.LogInformation($"未能获取到照片信息，参数是:host={MongoDBImageProvider.DefaultHostName},id={id},metahash={metahash},quality={quality},options={options}");
                return NotFound();
            }
        }

        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="id">照片Id</param>
        /// <param name="metahash">照片 meta hash</param>
        /// <param name="width">照片宽度</param>
        /// <param name="height">照片高度</param>
        /// <param name="quality">照片质量</param>
        /// <returns></returns>
        [HttpGet("/image/{id:gridfs}/h-{metahash:metahash}/{quality:range(0,100)}/{width:range(0,5000)}x{height:range(0,5000)}")]
        public async Task<IActionResult> GetImageWithoutOptionsAsync(string id, string metahash, int width, int height, int quality)
        {
            var result = await _imageService.GetImageFromMongoDBAsync(MongoDBImageProvider.DefaultHostName, id, metahash, width, height, quality);
            if (result.Success)
                return File(result.Data.Imagebytes, result.Data.Mime);
            else
            {
                _logger.LogInformation($"未能获取到照片信息，参数是:host={MongoDBImageProvider.DefaultHostName},id={id},metahash={metahash},width={width},height={height},quality={quality}");
                return NotFound();
            }
        }

        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="id">照片Id</param>
        /// <param name="width">照片宽度</param>
        /// <param name="height">照片高度</param>
        /// <param name="quality">照片质量</param>
        /// <param name="options">附加选项([tgf]{1,3})</param>
        /// <returns></returns>
        [HttpGet("/image/{id:gridfs}/{width:range(0,5000)}x{height:range(0,5000)}/{quality:range(0,100)}/{options:options}")]
        public async Task<IActionResult> GetImageWithoutMetahashAsync(string id, int width, int height, int quality, string options)
        {
            var result = await _imageService.GetImageFromMongoDBAsync(MongoDBImageProvider.DefaultHostName, id, width, height, quality, options);
            if (result.Success)
                return File(result.Data.Imagebytes, result.Data.Mime);
            else
            {
                _logger.LogInformation($"未能获取到照片信息，参数是:host={MongoDBImageProvider.DefaultHostName},id={id},width={width},height={height},quality={quality},options={options}");
                return NotFound();
            }
        }

        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="id">照片Id</param>
        /// <param name="width">照片宽度</param>
        /// <param name="height">照片高度</param>
        /// <param name="quality">照片质量</param>
        /// <returns></returns>
        [HttpGet("/image/{id:gridfs}/{quality:range(0,100)}/{width:range(0,5000)}x{height:range(0,5000)}")]
        public async Task<IActionResult> GetImageFromWithoutMetahashAndOptionsAsync(string id, int width, int height, int quality)
        {
            var result = await _imageService.GetImageFromMongoDBAsync(MongoDBImageProvider.DefaultHostName, id, width, height, quality);
            if (result.Success)
                return File(result.Data.Imagebytes, result.Data.Mime);
            else
            {
                _logger.LogInformation($"未能获取到照片信息，参数是:host={MongoDBImageProvider.DefaultHostName},id={id},width={width},height={height},quality={quality}");
                return NotFound();
            }
        }

        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="id">照片Id</param>
        /// <returns></returns>
        [HttpGet("/image/{id:gridfs}")]
        public async Task<IActionResult> GetRawImageAsync(string id)
        {
            var result = await _imageService.GetRawImageAsync(HostType.GridFs, MongoDBImageProvider.DefaultHostName, id);
            if (result.Success)
                return File(result.Data.Imagebytes, result.Data.Mime);
            else
            {
                _logger.LogInformation($"未能获取到照片信息，参数是:host={MongoDBImageProvider.DefaultHostName},id={id}");
                return NotFound();
            }
        }
        #endregion

        #region 存放图片
        /// <summary>
        /// 保存照片到MongoDB
        /// </summary>
        /// <param name="contentBasedImageInput">照片信息</param>
        /// <returns></returns>
        [HttpPost("/image/contentbased")]
        public async Task<Result<MongoDBImageSaveOutput>> SaveImageByContentAsync(ContentBasedImageSaveInput contentBasedImageInput)
        {
            return await _imageService.SaveImageToMongoDBAsync(hostName: MongoDBImageProvider.DefaultHostName, contentBasedImageInput: contentBasedImageInput);
        }

        /// <summary>
        /// 保存照片到MongoDB
        /// </summary>
        /// <param name="uriBasedImageInput">照片信息</param>
        /// <returns></returns>
        [HttpPost("/image/uribased")]
        public async Task<Result<MongoDBImageSaveOutput>> SaveImageByUriAsync(UriBasedImageSaveInput uriBasedImageInput)
        {
            return await _imageService.SaveImageToMongoDBAsync(hostName: MongoDBImageProvider.DefaultHostName, uriBasedImageInput: uriBasedImageInput);
        }

        #endregion
    }
}
