using PickleAndHope.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PickleAndHope.DataAccess
{
    // repository has collection-like methods. Layer that handles accessing data
    // anything that is data storage should live here
    public class PickleRepository
    {
        // static means field should be shared by every instance of pickles controller for long erm data storage
        static List<Pickle> _pickles = new List<Pickle>() { new Pickle {Id = 1, Type = "Bread and Butter", NumberInStock = 5 } };
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
            return _pickles.FirstOrDefault(p => p.Type == type);
        }
        public List<Pickle> GetAll()
        {
            return _pickles;
        }
        public Pickle GetById(int id)
        {
            // return the first pickle.Id that matches id
            return _pickles.FirstOrDefault(pickle => pickle.Id == id);
        }
    }
}
