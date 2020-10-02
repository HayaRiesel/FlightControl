using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FlightsControlWeb.Model
{
	/// <summary>
	/// server class. save information about server.
	/// </summary>
	public class Server
	{
		/// <summary>
		/// server id information.
		/// </summary>
		[JsonPropertyName("ServerId")]
		public string ServerId { get; set; }

		/// <summary>
		/// server url inforamtion.
		/// </summary>
		[JsonPropertyName("ServerURL")]
		public string ServerURL { get; set; }
	}
}
