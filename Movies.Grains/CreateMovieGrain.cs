using Movies.Contracts;
using Movies.DAL;
using Orleans;
using Orleans.Providers;
using System.Threading.Tasks;

namespace Movies.Grains
{
	[StorageProvider(ProviderName = "Default")]
	public class CreateMovieGrain : Grain, ICreateMovieGrain
	{
		private readonly ReferenceDataService _referenceDataService;

		public CreateMovieGrain(
			ReferenceDataService referenceDataService
		)
		{
			_referenceDataService = referenceDataService;
		}

		public async Task<long> Create(string name, string description, string img, string key, string length, decimal rate)
		{
			var movie = new MovieModel()
			{
				Name = name,
				Description = description,
				Img = img,
				Key = key,
				Length = length,
				Rate = rate
			};

			return await _referenceDataService.CreateMovieAsync(movie);
		}
	}
}
