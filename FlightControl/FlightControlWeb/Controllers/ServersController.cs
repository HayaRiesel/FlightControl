using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightsControlWeb.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightsControlWeb.Controllers
{
    /// <summary>
    /// ServersController manger asking about server.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ServersController : ControllerBase
    {
        private IServerModel manger;
        /// <summary>
        /// controler constctor. create controler with server manger. 
        /// </summary>
        /// <param name="manger"></param>
        public ServersController(IServerModel serverModel)
        {
            this.manger = serverModel;
        }

        /// <summary>
        /// get return server list of all server.
        /// GET: api/Server.
        /// </summary>
        /// <returns> ok and server list </returns>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(manger.GetServers());
        }

        /// <summary>
        /// post add new server to program.
        /// POST: api/Server.
        /// </summary>
        /// <param name="server"> server to add </param>
        /// <returns> ok or not found if have problem </returns>
        [HttpPost]
        public IActionResult Post([FromBody]Server server)
        {
            try
            {
                manger.AddServer(server);
                return Ok();
            }
            catch
            {
                return NotFound();
            }

        }
        /// <summary>
        /// delete server accurding id.
        /// DELETE: api/ApiWithActions/5.
        /// </summary>
        /// <param name="id"> of server to delete </param>
        /// <returns> ok or not found if have problem </returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                manger.DeleteServer(id);
                return Ok();
            }
            catch
            {
                return NotFound();
            }

        }
    }
}

