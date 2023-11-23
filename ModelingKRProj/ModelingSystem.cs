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
        //Значения системы
        private double step = 1;
        private double timeofModeling = 20;
        private double gainCooficient = 1;
        private double constofTime = 1;
        private double timeofDelay = 1;
        private double cooficientP = 1;
        private double cooficientI = 1;
        #endregion
        #region Propers
        /// <summary>
        /// Шаг моделирования ht
        /// </summary>
        [Category("Моделирование")]
        [Description("Шаг моделирования h")]
        [DisplayName("Шаг моделирования h")]
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
        /// <summary>
        /// Время моделирования системы tрег
        /// </summary>
        [Category("Моделирование")]
        [Description("Время моделирования tрег")]
        [DisplayName("Время моделирования tрег")]
        public double TimeofModeling
        {
            get => timeofModeling;
            set
            {
                if (value <= 0)
                    throw new Exception("Время моделирования не может быть равень 0 или отрицательным числом");
                timeofModeling = value;
            }
        }
        /// <summary>
        /// Входной шум системы f 
        /// </summary>
        [Category("Значения системы")]
        [Description("Помеха f")]
        [DisplayName("Помеха f")]
        public double Noise
        {
            get; set;
        }
        /// <summary>
        /// Входное значение ys
        /// </summary>
        [Category("Значения системы")]
        [Description("Входное значение ys")]
        [DisplayName("Входное значение ys")]
        public double InputValue
        {
            get; set;
        }
        /// <summary>
        /// Коофициент усиление инерционного звена k
        /// </summary>
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
        /// <summary>
        /// Постоянная времени инерционного звена T
        /// </summary>
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
        /// <summary>
        /// Время запаздывания звена запаздывания tau
        /// </summary>
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
        /// <summary>
        /// Коофициент П ПИ-регулятора kp
        /// </summary>
        [Category("Значения ПИ-регулятора")]
        [Description("Коофициент П kp")]
        [DisplayName("Коофициент П kp")]
        public double Pvalue
        {
            get => cooficientP;
            set
            {
                //if (value < 0)
                //    throw new Exception("Коофициент П не может быть отрицательным");
                cooficientP = value;
            }
        }
        /// <summary>
        /// Коофициент И ПИ-регулятора ki
        /// </summary>
        [Category("Значения ПИ-регулятора")]
        [Description("Коофициент И ki")]
        [DisplayName("Коофициент И ki")]
        public double Ivalue
        {
            get => cooficientI;
            set
            {
                //if (value < 0)
                //    throw new Exception("Коофициент И не может быть отрицательным");
                cooficientI = value;
            }
        }
        /// <summary>
        /// ISE критерий системы
        /// </summary>
        [Category("Критерии оптимизации")]
        public double ISE { get; private set; }
        /// <summary>
        /// IAE критерий системы
        /// </summary>
        [Category("Критерии оптимизации")]
        public double IAE { get; private set; }
        /// <summary>
        /// ITAE критерий системы
        /// </summary>
        [Category("Критерии оптимизации")]
        public double ITAE { get; private set; }
        /// <summary>
        /// ITSE критерий системы
        /// </summary>
        [Category("Критерии оптимизации")]
        public double ITSE { get; private set; }
        /// <summary>
        /// Перерегулирование системы sigma
        /// </summary>
        [Category("Характеристики САУ")]
        [Description("Перерегулирование")]
        [DisplayName("Перерегулирование")]
        public double Overregulation { get; private set; }
        /// <summary>
        /// Колебательность системы psi
        /// </summary>
        [Category("Характеристики САУ")]
        [Description("Колебательность")]
        [DisplayName("Колебательность")]
        public double Oscillation { get; private set; }
        /// <summary>
        /// Время регулирования системы tпер
        /// </summary>
        [Category("Характеристики САУ")]
        [Description("Время регулирования")]
        [DisplayName("Время регулирования")]
        public double TimeofRegulation { get; private set; }
        #endregion
        #region Events
        /// <summary>
        /// Событие моделирования системы
        /// </summary>
        public EventHandler<ModelingEventArgs> ModelingEvent;
        #endregion
        #region Methods
        /// <summary>
        /// Вызов моделирования системы
        /// </summary>
        public void InvokeModeling()
        {
            EnzeroValues();//Установка определнных свойств системы на "0"

            //Выход системы
            double yx = 0;
            double y = 0;

            //Интеграл для интегрального звена ПИ-регулятора
            double I = 0;

            //Массив для моделирования запаздывания
            int n1 = (int)(TimeofDelay / StepofModeling);
            double[] yp = new double[n1];

            int i = 0;

            //Вспомогательные коофициенты для уравнения Эйлера
            double c1 = GainCooficient / TimeConst;
            double c2 = -1 / TimeConst;

            double x1 = 0;//Значение ошибки

            //Определяет значение в которое должно установиться модель +=5%
            double autorErr = Math.Abs(InputValue) * 0.05;
            bool isInf = false;//Бесконечно ли моделирование?
            double max1 = 0;//Амплитуда 1
            double max2 = 0;//Амплитуда 2
            double xPast = 0;//Прошлое значение x
            double xPastPast = 0;//Позапрошлое значение x

            //Квант времени
            double time = 0;

            do//Моделирование системы
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

                if (n1 != 0)
                {
                    y = yp[i];

                    yp[i] = yx;

                    i++;
                    if (i >= n1)
                        i = 0;
                }
                else
                {
                    y = yx;
                }
                //Вызов события моделирования
                ModelingEvent?.Invoke(this, new ModelingEventArgs(this, time, y, x1, x2));

                time = time + StepofModeling;

                //Определение времени регулирования
                if (autorErr > Math.Abs(x1) && isInf == true)
                {
                    isInf = false;
                    TimeofRegulation = time;
                }
                else if (autorErr < Math.Abs(x1))
                {
                    isInf = true;
                    TimeofRegulation = 0;
                }

                //Определение коофициента затухания
                if (xPastPast < xPast && xPast > y)
                {
                    if (max1 == 0)
                        max1 = xPast;
                    else if (max2 == 0)
                        max2 = xPast;
                }

                xPastPast = xPast;
                xPast = y;

                //Подсчёт значений для оптимизации
                ISE = ISE + Math.Pow(x1, 2d) * StepofModeling;
                IAE = IAE + Math.Abs(x1) * StepofModeling;
                ITAE = ITAE + Math.Abs(x1) * time * StepofModeling;
                ITSE = ITSE + Math.Pow(x1, 2d) * time * StepofModeling;
            }
            while (time < TimeofModeling);//Моделируем до указанного времени

            //Доп определение качества системы
            if (TimeofRegulation != 0)
            {
                if (max1 > y)
                    Overregulation = (max1 - y) * 100 / y;
                if (max1 > y && max2 > y)
                    Oscillation = Math.Round((max2 - y) / (max1 - y) * 100, 2);
            }
            else
            {
                Overregulation = TimeofRegulation > 0 ? Overregulation : 0;
                Oscillation = max2 / max1;
            }
        }

        /// <summary>
        /// Очистка от значений определённых свойств системы
        /// </summary>
        private void EnzeroValues()
        {
            ISE = 0;
            IAE = 0;
            ITAE = 0;
            ITSE = 0;
            Overregulation = 0;
            Oscillation = 0;
            TimeofRegulation = 0;
        }

        /// <summary>
        /// Клонирование системы без копирования обработчика событий
        /// </summary>
        /// <returns></returns>
        public ModelingSystem CloneSystem()
        {
            ModelingSystem clone = new ModelingSystem()
            {
                GainCooficient = this.GainCooficient,
                TimeConst = this.TimeConst,
                TimeofDelay = this.TimeofDelay,
                Pvalue = this.Pvalue,
                Ivalue = this.Ivalue,
                TimeofModeling = this.TimeofModeling,
                StepofModeling = this.StepofModeling,
                InputValue = this.InputValue,
                Noise = this.Noise,
                ISE = this.ISE,
                IAE = this.IAE,
                ITAE = this.ITAE,
                ITSE = this.ITSE,
                Overregulation = this.Overregulation,
                Oscillation = this.Oscillation,
                TimeofRegulation = this.TimeofRegulation,
            };
            return clone;
        }
        #endregion
    }
}
