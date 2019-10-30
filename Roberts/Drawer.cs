using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Roberts
{
    class Drawer
    {
        private MyMatrix<double> m_projection;
        private int m_screenWidth;
        private int m_screenHeight;
        
        public Drawer(MyMatrix<double> projection, int width, int height)
        {
            m_projection = projection;
            m_screenWidth = width;
            m_screenHeight = height;
        }

        public void Draw(WriteableBitmap bitmap, Mesh mesh)
        {
            var projectedVertices = Project(mesh.GetWorldCoordinates());
            var screenCoordinates = CalculateScreenCoordinates(projectedVertices);
            //var faces = mesh.GetVisibleFaces(0, 0, 15);
            var faces = mesh.Faces;
            for (var i = 0; i < faces.Count; ++i)
            {
                for (var j = 0; j < faces[i].Indices.Count; ++j)
                {
                    var x1 = screenCoordinates[faces[i].Indices[j], 0];
                    var y1 = screenCoordinates[faces[i].Indices[j], 1];
                    var x2 = screenCoordinates[faces[i].Indices[(j + 1) % faces[i].Indices.Count], 0];
                    var y2 = screenCoordinates[faces[i].Indices[(j + 1) % faces[i].Indices.Count], 1];
                    DrawAlgorithm.DrawLine(bitmap, Colors.Blue, x1, y1, x2, y2);
                }
            }
            //for (var i = 0; i < mesh.Vertices.Height; ++i)
            //{
            //    DrawAlgorithm.SetPixelIfPossible(screenCoordinates[i, 0], screenCoordinates[i, 1], Colors.White, bitmap);
            //}
        }

        private MyMatrix<double> Project(MyMatrix<double> vertices)
        {
            var result = vertices * m_projection;
            for (var i = 0; i < result.Height; ++i)
            {
                result[i, 0] /= result[i, 3];
                result[i, 1] /= result[i, 3];
                result[i, 3] = 1.0;
            }
            return result;
        }

        private MyMatrix<int> CalculateScreenCoordinates(MyMatrix<double> projectedVertices)
        {
            var result = new MyMatrix<int>(projectedVertices.Height, 2);
            int halfWidth = m_screenWidth / 2;
            int halfHeight = m_screenHeight / 2;
            for (var i = 0; i < result.Height; ++i)
            {
                result[i, 0] = (int)(halfWidth * projectedVertices[i, 0] + halfWidth);
                result[i, 1] = (int)(-halfHeight * projectedVertices[i, 1] + halfHeight);
            }
            return result;
        }

        
    }
}
