using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace LootTrainer.Controllers
{
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public class BaseController : ControllerBase
    {
    }
}
