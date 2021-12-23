using Microsoft.Extensions.DependencyInjection;
using Movies.Contracts;
using Movies.DAL;

namespace Movies.GrainClients
{
	public static class GrainClientsServiceCollectionExtensions
	{
		public static void AddAppClients(this IServiceCollection services)
		{
			services.AddSingleton<ReferenceDataService>();

			services.AddSingleton<IMovieGrainClient, MovieGrainClient>();
			services.AddSingleton<ISearchGrainClient, SearchGrainClient>();
		}
	}
}