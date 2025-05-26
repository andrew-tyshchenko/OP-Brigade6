using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace LuckyAceForm
{
    public class MatchRepository
    {
        private readonly SQLiteDb _db;

        public MatchRepository(SQLiteDb db)
        {
            _db = db;
        }

        public List<Match> GetAll()
        {
            var matches = new List<Match>();

            using (var connection = _db.GetConnection())
            {
                connection.Open();
                const string query = "SELECT * FROM Matches";

                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        matches.Add(new Match(
                            reader.GetInt32(0),
                            DateTime.Parse(reader.GetString(1)),
                            reader.GetString(2),
                            reader.GetString(3),
                            reader.GetString(4)
                        ));
                    }
                }
            }
            return matches;
        }

        public Match GetById(int id)
        {
            using (var connection = _db.GetConnection())
            {
                connection.Open();
                const string query = "SELECT * FROM Matches WHERE Id = @Id";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Match(
                                reader.GetInt32(0),
                                DateTime.Parse(reader.GetString(1)),
                                reader.GetString(2),
                                reader.GetString(3),
                                reader.GetString(4)
                            );
                        }
                    }
                }
            }
            return null;
        }

        public void Add(Match match)
        {
            using (var connection = _db.GetConnection())
            {
                connection.Open();
                const string query = @"
                    INSERT INTO Matches (Date, Name, Team1, Team2) 
                    VALUES (@Date, @Name, @Team1, @Team2)";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Date", match.Date.ToString("yyyy-MM-dd HH:mm:ss"));
                    command.Parameters.AddWithValue("@Name", match.Name);
                    command.Parameters.AddWithValue("@Team1", match.Team1);
                    command.Parameters.AddWithValue("@Team2", match.Team2);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void Update(Match match)
        {
            using (var connection = _db.GetConnection())
            {
                connection.Open();
                const string query = @"
                    UPDATE Matches 
                    SET Date = @Date, 
                        Name = @Name, 
                        Team1 = @Team1, 
                        Team2 = @Team2 
                    WHERE Id = @Id";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", match.Id);
                    command.Parameters.AddWithValue("@Date", match.Date.ToString("yyyy-MM-dd HH:mm:ss"));
                    command.Parameters.AddWithValue("@Name", match.Name);
                    command.Parameters.AddWithValue("@Team1", match.Team1);
                    command.Parameters.AddWithValue("@Team2", match.Team2);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var connection = _db.GetConnection())
            {
                connection.Open();
                const string query = "DELETE FROM Matches WHERE Id = @Id";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}