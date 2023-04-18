using HaviraApi.Entities;
using HaviraApi.Exceptions;
using HaviraApi.Repositories;

namespace HaviraApi.Test.Repositories;

public class FakeProfileRepository : IProfileRepository
{
    private readonly Dictionary<string, UserProfile> _userProfiles = new();

    public UserProfile CreateProfile(string userId, string userName)
    {
        var newUserProfile = new UserProfile { Id = userId, Name = userName };
        _userProfiles[userId] = newUserProfile;
        return newUserProfile;
    }

    public UserProfile GetUserProfileById(string userId)
    {
        if (_userProfiles.TryGetValue(userId, out var userProfile))
        {
            return userProfile;
        }
        throw new NotFoundException("Profile not found");
    }
}