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
        private WriteableBitmap writeableBitmap;
        private Mesh tethraeder;
        private Drawer drawer;
        private bool m_cutFaces = false;

        public MainWindow()
        {
            InitializeComponent();
            writeableBitmap = new WriteableBitmap((int)image.Width, (int)image.Height, 96, 96, PixelFormats.Bgr32, null);
            image.Source = writeableBitmap;
            image.Stretch = Stretch.None;
            image.HorizontalAlignment = HorizontalAlignment.Left;
            image.VerticalAlignment = VerticalAlignment.Top;

            var vertices = new double[,]
            {
                {0, 0, 0, 1},
                {1, 0, 0, 1},
                {0.5, -0.2, 1, 1},
                {0.5, 1, 0.5, 1}
            };

            var faces = new int[,]
            {
                {1, 0, 3},
                {2, 1, 3},
                {0, 2, 3},
                {2, 0, 1}
            };

            //tethraeder = new Mesh(new MyMatrix<int>(faces), new MyMatrix<double>(vertices));
            tethraeder = ShapeFactory.CreateShape(Shape.SphereWithoutPole, 0.5);
            tethraeder.SaveToFile(@"C:\Programs\mesh.txt");
            var r = -1.0 / 15.0;
            var perspective = new double[,]
            {
                { 1, 0, 0, 0 },
                { 0, 1, 0, 0 },
                { 0, 0, 0, r },
                { 0, 0, 0, 1 }
            };
            var projection = new MyMatrix<double>(perspective);

            drawer = new Drawer(projection, (int)writeableBitmap.Width, (int)writeableBitmap.Height);
            drawer.Draw(writeableBitmap, tethraeder, m_cutFaces);
        }

        private void ClearImage()
        {
            DrawAlgorithm.ResetColor(Colors.Black, writeableBitmap);
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //var translation = TransformFactory.CreateTranslation(0, slider.Value, 0);
            //tethraeder.SetTranslation(translation);
            var rotation = TransformFactory.CreateOxRotation(slider.Value * 90);
            tethraeder.SetRotation(rotation);
            ClearImage();
            drawer.Draw(writeableBitmap, tethraeder, m_cutFaces);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            m_cutFaces = !m_cutFaces;
            ClearImage();
            drawer.Draw(writeableBitmap, tethraeder, m_cutFaces);
        }
    }
}
