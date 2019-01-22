using Ft.ImageServer.Core.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ft.ImageServer.MongoDB
{
    public interface IImageServerMongoDbContext
    {
        IMongoDatabase Database { get; }

        IMongoCollection<ImageMetadata> ImageMetadata { get; }
    }
}
