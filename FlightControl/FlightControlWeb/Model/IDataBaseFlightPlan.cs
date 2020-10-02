using FlightControl.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Model
{
    /// <summary>
    /// IDataBaseFlightPlan interface of server data base.
    /// </summary>
	public interface IDataBaseFlightPlan
	{
        /// <summary>
        /// Error if the DB cancel in open
        /// </summary>
        public class ErrorInOpenDataBase : Exception { };
        /// <summary>
        /// Exception class of flightPlan id not exists in program. 
        /// </summary>
        public class ErrorIdNotExists : Exception
        {
            public string message => "Not exists fly plan with this id.";
        }
        /// <summary>
        /// Exception class of flighePlan missing importent infotmation.
        /// </summary>
        public class ErrorMissingInformationObject : Exception
        {
            public string message => "Missing information to flight plan. check the json file.";
        }
        /// <summary>
        /// hold the list of the Id
        /// </summary>
        /// <returns></returns>
        public HashSet<string> LoadDbID();
        /// <summary>
        /// AddFlightPlan go to the data base and add a new flight Plan 
        /// </summary>
        /// <param name="flightPlan"></param>
        public void AddFlightPlan(FlightPlan flightPlan);
        /// <summary>
        ///  GetFlightPlan run on data base and return the requir flight Plan by Id. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FlightPlan GetFlightPlan(string id);
        /// <summary>
        /// DeleteFlightPlan run on data base and delete the requir flight Plan by Id.
        /// </summary>
        /// <param name="id"></param>
        public void DeleteFlightPlan(string id);
    }
}
