using Ft.ImageServer.Application.Dtos;
using Ft.ImageServer.Core.HostConfigs;
using Ft.ImageServer.Core.Result;
using Ft.ImageServer.Service;
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
    /// 基于host的图片
    /// </summary>
    [AllowAnonymous, ApiController]
    public class HostBasedImageController : AbpController
    {
        private readonly IImageService _imageService;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<HostBasedImageController> _logger;

        /// <inheritdoc/>
        public HostBasedImageController(IImageService imageService,
            IServiceProvider serviceProvider, ILogger<HostBasedImageController> logger, IUnitOfWorkManager unitOfWorkManager)
        {
            _logger = logger;
            _imageService = imageService;
            _serviceProvider = serviceProvider;
            UnitOfWorkManager = unitOfWorkManager;
        }

        #region 获取图片 支持从m（MongoDB）,f（文件系统）,w（网络）三种途径获取图片
        /// <summary>
        /// 从MongoDB获取图片
        /// </summary>
        /// <param name="host">host name</param>
        /// <param name="id">照片Id</param>
        /// <param name="metahash">照片 meta hash</param>
        /// <param name="width">照片宽度</param>
        /// <param name="height">照片高度</param>
        /// <param name="quality">照片质量</param>
        /// <param name="options">附加选项([tgf]{1,3})</param>
        /// <returns></returns>
        [HttpGet("/image/m/{host}/{id:gridfs}/h-{metahash:metahash}/{width:range(0,5000)}x{height:range(0,5000)}/{quality:range(0,100)}/{options:options}")]
        public async Task<IActionResult> GetImageFromMongoDBAsync(string host, string id, string metahash, int width, int height, int quality, string options)
        {
            var result = await _imageService.GetImageFromMongoDBAsync(host, id, metahash, width, height, quality, options);
            if (result.Success)
                return File(result.Data.Imagebytes, result.Data.Mime);
            else
            {
                _logger.LogInformation($"未能获取到照片信息，参数是:host={host},id={id},metahash={metahash},width={width},height={height},quality={quality},options={options}");
                return NotFound();
            }
        }

        /// <summary>
        /// 从MongoDB获取图片
        /// </summary>
        /// <param name="host">host name</param>
        /// <param name="id">照片Id</param>
        /// <param name="metahash">照片 meta hash</param>
        /// <param name="quality">照片质量</param>
        /// <returns></returns>
        [HttpGet("/image/m/{host}/{id:gridfs}/h-{metahash:metahash}/{quality:range(0,100)}")]
        public async Task<IActionResult> GetImageFromMongoDBWithoutOptionsAsync(string host, string id, string metahash, int quality)
        {
            var result = await _imageService.GetImageFromMongoDBAsync(host, id, metahash, quality);
            if (result.Success)
                return File(result.Data.Imagebytes, result.Data.Mime);
            else
            {
                _logger.LogInformation($"未能获取到照片信息，参数是:host={host},id={id},metahash={metahash},quality={quality}");
                return NotFound();
            }
        }

        /// <summary>
        /// 从MongoDB获取图片
        /// </summary>
        /// <param name="host">host name</param>
        /// <param name="id">照片Id</param>
        /// <param name="metahash">照片 meta hash</param>
        /// <param name="quality">照片质量</param>
        /// <param name="options">附加选项([tgf]{1,3})</param>
        /// <returns></returns>
        [HttpGet("/image/m/{host}/{id:gridfs}/h-{metahash:metahash}/{quality:range(0,100)}/{options:options}")]
        public async Task<IActionResult> GetImageFromMongoDBWithoutOptionsAsync(string host, string id, string metahash, int quality, string options)
        {
            var result = await _imageService.GetImageFromMongoDBAsync(host, id, metahash, quality, options);
            if (result.Success)
                return File(result.Data.Imagebytes, result.Data.Mime);
            else
            {
                _logger.LogInformation($"未能获取到照片信息，参数是:host={host},id={id},metahash={metahash},quality={quality},options={options}");
                return NotFound();
            }
        }

        /// <summary>
        /// 从MongoDB获取图片
        /// </summary>
        /// <param name="host">host name</param>
        /// <param name="id">照片Id</param>
        /// <param name="metahash">照片 meta hash</param>
        /// <param name="width">照片宽度</param>
        /// <param name="height">照片高度</param>
        /// <param name="quality">照片质量</param>
        /// <returns></returns>
        [HttpGet("/image/m/{host}/{id:gridfs}/h-{metahash:metahash}/{quality:range(0,100)}/{width:range(0,5000)}x{height:range(0,5000)}")]
        public async Task<IActionResult> GetImageFromMongoDBWithoutOptionsAsync(string host, string id, string metahash, int width, int height, int quality)
        {
            var result = await _imageService.GetImageFromMongoDBAsync(host, id, metahash, width, height, quality);
            if (result.Success)
                return File(result.Data.Imagebytes, result.Data.Mime);
            else
            {
                _logger.LogInformation($"未能获取到照片信息，参数是:host={host},id={id},metahash={metahash},width={width},height={height},quality={quality}");
                return NotFound();
            }
        }

        /// <summary>
        /// 从MongoDB获取图片
        /// </summary>
        /// <param name="host">host name</param>
        /// <param name="id">照片Id</param>
        /// <param name="width">照片宽度</param>
        /// <param name="height">照片高度</param>
        /// <param name="quality">照片质量</param>
        /// <param name="options">附加选项([tgf]{1,3})</param>
        /// <returns></returns>
        [HttpGet("/image/m/{host}/{id:gridfs}/{width:range(0,5000)}x{height:range(0,5000)}/{quality:range(0,100)}/{options:options}")]
        public async Task<IActionResult> GetImageFromMongoDBWithoutMetahashAsync(string host, string id, int width, int height, int quality, string options)
        {
            var result = await _imageService.GetImageFromMongoDBAsync(host, id, width, height, quality, options);
            if (result.Success)
                return File(result.Data.Imagebytes, result.Data.Mime);
            else
            {
                _logger.LogInformation($"未能获取到照片信息，参数是:host={host},id={id},width={width},height={height},quality={quality},options={options}");
                return NotFound();
            }
        }

        /// <summary>
        /// 从MongoDB获取图片
        /// </summary>
        /// <param name="host">host name</param>
        /// <param name="id">照片Id</param>
        /// <param name="width">照片宽度</param>
        /// <param name="height">照片高度</param>
        /// <param name="quality">照片质量</param>
        /// <returns></returns>
        [HttpGet("/image/m/{host}/{id:gridfs}/{quality:range(0,100)}/{width:range(0,5000)}x{height:range(0,5000)}")]
        public async Task<IActionResult> GetImageFromMongoDBWithoutMetahashAndOptionsAsync(string host, string id, int width, int height, int quality)
        {
            var result = await _imageService.GetImageFromMongoDBAsync(host, id, width, height, quality);
            if (result.Success)
                return File(result.Data.Imagebytes, result.Data.Mime);
            else
            {
                _logger.LogInformation($"未能获取到照片信息，参数是:host={host},id={id},width={width},height={height},quality={quality}");
                return NotFound();
            }
        }

        /// <summary>
        /// 从MongoDB获取图片
        /// </summary>
        /// <param name="host">host name</param>
        /// <param name="id">照片Id</param>
        /// <returns></returns>
        [HttpGet("/image/m/{host}/{id:gridfs}")]
        public async Task<IActionResult> GetRawImageAsync(string host, string id)
        {
            var result = await _imageService.GetRawImageAsync(HostType.GridFs, host, id);
            if (result.Success)
                return File(result.Data.Imagebytes, result.Data.Mime);
            else
            {
                _logger.LogInformation($"未能获取到照片信息，参数是:host={host},id={id}");
                return NotFound();
            }
        }

        ///// <summary>
        ///// 从文件系统获取照片(暂未实现)
        ///// </summary>
        ///// <param name="host">host name</param>
        ///// <param name="filepath">文件路径</param>
        ///// <returns></returns>
        //[HttpGet("/image/f/{host}/{*filepath}")]
        //public async Task<IActionResult> GetImageFromFileSystemAsync(string host, string filepath)
        //{
        //    return NotFound();
        //}

        ///// <summary>
        ///// 从Web获取照片(暂未实现)
        ///// </summary>
        ///// <param name="host">host name</param>
        ///// <param name="filepath">文件路径</param>
        ///// <returns></returns>
        //[HttpGet("/image/w/{host}/{*filepath}")]
        //public async Task<IActionResult> GetImageFromWebAsync(string host, string filepath)
        //{
        //    return NotFound();
        //}

        #endregion

        #region 存放照片，支持存放照片到m（MongoDB）,f（文件系统）
        /// <summary>
        /// 保存照片到MongoDB
        /// </summary>
        /// <param name="host">host name</param>
        /// <param name="contentBasedImageInput">照片信息</param>
        /// <returns></returns>
        [HttpPost("/image/m/{host}/contentbased")]
        public async Task<Result<MongoDBImageSaveOutput>> SaveImageToMongoDBByContentAsync(string host, ContentBasedImageSaveInput contentBasedImageInput)
        {
            return await _imageService.SaveImageToMongoDBAsync(hostName: host, contentBasedImageInput: contentBasedImageInput);
        }

        /// <summary>
        /// 保存照片到MongoDB
        /// </summary>
        /// <param name="host">host name</param>
        /// <param name="uriBasedImageInput">照片信息</param>
        /// <returns></returns>
        [HttpPost("/image/m/{host}/uribased")]
        public async Task<Result<MongoDBImageSaveOutput>> SaveImageToMongoDBByUriAsync(string host, UriBasedImageSaveInput uriBasedImageInput)
        {
            return await _imageService.SaveImageToMongoDBAsync(hostName: host, uriBasedImageInput: uriBasedImageInput);
        }

        ///// <summary>
        ///// 保存照片到文件系统（暂未实现）
        ///// </summary>
        ///// <param name="host">host name</param>
        ///// <param name="contentBasedImageInput">照片信息</param>
        ///// <returns></returns>
        //[HttpPost("/image/f/{host}/contentbased")]
        //public async Task<Result<string>> SaveImageToFileSystemByContentAsync(string host, ContentBasedImageInput contentBasedImageInput)
        //{
        //    return Result.FromData(string.Empty);
        //}

        ///// <summary>
        ///// 保存照片到文件系统（暂未实现）
        ///// </summary>
        //[HttpPost("/image/f/{host}/uribased")]
        //public async Task<Result<string>> SaveImageToFileSystemByUriAsync(UriBasedImageInput uriBasedImageInput)
        //{
        //    return Result.FromData(string.Empty);
        //}

        #endregion
    }
}
