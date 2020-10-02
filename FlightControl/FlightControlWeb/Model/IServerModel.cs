using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightsControlWeb.Model
{
	/// <summary>
	/// IServerModel interface of manger the server.
	/// </summary>
	public interface IServerModel
	{
		/// <summary>
		/// GetServers return list of all server in program.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Server> GetServers();
		/// <summary>
		/// AddServer add server to data base. throw error if not secssed.
		/// add http if not have.
		/// </summary>
		/// <param name="server"> to add </param>
		public void AddServer(Server server);

		/// <summary>
		/// DeleteServer try delete server accurding id. 
		/// throw error if not secssed.
		/// </summary>
		/// <param name="id"> of server </param>
		public void DeleteServer(string id);
	}
}