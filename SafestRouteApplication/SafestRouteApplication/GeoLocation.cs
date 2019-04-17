using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace SafestRouteApplication
{
    public class Geolocation
    {
        string _lat;
        string _long;
        public string Latitude { get { return _lat; } }
        public string Longitude { get { return _long; } }
        HttpClient client = new HttpClient();
        public void Retrieve(string address)
        {
            string baseaddress = "https://www.googleapis.com/geolocation/v1/geolocate?key=AIzaSyCkWL84dG2bkXEffwiI8MGLOJHzYWSSdWI";
            RunDataRetrieval(baseaddress).GetAwaiter().GetResult();
        }

        async Task RunDataRetrieval(string address)
        {
            client.BaseAddress = new Uri(address);
            try
            {
                GeoLocationObj jsonObj = await GetRequest(address, client).ConfigureAwait(false);
                _lat = jsonObj.results[0].geometry.location.lat;
                _long = jsonObj.results[0].geometry.location.lng;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        async Task<GeoLocationObj> GetRequest(string path, HttpClient client)
        {
            GeoLocationObj jsonObj = null;
            HttpResponseMessage response = await client.GetAsync(path).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                jsonObj = await response.Content.ReadAsAsync<GeoLocationObj>();

            }
            return jsonObj;
        }
    }
    class GeoLocationObj
    {
        public Results[] results { get; set; }
    }

}