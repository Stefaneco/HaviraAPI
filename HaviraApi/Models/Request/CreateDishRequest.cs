using System;
using HaviraApi.Entities;

namespace HaviraApi.Models.Request;

public class CreateDishRequest
{
    public long GroupId { get; set; }
    public string Title { get; set; }
    public string Desc { get; set; }
}

