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
            double stretching = 0.5d;

            

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

            while (true)
            {
                points.Sort(ComparePointsByValues);
                PointD bestPoint = points[0];
                PointD goodPoint = points[1];
                PointD worstPoint = points[2];

                PointD middle = (bestPoint + goodPoint) / 2;
                GetFuncValue(ref middle);



            }
        }
    }
}
