using System;
using HaviraApi.Entities;
using HaviraApi.Exceptions;
using HaviraApi.Models.Request;
using HaviraApi.Repositories;

namespace HaviraApi.Services;

public class DishService : IDishService
{
	private readonly IDishRepository _dishRepository;
    private readonly IGroupRepository _groupRepository;

	public DishService(
		IDishRepository dishRepository,
        IGroupRepository groupRepository
		)
	{
		_dishRepository = dishRepository;
        _groupRepository = groupRepository;
	}

    public Dish CreateDish(CreateDishRequest request, string ownerId)
    {
        var dish = new Dish
        {
            OwnerId = ownerId,
            GroupId = request.GroupId,
            Title = request.Title,
            Desc = request.Desc,
            Rating = 0f,
            NofRatings = 0,
            CreatedTimestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds()
        };
        var createdDish = _dishRepository.CreateDish(dish);
        return createdDish;
    }

    public DishPrep CreateDishPrep(CreateDishPrepRequest request, string userId, long dishId)
    {
        var newDishPrep = new DishPrep {
            Rating = request.Rating,
            UserProfileId = userId,
            DishId = dishId,
            DateTimestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds()
        };
        var createdDishPrep = _dishRepository.CreateDishPrep(newDishPrep);

        var dish = _dishRepository.GetDishWithDishPrepsAndProfiles(createdDishPrep.DishId);
        
        dish.LastMadeTimestamp = createdDishPrep.DateTimestamp;
        dish.Rating =
            ((dish.Rating * dish.NofRatings)
            + createdDishPrep.Rating) / (dish.NofRatings + 1);
        dish.NofRatings += 1;

        _dishRepository.UpdateDish(dish);

        return createdDishPrep;
    }

    public void DeleteDish(long dishId, string userId)
    {
        var dish = _dishRepository.GetDishWithDishPrepsAndProfiles(dishId);
        var group = _groupRepository.GetGroupById(dish.GroupId);
        var isUserInGroup = group.UserProfiles.Exists(u => u.Id == userId);
        if (!isUserInGroup) throw new BadRequestException("User is not a member of this group");
        _dishRepository.DeleteDish(dish);
    }

    public Dish GetDish(long dishId)
    {
        var dish = _dishRepository.GetDishWithDishPrepsAndProfiles(dishId);
        return dish;
    }

    public Dish UpdateDish(UpdateDishRequest request, long dishId, string userId)
    {
        var dish = _dishRepository.GetDish(dishId);
        var group = _groupRepository.GetGroupById(dish.GroupId);
        var isUserInGroup = group.UserProfiles.Exists(u => u.Id == userId);
        if (!isUserInGroup) throw new BadRequestException("User is not a member of this group");

        dish.Desc = request.Desc;
        dish.Title = request.Title;

        var updatedDish = _dishRepository.UpdateDish(dish);
        return updatedDish;
    }
}

