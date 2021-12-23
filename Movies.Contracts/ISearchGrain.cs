using Orleans;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.Contracts
{
	public interface ISearchGrain : IGrainWithStringKey
	{
		Task<List<MovieModel>> GetMostRated();
		Task<List<MovieModel>> GetAll();
		Task<List<MovieModel>> Get();
	}
}
