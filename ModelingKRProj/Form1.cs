using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModelingKRProj
{
    public partial class Form1 : Form
    {
        ModelingSystem system;

        public Form1()
        {
            InitializeComponent();

            system = new ModelingSystem();

            propertyGrid1.SelectedObject = system;

            //propertyGrid1.Refresh();
        }

        private void propertyGrid1_SelectedObjectsChanged(object sender, EventArgs e)
        {
            ((PropertyGrid)(sender)).Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = system;
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            
        }
    }
}
