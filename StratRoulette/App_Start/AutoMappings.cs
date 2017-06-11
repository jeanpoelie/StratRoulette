// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AutoMappings.cs" company="">
//   
// </copyright>
// <summary>
//   The auto mappings.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StratRoulette
{
	using AutoMapper;

	using ModelLibrary.Models;

	using Models;

	/// <summary>
	/// The auto mappings.
	/// </summary>
	public class AutoMappings
	{
		/// <summary>
		/// The register mappings.
		/// </summary>
		public static void RegisterMappings()
		{
			Mapper.Initialize(
				cfg =>
					{

					});
		}
	}
}