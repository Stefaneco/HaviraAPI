using System;
using HaviraApi.Entities;

namespace HaviraApi.Repositories;

public interface IGroupRepository
{

    public Group CreateGroup(Group group);

    public Group GetGroup(long Id);

    public List<Group> GetGroupsByUserId(string userId);
}

