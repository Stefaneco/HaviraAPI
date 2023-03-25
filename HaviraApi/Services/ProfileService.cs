using System;
using Azure.Storage.Blobs;
using HaviraApi.Repositories;

namespace HaviraApi.Services;

public class ProfileService : IProfileService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly IProfileRepository _profileRepository;

	public ProfileService(BlobServiceClient blobServiceClient, IProfileRepository profileRepository)
	{
        _blobServiceClient = blobServiceClient;
        _profileRepository = profileRepository;
	}

    public void CreateProfile(string userId, IFormFile image, string userName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient("profilepictures");
        var blobClient = containerClient.GetBlobClient(userId);

        var fileName = image.FileName;
        var contentType = image.ContentType;
        var stream = image.OpenReadStream();

        blobClient.UploadAsync(stream, overwrite: true);

        _profileRepository.CreateProfile(userId, userName);
    }
}

