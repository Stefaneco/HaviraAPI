using System;
using System.Security.Claims;
using HaviraApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace HaviraApi.Controllers;

[Route("api/[controller]")]
public class ProfileController : ControllerBase
{
    private readonly IProfileService _profileService;

	public ProfileController(IProfileService profileService)
	{
        _profileService = profileService;
	}

	[HttpPost]
	public ActionResult CreateProfile([FromForm] IFormFile image, [FromForm] string name) {
        var clientId = Request.Headers["X-MS-CLIENT-PRINCIPAL-ID"].ToString();
        if (clientId is null) return Unauthorized();
        _profileService.CreateProfile(clientId, image, name);
        return Ok();
	}

	[HttpGet]
    public ActionResult Test()
    {
        var clientId = Request.Headers["X-MS-CLIENT-PRINCIPAL-ID"].ToString();
        return Ok(clientId);
    }
}