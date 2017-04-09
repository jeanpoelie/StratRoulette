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
						cfg.CreateMap<ChallengeModel, ChallengeModel>();
						cfg.CreateMap<ChallengeModel, ChallengeModel>();
						cfg.CreateMap<VideoModel, VideoModel>();
						cfg.CreateMap<VideoModel, VideoModel>();
						cfg.CreateMap<OperatorModel, OperatorModel>().ForMember(m => m.Loadout, p => p.Ignore());
						cfg.CreateMap<OperatorModel, OperatorModel>();
						cfg.CreateMap<OperatorLoadoutModel, OperatorLoadoutModel>();
						cfg.CreateMap<OperatorLoadoutModel, OperatorLoadoutModel>();
						cfg.CreateMap<StatisticModel, StatisticModel>();
						cfg.CreateMap<StatisticModel, StatisticModel>();
						cfg.CreateMap<MapModel, MapModel>();
						cfg.CreateMap<MapModel, MapModel>();
						cfg.CreateMap<GameModeModel, GameModeModel>();
						cfg.CreateMap<GameModeModel, GameModeModel>();
					});
		}
	}
}