using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Data Source=DESKTOP-4SK39GD;Initial Catalog=ITacademy;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "select * from Users where BookId = @bookId";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@bookId", GetBookId("Війна та мир"));
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine(reader["Name"]);
                }
                reader.Close();
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "select Author from Books where Title = @title";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@title", "Війна та мир");
                object author = command.ExecuteScalar();
                Console.WriteLine(author);
            }
            DateTime date = new DateTime(2023, 2, 15);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "select Title from Books where TakeDate > @date";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@date", date);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine(reader["Title"]);
                }
                reader.Close();
            }
            int userId = GetUserId("Петро");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "select Price from Books where Id in " +
                            "(select BookId from Users where Id = @userId)";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@userId", userId);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine(reader["Price"]);
                }
                reader.Close();
            }
            int price = 100;
            DateTime date = new DateTime(2010, 1, 1);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "select Title from Books where Price > @price and ReleaseDate <= @date";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@price", price);
                command.Parameters.AddWithValue("@date", date);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine(reader["Title"]);
                }
                reader.Close();
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("GetBooksCount", connection);
                command.CommandType = CommandType.StoredProcedure;
                int count = (int)command.ExecuteScalar();
                Console.WriteLine("Кількість книг: " + count);
            }
            string author = "Лев Толстой";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("GetAuthorBooksInfo", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@author", author);
                SqlParameter sumPrices = new SqlParameter("@sumPrices", SqlDbType.Int);
                sumPrices.Direction = ParameterDirection.Output;
                command.Parameters.Add(sumPrices);
                SqlParameter sumPages = new SqlParameter("@sumPages", SqlDbType.Int);
                sumPages.Direction = ParameterDirection.Output;
                command.Parameters.Add(sumPages);
                command.ExecuteNonQuery();
                Console.WriteLine("Сума цін книг {0}: {1}", author, sumPrices.Value);
                Console.WriteLine("Сума сторінок книг {0}: {1}", author, sumPages.Value);
            }
        }
        static int GetBookId(string title)
        {
            return 1;
        }

        static int GetUserId(string name)
        {
            return 1;
        }
    }
}
