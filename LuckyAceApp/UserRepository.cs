using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuckyAceForm
{
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
}
