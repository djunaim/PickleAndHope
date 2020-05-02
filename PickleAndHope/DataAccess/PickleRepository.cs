using PickleAndHope.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace PickleAndHope.DataAccess
{
    // repository has collection-like methods. Layer that handles accessing data
    // anything that is data storage should live here
    public class PickleRepository
    {
        // static means field should be shared by every instance of pickles controller for long erm data storage
        static List<Pickle> _pickles = new List<Pickle>() { new Pickle {Id = 1, Type = "Bread and Butter", NumberInStock = 5 } };

        const string ConnectionString = "Server=localhost;Database=PickleAndHope;Trusted_Connection=True;";       
        public void Add(Pickle pickle)
        {
            pickle.Id = _pickles.Max(x => x.Id) + 1;
            _pickles.Add(pickle);
        }
        public void Remove(string type)
        {
            throw new NotImplementedException();
        }
        public Pickle Update(Pickle pickle)
        {
            // find the first pickle type that matches and add it to the number in stock for the existing pickle 
            var pickleToUpdate = GetByType(pickle.Type);
            pickleToUpdate.NumberInStock += pickle.NumberInStock;
            return pickleToUpdate;
        }
        public Pickle GetByType(string type)
        {
            // firstordefault returns null
            //return _pickles.FirstOrDefault(p => p.Type == type);

            //sql connection
            //using statement allows connection to be ended once reach end curly brace by disposing that connection
            using (var connection = new SqlConnection(ConnectionString))
            {            
                connection.Open();

                //declare variable beforehand to avoid sql injection
                var query = @"select *
                            from Pickle
                            where Type = @type";

                //sql command
                var cmd = connection.CreateCommand();
                cmd.CommandText = query;
                //similar to declare statement in Sql
                //Type from query have to match the first Type from Paramters
                //Paramters.AddWithValue prevents sql injection
                cmd.Parameters.AddWithValue("Type", type);

                //execute the command
                var reader = cmd.ExecuteReader();

                //if there is 1 result, return the following and map it to something c# will understand
                //equivalen to firstordefault
                if (reader.Read())
                {
                    //var pickle = new Pickle
                    //{
                    //    Id = (int)reader["Id"],
                    //    Type = (string)reader["Type"],
                    //    Price = (decimal)reader["Price"],
                    //    NumberInStock = (int)reader["NumberInStock"],
                    //    Size = (string)reader["Size"]
                    //};

                    //return pickle;
                    return MapReaderToPickle(reader);
                }

                return null;
            }
        }
        public List<Pickle> GetAll()
        {
            //return _pickles;           

            //connection string
            // how to connect to database
            //var connectionString = "Server=localhost;Database=PickleAndHope;Trusted_Connection=True;";

            //sql connection - need to tell to open up in the beginning and close it at the end
            //actual connection
            var connection = new SqlConnection(ConnectionString);
            connection.Open();

            //sql command
            //createCommand goes down SqlConnection from connection
            //tell sql what to do on how to connect
            var cmd = connection.CreateCommand();

            //query to run against connection to run against connectionString
            cmd.CommandText = "select * from pickle";

            //sql data reader
            //run query and give results back
            //executing sql syntax from line 56
            var reader = cmd.ExecuteReader();

            var pickles = new List<Pickle>();

            //Read returns boolean and will keep reading until there are no more rows, which will return false
            //the current row that it reads, it becomes avilable in the reader            
            while (reader.Read())
            {
                //casting - take one value and saying it's a different kind of value
                //var id = (int)reader["Id"];
                //var type = (string)reader["Type"];


                //taking sql query and map to c# object
                //var pickle = new Pickle
                //{
                //    Id = (int) reader["Id"],
                //    Type = (string) reader["Type"],
                //    Price = (decimal) reader["Price"],
                //    NumberInStock = (int) reader["NumberInStock"],
                //    Size = (string) reader["Size"]
                //};
                var pickle = MapReaderToPickle(reader);
                pickles.Add(pickle);
            }

            connection.Close();

            return pickles;
        }
        public Pickle GetById(int id)
        {
            // return the first pickle.Id that matches id
            //return _pickles.FirstOrDefault(pickle => pickle.Id == id);

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                var cmd = connection.CreateCommand();

                //set command up with query necessary
                var query = @"select *
                            from Pickle
                            where Type = @id";

                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("id", id);

                var reader = cmd.ExecuteReader();

                //map results to object c# will understand
                if (reader.Read())
                {
                    return MapReaderToPickle(reader);
                }
                return null;
            }
        }

        Pickle MapReaderToPickle(SqlDataReader reader)
        {
            var pickle = new Pickle
            {
                Id = (int)reader["Id"],
                Type = (string)reader["Type"],
                Price = (decimal)reader["Price"],
                NumberInStock = (int)reader["NumberInStock"],
                Size = (string)reader["Size"]
            };

            return pickle;
        }
    }
}
