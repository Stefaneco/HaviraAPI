using System;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Azure.Storage.Blobs;
using HaviraApi.Entities;
using HaviraApi.Models;
using HaviraApi.Models.Dto;
using HaviraApi.Models.Request;
using HaviraApi.Models.Response;
using HaviraApi.Repositories;

namespace HaviraApi.Services;

public class GroupService : IGroupService
{
    //private readonly BlobServiceClient _blobServiceClient;
    private readonly IGroupRepository _groupRepository;
    private readonly IMapper _mapper;

    public GroupService(IGroupRepository groupRepository, IMapper mapper)
	{
        _groupRepository = groupRepository;
        _mapper = mapper;
    }

    public GroupListItemDto CreateGroup(CreateGroupRequest request, string userId)
    {
        //var containerClient = _blobServiceClient.GetBlobContainerClient("profilepictures");

        var group = new Group()
        {
            Name = request.Name,
            OwnerId = userId,
            JoinCode = GenerateRandomString(6),
            CreatedTimestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
            UserProfiles = new List<UserProfile>(),
            Dishes = new List<Dish>()
        };
        var createdGroup = _groupRepository.CreateGroup(group);

        return _mapper.Map<GroupListItemDto>(createdGroup);
    }

    public GroupDto GetGroupById(long id)
    {
        var group = _groupRepository.GetGroup(id);
        return _mapper.Map<GroupDto>(group);
    }

    public List<GroupListItemDto> GetGroupsByUserId(string userId)
    {
        var groups = _groupRepository.GetGroupsByUserId(userId);
        return _mapper.Map<List<GroupListItemDto>>(groups);
        throw new NotImplementedException();
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

