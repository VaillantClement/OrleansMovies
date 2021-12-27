using GraphQL.Types;
using Movies.Contracts;
using Movies.Server.Gql.Types;
using System;

namespace Movies.Server.Gql.App
{
	public class AppGraphQuery : ObjectGraphType
	{
		public AppGraphQuery(
			IMovieGrainClient movieClient,
			ISearchGrainClient searchClient,
			IGenreGrainClient genreClient
		)
		{
			Name = "AppQueries";

			Field<MovieGraphType>("movie",
				arguments: new QueryArguments(new QueryArgument<StringGraphType>
				{
					Name = "id"
				}),
				resolve: ctx => movieClient.Get(Convert.ToInt64(ctx.Arguments["id"]))
			);

			Field<ListGraphType<MovieGraphType>>("movies",
				resolve: ctx => searchClient.GetAll()
			);

			Field<ListGraphType<MovieGraphType>>("mostratedmovies",
				resolve: ctx => searchClient.GetMostRated()
			);

			Field<ListGraphType<MovieGraphType>>("searchmovie",
				arguments: new QueryArguments(new QueryArgument<StringGraphType>
				{
					Name = "query"
				}),
				resolve: ctx => searchClient.Get(ctx.Arguments["query"].ToString())
			);

			Field<ListGraphType<MovieGraphType>>("getallbygenre",
				arguments: new QueryArguments(new QueryArgument<StringGraphType>
				{
					Name = "genreid"
				}),
				resolve: ctx => genreClient.GetMoviesByGenre(Convert.ToInt64(ctx.Arguments["genreid"].ToString()))
			);
		}
	}
}
