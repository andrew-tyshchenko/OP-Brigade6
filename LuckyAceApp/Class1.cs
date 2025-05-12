using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SQLite;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace LuckyAceForm
{
    // База даних
    public class SQLiteDb
    {
        private const string DbFileName = "LuckyAce.db";
        private readonly string connectionString = $"Data Source={DbFileName};Version=3;";

        public SQLiteDb()
        {
            if (!File.Exists(DbFileName))
            {
                CreateDatabase();
            }
        }

        private void CreateDatabase()
        {
            SQLiteConnection.CreateFile(DbFileName);

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Create Matches table
                string createMatchesTable = @"
                CREATE TABLE Matches (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT NOT NULL,
                    Name TEXT NOT NULL,
                    Team1 TEXT NOT NULL,
                    Team2 TEXT NOT NULL
                )";

                ExecuteNonQuery(connection, createMatchesTable);

                // Create Users table
                string createUsersTable = @"
                CREATE TABLE Users (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT NOT NULL UNIQUE,
                    Password TEXT NOT NULL,
                    Balance REAL NOT NULL DEFAULT 0
                )";

                ExecuteNonQuery(connection, createUsersTable);

                // Create Bets table
                string createBetsTable = @"
                CREATE TABLE Bets (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    UserId INTEGER NOT NULL,
                    MatchId INTEGER NOT NULL,
                    Amount REAL NOT NULL,
                    TeamBetOn TEXT NOT NULL,
                    FOREIGN KEY(UserId) REFERENCES Users(Id),
                    FOREIGN KEY(MatchId) REFERENCES Matches(Id)
                )";

                ExecuteNonQuery(connection, createBetsTable);
            }
        }

        private void ExecuteNonQuery(SQLiteConnection connection, string commandText)
        {
            using (var command = new SQLiteCommand(commandText, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        public SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(connectionString);
        }
    }

    // Валідація даних
    public static class ValidationService
    {
        public static List<ValidationResult> Validate<T>(T entity)
        {
            var context = new ValidationContext(entity);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(entity, context, results,
            validateAllProperties: true);
            return results;
        }
        public static bool IsValid<T>(T entity)
        {
            return Validate(entity).Count == 0;
        }
    }

    // Базовий класс від якого наслідуються інші
    public abstract class BaseEntity
    {
        [Required]
        public int Id { get; set; }
    }
    
    // Класс матчу
    public class Match : BaseEntity
    {
        [Required]
        public DateTime Date { get; set; }
        [Required, MinLength(3), MaxLength(30)]
        public string Name { get; set; }
        [Required, MinLength(3), MaxLength(30)]
        public string Team1 { get; set; }
        [Required, MinLength(3), MaxLength(30)]
        public string Team2 { get; set; }

        public Match(int id, DateTime date, string name, string team1, string team2)
        {
            Id = id;
            Date = date;
            Name = name;
            Team1 = team1;
            Team2 = team2;
        }
    }

    // Клас користувачів
    public class User : BaseEntity
    {
        [Required, MinLength(3), MaxLength(30)]
        public string Username { get; set; }
        [Required, MinLength(3), MaxLength(30)]
        public string Password { get; set; }
        [Required]
        public decimal Balance { get; set; }

        // Updated constructor with balance parameter
        public User(int id, string username, string password, decimal balance)
        {
            Id = id;
            Username = username;
            Password = password;
            Balance = balance;
        }

        // For creating new users with default balance
        public User(string username, string password) : this(0, username, password, 1000)
        {
        }
    }

    // Клас ставок
    public class Bet : BaseEntity
    {
        [Required]
        public int UserId { get; set; }  // Reference to User, not the User object
        [Required]
        public int MatchId { get; set; } // Reference to Match, not the Match object
        [Required]
        public decimal Amount { get; set; }
        [Required, MinLength(1), MaxLength(30)]
        public string Team { get; set; }

        public Bet(int id, int userId, int matchId, decimal amount, string team)
        {
            Id = id;
            UserId = userId;
            MatchId = matchId;
            Amount = amount;
            Team = team;
        }
    }


    public interface IDataStorage<T> where T : BaseEntity
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);
        T? GetById(int id);
        List<T> GetAll();
        void Save();
    }

    // Загальний репозиторій для роботи з усіма сутностями
    public class GenericRepository<T> where T : BaseEntity
    {
        private readonly IDataStorage<T> _storage;

        public GenericRepository(IDataStorage<T> storage)
        {
            _storage = storage;
        }

        public void Add(T entity)
        {
            _storage.Add(entity);
            _storage.Save();
        }

        public void Update(T entity)
        {
            _storage.Update(entity);
            _storage.Save();
        }

        public void Delete(int id)
        {
            _storage.Delete(id);
            _storage.Save();
        }

        public T? GetById(int id)
        {
            return _storage.GetById(id);
        }

        public List<T> GetAll()
        {
            return _storage.GetAll();
        }
    }

    // Репозиторій для роботи з матчами
    public class MatchRepository
    {
        private readonly SQLiteDb db;

        public MatchRepository(SQLiteDb db)
        {
            this.db = db;
        }

        public List<Match> GetAll()
        {
            var matches = new List<Match>();

            using (var connection = db.GetConnection())
            {
                connection.Open();
                string query = "SELECT * FROM Matches";

                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        matches.Add(new Match(
                            reader.GetInt32(0),        // Id
                            DateTime.Parse(reader.GetString(1)), // Date
                            reader.GetString(2),       // Name
                            reader.GetString(3),       // Team1
                            reader.GetString(4)        // Team2
                        ));
                    }
                }
            }

            return matches;
        }

        public Match GetById(int id)
        {
            using (var connection = db.GetConnection())
            {
                connection.Open();
                string query = "SELECT * FROM Matches WHERE Id = @Id";

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
            using (var connection = db.GetConnection())
            {
                connection.Open();
                string query = @"
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
            using (var connection = db.GetConnection())
            {
                connection.Open();
                string query = @"
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
            using (var connection = db.GetConnection())
            {
                connection.Open();
                string query = "DELETE FROM Matches WHERE Id = @Id";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }

    // Репозиторій для роботи з користувачами
    public class UserRepository
    {
        private readonly SQLiteDb db;

        public UserRepository(SQLiteDb db)
        {
            this.db = db;
        }

        public List<User> GetAll()
        {
            var users = new List<User>();

            using (var connection = db.GetConnection())
            {
                connection.Open();
                string query = "SELECT * FROM Users";

                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User(
                            reader.GetInt32(0),        // Id
                            reader.GetString(1),       // Username
                            reader.GetString(2),       // Password
                            reader.GetDecimal(3)       // Balance
                        ));
                    }
                }
            }

            return users;
        }

        public User GetById(int id)
        {
            using (var connection = db.GetConnection())
            {
                connection.Open();
                string query = "SELECT * FROM Users WHERE Id = @Id";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User(
                                reader.GetInt32(0),
                                reader.GetString(1),
                                reader.GetString(2),
                                reader.GetDecimal(3)
                            );
                        }
                    }
                }
            }

            return null;
        }

        public User GetByUsername(string username)
        {
            using (var connection = db.GetConnection())
            {
                connection.Open();
                string query = "SELECT * FROM Users WHERE Username = @Username";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User(
                                reader.GetInt32(0),
                                reader.GetString(1),
                                reader.GetString(2),
                                reader.GetDecimal(3)
                            );
                        }
                    }
                }
            }

            return null;
        }

        public void Add(User user)
        {
            using (var connection = db.GetConnection())
            {
                connection.Open();
                string query = @"
            INSERT INTO Users (Username, Password, Balance) 
            VALUES (@Username, @Password, @Balance)";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", user.Username);
                    command.Parameters.AddWithValue("@Password", user.Password);
                    command.Parameters.AddWithValue("@Balance", user.Balance);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void Update(User user)
        {
            using (var connection = db.GetConnection())
            {
                connection.Open();
                string query = @"
            UPDATE Users 
            SET Username = @Username, 
                Password = @Password, 
                Balance = @Balance 
            WHERE Id = @Id";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", user.Id);
                    command.Parameters.AddWithValue("@Username", user.Username);
                    command.Parameters.AddWithValue("@Password", user.Password);
                    command.Parameters.AddWithValue("@Balance", user.Balance);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var connection = db.GetConnection())
            {
                connection.Open();
                string query = "DELETE FROM Users WHERE Id = @Id";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public bool UsernameExists(string username)
        {
            using (var connection = db.GetConnection())
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    long count = (long)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }
    }

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

    public static class SQLiteHelper
    {
        public static DateTime? GetNullableDateTime(SQLiteDataReader reader, int ordinal)
        {
            return reader.IsDBNull(ordinal) ?
                (DateTime?)null :
                DateTime.Parse(reader.GetString(ordinal));
        }

        public static string FormatDateTime(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}



