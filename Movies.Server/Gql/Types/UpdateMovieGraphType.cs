using GraphQL.Types;
using Movies.Contracts;

namespace Movies.Server.Gql.Types
{
	public class UpdateMovieGraphType : InputObjectGraphType
	{
		public UpdateMovieGraphType()
		{
			Name = "UpdateMovie";

			Field<NonNullGraphType<StringGraphType>>("id");
			Field<StringGraphType>("key");
			Field<StringGraphType>("name");
			Field<StringGraphType>("description");
			Field<StringGraphType>("img");
			Field<StringGraphType>("length");
			Field<FloatGraphType>("rate");
		}
	}
}