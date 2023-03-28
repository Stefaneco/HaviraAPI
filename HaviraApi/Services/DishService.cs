using System;
using HaviraApi.Entities;
using HaviraApi.Models.Request;
using HaviraApi.Repositories;

namespace HaviraApi.Services;

public class DishService : IDishService
{
	private readonly IDishRepository _dishRepository;

	public DishService(
		IDishRepository dishRepository
		)
	{
		_dishRepository = dishRepository;
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
        return createdDishPrep;
    }

    public Dish GetDish(long dishId)
    {
        var dish = _dishRepository.GetDish(dishId);
        return dish;
    }
}

