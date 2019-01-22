using System;
using System.Collections.Generic;
using System.Text;

namespace Ft.ImageServer.Service.Exceptions
{
     public class InvalidGridFsObjectIdException: Exception
    {
        public InvalidGridFsObjectIdException(string message) : base(message)
        {
        }
    }
}
