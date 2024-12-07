using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Greenhouse
{
    public class LightSource : Device
    {
        private bool state = true;

        public LightSource(Canvas canvasReference, Admin adminReference, string color, string name)
            : base(canvasReference, adminReference, 70, color, name, 100, 60)
        {
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
    }
}
