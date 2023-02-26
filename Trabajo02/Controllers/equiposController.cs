using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trabajo02.Models;
using Microsoft.EntityFrameworkCore;

namespace Trabajo02.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class equiposController : ControllerBase
    {
        private readonly equiposContext equiposContext;

        public equiposController(equiposContext equiposContext)
        {
            this.equiposContext = equiposContext;
        }
    }
}
