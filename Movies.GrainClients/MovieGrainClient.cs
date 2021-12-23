using Movies.Contracts;
using Orleans;
using System.Threading.Tasks;

namespace Movies.GrainClients
{
	public class MovieGrainClient : IMovieGrainClient
	{
		private readonly IGrainFactory _grainFactory;

		public MovieGrainClient(
			IGrainFactory grainFactory
		)
		{
			_grainFactory = grainFactory;
		}

		public Task<MovieModel> Get(long id)
		{
			var grain = _grainFactory.GetGrain<IMovieGrain>(id);
			return grain.Get();
		}

		public Task Set(long id, string name, string description, string img, string key, string length, decimal rate)
		{
			var grain = _grainFactory.GetGrain<IMovieGrain>(id);
			return grain.Set(name, description, img, key, length, rate);
		}
	}
}