using Ft.ImageServer.Core.Domain;
using Ft.ImageServer.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Ft.ImageServer.MongoDB.Repositories
{
    /// <summary>
    /// 图片元数据仓储
    /// </summary>
    public class ImageMetadataRepository : MongoDbRepository<ImageServerMongoDbContext, ImageMetadata, ObjectId>, IImageMetadataRepository
    {
        /// <inheritdoc/>
        public ImageMetadataRepository(IMongoDbContextProvider<ImageServerMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }

        /// <summary>
        /// 根据ContentHash获取记录
        /// </summary>
        /// <param name="contentHash">content hash </param>
        /// <returns></returns>
        public async Task<ImageMetadata> GetByContentHashAsync(string contentHash)
        {
            return await (await Collection.FindAsync(x => x.ContentHash == contentHash)).FirstOrDefaultAsync();
        }

        /// <summary>
        /// 是否存在值为contentHash的记录
        /// </summary>
        /// <param name="contentHash">content hash </param>
        /// <returns></returns>
        public async Task<bool> IsExistAsync(string contentHash)
        {
            return await (await Collection.FindAsync(x => x.ContentHash == contentHash)).AnyAsync();
        }
    }
}
