using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace SafestRouteApplication
{
    public class GeoCode
    {
        static HttpClient client = new HttpClient();
        public string Retrieve(string address)
        {
            string formattedAddress = address.Replace(' ', '+');
            string Key = Keys.GoogleKey;//GoogleAPIKEY
            string url = "https://maps.googleapis.com/maps/api/geocode/json?address=" + formattedAddress + "&key=" + Key;
            WebRequest requestObject = WebRequest.Create(url);
            requestObject.Method = "GET";
            HttpWebResponse responseObject = null;
            responseObject = (HttpWebResponse)requestObject.GetResponse();
            string urlResult = null;
            using (Stream stream = responseObject.GetResponseStream())
            {
                StreamReader sr = new StreamReader(stream);
                urlResult = sr.ReadToEnd();
                sr.Close();
            }
            string lat;
            string longitutde; 
            GeoCodeObj geo = JsonConvert.DeserializeObject<GeoCodeObj>(urlResult);
            try
            {
                lat = geo.results[0].geometry.location.lat;
                longitutde = geo.results[0].geometry.location.lng;
            }
            catch
            {
                return null;
            }
            string coords = lat+","+longitutde;
            return coords;
        }






        //public string Retrieve(string address)
        //{
        //    string formattedAddress = address.Replace(' ', '+');
        //    string Key = Keys.GoogleKey;//GoogleAPIKEY
        //    string baseaddress = "https://maps.googleapis.com/maps/api/geocode/json?address="+formattedAddress+"&key="+Key;
        //    var retrievedString = RunDataRetrieval(baseaddress);//Need to solidify for synchronous use. both start and end trying to use same static class. 
        //    return (retrievedString.GetAwaiter());
        //}

        //async Task<string> RunDataRetrieval(string address)
        //{
        //    client.BaseAddress = new Uri(address);
        //    try
        //    {
        //        GeoCodeObj jsonObj = await GetRequest(address, client).ConfigureAwait(false);
        //        string _lat = jsonObj.results[0].geometry.location.lat;
        //        string _long = jsonObj.results[0].geometry.location.lng;
        //        return (_lat + "," + _long);
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //        return "";
        //    }

        //}
        //async Task<GeoCodeObj> GetRequest(string path, HttpClient client)
        //{
        //    GeoCodeObj jsonObj = null;
        //    HttpResponseMessage response = await client.GetAsync(path).ConfigureAwait(false);
        //    if (response.IsSuccessStatusCode)
        //    {
        //        jsonObj = await response.Content.ReadAsAsync<GeoCodeObj>();

        //    }
        //    return jsonObj;
        //}
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