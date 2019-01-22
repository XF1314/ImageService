using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ft.ImageServer.Core.HostConfigs;
using Ft.ImageServer.Core.Result;
using Ft.ImageServer.MongoDB;
using Ft.ImageServer.Service.Exceptions;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using Volo.Abp.MongoDB;
using Volo.Abp.Uow;

namespace Ft.ImageServer.Service.ImageProviders
{
    public class MongoDBImageProvider : IImageProvider
    {
        public const string DefaultHostName = "default";
        private readonly List<MongoDBHostConfig> _mongoDBHostConfigs;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IMongoDbContextProvider<ImageServerMongoDbContext> _mongoDbContextProvider;

        public MongoDBImageProvider(IOptions<List<MongoDBHostConfig>> mongoDBHostConfigs,
           IUnitOfWorkManager unitOfWorkManager, IMongoDbContextProvider<ImageServerMongoDbContext> mongoDbContextProvider)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _mongoDBHostConfigs = mongoDBHostConfigs.Value;
            _mongoDbContextProvider = mongoDbContextProvider;
        }


        public async Task<Result<byte[]>> GetImageAsync(string hostName, string fileName)
        {
            byte[] imageBytes = null;
            var hostConfig = _mongoDBHostConfigs.FirstOrDefault(x => x.HostName.Equals(hostName, StringComparison.OrdinalIgnoreCase));
            if (hostConfig == null)
                return Result.FromError<byte[]>($"HostName:{hostName}无可匹配的HostConfig项");
            else
            {
                var dotIndex = fileName.LastIndexOf(",");
                fileName = dotIndex == -1 ? fileName : fileName.Substring(0, dotIndex);
                var gridFSBucket = GetGridFSBucket(hostConfig);
                try
                {
                    var objectId = new ObjectId(fileName);
                    if (objectId != ObjectId.Empty)
                        imageBytes = await gridFSBucket.DownloadAsBytesAsync(objectId);
                }
                catch (ArgumentException exception)
                { throw new InvalidGridFsObjectIdException(exception.Message); }
                catch (FormatException exception)
                { throw new InvalidGridFsObjectIdException(exception.Message); }
                catch (Exception exception)
                {
                    if (!(exception is GridFSFileNotFoundException) && !(exception is IndexOutOfRangeException))
                        throw;
                }

                if (imageBytes == null && !string.IsNullOrWhiteSpace(hostConfig.FallbackImageUri))
                    throw new RedirectToFallbackException(hostConfig.FallbackImageUri, "图片未找到, 重定向到fallback image");
                else
                {
                    return Result.FromData(imageBytes);
                }
            }
        }

        private GridFSBucket GetGridFSBucket(MongoDBHostConfig mongoDBHostConfig)
        {
            var ss = _unitOfWorkManager.Current;
            if (mongoDBHostConfig.HostName.Equals(DefaultHostName, StringComparison.OrdinalIgnoreCase))
                return new GridFSBucket(_mongoDbContextProvider.GetDbContext().Database);
            else
            {
                var mongoClient = new MongoClient(mongoDBHostConfig.ConnectionString);
                var database = mongoClient.GetDatabase(mongoDBHostConfig.DatabaseName);
                return new GridFSBucket(database);
            }
        }
    }
}
