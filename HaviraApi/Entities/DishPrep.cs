using System;
namespace HaviraApi.Entities;

public class DishPrep
{
	public long Id { get; set; }
	public string UserProfileId { get; set; }
	public long DishId { get; set; }
	public int Rating { get; set; }
	public long DateTimestamp { get; set; }

	public virtual UserProfile UserProfile { get; set; }
	public virtual Dish Dish { get; set; }
}

