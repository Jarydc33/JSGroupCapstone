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
            string Key = "";//GoogleAPIKEY
            string baseaddress = "https://maps.googleapis.com/maps/api/geocode/json?address="+formattedAddress+"&key="+Key;
            RunDataRetrieval(baseaddress).GetAwaiter().GetResult();
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
    class GeoCodeObj
    {
        public Results[] results { get; set; }
    }
    class Results
    {
        public Geometry geometry { get; set; }
    }
    class Geometry
    {
        public Location location { get; set; }
    }
    class Location
    {
        public string lat { get; set; }
        public string lng { get; set; }
    }
}