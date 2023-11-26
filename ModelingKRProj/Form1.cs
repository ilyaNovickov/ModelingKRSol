using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

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
    }
}
