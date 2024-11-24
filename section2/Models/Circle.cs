using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace section2.Models
{
    public class Circle(int id, double x, double y)
    {
        public int Id { get; } = id;
        public double X { get; } = x;
        public double Y { get; } = y;
    }
}
