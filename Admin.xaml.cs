using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Greenhouse
{
    public partial class Admin : Window
    {
        public List<Sensor> sensors = [];
        public List<ActiveDevice> activeDevices = [];
        public List<LightSource> lightSources = [];

        private readonly AutomatedSystem automatedSystemReference;
        private readonly Nature natureReference;

        private readonly TextBlock TextBlockInfo;
        private string first_text_line = "";

        public Admin() {
            InitializeComponent();

            TextBlockInfo = new TextBlock()
            {
                TextWrapping = new System.Windows.TextWrapping(),
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new System.Windows.Thickness(10),
                Text = "Nothing To Show Right Now",
            };
            InfoCanvas.Children.Add(TextBlockInfo);

            natureReference = new Nature(this);
            automatedSystemReference = new AutomatedSystem(this, natureReference);
            Thread ASThread = new Thread(automatedSystemReference.MainLoop);
            Thread InfoThread = new Thread(automatedSystemReference.UpdateInfoTextBox);
            ASThread.Start();
            InfoThread.Start();
        }

        public void SetFirstTextLine(string text) {
            first_text_line = text;
        }

        public void SetTimeText(string text) {
            this.Dispatcher.Invoke(() => {
                TextBlockInfo.Text = first_text_line + text;
            });
        }

        public void MouseMoveHandler(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (e.Source is Border borderSource)
                {
                    System.Windows.Point position = e.GetPosition(MainCanvas);
                    double pX = position.X;
                    double pY = position.Y;
                    if ((pX <= MainCanvas.ActualWidth - borderSource.Width) && (pY >= 0 + borderSource.Height / 2))
                    {
                        borderSource.SetValue(Canvas.LeftProperty, pX - borderSource.Width / 2);
                        borderSource.SetValue(Canvas.TopProperty, pY - borderSource.Height / 2);
                    }
                }
            }
        }

        public void MouseWheelHandler(object sender, MouseEventArgs e)
        {
            if (e.Source is Device source)
            {
                source.RenderTransform = new RotateTransform(-90);
            }
        }

        public void MouseRightButtonDownHandler(object sender, MouseEventArgs e)
        {
            if (e.Source is Sensor sensor_source)
            {
                MainCanvas.Children.Remove(sensor_source);
                sensors.Remove(sensor_source);
            }
            else if (e.Source is ActiveDevice active_device_source)
            {
                MainCanvas.Children.Remove(active_device_source);
                activeDevices.Remove(active_device_source);
            }
            else if (e.Source is LightSource light_source)
            {
                MainCanvas.Children.Remove(light_source);
                lightSources.Remove(light_source);
            }
        }

        // --------------------------------------------------------------------------------------
        // ------------------------------------ GUI Buttons  ------------------------------------
        // --------------------------------------------------------------------------------------

        private void SwitchPauseState(object sender, RoutedEventArgs e)
        {
            automatedSystemReference.SwitchPauseState();
        }

        private void SwitchAutomationState(object sender, RoutedEventArgs e)
        {
            automatedSystemReference.SwitchAutomationState();
        }

        private void SwitchHeaterState(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < activeDevices.Count; i++)
                if (activeDevices[i].GetName() == "Heater")
                    activeDevices[i].SetState(!activeDevices[i].GetState());
        }

        private void SwitchConditionerState(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < activeDevices.Count; i++)
                if (activeDevices[i].GetName() == "Conditioner")
                    activeDevices[i].SetState(!activeDevices[i].GetState());
        }

        private void SwitchHumidifierState(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < activeDevices.Count; i++)
                if (activeDevices[i].GetName() == "Humidifier")
                    activeDevices[i].SetState(!activeDevices[i].GetState());
        }

        private void SwitchFertilizerDispenserState(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < activeDevices.Count; i++)
                if (activeDevices[i].GetName() == "FertilizerDispenser")
                    activeDevices[i].SetState(!activeDevices[i].GetState());
        }

        private void SwitchLightSourceState(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < lightSources.Count; i++)
                lightSources[i].SetState(!lightSources[i].GetState());
        }

        private void AddTemperatureSensor(object sender, RoutedEventArgs e){
            Sensor temp = new Sensor(MainCanvas, this, "aquamarine", "TemperatureSensor");
            sensors.Add(temp);
        }

        private void AddHumiditySensor(object sender, RoutedEventArgs e)
        {
            Sensor temp = new Sensor(MainCanvas, this, "blue", "HumiditySensor");
            sensors.Add(temp);
        }

        private void AddPhSensor(object sender, RoutedEventArgs e)
        {
            Sensor temp = new Sensor(MainCanvas, this, "red", "PhSensor");
            sensors.Add(temp);
        }

        private void AddLightSource(object sender, RoutedEventArgs e)
        {
            LightSource temp = new LightSource(MainCanvas, this, "yellow", "LightSource");
            lightSources.Add(temp);
        }

        private void AddHeater(object sender, RoutedEventArgs e)
        {
            ActiveDevice temp = new ActiveDevice(MainCanvas, this, ["TemperatureSensor", "HumiditySensor"], "aquamarine", "Heater");
            temp.SetPower(1, -1);
            activeDevices.Add(temp);
        }

        private void AddConditioner(object sender, RoutedEventArgs e)
        {
            ActiveDevice temp = new ActiveDevice(MainCanvas, this, ["TemperatureSensor"], "blue", "Conditioner");
            temp.SetPower(0, -1);
            activeDevices.Add(temp);
        }

        private void AddHumidifier(object sender, RoutedEventArgs e)
        {
            ActiveDevice temp = new ActiveDevice(MainCanvas, this, ["HumiditySensor"], "blue", "Humidifier");
            activeDevices.Add(temp);
        }
        
        private void AddFertilizerDispenser(object sender, RoutedEventArgs e)
        {
            ActiveDevice temp = new ActiveDevice(MainCanvas, this, ["PhSensor"], "violet", "FertilizerDispenser");
            temp.SetPower(0, (float)0.1);
            activeDevices.Add(temp);
        }
    }
}