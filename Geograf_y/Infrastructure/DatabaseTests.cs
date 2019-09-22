using System;
using System.Data.SqlClient;
using Geograf_y.Base;

namespace Geograf_y
{
    public class DatabaseTests : IDisposable
    {
        private const string ConnectionString =
            @"Server=.;Initial Catalog=Geografy;Persist Security Info=False;Integrated Security=True;MultipleActiveResultSets=False;Connection Timeout=180;";

        protected SqlConnection SqlConnection { get; }

        public DatabaseTests()
        {
            SqlConnection = new SqlConnection(ConnectionString);
            SqlConnection.Open();
        }


        public void Dispose()
        {
            SqlConnection?.Dispose();
        }

        protected void DoInsert(ICanInsert canInsert) =>
            new SqlCommand(canInsert.InsertString(), SqlConnection).ExecuteNonQuery();

        protected void Do(string query) => new SqlCommand(query, SqlConnection).ExecuteNonQuery();
        protected T MakeScalar<T>(string query) => (T) new SqlCommand(query, SqlConnection).ExecuteScalar();
    }
}