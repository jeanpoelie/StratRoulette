namespace Business
{
	using System.Collections.Generic;

	using Extensions;

	using ModelLibrary.Models;

	public class Operators
	{
		public static IList<OperatorModel> GetAll()
		{
			return DatabaseCommunication.GetAllOperators().ToList<OperatorModel>();
		}

		public static IList<OperatorModel> GetSide(string side, int number = 0, bool includeRecruit = true)
		{
			return DatabaseCommunication.GetOperatorsOnSide(side, number, includeRecruit).ToList<OperatorModel>();
		}

		public static IList<OperatorLoadoutModel> GetLoadout(string operatorName)
		{
			return DatabaseCommunication.GetOperatorLoadout(operatorName).ToList<OperatorLoadoutModel>();
		}

		#region [Stored Procedures]	

		public static IList<OperatorModel> GetStoredProcedure(string side, string excludedOperators = "", int numberOfOperators = 1)
		{
			return DatabaseCommunication.GetOperatorStoredProcedure(side, excludedOperators, numberOfOperators).ToList<OperatorModel>();
		}

		#endregion
	}
}