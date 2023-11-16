using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingKRProj
{
    public class ModelingSystem
    {
        #region Vars
        private double step;
        private double timeofRegulation;
        private double gainCooficient;
        private double constofTime;
        private double timeofDelay;
        private double cooficientP;
        private double cooficientI;
        private double cooficientD;
        #endregion
        #region Propers
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
            get => gainCooficient;
            set
            {
                if (value <= 0)
                    throw new Exception("Коофициент уселения не может быть отрицательным");
                gainCooficient = value;
            }
        }

        public double TimeConst
        {
            get => constofTime;
            set
            {
                if (value <= 0)
                    throw new Exception("Постоянная времени не может быть отрицательным");
                constofTime = value;
            }
        }

        public double TimeofDelay
        {
            get => timeofDelay;
            set
            {
                if (value <= 0)
                    throw new Exception("Время запаздывания не может быть отрицательным");
                timeofDelay = value;
            }
        }

        public double Pvalue
        {
            get => cooficientP;
            set
            {
                if (value <= 0)
                    throw new Exception("Коофициент П не может быть отрицательным");
                cooficientP = value;
            }
        }
        public double Ivalue
        {
            get => cooficientI;
            set
            {
                if (value <= 0)
                    throw new Exception("Коофициент И не может быть отрицательным");
                cooficientI = value;
            }
        }
        public double Dvalue
        {
            get => cooficientD;
            set
            {
                if (value <= 0)
                    throw new Exception("Коофициент Д не может быть отрицательным");
                cooficientD = value;
            }
        }
        #endregion
        public void InvokeModeling()
        {
            double xp = 0;
            double y = 0;
            double yx = 0;
            double xt = 0;
            double I = 0;
            int n1 = (int)(TimeofDelay / StepofModeling);
            double[] yp = new double[n1];

            int i = 0;
            int p = 0;
            int pos = 0;

            double c1 = GainCooficient / TimeConst;
            double c2 = -1 / TimeConst;

            double time = 0;

            do
            {
                double x1 = InputValue - y;
                double x2 = (x1 - xp) / StepofModeling;
                xp = x1;
                I = I + ((x1 + xp) / 2) * StepofModeling;
                double u = Pvalue * x1 + Ivalue * I;
                xt = Noise + u;
                double y1 = c1 * xt + c2 * yx;
                yx = yx + y1 * StepofModeling;
                y = yp[i];
                yp[i] = yx;
                i++;
                if (i >= n1)
                    i = 0;
                if (p >= 0)
                {
                    //output
                    pos++;
                }
                time = time + StepofModeling;
                p++;
            }
            while (time < TimeofRegulation);
        }
    }
}
