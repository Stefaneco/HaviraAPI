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
        var createdProfile = _profileService.CreateProfile(clientId, image, name);
        return Created(createdProfile.Id, createdProfile);
	}

    [HttpPost("Photo")]
    public ActionResult CreatePhoto([FromForm] IFormFile image)
    {
        var clientId = Request.Headers["X-MS-CLIENT-PRINCIPAL-ID"].ToString();
        if (clientId is null) return Unauthorized();
        var stream = image.OpenReadStream();
        return Ok(image.ContentType + " " + image.FileName);
    }

    [HttpGet]
    public ActionResult GetUserProfile() {
        var clientId = Request.Headers["X-MS-CLIENT-PRINCIPAL-ID"].ToString();
        if (clientId is null) return Unauthorized();
        var profile = _profileService.GetUserProfileById(clientId);
        return Ok(profile);
    }
}