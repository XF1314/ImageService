using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Ft.ImageServer.Host.Controllers
{
    /// <summary>
    /// Home
    /// </summary>
    [AllowAnonymous, ApiExplorerSettings(IgnoreApi = true)]
    public class HomeController : ControllerBase
    {
        /// <summary>
        /// Index
        /// </summary>
        public ActionResult Index()
        {
            return Redirect("/swagger");
        }
    }
}
