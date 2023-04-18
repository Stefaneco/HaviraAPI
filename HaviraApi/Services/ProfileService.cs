using System;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using HaviraApi.Entities;
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

    public UserProfile CreateProfile(string userId, IFormFile image, string userName)
    {
        if(image != null) {
            var containerClient = _blobServiceClient.GetBlobContainerClient("profilepictures");
            var blobClient = containerClient.GetBlobClient(userId);

            var stream = image.OpenReadStream();
            
            blobClient.Upload(stream, overwrite: true);

            blobClient.SetHttpHeaders(new BlobHttpHeaders { ContentType = image.ContentType });
        }

        var createdProfile = _profileRepository.CreateProfile(userId, userName);
        return createdProfile;
    }

    public UserProfile GetUserProfileById(string userId)
    {
        var profile = _profileRepository.GetUserProfileById(userId);
        return profile;
    }
}

