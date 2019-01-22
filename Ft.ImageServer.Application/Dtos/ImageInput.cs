using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ft.ImageServer.Application.Dtos
{
    public abstract class ImageInput
    {
        /// <summary>
        /// 图片Title
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// 图片描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 图片截取横座标偏移量1
        /// </summary>
        public int? X1;

        /// <summary>
        /// 图片纵取横座标偏移量1
        /// </summary>
        public int? Y1;

        /// <summary>
        /// 图片截取横座标偏移量2
        /// </summary>
        public int? X2;

        /// <summary>
        /// 图片纵取横座标偏移量2
        /// </summary>
        public int? Y2;
    }
}
