using PickleAndHope.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using Dapper;

namespace PickleAndHope.DataAccess
{
    // repository has collection-like methods. Layer that handles accessing data
    // anything that is data storage should live here
    public class PickleRepository
    {
        // static means field should be shared by every instance of pickles controller for long erm data storage
        //static List<Pickle> _pickles = new List<Pickle>() { new Pickle {Id = 1, Type = "Bread and Butter", NumberInStock = 5 } };

        const string ConnectionString = "Server=localhost;Database=PickleAndHope;Trusted_Connection=True;";       
        public Pickle Add(Pickle pickle)
        {
            //pickle.Id = _pickles.Max(x => x.Id) + 1;
            //_pickles.Add(pickle);

            //creating variables with the @ symbol
            var sql = @"insert into Pickle(NumberInStock, Price, Size, Type)                        
                        output inserted.*
                        values(@NumberInStock, @Price, @Size, @Type)";

            //Dapper adds features to connection
            using (var db = new SqlConnection(ConnectionString))
            {
                //can map to c# object in angle brackets. Match property name with name of columns
                //can map property of pickle into parameter that we need because property names of pickle class match the parameters that we are trying to find
                //also opens up connection for us
                var result = db.QueryFirstOrDefault<Pickle>(sql, pickle);
                return result;

                //db.Open();

                ////create command
                //var cmd = db.CreateCommand();
                ////set command text
                //cmd.CommandText = sql;

                ////create parameters
                ////how pass data from c# to tsql
                //cmd.Parameters.AddWithValue("NumberInStock", pickle.NumberInStock);
                //cmd.Parameters.AddWithValue("Price", pickle.Price);
                //cmd.Parameters.AddWithValue("Size", pickle.Size);
                //cmd.Parameters.AddWithValue("Type", pickle.Type);

                //var reader = cmd.ExecuteReader();

                //if (reader.Read())
                //{
                //    var newPickle = MapReaderToPickle(reader);
                //    return newPickle;
                //}

                //return null;
            }
        }
        public void Remove(string type)
        {
            throw new NotImplementedException();
        }
        public Pickle Update(Pickle pickle)
        {
            // find the first pickle type that matches and add it to the number in stock for the existing pickle 
            //var pickleToUpdate = GetByType(pickle.Type);
            //pickleToUpdate.NumberInStock += pickle.NumberInStock;
            //return pickleToUpdate;

            var sql = @"update Pickle
                        set NumberInStock = NumberInStock + @NewStock
                        where Id = @id";

            using (var db = new SqlConnection(ConnectionString))
            {
                var parameters = new 
                { 
                    NewStock = pickle.NumberInStock, 
                    Id = pickle.Id 
                };

                return db.QueryFirstOrDefault<Pickle>(sql, parameters);
                
                //connection.Open();

                //var cmd = connection.CreateCommand();
                //cmd.CommandText = sql;

                //cmd.Parameters.AddWithValue("NewStock", pickle.NumberInStock);
                //cmd.Parameters.AddWithValue("id", pickle.Id);

                //var reader = cmd.ExecuteReader();
                //if (reader.Read())
                //{
                //    var updatedPickle = MapReaderToPickle(reader);
                //    return updatedPickle;
                //}

                //return null;
            }
        }
        public Pickle GetByType(string type)
        {
            // firstordefault returns null
            //return _pickles.FirstOrDefault(p => p.Type == type);

            //declare variable beforehand to avoid sql injection
            var query = @"select *
                        from Pickle
                        where Type = @type";

            //sql connection
            //using statement allows connection to be ended once reach end curly brace by disposing that connection
            using (var db = new SqlConnection(ConnectionString))
            {
                var parameters = new { Type = type };
                var pickle = db.QueryFirstOrDefault<Pickle>(query, parameters);
                return pickle;
                
                //connection.Open();

                ////sql command
                //var cmd = connection.CreateCommand();
                //cmd.CommandText = query;
                ////similar to declare statement in Sql
                ////Type from query have to match the first Type from Paramters
                ////Parameters.AddWithValue prevents sql injection
                //cmd.Parameters.AddWithValue("Type", type);

                ////execute the command
                //var reader = cmd.ExecuteReader();

                ////if there is 1 result, return the following and map it to something c# will understand
                ////equivalen to firstordefault
                //if (reader.Read())
                //{
                //    return MapReaderToPickle(reader);
                //}

                //return null;
            }
        }
        public List<Pickle> GetAll()
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                //give all Pickle from query
                return db.Query<Pickle>("select * from pickle").ToList();
            }

            ////return _pickles;           

            ////connection string
            //// how to connect to database
            ////var connectionString = "Server=localhost;Database=PickleAndHope;Trusted_Connection=True;";

            ////sql connection - need to tell to open up in the beginning and close it at the end
            ////actual connection
            //var connection = new SqlConnection(ConnectionString);
            //connection.Open();

            ////sql command
            ////createCommand goes down SqlConnection from connection
            ////tell sql what to do on how to connect
            //var cmd = connection.CreateCommand();

            ////query to run against connection to run against connectionString
            //cmd.CommandText = "select * from pickle";

            ////sql data reader
            ////run query and give results back
            ////executing sql syntax from line commandText
            //var reader = cmd.ExecuteReader();

            //var pickles = new List<Pickle>();

            ////Read returns boolean and will keep reading until there are no more rows, which will return false
            ////the current row that it reads, it becomes avilable in the reader            
            //while (reader.Read())
            //{
            //    //casting - take one value and saying it's a different kind of value
            //    //var id = (int)reader["Id"];
            //    //var type = (string)reader["Type"];

            //    var pickle = MapReaderToPickle(reader);
            //    pickles.Add(pickle);
            //}

            //connection.Close();

            //return pickles;
        }
        public Pickle GetById(int id)
        {
            // return the first pickle.Id that matches id
            //return _pickles.FirstOrDefault(pickle => pickle.Id == id);

            var query = @"select *
                        from Pickle
                        where Type = @id";

            using (var db = new SqlConnection(ConnectionString))
            {
                //dapper only map parameter names to properties of real class to match what is in query in where clause
                //new creates anonymous type of Id class with property name of id. Only exists here at this line
                var pickle = db.QueryFirstOrDefault<Pickle>(query, new { Id = id});
                return pickle;
                //connection.Open();

                //var cmd = connection.CreateCommand();

                ////set command up with query necessary

                //cmd.CommandText = query;
                //cmd.Parameters.AddWithValue("id", id);

                //var reader = cmd.ExecuteReader();

                ////map results to object c# will understand
                //if (reader.Read())
                //{
                //    return MapReaderToPickle(reader);
                //}
                //return null;
            }
        }

        Pickle MapReaderToPickle(SqlDataReader reader)
        {
            //taking sql query and map to c# object
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
