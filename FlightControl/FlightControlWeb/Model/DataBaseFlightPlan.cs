using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.IO;
using FlightControl.Model;
using Microsoft.AspNetCore.Mvc;
using FlightControlWeb.Model;
using System.Diagnostics;

namespace FlightsControlWeb.Model
{

    public class DataBaseFlightPlan : IDataBaseFlightPlan
    {
        DataSet planDataSet = null;
        SQLiteDataAdapter planDataAdapter = null;
        DataTable planDataTable = null;
        DataSet segmantsDataSet = null;
        SQLiteDataAdapter segmantsDataAdapter = null;
        DataTable segmantsDataTable = null;
        public static int ErrorObject { get; set; }
        public DataBaseFlightPlan()
        {
            string createTablePlanQuery = @"CREATE TABLE IF NOT EXISTS [FlightPlanTable] (
                [Flight_id] VARCHAR(10) NOT NULL PRIMARY KEY,
                [Passengers] INT(255) NOT NULL,
                [Initial_Longitude] Double NOT NULL,
                [Initial_Latitude] Double NOT NULL,
                [Initial_Date_time] VARCHAR(2048) NOT NULL,
                [End_Flight_Time] VARCHAR(2048) NOT NULL,
                [Company_name] TEXT(500) NOT NULL
            )";

            string createSegmantsTable = @"CREATE TABLE IF NOT EXISTS [Segmants] (
                [id] VARCHAR(100)  NOT NULL PRIMARY KEY,
                [Flight_id] VARCHAR(10) NOT NULL,
                [Time_Span_Second] VARCHAR(2048) NOT NULL,
                [Segments_Longitude] Double NOT NULL,
                [Segments_Latitude] Double NOT NULL
            )";

