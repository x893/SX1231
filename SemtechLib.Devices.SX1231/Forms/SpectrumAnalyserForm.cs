namespace SemtechLib.Devices.SX1231.Forms
{
    using SemtechLib.Controls;
    using SemtechLib.Devices.SX1231;
    using SemtechLib.Devices.SX1231.Controls;
    using SemtechLib.Devices.SX1231.Enumerations;
    using SemtechLib.General;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using ZedGraph;

    public class SpectrumAnalyserForm : Form
    {
        private ApplicationSettings appSettings;
        private ComboBox cBoxLanGainSelect;
        private IContainer components;
        private SpectrumGraphControl graph;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private NumericUpDownEx nudChanBw;
        private NumericUpDownEx nudFreqCenter;
        private NumericUpDownEx nudFreqSpan;
        private Panel panel1;
        private Panel panel2;
        private PointPairList points;
        private decimal rxBw = 10417M;
        private SemtechLib.Devices.SX1231.SX1231 sx1231;

        public SpectrumAnalyserForm()
        {
            this.InitializeComponent();
        }

        private void cBoxLanGainSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.sx1231.SetLnaGainSelect(this.LnaGainSelect);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(SpectrumAnalyserForm));
            this.panel1 = new Panel();
            this.cBoxLanGainSelect = new ComboBox();
            this.nudFreqCenter = new NumericUpDownEx();
            this.nudFreqSpan = new NumericUpDownEx();
            this.nudChanBw = new NumericUpDownEx();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panel2 = new Panel();
            this.graph = new SpectrumGraphControl();
            this.panel1.SuspendLayout();
            this.nudFreqCenter.BeginInit();
            this.nudFreqSpan.BeginInit();
            this.nudChanBw.BeginInit();
            this.panel2.SuspendLayout();
            base.SuspendLayout();
            this.panel1.Anchor = AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Top;
            this.panel1.BackColor = Color.Black;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.cBoxLanGainSelect);
            this.panel1.Controls.Add(this.nudFreqCenter);
            this.panel1.Controls.Add(this.nudFreqSpan);
            this.panel1.Controls.Add(this.nudChanBw);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Location = new Point(0x22d, 0);
            this.panel1.Margin = new Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0xdf, 370);
            this.panel1.TabIndex = 0;
            this.cBoxLanGainSelect.FormattingEnabled = true;
            this.cBoxLanGainSelect.Items.AddRange(new object[] { "G1", "G2", "G3", "G4", "G5", "G6" });
            this.cBoxLanGainSelect.Location = new Point(0x63, 0xb5);
            this.cBoxLanGainSelect.Name = "cBoxLanGainSelect";
            this.cBoxLanGainSelect.Size = new Size(0x62, 0x15);
            this.cBoxLanGainSelect.TabIndex = 10;
            this.cBoxLanGainSelect.SelectedIndexChanged += new EventHandler(this.cBoxLanGainSelect_SelectedIndexChanged);
            this.nudFreqCenter.Anchor = AnchorStyles.None;
            int[] bits = new int[4];
            bits[0] = 0x3d;
            this.nudFreqCenter.Increment = new decimal(bits);
            this.nudFreqCenter.Location = new Point(0x63, 0x67);
            int[] numArray2 = new int[4];
            numArray2[0] = 0x3ccbf700;
            this.nudFreqCenter.Maximum = new decimal(numArray2);
            int[] numArray3 = new int[4];
            numArray3[0] = 0x11490c80;
            this.nudFreqCenter.Minimum = new decimal(numArray3);
            this.nudFreqCenter.Name = "nudFreqCenter";
            this.nudFreqCenter.Size = new Size(0x62, 20);
            this.nudFreqCenter.TabIndex = 1;
            this.nudFreqCenter.ThousandsSeparator = true;
            int[] numArray4 = new int[4];
            numArray4[0] = 0x3689cac0;
            this.nudFreqCenter.Value = new decimal(numArray4);
            this.nudFreqCenter.ValueChanged += new EventHandler(this.nudFreqCenter_ValueChanged);
            this.nudFreqSpan.Anchor = AnchorStyles.None;
            int[] numArray5 = new int[4];
            numArray5[0] = 0x3d;
            this.nudFreqSpan.Increment = new decimal(numArray5);
            this.nudFreqSpan.Location = new Point(0x63, 0x81);
            int[] numArray6 = new int[4];
            numArray6[0] = 0x5f5e100;
            this.nudFreqSpan.Maximum = new decimal(numArray6);
            this.nudFreqSpan.Name = "nudFreqSpan";
            this.nudFreqSpan.Size = new Size(0x62, 20);
            this.nudFreqSpan.TabIndex = 4;
            this.nudFreqSpan.ThousandsSeparator = true;
            int[] numArray7 = new int[4];
            numArray7[0] = 0xf4240;
            this.nudFreqSpan.Value = new decimal(numArray7);
            this.nudFreqSpan.ValueChanged += new EventHandler(this.nudFreqSpan_ValueChanged);
            this.nudChanBw.Anchor = AnchorStyles.None;
            this.nudChanBw.Location = new Point(0x63, 0x9b);
            int[] numArray8 = new int[4];
            numArray8[0] = 0x7a120;
            this.nudChanBw.Maximum = new decimal(numArray8);
            int[] numArray9 = new int[4];
            numArray9[0] = 0xf42;
            this.nudChanBw.Minimum = new decimal(numArray9);
            this.nudChanBw.Name = "nudChanBw";
            this.nudChanBw.Size = new Size(0x62, 20);
            this.nudChanBw.TabIndex = 7;
            this.nudChanBw.ThousandsSeparator = true;
            int[] numArray10 = new int[4];
            numArray10[0] = 0x28b1;
            this.nudChanBw.Value = new decimal(numArray10);
            this.nudChanBw.ValueChanged += new EventHandler(this.nudChanBw_ValueChanged);
            this.label2.Anchor = AnchorStyles.None;
            this.label2.AutoSize = true;
            this.label2.BackColor = Color.Transparent;
            this.label2.ForeColor = Color.Gray;
            this.label2.Location = new Point(-2, 0x85);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x23, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Span:";
            this.label1.Anchor = AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.BackColor = Color.Transparent;
            this.label1.ForeColor = Color.Gray;
            this.label1.Location = new Point(-2, 0x6b);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x5b, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Center frequency:";
            this.label6.Anchor = AnchorStyles.None;
            this.label6.AutoSize = true;
            this.label6.ForeColor = Color.Gray;
            this.label6.Location = new Point(0xcb, 0x9f);
            this.label6.Name = "label6";
            this.label6.Size = new Size(20, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Hz";
            this.label7.Anchor = AnchorStyles.None;
            this.label7.AutoSize = true;
            this.label7.BackColor = Color.Transparent;
            this.label7.ForeColor = Color.Gray;
            this.label7.Location = new Point(-2, 0xb9);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x36, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "LNA gain:";
            this.label3.Anchor = AnchorStyles.None;
            this.label3.AutoSize = true;
            this.label3.BackColor = Color.Transparent;
            this.label3.ForeColor = Color.Gray;
            this.label3.Location = new Point(-2, 0x9f);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x65, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Channel bandwidth:";
            this.label4.Anchor = AnchorStyles.None;
            this.label4.AutoSize = true;
            this.label4.ForeColor = Color.Gray;
            this.label4.Location = new Point(0xcb, 0x6b);
            this.label4.Name = "label4";
            this.label4.Size = new Size(20, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Hz";
            this.label5.Anchor = AnchorStyles.None;
            this.label5.AutoSize = true;
            this.label5.ForeColor = Color.Gray;
            this.label5.Location = new Point(0xcb, 0x85);
            this.label5.Name = "label5";
            this.label5.Size = new Size(20, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Hz";
            this.panel2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.panel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.graph);
            this.panel2.Location = new Point(0, 0);
            this.panel2.Margin = new Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(0x22d, 370);
            this.panel2.TabIndex = 2;
            this.graph.Dock = DockStyle.Fill;
            this.graph.Location = new Point(0, 0);
            this.graph.Name = "graph";
            this.graph.Size = new Size(0x22b, 0x170);
            this.graph.TabIndex = 0;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(780, 370);
            base.Controls.Add(this.panel2);
            base.Controls.Add(this.panel1);
            base.Icon = (Icon) resources.GetObject("$this.Icon");
            base.Name = "SpectrumAnalyserForm";
            this.Text = "Spectrum analyser";
            base.Load += new EventHandler(this.SpectrumAnalyserForm_Load);
            base.FormClosed += new FormClosedEventHandler(this.SpectrumAnalyserForm_FormClosed);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.nudFreqCenter.EndInit();
            this.nudFreqSpan.EndInit();
            this.nudChanBw.EndInit();
            this.panel2.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private bool IsFormLocatedInScreen(Form frm, Screen[] screens)
        {
            int upperBound = screens.GetUpperBound(0);
            bool flag = false;
            for (int i = 0; i <= upperBound; i++)
            {
                if (((frm.Left < screens[i].WorkingArea.Left) || (frm.Top < screens[i].WorkingArea.Top)) || ((frm.Left > screens[i].WorkingArea.Right) || (frm.Top > screens[i].WorkingArea.Bottom)))
                {
                    flag = false;
                }
                else
                {
                    return true;
                }
            }
            return flag;
        }

        private void nudChanBw_ValueChanged(object sender, EventArgs e)
        {
            decimal[] array = SemtechLib.Devices.SX1231.SX1231.ComputeRxBwFreqTable(this.sx1231.FrequencyXo, this.sx1231.ModulationType);
            int index = 0;
            int num2 = (int) (this.nudChanBw.Value - this.RxBw);
            if ((num2 >= -1) && (num2 <= 1))
            {
                index = Array.IndexOf<decimal>(array, this.RxBw) - num2;
            }
            else
            {
                int mant = 0;
                int exp = 0;
                decimal num5 = 0M;
                SemtechLib.Devices.SX1231.SX1231.ComputeRxBwMantExp(this.sx1231.FrequencyXo, this.sx1231.ModulationType, this.nudChanBw.Value, ref mant, ref exp);
                num5 = SemtechLib.Devices.SX1231.SX1231.ComputeRxBw(this.sx1231.FrequencyXo, this.sx1231.ModulationType, mant, exp);
                index = Array.IndexOf<decimal>(array, num5);
            }
            this.nudChanBw.ValueChanged -= new EventHandler(this.nudChanBw_ValueChanged);
            this.nudChanBw.Value = array[index];
            this.nudChanBw.ValueChanged += new EventHandler(this.nudChanBw_ValueChanged);
            this.RxBw = this.nudChanBw.Value;
            this.sx1231.SetRxBw(this.RxBw);
        }

        private void nudFreqCenter_ValueChanged(object sender, EventArgs e)
        {
            this.FrequencyRf = this.nudFreqCenter.Value;
            this.sx1231.SetFrequencyRf(this.FrequencyRf);
        }

        private void nudFreqSpan_ValueChanged(object sender, EventArgs e)
        {
            this.FrequencySpan = this.nudFreqSpan.Value;
            this.sx1231.SpectrumFrequencySpan = this.FrequencySpan;
        }

        private void OnError(byte status, string message)
        {
            this.Refresh();
        }

        private void OnSX1231PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                switch (e.PropertyName)
                {
                    case "FrequencyRf":
                        this.FrequencyRf = this.sx1231.FrequencyRf;
                        this.UpdatePointsList();
                        return;

                    case "RxBw":
                        this.RxBw = this.sx1231.RxBw;
                        this.UpdatePointsList();
                        return;

                    case "RxBwMin":
                        this.nudChanBw.Minimum = this.sx1231.RxBwMin;
                        return;

                    case "RxBwMax":
                        this.nudChanBw.Maximum = this.sx1231.RxBwMax;
                        return;

                    case "SpectrumFreqSpan":
                        this.FrequencySpan = this.sx1231.SpectrumFrequencySpan;
                        this.UpdatePointsList();
                        return;

                    case "LnaGainSelect":
                        this.LnaGainSelect = this.sx1231.LnaGainSelect;
                        return;

                    case "SpectrumData":
                    {
                        bool flag1 = this.sx1231.SpectrumRssiValue < -40M;
                        this.graph.UpdateLineGraph(this.sx1231.SpectrumFrequencyId, (double) this.sx1231.SpectrumRssiValue);
                        return;
                    }
                }
            }
            catch (Exception exception)
            {
                this.OnError(1, exception.Message);
            }
        }

        private void SpectrumAnalyserForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                this.appSettings.SetValue("SpectrumAnalyserTop", base.Top.ToString());
                this.appSettings.SetValue("SpectrumAnalyserLeft", base.Left.ToString());
                this.sx1231.SpectrumOn = false;
            }
            catch (Exception)
            {
            }
        }

        private void SpectrumAnalyserForm_Load(object sender, EventArgs e)
        {
            string s = this.appSettings.GetValue("SpectrumAnalyserTop");
            if (s != null)
            {
                try
                {
                    base.Top = int.Parse(s);
                }
                catch
                {
                    MessageBox.Show(this, "Error getting Top value.");
                }
            }
            s = this.appSettings.GetValue("SpectrumAnalyserLeft");
            if (s != null)
            {
                try
                {
                    base.Left = int.Parse(s);
                }
                catch
                {
                    MessageBox.Show(this, "Error getting Left value.");
                }
            }
            Screen[] allScreens = Screen.AllScreens;
            if (!this.IsFormLocatedInScreen(this, allScreens))
            {
                base.Top = allScreens[0].WorkingArea.Top;
                base.Left = allScreens[0].WorkingArea.Left;
            }
            this.sx1231.SpectrumOn = true;
        }

        private void sx1231_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (base.InvokeRequired)
            {
                base.BeginInvoke(new SX1231DataChangedDelegate(this.OnSX1231PropertyChanged), new object[] { sender, e });
            }
            else
            {
                this.OnSX1231PropertyChanged(sender, e);
            }
        }

        private void UpdatePointsList()
        {
            GraphPane pane = this.graph.PaneList[0];
            pane.XAxis.Scale.Max = (double) this.sx1231.SpectrumFrequencyMax;
            pane.XAxis.Scale.Min = (double) this.sx1231.SpectrumFrequencyMin;
            this.sx1231.SpectrumFrequencyId = 0;
            this.points = new PointPairList();
            for (int i = 0; i < this.sx1231.SpectrumNbFrequenciesMax; i++)
            {
                this.points.Add(new PointPair((double) (this.sx1231.SpectrumFrequencyMin + (this.sx1231.SpectrumFrequencyStep * i)), -127.5));
            }
            pane.CurveList[0] = new LineItem("", this.points, Color.Yellow, SymbolType.None);
            pane.AxisChange();
            this.graph.Invalidate();
            this.graph.Refresh();
        }

        public ApplicationSettings AppSettings
        {
            get
            {
                return this.appSettings;
            }
            set
            {
                this.appSettings = value;
            }
        }

        private decimal FrequencyRf
        {
            get
            {
                return this.nudFreqCenter.Value;
            }
            set
            {
                try
                {
                    this.nudFreqCenter.ValueChanged -= new EventHandler(this.nudFreqCenter_ValueChanged);
                    uint num = (uint) Math.Round((decimal) (value / this.sx1231.FrequencyStep), MidpointRounding.AwayFromZero);
                    this.nudFreqCenter.Value = num * this.sx1231.FrequencyStep;
                }
                catch (Exception)
                {
                    this.nudFreqCenter.BackColor = ControlPaint.LightLight(Color.Red);
                }
                finally
                {
                    this.nudFreqCenter.ValueChanged += new EventHandler(this.nudFreqCenter_ValueChanged);
                }
            }
        }

        private decimal FrequencySpan
        {
            get
            {
                return this.nudFreqSpan.Value;
            }
            set
            {
                try
                {
                    this.nudFreqSpan.ValueChanged -= new EventHandler(this.nudFreqSpan_ValueChanged);
                    uint num = (uint) Math.Round((decimal) (value / this.sx1231.FrequencyStep), MidpointRounding.AwayFromZero);
                    this.nudFreqSpan.Value = num * this.sx1231.FrequencyStep;
                }
                catch (Exception)
                {
                    this.nudFreqSpan.BackColor = ControlPaint.LightLight(Color.Red);
                }
                finally
                {
                    this.nudFreqSpan.ValueChanged += new EventHandler(this.nudFreqSpan_ValueChanged);
                }
            }
        }

        private LnaGainEnum LnaGainSelect
        {
            get
            {
                return (LnaGainEnum) (this.cBoxLanGainSelect.SelectedIndex + 1);
            }
            set
            {
                this.cBoxLanGainSelect.SelectedIndex = ((int) value) - 1;
            }
        }

        private decimal RxBw
        {
            get
            {
                return this.rxBw;
            }
            set
            {
                try
                {
                    this.nudChanBw.ValueChanged -= new EventHandler(this.nudChanBw_ValueChanged);
                    int mant = 0;
                    int exp = 0;
                    SemtechLib.Devices.SX1231.SX1231.ComputeRxBwMantExp(this.sx1231.FrequencyXo, this.sx1231.ModulationType, value, ref mant, ref exp);
                    this.rxBw = SemtechLib.Devices.SX1231.SX1231.ComputeRxBw(this.sx1231.FrequencyXo, this.sx1231.ModulationType, mant, exp);
                    this.nudChanBw.Value = this.rxBw;
                }
                catch (Exception)
                {
                }
                finally
                {
                    this.nudChanBw.ValueChanged += new EventHandler(this.nudChanBw_ValueChanged);
                }
            }
        }

        public SemtechLib.Devices.SX1231.SX1231 SX1231
        {
            set
            {
                if (this.sx1231 != value)
                {
                    this.sx1231 = value;
                    this.sx1231.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(this.sx1231_PropertyChanged);
                    this.FrequencyRf = this.sx1231.FrequencyRf;
                    this.RxBw = this.sx1231.RxBw;
                    this.LnaGainSelect = this.sx1231.LnaGainSelect;
                    this.UpdatePointsList();
                }
            }
        }

        private delegate void SX1231DataChangedDelegate(object sender, PropertyChangedEventArgs e);
    }
}

