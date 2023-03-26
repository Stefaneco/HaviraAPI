using System;
using HaviraApi.Entities;

namespace HaviraApi.Repositories;

public class ProfileRepository : IProfileRepository
{
    private readonly HaviraDbContext _dbContext;

    public ProfileRepository(HaviraDbContext dbContext)
	{
        _dbContext = dbContext;
	}

    public void CreateProfile(string userId, string userName)
    {
        _dbContext.UserProfiles.Add(new UserProfile { Id = userId, Name = userName });
        _dbContext.SaveChanges();
    }

    public UserProfile GetUserProfileById(string userId)
    {
        return _dbContext.UserProfiles.First(u => u.Id == userId);
    }
}

