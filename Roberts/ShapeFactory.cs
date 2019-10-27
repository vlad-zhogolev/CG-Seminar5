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
                case Shape.Hexahedron:
                    return CreateHexaedron(radius);
                case Shape.Octahedron:
                    return CreateOctahedron(radius);
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

        private static Mesh CreateHexaedron(double r)
        {
            var vertices = new MyMatrix<double>(8, 4);
            var a = 2 * r / Math.Sqrt(3);
            // x
            vertices[0, 0] = vertices[1, 0] = vertices[4, 0] = vertices[5, 0] = -a / 2;
            vertices[2, 0] = vertices[3, 0] = vertices[6, 0] = vertices[7, 0] = a / 2;
            // y
            vertices[0, 1] = vertices[1, 1] = vertices[2, 1] = vertices[3, 1] = a / 2;
            vertices[4, 1] = vertices[5, 1] = vertices[6, 1] = vertices[7, 1] = -a / 2;
            // z
            vertices[0, 2] = vertices[3, 2] = vertices[4, 2] = vertices[7, 2] = a / 2;
            vertices[1, 2] = vertices[2, 2] = vertices[5, 2] = vertices[6, 2] = -a / 2;

            for (var i = 0; i < vertices.Height; ++i)
            {
                vertices[i, 3] = 1;
            }

            var faces = new MyMatrix<int>(new int[,]
            {
                {0, 4, 5, 1 }, // left
                {1, 5, 6, 2 }, // back
                {2, 6, 7, 3 }, // right
                {3, 7, 4, 0 }, // front
                {4, 7, 6, 5 }, // up
                {0, 1, 2, 3 }, // bottom
            });
            return new Mesh(faces, vertices);
        }

        private static Mesh CreateOctahedron(double r)
        {
            var vertices = new MyMatrix<double>(6, 4);
            var a = 6 * r / Math.Sqrt(6);

            vertices[0, 0] = vertices[1, 0] = -a / 2;
            vertices[2, 0] = vertices[3, 0] = a / 2;
            vertices[4, 0] = vertices[5, 0] = 0;

            vertices[0, 1] = vertices[1, 1] = vertices[2, 1] = vertices[3, 1] = 0;
            vertices[4, 1] = r;
            vertices[5, 1] = -r;

            vertices[1, 2] = vertices[2, 2] = -a / 2;
            vertices[0, 2] = vertices[3, 2] = a / 2;
            vertices[4, 2] = vertices[5, 2] = 0;

            for (var i = 0; i < vertices.Height; ++i)
            {
                vertices[i, 3] = 1;
            }

            var faces = new MyMatrix<int>(new int[,]
            {
                { 1, 0, 4 },
                { 2, 1, 4 },
                { 3, 2, 4 },
                { 0, 3, 4 },
                { 0, 1, 5 },
                { 1, 2, 5 },
                { 2, 3, 5 },
                { 3, 0, 5 }
            });

            return new Mesh(faces, vertices);
        }
    }
}
