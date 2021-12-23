using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.Contracts
{
	public interface IMovieGrainClient
	{
		Task<MovieModel> Get(long id);
		Task Set(long id, string name, string description, string img, string key, string length, decimal rate);
	}
}
