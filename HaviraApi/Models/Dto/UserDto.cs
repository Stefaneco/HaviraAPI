using System;
using Azure.Storage.Blobs.Models;

namespace HaviraApi.Models.Dto;

public class UserDto
{
	public string Id { get; set; }
    public string Name { get; set; }
    //public BlobInfo Picture { get; set; }
}

