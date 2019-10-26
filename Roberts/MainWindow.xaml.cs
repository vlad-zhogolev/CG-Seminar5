using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Roberts
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var a = new MyMatrix<double>(new double[,]
            {
                {1.0, 0.0},
                {0.0, 2.0}
            });
            var b = new MyMatrix<double>(new double[,]
            {
                {1.0, 2.0},
                {2.0, 3.0}
            });
            var result = a * b;
            result = MyMatrix<double>.Incident(3);
        }
    }
}
