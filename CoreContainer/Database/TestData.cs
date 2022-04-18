using Database.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Database
{
    public class TestData : ITestData
    {
        // Don't let data layer depend on NpgSql if possible
        // Pass in only base abstract classes
        private readonly DbConnection connection;

        public TestData(DbConnection connection)
        {
            this.connection = connection;
        }

        public async Task<TestObject> GetTestObject(long id)
        {
            await connection.OpenAsync();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT \"Id\", \"Value\" FROM public.\"TestTable\" WHERE \"Id\" = {id}";

                var reader = await command.ExecuteReaderAsync();
                return new TestObject
                {
                    Id = reader.GetInt64(0),
                    Value = reader.GetString(1)
                };
            }
        }

        public async Task UpdateTestObject(TestObject obj, CancellationToken token)
        {
            await connection.OpenAsync();
            using (var transaction = await connection.BeginTransactionAsync(token))
            {
                // Starting a transaction while another transaction is already in progress will throw an exception. 
                // Because of this, it isn't necessary to pass the NpgsqlTransaction object returned from BeginTransaction() 
                // to commands you execute - starting a transaction means that all subsequent commands will automatically 
                // participate in the transaction, until either a commit or rollback is performed. 
                // However, for maximum portability it's recommended to set the transaction on your commands.
                //Update if exists
                using (var command = connection.CreateCommand())
                {
                    command.Transaction = transaction;
                    command.CommandText = $"UPDATE public.\"TestTable\" SET \"Value\"='{obj.Value}' WHERE \"Id\"={obj.Id}";
                    await command.ExecuteNonQueryAsync(token);
                }

                await transaction.CommitAsync();
            }
        }

        public async Task<long> InsertTestObject(string value, CancellationToken token)
        {
            long result;

            await connection.OpenAsync();
            using (var transaction = await connection.BeginTransactionAsync(token))
            {
                using (var command = connection.CreateCommand())
                {
                    command.Transaction = transaction;
                    command.CommandText = $"INSERT INTO public.\"TestTable\" (\"Value\") VALUES ('{value}') RETURNING \"Id\"";
                    var scalar = await command.ExecuteScalarAsync(token);
                    result = Convert.ToInt64(scalar);
                }

                await transaction.CommitAsync();
            }

            return result;
        }

        public async Task DeleteTestObject(long id, CancellationToken token)
        {
            await connection.OpenAsync();

            using (var transaction = await connection.BeginTransactionAsync(token))
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"DELETE FROM public.\"TestTable\" WHERE \"Id\" = {id}";

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
