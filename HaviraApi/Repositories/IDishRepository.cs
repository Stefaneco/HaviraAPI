using System;
using HaviraApi.Entities;

namespace HaviraApi.Repositories;

public interface IDishRepository
{
    public Dish GetDishWithDishPrepsAndProfiles(long dishId);

    public Dish CreateDish(Dish dish);

    public List<DishPrep> GetDishPrepsByDishId(long dishId);

    public DishPrep CreateDishPrep(DishPrep dishPrep);

    public Dish UpdateDish(Dish dish);

    public void DeleteDish(Dish dish);

    public Dish GetDish(long dishId);
}

