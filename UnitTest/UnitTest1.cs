using System;
using System.Data.SQLite;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LuckyAceForm;

namespace LuckyAceForm
{
    public class TestBase : IDisposable
    {
        protected SQLiteConnection Connection;
        protected string TestDbPath = "Test_LuckyAce.db";

        public TestBase()
        {
            if (File.Exists(TestDbPath))
                File.Delete(TestDbPath);

            Connection = new SQLiteConnection($"Data Source={TestDbPath};Version=3;");
            Connection.Open();

            // Initialize test database
            InitializeTestDatabase();
        }

        private void InitializeTestDatabase()
        {
            // Same schema creation as your production code
            var createTables = @"
        CREATE TABLE Matches (Id INTEGER PRIMARY KEY AUTOINCREMENT, Date TEXT, Name TEXT, Team1 TEXT, Team2 TEXT);
        CREATE TABLE Users (Id INTEGER PRIMARY KEY AUTOINCREMENT, Username TEXT UNIQUE, Password TEXT, Balance REAL);
        CREATE TABLE Bets (Id INTEGER PRIMARY KEY AUTOINCREMENT, UserId INTEGER, MatchId INTEGER, Amount REAL, TeamBetOn TEXT);";

            new SQLiteCommand(createTables, Connection).ExecuteNonQuery();
        }

        public void Dispose()
        {
            Connection.Close();
            File.Delete(TestDbPath);
        }
    }
    public class MatchRepositoryTests : TestBase
    {
        public void AddMatch_ShouldPersistMatch()
        {
            // Arrange
            var repo = new MatchRepository(new SQLiteDb(TestDbPath));
            var testMatch = new Match(0, DateTime.Now, "Test Match", "Team A", "Team B");

            // Act
            repo.Add(testMatch);
            var matches = repo.GetAll();

            // Assert
            Assert.Single(matches);
            Assert.Equal("Test Match", matches[0].Name);
        }

        public void UpdateMatch_ShouldModifyExistingMatch()
        {
            // Arrange
            var repo = new MatchRepository(new SQLiteDb(TestDbPath));
            var testMatch = new Match(0, DateTime.Now, "Original", "A", "B");
            repo.Add(testMatch);
            var savedMatch = repo.GetAll().First();

            // Act
            savedMatch.Name = "Updated";
            repo.Update(savedMatch);
            var updatedMatch = repo.GetById(savedMatch.Id);

            // Assert
            Assert.Equal("Updated", updatedMatch.Name);
        }

        public void DeleteMatch_ShouldRemoveFromDatabase()
        {
            // Arrange
            var repo = new MatchRepository(new SQLiteDb(TestDbPath));
            var testMatch = new Match(0, DateTime.Now, "To Delete", "A", "B");
            repo.Add(testMatch);
            var savedMatch = repo.GetAll().First();

            // Act
            repo.Delete(savedMatch.Id);
            var matches = repo.GetAll();

            // Assert
            Assert.Empty(matches);
        }
    }
}
