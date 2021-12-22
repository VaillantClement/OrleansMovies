using System.Collections.Generic;

namespace Movies.Contracts
{
	public class MovieModel
	{
		public long Id { get; set; }
		public string Key { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public List<string> Genres { get; set; }
		public decimal Rate { get; set; }
		public string Length { get; set; }
		public string Img { get; set; }
	}
}