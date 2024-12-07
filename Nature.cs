﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Greenhouse
{
    internal class Nature(Admin adminReference)
    {
        private readonly Admin adminReference = adminReference;

        public void GenerateDeviations() {
            for (int i = 0; i < adminReference.sensors.Count; i++) 
            {
                if (adminReference.sensors[i].GetName() == "TemperatureSensor")
                    adminReference.sensors[i].SetState(adminReference.sensors[i].GetState() + new Random().NextInt64(-2, 2));
                else if (adminReference.sensors[i].GetName() == "HumiditySensor")
                    adminReference.sensors[i].SetState(adminReference.sensors[i].GetState() + new Random().NextInt64(-4, 4));
                else if (adminReference.sensors[i].GetName() == "PhSensor")
                    adminReference.sensors[i].SetState(adminReference.sensors[i].GetState() - (float)(new Random().Next(0, 4))/10);
            }
        }
    }
}