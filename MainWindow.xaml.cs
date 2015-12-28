using System;
using System.Windows;

using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using Microsoft.Kinect.Toolkit.Controls;

namespace KinectToolbox
{

    public partial class MainWindow : Window
    {
        private KinectSensorChooser sensorChooser;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(Window_Loaded);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.sensorChooser = new KinectSensorChooser();
            this.sensorChooser.KinectChanged += SensorChooserOnKinectChanged;
            this.sensorChooserUi.KinectSensorChooser = this.sensorChooser;
            this.sensorChooser.Start();
            createButtons();
        }

        private void SensorChooserOnKinectChanged(object sender, KinectChangedEventArgs args)
        {
            bool error = false;

            if (args.OldSensor != null)
            {
                try
                {
                    args.OldSensor.DepthStream.Range = DepthRange.Default;
                    args.OldSensor.SkeletonStream.EnableTrackingInNearRange = false;
                    args.OldSensor.DepthStream.Disable();
                    args.OldSensor.SkeletonStream.Disable();
                }
                catch (InvalidOperationException)
                {
                    // KinectSensor entra en estado invalid mientras se habilitan/deshabilitan
                    // streams.
                    // Ej.: Se desconecta sensor de forma abrupta.
                    error = true;
                }
            }
            if (args.NewSensor != null)
            {
                try
                {
                    //ToDo: Habilite el stream de profundidad con formato Resolution640x480Fps30
                    args.NewSensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);

                    //ToDo: Habilite el stream de esqueletos
                    args.NewSensor.SkeletonStream.Enable();
                }
                catch (InvalidOperationException)
                {
                    error = true;
                }
            }

            if (!error) kinectRegion.KinectSensor = args.NewSensor;

        }


        private void ButtonOnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("¡Estupendo!");
        }

        private void createButtons()
        {
            for (int i = 1; i <= 50; i++)
            {
                KinectCircleButton btn = new KinectCircleButton();
 
                btn.Name = "button_" + i.ToString();
                btn.Content = i.ToString();
                btn.Height  = 200;
                btn.Click += OnClickRandomButton;

                scrollContent.Children.Add(btn);
            }
        }

        private void OnClickRandomButton(object sender, RoutedEventArgs e)
        {
            KinectCircleButton btn = (KinectCircleButton)e.Source;
            MessageBox.Show("Ha pulsado el botón: " + btn.Content.ToString());
        }

    }
}
