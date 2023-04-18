using HaviraApi.Entities;
using HaviraApi.Exceptions;
using HaviraApi.Repositories;

namespace HaviraApi.Test.Repositories;

public class FakeGroupRepository : IGroupRepository
{
    private readonly Dictionary<long, Group> _groups = new();
    private long _nextGroupId = 1;

    public Group CreateGroup(Group group)
    {
        group.Id = _nextGroupId++;
        group.JoinCode += group.Id;
        group.UserProfiles = group.UserProfiles;
        group.Dishes = new List<Dish>();
        _groups[group.Id] = group;
        return group;
    }

    public Group GetGroupById(long id)
    {
        if (_groups.TryGetValue(id, out var group))
        {
            return group;
        }
        throw new NotFoundException("Group not found");
    }

    public Group GetGroupByJoinCode(string joinCode)
    {
        var group = _groups.Values.FirstOrDefault(g => g.JoinCode == joinCode);
        if (group != null)
        {
            return group;
        }
        throw new NotFoundException("Group not found");
    }

    public List<Group> GetGroupsByUserId(string userId)
    {
        return _groups.Values.Where(g => g.UserProfiles.Any(u => u.Id == userId)).ToList();
    }

    public void UpdateGroup(Group group)
    {
        if (_groups.ContainsKey(group.Id))
        {
            _groups[group.Id] = group;
        }
        else
        {
            throw new NotFoundException("Group not found");
        }
    }
}
