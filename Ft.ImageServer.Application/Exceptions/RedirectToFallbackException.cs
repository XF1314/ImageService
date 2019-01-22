using System;
using System.Collections.Generic;
using System.Text;

namespace Ft.ImageServer.Service.Exceptions
{
    public class RedirectToFallbackException:Exception
    {
        public  string FallbackImage { get; private set; }

        public RedirectToFallbackException(string fallbackImage, string message) : base(message)
        {
            FallbackImage = fallbackImage;
        }
    }
}
