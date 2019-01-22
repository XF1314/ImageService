using Ft.ImageServer.Core.Utils;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace Ft.ImageServer.Core.Entities
{
    public class ImageCustomization
    {
        [BsonElement("Hash")]
        public string Hash;

        /// <summary>
        /// 图片截取横座标偏移量1
        /// </summary>
        [BsonElement("X1")]
        public int X1;

        /// <summary>
        /// 图片纵取横座标偏移量1
        /// </summary>
        [BsonElement("Y1")]
        public int Y1;

        /// <summary>
        /// 图片截取横座标偏移量2
        /// </summary>
        [BsonElement("X2")]
        public int X2;

        /// <summary>
        /// 图片纵取横座标偏移量2
        /// </summary>
        [BsonElement("Y2")]
        public int Y2;

        public void ComputeHash()
        {
            Hash = $"{X1}{Y1}{X2}{Y2}".ToMD5HashString();
        }
    }
}
