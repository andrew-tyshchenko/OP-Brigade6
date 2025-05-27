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
            var user = new User(1, "validuser", "validpassword123", 1000);

            var results = ValidationService.Validate(user);

            Assert.Empty(results);
        }

        [Fact]
        public void User_ShortUsername_ShouldFailValidation()
        {
            var user = new User(1, "ab", "validpassword123", 1000);

            var results = ValidationService.Validate(user);

            Assert.Contains(results, r => r.MemberNames.Contains("Username"));
        }

        [Fact]
        public void User_NegativeBalance_ShouldFailValidation()
        {
            var user = new User(1, "validuser", "validpassword123", -100);

            var results = ValidationService.Validate(user);

            Assert.Contains(results, r => r.MemberNames.Contains("Balance"));
        }

        [Fact]
        public void User_DefaultConstructor_SetsDefaultBalance()
        {
            var user = new User("testuser", "testpass");

            Assert.Equal(1000, user.Balance);
        }
    }

    public class MatchTests
    {
        [Fact]
        public void Match_ValidData_ShouldPassValidation()
        {
            var match = new Match(1, DateTime.Now.AddDays(1), "Valid Match", "Team A", "Team B");

            var results = ValidationService.Validate(match);

            Assert.Empty(results);
        }

        [Fact]
        public void Match_PastDate_ShouldPassValidation()
        {
            var match = new Match(1, DateTime.Now.AddDays(-1), "Past Match", "Team A", "Team B");

            var results = ValidationService.Validate(match);

            Assert.Empty(results);
        }

        [Fact]
        public void Match_ShortTeamName_ShouldFailValidation()
        {
            var match = new Match(1, DateTime.Now.AddDays(1), "Valid Match", "A", "Team B");

            var results = ValidationService.Validate(match);

            Assert.Contains(results, r => r.MemberNames.Contains("Team1"));
        }
    }

    public class BetTests
    {
        [Fact]
        public void Bet_ValidData_ShouldPassValidation()
        {
            var bet = new Bet(1, 1, 1, 100, "Team A");

            var results = ValidationService.Validate(bet);

            Assert.Empty(results);
        }

        [Fact]
        public void Bet_ZeroAmount_ShouldFailValidation()
        {
            var bet = new Bet(1, 1, 1, 0, "Team A");

            var results = ValidationService.Validate(bet);

            Assert.Contains(results, r => r.MemberNames.Contains("Amount"));
        }

        [Fact]
        public void Bet_EmptyTeam_ShouldFailValidation()
        {
            var bet = new Bet(1, 1, 1, 100, "");

            var results = ValidationService.Validate(bet);

            Assert.Contains(results, r => r.MemberNames.Contains("Team"));
        }
    }

    public class SQLiteDbTests
    {
        [Fact]
        public void GetConnection_ShouldReturnValidConnection()
        {
            var db = new SQLiteDb();

            using var connection = db.GetConnection();

            Assert.NotNull(connection);
            Assert.Equal("System.Data.SQLite.SQLiteConnection", connection.GetType().ToString());
        }
    }

    public class MatchRepositoryTests
    {
        [Fact]
        public void AddMatch_ShouldNotThrowException()
        {
            var db = new SQLiteDb();
            var repository = new MatchRepository(db);
            var match = new Match(1, DateTime.Now.AddDays(1), "Test Match", "Team A", "Team B");

            var exception = Record.Exception(() => repository.Add(match));
            Assert.Null(exception);
        }

        [Fact]
        public void GetById_NonExistentMatch_ShouldReturnNull()
        {
            var db = new SQLiteDb();
            var repository = new MatchRepository(db);

            var result = repository.GetById(-1);

            Assert.Null(result);
        }
    }

    public class UserRepositoryTests
    {
        [Fact]
        public void GetByUsername_NonExistentUser_ShouldReturnNull()
        {
            var db = new SQLiteDb();
            var repository = new UserRepository(db);

            var result = repository.GetByUsername("nonexistentuser");

            Assert.Null(result);
        }

        [Fact]
        public void UsernameExists_NonExistentUser_ShouldReturnFalse()
        {
            var db = new SQLiteDb();
            var repository = new UserRepository(db);

            var result = repository.UsernameExists("nonexistentuser");

            Assert.False(result);
        }
    }

    public class BetRepositoryTests
    {
        [Fact]
        public void GetTotalBetAmountForMatch_NoBets_ShouldReturnZero()
        {
            var db = new SQLiteDb();
            var repository = new BetRepository(db);

            var result = repository.GetTotalBetAmountForMatch(-1);

            Assert.Equal(0, result);
        }

        [Fact]
        public void GetByUserId_NoBets_ShouldReturnEmptyList()
        {
            var db = new SQLiteDb();
            var repository = new BetRepository(db);

            var result = repository.GetByUserId(-1);

            Assert.Empty(result);
        }
    }

    public class GenericRepositoryTests
    {
        [Fact]
        public void Add_ValidEntity_ShouldCallStorageAdd()
        {
            var mockStorage = new MockDataStorage<User>();
            var repository = new GenericRepository<User>(mockStorage);
            var user = new User("testuser", "testpass");

            repository.Add(user);

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