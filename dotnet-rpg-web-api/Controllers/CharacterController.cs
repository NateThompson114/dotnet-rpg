using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_rpg_web_api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CharacterController : ControllerBase
{
    private static Character Knight = new Character();

    [HttpGet(Name = "GetKnight")]
    public ActionResult<Character> Get()
    {
        return Ok(Knight);
    }
}