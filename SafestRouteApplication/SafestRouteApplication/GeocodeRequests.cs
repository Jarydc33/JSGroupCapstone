using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace SafestRouteApplication
{ 
    //Can stay in backend since it is single requests upon the input of initial directions, user models will need to keep track of their own route information
    //this class essentially just provides the initial lat/long for user on first request for a route. 
    public class GeocodeRequests
    {
        string _lat;
        string _long;
        public string GeocodeLat { get { return _lat; } }
        public string GeocodeLong { get { return _long; } }
        HttpClient client = new HttpClient();

        public void Retrieve(string address)
        {
            string baseaddress = "https://maps.googleapis.com/maps/api/geocode/json?address=" + address.Replace(' ', '+') + "&key="; //Needs Key
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
            HttpResponseMessage response = await client.GetAsync(path).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                jsonObj = await response.Content.ReadAsAsync<RequestObj>();

            }
            return jsonObj;
        }

    }
    
}