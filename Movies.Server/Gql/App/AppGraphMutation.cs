using GraphQL;
using GraphQL.Types;
using Movies.Contracts;
using Movies.Server.Gql.Types;

namespace Movies.Server.Gql.App
{
	public class AppGraphMutation : ObjectGraphType
	{
		public AppGraphMutation(
			IMovieGrainClient movieClient
		)
		{
			Name = "AppGraphMutation";

			Field<MovieGraphType>("updateMovie",
			  arguments: new QueryArguments(
				new QueryArgument<NonNullGraphType<InputMovieGraphType>> { Name = "movie" }
			  ),
			  resolve: context =>
			  {
				  var movie = context.GetArgument<MovieModel>("movie");
				  movieClient.Set(movie.Id, movie.Name, movie.Description, movie.Img, movie.Key, movie.Length, movie.Rate);

				  return movieClient.Get(movie.Id);
			  });
		}
	}
}