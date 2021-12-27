using Movies.Contracts;
using Movies.DAL;
using Orleans;
using Orleans.Providers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Movies.Grains
{
	[StorageProvider(ProviderName = "Default")]
	public class GenreGrain : Grain<GenreModel>, IGenreGrain
	{
		private readonly IGrainFactory _grainFactory;
		private readonly ReferenceDataService _referenceDataService;
		private List<MovieModel> _cachedResult;
		private long _cachedQuery;
		private readonly Stopwatch _timeSinceLastUpdate = new Stopwatch();


		public GenreGrain(
			IGrainFactory grainFactory,
			ReferenceDataService referenceDataService
		)
		{
			_grainFactory = grainFactory;
			_referenceDataService = referenceDataService;
		}

		public async Task<List<MovieModel>> GetMoviesByGenre()
		{
			// This grain is keyed on the search query, so use that to search
			var query = this.GetPrimaryKeyLong();

			// If the query has already been performed, return the result from cache.
			if (_cachedResult is not null
				&& _timeSinceLastUpdate.Elapsed < TimeSpan.FromSeconds(10)
				&& query == _cachedQuery)
			{
				return _cachedResult;
			}

			// Search for possible matches from the full-text-search database
			var movieIds = await _referenceDataService.QueryAllIdsByGenreAsync(query);

			// Fan out and get all of the movies for each matching ids
			var tasks = new List<Task<MovieModel>>();
			foreach (var id in movieIds)
			{
				var entryGrain = _grainFactory.GetGrain<IMovieGrain>(id);
				tasks.Add(entryGrain.Get());
			}

			// Wait for all calls to complete
			await Task.WhenAll(tasks);

			// Collect the results into a list to return
			var results = new List<MovieModel>(tasks.Count);
			foreach (var task in tasks)
			{
				results.Add(await task);
			}

			// Cache the result for next time
			_cachedResult = results;
			_cachedQuery = query;
			_timeSinceLastUpdate.Restart();

			return results;
		}
	}
}
