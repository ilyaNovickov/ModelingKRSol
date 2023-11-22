using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Net.Mime.MediaTypeNames;

namespace ModelingKRProj
{
    public class Class1
    {
        public struct PointD
        {
            public PointD(double x, double y)
            {
                X = x;
                Y = y;
                Value = 0;
            }

            public double X { get; set; }
            public double Y { get; set; }
            public double Value { get; set; }

            public static PointD operator +(PointD p1, PointD p2)
            {
                return new PointD(p1.X+ p2.X, p1.Y + p2.Y);
            }
            public static PointD operator -(PointD p1, PointD p2)
            {
                return new PointD(p1.X - p2.X, p1.Y - p2.Y);
            }
            public static PointD operator *(PointD p1, PointD p2)
            {
                return new PointD(p1.X * p2.X, p1.Y * p2.Y);
            }
            public static PointD operator /(PointD p1, PointD p2)
            {
                return new PointD(p1.X / p2.X, p1.Y / p2.Y);
            }
            public static PointD operator *(PointD p1, double p2)
            {
                return new PointD(p1.X * p2, p1.Y * p2);
            }
            public static PointD operator *(double p2 , PointD p1)
            {
                return new PointD(p1.X * p2, p1.Y * p2);
            }
            public static PointD operator /(PointD p1, double p2)
            {
                return new PointD(p1.X / p2, p1.Y / p2);
            }
        }

        public class OptimizingValue
        {
            public double Value { get; set; }
            public static double MaxValue { get; set; }
            public static double MinValue { get; set; }
        }
        public ModelingSystem OptimizingSystem { get; set; }

        public Func<ModelingSystem, double> Func { get; set; }

        public OptimizingValue[] optimizingValues { get; set; }

        public void Opt()
        {
            List<PointD> points = new List<PointD>(3)
            {
                new PointD(OptimizingSystem.Pvalue, OptimizingSystem.Ivalue),
                new PointD(OptimizingSystem.Pvalue + 0.1, OptimizingSystem.Ivalue),
                new PointD(OptimizingSystem.Pvalue, OptimizingSystem.Ivalue + 0.1)
                //new PointD(OptimizingValue.MinValue, OptimizingValue.MinValue),
                //new PointD(OptimizingValue.MinValue, OptimizingValue.MaxValue),
                //new PointD(OptimizingValue.MaxValue, OptimizingValue.MinValue)
            };


            int iterations = 0;

            //while (iterations <= 100)
            while (GetDispersion(points) > 0.01d)
            {
                for (int i = 0; i < points.Count; i++)
                {
                    OptimizingSystem.Pvalue = points[i].X;
                    OptimizingSystem.Ivalue = points[i].Y;

                    PointD newPoint = points[i];
                    newPoint.Value = this.Func(OptimizingSystem);
                    points[i] = newPoint;
                }

                BubbleSort(points);

                PointD mid = new PointD((points[0].X + points[1].X) / 2d, (
                    points[0].Y + points[1].Y) / 2d);

                PointD xr = mid + (mid - points[2]);
                OptimizingSystem.Pvalue = xr.X;
                OptimizingSystem.Ivalue = xr.Y;
                xr.Value = this.Func(OptimizingSystem);

                if (xr.Value < points[0].Value)
                {
                    PointD e = (1d - 2d) * mid + 2d * xr;
                    OptimizingSystem.Pvalue = e.X;
                    OptimizingSystem.Ivalue = e.Y;
                    e.Value = Func(OptimizingSystem);

                    if (e.Value < xr.Value)
                        points[2] = e;
                    else
                        points[2] = xr;
                }
                else if (xr.Value < points[1].Value)
                {
                    points[2] = xr;
                }
                else
                {
                    if (xr.Value < points[2].Value)
                    {
                        points[2] = xr;
                    }
                    
                    BubbleSort(points);

                    PointD s = 0.5d * points[2] + (1 - 0.5) * mid;
                    OptimizingSystem.Pvalue = s.X;
                    OptimizingSystem.Ivalue = s.Y;
                    s.Value = Func(OptimizingSystem);

                    if (s.Value < points[2].Value)
                        points[2] = s;
                    else
                    {
                        PointD b = points[0];
                        for (int i = 0; i < points.Count; i++)
                        {
                            PointD newPoint = b + (points[i] - b) / 2;
                            newPoint.Value = Func(OptimizingSystem);
                            points[i] = newPoint;
                        }
                    }
                }

                iterations++;
            }

        }
        private static double GetDispersion(List<PointD> list)
        {
            // для определения погрешности определяется дисперсия разброса точек
            int argsLength = 2;
            //Математическое ожидание
            double[] MatOj = new double[argsLength];
            for (int i = 0; i < argsLength; i++)
            {
                for (int j = 0; j < list.Count; j++)
                {
                    MatOj[i] += list[j].X;
                    MatOj[i] += list[j].Y;
                }
                MatOj[i] = MatOj[i] / list.Count;
            }
            double res = 0;
            for (int i = 0; i < list.Count; i++)
            {
                //расстояние от мат. ожидания до каждой точки
                double tmp = 0;
                for (int j = 0; j < argsLength; j++)
                {
                    tmp += Math.Pow(MatOj[j] - list[i].X, 2);
                    tmp += Math.Pow(MatOj[j] - list[i].Y, 2);
                }
                res += Math.Sqrt(tmp);
            }
            return res / list.Count;
        }
        private static void BubbleSort(List<PointD> collection)
        {
            int size = collection.Count;
            PointD temp;
            for (int i = 0; i < size; i++)
            {
                for (int j = i + 1; j < size; j++)
                {
                    if (collection[i].Value > collection[j].Value)
                    {
                        temp = collection[i];
                        collection[i] = collection[j];
                        collection[j] = temp;
                    }
                }
            }
        }
        
    }
}
