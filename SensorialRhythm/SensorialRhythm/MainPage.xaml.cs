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
using SharpDX.XAudio2;
using SharpDX.Multimedia;
using SharpDX.IO;

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

        public enum SpheroMovementType {
            None,
            PitchForward,
            PitchBackwards,
            RollLeft,
            RollRight,
            YawlClockwise,
            YawlCounterClockwise,
            ShakeIt,
            DoubleTap
        };
        SpheroMovementType _movType = SpheroMovementType.None;

        public enum SpheroTapType
        {
            None,
            DoubleTap
        };
        SpheroTapType _tapType = SpheroTapType.None;

        public enum GameState
        {
            None,
            Connecting,
            Connected,
            ConnectionFailed,
            Ready,
            ThreeTwoOneGo,
            LevelUp,
            GameOver
        }
        GameState _gameState = GameState.None;


        //SoundEffect _anthem = new SoundEffect(@"Sound\Amhran_na_bhFiann.wav");

        SoundEffect _tribal = new SoundEffect(@"Sound\african_tribal_short.wav");


        // debug
        CanvasTextFormat _debugTextFormat = new CanvasTextFormat();
        CanvasTextFormat _debugSequenceTextFormat = new CanvasTextFormat();

        public class SpheroColor {
            public Color _main;
            public Color _inner;
            public Color _outter;
            public Color _glow;

            public SpheroColor(Color color)
            {
                //_glow = Color.FromArgb((byte)(Math.Max(color.A - 240, 0)), color.R, color.G, color.B);
                _main = Color.FromArgb(color.A, color.R, color.G, color.B);
                _inner = Color.FromArgb((byte)(Math.Max(color.A - 50, 0)), (byte)(Math.Max(color.R - 50, 0)), (byte)(Math.Max(color.G - 50, 0)), (byte)(Math.Max(color.B - 50, 0)));
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
            _debugSequenceTextFormat.FontSize = 16;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
        }

        #region Sphero Connection

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
            if (_robot != null && _robot.ConnectionState == ConnectionState.Connected)
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

                //RobotProvider provider = RobotProvider.GetSharedProvider();
                //provider.DiscoveredRobotEvent -= OnRobotDiscovered;
                //provider.NoRobotsEvent -= OnNoRobotsEvent;
                //provider.ConnectedRobotEvent -= OnRobotConnected;
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

                _gameState = GameState.Connecting;
            }
        }


        private void OnNoRobotsEvent(object sender, EventArgs e)
        {
            MessageDialog dialog = new MessageDialog("No Sphero Paired");
            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 1;
            dialog.ShowAsync();

            _gameState = GameState.ConnectionFailed;
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

            //_robot.SetHeading(0);
            //_robot.Roll(0, 1);

            //m_robot.SensorControl.StopAll();

            // stop rotors
            _robot.WriteToRobot(new DeviceMessage(2, 0x33, new byte[] { 0, 0, 0, 0 }));

            _robot.SensorControl.Hz = 10;

            _robot.SensorControl.AccelerometerUpdatedEvent += OnAccelerometerUpdated;
            _robot.SensorControl.AttitudeUpdatedEvent += SensorControl_AttitudeUpdatedEvent;
            _robot.SensorControl.GyrometerUpdatedEvent += OnGyrometerUpdated;

            _robot.CollisionControl.StartDetectionForWallCollisions();
            _robot.CollisionControl.CollisionDetectedEvent += OnCollisionDetected;

            _gameState = GameState.Connected;
            _currentColor = Colors.Red;
            _robot.SetRGBLED(_currentColor.R,
                             _currentColor.G,
                             _currentColor.B);
        }

        private void OnCollisionDetected(object sender, CollisionData e)
        {
            _tapType = SpheroTapType.DoubleTap;
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

        #endregion


        private void CanvasAnimatedControl_Update(ICanvasAnimatedControl sender, CanvasAnimatedUpdateEventArgs args)
        {
            if (_gameState == GameState.Connecting)
            {
                if (_robot.ConnectionState == ConnectionState.Failed/* ||
                    _robot.ConnectionState == ConnectionState.Disconnected*/)
                {
                    _gameState = GameState.ConnectionFailed;
                }
                return;
            }
            if (_gameState == GameState.ConnectionFailed)
            {
                _robot = null;
                SetupRobotConnection();
                _gameState = GameState.Connecting;
                return;
            }


            ProcessMovementType();

            if (_gameState == GameState.Connected)
            {
                _gameState = GameState.Ready;
            }

            if (_gameState == GameState.Ready)
            {
                if (_movType == SpheroMovementType.ShakeIt)
                {
                    _currentLevel = 0;
                    _gameState = GameState.ThreeTwoOneGo;
                    _currentSequence = _gameSequence[_currentLevel]; // starting sequence
                }
            }

            ProcessLevel(args);
        }

        private void ProcessMovementType()
        {
            float minValue = -650;
            float maxValue = 650;
            float minShakeValue = -1300;
            float maxShakeValue = 1300;

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
                new CanvasGradientStop { Position = 0, Color = _currentColor },
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
            SpheroCircle sphero = new SpheroCircle(150, _currentColor);
            sphero.Draw(centerScreen, args.DrawingSession);

            // Debug
            args.DrawingSession.DrawText("Game State: " + _gameState, 10, (float)sender.Size.Height - 115, Colors.Gray, _debugTextFormat);
            args.DrawingSession.DrawText("Color [" + _currentBeatTime + "]: " + _currentColor.ToString(), 10, (float)sender.Size.Height - 100, Colors.Gray, _debugTextFormat);
            args.DrawingSession.DrawText("Gyroscope X: " + _gyroscopeX, 10, (float)sender.Size.Height - 85, Colors.Gray, _debugTextFormat);
            args.DrawingSession.DrawText("Gyroscope Y: " + _gyroscopeY, 10, (float)sender.Size.Height - 70, Colors.Gray, _debugTextFormat);
            args.DrawingSession.DrawText("Gyroscope Z: " + _gyroscopeZ, 10, (float)sender.Size.Height - 55, Colors.Gray, _debugTextFormat);
            args.DrawingSession.DrawText("Movement Type: " + _movType, 10, (float)sender.Size.Height - 40, Colors.Gray, _debugTextFormat);
            args.DrawingSession.DrawText("Tap Type: " + _tapType, 10, (float)sender.Size.Height - 25, Colors.Gray, _debugTextFormat);
        }

        // Game Sequence
        public struct GameSequence {
            public int Tempo;
            public int Times;
            public Color[] Colors;
            public SpheroMovementType[] Movements;
            public int TempoColorSwitch;

            public GameSequence(int tempo, int times, Color[] colors, SpheroMovementType[] movements, int tempoColorSwitch)
            {
                Tempo = tempo;
                Times = times;
                Colors = colors;
                Movements = movements;
                TempoColorSwitch = tempoColorSwitch;
            }
        }

        // Sequence numbers correspond to Level UPs
        GameSequence[] _gameSequence = {
            new GameSequence(1, 1, new Color[]{ Color.FromArgb(255, 0, 255, 0), // green, 
            },
                                      new SpheroMovementType[]{ SpheroMovementType.PitchForward },
            0),
            new GameSequence(2400, 7, new Color[]{ Colors.Red,
                                                   Colors.Blue,
                                                   Color.FromArgb(255, 0, 255, 0), // green
                                                   Colors.White,
                                                   Color.FromArgb(255, 0, 255, 0), // green
                                                   Colors.Red,
                                                   Colors.White }, 
                                      new SpheroMovementType[]{ SpheroMovementType.PitchForward,
                                                                SpheroMovementType.PitchBackwards,
                                                                SpheroMovementType.RollLeft,
                                                                SpheroMovementType.RollRight,
                                                                SpheroMovementType.PitchBackwards,
                                                                SpheroMovementType.RollLeft,
                                                                SpheroMovementType.PitchForward },
            0),
            new GameSequence(480, 2, new Color[]{ Colors.Blue,
                                                Colors.White 
            },
                                      new SpheroMovementType[]{ SpheroMovementType.PitchForward,
                                                                SpheroMovementType.PitchForward},
            0),
            new GameSequence(1420, 1, new Color[]{ Color.FromArgb(255, 0, 255, 0), // green, 
            },
                                      new SpheroMovementType[]{ SpheroMovementType.PitchForward },
            0),
            new GameSequence(480, 2, new Color[]{ Colors.Blue,
                                                Colors.White
            },
                                      new SpheroMovementType[]{ SpheroMovementType.PitchForward },
            0),
            new GameSequence(1420, 1, new Color[]{ Colors.Red,
                                                   Colors.Blue,
                                                   Color.FromArgb(255, 0, 255, 0), // green
                                                   Colors.White,
                                                   Color.FromArgb(255, 0, 255, 0), // green
                                                   Colors.Red,
                                                   Colors.Blue,
                                                   Colors.Red,
                                                   Colors.Blue,
                                                   Color.FromArgb(255, 0, 255, 0), // green
                                                   Colors.White,
                                                   Color.FromArgb(255, 0, 255, 0), // green
                                                   Colors.Red,
                                                   Colors.Blue},
                                      new SpheroMovementType[]{ SpheroMovementType.PitchForward,
                                                                SpheroMovementType.PitchBackwards,
                                                                SpheroMovementType.RollLeft,
                                                                SpheroMovementType.RollRight,
                                                                SpheroMovementType.PitchBackwards,
                                                                SpheroMovementType.RollLeft,
                                                                SpheroMovementType.PitchForward },
            0),
            new GameSequence(480, 2, new Color[]{ Colors.Blue,
                                                Colors.White
            },
                                      new SpheroMovementType[]{ SpheroMovementType.PitchForward },
            0),
            new GameSequence(1420, 1, new Color[]{ Colors.Red,
                                                   Colors.Blue,
                                                   Color.FromArgb(255, 0, 255, 0), // green
                                                   Colors.White,
                                                   Color.FromArgb(255, 0, 255, 0), // green
                                                   Colors.Red,
                                                   Colors.Blue,
                                                   Colors.Red,
                                                   Colors.Blue,
                                                   Color.FromArgb(255, 0, 255, 0), // green
                                                   Colors.White,
                                                   Color.FromArgb(255, 0, 255, 0), // green
                                                   Colors.Red,
                                                   Colors.Blue},
                                      new SpheroMovementType[]{ SpheroMovementType.PitchForward,
                                                                SpheroMovementType.PitchBackwards,
                                                                SpheroMovementType.RollLeft,
                                                                SpheroMovementType.RollRight,
                                                                SpheroMovementType.PitchBackwards,
                                                                SpheroMovementType.RollLeft,
                                                                SpheroMovementType.PitchForward },
            0),
            new GameSequence(480, 2, new Color[]{ Colors.Blue,
                                                Colors.White
            },
                                      new SpheroMovementType[]{ SpheroMovementType.PitchForward },
            0),
            new GameSequence(1420, 1, new Color[]{ Colors.Red,
                                                   Colors.Blue,
                                                   Color.FromArgb(255, 0, 255, 0), // green
                                                   Colors.White,
                                                   Color.FromArgb(255, 0, 255, 0), // green
                                                   Colors.Red,
                                                   Colors.Blue,
                                                   Colors.Red,
                                                   Colors.Blue,
                                                   Color.FromArgb(255, 0, 255, 0), // green
                                                   Colors.White,
                                                   Color.FromArgb(255, 0, 255, 0), // green
                                                   Colors.Red,
                                                   Colors.Blue},
                                      new SpheroMovementType[]{ SpheroMovementType.PitchForward,
                                                                SpheroMovementType.PitchBackwards,
                                                                SpheroMovementType.RollLeft,
                                                                SpheroMovementType.RollRight,
                                                                SpheroMovementType.PitchBackwards,
                                                                SpheroMovementType.RollLeft,
                                                                SpheroMovementType.PitchForward },
            0),
            new GameSequence(480, 2, new Color[]{ Colors.Blue,
                                                Colors.White
            },
                                      new SpheroMovementType[]{ SpheroMovementType.PitchForward },
            0),
            new GameSequence(1420, 1, new Color[]{ Colors.Red,
                                                   Colors.Blue,
                                                   Color.FromArgb(255, 0, 255, 0), // green
                                                   Colors.White,
                                                   Color.FromArgb(255, 0, 255, 0), // green
                                                   Colors.Red,
                                                   Colors.Blue,
                                                   Colors.Red,
                                                   Colors.Blue,
                                                   Color.FromArgb(255, 0, 255, 0), // green
                                                   Colors.White,
                                                   Color.FromArgb(255, 0, 255, 0), // green
                                                   Colors.Red,
                                                   Colors.Blue},
                                      new SpheroMovementType[]{ SpheroMovementType.PitchForward,
                                                                SpheroMovementType.PitchBackwards,
                                                                SpheroMovementType.RollLeft,
                                                                SpheroMovementType.RollRight,
                                                                SpheroMovementType.PitchBackwards,
                                                                SpheroMovementType.RollLeft,
                                                                SpheroMovementType.PitchForward },
            0),
            new GameSequence(480, 2, new Color[]{ Colors.Blue,
                                                Colors.White
            },
                                      new SpheroMovementType[]{ SpheroMovementType.PitchForward },
            0),
            new GameSequence(1420, 1, new Color[]{ Colors.Red,
                                                   Colors.Blue,
                                                   Color.FromArgb(255, 0, 255, 0), // green
                                                   Colors.White,
                                                   Color.FromArgb(255, 0, 255, 0), // green
                                                   Colors.Red,
                                                   Colors.Blue,
                                                   Colors.Red,
                                                   Colors.Blue,
                                                   Color.FromArgb(255, 0, 255, 0), // green
                                                   Colors.White,
                                                   Color.FromArgb(255, 0, 255, 0), // green
                                                   Colors.Red,
                                                   Colors.Blue},
                                      new SpheroMovementType[]{ SpheroMovementType.PitchForward,
                                                                SpheroMovementType.PitchBackwards,
                                                                SpheroMovementType.RollLeft,
                                                                SpheroMovementType.RollRight,
                                                                SpheroMovementType.PitchBackwards,
                                                                SpheroMovementType.RollLeft,
                                                                SpheroMovementType.PitchForward },
            0),
            new GameSequence(480, 2, new Color[]{ Colors.Blue,
                                                Colors.White
            },
                                      new SpheroMovementType[]{ SpheroMovementType.PitchForward },
            0),
            new GameSequence(1420, 1, new Color[]{ Colors.Red,
                                                   Colors.Blue,
                                                   Color.FromArgb(255, 0, 255, 0), // green
                                                   Colors.White,
                                                   Color.FromArgb(255, 0, 255, 0), // green
                                                   Colors.Red,
                                                   Colors.Blue,
                                                   Colors.Red,
                                                   Colors.Blue,
                                                   Color.FromArgb(255, 0, 255, 0), // green
                                                   Colors.White,
                                                   Color.FromArgb(255, 0, 255, 0), // green
                                                   Colors.Red,
                                                   Colors.Blue},
                                      new SpheroMovementType[]{ SpheroMovementType.PitchForward,
                                                                SpheroMovementType.PitchBackwards,
                                                                SpheroMovementType.RollLeft,
                                                                SpheroMovementType.RollRight,
                                                                SpheroMovementType.PitchBackwards,
                                                                SpheroMovementType.RollLeft,
                                                                SpheroMovementType.PitchForward },
            0),
            new GameSequence(480, 2, new Color[]{ Colors.Blue,
                                                Colors.White
            },
                                      new SpheroMovementType[]{ SpheroMovementType.PitchForward },
            0)
        };

        int _currentLevel = 0;
        int _currentBeatTime = 0;
        TimeSpan _currentElapsedTime = TimeSpan.Zero;
        GameSequence _currentSequence;
        Color _currentColor = Colors.DarkGray;

        void ProcessLevel(CanvasAnimatedUpdateEventArgs args)
        {
            if (_gameState == GameState.LevelUp)
            {
                _currentLevel++;
                if (_currentLevel > _gameSequence.Length - 1)
                    _currentLevel = _gameSequence.Length - 1;
                _currentBeatTime = 0;
                _currentSequence = _gameSequence[_currentLevel];
                _currentElapsedTime = TimeSpan.Zero;

                _gameState = GameState.ThreeTwoOneGo;
            }

            if (_gameState != GameState.ThreeTwoOneGo)
                return;

            if (!_tribal.IsPlaying)
            {
                _tribal.Play();
            }

            if (_tapType == SpheroTapType.DoubleTap)
            {
                _tapType = SpheroTapType.None;
            }

            _previousElapsedTime = _elapsedTime;
            _elapsedTime = args.Timing.ElapsedTime;

            //_colorTime += _elapsedTime;

            //if (_colorTime.Milliseconds > 500)
            //{
            //    _colorIdx++; // = RAND.Next(0, randomColors.Length);
            //    if (_colorIdx >= randomColors.Length - 1)
            //        _colorIdx = 0;
            //    _colorTime = TimeSpan.Zero;

            //    // TODO change this to a proper place

            //    _robot.SetRGBLED(randomColors[_colorIdx].R, randomColors[_colorIdx].G, randomColors[_colorIdx].B);
            //}
            
            _currentElapsedTime += _elapsedTime;
            //_currentSequence = _gameSequence[_currentLevel];

            if (_currentBeatTime < _currentSequence.Times)
            {
                if (_currentElapsedTime.TotalMilliseconds > _currentSequence.Tempo ||
                    _currentElapsedTime.TotalMilliseconds > _currentSequence.Tempo + _currentSequence.TempoColorSwitch)
                {
                    _currentColor = _currentSequence.Colors[_currentBeatTime];
                    if (_currentElapsedTime.TotalMilliseconds > _currentSequence.Tempo + _currentSequence.TempoColorSwitch)
                        _currentElapsedTime = TimeSpan.Zero;

                    _robot.SetRGBLED(_currentColor.R,
                                     _currentColor.G,
                                     _currentColor.B);

                    _currentBeatTime++;

                    if (_currentBeatTime == _currentSequence.Times)
                        _gameState = GameState.LevelUp;
                }
            }

            if (_currentBeatTime >= _currentSequence.Times && 
                _movType == SpheroMovementType.ShakeIt)
            {
                _movType = SpheroMovementType.None;
                _tribal.Stop();

                _currentBeatTime = 0;
                _currentLevel = 0;
                _currentElapsedTime = TimeSpan.Zero;
            }
        }
    }
}
