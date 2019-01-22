using Ft.ImageServer.Core.Entities;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Ft.ImageServer.Core.Domain
{
    public interface IImageMetadataRepository : IBasicRepository<ImageMetadata, ObjectId>
    {
        /// <summary>
        /// 是否存在值为contentHash的记录
        /// </summary>
        /// <param name="contentHash">content hash </param>
        /// <returns></returns>
        Task<bool> IsExistAsync(string contentHash);

        /// <summary>
        /// 根据ContentHash获取记录
        /// </summary>
        /// <param name="contentHash">content hash </param>
        /// <returns></returns>
        Task<ImageMetadata> GetByContentHashAsync(string contentHash);
    }
}
