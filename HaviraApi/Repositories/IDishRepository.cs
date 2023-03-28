using System;
using HaviraApi.Entities;

namespace HaviraApi.Repositories;

public interface IDishRepository
{
    public Dish GetDish(long dishId);

    public Dish CreateDish(Dish dish);

    public List<DishPrep> GetDishPrepsByDishId(long dishId);

    public DishPrep CreateDishPrep(DishPrep dishPrep);
}

