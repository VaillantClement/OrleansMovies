using Microsoft.Data.Sqlite;
using Movies.Contracts;
using System.Data.Common;

namespace Movies.DAL
{
	public class ReferenceDataService
	{
		private readonly string _connectionString;
		private readonly TaskScheduler _scheduler;

		public ReferenceDataService()
		{
			var dbPath = Path.Combine(Environment.CurrentDirectory, "Data/Movies.db");
			var builder = new SqliteConnectionStringBuilder($"Data Source={dbPath};")
			{
				Mode = SqliteOpenMode.ReadWriteCreate,
				Cache = SqliteCacheMode.Shared,
			};

			_connectionString = builder.ToString();
			_scheduler = new ConcurrentExclusiveSchedulerPair(TaskScheduler.Default, maxConcurrencyLevel: 1).ExclusiveScheduler;
		}

		public Task<long> CreateMovieAsync(MovieModel movie) =>
			Task.Factory.StartNew(() =>
			{
				using var connection = new SqliteConnection(_connectionString);
				connection.Open();
				var insertCmd = new SqliteCommand($"insert into movies (name, description, img, key, length, rate) Values ($name, $description, $img, $key, $length, $rate)", connection);
				insertCmd.Parameters.AddWithValue("$name", movie.Name);
				insertCmd.Parameters.AddWithValue("$description", movie.Description);
				insertCmd.Parameters.AddWithValue("$img", movie.Img);
				insertCmd.Parameters.AddWithValue("$key", movie.Key);
				insertCmd.Parameters.AddWithValue("$length", movie.Length);
				insertCmd.Parameters.AddWithValue("$rate", movie.Rate);
				insertCmd.Prepare();
				insertCmd.ExecuteNonQuery();

				var cmd = new SqliteCommand($"select Id from movies where Key=$key limit 1", connection);
				cmd.Parameters.AddWithValue("$key", movie.Key);
				cmd.Prepare();

				var reader = cmd.ExecuteReader();
				return ReadAllAsMovieId(reader).First();
			},
			CancellationToken.None,
			TaskCreationOptions.RunContinuationsAsynchronously,
			_scheduler);

		public Task<List<long>> QueryMostRatedMovieIdsAsync() =>
			Task.Factory.StartNew(() =>
			{
				using var connection = new SqliteConnection(_connectionString);
				connection.Open();
				var cmd = new SqliteCommand("select Id from movies order by rate desc limit 5", connection);
				cmd.Prepare();

				var reader = cmd.ExecuteReader();
				return ReadAllAsMovieId(reader);
			},
			CancellationToken.None,
			TaskCreationOptions.RunContinuationsAsynchronously,
			_scheduler);

		public Task<List<long>> QueryAllIdsByGenreAsync(long genreId) =>
			Task.Factory.StartNew(() =>
			{
				using var connection = new SqliteConnection(_connectionString);
				connection.Open();
				var cmd = new SqliteCommand("SELECT distinct Movies.Id from Movies INNER JOIN MoviesGenres ON Movies.Id = MoviesGenres.MovieId WHERE MoviesGenres.GenreId = $genreId", connection);
				cmd.Parameters.AddWithValue("$genreId", genreId);
				cmd.Prepare();

				var reader = cmd.ExecuteReader();
				return ReadAllAsMovieId(reader);
			},
			CancellationToken.None,
			TaskCreationOptions.RunContinuationsAsynchronously,
			_scheduler);

		public Task<List<long>> QueryAllIdsAsync() =>
			Task.Factory.StartNew(() =>
			{
				using var connection = new SqliteConnection(_connectionString);
				connection.Open();
				var cmd = new SqliteCommand("select Id from movies", connection);
				cmd.Prepare();

				var reader = cmd.ExecuteReader();
				return ReadAllAsMovieId(reader);
			},
			CancellationToken.None,
			TaskCreationOptions.RunContinuationsAsynchronously,
			_scheduler);

		public Task<List<long>> QueryByNameAsync(string query) =>
			Task.Factory.StartNew(() =>
			{
				using var connection = new SqliteConnection(_connectionString);
				connection.Open();
				var cmd = new SqliteCommand($"select Id from movies where Name like '%{query}%'", connection);
				cmd.Prepare();

				var reader = cmd.ExecuteReader();
				return ReadAllAsMovieId(reader);
			},
			CancellationToken.None,
			TaskCreationOptions.RunContinuationsAsynchronously,
			_scheduler);

		public Task<List<MovieModel>> QueryByIdAsync(string query) => 
			Task.Factory.StartNew(() =>
			{
				using var connection = new SqliteConnection(_connectionString);
				connection.Open();
				var cmd = new SqliteCommand("select * from movies where Id=$id", connection);
				cmd.Parameters.AddWithValue("$id", query);
				cmd.Prepare();

				var reader = cmd.ExecuteReader();
				return ReadAllAsMovie(reader);
			},
			CancellationToken.None,
			TaskCreationOptions.RunContinuationsAsynchronously,
			_scheduler);

		private static List<long> ReadAllAsMovieId(DbDataReader reader)
		{
			int rowIdColId = reader.GetOrdinal("Id");

			var results = new List<long>();
			while (reader.Read())
			{
				var result = reader.GetInt32(rowIdColId);
				results.Add(result);
			}

			return results;
		}

		private static List<MovieModel> ReadAllAsMovie(DbDataReader reader)
		{
			int rowIdColId = reader.GetOrdinal("Id");
			int keyColId = reader.GetOrdinal("Key");
			int nameColId = reader.GetOrdinal("Name");
			int descriptionColId = reader.GetOrdinal("Description");
			int rateColId = reader.GetOrdinal("Rate");
			int lengthColId = reader.GetOrdinal("Length");
			int imgColId = reader.GetOrdinal("Img");

			var results = new List<MovieModel>();
			while (reader.Read())
			{
				var result = new MovieModel
				{
					Id = reader.GetInt64(rowIdColId),
					Key = reader.IsDBNull(keyColId) ? null : reader.GetString(keyColId),
					Name = reader.IsDBNull(nameColId) ? null : reader.GetString(nameColId),
					Description = reader.IsDBNull(descriptionColId) ? null : reader.GetString(descriptionColId),
					Rate = reader.IsDBNull(rateColId) ? 0 : reader.GetDecimal(rateColId),
					Length = reader.IsDBNull(lengthColId) ? null : reader.GetString(lengthColId),
					Img = reader.IsDBNull(imgColId) ? null : reader.GetString(imgColId)
				};
				results.Add(result);
			}

			return results;
		}

	}
}
