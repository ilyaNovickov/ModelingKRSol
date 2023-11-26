using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Excel = Microsoft.Office.Interop.Excel;

namespace ModelingKRProj
{
    public partial class MainForm : Form
    {
        ModelingSystem system;//Моделируемая САУ

        Series modelingSeries;//График для переходного процесса

        Series phaseSeries;//график для фазового портрета

        public MainForm()
        {
            InitializeComponent();
            //Инициализация САУ
            system = new ModelingSystem()
            {
                StepofModeling = 0.01d,
                InputValue = 1d,
                GainCooficient = 4.9d,
                TimeConst = 4.8d,
                TimeofDelay = 4.8d,
                Pvalue = 0.2d,
                Ivalue = 0.02d,
                TimeofModeling = 200d
            };
            //Подключение события 
            system.ModelingEvent += ModelingEvent;
            //Установка САУ в PropertyGrid для быстрого изменения
            //значение САУ
            propertyGrid.SelectedObject = system;

            propertyGrid.Refresh();

            //Инициализация графиков
            modelingSeries = new Series(nameof(modelingSeries))
            {
                ChartType = SeriesChartType.Line,
                Color = Color.Blue
            };

            this.modelingChart.Series.Add(modelingSeries);

            phaseSeries = new Series(nameof(phaseSeries))
            {
                ChartType = SeriesChartType.Line,
                Color = Color.Red
            };

            this.phaseChart.Series.Add(phaseSeries);
        }

        //Нажатие на кнопку "Start"
        private void startButton_Click(object sender, EventArgs e)
        {
            //Перерисовка графиков
            modelingChart.Invalidate();
            phaseChart.Invalidate();
            //очистка графиков от прошлых значений
            modelingSeries.Points.Clear();
            phaseSeries.Points.Clear();
            //Моделирование САУ
            system.InvokeModeling();
            //обновление данных
            modelingChart.Update();
            phaseChart.Update();
            propertyGrid.Refresh();
        }
        //Обработчик события моделирования САУ 
        private void ModelingEvent(object sender, ModelingEventArgs e)
        {
            //добавление точек на графики
            modelingSeries.Points.Add(new DataPoint(e.CurrentTime_t, e.OutputValue_y));
            phaseSeries.Points.Add(new DataPoint(e.FistError_x1, e.DiffofError_x2));
        }

        //Нажатие на кнопку оптимизации САУ
        private void optimiationButton_Click(object sender, EventArgs e)
        {
            //Инициализация 2-у мерного оптимизатора коофициентов ПИ-регулятора
            Optimizator2D newopt = new Optimizator2D();
            newopt.Func = (system) =>
            { //Целевая функция
                system.InvokeModeling();
                return system.ISE;
            };
            newopt.OptimizingSystem = system;
            //Оптимизация
            newopt.NelderMidOptimization();
            //Выхов события нажатия на кнопку "Start" для обновления данных
            startButton_Click(null, EventArgs.Empty);
        }

        //Нажатие на кнопки экспорта графиков
        private void ChartButton_Click(object sender, EventArgs e)
        {
            //инициализация диалогового окна сохранения файлов
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "JPEG (*.jpeg)|*.jpeg";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    string path = sfd.FileName;

                    if (path == null || path == "")
                    {
                        MessageBox.Show("Ошибка: не выбран файл сохранения");
                        return;
                    }
                    //Сохранение графика в jpeg изображение
                    if (sender == regulagionChartButton)
                        this.modelingChart.SaveImage(path, ChartImageFormat.Jpeg);
                    else if (sender == phaseChartButton)
                        this.phaseChart.SaveImage(path, ChartImageFormat.Jpeg);
                }
            }
        }

        private void excelButton_Click(object sender, EventArgs e)
        {
            void SaveToExcel(string path)
            {
                //Открытие приложения Excel
                Excel.Application excelApp = new Excel.Application();
                excelApp.Visible = true;
                //Добавление книги в таблицу Excel
                Excel.Workbook workbook = excelApp.Workbooks.Add();

                Excel.Worksheet sheet = (Excel.Worksheet)excelApp.Sheets[1];

                excelApp.Cells[1, 1] = "Значение T";
                excelApp.Cells[1, 2] = "Значение Y";
                excelApp.Cells[1, 4] = "Значение X1";
                excelApp.Cells[1, 5] = "Значение X2";

                for (int row = 2; row < modelingSeries.Points.Count + 2; row++)
                {
                    excelApp.Cells[row, 1] = modelingSeries.Points[row - 2].XValue;
                    excelApp.Cells[row, 2] = modelingSeries.Points[row - 2].YValues[0];
                    excelApp.Cells[row, 4] = phaseSeries.Points[row - 2].XValue;
                    excelApp.Cells[row, 5] = phaseSeries.Points[row - 2].YValues[0];
                }
                //График переходного процесса
                Excel.ChartObjects xlCharts = (Excel.ChartObjects)sheet.ChartObjects();
                Excel.ChartObject myChart = xlCharts.Add(110, 0, 350, 250);
                Excel.Chart chart = myChart.Chart;
                Excel.SeriesCollection seriesCollection = (Excel.SeriesCollection)chart.SeriesCollection();
                Excel.Series series = seriesCollection.NewSeries();
                series.XValues = sheet.get_Range("A2", "A" + (modelingSeries.Points.Count + 2));
                series.Values = sheet.get_Range("B2", "B" + (modelingSeries.Points.Count + 2));
                chart.ChartType = Excel.XlChartType.xlXYScatterSmooth;
                //Фазовый портрет
                Excel.ChartObject myChart2 = xlCharts.Add(210, 0, 350, 250);
                Excel.Chart chart2 = myChart2.Chart;
                Excel.SeriesCollection seriesCollection2 = (Excel.SeriesCollection)chart.SeriesCollection();
                Excel.Series series2 = seriesCollection2.NewSeries();
                series2.XValues = sheet.get_Range("D2", "D" + (modelingSeries.Points.Count + 2));
                series2.Values = sheet.get_Range("F2", "F" + (modelingSeries.Points.Count + 2));
                chart2.ChartType = Excel.XlChartType.xlXYScatterSmooth;

                //Сохранение документа по указанному пути
                workbook.SaveAs(path);
                //Выход из программы Excel
                excelApp.Quit();
            }

            if (modelingSeries.Points.Count == 0)
                return;
            //инициализация диалогового окна сохранения фала
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.FileName = "Excel book (*.xlsx)|*.xlsx";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    string path = sfd.FileName;

                    if (path == null || path == "")
                        return;
                    //Сохранение в файл
                    SaveToExcel(path);
                }
            }
        }
    }
}
