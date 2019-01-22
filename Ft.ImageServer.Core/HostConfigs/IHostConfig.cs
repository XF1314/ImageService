﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Ft.ImageServer.Core.HostConfigs
{
    public interface IHostConfig
    {
        /// <summary>
        /// Host名称
        /// </summary>
         string HostName { get; set; }

        /// <summary>
        /// Host类型
        /// </summary>
         HostType HostType { get;  }

        /// <summary>
        /// Fallback Image
        /// </summary>
         string FallbackImageUri { get; set; }
    }
}
