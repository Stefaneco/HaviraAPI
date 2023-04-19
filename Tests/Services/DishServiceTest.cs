using HaviraApi.Test.Repositories;
using HaviraApi.Services;
using HaviraApi.Models.Request;
using HaviraApi.Exceptions;
using HaviraApi.Entities;

namespace HaviraApi.Tests.Services;

public class DishServiceTests
{
    private readonly DishService _dishService;
    private readonly FakeDishRepository _fakeDishRepository;
    private readonly FakeGroupRepository _fakeGroupRepository;
    private readonly FakeProfileRepository _fakeProfileRepository;

    public DishServiceTests()
    {
        _fakeDishRepository = new FakeDishRepository();
        _fakeGroupRepository = new FakeGroupRepository();
        _fakeProfileRepository = new FakeProfileRepository();
        _dishService = new DishService(_fakeDishRepository, _fakeGroupRepository);
    }

    [Fact]
    public void CreateDish_ShouldCreateDishSuccessfully()
    {
        // Arrange
        var ownerId = "test-owner";
        var request = new CreateDishRequest
        {
            GroupId = 1,
            Title = "Test Dish",
            Desc = "Test description"
        };

        // Act
        var createdDish = _dishService.CreateDish(request, ownerId);

        // Assert
        Assert.NotNull(createdDish);
        Assert.Equal(ownerId, createdDish.OwnerId);
        Assert.Equal(request.GroupId, createdDish.GroupId);
        Assert.Equal(request.Title, createdDish.Title);
        Assert.Equal(request.Desc, createdDish.Desc);
    }

    [Fact]
    public void CreateDishPrep_ShouldCreateDishPrepAndUpdateDish()
    {
        // Arrange
        var ownerId = "test-owner";
        var dish = new Dish
        {
            OwnerId = ownerId,
            GroupId = 1,
            Title = "Test Dish",
            Desc = "Test description",
            Rating = 0f,
            NofRatings = 0,
            CreatedTimestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds()
        };
        var createdDish = _fakeDishRepository.CreateDish(dish);

        var userId = "test-user";
        var dishPrepRequest = new CreateDishPrepRequest
        {
            Rating = 5
        };

        // Act
        var createdDishPrep = _dishService.CreateDishPrep(dishPrepRequest, userId, createdDish.Id);

        // Assert
        Assert.NotNull(createdDishPrep);
        Assert.Equal(userId, createdDishPrep.UserProfileId);
        Assert.Equal(createdDish.Id, createdDishPrep.DishId);
        Assert.Equal(dishPrepRequest.Rating, createdDishPrep.Rating);

        var updatedDish = _fakeDishRepository.GetDish(createdDish.Id);
        Assert.Equal(1, updatedDish.NofRatings);
        Assert.Equal(dishPrepRequest.Rating, updatedDish.Rating);
        Assert.Equal(createdDishPrep.DateTimestamp, updatedDish.LastMadeTimestamp);
    }

    [Fact]
    public void CreateDishPrep_NonexistentDish_ThrowsNotFoundException()
    {
        // Arrange
        var userId = "test-user";
        var dishPrepRequest = new CreateDishPrepRequest
        {
            Rating = 5
        };
        var nonExistentDishId = 9999L;

        // Act & Assert
        Assert.Throws<NotFoundException>(() => _dishService.CreateDishPrep(dishPrepRequest, userId, nonExistentDishId));
    }

    [Fact]
    public void CreateMultipleDishPreps_ShouldUpdateDishRatingAndNofRatings()
    {
        // Arrange
        var ownerId = "test-owner";
        var dish = new Dish
        {
            GroupId = 1,
            Title = "Test Dish",
            Desc = "Test description",
            OwnerId = ownerId,
            CreatedTimestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds()
        };
        var createdDish = _fakeDishRepository.CreateDish(dish);

        var userId1 = "test-user1";
        var dishPrepRequest1 = new CreateDishPrepRequest
        {
            Rating = 5
        };

        var userId2 = "test-user2";
        var dishPrepRequest2 = new CreateDishPrepRequest
        {
            Rating = 3
        };

        // Act
        _dishService.CreateDishPrep(dishPrepRequest1, userId1, createdDish.Id);
        _dishService.CreateDishPrep(dishPrepRequest2, userId2, createdDish.Id);

        // Assert
        var updatedDish = _fakeDishRepository.GetDish(dish.Id);
        Assert.Equal(2, updatedDish.NofRatings);
        Assert.Equal(4, updatedDish.Rating);
    }

