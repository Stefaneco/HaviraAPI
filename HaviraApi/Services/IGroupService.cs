using System;
using HaviraApi.Entities;
using HaviraApi.Models;
using HaviraApi.Models.Dto;
using HaviraApi.Models.Request;
using HaviraApi.Models.Response;

namespace HaviraApi.Services;

public interface IGroupService
{
    public GroupListItemDto CreateGroup(CreateGroupRequest request, string userId);

    public List<GroupListItemDto> GetGroupsByUserId(string userId);

    public GroupDto GetGroupById(long id);

    public GroupListItemDto JoinGroup(string joinCode, string userId);

    public void LeaveGroup(long groupId, string userId);

    public void DeleteGroup(long groupId, string userId);
}

