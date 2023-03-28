using System;
namespace HaviraApi.Models.Dto;

public class DishListItemDto
{
    public long Id { get; set; }
    public string OwnerId { get; set; }
    public long GroupId { get; set; }
    public string Title { get; set; }
    public string Desc { get; set; }
    public float Rating { get; set; }
    public int NofRatings { get; set; }
    public long? LastMadeTimestamp { get; set; }
    public long CreatedTimestamp { get; set; }
}

