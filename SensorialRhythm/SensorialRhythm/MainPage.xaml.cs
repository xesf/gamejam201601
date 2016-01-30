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
using RobotKit;
using Windows.ApplicationModel;
using System.Diagnostics;
using RobotKit.Internal;
using Windows.UI.Popups;
using Microsoft.Graphics.Canvas.Text;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SensorialRhythm
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Color[] randomColors = {
        Colors.Blue,
        Colors.BlueViolet,
        Colors.CornflowerBlue,
        /*Colors.DarkBlue,
        Colors.DarkGreen,
        Colors.DarkMagenta,
        Colors.DarkOliveGreen,
        Colors.DarkOrange,
        Colors.DarkRed,
        Colors.DarkTurquoise,
        Colors.DarkViolet,*/
        Colors.DeepPink,
        Colors.Firebrick,
        Colors.ForestGreen,
        Colors.Fuchsia,
        Colors.Green,
        Colors.GreenYellow,
        Colors.HotPink,
        Colors.IndianRed,
        Colors.LightPink,
        Colors.Lime,
        Colors.LimeGreen,
        Colors.Magenta,
        Colors.Maroon,
        Colors.MediumBlue,
        Colors.MediumPurple,
        Colors.MediumTurquoise,
        Colors.MediumVioletRed,
        //Colors.Orange,
        //Colors.OrangeRed,
        Colors.Pink,
        Colors.Purple,
        Colors.Red,
        Colors.Turquoise,
        Colors.Violet,
        Colors.White,
        Colors.YellowGreen
        };

        Random RAND = new Random(DateTime.Now.Millisecond);
        int _colorIdx = 0; // color randomized index for the color array
        TimeSpan _previousElapsedTime;
        TimeSpan _elapsedTime;
        TimeSpan _colorTime;

        Sphero _robot = null;
        float _gyroscopeX = 0;
        float _gyroscopeY = 0;
        float _gyroscopeZ = 0;

        enum SpheroMovementType {
            None,
            PitchForward,
            PitchBackwards,
            RollLeft,
            RollRight,
            YawlClockwise,
            YawlCounterClockwise,
            ShakeIt
        };
        SpheroMovementType _movType = SpheroMovementType.None;

        // debug
        CanvasTextFormat _debugTextFormat = new CanvasTextFormat();

        public class SpheroColor {
            public Color _main;
            public Color _inner;
            public Color _outter;
            public Color _glow;

            public SpheroColor(Color color)
            {
                //_glow = Color.FromArgb((byte)(Math.Max(color.A - 240, 0)), color.R, color.G, color.B);
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
                //canvas.FillCircle(pos, _radius + 30, _color._glow);
                canvas.FillCircle(pos, _radius - 18, _color._main);
                canvas.FillCircle(pos, _radius - 5, _color._inner);
                canvas.FillCircle(pos, _radius, _color._outter);
                

                canvas.FillEllipse(new Vector2(pos.X, pos.Y - 60), _radius - 60, _radius - 90, Color.FromArgb(50, 255, 255, 255)); 
            }
        }


        public MainPage()
        {
            this.InitializeComponent();

            // INIT
            _debugTextFormat.FontSize = 12;
        }
    
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            SetupRobotConnection();
            Application app = Application.Current;
            app.Suspending += OnSuspending;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            ShutdownRobotConnection();
            //ShutdownControls();

            Application app = Application.Current;
            app.Suspending -= OnSuspending;
        }


        private void OnSuspending(object sender, SuspendingEventArgs args)
        {
            ShutdownRobotConnection();
        }

        private void SetupRobotConnection()
        {
            //SpheroName.Text = kNoSpheroConnected;

            RobotProvider provider = RobotProvider.GetSharedProvider();
            provider.DiscoveredRobotEvent += OnRobotDiscovered;
            provider.NoRobotsEvent += OnNoRobotsEvent;
            provider.ConnectedRobotEvent += OnRobotConnected;
            provider.FindRobots();
        }
        
        private void ShutdownRobotConnection()
        {
            if (_robot != null)
            {
                _robot.SensorControl.StopAll();
                _robot.Sleep();
                // temporary while I work on Disconnect.
                _robot.Disconnect();
                //ConnectionToggle.OffContent = "Disconnected";
                //SpheroName.Text = kNoSpheroConnected;

                //_robot.SensorControl.AccelerometerUpdatedEvent -= OnAccelerometerUpdated;
                //_robot.SensorControl.AttitudeUpdatedEvent -= SensorControl_AttitudeUpdatedEvent;
                _robot.SensorControl.GyrometerUpdatedEvent -= OnGyrometerUpdated;

                //m_robot.CollisionControl.StopDetection();
                //m_robot.CollisionControl.CollisionDetectedEvent -= OnCollisionDetected;

                RobotProvider provider = RobotProvider.GetSharedProvider();
                provider.DiscoveredRobotEvent -= OnRobotDiscovered;
                provider.NoRobotsEvent -= OnNoRobotsEvent;
                provider.ConnectedRobotEvent -= OnRobotConnected;
            }
        }

        private void OnRobotDiscovered(object sender, Robot robot)
        {
            Debug.WriteLine(string.Format("Discovered \"{0}\"", robot.BluetoothName));

            if (_robot == null)
            {
                RobotProvider provider = RobotProvider.GetSharedProvider();
                provider.ConnectRobot(robot);
                //ConnectionToggle.OnContent = "Connecting...";
                _robot = (Sphero)robot;
                //SpheroName.Text = string.Format(kConnectingToSphero, robot.BluetoothName);
            }
        }


        private void OnNoRobotsEvent(object sender, EventArgs e)
        {
            MessageDialog dialog = new MessageDialog("No Sphero Paired");
            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 1;
            dialog.ShowAsync();
        }

        
        private void OnRobotConnected(object sender, Robot robot)
        {
            Debug.WriteLine(string.Format("Connected to {0}", robot));
            //ConnectionToggle.IsOn = true;
            //ConnectionToggle.OnContent = "Connected";

            _robot.SetBackLED(127);
            //m_robot.SetRGBLED(255, 255, 255);
            //SpheroName.Text = string.Format(kSpheroConnected, robot.BluetoothName);
            //SetupControls();

            //m_robot.SetHeading(0);

            //m_robot.SensorControl.StopAll();

            // stop rotors
            _robot.WriteToRobot(new DeviceMessage(2, 0x33, new byte[] { 0, 0, 0, 0 }));

            _robot.SensorControl.Hz = 10;

            _robot.SensorControl.AccelerometerUpdatedEvent += OnAccelerometerUpdated;
            _robot.SensorControl.AttitudeUpdatedEvent += SensorControl_AttitudeUpdatedEvent;
            _robot.SensorControl.GyrometerUpdatedEvent += OnGyrometerUpdated;

            //m_robot.CollisionControl.StartDetectionForWallCollisions();
            //m_robot.CollisionControl.CollisionDetectedEvent += OnCollisionDetected;
        }

        private void SensorControl_AttitudeUpdatedEvent(object sender, AttitudeReading reading)
        {
            //AtittudeRoll.Text = "" + reading.Roll;
            //AtittudePitch.Text = "" + reading.Pitch;
            //AtittudeYaw.Text = "" + reading.Yaw;
        }

        private void OnAccelerometerUpdated(object sender, AccelerometerReading reading)
        {
            //AccelerometerX.Text = "" + reading.X;
            //AccelerometerY.Text = "" + reading.Y;
            //AccelerometerZ.Text = "" + reading.Z;
        }

        private void OnGyrometerUpdated(object sender, GyrometerReading reading)
        {
            _gyroscopeX = reading.X;
            _gyroscopeY = reading.Y;
            _gyroscopeZ = reading.Z;
        }

        private void CanvasAnimatedControl_Update(ICanvasAnimatedControl sender, CanvasAnimatedUpdateEventArgs args)
        {
            _previousElapsedTime = _elapsedTime;
            _elapsedTime = args.Timing.ElapsedTime;

            _colorTime += _elapsedTime;

            if (_colorTime.Milliseconds > 500)
            {
                _colorIdx++; // = RAND.Next(0, randomColors.Length);
                if (_colorIdx >= randomColors.Length - 1)
                    _colorIdx = 0;
                _colorTime = TimeSpan.Zero;

                // TODO change this to a proper place

                _robot.SetRGBLED(randomColors[_colorIdx].R, randomColors[_colorIdx].G, randomColors[_colorIdx].B);
            }

            ProcessMovementType();
        }

        private void ProcessMovementType()
        {
            float minValue = -650;
            float maxValue = 650;
            float minShakeValue = -1200;
            float maxShakeValue = 1200;

            _movType = SpheroMovementType.None;

            if (_gyroscopeX > maxValue)
                _movType = SpheroMovementType.PitchForward;
            else if (_gyroscopeX < minValue)
                _movType = SpheroMovementType.PitchBackwards;
            else if (_gyroscopeY > maxValue)
                _movType = SpheroMovementType.RollLeft;
            else if (_gyroscopeY < minValue)
                _movType = SpheroMovementType.RollRight;
            else if (_gyroscopeZ > maxValue)
                _movType = SpheroMovementType.YawlCounterClockwise;
            else if (_gyroscopeZ < minValue)
                _movType = SpheroMovementType.YawlClockwise;

            if (_gyroscopeX > maxShakeValue || _gyroscopeX < minShakeValue ||
                _gyroscopeY > maxShakeValue || _gyroscopeY < minShakeValue ||
                _gyroscopeZ > maxShakeValue || _gyroscopeZ < minShakeValue)
            {
                _movType = SpheroMovementType.ShakeIt;
            }
        }

        private void CanvasAnimatedControl_Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            sender.ClearColor = Colors.Black;
            
            Vector2 centerScreen = new Vector2((float)sender.Size.Width / 2, ((float)sender.Size.Height / 2f) + 20);
            Vector2 centerShadow = new Vector2((float)sender.Size.Width / 2, ((float)sender.Size.Height / 2f) + 180);
            
            args.DrawingSession.DrawText("Sensorial Rhythm", 100, 100, Colors.Red);


            var gradientStops = new CanvasGradientStop[]
            {
                new CanvasGradientStop { Position = 0, Color = randomColors[_colorIdx] },
                new CanvasGradientStop { Position = 1, Color = Colors.Transparent }
            };

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
            
            // Color.FromArgb(255,0,192,0) // green
            SpheroCircle sphero = new SpheroCircle(150, randomColors[_colorIdx]);
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


            // Debug
            args.DrawingSession.DrawText("Color [" + _colorIdx + "] " + randomColors[_colorIdx].ToString(), 10, (float)sender.Size.Height - 100, Colors.Gray, _debugTextFormat);
            args.DrawingSession.DrawText("Gyroscope X :" + _gyroscopeX, 10, (float)sender.Size.Height - 85, Colors.Gray, _debugTextFormat);
            args.DrawingSession.DrawText("Gyroscope Y :" + _gyroscopeY, 10, (float)sender.Size.Height - 70, Colors.Gray, _debugTextFormat);
            args.DrawingSession.DrawText("Gyroscope Z :" + _gyroscopeZ, 10, (float)sender.Size.Height - 55, Colors.Gray, _debugTextFormat);
            args.DrawingSession.DrawText("Movement Type :" + _movType, 10, (float)sender.Size.Height - 40, Colors.Gray, _debugTextFormat);
        }

    }
}
