using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Greenhouse
{
    public class Sensor(Canvas canvasReference, Admin adminReference, string color, string name) 
        : Device(canvasReference, adminReference, 20, color, name)
    {
        private float state = 0;

        public float GetState()
        {
            return this.state;
        }

        public void SetState(float state)
        {
            this.state = state;
            if (this.name == "PhSensor" && state < 0)
                this.state = 0;
            else if (this.name == "HumiditySensor" && state < 0)
                this.state = 0;
            else if (this.name == "HumiditySensor" && state > 100)
                this.state = 100;

            this.textblock.Dispatcher.Invoke(new Action(delegate
            {
                this.textblock.Text = this.name + "\n" + this.state.ToString();
            }));
        }
    }
}
