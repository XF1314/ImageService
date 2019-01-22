using Ft.ImageServer.Application.Dtos;
using Ft.ImageServer.Core.Domain;
using Ft.ImageServer.Core.Entities;
using Ft.ImageServer.Core.Extensions;
using Ft.ImageServer.Core.HostConfigs;
using Ft.ImageServer.Core.Result;
using Ft.ImageServer.Core.Utils;
using Ft.ImageServer.MongoDB;
using Ft.ImageServer.Service.Dtos;
using Ft.ImageServer.Service.ImageProviders;
using ImageMagick;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.MongoDB;

namespace Ft.ImageServer.Service
{
    public class ImageService : IApplicationService, IImageService
    {
        private readonly ILogger<ImageService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpClientFactory _clientFactory;
        private readonly List<MongoDBHostConfig> _mongoDBHostConfigs;
        private readonly IImageMetadataRepository _imageMetadataRepository;

        public ImageService(IImageMetadataRepository imageMetadataRepository,
          IHttpClientFactory clientFactory, IServiceProvider serviceProvider, IOptions<List<MongoDBHostConfig>> mongoDBHostConfigs, ILogger<ImageService> logger)
        {
            _logger = logger;
            _clientFactory = clientFactory;
            _serviceProvider = serviceProvider;
            _mongoDBHostConfigs = mongoDBHostConfigs.Value;
            _imageMetadataRepository = imageMetadataRepository;
        }

        public async Task<Result<ImageDto>> GetImageFromMongoDBAsync(string hostName, string id, string metahash, int width, int height, int quality)
        {
            return await GetImageFromMongoDBAsync(hostName, id, metahash, width, height, quality, string.Empty);
        }

        public async Task<Result<ImageDto>> GetImageFromMongoDBAsync(string hostName, string id, int width, int height, int quality, string options)
        {
            return await GetImageFromMongoDBAsync(hostName, id, string.Empty, width, height, quality, options);
        }

        public async Task<Result<ImageDto>> GetImageFromMongoDBAsync(string hostName, string id, int width, int height, int quality)
        {
            return await GetImageFromMongoDBAsync(hostName, id, string.Empty, width, height, quality, string.Empty);
        }

        public async Task<Result<ImageDto>> GetImageFromMongoDBAsync(string hostName, string id, string metahash, int quality = 100, string options = "")
        {
            var imageDto = new ImageDto { FileName = id };
            var mongoDBImageProvider = _serviceProvider.GetService<MongoDBImageProvider>();
            var imageResult = await mongoDBImageProvider.GetImageAsync(hostName, id);
            if (!imageResult.Success)
                return Result.FromError<ImageDto>(imageResult.Message);

            var imageBytes = imageResult.Data;
            var imageInfo = new MagickImageInfo(imageBytes);
            if (imageInfo.Format == MagickFormat.Gif || imageInfo.Format == MagickFormat.Gif87)
            {
                using (MagickImageCollection collection = new MagickImageCollection(imageBytes))
                {
                    collection.Coalesce();
                    foreach (var magickImage in collection)
                    {
                        var gifImage = (MagickImage)magickImage;
                        ResizeImage(imageInfo.Width, imageInfo.Height, quality, options, gifImage);
                    }
                    collection.RePage();
                    collection.Optimize();
                    collection.OptimizeTransparency();
                    imageDto.Mime = "image/gif";
                    imageDto.Imagebytes = collection.ToByteArray();

                    return Result.FromData(imageDto);
                }
            }
            else
            {
                var imageMetaDataResult = await GetImageMetadataAsync(id);
                if (!imageMetaDataResult.Success)
                    return Result.FromError<ImageDto>(imageMetaDataResult.Message);
                else
                {
                    var imageCustomization = imageMetaDataResult.Data.Customizations.First();
                    if (!string.IsNullOrEmpty(metahash))
                    {
                        imageCustomization = imageMetaDataResult.Data.Customizations.FirstOrDefault(x => x.Hash == metahash);
                        if (imageCustomization == null)
                            return Result.FromError<ImageDto>("无照片原数据信息");
                    }

                    using (var magickImage = new MagickImage(imageBytes))
                    {
                        CropImage(imageCustomization, magickImage);
                        ResizeImage(magickImage.Width, magickImage.Height, quality, options, magickImage);
                        if (magickImage.HasAlpha)
                        {
                            imageDto.Mime = "image/png";
                            imageDto.Imagebytes = magickImage.ToByteArray(MagickFormat.Png);
                        }
                        else
                        {
                            imageDto.Mime = "image/jpeg";
                            imageDto.Imagebytes = magickImage.ToByteArray(MagickFormat.Pjpeg);
                        }

                        return Result.FromData(imageDto);
                    }
                }
            }
        }

