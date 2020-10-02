using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.IO;
using FlightControl.Model;
using FlightControlWeb.Model;

namespace FlightsControlWeb.Model
{

    public class DataBaseServer : IDataBaseServer
	{
		SQLiteDataAdapter serverDataAdapter = null;
		DataTable serverDataTable = null;
		DataSet serverDataSet = null;

        /// <summary>
        ///  DataBaseServer constrctor. create data base or open him if exists.
        ///  create table with data base.
        ///  if not seccsed, close pergram.
        /// </summary>
        public DataBaseServer()
		{
			string createServersTable = @"CREATE TABLE IF NOT EXISTS [Servers] (
                [ServerId] VARCHAR(10) NOT NULL PRIMARY KEY,
                [ServerURL] TEXT(500) NOT NULL
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
                com.CommandText = createServersTable;
                com.ExecuteNonQuery();
                this.serverDataSet = new DataSet();
                this.serverDataTable = new DataTable("Servers");
                SQLiteCommand cmdThree = new SQLiteCommand("Select * from Servers", con);
                this.serverDataAdapter = new SQLiteDataAdapter(cmdThree);
                //connect is close
                this.serverDataAdapter.Fill(serverDataTable);
                this.serverDataSet.Tables.Add(serverDataTable);
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
        /// <summary>
        ///  AddServer add this server to data base. if missing information, throw exception.
        /// </summary>
        /// <param name="server"> server object </param>
        public void AddServer(Server server)
        {
            SQLiteCommandBuilder builder = new SQLiteCommandBuilder(this.serverDataAdapter);
            DataRow row = serverDataSet.Tables["Servers"].NewRow();
            row["ServerId"] = server.ServerId;
            row["ServerURL"] = server.ServerURL;
            this.serverDataSet.Tables["Servers"].Rows.Add(row);
            // update the data base.
            builder.GetInsertCommand();
            try { this.serverDataAdapter.Update(serverDataTable); }
            catch
            {
                // if missing inforamtion
                row.Delete();
                throw new IDataBaseServer.ErrorMissingInformationObject();
            }
        }
        /// <summary>
        ///  DeleteServer search id server in data base. if find, delete him.
        ///  else throw exception.
        /// </summary>
        /// <param name="id"> of server </param>
        public void DeleteServer(string id)
        {
            SQLiteCommandBuilder builder = new SQLiteCommandBuilder(this.serverDataAdapter);
            string expressionIdToFind = "ServerId = '" + id + "'";
            DataRow serverIdRow;
            try
            {
                serverIdRow = this.serverDataTable.Select(expressionIdToFind).First();
            }
            catch
            {
                throw new IDataBaseServer.ErrorIdNotExists();
            }
            serverIdRow.Delete();
            // update data base.
            builder.GetInsertCommand();
            this.serverDataAdapter.Update(serverDataTable);
            this.serverDataTable.AcceptChanges();
        }
        /// <summary>
        /// GetServers run on data base and return list of all server.
        /// </summary>
        /// <returns>  list of all server </returns>
        public IEnumerable<Server> GetServers()
        {
            List<Server> servers = new List<Server>();
            foreach (DataRow row in serverDataTable.Rows)
            {
                Server tempServer = new Server();
                tempServer.ServerId = Convert.ToString(row["ServerId"]);
                tempServer.ServerURL = Convert.ToString(row["ServerURL"]);
                servers.Add(tempServer);
            }
            return servers;
        }

    }
}