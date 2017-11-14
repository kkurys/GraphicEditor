using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphicEditor
{
    /// <summary>
    /// Interaction logic for BezierCurveWindow.xaml
    /// </summary>
    public partial class BezierCurveWindow : Window
    {
        private int _level = 5;
        public int Level
        {
            get
            {
                return _level;
            }
            set
            {
                _level = value;
                GenerateBezierPoints();
                DrawScreen();
            }
        }
        public Point[] TangentPoints { get; set; }
        public Ellipse[] TangentPointsEllipses { get; set; }
        public List<int> LevelOptions { get; set; } = new List<int>()
        {
            3,
            4,
            5,
            6,
            7,
            8,
            9,
            10
        };
        public int MovedElementId { get; set; } = -1;
        public BezierCurveWindow()
        {
            InitializeComponent();
            DataContext = this;

            Canvas.MouseDown += Canvas_MouseDown;
            Canvas.MouseMove += Canvas_MouseMove;
            Canvas.MouseUp += Canvas_MouseUp;
            Canvas.LostMouseCapture += Canvas_LostMouseCapture;
            Canvas.MouseRightButtonDown += Canvas_MouseRightButtonDown;
            GenerateBezierPoints();
            DrawScreen();
        }

        private void Canvas_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MovedElementId = -1;
        }

        private void Canvas_LostMouseCapture(object sender, System.Windows.Input.MouseEventArgs e)
        {
            MovedElementId = -1;
        }

        private void Canvas_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MovedElementId = -1;
        }

        private void Canvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (MovedElementId != -1)
            {
                var pos = e.GetPosition(Canvas);
                TangentPoints[MovedElementId].X = pos.X;
                TangentPoints[MovedElementId].Y = pos.Y;
                DrawScreen();
            }
        }
        private void Canvas_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.Source is Ellipse)
            {
                var el = e.Source as Ellipse;
                MovedElementId = int.Parse(el.Name.Substring(1));
            }
        }

        private void DrawScreen()
        {
            Canvas.Children.Clear();
            for (int i = 0; i < Level; i++)
            {
                Canvas.Children.Add(TangentPointsEllipses[i]);
                Canvas.SetTop(TangentPointsEllipses[i], TangentPoints[i].Y - 5);
                Canvas.SetLeft(TangentPointsEllipses[i], TangentPoints[i].X - 5);
            }
            var b = GetBezierApproximation(TangentPoints, 256);
            PathFigure pf = new PathFigure(b.Points[0], new[] { b }, false);
            PathFigureCollection pfc = new PathFigureCollection();
            pfc.Add(pf);
            var pge = new PathGeometry();
            pge.Figures = pfc;
            Path p = new Path();
            p.Data = pge;
            p.Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            p.StrokeThickness = 3;
            Canvas.Children.Add(p);

        }
        private void GenerateBezierPoints()
        {
            TangentPoints = new Point[Level];
            TangentPointsEllipses = new Ellipse[Level];
            for (int i = 0; i < Level; i++)
            {
                TangentPoints[i] = new Point(i * 100 + 20, i % 2 == 0 ? 200 : 20);
                TangentPointsEllipses[i] = new Ellipse()
                {
                    Fill = new SolidColorBrush(i == 0 || i == Level - 1 ? Colors.Black : Colors.Blue),
                    Height = 10,
                    Width = 10,
                    Name = "E" + i.ToString()
                };
            }
        }
        PolyLineSegment GetBezierApproximation(Point[] controlPoints, int outputSegmentCount)
        {
            Point[] points = new Point[outputSegmentCount + 1];
            for (int i = 0; i <= outputSegmentCount; i++)
            {
                double t = (double)i / outputSegmentCount;
                points[i] = GetBezierPoint(t, controlPoints, 0, controlPoints.Length);
            }
            return new PolyLineSegment(points, true);
        }

        Point GetBezierPoint(double t, Point[] controlPoints, int index, int count)
        {
            if (count == 1)
                return controlPoints[index];
            var P0 = GetBezierPoint(t, controlPoints, index, count - 1);
            var P1 = GetBezierPoint(t, controlPoints, index + 1, count - 1);
            return new Point((1 - t) * P0.X + t * P1.X, (1 - t) * P0.Y + t * P1.Y);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Level = int.Parse(e.AddedItems[0].ToString());
        }
    }
}
