using Microsoft.Extensions.DependencyInjection;
using Movies.Contracts;
using Movies.DAL;

namespace Movies.GrainClients
{
	public static class GrainClientsServiceCollectionExtensions
	{
		public static void AddAppClients(this IServiceCollection services)
		{
			services.AddSingleton<ISampleGrainClient, SampleGrainClient>();
			services.AddSingleton<IMovieGrainClient, MovieGrainClient>();
		}
	}
}