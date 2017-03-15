namespace Business
{
	using System;
	using System.Collections.Generic;

	using Models;

	public class User
	{ 
		public static void Add(BusinessChallengeModel user)
		{
			if (user == null)
			{
				throw new NullReferenceException("The user is empty");
			}

			//DatabaseCommunication.AddUser(user);
		}

		public static BusinessChallengeModel Get(long id)
		{
			if (id <= 0)
			{
				throw new NullReferenceException("This user does not exist.");
			}

			return null;//DatabaseCommunication.GetUser(id).ToList<BusinessDiscordUserModel>().FirstOrDefault();
		}

		public static IList<BusinessChallengeModel> GetAll()
		{
			return null;//DatabaseCommunication.GetAllUsers().ToList<BusinessDiscordUserModel>();
		}

		public static IList<BusinessChallengeModel> GetSubscribedUsers(BusinessChallengeModel user)
		{
			return null;//DatabaseCommunication.GetSubscribedUsers(user.Id).ToList<BusinessDiscordUserModel>();
		}

		public static IList<BusinessChallengeModel> GetUsersWithBirthdate(DateTime birthDate)
		{
			return null;//DatabaseCommunication.GetUsersWithBirthdate(birthDate).ToList<BusinessDiscordUserModel>();
		}

		public static void Update(BusinessChallengeModel user)
		{
			if (user == null)
			{
				throw new NullReferenceException("The user is empty.");
			}

			//DatabaseCommunication.UpdateUser(user);
		}

		//public static void UpdateOrInsert(BusinessDiscordUserModel user)
		//{
		//	var discordUser = Get(user.Id);

		//	if (discordUser == null)
		//	{
		//		Console.WriteLine("Added a new user: " + user.Name);
		//		Add(user);
		//		return;
		//	}

		//	Console.WriteLine("Updated a user: " + user.Name);
		//	Update(discordUser);
		//}

		public static BusinessChallengeModel AuthorizeUser(string token, string uid)
		{
			//var user = DatabaseCommunication.AuthorizeUser(token, uid).ToList<BusinessDiscordUserModel>();
			//return user.FirstOrDefault();
			return null;
		}
	}
}