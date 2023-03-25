using System;
namespace HaviraApi.Repositories
{
	public interface IProfileRepository
	{
		public void CreateProfile(string userId, string userName);
	}
}

