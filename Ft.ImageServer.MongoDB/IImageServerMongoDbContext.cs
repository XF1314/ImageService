using Ft.ImageServer.Core.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ft.ImageServer.MongoDB
{
    /// <summary>
    /// 图片服务DbContext接口
    /// </summary>
    public interface IImageServerMongoDbContext
    {
        IMongoDatabase Database { get; }

        IMongoCollection<ImageMetadata> ImageMetadata { get; }
    }
}
