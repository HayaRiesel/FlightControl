using FlightsControlWeb.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Model
{
    /// <summary>
    /// IDataBaseServer interface of server data base.
    /// </summary>
	public interface IDataBaseServer
    {
        /// <summary>
        /// Exception class of server id not exists in program. 
        /// </summary>
        public class ErrorIdNotExists : Exception
        {
            public string message => "Not exists server with this id.";
        }
        /// <summary>
        ///  Exception class of server missing importent infotmation.
        /// </summary>
        public class ErrorMissingInformationObject : Exception
        {
            public string message => "Missing information to server. check the json file.";
        }
        /// <summary>
        /// GetServers run on data base and return list of all server. 
        /// </summary>
        /// <returns>  list of all server </returns>
        public IEnumerable<Server> GetServers();
        /// <summary>
        ///  DeleteServer search id server in data base. if find, delete him.
        ///  else throw exception.
        /// </summary>
        /// <param name="id"> of server </param>
        public void DeleteServer(string id);
        /// <summary>
        ///  AddServer add this server to data base. if missing information, throw exception.
        /// </summary>
        /// <param name="server"> server object </param>
        public void AddServer(Server server);
    }
}