    [Fact]
    public void GetDish_ShouldReturnDish()
    {
        // Arrange
        var ownerId = "test-owner";
        var dish = new Dish
        {
            GroupId = 1,
            Title = "Test Dish",
            Desc = "Test description",
            OwnerId = ownerId,
            CreatedTimestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds()
        };
        var createdDish = _fakeDishRepository.CreateDish(dish);

        // Act
        var fetchedDish = _dishService.GetDish(createdDish.Id);

        // Assert
        Assert.NotNull(fetchedDish);
        Assert.Equal(createdDish.Id, fetchedDish.Id);
        Assert.Equal(createdDish.Title, fetchedDish.Title);
    }

    [Fact]
    public void GetDish_NonexistentDish_ThrowsNotFoundException()
    {
        // Arrange
        var nonExistentDishId = 9999L;

        // Act & Assert
        Assert.Throws<NotFoundException>(() => _dishService.GetDish(nonExistentDishId));
    }

    [Fact]
    public void UpdateDish_ShouldUpdateDishWhenUserIsOwnerOfGroup()
    {
        // Arrange
        var userId = "test-user";
        var userName = "Test User";
        var userProfile = _fakeProfileRepository.CreateProfile(userId, userName);

        var group = new Group
        {
            Name = "Test Group",
            OwnerId = userId,
            UserProfiles = new List<UserProfile>() { userProfile }
        };
        var createdGroup = _fakeGroupRepository.CreateGroup(group);

        var dish = new Dish { Title = "Original Title", Desc = "Original Desc", GroupId = createdGroup.Id };
        var createdDish = _fakeDishRepository.CreateDish(dish);

        var updateRequest = new UpdateDishRequest { Title = "Updated Title", Desc = "Updated Desc" };

        // Act
        var updatedDish = _dishService.UpdateDish(updateRequest, createdDish.Id, userId);

        // Assert
        Assert.Equal(createdDish.Id, updatedDish.Id);
        Assert.Equal(updateRequest.Title, updatedDish.Title);
        Assert.Equal(updateRequest.Desc, updatedDish.Desc);
    }

    [Fact]
    public void UpdateDish_ShouldUpdateDishWhenUserIsMemberOfGroup()
    {
        // Arrange
        var ownerId = "test-user";
        var ownerName = "Test User";
        var ownerProfile = _fakeProfileRepository.CreateProfile(ownerId, ownerName);

        var userId = "test-user";
        var userName = "Test User";
        var userProfile = _fakeProfileRepository.CreateProfile(userId, userName);

        var group = new Group
        {
            Name = "Test Group",
            OwnerId = ownerId,
            UserProfiles = new List<UserProfile>() { userProfile, ownerProfile }
        };
        var createdGroup = _fakeGroupRepository.CreateGroup(group);

        var dish = new Dish { Title = "Original Title", Desc = "Original Desc", GroupId = createdGroup.Id };
        var createdDish = _fakeDishRepository.CreateDish(dish);

        var updateRequest = new UpdateDishRequest { Title = "Updated Title", Desc = "Updated Desc" };

        // Act
        var updatedDish = _dishService.UpdateDish(updateRequest, createdDish.Id, userId);

        // Assert
        Assert.Equal(createdDish.Id, updatedDish.Id);
        Assert.Equal(updateRequest.Title, updatedDish.Title);
        Assert.Equal(updateRequest.Desc, updatedDish.Desc);
    }

    [Fact]
    public void UpdateDish_ShouldThrowBadRequestExceptionWhenUserNotMemberOfGroup()
    {
        // Arrange
        var ownerId = "test-owner";
        var ownerName = "Test Owner";
        var ownerProfile = _fakeProfileRepository.CreateProfile(ownerId, ownerName);

        var group = new Group
        {
            Name = "Test Group",
            OwnerId = ownerId,
            UserProfiles = new List<UserProfile>() { ownerProfile }
        };
        var createdGroup = _fakeGroupRepository.CreateGroup(group);

        var dish = new Dish { Title = "Original Title", Desc = "Original Desc", GroupId = createdGroup.Id };
        var createdDish = _fakeDishRepository.CreateDish(dish);

        var updateRequest = new UpdateDishRequest { Title = "Updated Title", Desc = "Updated Desc" };

        // Act & Assert
        Assert.Throws<BadRequestException>(() => _dishService.UpdateDish(updateRequest, createdDish.Id, "unrelated-user-id"));
    }

    [Fact]
    public void UpdateDish_ShouldThrowNotFoundExceptionWhenDishNotFound()
    {
        // Arrange
        var userId = "test-user";
        var userName = "Test User";
        var userProfile = _fakeProfileRepository.CreateProfile(userId, userName);

        var group = new Group
        {
            Name = "Test Group",
            OwnerId = userId,
            UserProfiles = new List<UserProfile>() { userProfile }
        };
        var createdGroup = _fakeGroupRepository.CreateGroup(group);

        var updateRequest = new UpdateDishRequest { Title = "Updated Title", Desc = "Updated Desc" };

        // Act & Assert
        Assert.Throws<NotFoundException>(() => _dishService.UpdateDish(updateRequest, -1, userId));
    }

}


