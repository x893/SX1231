using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ZedGraph;

namespace SemtechLib.Devices.SX1231.Controls
{
	public class RssiGraphControl : UserControl
	{
		private const float AxisScaleSize = 6f;
		private const float AxisTitleSize = 6f;
		private const float BarGraphLabelSize = 5f;
		private IContainer components;
		private ZedGraphControl graph;
		private RollingPointPairList[] graphCurveListPoints = new RollingPointPairList[13];
		private Color[] graphDataColors = new Color[] { Color.Red, Color.Aqua, Color.Yellow, Color.Blue, Color.Orange, Color.Cyan, Color.Brown, Color.Lavender, Color.Ivory, Color.Indigo, Color.HotPink, Color.LightPink, Color.Green };
		private const float GraphPaneBaseDimension = 4f;
		private const float GraphTitleSize = 14f;
		private const float LegendSize = 12f;
		private MasterPane masterPane;
		private const float MasterPaneBaseDimension = 8f;
		private const float TitleSize = 14f;

		public RssiGraphControl()
		{
			InitializeComponent();
			graph.ContextMenuBuilder += new ZedGraphControl.ContextMenuBuilderEventHandler(graph_ContextMenuBuilder);
			graph.IsShowPointValues = true;
			for (int i = 0; i < graphCurveListPoints.Length; i++)
			{
				graphCurveListPoints[i] = new RollingPointPairList(0x4b0);
			}
			MasterPaneInit();
			MasterPaneAddGraph(CreateLineGraph(3, "", "Samples", "Power [dBm]", true, true, false, true, false, false, true, false));
			MasterPaneLayout();
		}

		public void AddLineGraphPoint(int serie, double time, double value)
		{
			graphCurveListPoints[serie].Add(time, value);
		}

		public void ClearAllGraphData()
		{
			try
			{
				for (int i = 0; i < masterPane.PaneList.Count; i++)
				{
					if (masterPane.PaneList[i].CurveList.Count <= 0)
						return;
					foreach (CurveItem item in masterPane.PaneList[i].CurveList)
					{
						if (item is LineItem)
							(item.Points as IPointListEdit).Clear();
						if (item is BarItem)
							item.Points[0].Y = 0.0;
					}
					masterPane.PaneList[i].AxisChange();
				}
				graph.Invalidate();
			}
			catch
			{
				throw new Exception("While Clearing data");
			}
		}

		public GraphPane CreateLineGraph(int nSeries, string title, string xAxisTitle, string yAxisTitle, bool xAxisTitleVisible, bool yAxisTitleVisible, bool xScaleVisible, bool yScaleVisible, bool xMajorGridVisible, bool xIsBetweenLabels, bool yMajorGridVisible, bool yIsBetweenLabels)
		{
			GraphPane graphPane = new GraphPane();
			SetupGraphPane(graphPane, title, xAxisTitle, yAxisTitle, xAxisTitleVisible, yAxisTitleVisible, xScaleVisible, yScaleVisible, xMajorGridVisible, xIsBetweenLabels, yMajorGridVisible, yIsBetweenLabels);
			RollingPointPairList[] listArray = new RollingPointPairList[nSeries];
			for (int i = 0; i < listArray.Length; i++)
			{
				listArray[i] = new RollingPointPairList(0x4b0);
				graphPane.AddCurve("", listArray[i], graphDataColors[i], SymbolType.None);
			}
			return graphPane;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
				components.Dispose();
			base.Dispose(disposing);
		}

		private void graph_ContextMenuBuilder(ZedGraphControl control, ContextMenuStrip menuStrip, Point mousePt, ZedGraphControl.ContextMenuObjectState objState)
		{
			foreach (ToolStripMenuItem item in menuStrip.Items)
			{
				if (((string)item.Tag) == "set_default")
				{
					menuStrip.Items.Remove(item);
					break;
				}
			}
		}

		private void InitializeComponent()
		{
			components = new Container();
			graph = new ZedGraphControl();
			base.SuspendLayout();
			graph.Dock = DockStyle.Fill;
			graph.Location = new Point(0, 0);
			graph.Margin = new Padding(0);
			graph.Name = "graph";
			graph.ScrollGrace = 0.0;
			graph.ScrollMaxX = 0.0;
			graph.ScrollMaxY = 0.0;
			graph.ScrollMaxY2 = 0.0;
			graph.ScrollMinX = 0.0;
			graph.ScrollMinY = 0.0;
			graph.ScrollMinY2 = 0.0;
			graph.Size = new Size(0xcd, 0x85);
			graph.TabIndex = 3;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(graph);
			base.Name = "GraphDisplay";
			base.Size = new Size(0xcd, 0x85);
			base.ResumeLayout(false);
		}

		public void MasterPaneAddGraph(GraphPane graphPane)
		{
			masterPane.Add(graphPane);
		}

		public void MasterPaneInit()
		{
			masterPane = graph.MasterPane;
			masterPane.PaneList.Clear();
			masterPane.Title.Text = "RSSI";
			masterPane.Title.IsVisible = false;
			masterPane.Title.FontSpec.Size = 14f;
			masterPane.Title.FontSpec.FontColor = Color.Gray;
			masterPane.Title.FontSpec.IsBold = true;
			masterPane.Fill = new Fill(Color.Black);
			masterPane.Margin.All = 0f;
			masterPane.InnerPaneGap = 0f;
			masterPane.Legend.IsVisible = false;
			masterPane.Legend.Position = LegendPos.TopCenter;
			masterPane.Legend.Fill = new Fill(Color.Black);
			masterPane.Legend.FontSpec.FontColor = Color.Gray;
			masterPane.Legend.FontSpec.Size = 12f;
			masterPane.Legend.Border.Color = Color.Gray;
			masterPane.BaseDimension = 10f;
		}

