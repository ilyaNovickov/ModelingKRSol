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
    public class Opt
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

    class Optimization
    {
        public void GetResultOptByNM()
        {
            #region 1
            double reflKoef = 1;//коэффициент отражения
            double strKoef = 2;//коэффициент сжатия
            double comprKoef = 0.5;//коэффициент растяжения
            List<NMPoint> preferList = new List<NMPoint>(Args.Length);//объяление списка для сортировки точек
                                                                      // определение начального положения вершин симплекса
            for (int i = 0; i <= Args.Length; i++)
            {
                double[] argArr = new double[Args.Length];
                for (int j = 0; j < Args.Length; j++)
                {
                    argArr[j] = Args[j];
                    if (i == j && i != Args.Length)
                        argArr[j] += H;
                }
                preferList.Add(new NMPoint(argArr));
            }
            int lastpreferListIndx = preferList.Count - 1;

            //вычисление значений функций отклика точек
            foreach (NMPoint p in preferList)
                p.F = F(p.Args);
            //Последний шаг — проверка сходимости. Выполняется посредством оценки
            //дисперсии набора точек. Суть проверки заключается в том, чтобы проверить
            //взаимную близость полученных вершин симплекса, что предполагает и близость их к
            //искомому минимуму. Если требуемая точность ещё не достигнута, итерации продолжаются
            #endregion
            while (GetDispersion(preferList) > StopVal)
            {
                preferList = preferList.OrderBy(p => p.F).ToList();//Сортировка
                NMPoint b = preferList[0];//лучшая точка
                NMPoint g = preferList[1];//хорошая точка
                NMPoint w = preferList[lastpreferListIndx];//худшая точка
                //центр тяжести всех точек, кроме худшей
                NMPoint mid = new NMPoint(b.Args);
                for (int i = 1; i < preferList.Count - 1; i++)
                    mid += preferList[i];
                mid = mid / (preferList.Count - 1);
                NMPoint r = (1 + reflKoef) * mid - reflKoef * w;//Отражение
                r.F = F(r.Args);
                //определение того, насколько удалось уменьшить функцию
                if (r.F < b.F)
                {
                    //направление выбрано удачное и можно попробовать увеличить шаг
                    NMPoint e = (1 - strKoef) * mid + strKoef * r;//растяжение
                    e.F = F(e.Args);//определение функции отклика
                    if (e.F < r.F)
                        preferList[lastpreferListIndx] = e;//можно расширить симплекс до этой точки: присваиваем точке w значение e
                    else
                        preferList[lastpreferListIndx] = r; //переместились слишком далеко: присваиваем точке w значение r
                }
                else if (r.F < g.F)
                    preferList[lastpreferListIndx] = r;//выбор точки неплохой (новая лучше двух прежних). Присваиваем точке w значение r
                else
                {
                    if (r.F < w.F)
                    {
                        w = r;
                        preferList[lastpreferListIndx] = r;
                    }
                    preferList = preferList.OrderBy(p => p.F).ToList();//Сортировка
                    b = preferList[0];//лучшая точка
                    w = preferList[lastpreferListIndx];//худшая точка

                    NMPoint s = comprKoef * w + (1 - comprKoef) * mid;//«Сжатие»
                    s.F = F(s.Args);
                    if (s.F < w.F)
                        preferList[lastpreferListIndx] = s;
                    else
                    {
                        //Делаем «глобальное сжатие» симплекса 
                        for (int i = 0; i < preferList.Count; i++)
                        {
                            preferList[i] = b + (preferList[i] - b) / 2;
                            preferList[i].F = F(preferList[i].Args);
                        }
                    }
                }
            }
            preferList = preferList.OrderBy(p => p.F).ToList();//Сортировка
            Args = preferList[0].Args;//лучшая точка
            FRes = F(Args);
        }

        private static double GetDispersion(List<NMPoint> list)
        {
            // для определения погрешности определяется дисперсия разброса точек
            int argsLength = list[0].Args.Length;
            //Математическое ожидание
            double[] MatOj = new double[argsLength];
            for (int i = 0; i < argsLength; i++)
            {
                for (int j = 0; j < list.Count; j++)
                    MatOj[i] += list[j].Args[i];
                MatOj[i] = MatOj[i] / list.Count;
            }
            double res = 0;
            for (int i = 0; i < list.Count; i++)
            {
                //расстояние от мат. ожидания до каждой точки
                double tmp = 0;
                for (int j = 0; j < argsLength; j++)
                    tmp += Math.Pow(MatOj[j] - list[i].Args[j], 2);
                res += Math.Sqrt(tmp);
            }

            double result = res / list.Count;



            // для определения погрешности определяется дисперсия разброса точек
            int argsLength1 = list[0].Args.Length;
            //Математическое ожидание
            double[] MatOj1 = new double[argsLength1];
            for (int i = 0; i < argsLength1; i++)
            {
                for (int j = 0; j < list.Count; j++)
                    MatOj1[i] += list[j].Args[i];
                MatOj1[i] = MatOj1[i] / list.Count;
            }
            double res1 = 0;
            for (int i = 0; i < list.Count; i++)
            {
                //расстояние от мат. ожидания до каждой точки
                double tmp1 = 0;
                for (int j = 0; j < argsLength1; j++)
                    tmp1 += Math.Pow(MatOj1[j] - list[i].Args[j], 2);
                res += Math.Sqrt(tmp1);
            }

            double result2 = res1 / list.Count;


            return res / list.Count;
        }
        public double[] Args { get; set; }
        public Func<double[], double> F { get; set; }
        public double FRes { get; set; }
        public double H { get; set; } = 0.1;
        public double StopVal { get; set; } = 0.01;
        private class NMPoint
        {
            public double[] Args { get; set; }
            public double F { get; set; }
            public NMPoint(double[] args)
            {
                Args = args;
            }
            public static NMPoint operator +(NMPoint p1, NMPoint p2)
            {
                int arrCount = p1.Args.Length;
                double[] resArr = new double[arrCount];
                for (int i = 0; i < arrCount; i++)
                    resArr[i] = p1.Args[i] + p2.Args[i];
                return new NMPoint(resArr);
            }
            public static NMPoint operator -(NMPoint p1, NMPoint p2)
            {
                int arrCount = p1.Args.Length;
                double[] resArr = new double[arrCount];
                for (int i = 0; i < arrCount; i++)
                    resArr[i] = p1.Args[i] - p2.Args[i];
                return new NMPoint(resArr);
            }
            public static NMPoint operator /(NMPoint p1, NMPoint p2)
            {
                int arrCount = p1.Args.Length;
                double[] resArr = new double[arrCount];
                for (int i = 0; i < arrCount; i++)
                    resArr[i] = p1.Args[i] / p2.Args[i];
                return new NMPoint(resArr);
            }
            public static NMPoint operator *(NMPoint p1, NMPoint p2)
            {
                int arrCount = p1.Args.Length;
                double[] resArr = new double[arrCount];
                for (int i = 0; i < arrCount; i++)
                    resArr[i] = p1.Args[i] * p2.Args[i];
                return new NMPoint(resArr);
            }
            public static NMPoint operator +(NMPoint p1, double k)
            {
                int arrCount = p1.Args.Length;
                double[] resArr = new double[arrCount];
                for (int i = 0; i < arrCount; i++)
                    resArr[i] = p1.Args[i] + k;
                return new NMPoint(resArr);
            }
            public static NMPoint operator -(NMPoint p1, double k)
            {
                int arrCount = p1.Args.Length;
                double[] resArr = new double[arrCount];
                for (int i = 0; i < arrCount; i++)
                    resArr[i] = p1.Args[i] - k;
                return new NMPoint(resArr);
            }
            public static NMPoint operator /(NMPoint p1, double k)
            {
                int arrCount = p1.Args.Length;
                double[] resArr = new double[arrCount];
                for (int i = 0; i < arrCount; i++)
                    resArr[i] = p1.Args[i] / k;
                return new NMPoint(resArr);
            }
            public static NMPoint operator *(NMPoint p1, double k)
            {
                int arrCount = p1.Args.Length;
                double[] resArr = new double[arrCount];
                for (int i = 0; i < arrCount; i++)
                    resArr[i] = p1.Args[i] * k;
                return new NMPoint(resArr);
            }
            public static NMPoint operator +(double k, NMPoint p1)
            {
                return p1 + k;
            }
            public static NMPoint operator *(double k, NMPoint p1)
            {
                return p1 * k;
            }
        };
    }
}
