using System;
using System.Collections.Generic;
using System.Text;

namespace Ft.ImageServer.Service.Exceptions
{
    public   class HostNotFoundException: Exception
    {
        public HostNotFoundException(string message) : base(message)
        {
        }
    }
}
