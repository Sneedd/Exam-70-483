﻿using System;
using System.Data.Common;
using System.Linq;
using System.IO;

namespace Example
{
    /// <summary>
    /// 
    /// </summary>
    public class ConsumeData
    {
        #region ADO.NET Examples

        public void RunAdoNetExamples()
        {
            // --------------------------------------------------------------------------------------------
            // Creating the abstract base classes
            string path = Path.Combine(Environment.CurrentDirectory, "database.mdf");
            DbConnection connection = new System.Data.SqlClient.SqlConnection(
                string.Format(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={0};Integrated Security=True", path));
            connection.Open();
            DbProviderFactory factory = DbProviderFactories.GetFactory(connection);

            // --------------------------------------------------------------------------------------------
            // ExecuteNonQuery            
            using (DbCommand createCommand = factory.CreateCommand())
            {
                createCommand.Connection = connection;
                createCommand.CommandText = "CREATE TABLE [Bible] ([Id] INT NOT NULL PRIMARY KEY, [Word] NVARCHAR(50) NOT NULL, [Count] INT NOT NULL)";
                createCommand.ExecuteNonQuery();
            }

            // --------------------------------------------------------------------------------------------
            // ExecuteNonQuery with parameters
            using (DbCommand insertCommand = factory.CreateCommand())
            {
                DateTime current = DateTime.UtcNow;
                insertCommand.Connection = connection;
                insertCommand.CommandText = "INSERT INTO [Bible] ([Id], [Word], [Count]) VALUES (@id, @word, @count)";

                DbParameter idParameter = factory.CreateParameter();
                idParameter.DbType = System.Data.DbType.Int32;
                idParameter.ParameterName = "@id";
                idParameter.Size = 4;
                idParameter.Value = 0;
                insertCommand.Parameters.Add(idParameter);

                DbParameter wordParameter = factory.CreateParameter();
                wordParameter.DbType = System.Data.DbType.String;
                wordParameter.ParameterName = "@word";
                wordParameter.Size = 100;
                wordParameter.Value = 0;
                insertCommand.Parameters.Add(wordParameter);

                DbParameter countParameter = factory.CreateParameter();
                countParameter.DbType = System.Data.DbType.Int32;
                countParameter.Size = 4;
                countParameter.ParameterName = "@count";
                countParameter.Value = 0;
                insertCommand.Parameters.Add(countParameter);

                insertCommand.Prepare();

                int id = 0;
                string bible = StringData.GetBible();
                string[] words = bible.Split(new char[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string word in words.Distinct())
                {
                    idParameter.Value = id++;
                    wordParameter.Value = word;
                    countParameter.Value = words.Count(a => a == word);
                    insertCommand.ExecuteNonQuery();
                }
                Console.WriteLine("[DbCommand.Parameters] Inserted {0} rows in {1:0.000}s", id+1, (DateTime.UtcNow - current).TotalSeconds);
            }

            // --------------------------------------------------------------------------------------------
            // ExecuteScalar
            //   TODO
            using (DbCommand countCommand = factory.CreateCommand())
            {
                DateTime current = DateTime.UtcNow;
                countCommand.Connection = connection;
                countCommand.CommandText = "SELECT COUNT(*) FROM [Bible]";
                int count = (int)countCommand.ExecuteScalar();
                Console.WriteLine("[DbCommand.ExecuteScalar] Result Count = {0} in {1:0.000}s", count, (DateTime.UtcNow - current).TotalSeconds);
            }


        }

        #endregion

        #region Using WebServices Examples

        public void RunUsingWebServicesExamples()
        {

        }

        #endregion

        #region Consume JSON or XML Data Examples

        public void RunConsumeJsonXmlDataExamples()
        {
        }

        #endregion
    }
}