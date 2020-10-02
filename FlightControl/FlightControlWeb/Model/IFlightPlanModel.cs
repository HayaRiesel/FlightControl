using FlightControl.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightsControlWeb.Model
{
	/// <summary>
	/// IFlightPlanModel interface of manger the flightPlan.
	/// </summary>
	public interface IFlightPlanModel
	{
		public HashSet<string> IdFlightSet { get; }
		/// <summary>
		///  hold the dictionary of the ID and server
		/// </summary>
		public Dictionary<string, string> DictIdPlanAndServer { get; }
		public void AddFlightPlan(FlightPlan flightPlan);
		/// <summary>
		/// get the flightPlan by ID
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public FlightPlan GetFlightPlan(string id);
		/// <summary>
		/// delete flight plan from this id flight.
		/// </summary>
		/// <param name="id"></param> id oh the flightPlan
		public void DeleteFlightPlan(string id);
	}
}