        public async Task<Result<ImageDto>> GetImageFromMongoDBAsync(string hostName, string id, string metahash, int width, int height, int quality, string options)
        {
            var imageDto = new ImageDto { FileName = id };
            var mongoDBImageProvider = _serviceProvider.GetService<MongoDBImageProvider>();
            var imageResult = await mongoDBImageProvider.GetImageAsync(hostName, id);
            if (!imageResult.Success)
                return Result.FromError<ImageDto>(imageResult.Message);

            var imageBytes = imageResult.Data;
            var imageInfo = new MagickImageInfo(imageBytes);
            if (imageInfo.Format == MagickFormat.Gif || imageInfo.Format == MagickFormat.Gif87)
            {
                using (MagickImageCollection collection = new MagickImageCollection(imageBytes))
                {
                    collection.Coalesce();
                    foreach (var magickImage in collection)
                    {
                        var gifImage = (MagickImage)magickImage;
                        ResizeImage(width, height, quality, options, gifImage);
                    }
                    collection.RePage();
                    collection.Optimize();
                    collection.OptimizeTransparency();
                    imageDto.Mime = "image/gif";
                    imageDto.Imagebytes = collection.ToByteArray();

                    return Result.FromData(imageDto);
                }
            }
            else
            {
                var imageMetaDataResult = await GetImageMetadataAsync(id);
                if (!imageMetaDataResult.Success)
                    return Result.FromError<ImageDto>(imageMetaDataResult.Message);
                else
                {
                    var imageCustomization = imageMetaDataResult.Data.Customizations.First();
                    if (!string.IsNullOrEmpty(metahash))
                    {
                        imageCustomization = imageMetaDataResult.Data.Customizations.FirstOrDefault(x => x.Hash == metahash);
                        if (imageCustomization == null)
                            return Result.FromError<ImageDto>("无照片原数据信息");
                    }

                    using (var magickImage = new MagickImage(imageBytes))
                    {
                        CropImage(imageCustomization, magickImage);
                        ResizeImage(width, height, quality, options, magickImage);
                        if (magickImage.HasAlpha)
                        {
                            imageDto.Mime = "image/png";
                            imageDto.Imagebytes = magickImage.ToByteArray(MagickFormat.Png);
                        }
                        else
                        {
                            imageDto.Mime = "image/jpeg";
                            imageDto.Imagebytes = magickImage.ToByteArray(MagickFormat.Pjpeg);
                        }

                        return Result.FromData(imageDto);
                    }
                }
            }
        }

        public async Task<Result<ImageDto>> GetRawImageAsync(HostType hostType, string hostName, string imageId)
        {
            switch (hostType)
            {
                case HostType.GridFs:
                    return await GetRawImageFromMongoDBAsync(hostName, imageId);
                default:
                    return Result.FromError<ImageDto>("暂不支持的Host");
            }
        }

        public async Task<Result<ImageMetadata>> GetImageMetadataAsync(string id)
        {
            var imageMetaData = await _imageMetadataRepository.GetAsync(new ObjectId(id));
            if (imageMetaData == null)
                return Result.FromError<ImageMetadata>("未能找到图片原数据信息");
            else
            {
                return Result.FromData(imageMetaData);
            }
        }

