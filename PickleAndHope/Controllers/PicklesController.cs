using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PickleAndHope.DataAccess;
using PickleAndHope.Models;

namespace PickleAndHope.Controllers
{
    [Route("api/pickles")] // exposed at which endpoint
    [ApiController] // making it into http api
    public class PicklesController : ControllerBase // class is controller
    {       

        PickleRepository _repository = new PickleRepository();

        // api/pickles
        [HttpPost]
        // will add pickle object that has different properties
        public IActionResult AddPickle(Pickle pickleToAdd)
        {

            // Any method will return boolean if pickle type is already in list or not
            var existingPickle = _repository.GetByType(pickleToAdd.Type);
            // does pickle exists, if not, create it
            if (existingPickle == null)
            {
                _repository.Add(pickleToAdd);
                // will give 201 http response
                return Created("", pickleToAdd);
            }
            else // if pickle already exists, just update it
            {
                var updatedPickle = _repository.Update(pickleToAdd);
                return Ok(updatedPickle);
            }
        }

        [HttpGet]
        public IActionResult GetAllPickles()
        {
        var allPickles = _repository.GetAll();
            return Ok(allPickles);
        }

        // api/pickles/{id}
        [HttpGet("{id}")]
        public IActionResult GetPickleById(int id)
        {
            var pickle = _repository.GetById(id);
            if (pickle == null)
            {
                return NotFound("No pickle with that id can be found.");
            }
            return Ok(pickle);
        }
    }
}