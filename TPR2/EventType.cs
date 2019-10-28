using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TPR2
{
    public partial class EventType : Form
    {
        public EventType()
        {
            InitializeComponent();
        }

        public EventLogic S;

        private void button2_Click(object sender, EventArgs e)
        {
            S = EventLogic.And;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            S = EventLogic.Or;
            this.Close();
        }
    }
}
