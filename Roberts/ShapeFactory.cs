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
        public static Mesh CreateShape(Shape shape, double radius)
        {
            switch (shape)
            {
                case Shape.Tetrahedron:
                    return CreateTethraedron(radius);
                default:
                    throw new ArgumentException("Can't create shape of type: " + shape);
            }
        }

        private static Mesh CreateTethraedron(double r)
        {
            var vertices = new MyMatrix<double>(4, 4);
            var a = 4 * r / Math.Sqrt(6);
            var c = (a / 2) / Math.Cos(Utilities.ToRadians(30));
            for (var i = 0; i < 3; ++i)
            {
                vertices[i, 0] = c * (Math.Cos(Utilities.ToRadians(150 + i * 120)));
                vertices[i, 1] = -Math.Sqrt(r * r - c * c);
                vertices[i, 2] = c * (Math.Sin(Utilities.ToRadians(-30 + i * 120)));
            }
            vertices[3, 0] = 0;
            vertices[3, 1] = r;
            vertices[3, 2] = 0;
            for (var i = 0; i < vertices.Height; ++i)
            {
                vertices[i, 3] = 1;
            }
            var faces = new MyMatrix<int>(new int[,]
            {
                {1, 0, 3},
                {2, 1, 3},
                {0, 2, 3},
                {2, 0, 1}
            });
            return new Mesh(faces, vertices);
        }
    }
}
