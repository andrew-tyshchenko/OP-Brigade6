using LuckyAceForm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace LuckyAceForm.Tests
{
    public class UserTests
    {
        [Fact]
        public void User_ValidData_ShouldPassValidation()
        {
            // Arrange
            var user = new User(1, "validuser", "validpassword123", 1000);

            // Act
            var results = ValidationService.Validate(user);

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public void User_ShortUsername_ShouldFailValidation()
        {
            // Arrange
            var user = new User(1, "ab", "validpassword123", 1000);

            // Act
            var results = ValidationService.Validate(user);

            // Assert
            Assert.Contains(results, r => r.MemberNames.Contains("Username"));
        }

        [Fact]
        public void User_NegativeBalance_ShouldFailValidation()
        {
            // Arrange
            var user = new User(1, "validuser", "validpassword123", -100);

            // Act
            var results = ValidationService.Validate(user);

            // Assert
            Assert.Contains(results, r => r.MemberNames.Contains("Balance"));
        }

        [Fact]
        public void User_DefaultConstructor_SetsDefaultBalance()
        {
            // Arrange & Act
            var user = new User("testuser", "testpass");

            // Assert
            Assert.Equal(1000, user.Balance);
        }
    }

    public class MatchTests
    {
        [Fact]
        public void Match_ValidData_ShouldPassValidation()
        {
            // Arrange
            var match = new Match(1, DateTime.Now.AddDays(1), "Valid Match", "Team A", "Team B");

            // Act
            var results = ValidationService.Validate(match);

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public void Match_PastDate_ShouldPassValidation()
        {
            // Arrange
            var match = new Match(1, DateTime.Now.AddDays(-1), "Past Match", "Team A", "Team B");

            // Act
            var results = ValidationService.Validate(match);

            // Assert
            Assert.Empty(results); // Assuming date validation isn't implemented
        }

        [Fact]
        public void Match_ShortTeamName_ShouldFailValidation()
        {
            // Arrange
            var match = new Match(1, DateTime.Now.AddDays(1), "Valid Match", "A", "Team B");

            // Act
            var results = ValidationService.Validate(match);

            // Assert
            Assert.Contains(results, r => r.MemberNames.Contains("Team1"));
        }
    }

    public class BetTests
    {
        [Fact]
        public void Bet_ValidData_ShouldPassValidation()
        {
            // Arrange
            var bet = new Bet(1, 1, 1, 100, "Team A");

            // Act
            var results = ValidationService.Validate(bet);

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public void Bet_ZeroAmount_ShouldFailValidation()
        {
            // Arrange
            var bet = new Bet(1, 1, 1, 0, "Team A");

            // Act
            var results = ValidationService.Validate(bet);

            // Assert
            Assert.Contains(results, r => r.MemberNames.Contains("Amount"));
        }

        [Fact]
        public void Bet_EmptyTeam_ShouldFailValidation()
        {
            // Arrange
            var bet = new Bet(1, 1, 1, 100, "");

            // Act
            var results = ValidationService.Validate(bet);

            // Assert
            Assert.Contains(results, r => r.MemberNames.Contains("Team"));
        }
    }

    public class SQLiteDbTests
    {
        [Fact]
        public void GetConnection_ShouldReturnValidConnection()
        {
            // Arrange
            var db = new SQLiteDb();

            // Act
            using var connection = db.GetConnection();

            // Assert
            Assert.NotNull(connection);
            Assert.Equal("System.Data.SQLite.SQLiteConnection", connection.GetType().ToString());
        }
    }

    public class MatchRepositoryTests
    {
        [Fact]
        public void AddMatch_ShouldNotThrowException()
        {
            // Arrange
            var db = new SQLiteDb();
            var repository = new MatchRepository(db);
            var match = new Match(1, DateTime.Now.AddDays(1), "Test Match", "Team A", "Team B");

            // Act & Assert
            var exception = Record.Exception(() => repository.Add(match));
            Assert.Null(exception);
        }

        [Fact]
        public void GetById_NonExistentMatch_ShouldReturnNull()
        {
            // Arrange
            var db = new SQLiteDb();
            var repository = new MatchRepository(db);

            // Act
            var result = repository.GetById(-1); // Assuming -1 doesn't exist

            // Assert
            Assert.Null(result);
        }
    }

    public class UserRepositoryTests
    {
        [Fact]
        public void GetByUsername_NonExistentUser_ShouldReturnNull()
        {
            // Arrange
            var db = new SQLiteDb();
            var repository = new UserRepository(db);

            // Act
            var result = repository.GetByUsername("nonexistentuser");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void UsernameExists_NonExistentUser_ShouldReturnFalse()
        {
            // Arrange
            var db = new SQLiteDb();
            var repository = new UserRepository(db);

            // Act
            var result = repository.UsernameExists("nonexistentuser");

            // Assert
            Assert.False(result);
        }
    }

    public class BetRepositoryTests
    {
        [Fact]
        public void GetTotalBetAmountForMatch_NoBets_ShouldReturnZero()
        {
            // Arrange
            var db = new SQLiteDb();
            var repository = new BetRepository(db);

            // Act
            var result = repository.GetTotalBetAmountForMatch(-1); // Assuming -1 doesn't exist

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void GetByUserId_NoBets_ShouldReturnEmptyList()
        {
            // Arrange
            var db = new SQLiteDb();
            var repository = new BetRepository(db);

            // Act
            var result = repository.GetByUserId(-1); // Assuming -1 doesn't exist

            // Assert
            Assert.Empty(result);
        }
    }

    public class GenericRepositoryTests
    {
        // Note: This would require a mock implementation of IDataStorage for proper testing
        // This is just a placeholder to show how you might structure such tests
        [Fact]
        public void Add_ValidEntity_ShouldCallStorageAdd()
        {
            // Arrange
            var mockStorage = new MockDataStorage<User>();
            var repository = new GenericRepository<User>(mockStorage);
            var user = new User("testuser", "testpass");

            // Act
            repository.Add(user);

            // Assert
            Assert.True(mockStorage.AddWasCalled);
        }

        private class MockDataStorage<T> : IDataStorage<T> where T : BaseEntity
        {
            public bool AddWasCalled { get; private set; }

            public void Add(T entity)
            {
                AddWasCalled = true;
            }

            public void Delete(int id) { }
            public List<T> GetAll() => new List<T>();
            public T? GetById(int id) => default;
            public void Save() { }
            public void Update(T entity) { }
        }
    }
}