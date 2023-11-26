using System;
using System.Collections.Generic;

namespace ModelingKRProj
{
    /// <summary>
    /// Оптимизатор для ПИ-регулятора
    /// </summary>
    public class Optimizator2D
    {
        /// <summary>
        /// Структура точки, которая хранит свои координаты
        /// как тип double,
        /// а также значение целевой функции
        /// </summary>
        public struct PointD
        {
            /// <summary>
            /// Иницилизирует точку
            /// </summary>
            /// <param name="x">Координата X</param>
            /// <param name="y">Координата Y</param>
            public PointD(double x, double y)
            {
                X = x;
                Y = y;
                FuncValue = 0;
            }
            /// <summary>
            /// Координата X
            /// </summary>
            public double X { get; set; }
            /// <summary>
            /// Координата Y
            /// </summary>
            public double Y { get; set; }
            /// <summary>
            /// Значение целевой функции
            /// </summary>
            public double FuncValue { get; set; }
            //Переопределённые операторы для структуры
            //... для двух точек
            public static PointD operator +(PointD left, PointD right)
            {
                return new PointD(left.X + right.X, left.Y + right.Y);
            }
            public static PointD operator -(PointD left, PointD right)
            {
                return new PointD(left.X - right.X, left.Y - right.Y);
            }
            public static PointD operator *(PointD left, PointD right)
            {
                return new PointD(left.X * right.X, left.Y * right.Y);
            }
            public static PointD operator /(PointD left, PointD right)
            {
                return new PointD(left.X / right.X, left.Y / right.Y);
            }
            //... для точки и некотого коофициента
            public static PointD operator *(PointD left, double right)
            {
                return new PointD(left.X * right, left.Y * right);
            }
            public static PointD operator /(PointD left, double right)
            {
                return new PointD(left.X / right, left.Y / right);
            }
            public static PointD operator *(double left, PointD right)
            {
                return new PointD(right.X * left, right.Y * left);
            }

        }
        /// <summary>
        /// Оптимизируемая САУ с ПИ-регулятором
        /// </summary>
        public ModelingSystem OptimizingSystem { get; set; }
        /// <summary>
        /// Целевая функция, по которой происходит минимизация
        /// </summary>
        public Func<ModelingSystem, double> Func { get; set; }
        /// <summary>
        /// Значение остановки алгоритма
        /// </summary>
        public double StopValue { get; set; } = 0.01d;
        /// <summary>
        /// Значение разброса начальных точек симплекса
        /// </summary>
        public double StartSpread { get; set; } = 0.1d;

