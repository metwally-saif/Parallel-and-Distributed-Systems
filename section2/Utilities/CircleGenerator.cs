using section2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace section2.Utilities
{
    public static class CircleGenerator
    {
        public static List<Circle> GenerateRandomCircles(int numberOfCircles, double fieldSize)
        {
            var circles = new List<Circle>();
            var rand = new Random();
            for (int i = 0; i < numberOfCircles; i++)
            {
                double x = rand.NextDouble() * fieldSize;
                double y = rand.NextDouble() * fieldSize;
                circles.Add(new Circle(i, x, y));
            }
            return circles;
        }
    }
}
