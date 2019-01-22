using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Ft.ImageServer.Application.Dtos
{
    public class MongoDBImageSaveOutput : EntityDto<Guid>
    {
        /// <summary>
        /// 图片Id
        /// </summary>
        public ObjectId Id { get; set; }

        /// <summary>
        /// 图片内容 hash
        /// </summary>
        public string ContentHash { get; set; }

        /// <summary>
        /// 图片 meta hash
        /// </summary>
        public string MetaHash { get; set; }

    }
}
