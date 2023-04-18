using System;
using HaviraApi.Entities;
using HaviraApi.Exceptions;
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
        var result = _dbContext.Add(group);
        _dbContext.SaveChanges();
        group.Id = result.Entity.Id;
        group.JoinCode += result.Entity.Id;
        _dbContext.Update(group);
        _dbContext.SaveChanges();
        return group;
    }

    public void DeleteGroup(Group group)
    {
        _dbContext.Groups.Remove(group);
        _dbContext.SaveChanges();
    }

    public Group GetGroupById(long Id)
    {
        var group = _dbContext.Groups
            .Include(g => g.UserProfiles)
            .Include(g => g.Dishes)
            .First(g => g.Id == Id)
            ?? throw new NotFoundException("Group not found");
        return group;
    }

    public Group GetGroupByJoinCode(string joinCode)
    {
        var group = _dbContext.Groups
            .Include(g => g.UserProfiles)
            .Include(g => g.Dishes)
            .First(g => g.JoinCode == joinCode)
            ?? throw new NotFoundException("Group not found");
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

