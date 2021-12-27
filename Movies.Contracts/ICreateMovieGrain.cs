using Orleans;
using System.Threading.Tasks;

namespace Movies.Contracts
{
	public interface ICreateMovieGrain : IGrainWithGuidKey
	{
		Task<long> Create(string name, string description, string img, string key, string length, decimal rate);
	}
}
