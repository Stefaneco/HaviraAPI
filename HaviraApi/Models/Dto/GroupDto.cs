using System;
namespace HaviraApi.Models.Dto;

public class GroupDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string OwnerId { get; set; }
    public long CreatedTimestamp { get; set; }
    public string JoinCode { get; set; }
    public List<UserDto> UserDtos { get; set; }
    public List<DishListItemDto> DishListItemDtos { get; set; }
}

