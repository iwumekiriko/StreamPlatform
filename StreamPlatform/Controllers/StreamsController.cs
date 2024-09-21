using Microsoft.AspNetCore.Mvc;
using StreamPlatform.Data;
using Stream = StreamPlatform.Models.Stream;
using Microsoft.Extensions.Logging;
using StreamPlatform.ViewModels;
using Microsoft.AspNetCore.Identity;
using StreamPlatform.Models;
using Microsoft.AspNetCore.Authorization;

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
        var streams = _context.Streams.Where(s => s.IsActive).ToList();
        return View(streams);
    }
    public IActionResult Create()
    {
        return View();
    }
    public IActionResult Details(string id)
    {
        var stream = _context.Streams.Find(id);
        return View(stream);
    }
}