        public async Task<Result<MongoDBImageSaveOutput>> SaveImageToMongoDBAsync(string hostName, ContentBasedImageInput contentBasedImageInput)
        {
            var hostConfig = _mongoDBHostConfigs.FirstOrDefault(x => x.HostName.Equals(hostName, StringComparison.OrdinalIgnoreCase));
            if (hostConfig == null)
                return Result.FromError<MongoDBImageSaveOutput>($"HostName:{hostName}无可匹配的HostConfig项");
            else
            {
                var imageInfo = new MagickImageInfo(contentBasedImageInput.ImageBytes);
                var imageCustomization = new ImageCustomization
                {
                    X1 = contentBasedImageInput.X1 ?? 0,
                    X2 = contentBasedImageInput.X2 ?? imageInfo.Width,
                    Y1 = contentBasedImageInput.Y1 ?? 0,
                    Y2 = contentBasedImageInput.Y2 ?? imageInfo.Height
                };
                imageCustomization.ComputeHash();

                var contentHash = contentBasedImageInput.Base64String.ToMD5HashString();
                var imageMetadata = await _imageMetadataRepository.GetByContentHashAsync(contentHash);
                if (imageMetadata != null && imageMetadata.Customizations.Any(x => x.Hash == imageCustomization.Hash))
                {
                    var imageSaveOutput = new MongoDBImageSaveOutput { Id = imageMetadata.Id, ContentHash = contentHash, MetaHash = imageCustomization.Hash };
                    return Result.FromError(imageSaveOutput, "已经存在该图片信息，请勿重复提交");
                }
                else if (imageMetadata != null)//只保存原数据信息即可
                {
                    imageMetadata.Customizations.Add(imageCustomization);
                    await _imageMetadataRepository.UpdateAsync(imageMetadata);
                    var imageSaveOutput = new MongoDBImageSaveOutput { Id = imageMetadata.Id, ContentHash = contentHash, MetaHash = imageCustomization.Hash };
                    return Result.FromData(imageSaveOutput);
                }
                else//需要同时保存图片信息与原数据信息
                {
                    var gridFSBucket = GetGridFSBucket(hostConfig);
                     var fileId = await gridFSBucket.UploadFromBytesAsync(contentBasedImageInput.Title, contentBasedImageInput.ImageBytes);

                    imageMetadata = new ImageMetadata
                    {
                        Id = fileId,
                        ContentHash = contentHash,
                        Title = contentBasedImageInput.Title,
                        Description = contentBasedImageInput.Description,
                        Customizations = new List<ImageCustomization>()
                    };
                    imageMetadata.Customizations.Add(imageCustomization);
                    await _imageMetadataRepository.InsertAsync(imageMetadata);

                    return Result.FromData(new MongoDBImageSaveOutput { Id = fileId, ContentHash = contentHash, MetaHash = imageCustomization.Hash });
                }
            }
        }

        public async Task<Result<MongoDBImageSaveOutput>> SaveImageToMongoDBAsync(string hostName, UriBasedImageInput uriBasedImageInput)
        {
            var imageBytes = (await GetImageBytesFromWebAsync(uriBasedImageInput.Uri)).Data;
            if (imageBytes == null)
                return Result.FromError<MongoDBImageSaveOutput>($"无法从地址:{uriBasedImageInput.Uri}获取到图片数据请确认");
            else
            {
                var contentBasedImageInput = new ContentBasedImageInput
                {
                    Title = uriBasedImageInput.Title,
                    Description = uriBasedImageInput.Description,
                    X1 = uriBasedImageInput.X1,
                    X2 = uriBasedImageInput.X2,
                    Y1 = uriBasedImageInput.Y1,
                    Y2 = uriBasedImageInput.Y2,
                    Base64String = Convert.ToBase64String(imageBytes)
                };

                return await SaveImageToMongoDBAsync(hostName, contentBasedImageInput);
            }
        }

