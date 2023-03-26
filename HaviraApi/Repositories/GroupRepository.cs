using System;
using HaviraApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace HaviraApi.Repositories;

public class GroupRepository : IGroupRepository
{
    private readonly HaviraDbContext _dbContext;

	public GroupRepository(HaviraDbContext dbContext)
	{
        _dbContext = dbContext;
	}

    public Group CreateGroup(Group group)
    {
        var userProfile = _dbContext.UserProfiles.FirstOrDefault(u => u.Id == group.OwnerId)
            ?? throw new Exception("User profile not found!");
        group.UserProfiles.Add(userProfile);
        var result = _dbContext.Add(group);
        _dbContext.SaveChanges();
        group.Id = result.Entity.Id;
        group.JoinCode += result.Entity.Id;
        _dbContext.Update(group);
        _dbContext.SaveChanges();
        //var createdGroup = _dbContext.Groups.FirstOrDefault(g => g.Id == group.Id);
        return group;
    }

    public Group GetGroupById(long Id)
    {
        var group = _dbContext.Groups
            .Include(g => g.UserProfiles)
            .Include(g => g.Dishes)
            .First(g => g.Id == Id);
        return group;
    }

    public Group GetGroupByJoinCode(string joinCode)
    {
        var group = _dbContext.Groups
            .Include(g => g.UserProfiles)
            .Include(g => g.Dishes)
            .First(g => g.JoinCode == joinCode);
        return group;
    }

    public List<Group> GetGroupsByUserId(string userId)
    {
        var groups = _dbContext.Groups
            .Include(g => g.UserProfiles)
            //.Include(g => g.Dishes)
            .Where(g => g.UserProfiles.Any(u => u.Id == userId))
            .ToList();
        return groups;
    }

    public void UpdateGroup(Group group)
    {
        _dbContext.Groups.Update(group);
        _dbContext.SaveChanges();
    }
}

