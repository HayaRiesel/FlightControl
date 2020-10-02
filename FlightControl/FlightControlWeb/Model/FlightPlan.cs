using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace FlightControl.Model
{
    /// <summary>
    /// class that hold the data og flightPlan
    /// </summary>
    [Serializable]
    public partial class FlightPlan
    {
        /// <summary>
        /// hold the passenger number 
        /// </summary>
        [JsonProperty("passengers")]
        [JsonPropertyName("passengers")]
        public int Passengers { get; set; }
        /// <summary>
        ///  hold the company name 
        /// </summary>
        [JsonProperty("company_name")]
        [JsonPropertyName("company_name")]
        public string Company_name { get; set; }
        /// <summary>
        ///  hold the initial location
        /// </summary>
        [JsonProperty("initial_location")]
        [JsonPropertyName("initial_location")]
        public Initial_Location Initial_Location { get ; set ; }

        /// <summary>
        ///  hold the segmants
        /// </summary>
        [JsonProperty("segments")]
        [JsonPropertyName("segments")]
        public Segment[] Segments { get; set; }
        /// <summary>
        /// hold the flight id
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        public string Flight_id { get; set; }
        /// <summary>
        /// hold the end flight time, we calculate it
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        public DateTime End_Flight_Time { get; set; }
    }
    /// <summary>
    /// in the initial location thare are few paramters
    /// </summary>
    [Serializable]
    public partial class Initial_Location
    {
        /// <summary>
        /// hold the longitude
        /// </summary>
        [JsonProperty("longitude")]
        [JsonPropertyName("longitude")]
        public double Initial_Longitude { get; set; }
        /// <summary>
        /// hold the latitude
        /// </summary>
        [JsonProperty("latitude")]
        [JsonPropertyName("latitude")]
        public double Initial_Latitude { get; set; }
        /// <summary>
        /// hold the data time
        /// </summary>
        [JsonProperty("date_time")]
        [JsonPropertyName("date_time")]
        public DateTime Initial_Date_time { set; get; }
    }
    /// <summary>
    /// the segment have few paramaters, hold every part of them
    /// </summary>
    [Serializable]
    public partial class Segment
    {
        [JsonProperty("longitude")]
        [JsonPropertyName("longitude")]
        public double Segments_Longitude { get; set; }

        [JsonProperty("latitude")]
        [JsonPropertyName("latitude")]
        public double Segments_Latitude { get; set; }

        [JsonProperty("timespan_seconds")]
        [JsonPropertyName("timespan_seconds")]
        public Double Time_Span_Second { get; set; }
    }
}
