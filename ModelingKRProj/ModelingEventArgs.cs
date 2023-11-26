using System;

namespace ModelingKRProj
{
    /// <summary>
    /// Вспомогательный класс с информацией о событии моделирования
    /// </summary>
    public class ModelingEventArgs : EventArgs
    {
        #region Constr
        /// <summary>
        /// Иницилизирует экзеземпляр класса ModelingEventArgs
        /// </summary>
        /// <param name="system">САУ, которая вызвала событие</param>
        /// <param name="t">Текущий шаг моделирования (квант времени)</param>
        /// <param name="y">Выходное значение</param>
        /// <param name="x1">Значение ошибки</param>
        /// <param name="x2">Производная от ошибки x1</param>
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
        }
        #endregion
        #region Propers
        /// <summary>
        /// Текущей шаг моделирования (квант времени)
        /// </summary>
        public double CurrentTime_t
        {
            get; private set;
        }
        /// <summary>
        /// Текущее выходное значение
        /// </summary>
        public double OutputValue_y
        {
            get; private set;
        }
        /// <summary>
        /// Значение ошибки
        /// </summary>
        public double FistError_x1
        {
            get; private set;
        }
        /// <summary>
        /// Производная ошибки x1
        /// </summary>
        public double DiffofError_x2
        {
            get; private set;
        }
        #region ModelingSystemPropers
        /// <summary>
        /// Шаг моделирования ht
        /// </summary>
        public double StepofModeling
        {
            get;
            private set;
        }
        /// <summary>
        /// Время моделирования системы tрег
        /// </summary>
        public double TimeofRegulation
        {
            get;
            private set;
        }
        /// <summary>
        /// Входной шум системы f 
        /// </summary>
        public double Noise
        {
            get; set;
        }
        /// <summary>
        /// Входное значение ys
        /// </summary>
        public double InputValue
        {
            get; set;
        }
        /// <summary>
        /// Коофициент усиление инерционного звена k
        /// </summary>
        public double GainCooficient
        {
            get;
            private set;
        }
        /// <summary>
        /// Постоянная времени инерционного звена T
        /// </summary>
        public double TimeConst
        {
            get;
            private set;
        }
        /// <summary>
        /// Время запаздывания звена запаздывания tau
        /// </summary>
        public double TimeofDelay
        {
            get;
            private set;
        }
        /// <summary>
        /// Коофициент П ПИ-регулятора kp
        /// </summary>
        public double Pvalue
        {
            get;
            private set;
        }
        /// <summary>
        /// Коофициент И ПИ-регулятора ki
        /// </summary>
        public double Ivalue
        {
            get;
            private set;
        }
        #endregion
        #endregion
    }
}
