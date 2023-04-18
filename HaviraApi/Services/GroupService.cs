using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using HaviraApi.Entities;
using HaviraApi.Exceptions;
using HaviraApi.Models.Dto;
using HaviraApi.Models.Request;
using HaviraApi.Models.Response;
using HaviraApi.Repositories;

namespace HaviraApi.Services;

public class GroupService : IGroupService
{
    private readonly IGroupRepository _groupRepository;
    private readonly IProfileRepository _profileRepository;
    private readonly IMapper _mapper;

    public GroupService(
        IGroupRepository groupRepository,
        IProfileRepository profileRepository,
        IMapper mapper)
	{
        _groupRepository = groupRepository;
        _profileRepository = profileRepository;
        _mapper = mapper;
    }

    public GroupListItemDto CreateGroup(CreateGroupRequest request, string userId)
    {
        var userProfile = _profileRepository.GetUserProfileById(userId);
        var group = new Group()
        {
            Name = request.Name,
            OwnerId = userId,
            JoinCode = GenerateRandomString(6),
            CreatedTimestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
            UserProfiles = new List<UserProfile>(),
            Dishes = new List<Dish>()
        };
        group.UserProfiles.Add(userProfile);
        var createdGroup = _groupRepository.CreateGroup(group);

        return _mapper.Map<GroupListItemDto>(createdGroup);
    }

    public GroupDto GetGroupById(long id)
    {
        var group = _groupRepository.GetGroupById(id);
        return _mapper.Map<GroupDto>(group);
    }

    public List<GroupListItemDto> GetGroupsByUserId(string userId)
    {
        var groups = _groupRepository.GetGroupsByUserId(userId);
        return _mapper.Map<List<GroupListItemDto>>(groups);
    }

    public GroupListItemDto JoinGroup(string joinCode, string userId)
    {
        var group = _groupRepository.GetGroupByJoinCode(joinCode);
        var isUserAleardyInGroup = group.UserProfiles.Exists(u => u.Id == userId);
        if (isUserAleardyInGroup) throw new BadRequestException("User is already a member of this group");
        try {
            var userProfile = _profileRepository.GetUserProfileById(userId);
            group.UserProfiles.Add(userProfile);
        } catch(NotFoundException e) {
            throw new BadRequestException("User did not create profile");
        }
        _groupRepository.UpdateGroup(group);
        return _mapper.Map<GroupListItemDto>(group);
    }

    private string GenerateRandomString(int size)
    {
        char[] chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();

        byte[] data = new byte[4 * size];
        using (var crypto = RandomNumberGenerator.Create())
        {
            crypto.GetBytes(data);
        }
        StringBuilder result = new StringBuilder(size);
        for (int i = 0; i < size; i++)
        {
            var rnd = BitConverter.ToUInt32(data, i * 4);
            var idx = rnd % chars.Length;
        
            result.Append(chars[idx]);
        }

        return result.ToString();
    }
}

