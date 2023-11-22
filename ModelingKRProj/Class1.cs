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
    class Optimization
    {
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

        public void GetResultOptByNM()
        {
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
            return res / list.Count;
        }

        public void GetResultOptByHG()
        {
            // На начальном этапе задаётся стартовая точка 1
            double h = H;
            double[] x2 = new double[Args.Length], x3, x4 = new double[Args.Length];
            double fX2 = 0, fX4 = 0;
            FRes = F(Args);
            double StrKoef = 2;
            while (h > StopVal) //Когда ошибка станет меньше допустимой, алгоритм завершается, и точка 1 признаётся точкой минимума.
            {
                double[] x1 = Args;
                double fX1 = F(Args);
                bool isChangeCFunc = ExploratorySearch(ref x1, ref fX1, ref x2, ref fX2, h);
                //если лучшее значение не найдено, производится определение ошибки
                if (!isChangeCFunc)
                    h = h / 10; //если на следующей итерации ошибка не окажется меньше допустимой, шаг сокращается в 10 раз
                else //если лучшее значение найдено
                {
                    do
                    {
                        //На этапе поиска по образцу откладывается точка 3 в направлении от 1 к 2 на том же расстоянии.
                        x3 = new double[Args.Length];
                        for (int i = 0; i < Args.Length; i++)
                            x3[i] = Args[i] + StrKoef * (x2[i] - Args[i]);
                        Args = x2;
                        FRes = fX2;
                        // определение значений целевых функции точек рядом с точкой 3
                        if (ExploratorySearch(ref x3, ref fX2, ref x4, ref fX4, h))
                        {
                            // если в окрестностях точки 3 найжено лучшее решение,
                            // то сохраняется точка x2 как x1 и x4 как x2
                            x2 = x4;
                            fX2 = fX4;
                        }
                        else
                            isChangeCFunc = false;

                    } while (isChangeCFunc); // если дальнейший поиск по образцу не дал лучших результатов,
                                             // выход из цикла
                }
            }
        }

        private bool ExploratorySearch(ref double[] x1, ref double fX1, ref double[] x2, ref double fX2, double h)
        {
            x2 = (double[])x1.Clone();
            fX2 = fX1;
            for (int i = 0; i < Args.Length; i++)
            {
                //Этап исследующего поиска. Определяем лучшее значение функции в окрестностях точки x1 с шагом h. Точка с найденным значением становится точкой x2.
                double tmpArg = x2[i];
                x2[i] = tmpArg + h;
                double pastFx2 = fX2;
                double newfX2 = F(x2);
                // если целевая функция новой точки больше значения предыдущей точки,
                // производится определение ц. функции в противоположном направлении
                if (pastFx2 <= newfX2)
                {
                    x2[i] = tmpArg - h;
                    newfX2 = F(x2);
                    // если не найдено лучшее решение, сохраняется предыдущая точка
                    if (pastFx2 <= newfX2)
                        x2[i] = tmpArg;
                }
                // если найдено лучшее значение ц. функции, то кроме сохранения расположения новой
                // точки также сохраняется её целевая функция
                if (pastFx2 > newfX2)
                    fX2 = newfX2;
            }
            return fX2 < fX1;
        }

        public void GetResultOptByGrad()
        {
            double[] x = Args;
            int iEnd = x.Length; //Size of input array
            double[] fi = new double[iEnd];
            double[] newX = new double[iEnd];
            int n = 2;
            double h = 0.001,  //Tolerance factor
            g0 = F(x), //Initial estimate of result
            DelG = StopVal + 1,
            alpha = H;
            //Iterate until value is <= tolerance limit
            while (DelG > StopVal)
            {
                //Calculate next gradient
                fi = GradG(x, h);
                //Calculate initial norm
                DelG = 0;
                for (int i = 0; i < iEnd; i++)
                    DelG += Math.Pow(fi[i], 2);
                DelG = Math.Sqrt(DelG);
                double g1 = g0 + 1;
                //n--;
                while (g1 > g0)
                {
                    double b = alpha / DelG;
                    //Calculate next value
                    for (int i = 0; i < iEnd; i++)
                        newX[i] = x[i] - b * fi[i];
                    //Check value of given function
                    //with current values
                    g1 = F(newX);
                    alpha = H / n++;
                }
                g0 = g1;
                x = (double[])newX.Clone();
            }
            Args = x;//лучшая точка
            FRes = g0;
        }

        public void GetResultOptByGrad2()
        {
            double[] x = Args;
            int iEnd = x.Length; //Size of input array
            double[] fi = new double[iEnd];
            double[] newX = new double[iEnd];
            int n = 2;
            double h = 0.001,  //Tolerance factor
            g0 = F(x), //Initial estimate of result
            DelG = StopVal + 1,
            alpha = H;
            //Iterate until value is <= tolerance limit
            while (DelG > StopVal)
            {
                //Calculate next gradient
                fi = GradG(x, h);
                //Calculate initial norm
                DelG = 0;
                for (int i = 0; i < iEnd; i++)
                    DelG += Math.Pow(fi[i], 2);
                DelG = Math.Sqrt(DelG);
                alpha = H / n++;
                double err = 1,
                    a = 0,
                    b = alpha / DelG,
                    eps = b / 10,
                    x1 = 0,
                    x2 = 0,
                    f1 = 0,
                    f2 = 0;
                while (err > eps)
                {
                    if (f1 == 0)
                    {
                        x1 = a + 0.382 * (b - a);
                        for (int i = 0; i < iEnd; i++)
                            newX[i] = x[i] - x1 * fi[i];
                        f1 = F(newX);
                    }
                    if (f2 == 0)
                    {
                        x2 = b - 0.382 * (b - a);
                        for (int i = 0; i < iEnd; i++)
                            newX[i] = x[i] - x2 * fi[i];
                        f2 = F(newX);
                    }
                    if (f1 <= f2)
                    {
                        b = x2;
                        x2 = x1;
                        f2 = f1;
                        f1 = 0;
                    }
                    else
                    {
                        a = x1;
                        x1 = x2;
                        f1 = f2;
                        f2 = 0;
                    }
                    err = b - a;
                }
                double tmp = 0;
                if (f1 == 0)
                {
                    g0 = f2;
                    tmp = x2;
                }
                else
                {
                    g0 = f1;
                    tmp = x1;
                }
                for (int i = 0; i < iEnd; i++)
                    x[i] = x[i] - tmp * fi[i];
            }
            Args = x;//лучшая точка
            FRes = g0;
        }

        public void GetResultOptByGrad3()
        {
            double[] x = Args;
            int iEnd = x.Length; //Size of input array
            double[] fOld = new double[iEnd];
            double[] fNew = new double[iEnd];
            double[] fi = new double[iEnd];
            double[] newX = new double[iEnd];
            int n = 2;
            double h = 0.001,  //Tolerance factor
            g0 = F(x), //Initial estimate of result
            alpha = H,
            sigmaNew = 1,
            sigmaOld = 1;
            int k = 0;
            //Iterate until value is <= tolerance limit
            while (alpha > StopVal)
            {
                //Calculate next gradient
                fNew = GradG(x, h);
                sigmaOld = sigmaNew;
                sigmaNew = 0;
                for (int i = 0; i < iEnd; i++)
                    sigmaNew += Math.Pow(fNew[i], 2);
                double beta = 0;
                if (k % x.Length != 0)
                    beta = sigmaNew / sigmaOld;
                for (int i = 0; i < iEnd; i++)
                    fi[i] = fNew[i] + beta * fOld[i];
                //Calculate initial norm
                alpha /= 2;
                double err = 1,
                    a = 0,
                    b = alpha,
                    eps = b / 10,
                    x1 = 0,
                    x2 = 0,
                    f1 = 0,
                    f2 = 0;
                while (err > eps)
                {
                    if (f1 == 0)
                    {
                        x1 = a + 0.382 * (b - a);
                        for (int i = 0; i < iEnd; i++)
                            newX[i] = x[i] - x1 * fi[i];
                        f1 = F(newX);
                    }
                    if (f2 == 0)
                    {
                        x2 = b - 0.382 * (b - a);
                        for (int i = 0; i < iEnd; i++)
                            newX[i] = x[i] - x2 * fi[i];
                        f2 = F(newX);
                    }
                    if (f1 <= f2)
                    {
                        b = x2;
                        x2 = x1;
                        f2 = f1;
                        f1 = 0;
                    }
                    else
                    {
                        a = x1;
                        x1 = x2;
                        f1 = f2;
                        f2 = 0;
                    }
                    err = b - a;
                }
                double tmp = 0;
                if (f1 == 0)
                {
                    g0 = f2;
                    tmp = x2;
                }
                else
                {
                    g0 = f1;
                    tmp = x1;
                }
                for (int i = 0; i < iEnd; i++)
                    x[i] = x[i] - tmp * fi[i];
                fOld = (double[])fi.Clone();
                k++;
            }
            Args = x;//лучшая точка
            FRes = g0;
        }

        public void GetResultOptByGrad4()
        {
            int iEnd = Args.Length; //Size of input array
            double[] fi = new double[iEnd];
            double[] newX = new double[iEnd];
            double[] x = new double[iEnd];
            double[] x0 = new double[iEnd];
            double[] x1 = Args;
            double h = 0.001,  //Tolerance factor
            beta = 0.5,
            err = StopVal + 1;
            for (int i = 0; i < iEnd; i++)
                x[i] = x1[i] + h;

            //Iterate until value is <= tolerance limit
            while (err > StopVal)
            {
                x0 = (double[])x1.Clone();
                x1 = (double[])x.Clone();
                double[] r = GradG(x1, h);
                for (int i = 0; i < iEnd; i++)
                    x[i] = x1[i] - H * r[i] + beta * (x1[i] - x0[i]);
                err = 0;
                for (int i = 0; i < iEnd; i++)
                    err += Math.Pow(r[i], 2);
                Math.Sqrt(err);
            }
            Args = x;//лучшая точка
            FRes = F(x);
        }

        private double[] GradG(double[] x, double h)
            => GradG(x, h, F(x));

        // Provides a rough calculation of gradient g(x).
        private double[] GradG(double[] x, double h, double g0)
        {
            int n = x.Length;
            double[] z = new double[n];
            double[] y = (double[])x.Clone();

            for (int i = 0; i < n; ++i)
            {
                y[i] += h;
                z[i] = (F(y) - g0) / h;
                y[i] -= h;
            }
            return z;
        }
    }
}
