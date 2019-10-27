using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roberts
{
    enum Shape
    {
        Tetrahedron,
        Hexahedron,
        Octahedron,
        Dodecahedron,
        Icosahedron
    }

    class ShapeFactory
    {
        public static Mesh CreateShape(Shape shape)
        {
            switch(shape)
            {
                case Shape.Tetrahedron:
                    return CreateTethraedron();
                default:
                    throw new ArgumentException("Can't create shape of type: " + shape);
            }
        }

        private static Mesh CreateTethraedron()
        {

        }
    }
}
