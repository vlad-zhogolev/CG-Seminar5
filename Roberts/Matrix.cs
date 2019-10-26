using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roberts
{
    class Matrix<T>
    {
        private T[,] m_matrix = null;

        public T this [int i, int j]
        {
            get { return m_matrix[i, j]; }
            set { m_matrix[i, j] = value; }
        }
    }
}
