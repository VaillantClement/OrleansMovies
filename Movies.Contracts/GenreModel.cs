using System.Collections.Generic;

namespace Movies.Contracts
{
	public class GenreModel
	{
		public long Id { get; set; }
		public string Key { get; set; }
		public string Name { get; set; }
		public List<MovieModel> Movies { get; set; }
	}
}
