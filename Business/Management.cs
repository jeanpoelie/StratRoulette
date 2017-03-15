namespace Business
{
	using System.Linq;

	using Business.Extensions;

	using Models;

	public class Management
	{
		
		public static BusinessManagementAccountModel Login(string username, string password)
		{
			return DatabaseCommunication.ManagementLogin(username, password).ToList<BusinessManagementAccountModel>().FirstOrDefault();
		}
	}
}