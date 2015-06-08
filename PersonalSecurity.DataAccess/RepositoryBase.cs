namespace PersonalSecurity.DataAccess
{
    using System;
    using System.Data.SqlClient;

    public class RepositoryBase
    {
        private readonly string _connectionString;

        public RepositoryBase(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected T WithConnection<T>(Func<SqlConnection, T> payload)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return payload.Invoke(connection);
            }
        }

        protected void WithConnection(Action<SqlConnection> payload)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                payload.Invoke(connection);
            }
        }

        protected void WithTransaction(Action<SqlConnection, SqlTransaction> payload)
        {
            WithConnection(connection =>
            {
                using (var transaction = connection.BeginTransaction())
                {
                    payload.Invoke(connection, transaction);
                    transaction.Commit();
                }
            });
        }
    }
}
