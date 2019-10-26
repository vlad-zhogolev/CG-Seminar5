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
            for (var i = 0; i < mesh.Faces.Height; ++i)
            {
                for (var j = 0; j < mesh.Faces.Width; ++j)
                {
                    var x1 = screenCoordinates[mesh.Faces[i, j], 0];
                    var y1 = screenCoordinates[mesh.Faces[i, j], 1];
                    var x2 = screenCoordinates[mesh.Faces[i, (j + 1) % mesh.Faces.Width], 0];
                    var y2 = screenCoordinates[mesh.Faces[i, (j + 1) % mesh.Faces.Width], 1];
                    DrawAlgorithm.DrawLine(bitmap, Colors.Blue, x1, y1, x2, y2);
                }
            }
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
                result[i, 1] = (int)(halfHeight * projectedVertices[i, 1] + halfHeight);
            }
            return result;
        }
    }
}
