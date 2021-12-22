using Orleans;
using System.Threading.Tasks;

namespace Movies.Contracts
{
	public interface IMovieGrain : IGrainWithIntegerKey
	{
		Task<MovieModel> Get();
		Task Set(string name, string description, string img, string key, string length, decimal rate);
	}
}
