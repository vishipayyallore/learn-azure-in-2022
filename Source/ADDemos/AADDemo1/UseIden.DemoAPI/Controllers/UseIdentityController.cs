using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UseIden.DemoAPI.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UseIdentityController : ControllerBase
    {

        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }

    }
}
