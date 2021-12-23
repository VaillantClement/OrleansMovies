using Orleans;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.Contracts
{
	public interface ISearchGrainClient : IGrainWithStringKey
	{
		Task<List<MovieModel>> GetAll();
		Task<List<MovieModel>> GetMostRated();
		Task<List<MovieModel>> Get(string query);
	}
}
