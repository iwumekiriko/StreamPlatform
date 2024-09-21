using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StreamPlatform.Data;
using StreamPlatform.Models;
using StreamPlatform.ViewModels;
using System;
using Stream = StreamPlatform.Models.Stream;

namespace StreamPlatform.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly AppDbContext _context;

    public AccountController(
        AppDbContext context,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager
    )
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var stream = new Stream()
                {
                    StreamUrl = $"https://localhost/live/{user.UserName}",
                    StreamKey = Guid.NewGuid().ToString(),
                    StreamName = $"{user.UserName}'s Stream",
                    IsActive = false,
                    User = user,
                    PreviewImageUrl = "/uploads/elden.jpg"
                };

                _context.Streams.Add(stream);
                await _context.SaveChangesAsync();

                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Cabinet", "Account");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        return View(model);
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager
                .PasswordSignInAsync(
                model.Email, model.Password,
                model.RememberMe, lockoutOnFailure: false
            );
            if (result.Succeeded)
            {
                return RedirectToAction("Cabinet", "Account");
            }
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        }
        return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Streams");
    }
    [Authorize]
    public async Task<IActionResult> Cabinet()
    {
        var user = await _userManager.GetUserAsync(User);
        var stream = await _context.Streams
            .FirstOrDefaultAsync(s => s.User == user);

        if (stream == null)
        {
            return NotFound("Stream not found for the user.");
        }

        return View(model: stream);
    }
    [Authorize]
    [HttpGet]
    public IActionResult EditProfile()
    {
        var model = new EditProfileViewModel
        {
            UserName = User.Identity.Name
        };
        return View(model);
    }
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> EditProfile(EditProfileViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);
            user.UserName = model.UserName;
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                TempData["SuccessMessage"] = "Имя пользователя успешно изменено!";
            }

            return RedirectToAction("Cabinet");
        }

        return View(model);
    }
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> EditPreview(IFormFile preview)
    {
        var user = await _userManager.GetUserAsync(User);
        var stream = await _context.Streams
            .FirstOrDefaultAsync(s => s.User == user);

        if (stream == null)
        {
            return NotFound("Stream not found");
        }

        if (preview != null && preview.Length > 0)
        {
            var filePath = Path.Combine(
                "wwwroot/uploads",
                $"{Guid.NewGuid()}_{preview.FileName}");
            using (var streamFile = new FileStream(filePath, FileMode.Create))
            {
                await preview.CopyToAsync(streamFile);
            }

            stream.PreviewImageUrl = $"/uploads/{Path.GetFileName(filePath)}";
            _context.Streams.Update(stream);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Cabinet");
    }
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> UpdateStreamName(string streamName)
    {
        if (string.IsNullOrEmpty(streamName))
        {
            return Json(new { success = false, message = "Имя стрима не может быть пустым." });
        }

        var user = await _userManager.GetUserAsync(User);
        var stream = await _context.Streams.FirstOrDefaultAsync(s => s.User == user);

        if (stream == null)
        {
            return Json(new { success = false, message = "Стрим не найден." });
        }

        stream.StreamName = streamName;
        _context.Streams.Update(stream);
        await _context.SaveChangesAsync();

        return RedirectToAction("Cabinet");
    }
}