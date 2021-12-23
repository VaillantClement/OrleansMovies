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
	public class SearchGrain : Grain<List<MovieModel>>, ISearchGrain
	{
		private readonly IGrainFactory _grainFactory;
		private readonly ReferenceDataService _searchDatabase;
		private List<MovieModel> _cachedResult;
		private string _cachedQuery;
		private Stopwatch _timeSinceLastUpdate = new Stopwatch();

		public SearchGrain(
			IGrainFactory grainFactory,
			ReferenceDataService referenceDataService)
		{
			_grainFactory = grainFactory;
			_searchDatabase = referenceDataService;
		}

		public async Task<List<MovieModel>> GetMostRated()
		{
			var query = "mostrated";

			// If the query has already been performed, return the result from cache.
			if (_cachedResult is object
				&& _timeSinceLastUpdate.Elapsed < TimeSpan.FromSeconds(10)
				&& query.Equals(_cachedQuery, StringComparison.InvariantCultureIgnoreCase))
			{
				return _cachedResult;
			}

			var movieIds = await _searchDatabase.QueryMostRatedMovieIdsAsync();

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

		public async Task<List<MovieModel>> GetAll()
		{
			var query = "all";

			// If the query has already been performed, return the result from cache.
			if (_cachedResult is object 
				&& _timeSinceLastUpdate.Elapsed < TimeSpan.FromSeconds(10)
				&& query.Equals(_cachedQuery, StringComparison.InvariantCultureIgnoreCase))
			{
				return _cachedResult;
			}

			var movieIds = await _searchDatabase.QueryAllIdsAsync();

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

		public async Task<List<MovieModel>> Get()
		{
			// This grain is keyed on the search query, so use that to search
			var query = this.GetPrimaryKeyString();

			// If the query has already been performed, return the result from cache.
			if (_cachedResult is object 
				&& _timeSinceLastUpdate.Elapsed < TimeSpan.FromSeconds(10)
				&& query.Equals(_cachedQuery, StringComparison.InvariantCultureIgnoreCase))
			{
				return _cachedResult;
			}

			// Search for possible matches from the full-text-search database
			var movieIds = await _searchDatabase.QueryByNameAsync(query);

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
