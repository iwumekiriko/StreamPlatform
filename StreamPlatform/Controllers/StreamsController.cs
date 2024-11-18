using Microsoft.AspNetCore.Mvc;
using StreamPlatform.Data;
using Stream = StreamPlatform.Models.Stream;
using Microsoft.AspNetCore.Identity;
using StreamPlatform.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace StreamPlatform.Controllers;

public class StreamsController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AppDbContext _context;
    private readonly ILogger<StreamsController> _logger;
    public StreamsController(
        UserManager<ApplicationUser> userManager,
        AppDbContext context,
        ILogger<StreamsController> logger)
    {
        _userManager = userManager;
        _context = context;
        _logger = logger;
    }
    public IActionResult Index()
    {
        var streams = _context.Streams
            .Where(s => s.IsActive)
            .Include(s => s.User)
            .ToList();
        return View(streams);
    }
    public IActionResult Create()
    {
        return View();
    }
    public IActionResult Details(string username)
    {
        Stream? stream = _context.Streams
            .Where(s => s.User.UserName == username)
            .Include(s => s.User)
            .FirstOrDefault();
        if (stream == null)
        {
            return RedirectToAction("NotExistingStream", "Errors");
        }
        if (!stream.IsActive)
        {
            return RedirectToAction("NotActiveStream", "Errors");
        }

        return View(stream);
    }
    public IActionResult GoOnline()
    {
        Stream? stream = _context.Streams
            .Where(s => s.User.UserName == User.Identity.Name)
            .FirstOrDefault();

        if (stream == null)
        {
            return RedirectToAction("NotExistingStream", "Errors");
        }

        stream.IsActive = true;
        _context.SaveChanges();


        Log.Information($"{User.Identity.Name} started Stream");
        return RedirectToAction("Details",
            new {username = User.Identity.Name});
    }

    public IActionResult GoOffline()
    {
        Stream? stream = _context.Streams
            .Where(s => s.User.UserName == User.Identity.Name)
            .FirstOrDefault();

        if (stream == null)
        {
            return RedirectToAction("NotExistingStream", "Errors");
        }

        stream.IsActive = false;
        _context.SaveChanges();


        Log.Information($"{User.Identity.Name} stopped Stream");
        return RedirectToAction("Cabinet", "Account");
    }
}
