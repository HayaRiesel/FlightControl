using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightControl.Model;
using FlightsControlWeb.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightsControlWeb.Controllers
{
    /// <summary>
    /// FlightPlanController manger asking about flight plan.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FlightPlanController : ControllerBase
    {
        private IFlightPlanModel manger;
        /// <summary>
        /// controler constctor. create controler with flight plan manger. 
        /// </summary>
        /// <param name="manger"></param>
        public FlightPlanController(IFlightPlanModel manger)
        {
            this.manger = manger;
        }
        /// <summary>
        /// get flight plan from the manager accurding id in asking.
        /// return error 404 if have problem.
        /// GET: api/FlightPlan/5.
        /// </summary>
        /// <param name="id"> id plan to return</param>
        /// <returns>IActionResult with ok and flight plan or not found error.</returns>
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(string id)
        {
            try 
            {
                return Ok(manger.GetFlightPlan(id)); 
            }
            catch(Exception e) 
            {
                return NotFound(e.Message); 
            }
        }

        /// <summary>
        /// add the flight plan to the program.
        /// POST: api/FlightPlan.
        /// </summary>
        /// <param name="flightPlan"> to add </param>
        /// <returns> IActionResult ok or not found if have error </returns>
        [HttpPost]
        public IActionResult Post([FromBody]FlightPlan flightPlan)
        {
            try
            {
                this.manger.AddFlightPlan(flightPlan);
                return Ok();
            }
            catch(Exception e)
            {
                return NotFound(e.Message);
            }
        }

        /// <summary>
        /// delete flight plan with this id. 
        /// DELETE: api/ApiWithActions/5.
        /// </summary>
        /// <param name="id"> of flight plan </param>
        /// <returns> IActionResult return ok or 404 error if have problem. </returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                this.manger.DeleteFlightPlan(id);
                return Ok();
            }
            catch(Exception e)
            {
                return NotFound(e.Message);
            }
        }
    }
}
