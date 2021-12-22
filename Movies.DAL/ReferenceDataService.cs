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
				Mode = SqliteOpenMode.ReadOnly,
				Cache = SqliteCacheMode.Shared,
			};

			_connectionString = builder.ToString();
			_scheduler = new ConcurrentExclusiveSchedulerPair(TaskScheduler.Default, maxConcurrencyLevel: 1).ExclusiveScheduler;
		}

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
