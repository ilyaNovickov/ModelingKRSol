using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ModelingKRProj
{
    public partial class Form1 : Form
    {
        ModelingSystem system;

        Series modelingSeries;

        Series phaseSeries;

        public Form1()
        {
            InitializeComponent();

            system = new ModelingSystem();

            system.ModelingEvent += ModelingEvent;

            system.ModelingEvent += PhaseModelingEvent;

            propertyGrid.SelectedObject = system;

            propertyGrid.Refresh();

            modelingSeries = new Series(nameof(modelingSeries))
            {
                ChartType = SeriesChartType.Line
            };

            this.modelingChart.Series.Add(modelingSeries);

            phaseSeries = new Series(nameof(phaseSeries))
            {
                ChartType = SeriesChartType.Line
            };

            this.phaseChart.Series.Add(phaseSeries);
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            system.InvokeModeling();
        }

        private void ModelingEvent(object sender, ModelingEventArgs e)
        {
            modelingSeries.Points.Add(new DataPoint(e.CurrentTime_t, e.OutputValue_y));
        }

        private void PhaseModelingEvent(object sender, ModelingEventArgs e)
        {

        }
    }
}
