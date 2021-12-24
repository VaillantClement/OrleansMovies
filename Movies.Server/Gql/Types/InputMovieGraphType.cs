using GraphQL.Types;
using Movies.Contracts;

namespace Movies.Server.Gql.Types
{
	public class InputMovieGraphType : InputObjectGraphType
	{
		public InputMovieGraphType()
		{
			Name = "InputMovie";

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