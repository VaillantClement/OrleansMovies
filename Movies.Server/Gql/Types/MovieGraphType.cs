using GraphQL.Types;
using Movies.Contracts;

namespace Movies.Server.Gql.Types
{
	public class MovieGraphType : ObjectGraphType<MovieModel>
	{
		public MovieGraphType()
		{
			Name = "Movie";
			Description = "A movie graphtype.";

			Field(x => x.Id).Description("Unique key.");
			Field(x => x.Name, nullable: true).Description("Name.");
			Field(x => x.Key, nullable: true).Description("Key.");
			Field(x => x.Description, nullable: true).Description("Description.");
			Field(x => x.Img, nullable: true).Description("Img.");
			Field(x => x.Length, nullable: true).Description("Length.");
			Field(x => x.Rate, nullable: true).Description("Rate.");
		}
	}
}