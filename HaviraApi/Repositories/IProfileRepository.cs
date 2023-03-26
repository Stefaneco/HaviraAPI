using System;
using HaviraApi.Entities;

namespace HaviraApi.Repositories
{
	public interface IProfileRepository
	{
		public void CreateProfile(string userId, string userName);

		public UserProfile GetUserProfileById(string userId);
	}
}

