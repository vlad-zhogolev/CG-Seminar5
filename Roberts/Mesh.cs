﻿using System;
using System.Collections.Generic;
using System.IO;
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

    class FaceBuilder
    {
        private IList<int> m_indices = new List<int>(0);
        
        public Face Build()
        {
            var result = new Roberts.Face(m_indices);
            m_indices = new List<int>(0); ;
            return result;
        }

        public void Add(int index)
        {
            m_indices.Add(index);
        }

        public void Add(int[] indices)
        {
            foreach (var index in indices)
            {
                m_indices.Add(index);
            }
        }
    }

    class Mesh
    {
        private IList<Face> m_faces = new List<Face>(0);
        private MyMatrix<double> m_vertices = null;
        private MyMatrix<double> m_translation = MyMatrix<double>.Incident(4);
        private MyMatrix<double> m_rotation = MyMatrix<double>.Incident(4);
        private MyMatrix<double> m_scale = MyMatrix<double>.Incident(4);

        public IList<Face> Faces { get { return m_faces; } }

        public MyMatrix<double> Vertices { get { return m_vertices; } }

        public Mesh(IList<Face> faces, MyMatrix<double> vertices)
        {
            CheckNullFacesOrVertices(faces, vertices);
            CheckVerticesShape(vertices);
            m_faces = faces;
            m_vertices = vertices;
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
            CheckVerticesShape(vertices);
            for (int i = 0; i < faces.Height; ++i)
            {
                var faceBuilder = new FaceBuilder();
                for (var j = 0; j < faces.Width; ++j)
                {
                    faceBuilder.Add(faces[i, j]);
                }
                m_faces.Add(faceBuilder.Build());
            }
            m_vertices = vertices;
        }

        public void SetTranslation(MyMatrix<double> translation) { m_translation = translation; }

        public void SetRotation(MyMatrix<double> rotation) { m_rotation = rotation; }

        public MyMatrix<double> GetWorldCoordinates()
        {
            return m_vertices * m_translation * m_rotation * m_scale;
        }

        public void SaveToFile(string path)
        {
            using ( System.IO.StreamWriter writer = new StreamWriter(path) )
            {
                for (var i = 0 ; i < m_vertices.Height ; ++i)
                {
                    writer.WriteLine("v " + m_vertices[i, 0] + " " + m_vertices[i, 1] + " " + m_vertices[i, 2]);
                }

                foreach(var face in Faces)
                {
                    string indices = "";
                    foreach (var index in face.Indices)
                    {
                        indices += index + " ";
                    }
                    writer.WriteLine("f " + indices);
                }
            }
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

        private void CheckVerticesShape(MyMatrix<double> vertices)
        {
            if (vertices.Height < 3 || vertices.Width != 4)
            {
                throw new ArgumentException(
                    "Wrong vertices matrix size, height = " + vertices.Height +
                    ", width = " + vertices.Width
                );
            }
        }
    }
}
