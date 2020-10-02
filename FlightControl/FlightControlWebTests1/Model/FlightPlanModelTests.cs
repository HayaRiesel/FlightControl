using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlightsControlWeb.Model;
using System;
using System.Collections.Generic;
using System.Text;
using FlightControl.Model;
using Moq;
using FlightControlWeb.Model;

namespace FlightControlWebTests1.Model
{
	[TestClass()]
	public class FlightPlanModelTests
	{
        /// <summary>
        /// Check that deleting flight planning works properly
        /// </summary>
        [TestMethod]
        public void FlightPlanModelDeleteTest()
        {
            //make the temp DB
            Mock<IDataBaseFlightPlan> mockDataBase = new Mock<IDataBaseFlightPlan>();
            HashSet<string> vs = new HashSet<string>();
            // change the return value to get the Information needed
            mockDataBase.Setup(x => x.LoadDbID()).Returns(vs);
            FlightPlanModel flightPlanModel = new FlightPlanModel(mockDataBase.Object);
            // add and delete
            flightPlanModel.idFlightSet.Add("1");
            mockDataBase.Setup(x => x.DeleteFlightPlan("1"));
            flightPlanModel.DeleteFlightPlan("1");
            Assert.IsFalse(flightPlanModel.idFlightSet.Contains("1"));
            //check that we get to the desired function
            mockDataBase.Verify();
        }
        /// <summary>
        ///  Check that adding flight planning works properly, with the members
        /// </summary>
        [TestMethod]
        public void FlightPlanModelAdd()
        {
            Mock<IDataBaseFlightPlan> mockDataBase = new Mock<IDataBaseFlightPlan>();
            // change the return value to get the Information needed
            mockDataBase.Setup(x => x.LoadDbID()).Returns(new HashSet<string>());
            FlightPlanModel flightPlanModel = new FlightPlanModel(mockDataBase.Object);
            FlightPlan flightPlan = ClassFlightTest.initFlightPlan("1");
            mockDataBase.Setup(x => x.AddFlightPlan(flightPlan));
            flightPlanModel.AddFlightPlan(flightPlan);
            // Checks if the id is created well
            Assert.IsTrue(flightPlan.Flight_id.Length > 0);
            DateTime time = TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime("2020-03-02T13:12:37Z"));
            //Checks if time is up to requir date
            Assert.AreEqual(time, flightPlan.End_Flight_Time);
            //check that we get to the desired function
            mockDataBase.Verify();
        }
        /// <summary>
        ///  Check that gettind flight planning works properly
        /// </summary>
        [TestMethod]
        public void FlightPlanModelGet()
        {
            Mock<IDataBaseFlightPlan> mockDataBase = new Mock<IDataBaseFlightPlan>();
            // change the return value to get the Information needed
            mockDataBase.Setup(x => x.LoadDbID()).Returns(new HashSet<string>());
            // change the return value to get the Information needed
            mockDataBase.Setup(x => x.LoadDbID()).Returns(new HashSet<string>());
            FlightPlanModel flightPlanModel = new FlightPlanModel(mockDataBase.Object);
            // Get a specific flight
            mockDataBase.Setup(x => x.GetFlightPlan("1"));
            flightPlanModel.GetFlightPlan("1");
            //check that we get to the desired function
            mockDataBase.Verify();
        }
    }
}