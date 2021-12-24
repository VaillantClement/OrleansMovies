using Movies.Contracts;
using Orleans;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.GrainClients
{
	public class GenreGrainClient : IGenreGrainClient
	{
		private readonly IGrainFactory _grainFactory;

		public GenreGrainClient(
			IGrainFactory grainFactory
		)
		{
			_grainFactory = grainFactory;
		}

		public Task<List<MovieModel>> GetMoviesByGenre(long genreId)
		{
			var grain = _grainFactory.GetGrain<IGenreGrain>(genreId);
			return grain.GetMoviesByGenre();
		}
	}
}
