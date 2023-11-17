using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingKRProj
{
    public class ModelingSystem
    {
        #region Vars
        private double step = 1;
        private double timeofRegulation = 20;
        private double gainCooficient = 1;
        private double constofTime = 1;
        private double timeofDelay = 1;
        private double cooficientP = 1;
        private double cooficientI = 1;
        private double cooficientD = 1;
        #endregion
        #region Propers
        [Category("Моделирование")]
        [Description("Шаг моделирования h")]
        [DisplayName("Шаг моделирования h")]
        //[TypeConverter(typeof(Class2))]
        public double StepofModeling
        {
            get => step;
            set
            {
                if (value <= 0)
                    throw new Exception("Шаг не может быть равень 0 или отрицательным числом");
                step = value;
            }
        }
        [Category("Моделирование")]
        [Description("Время моделирования tрег")]
        [DisplayName("Время моделирования tрег")]
        public double TimeofRegulation
        {
            get => timeofRegulation;
            set
            {
                if (value <= 0)
                    throw new Exception("Время моделирования не может быть равень 0 или отрицательным числом");
                timeofRegulation = value;
            }
        }
        [Category("Значения системы")]
        [Description("Помеха f")]
        [DisplayName("Помеха f")]
        public double Noise
        {
            get; set;
        }
        [Category("Значения системы")]
        [Description("Входное значение g")]
        [DisplayName("Входное значение g")]
        public double InputValue
        {
            get; set;
        }
        [Category("Значения звеньев")]
        [Description("Коофициент усиления k")]
        [DisplayName("Коофициент усиления k")]
        public double GainCooficient
        {
            get => gainCooficient;
            set
            {
                if (value < 0)
                    throw new Exception("Коофициент уселения не может быть отрицательным");
                gainCooficient = value;
            }
        }
        [Category("Значения звеньев")]
        [Description("Постоянная времени T")]
        [DisplayName("Постоянная времени T")]
        public double TimeConst
        {
            get => constofTime;
            set
            {
                if (value < 0)
                    throw new Exception("Постоянная времени не может быть отрицательным");
                constofTime = value;
            }
        }
        [Category("Значения звеньев")]
        [Description("Время запаздывания tau")]
        [DisplayName("Время запаздывания tau")]
        public double TimeofDelay
        {
            get => timeofDelay;
            set
            {
                if (value < 0)
                    throw new Exception("Время запаздывания не может быть отрицательным");
                timeofDelay = value;
            }
        }
        [Category("Значения ПИ-регулятора")]
        [Description("Коофициент П kp")]
        [DisplayName("Коофициент П kp")]
        public double Pvalue
        {
            get => cooficientP;
            set
            {
                if (value < 0)
                    throw new Exception("Коофициент П не может быть отрицательным");
                cooficientP = value;
            }
        }
        [Category("Значения ПИ-регулятора")]
        [Description("Коофициент И ki")]
        [DisplayName("Коофициент И ki")]
        public double Ivalue
        {
            get => cooficientI;
            set
            {
                if (value < 0)
                    throw new Exception("Коофициент И не может быть отрицательным");
                cooficientI = value;
            }
        }
        //[Category("Значения ПИД-регулятора")]
        //[Description("Коофициент Д kd")]
        //[DisplayName("Коофициент Д kd")]
        //public double Dvalue
        //{
        //    get => cooficientD;
        //    set
        //    {
        //        if (value <= 0)
        //            throw new Exception("Коофициент Д не может быть отрицательным");
        //        cooficientD = value;
        //    }
        //}
        #endregion
        #region Events
        public EventHandler<ModelingEventArgs> ModelingEvent;
        #endregion
        #region Methods
        public void InvokeModeling()
        {
            //Выход системы
            double yx = 0;
            double y = 0;

            double I = 0;

            int n1 = (int)(TimeofDelay / StepofModeling);
            double[] yp = new double[n1];

            int i = 0;

            double c1 = GainCooficient / TimeConst;
            double c2 = -1 / TimeConst;

            double x1 = 0;

            double time = 0;

            do
            {
                double xp = x1;

                x1 = InputValue - y;

                double x2 = xp == 0 ? 0 : (x1 - xp) / StepofModeling;

                xp = x1;

                I = I + ((x1 + xp) / 2) * StepofModeling;

                double u = Pvalue * x1 + Ivalue * I;

                double xt = Noise + u;

                double y1 = c1 * xt + c2 * yx;

                yx = yx + y1 * StepofModeling;

                y = yp[i];

                yp[i] = yx;

                i++;
                if (i >= n1)
                    i = 0;

                ModelingEvent?.Invoke(this, new ModelingEventArgs(this, time, y, x1, x2));

                time = time + StepofModeling;
            }
            while (time < TimeofRegulation);

            /*
            ModelingEvent?.Invoke(this, new ModelingEventArgs(this, time, y, x1, 0));
            */
        }
        #endregion
    }
}
