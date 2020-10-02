using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace FlightControl.Model
{
    /// <summary>
    /// Flight save information about flight.
    /// </summary>
    public class Flight
    {
        /// <summary>
        /// id flight. same him flight plan.
        /// </summary>
        [JsonPropertyName("flight_id")]
        public string Flight_id { get; set;  }
        /// <summary>
        /// passengers number.
        /// </summary>
        [JsonPropertyName("passengers")]
        public int Passengers { get; set; }
        /// <summary>
        /// location longitude.
        /// </summary>
        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }
        /// <summary>
        /// location latitude.
        /// </summary>
        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }
        /// <summary>
        /// company name.
        /// </summary>
        [JsonPropertyName("company_name")]
        public string Company_name { get; set; }
        /// <summary>
        /// time of flight.
        /// </summary>
        [JsonPropertyName("date_time")]
        public DateTime Date_time { get; set; }
        /// <summary>
        /// true if this flight is extrnal.
        /// </summary>
        [JsonPropertyName("is_external")]
        public bool Is_external { get; set; }
    }
}