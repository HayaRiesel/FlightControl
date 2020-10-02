using FlightControl.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FlightsControlWeb.Model
{
	/// <summary>
	/// FlightModel manger the flight.
	/// </summary>
	public class FlightModel : IFlightModel
	{
		IServerModel serverModel;
		IFlightPlanModel flightPlanModel;
		/// <summary>
		/// FlightModel constctor. create with server model and flight plan model. 
		/// </summary>
		/// <param name="flightPlanModel"> flight plan maneger </param>
		/// <param name="serverModel"> server manger </param>
		public FlightModel(IFlightPlanModel flightPlanModel, IServerModel serverModel)
		{
			this.flightPlanModel = flightPlanModel;
			this.serverModel = serverModel;
		}
		/// <summary>
		/// get flight list of this program that fly in time. 
		/// run on all flight plan and check if them flight in time.
		/// if yes, calculate flight location in this time.
		/// </summary>
		/// <param name="dataTimeString"> time of flights </param>
		/// <returns> list of flight in time </returns>
		public IEnumerable<Flight> GetFlight(string dataTimeString)
		{
			List<Flight> flightList = new List<Flight>();
			DateTime dataTime = TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(dataTimeString));
			HashSet<string> idOfFlightPlan = this.flightPlanModel.IdFlightSet;
			foreach (string planId in idOfFlightPlan)
			{
				FlightPlan plan = null;
				try { plan = this.flightPlanModel.GetFlightPlan(planId); }
				catch { continue; }
				// if flight isnt in input time range
				if (DateTime.Compare(plan.Initial_Location.Initial_Date_time, dataTime) > 0
					|| DateTime.Compare(plan.End_Flight_Time, dataTime) < 0)
				{
					continue;
				}
				// find the current flight of plan flight.
				Flight fly = FindCurrentSectionFlightForTime(dataTime, plan);
				if (fly != null)
				{
					flightList.Add(fly);
				}
			}
			return flightList;
		}
		/// <summary>
		/// get flight list of all servers and this that fly in time.
		/// run on all servers and asking them flight in this time.
		/// add this program current flight list.
		/// </summary>
		/// <param name="dataTimeString"> time of flights </param>
		/// <returns> list of all flight, of this and server.</returns>
		public IEnumerable<Flight> GetSyncFlight(string dataTimeString)
		{
			List<Flight> flightList = new List<Flight>();
			string asking = "/api/Flights?relative_to=" + dataTimeString;
			IEnumerable<Server> serverList = this.serverModel.GetServers();
			HttpClient httpClient = new HttpClient();
			foreach (Server server in serverList)
			{
				List<Flight> serverFlightList = null;
				try
				{
					// get list flight from server
					String UrlAsk = server.ServerURL + asking;
					var result = httpClient.GetStringAsync(UrlAsk).Result;
					string jsonFlight = result.ToString();
					serverFlightList = JsonConvert.DeserializeObject<List<Flight>>(jsonFlight);
				}
				catch
				{
					continue;
				}
				foreach(Flight flight in serverFlightList)
				{
					// update this is extrnal, and add to dict the id flight and url server.
					flight.Is_external = true;
					AddToDict(server, flight);
				}
				// add this list to bigger list.
				flightList.AddRange(serverFlightList);
			}
			return this.GetFlight(dataTimeString).Concat(flightList);
		}
		/// <summary>
		/// AddToDict add flight to dict with here server.
		/// </summary>
		/// <param name="server"> that send this flight </param>
		/// <param name="flight"> flight that get </param>
		public void AddToDict(Server server, Flight flight)
		{
			try
			{
				this.flightPlanModel.DictIdPlanAndServer.Add(flight.Flight_id, server.ServerURL);
			}
			catch { }
		}
		/// <summary>
		/// FindCurrentSectionFlightForTime run on segment in flight plan.
		/// if find the segment that in input time, return new flight for this segment. 
		/// if not find return null.
		/// </summary>
		/// <param name="dateTime"> time to check </param>
		/// <param name="plan"> plan flight to find him flight </param>
		/// <returns> flight in time </returns>
		public Flight FindCurrentSectionFlightForTime(DateTime dateTime, FlightPlan plan)
		{
			DateTime endTime = plan.Initial_Location.Initial_Date_time;
			int i = 0;
			for (; i < plan.Segments.Length; i++)
			{
				DateTime firstTime = endTime;
				endTime = endTime.AddSeconds(plan.Segments[i].Time_Span_Second);
				if (DateTime.Compare(endTime, dateTime) >= 0)
				{
					return CreateFlight(plan, dateTime, firstTime, i);
				}
			}
			return null;
		}
		/// <summary>
		/// CreateFlight create new flight in time.
		/// find location of flight i time, and updae auther inforamtion from plan flight.
		/// </summary>
		/// <param name="plan"> plan flight with information </param>
		/// <param name="dateTime"> time of flight </param>
		/// <param name="firstTime"> time begging the segment flight </param>
		/// <param name="i"> num segment in array </param>
		/// <returns> new flight in unput time </returns>
		public Flight CreateFlight(FlightPlan plan, DateTime dateTime, DateTime firstTime, int i)
		{
			double newLatitude = 0;
			double newLongitude = 0;
			double time = (dateTime - firstTime).TotalSeconds;
			// if this first segment, use begging location.
			if (i == 0)
			{
				newLatitude = CurrentPlaceInSection(plan.Initial_Location.Initial_Latitude,
					plan.Segments[i].Segments_Latitude, plan.Segments[i].Time_Span_Second, time);
				newLongitude = CurrentPlaceInSection(plan.Initial_Location.Initial_Longitude,
					plan.Segments[i].Segments_Longitude, plan.Segments[i].Time_Span_Second, time);
			}
			// else use befor segmeint location.
			else
			{
				newLatitude = CurrentPlaceInSection(plan.Segments[i - 1].Segments_Latitude,
					plan.Segments[i].Segments_Latitude, plan.Segments[i].Time_Span_Second, time);
				newLongitude = CurrentPlaceInSection(plan.Segments[i - 1].Segments_Longitude,
					plan.Segments[i].Segments_Longitude, plan.Segments[i].Time_Span_Second, time);
			}
			//create flight with flight plan information and location.
			return new Flight
			{
				Company_name = plan.Company_name,
				Date_time = dateTime,
				Flight_id = plan.Flight_id,
				Is_external = false,
				Passengers = plan.Passengers,
				Longitude = newLongitude,
				Latitude = newLatitude
			};
		}
		/// <summary>
		/// CurrentPlaceInSection calculate location flight.
		/// add to begging location relative  location to past time.
		/// </summary>
		/// <param name="beginLocation"> location begging this segment </param>
		/// <param name="endLocation"> end location this segment </param>
		/// <param name="timeSection"> time of segment </param>
		/// <param name="time"> that past from this segment </param>
		/// <returns> current location </returns>
		public double CurrentPlaceInSection(double beginLocation, double endLocation, double timeSection, double time)
		{
			// bagin local plus the change location in secend doul the secound that was.
			double RelativeDistanceOfTime = (endLocation - beginLocation) / timeSection;
			return beginLocation + (RelativeDistanceOfTime * time);
		}
		/// <summary>
		/// delete flight plan from this id flight.
		/// </summary>
		/// <param name="id"> id flight </param>
		public void DeleteFlight(string id)
		{
			try { this.flightPlanModel.DeleteFlightPlan(id); }
			catch(Exception e) { throw e; }
		}
	}
}
