using System;
using Microsoft.EntityFrameworkCore;

namespace HaviraApi.Entities;


public class HaviraDbContext : DbContext
{
	public HaviraDbContext(DbContextOptions<HaviraDbContext> options) : base(options) { }

	public DbSet<Group> Groups { get; set; }

	public DbSet<Dish> Dishes { get; set; }

	public DbSet<UserProfile> UserProfiles { get; set; }

	//public DbSet<GroupUser> GroupUsers { get; set; }
}

