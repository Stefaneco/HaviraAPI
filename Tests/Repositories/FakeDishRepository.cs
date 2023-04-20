using System;
using HaviraApi.Entities;
using HaviraApi.Exceptions;
using HaviraApi.Repositories;

namespace HaviraApi.Test.Repositories;

public class FakeDishRepository : IDishRepository
{
    private readonly Dictionary<long, Dish> _dishes = new();
    private readonly Dictionary<long, List<DishPrep>> _dishPreps = new();
    private long _nextDishId = 1;
    private long _nextDishPrepId = 1;

    public Dish CreateDish(Dish dish)
    {
        dish.Id = _nextDishId++;
        _dishes[dish.Id] = dish;
        _dishPreps[dish.Id] = new List<DishPrep>();
        return dish;
    }

    public DishPrep CreateDishPrep(DishPrep dishPrep)
    {
        if (!_dishes.ContainsKey(dishPrep.DishId))
        {
            throw new NotFoundException($"Dish with ID {dishPrep.DishId} not found");
        }
        dishPrep.Id = _nextDishPrepId++;
        _dishPreps[dishPrep.DishId].Add(dishPrep);
        return dishPrep;
    }

    public void DeleteDish(Dish dish)
    {
        if (_dishes.ContainsKey(dish.Id))
        {
            _dishes.Remove(dish.Id);
        }
        else
        {
            throw new NotFoundException("Dish not found");
        }
    }

    public Dish GetDishWithDishPrepsAndProfiles(long dishId)
    {
        if (_dishes.TryGetValue(dishId, out var dish))
        {
            dish.DishPreps = _dishPreps[dishId];
            return dish;
        }
        throw new NotFoundException("Dish not found");
    }

    public List<DishPrep> GetDishPrepsByDishId(long dishId)
    {
        if (_dishPreps.TryGetValue(dishId, out var dishPreps))
        {
            return dishPreps;
        }
        return new List<DishPrep>();
    }

    public Dish UpdateDish(Dish dish)
    {
        if (_dishes.ContainsKey(dish.Id))
        {
            _dishes[dish.Id] = dish;
            return dish;
        }
        throw new NotFoundException("Dish not found");
    }
}


