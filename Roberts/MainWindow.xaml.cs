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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Roberts
{

    class Default
    {
        public static readonly Vector3D SCALE = new Vector3D(0.25, 0.25, 0.25);
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WriteableBitmap writeableBitmap;
        private MyObject m_currentObject;
        private Drawer drawer;
        private bool m_cutFaces = false;
        private IDictionary<string, MyObject> m_objectsMap = new Dictionary<string, MyObject>();

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
            var defaultObject = new MyObject("default", new Vector3D(0, 0, 0), new Vector3D(0, 0, 0), Default.SCALE, Shape.Tetrahedron);
            AddObject(defaultObject);
            objectsListBox.SelectedIndex = 0;
            //m_objectsMap.Add(defaultObject.Name, defaultObject);
            //tethraeder.SaveToFile(@"C:\Programs\mesh.txt");
            var r = -1.0 / 15.0;
            var perspective = new double[,]
            {
                { 1, 0, 0, 0 },
                { 0, 1, 0, 0 },
                { 0, 0, 0, r },
                { 0, 0, 0, 1 }
            };
            var projection = new MyMatrix<double>(perspective);

            m_currentObject = GetCurrentObject();
            drawer = new Drawer(projection, (int)writeableBitmap.Width, (int)writeableBitmap.Height);
            //drawer.Draw(writeableBitmap, tethraeder, m_cutFaces);
            Redraw();
        }

        private void ClearImage()
        {
            DrawAlgorithm.ResetColor(Colors.Black, writeableBitmap);
        }

        private void Redraw()
        {
            ClearImage();
            foreach (var pair in m_objectsMap)
            {
                drawer.Draw(writeableBitmap, pair.Value, m_cutFaces);
            }
        }

        private void AddObject()
        {
            var name = objectNameTextBox.Text;
            AddObject(new MyObject(name, new Vector3D(0, 0, 0), new Vector3D(0, 0, 0), Default.SCALE, GetSelectedShape()));
        }

        private void AddObject(MyObject obj)
        {
            m_objectsMap.Add(obj.Name, obj);
            var item = new ListBoxItem();
            item.Content = obj.Name;
            objectsListBox.Items.Add(item);
        }

        public MyObject GetCurrentObject()
        {
            var lbi = objectsListBox.SelectedItem as ListBoxItem;
            var name = lbi.Content.ToString();
            MyObject result;
            m_objectsMap.TryGetValue(name, out result);
            return result;
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
            m_currentObject.Position = new Vector3D(xyz[0], xyz[1], xyz[2]);
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
            m_currentObject.Rotation = new Vector3D(xyz[0], xyz[1], xyz[2]);
            //m_currentObject.SetRotation(TransformFactory.CreateOxRotation(xyz[0]));
            //m_currentObject.AddRotation(TransformFactory.CreateOyRotation(xyz[1]));
            //m_currentObject.AddRotation(TransformFactory.CreateOzRotation(xyz[2]));
            Redraw();
        }

        private double[] GetRotationSliderValues()
        {
            return new double[] { xRotationSlider.Value, yRotationSlider.Value, zRotationSlider.Value };
        }

        private Shape GetSelectedShape()
        {
            var shape = (shapeComboBox.SelectedItem as ComboBoxItem).Content.ToString();
            switch(shape)
            {
                case "Tetrahedron":
                return Shape.Tetrahedron;
                case "Hexahedron":
                return Shape.Hexahedron;
                case "Octahedron":
                return Shape.Octahedron;
                case "Dodecahedron":
                return Shape.Dodecahedron;
                case "Icosahedron":
                return Shape.Icosahedron;
                case "Sphere":
                return Shape.Sphere;
                case "Sphere without pole":
                return Shape.SphereWithoutPole;

                default:
                throw new ArgumentException("No such shape type: " + shape);
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            m_cutFaces = !m_cutFaces;
            Redraw();
        }

        private void addObjectButton_Click(object sender, RoutedEventArgs e)
        {
            var name = objectNameTextBox.Text;
            if ( !m_objectsMap.ContainsKey(name) )
            {
                AddObject();
                //m_objectsMap.Add(name, new MyObject(name, new Vector3D(0, 0, 0), new Vector3D(0, 0, 0), Default.SCALE, Shape.Sphere));
            }
            Redraw();
        }

        private void objectsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            m_currentObject = GetCurrentObject();
            RestoreControlsForCurrentObject();
        }

        private void RestoreControlsForCurrentObject()
        {
            xTranslationSlider.ValueChanged -= xTranslationSlider_ValueChanged;
            yTranslationSlider.ValueChanged -= yTranslationSlider_ValueChanged;
            zTranslationSlider.ValueChanged -= zTranslationSlider_ValueChanged;
            xTranslationSlider.Value = m_currentObject.Position.X;
            yTranslationSlider.Value = m_currentObject.Position.Y;
            zTranslationSlider.Value = m_currentObject.Position.Z;
            xTranslationSlider.ValueChanged += xTranslationSlider_ValueChanged;
            yTranslationSlider.ValueChanged += yTranslationSlider_ValueChanged;
            zTranslationSlider.ValueChanged += zTranslationSlider_ValueChanged;

            xRotationSlider.Value = m_currentObject.Rotation.X;
            yRotationSlider.Value = m_currentObject.Rotation.Y;
            zRotationSlider.Value = m_currentObject.Rotation.Z;
        }
    }
};
