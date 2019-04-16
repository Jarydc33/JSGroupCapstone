using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace SafestRouteApplication
{
    public static class GeoCode
    {
        static string _lat;
        static string _long;
        static HttpClient client = new HttpClient();
        public static string Retrieve(string address)
        {
            string formattedAddress = address.Replace(' ', '+');
            string Key = Keys.GoogleKey;//GoogleAPIKEY
            string baseaddress = "https://maps.googleapis.com/maps/api/geocode/json?address="+formattedAddress+"&key="+Key;
            RunDataRetrieval(baseaddress).GetAwaiter().GetResult();//Need to solidify for synchronous use. both start and end trying to use same static class. 
            return (_lat + "," + _long);
        }

        static async Task RunDataRetrieval(string address)
        {
            client.BaseAddress = new Uri(address);
            try
            {
                GeoCodeObj jsonObj = await GetRequest(address, client).ConfigureAwait(false);
                _lat = jsonObj.results[0].geometry.location.lat;
                _long = jsonObj.results[0].geometry.location.lng;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return;
        }
        static async Task<GeoCodeObj> GetRequest(string path, HttpClient client)
        {
            GeoCodeObj jsonObj = null;
            HttpResponseMessage response = await client.GetAsync(path).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                jsonObj = await response.Content.ReadAsAsync<GeoCodeObj>();

            }
            return jsonObj;
        }
    }
    public class GeoCodeObj
    {
        public Results[] results { get; set; }
    }
    public class Results
    {
        public Geometry geometry { get; set; }
    }
    public class Geometry
    {
        public Location location { get; set; }
    }
    public class Location
    {
        public string lat { get; set; }
        public string lng { get; set; }
    }
}