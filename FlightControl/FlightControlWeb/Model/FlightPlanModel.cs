using FlightControl.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Net.Http;
using Newtonsoft.Json;
using FlightControlWeb.Model;
using System.Threading;

// i helping in random for 
namespace FlightsControlWeb.Model
{
	/// <summary>
	///  FlightPlanModel manger the flight Plan
	/// </summary>
	public class FlightPlanModel : IFlightPlanModel
	{
		/// <summary>
		/// hold the id and the server name in dictionarry
		/// </summary>
		public Dictionary<string, string> dictIdPlanAndServer = new Dictionary<string, string>();
		/// <summary>
		/// save the id in hash to make sure that ID is primarly
		/// </summary>
		public HashSet<string> idFlightSet = new HashSet<string>();
		public HashSet<string> IdFlightSet {
			//return the hash
			get {
				HashSet<string> vs = new HashSet<string>();
				foreach(string id in idFlightSet)
				{
					vs.Add(id);
				}
				return vs;
			}
		}
		public Dictionary<string, string> DictIdPlanAndServer {
			get { return dictIdPlanAndServer; } 
		}
		/// <summary>
		/// hold the dataBase
		/// </summary>
		IDataBaseFlightPlan dataBessFlightPlan;
		public FlightPlanModel(IDataBaseFlightPlan dataBaseFlightPlan)
		{
			//build
			dataBessFlightPlan = dataBaseFlightPlan;
			idFlightSet = this.dataBessFlightPlan.LoadDbID();
		}
		/// <summary>
		/// add new flightPlan from the controler
		/// </summary>
		/// <param name="flightPlan"></param> the new flightPlan
		public void AddFlightPlan(FlightPlan flightPlan)
		{
			//add the random ID
			flightPlan.Flight_id = RandomAndSingleId();
			DateTime endTime = flightPlan.Initial_Location.Initial_Date_time;
			//move over the segmants to find the end time
			for (int i = 0; i < flightPlan.Segments.Length; i++)
			{
				endTime = endTime.AddSeconds(flightPlan.Segments[i].Time_Span_Second);
			}
			flightPlan.End_Flight_Time = endTime;
			try { this.dataBessFlightPlan.AddFlightPlan(flightPlan); }
			catch(Exception e) { throw e; }
		}
		/// <summary>
		/// return flightPlan by ID
		/// </summary>
		/// <param name="id"></param> the id of the requir flightPlan
		/// <returns></returns>
		public FlightPlan GetFlightPlan(string id)
		{
			//search in the dictionary
			if (DictIdPlanAndServer.ContainsKey(id))
			{
				//call the client
				HttpClient httpClient = new HttpClient();
				string asking = "/api/FlightPlan/" + id;
				try
				{
					string http = DictIdPlanAndServer[id] + asking;
					var result = httpClient.GetStringAsync(http).Result;
					string jsonFlighPlan = result.ToString();
					FlightPlan flightPlanAnser = JsonConvert.DeserializeObject<FlightPlan>(jsonFlighPlan);
					return flightPlanAnser;
				}
				catch(Exception e) { throw e; }
			}
			try { return this.dataBessFlightPlan.GetFlightPlan(id); }
			catch(Exception e) { throw e; }
		}
		/// <summary>
		///delete flightPlan by ID
		/// </summary>
		/// <param name="id"></param> the requir id's flightplan
		public void DeleteFlightPlan(string id)
		{
			//remove from the hashset
			idFlightSet.Remove(id);
			//delete from the data base
			try { this.dataBessFlightPlan.DeleteFlightPlan(id); }
			catch(Exception e) { throw e; }
		}
		/// <summary>
		/// make the id by random way
		/// </summary>
		/// <returns></returns>
		public string RandomAndSingleId()
		{
			string id = null;
			do
			{
				//make the id
				StringBuilder builderString = new StringBuilder();
				Random random = new Random();
				for (int i = 0; i < 10; i++)
				{
					builderString = addNextChar(i, builderString, random);
				}
				id = builderString.ToString();
			} while (idFlightSet.Contains(id));
			idFlightSet.Add(id);
			return id;
		}
		/// <summary>
		/// helper to the build of the ID
		/// </summary>
		/// <param name="i"></param>
		/// <param name="builderString"></param>
		/// <param name="random"></param>
		/// <returns></returns>
		public StringBuilder addNextChar(int i, StringBuilder builderString, Random random)
		{
			if (i % 3 == 0)
			{
				char nextChar = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
				return builderString.Append(nextChar);
			}
			int nextNum = random.Next(0, 9);
			return builderString.Append(nextNum);
		}
	}
}
