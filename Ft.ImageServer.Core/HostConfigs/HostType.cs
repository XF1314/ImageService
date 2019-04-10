using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ft.ImageServer.Core.HostConfigs
{
    /// <summary>
    /// Host类型
    /// </summary>
    public enum HostType : byte
    {
        /// <summary>
        /// 未知
        /// </summary>
        [Display(Name ="未知")]
        UnKnown = 0,

        /// <summary>
        /// 文件系统
        /// </summary>
        [Display(Name = "文件系统")]
        FileSystem  = 1,

        /// <summary>
        /// MongoDB
        /// </summary>
        [Display(Name = "MongoDB")]
        GridFs = 2,

        /// <summary>
        /// MongoDB
        /// </summary>
        [Display(Name = "网络")]
        Web = 3,
    }
}
