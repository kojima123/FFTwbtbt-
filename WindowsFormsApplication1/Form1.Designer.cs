namespace WindowsFormsApplication1
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.Readchart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.open = new System.Windows.Forms.Button();
            this.Rsultchart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.Result1 = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.save = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.Clear = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.gabor = new System.Windows.Forms.Button();
            this.csvsave = new System.Windows.Forms.Button();
            this.morlet1 = new System.Windows.Forms.Button();
            this.FFT = new System.Windows.Forms.Button();
            this.Result2 = new System.Windows.Forms.TextBox();
            this.Result3 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.Readchart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Rsultchart)).BeginInit();
            this.SuspendLayout();
            // 
            // Readchart
            // 
            chartArea1.Name = "ChartArea1";
            this.Readchart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.Readchart.Legends.Add(legend1);
            this.Readchart.Location = new System.Drawing.Point(115, 8);
            this.Readchart.Name = "Readchart";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.Readchart.Series.Add(series1);
            this.Readchart.Size = new System.Drawing.Size(376, 192);
            this.Readchart.TabIndex = 0;
            this.Readchart.Text = "chart1";
            // 
            // open
            // 
            this.open.Location = new System.Drawing.Point(12, 104);
            this.open.Name = "open";
            this.open.Size = new System.Drawing.Size(75, 23);
            this.open.TabIndex = 1;
            this.open.Text = "開く";
            this.open.UseVisualStyleBackColor = true;
            this.open.Click += new System.EventHandler(this.open_Click);
            // 
            // Rsultchart
            // 
            chartArea2.Name = "ChartArea1";
            this.Rsultchart.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.Rsultchart.Legends.Add(legend2);
            this.Rsultchart.Location = new System.Drawing.Point(77, 329);
            this.Rsultchart.Name = "Rsultchart";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.Rsultchart.Series.Add(series2);
            this.Rsultchart.Size = new System.Drawing.Size(463, 168);
            this.Rsultchart.TabIndex = 8;
            this.Rsultchart.Text = "chart5";
            // 
            // Result1
            // 
            this.Result1.Location = new System.Drawing.Point(127, 239);
            this.Result1.Multiline = true;
            this.Result1.Name = "Result1";
            this.Result1.Size = new System.Drawing.Size(75, 39);
            this.Result1.TabIndex = 12;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(246, 210);
            this.save.Name = "save";
            this.save.Size = new System.Drawing.Size(75, 23);
            this.save.TabIndex = 17;
            this.save.Text = "保存";
            this.save.UseVisualStyleBackColor = true;
            this.save.Click += new System.EventHandler(this.save_Click);
            // 
            // Clear
            // 
            this.Clear.Location = new System.Drawing.Point(127, 210);
            this.Clear.Name = "Clear";
            this.Clear.Size = new System.Drawing.Size(75, 23);
            this.Clear.TabIndex = 21;
            this.Clear.Text = "クリア";
            this.Clear.UseVisualStyleBackColor = true;
            this.Clear.Click += new System.EventHandler(this.Clear_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 42;
            // 
            // gabor
            // 
            this.gabor.Location = new System.Drawing.Point(0, 0);
            this.gabor.Name = "gabor";
            this.gabor.Size = new System.Drawing.Size(75, 23);
            this.gabor.TabIndex = 32;
            this.gabor.Text = "ガボール";
            this.gabor.UseVisualStyleBackColor = true;
            this.gabor.Click += new System.EventHandler(this.gabor_Click_1);
            // 
            // csvsave
            // 
            this.csvsave.Location = new System.Drawing.Point(351, 210);
            this.csvsave.Name = "csvsave";
            this.csvsave.Size = new System.Drawing.Size(75, 23);
            this.csvsave.TabIndex = 38;
            this.csvsave.Text = "CSV保存";
            this.csvsave.UseVisualStyleBackColor = true;
            this.csvsave.Click += new System.EventHandler(this.csvsave_Click);
            // 
            // morlet1
            // 
            this.morlet1.Location = new System.Drawing.Point(2, 29);
            this.morlet1.Name = "morlet1";
            this.morlet1.Size = new System.Drawing.Size(109, 23);
            this.morlet1.TabIndex = 40;
            this.morlet1.Text = "FFT　モレット σ１";
            this.morlet1.UseVisualStyleBackColor = true;
            this.morlet1.Click += new System.EventHandler(this.morlet1_Click);
            // 
            // FFT
            // 
            this.FFT.Location = new System.Drawing.Point(2, 57);
            this.FFT.Margin = new System.Windows.Forms.Padding(2);
            this.FFT.Name = "FFT";
            this.FFT.Size = new System.Drawing.Size(98, 31);
            this.FFT.TabIndex = 29;
            this.FFT.Text = "FFT";
            this.FFT.UseVisualStyleBackColor = true;
            this.FFT.Click += new System.EventHandler(this.FFT_Click);
            // 
            // Result2
            // 
            this.Result2.Location = new System.Drawing.Point(246, 239);
            this.Result2.Multiline = true;
            this.Result2.Name = "Result2";
            this.Result2.Size = new System.Drawing.Size(75, 39);
            this.Result2.TabIndex = 25;
            // 
            // Result3
            // 
            this.Result3.Location = new System.Drawing.Point(125, 284);
            this.Result3.Multiline = true;
            this.Result3.Name = "Result3";
            this.Result3.Size = new System.Drawing.Size(77, 39);
            this.Result3.TabIndex = 26;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(617, 540);
            this.Controls.Add(this.morlet1);
            this.Controls.Add(this.csvsave);
            this.Controls.Add(this.gabor);
            this.Controls.Add(this.FFT);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Result3);
            this.Controls.Add(this.Result2);
            this.Controls.Add(this.Clear);
            this.Controls.Add(this.save);
            this.Controls.Add(this.Result1);
            this.Controls.Add(this.Rsultchart);
            this.Controls.Add(this.open);
            this.Controls.Add(this.Readchart);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Readchart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Rsultchart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart Readchart;
        private System.Windows.Forms.Button open;
        private System.Windows.Forms.DataVisualization.Charting.Chart Rsultchart;
        private System.Windows.Forms.TextBox Result1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button save;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button Clear;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button gabor;
        private System.Windows.Forms.Button csvsave;
        private System.Windows.Forms.Button morlet1;
        private System.Windows.Forms.Button FFT;
        private System.Windows.Forms.TextBox Result2;
        private System.Windows.Forms.TextBox Result3;
    }
}

