using System;
namespace HaviraApi.Entities;

public class Dish
{
	public long Id { get; set; }
	public long OwnerId { get; set; }
	public long GroupId { get; set; }
	public string Title { get; set; }
	public string Desc { get; set; }
	public float Rating { get; set; }
	public int NofRatings { get; set; }
	public long LastMadeTimestamp { get; set; }
	public long CreatedTimestamp { get; set; }
}

