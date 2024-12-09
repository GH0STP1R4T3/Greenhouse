using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace Greenhouse
{
    class AutomatedSystem
    {
        private readonly List<int> temperaturePlan = [];
        private readonly List<int> humidityPlan = [];
        private readonly List<int> phPlan = [];
        private readonly List<int> lightPlan = [];

        private string planPath;
        private int todaysLightHours = 0;
        private int htimeLeft = 0;
        private bool pause = true;
        private bool stopAutomation = false;
        string additionalInfoForInfoTextBox = "";

        private readonly Admin adminReference;
        private readonly Nature natureReference;

        public AutomatedSystem(Admin adminReference, Nature natureReference)
        {
            this.adminReference = adminReference;
            this.natureReference = natureReference;

            var dlg = new OpenFileDialog()
            {
                Filter = "All Files (*.*) | *.*",
                RestoreDirectory = true,
                ForcePreviewPane = true
            };
            dlg.ShowDialog();
            this.planPath = dlg.FileName;

            string[] lines = File.ReadAllLines(this.planPath);
            adminReference.SetFirstTextLine(lines[0]);
            for (int i = 1; i < lines.Length; i++)
            {
                string[] line = lines[i].Split(' ');
                temperaturePlan.Add(int.Parse(line[0]));
                humidityPlan.Add(int.Parse(line[1]));
                phPlan.Add(int.Parse(line[2]));
                lightPlan.Add(int.Parse(line[3]));
            }
        }

        public void UpdateInfoTextBox() 
        {
            while (true)
            {
                int dayNumber = htimeLeft / 24;
                string firstTextLine = String.Join(" ", " -- Time left:", dayNumber, "d", htimeLeft % 24, "h");
                if (pause)
                    firstTextLine += "          ----- PAUSED -----";
                if (stopAutomation)
                    firstTextLine += "          ----- AUTOMATION SYSTEM STOPPED -----";
                firstTextLine += additionalInfoForInfoTextBox;
                adminReference.SetTimeText(String.Join(" ", firstTextLine,
                            "\nTodays Plan:",
                            "\nTemperature:", temperaturePlan[dayNumber],
                            "\nHumidity:", humidityPlan[dayNumber],
                            "\nPh: ", phPlan[dayNumber],
                            "\nLight Plan (hours per day):", lightPlan[dayNumber]));
                System.Threading.Thread.Sleep(100);
            }
        }

        public void MainLoop() {
            while (true) {
                System.Threading.Thread.Sleep(2000);
                int dayNumber = htimeLeft / 24;

                if (htimeLeft == 0)
                {
                    for (int i = 0; i < adminReference.sensors.Count; i++)
                        if (adminReference.sensors[i].GetName() == "TemperatureSensor")
                            adminReference.sensors[i].SetState(temperaturePlan[dayNumber]);
                        else if (adminReference.sensors[i].GetName() == "HumiditySensor")
                            adminReference.sensors[i].SetState(humidityPlan[dayNumber]);
                        else if (adminReference.sensors[i].GetName() == "PhSensor")
                            adminReference.sensors[i].SetState(phPlan[dayNumber]);
                }
               

                if (!pause && dayNumber < temperaturePlan.Count)
                {
                    htimeLeft += 1;

                    if (!stopAutomation)
                    {
                        float currentTemperature = 0;
                        float currentHumidity = 0;
                        float currentPh = 0;

                        for (int i = 0; i < adminReference.sensors.Count; i++)
                        {
                            if (adminReference.sensors[i].GetName() == "TemperatureSensor")
                                currentTemperature = adminReference.sensors[i].GetState();
                            else if (adminReference.sensors[i].GetName() == "HumiditySensor")
                                currentHumidity = adminReference.sensors[i].GetState();
                            else if (adminReference.sensors[i].GetName() == "PhSensor")
                                currentPh = adminReference.sensors[i].GetState();
                        }

                        if (currentTemperature < temperaturePlan[dayNumber])
                        {
                            for (int i = 0; i < adminReference.activeDevices.Count; i++)
                            {
                                if (adminReference.activeDevices[i].GetName() == "Heater")
                                    adminReference.activeDevices[i].SetState(true);
                                if (adminReference.activeDevices[i].GetName() == "Conditioner")
                                    adminReference.activeDevices[i].SetState(false);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < adminReference.activeDevices.Count; i++)
                            {
                                if (adminReference.activeDevices[i].GetName() == "Heater")
                                    adminReference.activeDevices[i].SetState(false);
                                if (adminReference.activeDevices[i].GetName() == "Conditioner")
                                    adminReference.activeDevices[i].SetState(true);
                            }
                        }

                        if (currentHumidity < humidityPlan[dayNumber])
                        {
                            for (int i = 0; i < adminReference.activeDevices.Count; i++)
                            {
                                if (adminReference.activeDevices[i].GetName() == "Heater")
                                    adminReference.activeDevices[i].SetState(false);
                                if (adminReference.activeDevices[i].GetName() == "Humidifier")
                                    adminReference.activeDevices[i].SetState(true);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < adminReference.activeDevices.Count; i++)
                            {
                                if (adminReference.activeDevices[i].GetName() == "Heater")
                                    adminReference.activeDevices[i].SetState(true);
                                if (adminReference.activeDevices[i].GetName() == "Humidifier")
                                    adminReference.activeDevices[i].SetState(false);
                            }
                        }

                        if (currentPh < phPlan[dayNumber])
                        {
                            for (int i = 0; i < adminReference.activeDevices.Count; i++)
                                if (adminReference.activeDevices[i].GetName() == "FertilizerDispenser")
                                    adminReference.activeDevices[i].SetState(true);
                        }
                        else
                        {
                            for (int i = 0; i < adminReference.activeDevices.Count; i++)
                                if (adminReference.activeDevices[i].GetName() == "FertilizerDispenser")
                                    adminReference.activeDevices[i].SetState(false);
                        }

                        if (todaysLightHours == lightPlan[dayNumber])
                            for (int i = 0; i < adminReference.lightSources.Count; i++)
                                adminReference.lightSources[i].SetState(false);
                    }

                    for (int i = 0; i < adminReference.activeDevices.Count; i++)
                        adminReference.activeDevices[i].Work();

                    for (int i = 0; i < adminReference.lightSources.Count; i++)
                        if (adminReference.lightSources[i].GetState())
                        {
                            todaysLightHours += 1;
                            break;
                        }

                    if (htimeLeft % 4 == 0)
                        natureReference.GenerateDeviations();
                }
                else if (dayNumber >= temperaturePlan.Count) 
                {
                    additionalInfoForInfoTextBox = "--- PLAN IS FINISHED --- SYSTEM STOPS EXECUTION ---";
                }
            }
        }

        public void SwitchPauseState()
        {
            this.pause = !pause;
        }

        public void SwitchAutomationState()
        {
            this.stopAutomation = !stopAutomation;
        }

        public string GetPlanPath() {
            return planPath;
        }

        public int GetCurrentParameter(String parameter) 
        {
            int dayNumber = htimeLeft / 24;
            switch (parameter) 
            {
                case "temperature":
                    return temperaturePlan[dayNumber];
                case "humidity":
                    return humidityPlan[dayNumber];
                case "ph":
                    return phPlan[dayNumber];
                default:
                    return temperaturePlan[dayNumber];
            }
        }

        public int GetHTimeLeft() {
            return htimeLeft;
        }
    }
}
