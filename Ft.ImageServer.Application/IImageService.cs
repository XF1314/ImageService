using Ft.ImageServer.Application.Dtos;
using Ft.ImageServer.Core.Entities;
using Ft.ImageServer.Core.HostConfigs;
using Ft.ImageServer.Core.Result;
using Ft.ImageServer.Service.Dtos;
using System.Threading.Tasks;

namespace Ft.ImageServer.Service
{
    public interface IImageService
    {
        /// <summary>
        /// 从MongoDB获取照片信息
        /// </summary>
        /// <param name="hostName">host name</param>
        /// <param name="id">图片Id</param>
        /// <param name="metahash">图片 meta hash</param>
        /// <param name="width">图片宽度</param>
        /// <param name="height">图片高度</param>
        /// <param name="quality">图片质量</param>
        /// <param name="options">附加选项([tgf]{1,3})</param>
        /// <returns></returns>
        Task<Result<ImageDto>> GetImageFromMongoDBAsync(string hostName, string id, string metahash, int width, int height, int quality, string options);

        /// <summary>
        /// 从MongoDB获取照片信息
        /// </summary>
        /// <param name="hostName">host name</param>
        /// <param name="id">图片Id</param>
        /// <param name="metahash">图片meta hash</param>
        /// <param name="width">图片宽度</param>
        /// <param name="height">图片高度</param>
        /// <param name="quality">图片质量</param>
        /// <returns></returns>
        Task<Result<ImageDto>> GetImageFromMongoDBAsync(string hostName, string id, string metahash, int width, int height, int quality);

        /// <summary>
        /// 从MongoDB获取照片信息
        /// </summary>
        /// <param name="hostName">host name</param>
        /// <param name="id">图片Id</param>
        /// <param name="width">图片宽度</param>
        /// <param name="height">图片高度</param>
        /// <param name="quality">图片质量</param>
        /// <param name="options">附加选项([tgf]{1,3})</param>
        /// <returns></returns>
        Task<Result<ImageDto>> GetImageFromMongoDBAsync(string hostName, string id, int width, int height, int quality, string options);

        /// <summary>
        /// 从MongoDB获取照片信息
        /// </summary>
        /// <param name="hostName">host name</param>
        /// <param name="id">图片Id</param>
        /// <param name="width">图片宽度</param>
        /// <param name="height">图片高度</param>
        /// <param name="quality">图片质量</param>
        /// <returns></returns>
        Task<Result<ImageDto>> GetImageFromMongoDBAsync(string hostName, string id, int width, int height, int quality);

        /// <summary>
        /// 从MongoDB获取照片信息
        /// </summary>
        /// <param name="hostName">host name</param>
        /// <param name="id">图片Id</param>
        /// <param name="metahash">图片 meta hash</param>
        /// <param name="quality">图片质量</param>
        /// <param name="options">附加选项([tgf]{1,3})</param>
        /// <returns></returns>
        Task<Result<ImageDto>> GetImageFromMongoDBAsync(string hostName, string id, string metahash, int quality = 100, string options = "");

        /// <summary>
        /// 保存图片到MongoDB
        /// </summary>
        /// <param name="hostName">host name</param>
        /// <param name="contentBasedImageInput">图片信息</param>
        /// <returns></returns>
        Task<Result<MongoDBImageSaveOutput>> SaveImageToMongoDBAsync(string hostName, ContentBasedImageSaveInput contentBasedImageInput);

        /// <summary>
        /// 保存图片到MongoDB
        /// </summary>
        /// <param name="hostName">host name</param>
        /// <param name="uriBasedImageInput">图片信息</param>
        /// <returns></returns>
        Task<Result<MongoDBImageSaveOutput>> SaveImageToMongoDBAsync(string hostName, UriBasedImageSaveInput uriBasedImageInput);

        /// <summary>
        /// 获取图片Metadata（只对MongoDB方式存储的照片有效）
        /// </summary>
        /// <param name="id">图片Id</param>
        /// <returns></returns>
        Task<Result<ImageMetadata>> GetImageMetadataAsync(string id);

        /// <summary>
        /// 从MongoDB获取照片原图
        /// </summary>
        /// <param name="hostType">host type</param>
        /// <param name="hostName">host name </param>
        /// <param name="imageId">image id(对于MongoDB来说对应图片Id，对于Filesystem来说对应图片名，对于Web来说是图片Uri)</param>
        /// <returns></returns>
        Task<Result<ImageDto>> GetRawImageAsync(HostType hostType, string hostName, string imageId);

    }
}
