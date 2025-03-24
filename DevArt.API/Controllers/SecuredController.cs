using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevArt.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SecuredController : ControllerBase
{
    
}