        private async Task<Result<ImageDto>> GetRawImageFromMongoDBAsync(string hostName, string id)
        {
            var mongoDBImageProvider = _serviceProvider.GetService<MongoDBImageProvider>();
            var imageResult = await mongoDBImageProvider.GetImageAsync(hostName, id);
            if (!imageResult.Success)
                return Result.FromError<ImageDto>(imageResult.Message);

            var imageBytes = imageResult.Data;
            var imageInfo = new MagickImageInfo(imageBytes);
            var imageDto = new ImageDto { FileName = id, Imagebytes = imageBytes };
            if (imageInfo.Format == MagickFormat.Gif || imageInfo.Format == MagickFormat.Gif87)
                imageDto.Mime = "image/gif";
            else
            {
                using (var magickImage = new MagickImage(imageBytes))
                {
                    if (magickImage.HasAlpha)
                        imageDto.Mime = "image/png";
                    else
                    {
                        imageDto.Mime = "image/jpeg";
                    }
                }
            }

            return Result.FromData(imageDto);
        }

        private async Task<Result<byte[]>> GetImageBytesFromWebAsync(string imageUri)
        {
            var httpClient = _clientFactory.CreateClient();
            using (var stream = new MemoryStream())
            {
                using (var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, imageUri))
                using (var contentStream = await (await httpClient.SendAsync(httpRequestMessage)).Content.ReadAsStreamAsync())
                {
                    await contentStream.CopyToAsync(stream);
                }

                var imageBytes = stream.TryGetBuffer(out ArraySegment<byte> data) ? data.Array : null;

                return Result.FromData(imageBytes);
            }
        }

        private GridFSBucket GetGridFSBucket(MongoDBHostConfig mongoDBHostConfig)
        {
            if (mongoDBHostConfig.HostName.Equals(MongoDBImageProvider.DefaultHostName, StringComparison.OrdinalIgnoreCase))
                return new GridFSBucket(_serviceProvider.GetService<IMongoDbContextProvider<ImageServerMongoDbContext>>().GetDbContext().Database);
            else
            {
                var mongoClient = new MongoClient(mongoDBHostConfig.ConnectionString);
                var database = mongoClient.GetDatabase(mongoDBHostConfig.DatabaseName);
                return new GridFSBucket(database);
            }
        }

        private static void ResizeImage(int width, int height, int quality, string options, IMagickImage magickImage)
        {
            magickImage.Quality = quality;
            magickImage.Strip();

            if (options.Contains("g")) //grayscale
                magickImage.Grayscale(PixelIntensityMethod.Average);
            if (width == magickImage.BaseWidth && height == magickImage.BaseHeight)
                return;
            else if (options.Contains("f") || options.Contains("t"))
                magickImage.Resize(width, height);
            else
            {
                var magickGeometry = new MagickGeometry(width, height)
                {
                    IgnoreAspectRatio = false, //保持长宽比
                    FillArea = true
                };
                magickImage.Resize(magickGeometry);
                magickImage.Crop(magickGeometry, Gravity.Center);
            }
        }
        private static void CropImage(ImageCustomization imageCustomization, IMagickImage magickImage)
        {
            var cropWidth = Math.Abs(imageCustomization.X2 - imageCustomization.X1);
            var cropHeight = Math.Abs(imageCustomization.Y2 - imageCustomization.Y1);
            if ((cropWidth <= 0 || cropHeight <= 0)||cropWidth == magickImage.BaseWidth && cropHeight == magickImage.BaseHeight)
                return;
            else
            {
                var magickGeometry = new MagickGeometry(cropWidth, cropHeight)
                {
                    IgnoreAspectRatio = false, //保持长宽比
                    FillArea = false,
                    X = imageCustomization.X1,
                    Y = imageCustomization.Y1
                };

                magickImage.Crop(magickGeometry);
            }
        }
    }
}
