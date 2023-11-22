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
                new PointD(OptimizingValue.MinValue, OptimizingValue.MinValue),
                new PointD(OptimizingValue.MinValue, OptimizingValue.MaxValue),
                new PointD(OptimizingValue.MaxValue, OptimizingValue.MinValue)
            };


            int iterations = 0;

            while (iterations <= 100)
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



                iterations++;
            }

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
