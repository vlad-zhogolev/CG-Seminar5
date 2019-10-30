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
        private IDictionary<string, MyObject> m_objects = new Dictionary<string, MyObject>();

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
            tethraeder = ShapeFactory.CreateShape(Shape.Sphere, 0.15);
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

        private void Redraw()
        {
            ClearImage();
            drawer.Draw(writeableBitmap, tethraeder, m_cutFaces);
        }

        private void xTranslationSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TranslationSliderValueChanged();
        }

        private void yTranslationSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TranslationSliderValueChanged();
        }

        private void zTranslationSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TranslationSliderValueChanged();
        }

        private void TranslationSliderValueChanged()
        {
            var xyz = GetTranslationSliderValues();
            var translation = TransformFactory.CreateTranslation(xyz[0], xyz[1], xyz[2]);
            tethraeder.SetTranslation(translation);
            Redraw();
        }

        private double[] GetTranslationSliderValues()
        {
            return new double[] { xTranslationSlider.Value, yTranslationSlider.Value, zTranslationSlider.Value };
        }

        private void xRotationSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            RotationSliderValueChanged();
        }

        private void yRotationSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            RotationSliderValueChanged();
        }

        private void zRotationSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            RotationSliderValueChanged();
        }

        private void RotationSliderValueChanged()
        {
            var xyz = GetRotationSliderValues();
            tethraeder.SetRotation(TransformFactory.CreateOxRotation(xyz[0]));
            tethraeder.AddRotation(TransformFactory.CreateOyRotation(xyz[1]));
            tethraeder.AddRotation(TransformFactory.CreateOzRotation(xyz[2]));
            Redraw();
        }

        private double[] GetRotationSliderValues()
        {
            return new double[] { xRotationSlider.Value, yRotationSlider.Value, zRotationSlider.Value };
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            m_cutFaces = !m_cutFaces;
            Redraw();
        }
    }
}
