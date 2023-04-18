using System;
using AutoMapper;
using HaviraApi.Entities;
using HaviraApi.Exceptions;
using HaviraApi.Mappers;
using HaviraApi.Models.Dto;
using HaviraApi.Models.Request;
using HaviraApi.Repositories;
using HaviraApi.Services;
using HaviraApi.Test.Repositories;
using Xunit;

namespace HaviraApi.Test.Services
{
    public class GroupServiceTests
    {
        private readonly IGroupRepository _fakeGroupRepository;
        private readonly IProfileRepository _fakeProfileRepository;
        private readonly IMapper _mapper;
        private readonly GroupService _groupService;

        public GroupServiceTests()
        {
            _fakeGroupRepository = new FakeGroupRepository();
            _fakeProfileRepository = new FakeProfileRepository();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new HaviraMappingProfile()));
            _mapper = config.CreateMapper();

            _groupService = new GroupService(_fakeGroupRepository, _fakeProfileRepository, _mapper);
        }

        [Fact]
        public void CreateGroup_ShouldCreateGroupAndAddUser()
        {
            // Arrange
            var ownerId = "test-owner";
            var userName = "Test Owner";
            _fakeProfileRepository.CreateProfile(ownerId, userName);

            var createGroupRequest = new CreateGroupRequest
            {
                Name = "Test Group"
            };

            // Act
            var createdGroup = _groupService.CreateGroup(createGroupRequest, ownerId);

            // Assert
            Assert.NotNull(createdGroup);
            Assert.Equal(createGroupRequest.Name, createdGroup.Name);
            Assert.Equal(ownerId, createdGroup.OwnerId);

            var fetchedGroup = _fakeGroupRepository.GetGroupById(createdGroup.Id);
            Assert.Single(fetchedGroup.UserProfiles);
            Assert.Contains(fetchedGroup.UserProfiles, up => up.Id == ownerId);
        }

        [Fact]
        public void CreateGroup_ShouldThrowExceptionForInvalidUserId()
        {
            // Arrange
            var ownerId = "invalid-user";
            var createGroupRequest = new CreateGroupRequest
            {
                Name = "Test Group"
            };

            // Act & Assert
            Assert.Throws<NotFoundException>(() => _groupService.CreateGroup(createGroupRequest, ownerId));
        }

        [Fact]
        public void GetGroupById_ShouldReturnGroup()
        {
            // Arrange
            var group = new Group() {
                Name = "Test Group",
                OwnerId = "test-owner",
                CreatedTimestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                JoinCode = "joinCode"
            };
            _fakeGroupRepository.CreateGroup(group);

            // Act
            var fetchedGroup = _groupService.GetGroupById(group.Id);

            // Assert
            Assert.NotNull(fetchedGroup);
            Assert.Equal(group.Name, fetchedGroup.Name);
            Assert.Equal(group.OwnerId, fetchedGroup.OwnerId);
        }

        [Fact]
        public void GetGroupById_NonexistantGroup_ThrowsNotFoundException()
        {
            // Arrange
            var nonexistantGroupId = 9999L;

            // Assert & Act
            Assert.Throws<NotFoundException>(() => _groupService.GetGroupById(nonexistantGroupId));
        }

        [Fact]
        public void GetGroupsByUserId_ShouldReturnAllGroupsForUser()
        {
            // Arrange
            var ownerId = "test-owner";
            var userName = "Test Owner";
            var userProfile = _fakeProfileRepository.CreateProfile(ownerId, userName);

            var group1 = new Group { Name = "Test Group 1", OwnerId = ownerId, UserProfiles = new List<UserProfile>{ userProfile } };
            var group2 = new Group { Name = "Test Group 2", OwnerId = ownerId, UserProfiles = new List<UserProfile> { userProfile } };

            _fakeGroupRepository.CreateGroup(group1);
            _fakeGroupRepository.CreateGroup(group2);

            // Act
            var userGroups = _groupService.GetGroupsByUserId(ownerId);

            // Assert
            Assert.Equal(2, userGroups.Count);
            Assert.Contains(userGroups, g => g.Name == "Test Group 1");
            Assert.Contains(userGroups, g => g.Name == "Test Group 2");
        }

        [Fact]
        public void GetGroupsByUserId_ShouldNotReturnGroupsOfOtherUsers()
        {
            // Arrange
            var ownerId = "test-owner";
            var userName = "Test Owner";
            var userProfile = _fakeProfileRepository.CreateProfile(ownerId, userName);

            var ownerId2 = "test-owner2";
            var userName2 = "Test Owner2";
            var userProfile2 = _fakeProfileRepository.CreateProfile(ownerId2, userName2);

            var group1 = new Group { Name = "Test Group 1", OwnerId = ownerId, UserProfiles = new List<UserProfile> { userProfile } };
            var group2 = new Group { Name = "Test Group 2", OwnerId = ownerId2, UserProfiles = new List<UserProfile> { userProfile2 } };

            _fakeGroupRepository.CreateGroup(group1);
            _fakeGroupRepository.CreateGroup(group2);

            // Act
            var userGroups = _groupService.GetGroupsByUserId(ownerId);

            // Assert
            Assert.Single(userGroups);
            Assert.Contains(userGroups, g => g.Name == "Test Group 1");
        }


        [Fact]
        public void GetGroupsByUserId_ShouldReturnEmptyListForUserWithNoGroups()
        {
            // Arrange
            var ownerId = "test-owner";
            var userName = "Test Owner";
            _fakeProfileRepository.CreateProfile(ownerId, userName);

            // Act
            var userGroups = _groupService.GetGroupsByUserId(ownerId);

            // Assert
            Assert.Empty(userGroups);
        }

        [Fact]
        public void JoinGroup_ShouldJoinGroupSuccessfully()
        {
            // Arrange
            var ownerId = "test-owner";
            var ownerName = "Test Owner";
            var ownerProfile = _fakeProfileRepository.CreateProfile(ownerId, ownerName);

            var group = new Group {
                Name = "Test Group",
                OwnerId = ownerId,
                UserProfiles = new List<UserProfile>() {ownerProfile }
            };
            var createdGroup = _fakeGroupRepository.CreateGroup(group);

            var userId = "test-user";
            var userName = "Test User";
            _fakeProfileRepository.CreateProfile(userId, userName);

            // Act
            var joinedGroup = _groupService.JoinGroup(createdGroup.JoinCode, userId);

            // Assert
            Assert.NotNull(joinedGroup);
            Assert.Equal(createdGroup.Id, joinedGroup.Id);
            var updatedGroup = _fakeGroupRepository.GetGroupById(createdGroup.Id);
            Assert.Contains(updatedGroup.UserProfiles, u => u.Id == userId);
        }

        [Fact]
        public void JoinGroup_ShouldThrowBadRequestException_WhenUserAlreadyInGroup()
        {
            // Arrange
            var ownerId = "test-owner";
            var ownerName = "Test Owner";
            var ownerProfile = _fakeProfileRepository.CreateProfile(ownerId, ownerName);

            var userId = "test-user";
            var userName = "Test User";
            var userProfile = _fakeProfileRepository.CreateProfile(userId, userName);

            var group = new Group
            {
                Name = "Test Group",
                OwnerId = ownerId,
                UserProfiles = new List<UserProfile>() { ownerProfile, userProfile }
            };
            var createdGroup = _fakeGroupRepository.CreateGroup(group);

            // Act & Assert
            Assert.Throws<BadRequestException>(() => _groupService.JoinGroup(createdGroup.JoinCode, userId));
        }

        [Fact]
        public void JoinGroup_ShouldThrowNotFoundException_WhenGroupNotFound()
        {
            // Arrange
            var userId = "test-user";
            var userName = "Test User";
            _fakeProfileRepository.CreateProfile(userId, userName);

            var nonExistentJoinCode = "abcdef";

            // Act & Assert
            Assert.Throws<NotFoundException>(() => _groupService.JoinGroup(nonExistentJoinCode, userId));
        }

        [Fact]
        public void JoinGroup_ShouldThrowBadRequestException_WhenUserProfileDoesntExist()
        {
            // Arrange
            var ownerId = "test-owner";
            var ownerName = "Test Owner";
            var ownerProfile = _fakeProfileRepository.CreateProfile(ownerId, ownerName);

            var userId = "test-user";

            var group = new Group
            {
                Name = "Test Group",
                OwnerId = ownerId,
                UserProfiles = new List<UserProfile>() { ownerProfile }
            };
            var createdGroup = _fakeGroupRepository.CreateGroup(group);

            // Act & Assert
            Assert.Throws<BadRequestException>(() => _groupService.JoinGroup(createdGroup.JoinCode, userId));
        }

        [Fact]
        public void LeaveGroup_ShouldLeaveGroupSuccessfully()
        {
            // Arrange
            var ownerId = "test-owner";
            var ownerName = "Test Owner";
            var ownerProfile = _fakeProfileRepository.CreateProfile(ownerId, ownerName);

            var userId = "test-user";
            var userName = "Test User";
            var userProfile = _fakeProfileRepository.CreateProfile(userId, userName);

            var group = new Group {
                Name = "Test Group",
                OwnerId = ownerId,
                UserProfiles = new List<UserProfile>() { ownerProfile, userProfile }
            };
            var createdGroup = _fakeGroupRepository.CreateGroup(group);

            // Act
            _groupService.LeaveGroup(createdGroup.Id, userId);

            // Assert
            var updatedGroup = _fakeGroupRepository.GetGroupById(createdGroup.Id);
            Assert.Single(updatedGroup.UserProfiles);
            Assert.DoesNotContain(updatedGroup.UserProfiles, u => u.Id == userId);
        }

        [Fact]
        public void LeaveGroup_AsOwner_ShouldThrowBadRequestException()
        {
            // Arrange
            var ownerId = "test-owner";
            var ownerName = "Test Owner";
            var ownerProfile = _fakeProfileRepository.CreateProfile(ownerId, ownerName);

            var group = new Group {
                Name = "Test Group",
                OwnerId = ownerId,
                UserProfiles = new List<UserProfile>() { ownerProfile }
            };
            var createdGroup = _fakeGroupRepository.CreateGroup(group);

            // Act & Assert
            Assert.Throws<BadRequestException>(() => _groupService.LeaveGroup(createdGroup.Id, ownerId));
        }

        [Fact]
        public void LeaveGroup_NotAMember_ShouldThrowBadRequestException()
        {
            // Arrange
            var ownerId = "test-owner";
            var ownerName = "Test Owner";
            var ownerProfile = _fakeProfileRepository.CreateProfile(ownerId, ownerName);

            var group = new Group {
                Name = "Test Group",
                OwnerId = ownerId,
                UserProfiles = new List<UserProfile>() { ownerProfile }
            };
            var createdGroup = _fakeGroupRepository.CreateGroup(group);

            var userId = "test-user";

            // Act & Assert
            Assert.Throws<BadRequestException>(() => _groupService.LeaveGroup(createdGroup.Id, userId));
        }

        [Fact]
        public void LeaveGroup_NonExistentGroup_ShouldThrowNotFoundException()
        {
            // Arrange
            var ownerId = "test-owner";
            var ownerName = "Test Owner";
            var ownerProfile = _fakeProfileRepository.CreateProfile(ownerId, ownerName);

            var userId = "test-user";
            var userName = "Test User";
            var userProfile = _fakeProfileRepository.CreateProfile(userId, userName);

            var group = new Group
            {
                Name = "Test Group",
                OwnerId = ownerId,
                UserProfiles = new List<UserProfile>() { ownerProfile, userProfile }
            };
            _fakeGroupRepository.CreateGroup(group);

            long nonExistentGroupId = 9999;

            // Act & Assert
            Assert.Throws<NotFoundException>(() => _groupService.LeaveGroup(nonExistentGroupId, userId));
        }

        [Fact]
        public void LeaveGroup_MemberOfAnotherGroup_ShouldNotAffectOtherGroupMembership()
        {
            // Arrange
            var ownerId = "test-owner";
            var ownerName = "Test Owner";
            var ownerProfile = _fakeProfileRepository.CreateProfile(ownerId, ownerName);

            var userId = "test-user";
            var userName = "Test User";
            var userProfile = _fakeProfileRepository.CreateProfile(userId, userName);

            var group1 = new Group
            {
                Name = "Test Group 1",
                OwnerId = ownerId,
                UserProfiles = new List<UserProfile>() { ownerProfile, userProfile }
            };
            _fakeGroupRepository.CreateGroup(group1);

            var group2 = new Group
            {
                Name = "Test Group 2",
                OwnerId = ownerId,
                UserProfiles = new List<UserProfile>() { ownerProfile, userProfile }
            };
            _fakeGroupRepository.CreateGroup(group2);

            // Act
            _groupService.LeaveGroup(group1.Id, userId);

            // Assert
            var updatedGroup1 = _fakeGroupRepository.GetGroupById(group1.Id);
            Assert.False(updatedGroup1.UserProfiles.Any(u => u.Id == userId), "User should not be a member of Group 1");

            var updatedGroup2 = _fakeGroupRepository.GetGroupById(group2.Id);
            Assert.True(updatedGroup2.UserProfiles.Any(u => u.Id == userId), "User should still be a member of Group 2");

            var remainingGroups = _groupService.GetGroupsByUserId(userId);
            Assert.Single(remainingGroups);
            Assert.Equal(group2.Id, remainingGroups[0].Id);
        }

        [Fact]
        public void DeleteGroup_ShouldDeleteGroupWhenOwner()
        {
            // Arrange
            var ownerId = "test-owner";
            var ownerName = "Test Owner";
            var ownerProfile = _fakeProfileRepository.CreateProfile(ownerId, ownerName);

            var userId = "test-user";
            var userName = "Test User";
            var userProfile = _fakeProfileRepository.CreateProfile(userId, userName);

            var group = new Group
            {
                Name = "Test Group",
                OwnerId = ownerId,
                UserProfiles = new List<UserProfile>() { ownerProfile, userProfile }
            };
            var createdGroup = _fakeGroupRepository.CreateGroup(group);

            // Act
            _groupService.DeleteGroup(createdGroup.Id, ownerId);

            // Assert
            var exception = Assert.Throws<NotFoundException>(() => _fakeGroupRepository.GetGroupById(createdGroup.Id));
            Assert.Equal("Group not found", exception.Message);
        }

        [Fact]
        public void DeleteGroup_NotOwner_ThrowsBadRequestException()
        {
            // Arrange
            var ownerId = "test-owner";
            var ownerName = "Test Owner";
            var ownerProfile = _fakeProfileRepository.CreateProfile(ownerId, ownerName);

            var userId = "test-user";
            var userName = "Test User";
            var userProfile = _fakeProfileRepository.CreateProfile(userId, userName);

            var group = new Group
            {
                Name = "Test Group",
                OwnerId = ownerId,
                UserProfiles = new List<UserProfile>() { ownerProfile, userProfile }
            };
            var createdGroup = _fakeGroupRepository.CreateGroup(group);

            // Act
            var exception = Assert.Throws<BadRequestException>(() => _groupService.DeleteGroup(createdGroup.Id, userId));

            // Assert
            Assert.Equal("Only owner can delete this group", exception.Message);
        }


    }
}
