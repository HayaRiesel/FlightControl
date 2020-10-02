
using FlightControl.Model;
using FlightsControlWeb.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FlightControlWebTests1.Model
{
    [TestClass]
    public class ClassFlightTest
    {
        /// <summary>
        /// we want to check the quality of the flight mechanism of delete
        /// </summary>
        [TestMethod]
        public void FlightModelDeleteTest()
        {
            //we make the variables
            Mock<IFlightPlanModel> mockFlightPlanModel = new Mock<IFlightPlanModel>();
            FlightModel flightModel = new FlightModel(mockFlightPlanModel.Object, null);
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            //add 
            keyValuePairs.Add("1", "test");
            mockFlightPlanModel.Setup(x => x.DictIdPlanAndServer).Returns(keyValuePairs);
            mockFlightPlanModel.Setup(x => x.DeleteFlightPlan("1"));
            flightModel.DeleteFlight("1");
            //check that this id delete
            Assert.IsFalse(mockFlightPlanModel.Object.DictIdPlanAndServer.ContainsKey("1"));
            //check that we get to the desired function
            mockFlightPlanModel.Verify();
        }
        /// <summary>
        /// we update few flights and check if everry think make sure
        /// </summary>
        [TestMethod]
        public void FlightModelGetFlightTest()
        {
            //make the mock
            Mock<IFlightPlanModel> mockFlightPlanModel = new Mock<IFlightPlanModel>();
            FlightModel flightModel = new FlightModel(mockFlightPlanModel.Object, null);
            // update the hashset
            HashSet<string> set = new HashSet<string> { "1", "2", "3" };
            // change the return value to get the Information needed
            mockFlightPlanModel.Setup(x => x.IdFlightSet).Returns(set);
            FlightPlan flightPlan = new FlightPlan();
            mockFlightPlanModel.Setup(x => x.GetFlightPlan("1")).Returns(initFlightPlan("1"));
            mockFlightPlanModel.Setup(x => x.GetFlightPlan("2")).Returns(initFlightPlan("2"));
            mockFlightPlanModel.Setup(x => x.GetFlightPlan("3")).Returns(initFlightPlan("3"));
            IEnumerable<Flight> flights = flightModel.GetFlight("2020-06-02T12:43:35Z");
            // check if the return value is the value that we expected to get
            Assert.AreEqual(2, flights.ToList().Count);
        }
        /// <summary>
        /// init the flight from the file
        /// we use this func few times so we need a number to get information from diffrent file
        /// </summary>
        /// <param name="num"></param>  the name of the file
        /// <returns></returns>
        public static FlightPlan initFlightPlan(string num)
        {
            using (FileStream fs = File.OpenRead("FlightPlanTest" + num + ".json"))
            {
                //get from the file the indication
                FlightPlan flightPlan = System.Text.Json.JsonSerializer.DeserializeAsync<FlightPlan>(fs).Result;
                DateTime endTime = flightPlan.Initial_Location.Initial_Date_time;
                /// update the end time
                for (int i = 0; i < flightPlan.Segments.Length; i++)
                {
                    endTime = endTime.AddSeconds(flightPlan.Segments[i].Time_Span_Second);
                }
                flightPlan.End_Flight_Time = endTime;
                //return the flight
                return flightPlan;
            }
        }
    }
}
