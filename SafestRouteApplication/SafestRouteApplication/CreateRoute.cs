using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLab1
{
    public class CreateRoute
    {
        List<Coordinate> _safePoints;
        List<Coordinate> _avoidPoints;
        List<Coordinate> _intermediates;
        Coordinate _currentLocation;
        Coordinate _nextLocation;
        Coordinate _startLocation;
        Coordinate _endLocation;
        public List<Coordinate> Intermediates { get { return _safePoints; } }
        public Coordinate Start { get { return _startLocation; } }
        public Coordinate End { get { return _endLocation; } }

        public CreateRoute(Coordinate startPoint, Coordinate endPoint, List<Coordinate> avoidPoints)
        {
            _startLocation = startPoint;
            _currentLocation = startPoint;
            _endLocation = endPoint;
            _avoidPoints = avoidPoints;
            PopulateIntermediates();
            DetermineSafePoints();
        }
        public CreateRoute(double startPointx, double startPointy, double endPointx, double endPointy)
        {
            _startLocation = new Coordinate(startPointx, startPointy);
            _currentLocation = new Coordinate(startPointx, startPointy);
            _endLocation = new Coordinate(endPointx, endPointy);
            PopulateIntermediates();
            DetermineSafePoints();
        }

        private void DetermineSafePoints()
        {
            double distance_Start_End = MathSupp.Distance(_currentLocation, _endLocation);
            if (!DetectCollisions(_currentLocation, _endLocation))
            {
                return;
            }
            double max = 100000000;
            Coordinate maxCoordinate = new Coordinate(0, 0);
            double interdistance;
            foreach (Coordinate x in _intermediates)
            {
                interdistance = MathSupp.Distance(_endLocation, x);
                if (interdistance < max && DetectCollisions(x, _currentLocation))
                {
                    max = interdistance;
                    maxCoordinate = x;
                }
            }
            _safePoints.Add(maxCoordinate);
            DetermineSafePoints();
        }
        private bool DetectCollisions(Coordinate coord1, Coordinate coord2)
        {
            double m = coord1.latitude - coord2.latitude / coord1.longitude - coord2.longitude;
            double b = coord2.latitude - (m * coord1.longitude);
            foreach (Coordinate x in _avoidPoints)
            {
                if ((m * x.longitude + b) < x.latitude + .00004 && (m * x.longitude + b) > x.latitude - .00004)
                {
                    return true;
                }
            }
            return false;
        }
        private void PopulateIntermediates()
        {
            _intermediates = new List<Coordinate>();
            for (int x = 0; x < _avoidPoints.Count; x++)
            {
                for (int y = 0; y < _avoidPoints.Count; y++)
                {
                    _intermediates.Add(MathSupp.MidPoint(_avoidPoints[x], _avoidPoints[y]));
                }
            }
        }
    }
    public static class MathSupp
    {
        public static Coordinate MidPoint(Coordinate coord1, Coordinate coord2)
        {
            Coordinate midpoint = new Coordinate();
            midpoint.longitude = (coord1.longitude + coord2.longitude) / 2;
            midpoint.latitude = (coord1.latitude + coord2.latitude) / 2; ;
            return midpoint;
        }
        public static double Distance(Coordinate coord1, Coordinate coord2)
        {
            double distance = Math.Pow(Math.Pow((coord2.longitude - coord1.longitude), 2) + Math.Pow((coord2.latitude - coord1.latitude), 2), .5);
            return Math.Abs(distance);
        }
    }
    public class Coordinate
    {
        public double latitude { get; set; }//y
        public double longitude { get; set; }//x
        public Coordinate()
        {

        }
        public Coordinate(double x, double y)
        {
            latitude = y;
            longitude = x;
        }
    }


}