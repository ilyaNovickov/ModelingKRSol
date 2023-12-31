﻿namespace ModelingKRProj
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.startButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.optimiationButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.regulagionChartButton = new System.Windows.Forms.ToolStripButton();
            this.phaseChartButton = new System.Windows.Forms.ToolStripButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.modelingChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.phaseChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.excelButton = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.modelingChart)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.phaseChart)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startButton,
            this.toolStripSeparator1,
            this.optimiationButton,
            this.toolStripSeparator2,
            this.regulagionChartButton,
            this.phaseChartButton,
            this.toolStripSeparator3,
            this.excelButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 27);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // startButton
            // 
            this.startButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.startButton.Image = ((System.Drawing.Image)(resources.GetObject("startButton.Image")));
            this.startButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(44, 24);
            this.startButton.Text = "Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // optimiationButton
            // 
            this.optimiationButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.optimiationButton.Image = ((System.Drawing.Image)(resources.GetObject("optimiationButton.Image")));
            this.optimiationButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.optimiationButton.Name = "optimiationButton";
            this.optimiationButton.Size = new System.Drawing.Size(109, 24);
            this.optimiationButton.Text = "Оптимизация";
            this.optimiationButton.Click += new System.EventHandler(this.optimiationButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
            // 
            // regulagionChartButton
            // 
            this.regulagionChartButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.regulagionChartButton.Image = ((System.Drawing.Image)(resources.GetObject("regulagionChartButton.Image")));
            this.regulagionChartButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.regulagionChartButton.Name = "regulagionChartButton";
            this.regulagionChartButton.Size = new System.Drawing.Size(240, 24);
            this.regulagionChartButton.Text = "Сохранить переходный процесс";
            this.regulagionChartButton.Click += new System.EventHandler(this.ChartButton_Click);
            // 
            // phaseChartButton
            // 
            this.phaseChartButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.phaseChartButton.Image = ((System.Drawing.Image)(resources.GetObject("phaseChartButton.Image")));
            this.phaseChartButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.phaseChartButton.Name = "phaseChartButton";
            this.phaseChartButton.Size = new System.Drawing.Size(213, 24);
            this.phaseChartButton.Text = "Сохранить фазовый портрет";
            this.phaseChartButton.Click += new System.EventHandler(this.ChartButton_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tableLayoutPanel1.Controls.Add(this.propertyGrid, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 27);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 423);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // propertyGrid
            // 
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.Location = new System.Drawing.Point(3, 3);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(194, 417);
            this.propertyGrid.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(203, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(594, 417);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.modelingChart);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(586, 388);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Перехрдный процесс";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // modelingChart
            // 
            chartArea1.Name = "ChartArea1";
            this.modelingChart.ChartAreas.Add(chartArea1);
            this.modelingChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modelingChart.Location = new System.Drawing.Point(3, 3);
            this.modelingChart.Name = "modelingChart";
            series1.ChartArea = "ChartArea1";
            series1.Name = "Series1";
            this.modelingChart.Series.Add(series1);
            this.modelingChart.Size = new System.Drawing.Size(580, 382);
            this.modelingChart.TabIndex = 0;
            this.modelingChart.Text = "chart1";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.phaseChart);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(586, 384);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Фазовый портрет";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // phaseChart
            // 
            chartArea2.Name = "ChartArea1";
            this.phaseChart.ChartAreas.Add(chartArea2);
            this.phaseChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.phaseChart.Location = new System.Drawing.Point(3, 3);
            this.phaseChart.Name = "phaseChart";
            series2.ChartArea = "ChartArea1";
            series2.Name = "Series1";
            this.phaseChart.Series.Add(series2);
            this.phaseChart.Size = new System.Drawing.Size(580, 378);
            this.phaseChart.TabIndex = 1;
            this.phaseChart.Text = "chart1";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
            // 
            // excelButton
            // 
            this.excelButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.excelButton.Image = ((System.Drawing.Image)(resources.GetObject("excelButton.Image")));
            this.excelButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.excelButton.Name = "excelButton";
            this.excelButton.Size = new System.Drawing.Size(148, 24);
            this.excelButton.Text = "Сохранение в Excel";
            this.excelButton.Click += new System.EventHandler(this.excelButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "MainForm";
            this.Text = "Моделирование САУ";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.modelingChart)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.phaseChart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataVisualization.Charting.Chart modelingChart;
        private System.Windows.Forms.DataVisualization.Charting.Chart phaseChart;
        private System.Windows.Forms.ToolStripButton startButton;
        private System.Windows.Forms.ToolStripButton optimiationButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton regulagionChartButton;
        private System.Windows.Forms.ToolStripButton phaseChartButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton excelButton;
    }
}