            if (!File.Exists("FlightPlanTable.db3"))
            {
                SQLiteConnection.CreateFile("FlightPlanTable.db3");
            }
            SQLiteConnection con = null;
            try
            {
                con = new SQLiteConnection("Data source=FlightPlanTable.db3");
                SQLiteCommand com = new SQLiteCommand(con);
                con.Open();
                com.CommandText = createTablePlanQuery;
                com.ExecuteNonQuery();
                com.CommandText = createSegmantsTable;
                com.ExecuteNonQuery();
                this.planDataSet = new DataSet();
                this.segmantsDataSet = new DataSet();
                this.planDataTable = new DataTable("FlightPlanTable");
                this.segmantsDataTable = new DataTable("Segmants");
                SQLiteCommand cmd = new SQLiteCommand("Select * from FlightPlanTable", con);
                this.planDataAdapter = new SQLiteDataAdapter(cmd);
                //connect is close
                this.planDataAdapter.Fill(planDataTable);
                this.planDataSet.Tables.Add(planDataTable);
                SQLiteCommand cmdTwo = new SQLiteCommand("Select * from Segmants", con);
                this.segmantsDataAdapter = new SQLiteDataAdapter(cmdTwo); 
                //connect is close
                this.segmantsDataAdapter.Fill(segmantsDataTable);
                this.segmantsDataSet.Tables.Add(segmantsDataTable);
                con.Close();
            }
            catch
            {
                if (con != null && (con.State == ConnectionState.Open))
                {
                    con.Close();
                }
                Environment.Exit(-1);
            }
        }
        public HashSet<string> LoadDbID()
        {
            HashSet<string> idSet = new HashSet<string>();
            foreach (DataRow row in this.planDataTable.Rows)
            {
                idSet.Add(Convert.ToString(row["Flight_id"]));
            }
            return idSet;
        }
        public void AddFlightPlan(FlightPlan flightPlan)
        {    
            SQLiteCommandBuilder builder = new SQLiteCommandBuilder(this.planDataAdapter);
            DataRow row = planDataSet.Tables["FlightPlanTable"].NewRow();
            row["Flight_id"] = flightPlan.Flight_id;
            row["Passengers"] = flightPlan.Passengers;
            row["Initial_Longitude"] = flightPlan.Initial_Location.Initial_Longitude;
            row["Initial_Latitude"] = flightPlan.Initial_Location.Initial_Latitude;
            row["Initial_Date_time"] = flightPlan.Initial_Location.Initial_Date_time.ToString("yyyy-MM-ddTHH:mm:ssZ");
            row["End_Flight_Time"] = flightPlan.End_Flight_Time.ToString("yyyy-MM-ddTHH:mm:ssZ");
            row["Company_name"] = flightPlan.Company_name;
            this.planDataSet.Tables["FlightPlanTable"].Rows.Add(row);
            builder.GetInsertCommand();
            try { this.planDataAdapter.Update(planDataTable); }
            catch { throw new IDataBaseFlightPlan.ErrorMissingInformationObject(); }
            try { AddFlightPlanSegmants(flightPlan);}
            catch(Exception e) {
                row.Delete();
                throw e;
            }
        }
        public void AddFlightPlanSegmants(FlightPlan flightPlan)
        {
            SQLiteCommandBuilder builderSeg = new SQLiteCommandBuilder(this.segmantsDataAdapter);
            int numSeg = flightPlan.Segments.Length;
            for (int i = 0; i < numSeg; i++)
            {
                DataRow segRow = segmantsDataSet.Tables["Segmants"].NewRow();
                segRow["id"] = flightPlan.Flight_id + i.ToString(); 
                segRow["Flight_id"] = flightPlan.Flight_id;
                segRow["Segments_Longitude"] = flightPlan.Segments[i].Segments_Longitude;
                segRow["Segments_Latitude"] = flightPlan.Segments[i].Segments_Latitude;
                segRow["Time_Span_Second"] = flightPlan.Segments[i].Time_Span_Second;
                this.segmantsDataSet.Tables["Segmants"].Rows.Add(segRow);
                builderSeg.GetInsertCommand();
                try { this.segmantsDataAdapter.Update(segmantsDataTable); }
                catch
                {
                    segRow.Delete();
                    throw new IDataBaseFlightPlan.ErrorMissingInformationObject();
                }
            }

        }
        public FlightPlan GetFlightPlan(string id)
        {
            DataRow[] flightIdRowArr;
            string expressionIdToFind = "Flight_id = '" + id + "'";
            try
            {
                flightIdRowArr = this.planDataTable.Select(expressionIdToFind);
            }
            catch 
            {
                throw new IDataBaseFlightPlan.ErrorIdNotExists();
            }
            DataRow flightIdRow = flightIdRowArr.First();
            FlightPlan idPlan = new FlightPlan();
            idPlan.Flight_id = id;
            idPlan.Initial_Location = new Initial_Location();
            idPlan.Initial_Location.Initial_Date_time = TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(flightIdRow["Initial_Date_time"]));
            idPlan.Initial_Location.Initial_Latitude = Convert.ToDouble(flightIdRow["Initial_Latitude"]);
            idPlan.Initial_Location.Initial_Longitude = Convert.ToDouble(flightIdRow["Initial_Longitude"]);
            idPlan.Passengers = Convert.ToInt32(flightIdRow["Passengers"]);
            idPlan.End_Flight_Time = TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(flightIdRow["End_Flight_Time"]));
            idPlan.Company_name = Convert.ToString(flightIdRow["Company_name"]);

            idPlan.Segments = GetFlightPlanSegment(expressionIdToFind);
            return idPlan;
        }
        public Segment[] GetFlightPlanSegment(string expressionIdToFind)
        {
            DataRow[] flightIdRowSegArr = this.segmantsDataTable.Select(expressionIdToFind);
            int size = flightIdRowSegArr.Length;
            Segment[] segmants = new Segment[size];
            for (int i = 0; i < size; i++)
            {
                segmants[i] = new Segment();
                segmants[i].Segments_Latitude = Convert.ToDouble(flightIdRowSegArr[i]["Segments_Latitude"]);
                segmants[i].Segments_Longitude = Convert.ToDouble(flightIdRowSegArr[i]["Segments_Longitude"]);
                segmants[i].Time_Span_Second = Convert.ToDouble(flightIdRowSegArr[i]["Time_Span_Second"]);
            }
            return segmants;
        }
        public void DeleteFlightPlan(string id)
        {
            SQLiteCommandBuilder builder = new SQLiteCommandBuilder(this.planDataAdapter);
            string expressionIdToFind = "Flight_id = '" + id + "'";
            DataRow flightIdRow;
            try
            {
               flightIdRow = this.planDataTable.Select(expressionIdToFind).First();
            }
            catch
            {
                throw new IDataBaseFlightPlan.ErrorIdNotExists();
            }
            flightIdRow.Delete();
            builder.GetInsertCommand();
            this.planDataAdapter.Update(planDataTable);
            this.planDataTable.AcceptChanges();

            SQLiteCommandBuilder builderSeg = new SQLiteCommandBuilder(this.segmantsDataAdapter);
            DataRow[]flightIdRowSeg = this.segmantsDataTable.Select(expressionIdToFind);
            int size = flightIdRowSeg.Length;
            for(int i = 0; i < size; i++)
            {
                flightIdRowSeg[i].Delete();
            }
            builderSeg.GetInsertCommand();
            this.segmantsDataAdapter.Update(segmantsDataTable);
            this.segmantsDataTable.AcceptChanges();
        }
    }
}