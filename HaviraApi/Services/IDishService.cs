using System;
using HaviraApi.Entities;
using HaviraApi.Models.Request;

namespace HaviraApi.Services;

public interface IDishService
{

    public Dish GetDish(long dishId);

    public Dish CreateDish(CreateDishRequest request, string ownerId);

    public DishPrep CreateDishPrep(CreateDishPrepRequest request, string userId, long dishId);
}

