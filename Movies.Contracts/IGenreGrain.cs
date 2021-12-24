using Orleans;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.Contracts
{
	public interface IGenreGrain : IGrainWithIntegerKey
	{
		Task<List<MovieModel>> GetMoviesByGenre();
	}
}
