using System;
using HaviraApi.Entities;

namespace HaviraApi.Repositories;

public interface IGroupRepository
{

    public Group CreateGroup(Group group);

    public Group GetGroupById(long Id);

    public List<Group> GetGroupsByUserId(string userId);

    public Group GetGroupByJoinCode(string joinCode);

    public void UpdateGroup(Group group);
}

