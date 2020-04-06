using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PickleAndHope.Models
{
    // model class just to track data
    public class Pickle
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int NumberInStock { get; set; }
        public decimal Price { get; set; }
        public string Size { get; set; }
    }
}
