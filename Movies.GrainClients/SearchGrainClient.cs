using Movies.Contracts;
using Orleans;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.GrainClients
{
	public class SearchGrainClient : ISearchGrainClient
	{
		private readonly IGrainFactory _grainFactory;

		public SearchGrainClient(
			IGrainFactory grainFactory
		)
		{
			_grainFactory = grainFactory;
		}

		public Task<List<MovieModel>> GetAll()
		{
			var grain = _grainFactory.GetGrain<ISearchGrain>("all");
			return grain.GetAll();
		}

		public Task<List<MovieModel>> GetMostRated()
		{
			var grain = _grainFactory.GetGrain<ISearchGrain>("mostrated");
			return grain.GetMostRated();
		}

		public Task<List<MovieModel>> Get(string query)
		{
			var grain = _grainFactory.GetGrain<ISearchGrain>(query);
			return grain.Get();
		}
	}
}
