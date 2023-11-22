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

            system = new ModelingSystem()
            {
                StepofModeling = 0.01d,
                InputValue = 1d,
                GainCooficient = 4.9d,
                TimeConst = 4.8d,
                TimeofDelay = 4.8d,
                Pvalue = 0.2d,
                Ivalue = 0.02d
            };
            system.ModelingEvent += ModelingEvent;

            propertyGrid.SelectedObject = system;

            propertyGrid.Refresh();

            modelingSeries = new Series(nameof(modelingSeries))
            {
                ChartType = SeriesChartType.Line,
                Color = Color.Blue
            };

            this.modelingChart.Series.Add(modelingSeries);

            phaseSeries = new Series(nameof(phaseSeries))
            {
                ChartType = SeriesChartType.Line,
                Color = Color.Red
        };

            this.phaseChart.Series.Add(phaseSeries);
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            modelingChart.Invalidate();
            phaseChart.Invalidate();

            modelingSeries.Points.Clear();
            phaseSeries.Points.Clear();

            system.InvokeModeling();

            modelingChart.Update();
            phaseChart.Update();

            propertyGrid.Refresh();
        }

        private void ModelingEvent(object sender, ModelingEventArgs e)
        {
            modelingSeries.Points.Add(new DataPoint(e.CurrentTime_t, e.OutputValue_y));
            phaseSeries.Points.Add(new DataPoint(e.FistError_x1, e.DiffofError_x2));
        }

        private void optimiationButton_Click(object sender, EventArgs e)
        {


            //Class1.OptimizingValue.MinValue = 0d;
            //Class1.OptimizingValue.MaxValue = 10d;

            //Class1 class1 = new Class1();

            //class1.Func = (system) => { system.InvokeModeling(); return system.ISE; };
            //class1.OptimizingSystem = system;

            //class1.Opt();

            Optimization opt = new Optimization();

            opt.Args = new double[] { system.Pvalue, system.Ivalue };
            opt.F = (doubles) =>
            {
                system.Pvalue = doubles[0];
                system.Ivalue = doubles[1];
                system.InvokeModeling();
                return system.ISE;
            };
            opt.GetResultOptByNM();
            system.Pvalue = opt.Args[0];
            system.Ivalue = opt.Args[1];
            system.InvokeModeling();
        }
    }
}
