using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SensorialRhythm
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public class SpheroColor {
            public Color _main;
            public Color _inner;
            public Color _outter;

            public SpheroColor(Color color)
            {
                
                _main = Color.FromArgb(color.A, color.R, color.G, color.B);
                _inner = Color.FromArgb((byte)(Math.Max(color.A - 50,0)), (byte)(Math.Max(color.R - 50, 0)), (byte)(Math.Max(color.G - 50, 0)), (byte)(Math.Max(color.B - 50, 0)));
                _outter = Color.FromArgb((byte)(Math.Max(color.A - 100, 0)), (byte)(Math.Max(color.R - 100, 0)), (byte)(Math.Max(color.G - 100, 0)), (byte)(Math.Max(color.B - 100, 0)));
            }
        }

        public class SpheroCircle
        {
            public float _radius;
            public SpheroColor _color;

            public SpheroCircle(float radius, Color color)
            {
                _radius = radius;
                _color = new SpheroColor(color);
            }

            public void Draw(Vector2 pos, CanvasDrawingSession canvas)
            {
                canvas.FillCircle(pos, _radius - 18, _color._main);
                canvas.FillCircle(pos, _radius - 5, _color._inner);
                canvas.FillCircle(pos, _radius, _color._outter);

                canvas.FillEllipse(new Vector2(pos.X, pos.Y - 60), _radius - 60, _radius - 90, Color.FromArgb(50, 255, 255, 255)); 
            }
        }


        public MainPage()
        {
            this.InitializeComponent();


            
        }

        void CanvasControl_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            sender.ClearColor = Colors.Black;

            Vector2 centerScreen = new Vector2((float)sender.ActualWidth / 2, (float)sender.ActualHeight / 2f);
            Vector2 centerShadow = new Vector2((float)sender.ActualWidth / 2, ((float)sender.ActualHeight / 2f) + 150);


            //args.DrawingSession.DrawEllipse(155, 115, 80, 30, Colors.Black, 3);
            args.DrawingSession.DrawText("Sensorial Rhythm", 100, 100, Colors.Red);


            var gradientStops = new CanvasGradientStop[]
            {
                new CanvasGradientStop { Position = 0, Color = Color.FromArgb(100, 0,255,0) },
                new CanvasGradientStop { Position = 1, Color = Colors.Transparent }
            };

            //var middle = new Vector2((float)(sender.Size.Width / 2), (float)(sender.Size.Height / 2));

            var brush = new CanvasRadialGradientBrush(args.DrawingSession,
            gradientStops,
            CanvasEdgeBehavior.Mirror,
            CanvasAlphaMode.Premultiplied
            )
            {
                Center = new Vector2(centerShadow.X, centerShadow.Y),
                RadiusX = 300,
                RadiusY = 50,
            };

            //using (args.DrawingSession.CreateLayer(gradientBrush))
            {
                args.DrawingSession.FillEllipse(centerShadow, 300, 50, brush);
            }


            SpheroCircle sphero = new SpheroCircle(150, Color.FromArgb(255,0,192,0));
            sphero.Draw(centerScreen, args.DrawingSession);

            

            //var circleCenter = new Vector2(300, 300);
            //SpheroCircle sphero = new SpheroCircle(150, Colors.Green);
            //sphero.Draw(circleCenter, args.DrawingSession);

            //circleCenter = new Vector2(500, 300);
            //SpheroCircle sphero2 = new SpheroCircle(150, Colors.Blue);
            //sphero2.Draw(circleCenter, args.DrawingSession);

            //circleCenter = new Vector2(700, 300);
            //SpheroCircle sphero3 = new SpheroCircle(150, Colors.Red);
            //sphero3.Draw(circleCenter, args.DrawingSession);
        }
    }
}
