using System;
namespace HaviraApi.Entities;

public class UserProfile
{
	public string Id { get; set; }
	public string Name { get; set; }
	public List<Group> Groups { get; set; }
}

