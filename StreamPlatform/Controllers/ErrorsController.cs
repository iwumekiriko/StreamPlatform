using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace StreamPlatform.Controllers;

public class ErrorsController : Controller
{
    public IActionResult NotActiveStream()
    {
        Log.Warning("Not active");
        return View();
    }
    public IActionResult NotExistingStream()
    {
        Log.Warning("Not Found");
        return View();
    }
}
