using System;
using HaviraApi.Entities;
using HaviraApi.Exceptions;

namespace HaviraApi.Repositories;

public class ProfileRepository : IProfileRepository
{
    private readonly HaviraDbContext _dbContext;

    public ProfileRepository(HaviraDbContext dbContext)
	{
        _dbContext = dbContext;
	}

    public UserProfile CreateProfile(string userId, string userName)
    {
        var createdProfile = _dbContext.UserProfiles.Add(
            new UserProfile { Id = userId, Name = userName }
            );
        _dbContext.SaveChanges();
        return createdProfile.Entity;
    }

    public UserProfile GetUserProfileById(string userId)
    {
        var profile = _dbContext.UserProfiles.FirstOrDefault(u => u.Id == userId)
            ?? throw new NotFoundException("Profile not found");
        return profile;
    }
}

