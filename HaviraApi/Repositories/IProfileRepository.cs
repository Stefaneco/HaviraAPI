using System;
using HaviraApi.Entities;

namespace HaviraApi.Repositories
{
	public interface IProfileRepository
	{
		public UserProfile CreateProfile(string userId, string userName);

		public UserProfile GetUserProfileById(string userId);
	}
}

