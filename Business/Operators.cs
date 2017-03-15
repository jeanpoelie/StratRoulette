namespace Business
{
	using System.Collections.Generic;

	using Extensions;

	using Models;

	public class Operators
	{
		public static IList<BusinessOperatorModel> GetAll()
		{
			return DatabaseCommunication.GetAllOperators().ToList<BusinessOperatorModel>();
		}

		public static IList<BusinessOperatorModel> GetSide(string side, int number = 0, bool includeRecruit = true)
		{
			return DatabaseCommunication.GetOperatorsOnSide(side, number, includeRecruit).ToList<BusinessOperatorModel>();
		}

		public static IList<BusinessOperatorLoadoutModel> GetLoadout(string operatorName)
		{
			return DatabaseCommunication.GetOperatorLoadout(operatorName).ToList<BusinessOperatorLoadoutModel>();
		} 
	}
}