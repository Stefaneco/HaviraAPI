using System;
namespace HaviraApi.Entities;

public class Group
{
	public long Id { get; set; }
	public string Name { get; set; }
	public string OwnerId { get; set; }
	public long CreatedTimestamp { get; set; }
	public string JoinCode { get; set; }

	public virtual List<UserProfile> UserProfiles { get; set; }
	public virtual List<Dish> Dishes { get; set; }
}

