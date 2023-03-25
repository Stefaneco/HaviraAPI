using System;
namespace HaviraApi.Services;

public interface IProfileService
{
    public void CreateProfile(string userId, IFormFile image, string userName);
}

