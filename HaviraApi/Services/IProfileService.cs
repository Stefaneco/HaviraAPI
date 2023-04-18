using System;
using HaviraApi.Entities;

namespace HaviraApi.Services;

public interface IProfileService
{
    public UserProfile CreateProfile(string userId, IFormFile image, string userName);

    public UserProfile GetUserProfileById(string userId);
}

