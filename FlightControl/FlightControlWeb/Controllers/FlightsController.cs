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
    /// FlightController manger asking about flight.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private IFlightModel manger;
        /// <summary>
        /// controler constctor. create controler with flight manger. 
        /// </summary>
        /// <param name="manger"></param>
        public FlightsController(IFlightModel flightModel)
        {
            this.manger = flightModel;
        }

        /// <summary>
        /// get return list of flight that fly in asking time. 
        /// accurding the asking, return flight of this program if asking is only time,
        /// or flight of this program and all server that he have if asking with sync_all.
        /// Get : /api/Flights?relative_to=<DATE_TIME>&sync_all.
        /// </summary>
        /// <param name="relative"> input time </param>
        /// <returns> list of all the flight in this time </returns>
        [HttpGet]
        public IActionResult Get([FromQuery(Name = "relative_to")] string relative)
        {
            string input = Request.QueryString.Value;
            if (input.Contains("sync_all"))
            {
                string stringNewDataTime = relative.Substring(0, relative.IndexOf("&"));
                return Ok(this.manger.GetSyncFlight(stringNewDataTime));
            }
            else
            {
                return Ok(this.manger.GetFlight(relative));
            }
        }

        /// <summary>
        /// delete Flight (delete here flight plan) from program accurding id flight.
        /// DELETE: api/Flights/5.
        /// </summary>
        /// <param name="id"> of flight to delete</param>
        /// <returns> ok or nut found if hae problem </returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                manger.DeleteFlight(id);
                return Ok();
            }
            catch(Exception e)
            {
                return NotFound(e.Message);
            }
        }
    }
}
