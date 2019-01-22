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
    public class ImageMetadataRepository : MongoDbRepository<ImageServerMongoDbContext, ImageMetadata, ObjectId>, IImageMetadataRepository
    {
        public ImageMetadataRepository(IMongoDbContextProvider<ImageServerMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }

        public async Task<ImageMetadata> GetByContentHashAsync(string contentHash)
        {
            return await (await Collection.FindAsync(x => x.ContentHash == contentHash)).FirstOrDefaultAsync();
        }

        public async Task<bool> IsExistAsync(string contentHash)
        {
            return await (await Collection.FindAsync(x => x.ContentHash == contentHash)).AnyAsync();
        }
    }
}
