using SemtechLib.General;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace SemtechLib.Devices.SX1231.Controls
{
	public class RegisterTableControl : UserControl
	{
		private delegate void RegisterPropertyChangedDelegate(object sender, PropertyChangedEventArgs e);

		private IContainer components;
		private ErrorProvider errorProvider;
		private int invisibleCnt;
		private Label label;
		private int LABEL_SIZE_HEIGHT = 20;
		private int LABEL_SIZE_WIDTH = 0x69;
		private List<Label> labelList;
		private TableLayoutPanel panel;
		private string previousValue = "";
		private RegisterCollection registers = new RegisterCollection();
		private uint split = 1;
		private int tabIndex;
		private Timer tmrChangeValidated;

		public RegisterTableControl()
		{
			InitializeComponent();
			registers = new RegisterCollection();
			BuildTableHeader();
		}

		private void AddHeaderLabel(int col, int row)
		{
			for (int i = 0; i < 3; i++)
			{
				label = new Label();
				label.AutoSize = false;
				label.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
				label.TextAlign = ContentAlignment.MiddleCenter;
				label.TabIndex = tabIndex++;
				labelList.Add(label);
			}
			labelList[col].Text = "Register";
			labelList[col].Size = new Size(LABEL_SIZE_WIDTH, LABEL_SIZE_HEIGHT);
			labelList[col + 1].Text = "Addr";
			labelList[col + 1].Size = new Size(0x27, 20);
			labelList[col + 2].Text = "Value";
			labelList[col + 2].Size = new Size(0x27, 20);
			panel.Controls.Add(labelList[col], col, row);
			panel.Controls.Add(labelList[col + 1], col + 1, row);
			panel.Controls.Add(labelList[col + 2], col + 2, row);
		}

		private void BuildTable()
		{
			panel.Padding = new Padding(0, 0, 0, 0);
			panel.AutoSize = true;
			panel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			panel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
			panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
			panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
			panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
			panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			panel.Location = new Point(0, 0);
			panel.TabIndex = tabIndex++;
			AddHeaderLabel(0, 0);
			int column = 0;
			int row = 1;
			invisibleCnt = 0;

			foreach (Register item in registers)
				if (!item.Visible)
					invisibleCnt++;

			for (int i = 0; i < registers.Count; i++)
			{
				if (registers[i].Visible)
				{
					label = new Label();
					label.AutoSize = false;
					Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
					label.TextAlign = ContentAlignment.MiddleLeft;
					label.TabIndex = tabIndex++;
					label.Margin = new Padding(0);
					label.Size = new Size(LABEL_SIZE_WIDTH, LABEL_SIZE_HEIGHT);
					label.Text = registers[i].Name;
					panel.Controls.Add(label, column, row);
					label = new Label();
					label.AutoSize = false;
					label.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
					label.TextAlign = ContentAlignment.MiddleCenter;
					label.TabIndex = tabIndex++;
					label.Margin = new Padding(0);
					label.Size = new Size(0x27, 20);
					label.Text = "0x" + registers[i].Address.ToString("X02");
					panel.Controls.Add(label, column + 1, row);
					TextBox control = new TextBox();
					control.AutoSize = false;
					control.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
					control.TextAlign = HorizontalAlignment.Center;
					control.MaxLength = 4;
					control.TabIndex = tabIndex++;
					control.Tag = "0x" + registers[i].Address.ToString("X02");
					control.Margin = new Padding(0);
					control.Size = new Size(0x2d, 20);
					control.Text = "0x" + registers[i].Value.ToString("X02");
					control.ReadOnly = registers[i].ReadOnly;
					control.Validated += new EventHandler(tBox_Validated);
					control.Enter += new EventHandler(tBox_Enter);
					control.Validating += new CancelEventHandler(tBox_Validating);
					control.TextChanged += new EventHandler(tBox_TextChanged);
					panel.Controls.Add(control, column + 2, row++);

					if (row > ((registers.Count - invisibleCnt) / split))
					{
						row = 1;
						column += 3;
						if (column < (split * 3) || ((registers.Count - invisibleCnt) % split) != 0L)
						{
							AddHeaderLabel(column, 0);
						}
					}
				}
			}
			panel.ResumeLayout(false);
			panel.PerformLayout();
			base.Controls.Add(panel);
		}

		private void BuildTableHeader()
		{
			if (panel != null)
				base.Controls.Remove(panel);
			panel = new TableLayoutPanel();
			labelList = new List<Label>();
			panel.SuspendLayout();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
				components.Dispose();
			base.Dispose(disposing);
		}

		private int GetIndexFromTextBox(TextBox tBox)
		{
			int row = 0;
			int column = 0;
			foreach (Control control in panel.Controls)
			{
				if ((control is TextBox) && (control == tBox))
				{
					column = panel.GetColumn(control);
					row = panel.GetRow(control);
					break;
				}
			}
			Label controlFromPosition = (Label)panel.GetControlFromPosition(column - 2, row);
			return registers.IndexOf(registers[controlFromPosition.Text]);
		}

		private void InitializeComponent()
		{
			components = new Container();
			errorProvider = new ErrorProvider(components);
			tmrChangeValidated = new Timer(components);
			((ISupportInitialize)errorProvider).BeginInit();
			base.SuspendLayout();
			errorProvider.ContainerControl = this;
			tmrChangeValidated.Interval = 0x1388;
			tmrChangeValidated.Tick += new EventHandler(tmrChangeValidated_Tick);
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			AutoSize = true;
			base.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			base.Name = "RegisterTableControl";
			base.Size = new Size(0, 0);
			((ISupportInitialize)errorProvider).EndInit();
			base.ResumeLayout(false);
		}

		private void OnRegisterPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Value")
			{
				foreach (Control control in panel.Controls)
				{
					if (control is TextBox)
					{
						TextBox box = (TextBox)control;
						if (Convert.ToUInt32((string)control.Tag, 0x10) == ((Register)sender).Address)
						{
							if (box.Text != ("0x" + ((Register)sender).Value.ToString("X02")))
								box.ForeColor = Color.Red;
							box.Text = "0x" + ((Register)sender).Value.ToString("X02");
							break;
						}
					}
				}
			}
		}

		private void register_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (base.InvokeRequired)
				base.BeginInvoke(new RegisterPropertyChangedDelegate(OnRegisterPropertyChanged), new object[] { sender, e });
			else
				OnRegisterPropertyChanged(sender, e);
		}

		private void tBox_Enter(object sender, EventArgs e)
		{
			TextBox box = (TextBox)sender;
			previousValue = box.Text;
		}

		private void tBox_TextChanged(object sender, EventArgs e)
		{
			TextBox tBox = (TextBox)sender;
			try
			{
				if (tBox.Text != ("0x" + registers[GetIndexFromTextBox(tBox)].Value.ToString("X02")))
					tBox.ForeColor = Color.Red;
				else
					tmrChangeValidated.Enabled = true;
			}
			catch (Exception)
			{
			}
		}

		private void tBox_Validated(object sender, EventArgs e)
		{
			TextBox tBox = (TextBox)sender;
			byte num = Convert.ToByte(tBox.Text, 0x10);
			if (!tBox.Text.StartsWith("0x", true, null))
				tBox.Text = "0x" + num.ToString("X02");
			if (registers[GetIndexFromTextBox(tBox)].Value != num)
			{
				registers[GetIndexFromTextBox(tBox)].Value = num;
				tmrChangeValidated.Enabled = true;
			}
		}

		private void tBox_Validating(object sender, CancelEventArgs e)
		{
			TextBox box = (TextBox)sender;
			byte num = 0;
			byte num2 = 0xff;
			try
			{
				Convert.ToByte(box.Text, 0x10);
			}
			catch (Exception exception)
			{
				MessageBox.Show(exception.Message + "\rInput Format: Hex 0x" + num.ToString("X02") + " - 0x" + num2.ToString("X02"), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				box.Text = previousValue;
			}
		}

		private void tmrChangeValidated_Tick(object sender, EventArgs e)
		{
			tmrChangeValidated.Enabled = false;
			foreach (Control control in panel.Controls)
			{
				if (control is TextBox)
				{
					TextBox tBox = (TextBox)control;
					if (registers[GetIndexFromTextBox(tBox)].Value == Convert.ToByte(tBox.Text, 0x10))
						tBox.ForeColor = SystemColors.WindowText;
				}
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
		public RegisterCollection Registers
		{
			get { return registers; }
			set
			{
				registers = value;
				foreach (Register item in value)
					item.PropertyChanged += new PropertyChangedEventHandler(register_PropertyChanged);
				BuildTableHeader();
				BuildTable();
			}
		}

		[DefaultValue(1)]
		public uint Split
		{
			get { return split; }
			set
			{
				if (value == 0)
					split = 1;
				else
					split = value;
				BuildTable();
			}
		}
	}
}
