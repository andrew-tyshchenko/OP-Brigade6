using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuckyAceForm
{
    // Репозиторій для роботи зі ставками
    public class BetRepository
    {
        private readonly SQLiteDb db;

        public BetRepository(SQLiteDb db)
        {
            this.db = db;
        }

        public List<Bet> GetAll()
        {
            var bets = new List<Bet>();

            using (var connection = db.GetConnection())
            {
                connection.Open();
                string query = "SELECT * FROM Bets";

                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        bets.Add(new Bet(
                            reader.GetInt32(0),        // Id
                            reader.GetInt32(1),        // UserId
                            reader.GetInt32(2),        // MatchId
                            reader.GetDecimal(3),      // Amount
                            reader.GetString(4)        // TeamBetOn
                        ));
                    }
                }
            }

            return bets;
        }

        public List<Bet> GetByUserId(int userId)
        {
            var bets = new List<Bet>();

            using (var connection = db.GetConnection())
            {
                connection.Open();
                string query = "SELECT * FROM Bets WHERE UserId = @UserId";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bets.Add(new Bet(
                                reader.GetInt32(0),
                                reader.GetInt32(1),
                                reader.GetInt32(2),
                                reader.GetDecimal(3),
                                reader.GetString(4)
                            ));
                        }
                    }
                }
            }

            return bets;
        }

        public List<Bet> GetByMatchId(int matchId)
        {
            var bets = new List<Bet>();

            using (var connection = db.GetConnection())
            {
                connection.Open();
                string query = "SELECT * FROM Bets WHERE MatchId = @MatchId";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MatchId", matchId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bets.Add(new Bet(
                                reader.GetInt32(0),
                                reader.GetInt32(1),
                                reader.GetInt32(2),
                                reader.GetDecimal(3),
                                reader.GetString(4)
                            ));
                        }
                    }
                }
            }

            return bets;
        }

        public Bet GetById(int id)
        {
            using (var connection = db.GetConnection())
            {
                connection.Open();
                string query = "SELECT * FROM Bets WHERE Id = @Id";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Bet(
                                reader.GetInt32(0),
                                reader.GetInt32(1),
                                reader.GetInt32(2),
                                reader.GetDecimal(3),
                                reader.GetString(4)
                            );
                        }
                    }
                }
            }

            return null;
        }

        public void Add(Bet bet)
        {
            using (var connection = db.GetConnection())
            {
                connection.Open();
                string query = @"
            INSERT INTO Bets (UserId, MatchId, Amount, TeamBetOn) 
            VALUES (@UserId, @MatchId, @Amount, @TeamBetOn)";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", bet.UserId);
                    command.Parameters.AddWithValue("@MatchId", bet.MatchId);
                    command.Parameters.AddWithValue("@Amount", bet.Amount);
                    command.Parameters.AddWithValue("@TeamBetOn", bet.Team);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void Update(Bet bet)
        {
            using (var connection = db.GetConnection())
            {
                connection.Open();
                string query = @"
            UPDATE Bets 
            SET UserId = @UserId, 
                MatchId = @MatchId, 
                Amount = @Amount, 
                TeamBetOn = @TeamBetOn 
            WHERE Id = @Id";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", bet.Id);
                    command.Parameters.AddWithValue("@UserId", bet.UserId);
                    command.Parameters.AddWithValue("@MatchId", bet.MatchId);
                    command.Parameters.AddWithValue("@Amount", bet.Amount);
                    command.Parameters.AddWithValue("@TeamBetOn", bet.Team);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var connection = db.GetConnection())
            {
                connection.Open();
                string query = "DELETE FROM Bets WHERE Id = @Id";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public decimal GetTotalBetAmountForMatch(int matchId)
        {
            using (var connection = db.GetConnection())
            {
                connection.Open();
                string query = "SELECT SUM(Amount) FROM Bets WHERE MatchId = @MatchId";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MatchId", matchId);
                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToDecimal(result);
                    }
                    return 0;
                }
            }
        }
    }
}
