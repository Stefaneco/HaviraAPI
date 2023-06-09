﻿using System;
using HaviraApi.Entities;
using HaviraApi.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace HaviraApi.Repositories;

public class DishRepository : IDishRepository
{
    private readonly HaviraDbContext _dbContext;

    public DishRepository(HaviraDbContext dbContext)
	{
        _dbContext = dbContext;
    }

    public Dish CreateDish(Dish dish)
    {
        var createdDish = _dbContext.Dishes.Add(dish);
        _dbContext.SaveChanges();
        return createdDish.Entity;
    }

    public DishPrep CreateDishPrep(DishPrep dishPrep)
    {
        var createdDishPrep = _dbContext.DishPreps.Add(dishPrep);
        _dbContext.SaveChanges();
        return createdDishPrep.Entity;
    }

    public void DeleteDish(Dish dish)
    {
        _dbContext.Dishes.Remove(dish);
        _dbContext.SaveChanges();
    }

    public Dish GetDishWithDishPrepsAndProfiles(long dishId)
    {
        var dish = _dbContext.Dishes
            .Include(d => d.DishPreps)
            .ThenInclude(dp => dp.UserProfile)
            .First(d => d.Id == dishId) ?? throw new NotFoundException("Dish not found");
        return dish;
    }

    public List<DishPrep> GetDishPrepsByDishId(long dishId)
    {
        var dishPreps = _dbContext.DishPreps
            .Include(dp => dp.UserProfile)
            .Where(dp => dp.DishId == dishId);
        return dishPreps.ToList();
    }

    public Dish UpdateDish(Dish dish)
    {
        var updatedDish = _dbContext.Dishes.Update(dish);
        _dbContext.SaveChanges();
        if (updatedDish is null)
            throw new NotFoundException("Dish not found");
        return updatedDish.Entity;
    }

    public Dish GetDish(long dishId)
    {
        var dish = _dbContext.Dishes
           .First(d => d.Id == dishId) ?? throw new NotFoundException("Dish not found");
        return dish;
    }
}

