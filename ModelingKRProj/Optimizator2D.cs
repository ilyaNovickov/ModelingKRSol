using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Net.Mime.MediaTypeNames;

namespace ModelingKRProj
{
    public class Optimizator2D
    {
        public struct PointD
        {
            public PointD(double x, double y)
            {
                X = x;
                Y = y;
                FuncValue = 0;
            }

            public double X { get; set; }
            public double Y { get; set; }
            public double FuncValue { get; set; }

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

        public ModelingSystem OptimizingSystem { get; set; }
        public Func<ModelingSystem, double> Func { get; set; }
        public double StopValue { get; set; } = 0.01d;
        public double StartSpread { get; set; } = 0.1d;

        public void Optm()
        {
            ModelingSystem clone = OptimizingSystem.CloneSystem();

            void GetFuncValue(ref PointD testingPoint)
            {
                clone.Pvalue = testingPoint.X;
                clone.Ivalue = testingPoint.Y;
                testingPoint.FuncValue = Func(clone);
            }
            int ComparePointsByValues(PointD p1, PointD p2)
            {
                if (p1.FuncValue == p2.FuncValue)
                    return 0;
                else if (p1.FuncValue < p2.FuncValue)
                    return -1;
                else
                    return 1;
            }

            double reflactionCoof = 1d;
            double compressionCoof = 2d;
            double stretchingCoof = 0.5d;

            List<PointD> points = new List<PointD>(3);

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

            while (GetDispersion(points) > this.StopValue)
            {
                points.Sort(ComparePointsByValues);
                PointD bestPoint = points[0];
                PointD goodPoint = points[1];
                PointD worstPoint = points[2];

                PointD middle = (bestPoint + goodPoint) / 2;
                GetFuncValue(ref middle);

                PointD reflectionPoint = (1 + reflactionCoof) * middle - reflactionCoof * worstPoint;
                GetFuncValue(ref reflectionPoint);

                if (reflectionPoint.FuncValue < bestPoint.FuncValue)
                {
                    PointD stretchingPoint = (1 - stretchingCoof) * middle + stretchingCoof * reflectionPoint;
                    GetFuncValue(ref stretchingPoint);

                    if (stretchingPoint.FuncValue < reflectionPoint.FuncValue)
                        points[2] = stretchingPoint;
                    else
                        points[2] = reflectionPoint;
                }
                else if (reflectionPoint.FuncValue < goodPoint.FuncValue)
                {
                    points[2] = reflectionPoint;
                }
                else
                {
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

                points.Sort(ComparePointsByValues);

                OptimizingSystem.Pvalue = points[0].X;
                OptimizingSystem.Ivalue = points[0].Y;
            }
        }

        private double GetDispersion(List<PointD> points)
        {
            double[] mathExpection = new double[2];

            for (int i = 0; i < points.Count; i++)
            {
                mathExpection[0] += points[i].X;
                mathExpection[1] += points[i].Y;
            }

            mathExpection[0] = mathExpection[0] / points.Count;
            mathExpection[1] = mathExpection[1] / points.Count;

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
