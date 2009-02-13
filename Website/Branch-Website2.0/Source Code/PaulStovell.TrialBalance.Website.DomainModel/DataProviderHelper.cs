using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Reflection;
using PaulStovell.TrialBalance.Website.Common;
using System.Data;

namespace PaulStovell.TrialBalance.WebsiteCommon {
    /// <summary>
    /// A helper class for quickly loading data from and passing data to stored procedures.
    /// </summary>
    public class DataProviderHelper {
        private string _connectionString;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionString">The connection string to use.</param>
        public DataProviderHelper(string connectionString) {
            _connectionString = connectionString;
        }

        public T ExecuteSingleSelectCommand<T>(SqlCommand selectCommand) where T : new() {
            T result = default(T);

            BindingList<T> results = ExecuteSelectCommand<T>(selectCommand);
            if (results.Count > 0) {
                result = results[0];
            }

            return result;
        }

        public BindingList<T> ExecuteSelectCommand<T>(SqlCommand selectCommand) where T : new() {
            BindingList<T> results = new BindingList<T>();

            SqlConnection connection = null;
            try {
                connection = new SqlConnection(_connectionString);
                connection.Open();
                selectCommand.Connection = connection;

                SqlDataReader reader = selectCommand.ExecuteReader();
                while (reader.Read()) {
                    T t = new T();

                    for (int c = 0; c < reader.FieldCount; c++) {
                        string columnName = reader.GetName(c);
                        object columnValue = reader[c];

                        if (columnValue != DBNull.Value) {
                            PropertyInfo propertyInfo = t.GetType().GetProperty(columnName);
                            if (propertyInfo != null) {
                                propertyInfo.SetValue(t, columnValue, null);
                            }
                        }
                    }

                    results.Add(t);
                }
            } catch (SqlException ex) {
                ExceptionHandler.HandleException(new ApplicationException(ex.Message, ex));
            } finally {
                if (selectCommand != null) {
                    selectCommand.Dispose();
                }
                if (connection != null) {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return results;
        }

        public int ExecuteNonQuery(SqlCommand command) {
            int result = 0;
            SqlConnection connection = null;
            try {
                connection = new SqlConnection(_connectionString);
                connection.Open();
                command.Connection = connection;

                result = command.ExecuteNonQuery();
            } catch (SqlException ex) {
                ExceptionHandler.HandleException(new ApplicationException(ex.Message, ex));
            } finally {
                if (command != null) {
                    command.Dispose();
                }
                if (connection != null) {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return result;
        }

        public object ExecuteScalar(SqlCommand command) {
            object result = null;
            SqlConnection connection = null;
            try {
                connection = new SqlConnection(_connectionString);
                connection.Open();
                command.Connection = connection;

                result = command.ExecuteScalar();
            } catch (SqlException ex) {
                ExceptionHandler.HandleException(new ApplicationException(ex.Message, ex));
            } finally {
                if (command != null) {
                    command.Dispose();
                }
                if (connection != null) {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return result;
        }

        public SqlCommand CreateCommand(string procedure, params SqlParameter[] parameters) {
            SqlCommand command = new SqlCommand(procedure);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddRange(parameters);
            return command;
        }

        public SqlParameter CreateParameter(string name, object value) {
            return new SqlParameter(name, value);
        }

        public SqlParameter CreateOutParameter(string name, object value) {
            SqlParameter p = new SqlParameter(name, value);
            p.Direction = ParameterDirection.InputOutput;
            return p;
        }
    }
}
