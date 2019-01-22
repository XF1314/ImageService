using System;
using System.Collections.Generic;
using System.Text;

namespace Ft.ImageServer.Service.Dtos
{
    public class ImageDto
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Mime类型
        /// </summary>
        public string Mime { get; set; }

        /// <summary>
        /// Imagebytes
        /// </summary>
        public byte[] Imagebytes { get; set; }

    }
}
