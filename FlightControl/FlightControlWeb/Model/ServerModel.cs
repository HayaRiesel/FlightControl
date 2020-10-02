using FlightControlWeb.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FlightsControlWeb.Model
{
	/// <summary>
	/// ServerModel manger the server.
	/// </summary>
	public class ServerModel : IServerModel
	{
		private IDataBaseServer dataBaseServer;
		/// <summary>
		/// ServerModel constctor. create with server data base. 
		/// </summary>
		/// <param name="dataBaseServer"> data base</param>
		public ServerModel(IDataBaseServer dataBaseServer)
		{
			this.dataBaseServer = dataBaseServer;
		}
		/// <summary>
		/// AddServer add server to data base. throw error if not secssed.
		/// add http if not have.
		/// </summary>
		/// <param name="server"> to add </param>
		public void AddServer(Server server)
		{
			if (!server.ServerURL.Contains("http://"))
			{
				server.ServerURL = "http://" + server.ServerURL;
			}
			try
			{
				this.dataBaseServer.AddServer(server);
			}
			catch (Exception e)
			{
				throw e;
			}

		}
		/// <summary>
		/// DeleteServer try delete server accurding id. 
		/// throw error if not secssed.
		/// </summary>
		/// <param name="id"> of server </param>
		public void DeleteServer(string id)
		{
			try
			{
				dataBaseServer.DeleteServer(id);
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		/// <summary>
		/// GetServers return list of all server in program.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Server> GetServers()
		{
			return dataBaseServer.GetServers();
		}
	}
}


