using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roberts
{
    class Face
    {
        public Face(IList<int> indices)
        {
            if (indices == null)
            {
                throw new ArgumentException("Can't create face, indices parameter is null");
            }
            if (indices.Count < 3)
            {
                throw new ArgumentException("Face can't contain less than 3 vertices");
            }
            foreach (var index in indices)
            {
                if (index < 0)
                {
                    throw new ArgumentException("Index of vertex can't be less than zero");
                }
            }
            m_indices = indices;
        }

        public IList<int> Indices { get { return m_indices; } }

        private IList<int> m_indices = null;
    }

    class Mesh
    {
        private MyMatrix<int> m_faces = null;
        private MyMatrix<double> m_vertices = null;
        private MyMatrix<double> m_translation = MyMatrix<double>.Incident(4);
        private MyMatrix<double> m_rotation = MyMatrix<double>.Incident(4);
        private MyMatrix<double> m_scale = MyMatrix<double>.Incident(4);

        public MyMatrix<int> Faces { get { return m_faces; } }

        public Mesh(IList<Face> faces, MyMatrix<double> vertices)
        {
            CheckNullFacesOrVertices(faces, vertices);
            
        }

        public Mesh(MyMatrix<int> faces, MyMatrix<double> vertices)
        {
            CheckNullFacesOrVertices(faces, vertices);
            if (faces.Height <= 0 || faces.Width < 3)
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

        public void SetTranslation(MyMatrix<double> translation) { m_translation = translation; }

        public MyMatrix<double> GetWorldCoordinates()
        {
            return m_vertices * m_translation * m_rotation * m_scale;
        }

        private void CheckNullFacesOrVertices(Object faces, Object vertices)
        {
            if (faces == null || vertices == null)
            {
                throw new ArgumentException(
                    "Faces are " + (faces == null ? "" : "not") + " null, " +
                    "vertices are: " + (faces == null ? "" : "not") + " null"
                );
            }
        }
    }
}
