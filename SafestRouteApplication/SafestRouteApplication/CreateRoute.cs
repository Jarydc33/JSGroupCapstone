using SafestRouteApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SafestRouteApplication
{
    public static class CreateRoute
    {
        static Route route;
        static HttpClient client = new HttpClient();
        public static Route Retrieve(string start_address, string end_address, List<string> avoidances)
        {
            string startCoordinates = GeoCode.Retrieve(start_address);
            string endCoordinates = GeoCode.Retrieve(end_address);
            string avoidanceCoords = "";
            foreach(string x in avoidances)
            {
                avoidanceCoords += avoidanceCoords + ";";
            }
            avoidanceCoords = avoidanceCoords.Remove(avoidanceCoords.Length - 1);
            string appId = Keys.HEREAppID;//HERE api ID
            string appCode = Keys.HEREAppCode;//HERE api Code
            string baseaddress = "https://route.api.here.com/routing/7.2/calculateroute.json?app_id="+appId+"&app_code="+appCode+"&waypoint0="+startCoordinates+"&waypoint1="+endCoordinates+"&mode=fastest;car;traffic:disabled&avoidareas="+avoidanceCoords;
            RunDataRetrieval(baseaddress).GetAwaiter().GetResult();
            return route;
        }
        static async Task RunDataRetrieval(string address)
        {
            client.BaseAddress = new Uri(address);
            try
            {
                RequestObj jsonObj = await GetRequest(address, client).ConfigureAwait(false);
                route = jsonObj.response.route[0];
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        static async Task<RequestObj> GetRequest(string path, HttpClient client)
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
    public class MetaInfo
    {
        public DateTime timestamp { get; set; }
        public string mapVersion { get; set; }
        public string moduleVersion { get; set; }
        public string interfaceVersion { get; set; }
        public List<string> availableMapVersion { get; set; }
    }

    public class MappedPosition
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class OriginalPosition
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class Waypoint
    {
        public string linkId { get; set; }
        public MappedPosition mappedPosition { get; set; }
        public OriginalPosition originalPosition { get; set; }
        public string type { get; set; }
        public double spot { get; set; }
        public string sideOfStreet { get; set; }
        public string mappedRoadName { get; set; }
        public string label { get; set; }
        public int shapeIndex { get; set; }
    }

    public class Mode
    {
        public string type { get; set; }
        public List<string> transportModes { get; set; }
        public string trafficMode { get; set; }
        public List<object> feature { get; set; }
    }

    public class MappedPosition2
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class OriginalPosition2
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class Start
    {
        public string linkId { get; set; }
        public MappedPosition2 mappedPosition { get; set; }
        public OriginalPosition2 originalPosition { get; set; }
        public string type { get; set; }
        public double spot { get; set; }
        public string sideOfStreet { get; set; }
        public string mappedRoadName { get; set; }
        public string label { get; set; }
        public int shapeIndex { get; set; }
    }

    public class MappedPosition3
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class OriginalPosition3
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class End
    {
        public string linkId { get; set; }
        public MappedPosition3 mappedPosition { get; set; }
        public OriginalPosition3 originalPosition { get; set; }
        public string type { get; set; }
        public double spot { get; set; }
        public string sideOfStreet { get; set; }
        public string mappedRoadName { get; set; }
        public string label { get; set; }
        public int shapeIndex { get; set; }
    }

    public class Position
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class Maneuver
    {
        public Position position { get; set; }
        public string instruction { get; set; }
        public int travelTime { get; set; }
        public int length { get; set; }
        public string id { get; set; }
        public string _type { get; set; }
    }

    public class Leg
    {
        public Start start { get; set; }
        public End end { get; set; }
        public int length { get; set; }
        public int travelTime { get; set; }
        public List<Maneuver> maneuver { get; set; }
    }

    public class Summary
    {
        public int distance { get; set; }
        public int trafficTime { get; set; }
        public int baseTime { get; set; }
        public List<string> flags { get; set; }
        public string text { get; set; }
        public int travelTime { get; set; }
        public string _type { get; set; }
    }

    public class Route
    {
        public List<Waypoint> waypoint { get; set; }
        public Mode mode { get; set; }
        public List<Leg> leg { get; set; }
        public Summary summary { get; set; }
        public string request { get; set; }
    }

    public class Response
    {
        public MetaInfo metaInfo { get; set; }
        public List<Route> route { get; set; }
        public string language { get; set; }
    }

    public class RequestObj
    {
        public Response response { get; set; }
    }

}