using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace Ft.ImageServer.Core.Entities
{
    [BsonIgnoreExtraElements]
    public class ImageMetadata : Entity<ObjectId>
    {
        /// <summary>
        /// 图片内容Hash
        /// </summary>
        [BsonElement("ContentHash")]
        public string ContentHash { get; set; }

        /// <summary>
        /// 图片Title
        /// </summary>
        [BsonElement("Title")]
        public string Title { get; set; }

        /// <summary>
        /// 图片描述
        /// </summary>
        [BsonElement("Description")]
        public string Description { get; set; }

        /// <summary>
        /// 图片自定义参数
        /// </summary>
        [BsonElement("Customizations")]
        public List<ImageCustomization> Customizations { get; set; }
    }
}
