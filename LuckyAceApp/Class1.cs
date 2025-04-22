using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace LuckyAceForm
{

    public abstract class BaseEntity
    {
        public int Id { get; set; }
    }
    // Абстрактний клас для подій
    abstract class Event
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }

        protected Event(int id, DateTime date, string name)
        {
            Id = id;
            Date = date;
            Name = name;
        }

        public abstract void DisplayInfo();
    }

    // Наслідуваний клас Match
    public class Match : BaseEntity
    {
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string Team1 { get; set; }
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

    // Інтерфейс для ставок
    interface IBettable
    {
        void PlaceBet(decimal amount);
    }

    // Клас User
    class User : BaseEntity
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public decimal Balance { get; set; }

        public User(int id, string username, string email, decimal balance)
        {
            Id = id;
            Username = username;
            Email = email;
            Balance = balance;
        }

        // Клас Bet, що реалізує IBettable
        public class Bet : BaseEntity
        {
            public User Player { get; set; }
            public Match GameEvent { get; set; }
            public decimal Amount { get; set; }

            public Bet(int id, User player, Match gameEvent, decimal amount)
            {
                Id = id;
                Player = player;
                GameEvent = gameEvent;
                Amount = amount;
            }
        }

        // Репозиторій для роботи з сутностями
        interface IRepository<T>
        {
            void Add(T entity);
            void Remove(T entity);
            T GetById(int id);
            List<T> GetAll();
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
        public class MatchRepository : GenericRepository<Match>
        {
            public MatchRepository(IDataStorage<Match> storage) : base(storage) { }

            public List<Match> GetByTeamName(string team)
            {
                return GetAll().Where(m => m.Team1 == team || m.Team2 == team).ToList();
            }
        }

        // Репозиторій для роботи з користувачами
        public class UserRepository : GenericRepository<User>
        {
            public UserRepository(IDataStorage<User> storage) : base(storage) { }

            public User GetByUsername(string username)
            {
                return GetAll().FirstOrDefault(u => u.Username == username);
            }
        }

        // Репозиторій для роботи зі ставками
        public class BetRepository : GenericRepository<Bet>
        {
            public BetRepository(IDataStorage<Bet> storage) : base(storage) { }

            public List<Bet> GetByUserId(int userId)
            {
                return GetAll().Where(b => b.Player.Id == userId).ToList();
            }

            public List<Bet> GetByMatchId(int matchId)
            {
                return GetAll().Where(b => b.GameEvent.Id == matchId).ToList();
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

        public class JsonStorage<T> : IDataStorage<T> where T : BaseEntity
        {
            private readonly string _filePath;
            private List<T> _items;

            public JsonStorage(string filePath)
            {
                _filePath = filePath;
                _items = LoadFromFile();
            }

            private List<T> LoadFromFile()
            {
                if (!File.Exists(_filePath))
                    return new List<T>();

                var json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
            }

            public void Add(T entity)
            {
                entity.Id = _items.Any() ? _items.Max(e => e.Id) + 1 : 1;
                _items.Add(entity);
            }

            public void Update(T entity)
            {
                var index = _items.FindIndex(e => e.Id == entity.Id);
                if (index != -1)
                {
                    _items[index] = entity;
                }
            }

            public void Delete(int id)
            {
                var item = _items.FirstOrDefault(e => e.Id == id);
                if (item != null)
                {
                    _items.Remove(item);
                }
            }

            public T? GetById(int id)
            {
                return _items.FirstOrDefault(e => e.Id == id);
            }

            public List<T> GetAll()
            {
                return _items;
            }

            public void Save()
            {
                var json = JsonSerializer.Serialize(_items, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                File.WriteAllText(_filePath, json);
            }
        }

    }
}


