using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace SafestRouteApplication
{
    //Need to send Gelocation request(POST) asynchronously to track individual user. (NO STATIC) 
    //Do it all in JScript not C# for better results since it needs to feed back into map, move all map functions into JS to be run on 
    //client side. 
    public class GeolocationRequest
    {
        string _lat;
        string _long;
        public string GeocodeLat { get { return _lat; } }
        public string GeocodeLong { get { return _long; } }
        HttpClient client;
        public GeolocationRequest()
        {
            client = new HttpClient();
        }

        public void Retrieve(string address)
        {
            string baseaddress = "https://www.googleapis.com/geolocation/v1/geolocate?key=";//Needs Key
            RunDataRetrieval(baseaddress).GetAwaiter().GetResult();
        }

        async Task RunDataRetrieval(string address)
        {
            client.BaseAddress = new Uri(address);
            try
            {
                RequestObj jsonObj = await GetRequest(address, client).ConfigureAwait(false);
                _lat = jsonObj.results[0].geometry.location.lat;
                _long = jsonObj.results[0].geometry.location.lng;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        async Task<RequestObj> GetRequest(string path, HttpClient client)
        {
            RequestObj jsonObj = null;
            HttpResponseMessage response = await client.PostAsync(path, new StringContent("")).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                jsonObj = await response.Content.ReadAsAsync<RequestObj>();

            }
            return jsonObj;
        }
    }
    class RequestObj
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