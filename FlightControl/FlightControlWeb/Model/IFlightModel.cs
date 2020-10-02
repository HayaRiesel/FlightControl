using FlightControl.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace FlightsControlWeb.Model
{
	/// <summary>
	/// IFlightModel interface of manger the flight.
	/// </summary>
	public interface IFlightModel
	{
		/// <summary>
		/// get flight list of this program that fly in time. 
		/// run on all flight plan and check if them flight in time.
		/// if yes, calculate flight location in this time.
		/// </summary>
		/// <param name="dataTime"> time of flights </param>
		/// <returns> list of flight in time </returns>
		IEnumerable<Flight> GetFlight(string dataTime);
		/// <summary>
		/// get flight list of all servers and this that fly in time.
		/// run on all servers and asking them flight in this time.
		/// add this program current flight list.
		/// </summary>
		/// <param name="dataTimeString"> time of flights </param>
		/// <returns> list of all flight, of this and server.</returns>
		public IEnumerable<Flight> GetSyncFlight(string dataTimeString);
		/// <summary>
		/// delete flight plan from this id flight.
		/// </summary>
		/// <param name="id"> id flight </param>
		public void DeleteFlight(string id);
	}

}
