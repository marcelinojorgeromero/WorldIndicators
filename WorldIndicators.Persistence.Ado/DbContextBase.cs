using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace WorldIndicators.Persistence.Ado
{
    public class DbContextBase
    {
        protected readonly string ConnectionString;

        public DbContextBase()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public void Query(string queryStr, Action<SqlDataReader> readerAction, CommandType commandType = CommandType.StoredProcedure)
        {
            _Query(queryStr, readerAction, commandType);
        }

        public void Query(string queryStr, IEnumerable<SqlParameter> parameters, Action<SqlDataReader> readerAction, CommandType commandType = CommandType.StoredProcedure)
        {
            _Query(queryStr, readerAction, commandType, parameters);
        }

        private void _Query(string queryStr, Action<SqlDataReader> readerAction, CommandType commandType = CommandType.StoredProcedure, IEnumerable<SqlParameter> parameters = null)
        {
            SqlDataReader rdr = null;
            var connection = new SqlConnection(ConnectionString);
            
            try
            {
                connection.Open();

                var cmd = new SqlCommand(queryStr, connection) { CommandType = commandType };

                if (parameters != null) cmd.Parameters.AddRange(parameters.ToArray());

                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    readerAction(rdr);
                }
            }
            catch (Exception exception)
            {
                // TODO: Logger store the error
            }
            finally
            {
                if (rdr != null) rdr.Close();
                connection.Close();
            }
        }

        public void Execute(string queryStr, CommandType commandType = CommandType.StoredProcedure, Action<Exception> onError = null)
        {
            _Execute(queryStr, commandType, onError: onError);
        }

        public void Execute(string queryStr, IEnumerable<SqlParameter> parameters, CommandType commandType = CommandType.StoredProcedure, Action<Exception> onError = null)
        {
            _Execute(queryStr, commandType, parameters, onError);
        }

        private void _Execute(string queryStr, CommandType commandType = CommandType.StoredProcedure, IEnumerable<SqlParameter> parameters = null, Action<Exception> onError = null)
        {
            var connection = new SqlConnection(ConnectionString);

            try
            {
                connection.Open();
                var cmd = new SqlCommand(queryStr, connection) { CommandType = commandType };
                if (parameters != null) cmd.Parameters.AddRange(parameters.ToArray());
                cmd.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                onError?.Invoke(exception);
                // TODO: Logger store the error
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
