using MAS.API.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace MAS.API.Controllers;

[Produces("application/json")]
[Route("api/" + ApiConstants.ServiceName + "/v{api-version:apiVersion}/[controller]")]
[ApiController]
public class BaseController : ControllerBase
{

}
