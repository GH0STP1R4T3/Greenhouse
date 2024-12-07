using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;

namespace Greenhouse
{
    public class ActiveDevice: Device
    {
        private bool state = true;
        private List<float> power = [1, 1];
        private readonly List<string> correspondingSensorNames;
        private readonly Admin adminReference;

        public ActiveDevice(Canvas canvasReference, Admin adminReference, List<string> correspondingSensorNames, string color, string name) 
            : base(canvasReference, adminReference, 0, color, name)
        {
            this.adminReference = adminReference;
            this.correspondingSensorNames = correspondingSensorNames;
            this.SetState(true);
        }

        public bool GetState()
        {
            return this.state;
        }

        public void SetState(bool state)
        {
            this.state = state;
            this.textblock.Dispatcher.Invoke(new Action(delegate
            {
                if (this.state)
                    this.textblock.Text = this.name + "\n" + "enabled";
                else
                    this.textblock.Text = this.name + "\n" + "disabled";
            }));
        }

        public float GetPower(int index)
        {
            return this.power[index];
        }

        public void SetPower(int index, float power)
        {
            this.power[index] = power;
        }

        public void Work() 
        {
            if (state)
                for (int i = 0; i < adminReference.sensors.Count; i++)
                    if (correspondingSensorNames.Contains(adminReference.sensors[i].GetName()))
                        adminReference.sensors[i].SetState(adminReference.sensors[i].GetState() 
                            + this.power[correspondingSensorNames.IndexOf(adminReference.sensors[i].GetName())]);
        }
    }
}
