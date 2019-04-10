using Ft.ImageServer.Core.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Ft.ImageServer.MongoDB
{
    /// <summary>
    /// 图片服务DbContext
    /// </summary>
    [ConnectionStringName("ThemeParkImageServer")]
    public class ImageServerMongoDbContext: AbpMongoDbContext, IImageServerMongoDbContext
    {
        [MongoCollection("Metadata")] 
        public IMongoCollection<ImageMetadata> ImageMetadata => Collection<ImageMetadata>();

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);
        }
    }
}
