using Orleans;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.Contracts
{
	public interface IGenreGrainClient : IGrainWithIntegerKey
	{
		Task<List<MovieModel>> GetMoviesByGenre(long genreId);
	}
}