        public void NelderMidOptimization()
        {
            //Поулчение копии оптимизируемой системы
            //чтобы не вызывать обработчики событий
            ModelingSystem clone = OptimizingSystem.CloneSystem();
            //Получение значения целевой функции для точки
            void GetFuncValue(ref PointD testingPoint)
            {
                clone.Pvalue = testingPoint.X;
                clone.Ivalue = testingPoint.Y;
                testingPoint.FuncValue = Func(clone);
            }
            //Сравнение двух точек по их целевой функции
            int ComparePointsByValues(PointD p1, PointD p2)
            {
                //Точки...
                if (p1.FuncValue == p2.FuncValue)
                    return 0;//... равны
                else if (p1.FuncValue < p2.FuncValue)
                    return -1;//... меньше
                else
                    return 1;//... больше
            }
            //Значение коофициентов
            //отражения
            //сжатия
            // и растяжения
            double reflactionCoof = 1d;
            double compressionCoof = 2d;
            double stretchingCoof = 0.5d;
            //Список сортируемых точек
            List<PointD> points = new List<PointD>(3);
            //Получение начальных точек симплекса
            for (int i = 0; i < 3; i++)
            {
                PointD point;
                switch (i)
                {
                    default:
                    case 0:
                        point = new PointD(OptimizingSystem.Pvalue, OptimizingSystem.Ivalue);
                        break;
                    case 1:
                        point = new PointD(OptimizingSystem.Pvalue + StartSpread, OptimizingSystem.Ivalue);
                        break;
                    case 2:
                        point = new PointD(OptimizingSystem.Pvalue, OptimizingSystem.Ivalue + StartSpread);
                        break;
                }
                GetFuncValue(ref point);
                points.Add(point);
            }
            //Поиск оптимальных значений
            while (GetDispersion(points) > this.StopValue)
            {
                points.Sort(ComparePointsByValues);
                PointD bestPoint = points[0];
                PointD goodPoint = points[1];
                PointD worstPoint = points[2];
                //Получение центра тяжести
                PointD middle = (bestPoint + goodPoint) / 2;
                GetFuncValue(ref middle);
                //Получение отражённой точки
                PointD reflectionPoint = (1 + reflactionCoof) * middle - reflactionCoof * worstPoint;
                GetFuncValue(ref reflectionPoint);

                if (reflectionPoint.FuncValue < bestPoint.FuncValue)
                {
                    //Точки очень хороша
                    //Следовательно, применить растяжение
                    PointD stretchingPoint = (1 - stretchingCoof) * middle + stretchingCoof * reflectionPoint;
                    GetFuncValue(ref stretchingPoint);

                    if (stretchingPoint.FuncValue < reflectionPoint.FuncValue)
                        points[2] = stretchingPoint;
                    else
                        points[2] = reflectionPoint;
                }
                else if (reflectionPoint.FuncValue < goodPoint.FuncValue)
                {
                    //Точка неплоха
                    points[2] = reflectionPoint;
                }
                else
                {
                    //Точка плоха
                    //Попытка сжятия 
                    if (reflectionPoint.FuncValue < worstPoint.FuncValue)
                    {
                        worstPoint = reflectionPoint;
                        points[2] = worstPoint;
                    }
                    points.Sort(ComparePointsByValues);

                    bestPoint = points[0];
                    worstPoint = points[2];

                    PointD comprassionPoint = compressionCoof * worstPoint + (1 - compressionCoof) * middle;
                    GetFuncValue(ref comprassionPoint);

                    if (comprassionPoint.FuncValue < worstPoint.FuncValue)
                        points[2] = comprassionPoint;
                    else
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            PointD point = bestPoint + (points[i] - bestPoint) / 2;
                            GetFuncValue(ref point);
                            points[i] = point;
                        }
                    }
                }
                //Установка оптимизированных значений ПИ-регулятора
                points.Sort(ComparePointsByValues);

                OptimizingSystem.Pvalue = points[0].X;
                OptimizingSystem.Ivalue = points[0].Y;
            }
        }
        /// <summary>
        /// Получение разброса (дисперсии) точек
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        private double GetDispersion(List<PointD> points)
        {
            //Получение матиматического ожидания для 2d пространства
            double[] mathExpection = new double[2];

            for (int i = 0; i < points.Count; i++)
            {
                mathExpection[0] += points[i].X;
                mathExpection[1] += points[i].Y;
            }

            mathExpection[0] = mathExpection[0] / points.Count;
            mathExpection[1] = mathExpection[1] / points.Count;

            //Получение дисперсии
            double res0 = 0;
            for (int i = 0; i < points.Count; i++)
            {
                double temp = 0;

                temp += Math.Pow(mathExpection[0] - points[i].X, 2);
                temp += Math.Pow(mathExpection[1] - points[i].Y, 2);

                res0 += Math.Sqrt(temp);
            }

            //double res1 = res0 / points.Count;
            return res0 / points.Count;


            //Альтернативныый поиск
            //double res = 0;
            //for (int i = 0; i < points.Count; i++)
            //{
            //    double temp = 0;
            //    temp += Math.Pow(mathExpection[0] - points[i].X, 2);
            //    temp += Math.Pow(mathExpection[1] - points[i].Y, 2);

            //    res += temp;
            //}

            ////double res3 = Math.Sqrt(res / points.Count);

            //return Math.Sqrt(res / points.Count);
        }
    }
}
