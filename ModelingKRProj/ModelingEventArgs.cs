using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingKRProj
{
    public class ModelingEventArgs : EventArgs
    {
        #region Constr
        public ModelingEventArgs(ModelingSystem system, double t, double y, double x1, double x2)
        {
            this.CurrentTime_t = t;
            this.OutputValue_y = y;
            this.FistError_x1 = x1;
            this.DiffofError_x2 = x2;

            this.StepofModeling = system.StepofModeling;
            this.TimeofRegulation = system.TimeofRegulation;
            this.Noise = system.Noise;
            this.InputValue = system.InputValue;
            this.GainCooficient = system.GainCooficient;
            this.TimeConst = system.TimeConst;
            this.TimeofDelay = system.TimeofDelay;
            this.Pvalue = system.Pvalue;
            this.Ivalue = system.Ivalue;
            //this.Dvalue = system.Dvalue;
        }
        #endregion
        #region Propers
        public double CurrentTime_t
        {
            get; private set;
        }
        public double OutputValue_y
        {
            get; private set;
        }
        public double FistError_x1
        {
            get; private set;
        }
        public double DiffofError_x2
        {
            get; private set;
        }
        #region ModelingSystemPropers
        public double StepofModeling
        {
            get;
            private set;
        }
        public double TimeofRegulation
        {
            get;
            private set;
        }
        public double Noise
        {
            get; set;
        }
        public double InputValue
        {
            get; set;
        }

        public double GainCooficient
        {
            get;
            private set;
        }

        public double TimeConst
        {
            get;
            private set;
        }

        public double TimeofDelay
        {
            get;
            private set;
        }

        public double Pvalue
        {
            get;
            private set;
        }
        public double Ivalue
        {
            get;
            private set;
        }

        //public double Dvalue
        //{
        //    get;
        //    private set;
        //}
        #endregion
        #endregion
    }
}
