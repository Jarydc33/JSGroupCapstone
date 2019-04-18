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
    public class Geolocation
    {
        string _lat;
        string _long;
        public string Latitude { get { return _lat; } }
        public string Longitude { get { return _long; } }
        HttpClient client = new HttpClient();
        public string Retrieve()
        {
            string Key = Keys.GoogleKey;//GoogleAPIKEY
            string url = "https://www.googleapis.com/geolocation/v1/geolocate?key=" + Keys.GoogleKey;
            WebRequest requestObject = WebRequest.Create(url);
            requestObject.ContentLength = 0;
            requestObject.Method = "POST";
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
            GeoLocationObj geo = JsonConvert.DeserializeObject<GeoLocationObj>(urlResult);
            try
            {
                lat = geo.location.lat;
                longitutde = geo.location.lng;
            }
            catch
            {
                return null;
            }
            string coords = lat + "," + longitutde;
            return coords;
        }
    }
    public class GeoLocationObj
    {
        public Location location { get; set; }
    }

}
