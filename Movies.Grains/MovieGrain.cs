using Movies.Contracts;
using Movies.DAL;
using Orleans;
using Orleans.Providers;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Grains
{
	[StorageProvider(ProviderName = "Default")]
	public class MovieGrain : Grain<MovieModel>, IMovieGrain
	{
		private readonly ReferenceDataService _referenceDataService;

		public MovieGrain(
			ReferenceDataService referenceDataService)
		{
			_referenceDataService = referenceDataService;
		}

		public Task<MovieModel> Get()
			=> Task.FromResult(State);

		public Task Set(string name, string description, string img, string key, string length, decimal rate)
		{
			State = new MovieModel { 
				Id = this.GetPrimaryKeyLong(), 
				Name = name,
				Description = description,
				Img = img,
				Key = key,
				Length = length,
				Rate = rate
			};

			return Task.CompletedTask;
		}

		public override async Task OnActivateAsync()
		{
			// If there is no state saved for this entry yet, load the state from the reference dictionary and store it.
			if (State?.Key is null)
			{
				// Find the definiton from the reference data, using this grain's id to look it up
				var movieId = this.GetPrimaryKeyLong();
				var result = await _referenceDataService.QueryByIdAsync(movieId.ToString());

				if (result is { Count: > 0 } && result.FirstOrDefault() is MovieModel movie)
				{
					State = movie;

					// Write the state but don't wait for completion. If it fails, we will write it next time. 
					//State.WriteStateAsync().Ignore();
				}
			}
		}

	}
}