		public void MasterPaneLayout()
		{
			using (Graphics graphics = graph.CreateGraphics())
			{
				masterPane.SetLayout(graphics, PaneLayout.SquareColPreferred);
				masterPane.AxisChange(graphics);
			}
		}

		private void SetupGraphPane(GraphPane graphPane, string title, string xAxisTitle, string yAxisTitle, bool xAxisTitleVisible, bool yAxisTitleVisible, bool xScaleVisible, bool yScaleVisible, bool xMajorGridVisible, bool xIsBetweenLabels, bool yMajorGridVisible, bool yIsBetweenLabels)
		{
			graphPane.Title.Text = title;
			graphPane.Title.FontSpec.Size = 14f;
			graphPane.Title.FontSpec.FontColor = Color.Gray;
			graphPane.Title.FontSpec.IsBold = true;
			graphPane.Fill = new Fill(Color.Black);
			graphPane.Chart.Fill = new Fill(Color.Black);
			graphPane.Chart.Border.Color = Color.Gray;
			graphPane.Legend.IsVisible = false;
			graphPane.XAxis.Title.IsVisible = xAxisTitleVisible;
			graphPane.XAxis.Title.Text = xAxisTitle;
			graphPane.XAxis.Title.FontSpec.Size = 6f;
			graphPane.XAxis.Title.FontSpec.FontColor = Color.Gray;
			graphPane.XAxis.Color = Color.Gray;
			graphPane.XAxis.Scale.IsVisible = xScaleVisible;
			graphPane.XAxis.Scale.FontSpec.Size = 6f;
			graphPane.XAxis.Scale.FontSpec.FontColor = Color.Gray;
			graphPane.XAxis.MajorGrid.IsVisible = xMajorGridVisible;
			graphPane.XAxis.MajorGrid.Color = Color.LightGray;
			graphPane.XAxis.MinorGrid.Color = Color.LightGray;
			graphPane.XAxis.MajorTic.IsOpposite = false;
			graphPane.XAxis.MajorTic.IsBetweenLabels = xIsBetweenLabels;
			graphPane.XAxis.MajorTic.Color = Color.LightGray;
			graphPane.XAxis.MinorTic.IsOpposite = false;
			graphPane.XAxis.MinorTic.Color = Color.LightGray;
			graphPane.YAxis.Title.IsVisible = yAxisTitleVisible;
			graphPane.YAxis.Title.Text = yAxisTitle;
			graphPane.YAxis.Title.FontSpec.Size = 6f;
			graphPane.YAxis.Title.FontSpec.FontColor = Color.Gray;
			graphPane.YAxis.Color = Color.Gray;
			graphPane.YAxis.Scale.IsVisible = yScaleVisible;
			graphPane.YAxis.Scale.FontSpec.Size = 6f;
			graphPane.YAxis.Scale.FontSpec.FontColor = Color.Gray;
			graphPane.YAxis.Scale.Min = -150.0;
			graphPane.YAxis.Scale.Max = 0.0;
			graphPane.YAxis.MajorGrid.IsVisible = yMajorGridVisible;
			graphPane.YAxis.MinorGrid.IsVisible = yMajorGridVisible;
			graphPane.YAxis.MajorTic.IsAllTics = false;
			graphPane.YAxis.MajorGrid.Color = Color.LightGray;
			graphPane.YAxis.MinorGrid.Color = Color.LightGray;
			graphPane.YAxis.MajorTic.IsOpposite = false;
			graphPane.YAxis.MajorTic.IsBetweenLabels = yIsBetweenLabels;
			graphPane.YAxis.MajorTic.Color = Color.LightGray;
			graphPane.YAxis.MinorTic.IsOpposite = false;
			graphPane.YAxis.MinorTic.Color = Color.LightGray;
			graphPane.BaseDimension = 3f;
		}

		public void UpdateLineGraph(double time, double value)
		{
			UpdateLineGraph(0, time, value);
		}

		public void UpdateLineGraph(int serie, double time, double value)
		{
			try
			{
				GraphPane pane = masterPane.PaneList[0];
				if (pane.CurveList.Count > 0)
				{
					LineItem item = pane.CurveList[serie] as LineItem;
					if (item != null)
					{
						IPointListEdit points = item.Points as IPointListEdit;
						if (points != null)
						{
							points.Add(time, value);
							if ((time > pane.XAxis.Scale.Max) || (pane.XAxis.Scale.Max > 10.0))
							{
								pane.XAxis.Scale.Max = time;
								pane.XAxis.Scale.Min = pane.XAxis.Scale.Max - 10.0;
							}
							pane.AxisChange();
							graph.Invalidate();
						}
					}
				}
			}
			catch
			{
				throw new Exception("While updating data graph");
			}
		}

		public ZedGraph.PaneList PaneList
		{
			get { return masterPane.PaneList; }
		}
	}
}
