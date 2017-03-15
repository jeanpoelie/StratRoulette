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

	using Business.Models;

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
						cfg.CreateMap<BusinessChallengeModel, ChallengeModel>();
						cfg.CreateMap<ChallengeModel, BusinessChallengeModel>();
						cfg.CreateMap<BusinessVideoModel, VideoModel>();
						cfg.CreateMap<VideoModel, BusinessVideoModel>();
						cfg.CreateMap<BusinessOperatorModel, OperatorModel>().ForMember(m => m.Loadout, p => p.Ignore());
						cfg.CreateMap<OperatorModel, BusinessOperatorModel>();
						cfg.CreateMap<BusinessOperatorLoadoutModel, OperatorLoadoutModel>();
						cfg.CreateMap<OperatorLoadoutModel, BusinessOperatorLoadoutModel>();
						cfg.CreateMap<BusinessStatisticModel, StatisticModel>();
						cfg.CreateMap<StatisticModel, BusinessStatisticModel>();
						cfg.CreateMap<BusinessMapModel, MapModel>();
						cfg.CreateMap<MapModel, BusinessMapModel>();
						cfg.CreateMap<BusinessGameModeModel, GameModeModel>();
						cfg.CreateMap<GameModeModel, BusinessGameModeModel>();
					});
		}
	}
}