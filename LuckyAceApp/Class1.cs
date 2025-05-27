using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SQLite;
using System.Diagnostics.CodeAnalysis;
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

        public virtual SQLiteConnection GetConnection()
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
        [Range(typeof(decimal), "0.01", "79228162514264337593543950335", ErrorMessage = "Balance must be greater than zero.")]
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
        [Range(typeof(decimal), "0.01", "79228162514264337593543950335", ErrorMessage = "Bet must be greater than zero.")]
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



