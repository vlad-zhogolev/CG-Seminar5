using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roberts
{
    class Mesh
    {
        private MyMatrix<int> m_faces = null;
        private MyMatrix<double> m_vertices = null;
        private MyMatrix<double> m_translation = MyMatrix<double>.Incident(4);
        private MyMatrix<double> m_rotation = MyMatrix<double>.Incident(4);
        private MyMatrix<double> m_scale = MyMatrix<double>.Incident(4);

        public Mesh(MyMatrix<int> faces, MyMatrix<double> vertices)
        {
            if (faces == null || vertices == null)
            {
                throw new ArgumentException(
                    "Faces are " + (faces == null ? "" : "not") + " null, " +
                    "vertices are: " + (faces == null ? "" : "not") + " null"
                );
            }
            if (faces.Height <= 0 || faces.Width != 3)
            {
                throw new ArgumentException(
                    "Wrong faces matrix size, height = " + faces.Height +
                    ", width = " + faces.Width
                );
            }
            if (vertices.Height < 3 || vertices.Width != 4)
            {
                throw new ArgumentException(
                    "Wrong vertices matrix size, height = " + vertices.Height +
                    ", width = " + vertices.Width
                );
            }
            m_faces = faces;
            m_vertices = vertices;
        }

        MyMatrix<double> GetWorldCoordinates()
        {
            return m_vertices * m_translation * m_rotation * m_scale;
        }
    }
}
