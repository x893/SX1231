namespace SemtechLib.Controls.Graph
{
    using SemtechLib.Properties;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;

    public class GraphCtrl : UserControl
    {
        private Brush brushBackground;
        private Brush brushPoint;
        private Brush brushText;
        private CheckBox checkBoxAutoScale;
        private CheckBox checkBoxHand;
        private CheckBox checkBoxZoomIn;
        private CheckBox checkBoxZoomOut;
        private Color colorBackground;
        private Color colorFrame;
        private Color colorLine;
        private Color colorText;
        private IContainer components;
        private GraphDataCollection dataCollection = new GraphDataCollection();
        private Font fontText = new Font(FontFamily.GenericSansSerif, 8f);
        private bool frameFit;
        private GraphFrame frameFull = new GraphFrame();
        private GraphFrame frameZoom = new GraphFrame();
        private eGraphType graphType;
        private GraphGrid gridX = new GraphGrid();
        private GraphGrid gridY = new GraphGrid();
        private ToolTip grphToolTip;
        private int history;
        private Label labelSample;
        private Label labelScaleYmax;
        private Label labelScaleYmin;
        private int leftOffset;
        private Point mousePos = new Point();
        private Pen penFrame;
        private Pen penLine;
        private int previousClick = (SystemInformation.DoubleClickTime + 1);
        private int rightOffset;
        private GraphScale scaleX = new GraphScale();
        private GraphScale scaleY = new GraphScale();
        private TextBox textBoxScaleYmax;
        private TextBox textBoxScaleYmin;
        private System.Windows.Forms.Timer tmrRefresh;
        private bool updateData;
        private Rectangle workingArea = new Rectangle();
        private float zoomBottom;
        private float zoomLeft;
        private eZoomOption zoomOption;
        private float zoomRight;
        private float zoomTop;

        public GraphCtrl()
        {
            InitializeComponent();
            gridX.PropertyChanged += new GraphGrid.PropertyChangedEventHandler(Graph_Changed);
            gridY.PropertyChanged += new GraphGrid.PropertyChangedEventHandler(Graph_Changed);
            scaleX.PropertyChanged += new GraphScale.PropertyChangedEventHandler(Graph_Changed);
            scaleY.PropertyChanged += new GraphScale.PropertyChangedEventHandler(Graph_Changed);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            updateData = false;
            ColorBackground = Color.Black;
            ColorFrame = Color.Gray;
            ColorLine = Color.White;
            ColorText = Color.Gray;
            Type = eGraphType.Dot;
            History = 100;
            FrameFit = false;
            UpdateRate = 100;
            zoomOption = eZoomOption.None;
            zoomLeft = 0f;
            zoomRight = 0f;
            zoomTop = 0f;
            zoomBottom = 0f;
            ScaleX.Min = 0;
            ScaleX.Max = 100;
            ScaleX.Show = true;
            ScaleY.Min = 0;
            ScaleY.Max = 100;
            ScaleY.Show = true;
            GridX.Main = 0;
            GridX.Major = 0x19;
            GridX.Minor = 5;
            GridX.ShowMajor = true;
            GridX.ShowMinor = true;
            GridY.Main = 0;
            GridY.Major = 0x19;
            GridY.Minor = 5;
            GridY.ShowMajor = true;
            GridY.ShowMinor = true;
            Zoom = false;
            leftOffset = 0;
        }

        public void AddData(int series, int data, Color color, eGraphType graphType)
        {
            if (series >= dataCollection.Count)
            {
                dataCollection.Add(new GraphData(new List<int>(data), color, graphType));
            }
            dataCollection[series].Color = color;
            dataCollection[series].Value.Add(data);
            if (dataCollection[series].Value.Count > history)
            {
                dataCollection[series].Value.RemoveRange(0, dataCollection[series].Value.Count - history);
            }
            updateData = true;
        }

        private void checkBoxAutoScale_Click(object sender, EventArgs e)
        {
            int tickCount = Environment.TickCount;
            if ((tickCount - previousClick) <= SystemInformation.DoubleClickTime)
            {
                checkBoxAutoScale.Image = Resources.AutoSelected;
                checkBoxZoomIn.Image = Resources.ZoomIn;
                checkBoxZoomOut.Image = Resources.ZoomOut;
                checkBoxHand.Image = Resources.Move;
                checkBoxAutoScale.Checked = true;
                zoomOption = eZoomOption.AutoScale;
            }
            else if (checkBoxAutoScale.Checked)
            {
                checkBoxAutoScale.Checked = false;
                checkBoxAutoScale.Image = Resources.Auto;
                zoomOption = eZoomOption.None;
            }
            previousClick = tickCount;
        }

        private void checkBoxZoom_CheckedChanged(object sender, EventArgs e)
        {
            if (sender == checkBoxAutoScale)
            {
                if (checkBoxAutoScale.Checked)
                {
                    checkBoxAutoScale.Image = Resources.AutoSelected;
                    checkBoxZoomIn.Image = Resources.ZoomIn;
                    checkBoxZoomOut.Image = Resources.ZoomOut;
                    checkBoxHand.Image = Resources.Move;
                    checkBoxZoomIn.Checked = false;
                    checkBoxZoomOut.Checked = false;
                    checkBoxHand.Checked = false;
                    zoomOption = eZoomOption.AutoScale;
                }
                else
                {
                    checkBoxAutoScale.Image = Resources.Auto;
                    zoomOption = eZoomOption.None;
                }
            }
            else if (sender == checkBoxZoomIn)
            {
                if (checkBoxZoomIn.Checked)
                {
                    checkBoxAutoScale.Image = Resources.Auto;
                    checkBoxZoomIn.Image = Resources.ZoomInSelected;
                    checkBoxZoomOut.Image = Resources.ZoomOut;
                    checkBoxHand.Image = Resources.Move;
                    checkBoxAutoScale.Checked = false;
                    checkBoxZoomOut.Checked = false;
                    checkBoxHand.Checked = false;
                    zoomOption = eZoomOption.ZoomIn;
                }
                else
                {
                    checkBoxZoomIn.Image = Resources.ZoomIn;
                    zoomOption = eZoomOption.None;
                }
            }
            else if (sender == checkBoxZoomOut)
            {
                if (checkBoxZoomOut.Checked)
                {
                    checkBoxAutoScale.Image = Resources.Auto;
                    checkBoxZoomIn.Image = Resources.ZoomIn;
                    checkBoxZoomOut.Image = Resources.ZoomOutSelected;
                    checkBoxHand.Image = Resources.Move;
                    checkBoxAutoScale.Checked = false;
                    checkBoxZoomIn.Checked = false;
                    checkBoxHand.Checked = false;
                    zoomOption = eZoomOption.ZoomOut;
                }
                else
                {
                    checkBoxZoomOut.Image = Resources.ZoomOut;
                    zoomOption = eZoomOption.None;
                }
            }
            else if (sender == checkBoxHand)
            {
                if (checkBoxHand.Checked)
                {
                    checkBoxAutoScale.Image = Resources.Auto;
                    checkBoxZoomIn.Image = Resources.ZoomIn;
                    checkBoxZoomOut.Image = Resources.ZoomOut;
                    checkBoxHand.Image = Resources.MoveSelected;
                    checkBoxAutoScale.Checked = false;
                    checkBoxZoomIn.Checked = false;
                    checkBoxZoomOut.Checked = false;
                    zoomOption = eZoomOption.Hand;
                }
                else
                {
                    checkBoxHand.Image = Resources.Move;
                    zoomOption = eZoomOption.None;
                }
            }
            Refresh();
        }

        public void ClearData(int series)
        {
            dataCollection[series].Value.Clear();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private List<Point> GetPoints(GraphData graphData)
        {
            List<Point> list = new List<Point>();
            if ((frameFull.Height != 0) && (frameFull.Width != 0))
            {
                Point item = new Point();
                for (int i = 0; i < graphData.Value.Count; i++)
                {
                    int num = (frameFull.Right - graphData.Value.Count) + i;
                    item.Y = workingArea.Bottom - ((workingArea.Height * (graphData.Value[i] - frameFull.Bottom)) / frameFull.Height);
                    item.X = workingArea.Right - ((workingArea.Width * (frameFull.Right - num)) / (frameFull.Width - 1));
                    list.Add(item);
                }
            }
            return list;
        }

        private List<Point> GetPointsZoom(GraphData graphData)
        {
            List<Point> list = new List<Point>();
            if ((FrameZoom.Height != 0) && (FrameZoom.Width != 0))
            {
                Point item = new Point();
                for (int i = 0; i < graphData.Value.Count; i++)
                {
                    int num = (frameFull.Right - graphData.Value.Count) + i;
                    item.Y = workingArea.Bottom - ((workingArea.Height * (graphData.Value[i] - FrameZoom.Bottom)) / FrameZoom.Height);
                    item.X = workingArea.Right - ((workingArea.Width * (FrameZoom.Right - num)) / FrameZoom.Width);
                    list.Add(item);
                }
            }
            return list;
        }

        private void Graph_Changed()
        {
            frameFull.Left = scaleX.Min;
            frameFull.Right = scaleX.Max;
            frameFull.Top = scaleY.Max;
            frameFull.Bottom = scaleY.Min;
            updateData = true;
        }

        private void GraphCtrl_MouseDown(object sender, MouseEventArgs e)
        {
            switch (zoomOption)
            {
                case eZoomOption.None:
                case eZoomOption.AutoScale:
                case eZoomOption.FullScale:
                    break;

                case eZoomOption.ZoomIn:
                    if (e.Button != MouseButtons.Left)
                    {
                        break;
                    }
                    mousePos = e.Location;
                    return;

                case eZoomOption.ZoomOut:
                {
                    if (e.Button != MouseButtons.Left)
                    {
                        break;
                    }
                    GraphFrame frame = (GraphFrame) (FrameZoom * 1.5f);
                    FrameZoom = frame;
                    updateData = true;
                    return;
                }
                case eZoomOption.Hand:
                    if (e.Button == MouseButtons.Left)
                    {
                        mousePos = e.Location;
                        frameZoom = FrameZoom;
                    }
                    break;

                default:
                    return;
            }
        }

        private void GraphCtrl_MouseMove(object sender, MouseEventArgs e)
        {
            switch (zoomOption)
            {
                case eZoomOption.None:
                case eZoomOption.ZoomOut:
                case eZoomOption.AutoScale:
                    break;

                case eZoomOption.ZoomIn:
                {
                    if (e.Button != MouseButtons.Left)
                    {
                        break;
                    }
                    Refresh();
                    Graphics graphics = base.CreateGraphics();
                    Point point = new Point();
                    Point point2 = new Point();
                    point.X = (mousePos.X < e.X) ? mousePos.X : e.X;
                    point2.X = (mousePos.X >= e.X) ? mousePos.X : e.X;
                    point.Y = (mousePos.Y < e.Y) ? mousePos.Y : e.Y;
                    point2.Y = (mousePos.Y >= e.Y) ? mousePos.Y : e.Y;
                    Rectangle rect = new Rectangle(point.X, point.Y, point2.X - point.X, point2.Y - point.Y);
                    Rectangle rectangle2 = new Rectangle(point.X, point.Y, (point2.X - point.X) + 1, (point2.Y - point.Y) + 1);
                    graphics.SetClip(rectangle2);
                    Pen pen = new Pen(Color.Gray);
                    pen.DashStyle = DashStyle.Dot;
                    graphics.DrawRectangle(pen, rect);
                    return;
                }
                case eZoomOption.Hand:
                    if (e.Button == MouseButtons.Left)
                    {
                        GraphFrame frame = new GraphFrame();
                        workingArea = GraphWindow;
                        frame.Left = frameZoom.Left - ((frameZoom.Width * (e.X - mousePos.X)) / workingArea.Width);
                        frame.Right = frameZoom.Right - ((frameZoom.Width * (e.X - mousePos.X)) / workingArea.Width);
                        frame.Top = frameZoom.Top - ((frameZoom.Height * (mousePos.Y - e.Y)) / workingArea.Height);
                        frame.Bottom = frameZoom.Bottom - ((frameZoom.Height * (mousePos.Y - e.Y)) / workingArea.Height);
                        if (frame.Left < frameFull.Left)
                        {
                            frame.Left = frameFull.Left;
                            frame.Right = frameFull.Left + frameZoom.Width;
                        }
                        if (frame.Right > frameFull.Right)
                        {
                            frame.Right = frameFull.Right;
                            frame.Left = frameFull.Right - frameZoom.Width;
                        }
                        if (frame.Top > frameFull.Top)
                        {
                            frame.Top = frameFull.Top;
                            frame.Bottom = frameFull.Top - frameZoom.Height;
                        }
                        if (frame.Bottom < frameFull.Bottom)
                        {
                            frame.Bottom = frameFull.Bottom;
                            frame.Top = frameFull.Bottom + frameZoom.Height;
                        }
                        FrameZoom = frame;
                        updateData = true;
                    }
                    break;

                default:
                    return;
            }
        }

        private void GraphCtrl_MouseUp(object sender, MouseEventArgs e)
        {
            switch (zoomOption)
            {
                case eZoomOption.None:
                case eZoomOption.ZoomOut:
                case eZoomOption.AutoScale:
                case eZoomOption.Hand:
                case eZoomOption.FullScale:
                    return;

                case eZoomOption.ZoomIn:
                {
                    if (e.Button != MouseButtons.Left)
                    {
                        return;
                    }
                    int num = (mousePos.X < e.X) ? mousePos.X : e.X;
                    int num2 = (mousePos.X >= e.X) ? mousePos.X : e.X;
                    int num3 = (mousePos.Y < e.Y) ? mousePos.Y : e.Y;
                    int num4 = (mousePos.Y >= e.Y) ? mousePos.Y : e.Y;
                    if ((num != num2) && (num3 != num4))
                    {
                        GraphFrame frameZoom = new GraphFrame();
                        GraphFrame frame2 = new GraphFrame();
                        workingArea = GraphWindow;
                        frameZoom = FrameZoom;
                        frame2.Left = frameZoom.Left + ((frameZoom.Width * (num - workingArea.Left)) / workingArea.Width);
                        frame2.Right = frameZoom.Right - ((frameZoom.Width * (workingArea.Right - num2)) / workingArea.Width);
                        frame2.Top = frameZoom.Top - ((frameZoom.Height * (num3 - workingArea.Top)) / workingArea.Height);
                        frame2.Bottom = frameZoom.Bottom + ((frameZoom.Height * (workingArea.Bottom - num4)) / workingArea.Height);
                        FrameZoom = frame2;
                        break;
                    }
                    FrameZoom = (GraphFrame) (FrameZoom * 0.5f);
                    break;
                }
                default:
                    return;
            }
            updateData = true;
        }

        private void GraphCtrl_Paint(object sender, PaintEventArgs e)
        {
            if ((dataCollection.Count > 0) && (dataCollection[0].Value.Count > 0))
            {
                labelSample.Text = dataCollection[0].Value[dataCollection[0].Value.Count - 1].ToString();
            }
            if ((zoomOption == eZoomOption.AutoScale) && (dataCollection[0].Value.Count > 1))
            {
                int num2;
                int num = num2 = dataCollection[0].Value[0];
                for (int i = 1; i < dataCollection[0].Value.Count; i++)
                {
                    if (dataCollection[0].Value[i] < num)
                    {
                        num = dataCollection[0].Value[i];
                    }
                    if (dataCollection[0].Value[i] > num2)
                    {
                        num2 = dataCollection[0].Value[i];
                    }
                }
                if (num2 == num)
                {
                    num2++;
                    num--;
                }
                zoomTop = (frameFull.Top - (num2 + (((float) (num2 - num)) / 10f))) / ((float) frameFull.Height);
                zoomBottom = ((num - (((float) (num2 - num)) / 10f)) - frameFull.Bottom) / ((float) frameFull.Height);
                if (zoomTop < 0f)
                {
                    zoomTop = 0f;
                }
                if (zoomBottom < 0f)
                {
                    zoomBottom = 0f;
                }
                Graph_Changed();
            }
            e.Graphics.FillRectangle(brushBackground, base.ClientRectangle);
            workingArea = GraphWindow;
            if (!frameFit)
            {
                if (ScaleY.Show)
                {
                    labelScaleYmax.Text = FrameZoom.Top.ToString();
                    labelScaleYmin.Text = FrameZoom.Bottom.ToString();
                    Point point = new Point(0, GraphWindow.Top - (labelScaleYmax.Size.Height / 2));
                    labelScaleYmax.Location = point;
                    textBoxScaleYmax.Location = point;
                    point.Y = GraphWindow.Bottom - (labelScaleYmin.Size.Height / 2);
                    labelScaleYmin.Location = point;
                    textBoxScaleYmin.Location = point;
                    labelScaleYmax.Visible = true;
                    labelScaleYmin.Visible = true;
                    leftOffset = Math.Max((int) (labelScaleYmax.ClientRectangle.Width / 2), (int) (labelScaleYmin.ClientRectangle.Width / 2));
                }
                else
                {
                    labelScaleYmax.Visible = false;
                    labelScaleYmin.Visible = false;
                    leftOffset = 0;
                }
                if (ScaleX.Show)
                {
                }
            }
            else
            {
                labelScaleYmax.Visible = false;
                labelScaleYmin.Visible = false;
            }
            Point point2 = new Point();
            point2.X = (workingArea.Left + (workingArea.Width / 2)) - (labelSample.ClientRectangle.Width / 2);
            point2.Y = workingArea.Bottom - labelSample.ClientRectangle.Height;
            labelSample.Location = point2;
            e.Graphics.SetClip(workingArea);
            e.Graphics.DrawRectangle(penFrame, workingArea.X, workingArea.Y, workingArea.Width - 1, workingArea.Height - 1);
            penFrame.DashStyle = DashStyle.Dot;
            foreach (Line line in GridLinesX)
            {
                e.Graphics.DrawLine(penFrame, line.Pt1, line.Pt2);
            }
            foreach (Line line2 in GridLinesY)
            {
                e.Graphics.DrawLine(penFrame, line2.Pt1, line2.Pt2);
            }
            penFrame.DashStyle = DashStyle.Solid;
            int num4 = 0;
            foreach (GraphData item in dataCollection)
                if (item.GraphType == eGraphType.Bar)
                    num4++;
            int num5 = 0;
            foreach (GraphData data2 in dataCollection)
            {
                int num7;
                penLine.Color = data2.Color;
                brushPoint = new SolidBrush(data2.Color);
                List<Point> pointsZoom = GetPointsZoom(data2);
                switch (data2.GraphType)
                {
                    case eGraphType.Dot:
                        foreach (Point point3 in pointsZoom)
                        {
                            e.Graphics.FillEllipse(brushPoint, point3.X, point3.Y, 2, 2);
                        }
                        goto Label_0806;

                    case eGraphType.Line:
                        for (int j = 0; j < (pointsZoom.Count - 1); j++)
                        {
                            e.Graphics.DrawLine(penLine, pointsZoom[j], pointsZoom[j + 1]);
                        }
                        goto Label_0806;

                    case eGraphType.Bar:
                        if (pointsZoom.Count > 0)
                        {
                            num7 = workingArea.Bottom - ((workingArea.Height * -FrameZoom.Bottom) / FrameZoom.Height);
                            Point point7 = pointsZoom[pointsZoom.Count - 1];
                            if (point7.Y >= num7)
                            {
                                break;
                            }
                            Point point9 = pointsZoom[pointsZoom.Count - 1];
                            Point point10 = pointsZoom[pointsZoom.Count - 1];
                            e.Graphics.FillRectangle(brushPoint, new Rectangle(workingArea.Location.X + (num5 * (workingArea.Width / num4)), point9.Y, workingArea.Width / num4, num7 - point10.Y));
                        }
                        goto Label_0806;

                    default:
                        goto Label_0806;
                }
                Point point11 = pointsZoom[pointsZoom.Count - 1];
                if (point11.Y > num7)
                {
                    Point point13 = pointsZoom[pointsZoom.Count - 1];
                    e.Graphics.FillRectangle(brushPoint, new Rectangle(workingArea.Location.X + (num5 * (workingArea.Width / num4)), num7, workingArea.Width / num4, point13.Y - num7));
                }
                else
                {
                    penLine.Color = Color.Blue;
                    e.Graphics.DrawRectangle(penLine, (float) (workingArea.Location.X + (num5 * (workingArea.Width / num4))), (float) num7, (float) (workingArea.Width / num4), 0.1f);
                }
            Label_0806:
                num5++;
            }
            if (((FrameZoom.Top != frameFull.Top) || (FrameZoom.Bottom != frameFull.Bottom)) || ((FrameZoom.Left != frameFull.Left) || (FrameZoom.Right != frameFull.Right)))
            {
                Point point4 = new Point(workingArea.Location.X + (workingArea.Width / 50), workingArea.Location.Y + (workingArea.Height / 50));
                workingArea.Inflate((-2 * workingArea.Width) / 5, (-2 * workingArea.Height) / 5);
                workingArea.Location = point4;
                e.Graphics.SetClip(workingArea);
                e.Graphics.FillRectangle(brushBackground, workingArea);
                e.Graphics.DrawRectangle(penLine, workingArea.X, workingArea.Y, workingArea.Width - 1, workingArea.Height - 1);
                num5 = 0;
                foreach (GraphData data3 in dataCollection)
                {
                    penLine.Color = data3.Color;
                    brushPoint = new SolidBrush(data3.Color);
                    List<Point> points = GetPoints(data3);
                    switch (data3.GraphType)
                    {
                        case eGraphType.Dot:
                            foreach (Point point5 in points)
                            {
                                e.Graphics.FillEllipse(brushPoint, point5.X, point5.Y, 2, 2);
                            }
                            break;

                        case eGraphType.Line:
                            for (int k = 0; k < (points.Count - 1); k++)
                            {
                                e.Graphics.DrawLine(penLine, points[k], points[k + 1]);
                            }
                            break;

                        case eGraphType.Bar:
                            if (points.Count > 0)
                            {
                                Point point18 = points[points.Count - 1];
                                e.Graphics.FillRectangle(brushPoint, new Rectangle(workingArea.Location.X + (num5 * (workingArea.Width / num4)), point18.Y, workingArea.Width / num4, workingArea.Location.Y + workingArea.Height));
                            }
                            break;
                    }
                    num5++;
                }
                penFrame.DashStyle = DashStyle.Dot;
                e.Graphics.DrawRectangle(penFrame, ZoomRect);
                penFrame.DashStyle = DashStyle.Solid;
            }
            if (Zoom)
            {
                int x = base.ClientRectangle.Right - (checkBoxAutoScale.Width + 3);
                int y = (base.ClientRectangle.Bottom - base.ClientRectangle.Top) / 2;
                Point point6 = new Point(x, y - (checkBoxAutoScale.Height * 2));
                checkBoxAutoScale.Location = point6;
                point6 = new Point(x, y - checkBoxAutoScale.Height);
                checkBoxZoomIn.Location = point6;
                point6 = new Point(x, y);
                checkBoxZoomOut.Location = point6;
                point6 = new Point(x, y + checkBoxAutoScale.Height);
                checkBoxHand.Location = point6;
            }
        }

        private void InitializeComponent()
        {
            components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(GraphCtrl));
            tmrRefresh = new System.Windows.Forms.Timer(components);
            labelScaleYmin = new Label();
            labelScaleYmax = new Label();
            textBoxScaleYmax = new TextBox();
            textBoxScaleYmin = new TextBox();
            labelSample = new Label();
            grphToolTip = new ToolTip(components);
            checkBoxHand = new CheckBox();
            checkBoxZoomOut = new CheckBox();
            checkBoxZoomIn = new CheckBox();
            checkBoxAutoScale = new CheckBox();
            base.SuspendLayout();
            tmrRefresh.Enabled = true;
            tmrRefresh.Interval = 0x19;
            tmrRefresh.Tick += new EventHandler(tmrRefresh_Tick);
            labelScaleYmin.AutoSize = true;
            labelScaleYmin.Location = new Point(4, 0x2a);
            labelScaleYmin.Name = "labelScaleYmin";
            labelScaleYmin.Size = new Size(0x17, 13);
            labelScaleYmin.TabIndex = 1;
            labelScaleYmin.Text = "min";
            labelScaleYmin.Click += new EventHandler(labelScaleY_Click);
            labelScaleYmax.AutoSize = true;
            labelScaleYmax.Location = new Point(4, 0x1c);
            labelScaleYmax.Name = "labelScaleYmax";
            labelScaleYmax.Size = new Size(0x1a, 13);
            labelScaleYmax.TabIndex = 1;
            labelScaleYmax.Text = "max";
            labelScaleYmax.Click += new EventHandler(labelScaleY_Click);
            textBoxScaleYmax.BorderStyle = System.Windows.Forms.BorderStyle.None;
            textBoxScaleYmax.Location = new Point(0x24, 0x1c);
            textBoxScaleYmax.Name = "textBoxScaleYmax";
            textBoxScaleYmax.Size = new Size(0x2f, 13);
            textBoxScaleYmax.TabIndex = 2;
            textBoxScaleYmax.Text = "max";
            textBoxScaleYmax.Visible = false;
            textBoxScaleYmax.KeyPress += new KeyPressEventHandler(textBoxScaleY_KeyPress);
            textBoxScaleYmax.Validated += new EventHandler(textBoxScaleY_Validated);
            textBoxScaleYmin.BorderStyle = System.Windows.Forms.BorderStyle.None;
            textBoxScaleYmin.Location = new Point(0x24, 0x2a);
            textBoxScaleYmin.Name = "textBoxScaleYmin";
            textBoxScaleYmin.Size = new Size(0x2f, 13);
            textBoxScaleYmin.TabIndex = 2;
            textBoxScaleYmin.Text = "min";
            textBoxScaleYmin.Visible = false;
            textBoxScaleYmin.KeyPress += new KeyPressEventHandler(textBoxScaleY_KeyPress);
            textBoxScaleYmin.Validated += new EventHandler(textBoxScaleY_Validated);
            labelSample.AutoSize = true;
            labelSample.BackColor = Color.Transparent;
            labelSample.Location = new Point(50, 0x80);
            labelSample.Name = "labelSample";
            labelSample.Size = new Size(0x2a, 13);
            labelSample.TabIndex = 3;
            labelSample.Text = "Sample";
            checkBoxHand.Anchor = AnchorStyles.Right;
            checkBoxHand.Appearance = Appearance.Button;
            checkBoxHand.Image = Resources.Move;
            checkBoxHand.Location = new Point(0x7f, 0x6a);
            checkBoxHand.Name = "checkBoxHand";
            checkBoxHand.Size = new Size(0x1a, 0x1a);
            checkBoxHand.TabIndex = 0;
            checkBoxHand.TextAlign = ContentAlignment.MiddleCenter;
            grphToolTip.SetToolTip(checkBoxHand, "Move:\r\n\r\n-Left Mouse Button Click on the zone you want to move and move the mouse.");
            checkBoxHand.UseVisualStyleBackColor = true;
            checkBoxHand.CheckedChanged += new EventHandler(checkBoxZoom_CheckedChanged);
            checkBoxZoomOut.Anchor = AnchorStyles.Right;
            checkBoxZoomOut.Appearance = Appearance.Button;
            checkBoxZoomOut.Image = Resources.ZoomOut;
            checkBoxZoomOut.Location = new Point(0x7f, 80);
            checkBoxZoomOut.Name = "checkBoxZoomOut";
            checkBoxZoomOut.Size = new Size(0x1a, 0x1a);
            checkBoxZoomOut.TabIndex = 0;
            checkBoxZoomOut.TextAlign = ContentAlignment.MiddleCenter;
            grphToolTip.SetToolTip(checkBoxZoomOut, "ZoomOut:\r\n\r\nSelect the button and then each time the graphic is clicked it will zoom out.");
            checkBoxZoomOut.UseVisualStyleBackColor = true;
            checkBoxZoomOut.CheckedChanged += new EventHandler(checkBoxZoom_CheckedChanged);
            checkBoxZoomIn.Anchor = AnchorStyles.Right;
            checkBoxZoomIn.Appearance = Appearance.Button;
            checkBoxZoomIn.Image = Resources.ZoomIn;
            checkBoxZoomIn.Location = new Point(0x7f, 0x36);
            checkBoxZoomIn.Name = "checkBoxZoomIn";
            checkBoxZoomIn.Size = new Size(0x1a, 0x1a);
            checkBoxZoomIn.TabIndex = 0;
            checkBoxZoomIn.TextAlign = ContentAlignment.MiddleCenter;
            grphToolTip.SetToolTip(checkBoxZoomIn, "ZoomIn:\r\n\r\nDraw a rectangle with the Left Mouse button on the graphic zone to zoom");
            checkBoxZoomIn.UseVisualStyleBackColor = true;
            checkBoxZoomIn.CheckedChanged += new EventHandler(checkBoxZoom_CheckedChanged);
            checkBoxAutoScale.Anchor = AnchorStyles.Right;
            checkBoxAutoScale.Appearance = Appearance.Button;
            checkBoxAutoScale.Image = Resources.Auto;
            checkBoxAutoScale.Location = new Point(0x7f, 0x1c);
            checkBoxAutoScale.Name = "checkBoxAutoScale";
            checkBoxAutoScale.Size = new Size(0x1a, 0x1a);
            checkBoxAutoScale.TabIndex = 0;
            checkBoxAutoScale.TextAlign = ContentAlignment.MiddleCenter;
            grphToolTip.SetToolTip(checkBoxAutoScale, resources.GetString("checkBoxAutoScale.ToolTip"));
            checkBoxAutoScale.UseVisualStyleBackColor = true;
            checkBoxAutoScale.Click += new EventHandler(checkBoxAutoScale_Click);
            checkBoxAutoScale.CheckedChanged += new EventHandler(checkBoxZoom_CheckedChanged);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.Controls.Add(labelSample);
            base.Controls.Add(textBoxScaleYmin);
            base.Controls.Add(textBoxScaleYmax);
            base.Controls.Add(labelScaleYmax);
            base.Controls.Add(labelScaleYmin);
            base.Controls.Add(checkBoxHand);
            base.Controls.Add(checkBoxZoomOut);
            base.Controls.Add(checkBoxZoomIn);
            base.Controls.Add(checkBoxAutoScale);
            base.Name = "GraphCtrl";
            base.Size = new Size(0x9b, 0xa1);
            base.MouseDown += new MouseEventHandler(GraphCtrl_MouseDown);
            base.MouseMove += new MouseEventHandler(GraphCtrl_MouseMove);
            base.Paint += new PaintEventHandler(GraphCtrl_Paint);
            base.MouseUp += new MouseEventHandler(GraphCtrl_MouseUp);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void labelScaleY_Click(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            TextBox textBox = !label.Equals((object)this.labelScaleYmax) ? this.textBoxScaleYmin : this.textBoxScaleYmax;
            textBox.Text = label.Text;
            textBox.Visible = true;
            textBox.Focus();
            textBox.SelectAll();
        }

        private void textBoxScaleY_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!char.IsDigit(e.KeyChar) && (e.KeyChar != '\b')) && (e.KeyChar != '-'))
            {
                e.Handled = true;
                if (e.KeyChar == '\r')
                {
                    textBoxScaleY_Validated(sender, new EventArgs());
                }
            }
        }

        private void textBoxScaleY_Validated(object sender, EventArgs e)
        {
            TextBox box = (TextBox) sender;
            box.Visible = false;
            GraphFrame frameZoom = FrameZoom;
            if (box == textBoxScaleYmax)
            {
                frameZoom.Top = Convert.ToInt32(box.Text);
            }
            else if (box == textBoxScaleYmin)
            {
                frameZoom.Bottom = Convert.ToInt32(box.Text);
            }
            FrameZoom = frameZoom;
            updateData = true;
        }

        private void tmrRefresh_Tick(object sender, EventArgs e)
        {
            if (updateData)
            {
                base.Invalidate();
            }
            updateData = false;
        }

        [Category("Graph"), Description("Enable the Autoscale features."), DefaultValue(false)]
        public bool AutoScale
        {
            get
            {
                if (zoomOption != eZoomOption.AutoScale)
                {
                    return false;
                }
                return true;
            }
            set
            {
                if (value)
                {
                    zoomOption = eZoomOption.AutoScale;
                }
                else
                {
                    zoomOption = eZoomOption.None;
                }
            }
        }

        [Category("Graph"), Description("Color of the Graph Background")]
        public Color ColorBackground
        {
            get
            {
                return colorBackground;
            }
            set
            {
                colorBackground = value;
                brushBackground = new SolidBrush(colorBackground);
                labelScaleYmax.BackColor = colorBackground;
                textBoxScaleYmax.BackColor = colorBackground;
                labelScaleYmin.BackColor = colorBackground;
                textBoxScaleYmin.BackColor = colorBackground;
                labelSample.BackColor = Color.Transparent;
                updateData = true;
            }
        }

        [Category("Graph"), Description("Color of the Graph Frame")]
        public Color ColorFrame
        {
            get
            {
                return colorFrame;
            }
            set
            {
                colorFrame = value;
                penFrame = new Pen(colorFrame);
                updateData = true;
            }
        }

        [Description("Color of the Graph Line"), Category("Graph")]
        public Color ColorLine
        {
            get
            {
                return colorLine;
            }
            set
            {
                colorLine = value;
                penLine = new Pen(colorLine);
                brushPoint = new SolidBrush(colorLine);
                updateData = true;
            }
        }

        [Category("Graph"), Description("Color of the Graph Text")]
        public Color ColorText
        {
            get
            {
                return colorText;
            }
            set
            {
                colorText = value;
                brushText = new SolidBrush(colorText);
                labelScaleYmax.ForeColor = colorText;
                textBoxScaleYmax.ForeColor = colorText;
                labelScaleYmin.ForeColor = colorText;
                textBoxScaleYmin.ForeColor = colorText;
                labelSample.ForeColor = colorText;
                updateData = true;
            }
        }

        [Category("Graph"), DefaultValue(false), Description("Let the Frame Fit in the Control Window")]
        public bool FrameFit
        {
            get
            {
                return frameFit;
            }
            set
            {
                frameFit = value;
                updateData = true;
            }
        }

        private GraphFrame FrameZoom
        {
            get
            {
                GraphFrame frame = new GraphFrame();
                frame.Left = frameFull.Left + ((int) (frameFull.Width * zoomLeft));
                frame.Right = frameFull.Right - ((int) (frameFull.Width * zoomRight));
                frame.Top = frameFull.Top - ((int) (frameFull.Height * zoomTop));
                frame.Bottom = frameFull.Bottom + ((int) (frameFull.Height * zoomBottom));
                return frame;
            }
            set
            {
                zoomLeft = ((float) (value.Left - frameFull.Left)) / ((float) frameFull.Width);
                zoomRight = ((float) (frameFull.Right - value.Right)) / ((float) frameFull.Width);
                zoomTop = ((float) (frameFull.Top - value.Top)) / ((float) frameFull.Height);
                zoomBottom = ((float) (value.Bottom - frameFull.Bottom)) / ((float) frameFull.Height);
                if (zoomLeft < 0f)
                {
                    zoomLeft = 0f;
                }
                if (zoomRight < 0f)
                {
                    zoomRight = 0f;
                }
                if (zoomTop < 0f)
                {
                    zoomTop = 0f;
                }
                if (zoomBottom < 0f)
                {
                    zoomBottom = 0f;
                }
            }
        }

        [DefaultValue(false), Description("Sets the graph to full scale."), Category("Graph")]
        public bool FullScale
        {
            get
            {
                if (zoomOption != eZoomOption.FullScale)
                {
                    return false;
                }
                return true;
            }
            set
            {
                if (value)
                {
                    zoomOption = eZoomOption.FullScale;
                    zoomLeft = 0f;
                    zoomRight = 0f;
                    zoomTop = 0f;
                    zoomBottom = 0f;
                }
                else
                {
                    zoomOption = eZoomOption.None;
                }
            }
        }

        private Rectangle GraphWindow
        {
            get
            {
                Rectangle clientRectangle = base.ClientRectangle;
                if (!frameFit)
                {
                    clientRectangle.Inflate((((-5 * clientRectangle.Width) / 100) - rightOffset) - leftOffset, (-5 * clientRectangle.Height) / 100);
                    Point point = new Point(clientRectangle.Location.X, clientRectangle.Location.Y);
                    clientRectangle.Location = point;
                }
                return clientRectangle;
            }
        }

        private List<Line> GridLinesX
        {
            get
            {
                int minor;
                List<Line> list = new List<Line>();
                Point point = new Point(0, workingArea.Top);
                Point point2 = new Point(0, workingArea.Bottom);
                if (GridX.ShowMinor)
                {
                    minor = GridX.Minor;
                }
                else if (GridX.ShowMajor)
                {
                    minor = GridX.Major;
                }
                else
                {
                    return list;
                }
                int main = GridX.Main;
                while (main > FrameZoom.Left)
                {
                    main -= minor;
                }
                while (main < FrameZoom.Left)
                {
                    main += minor;
                }
                while (main < FrameZoom.Right)
                {
                    point.X = point2.X = workingArea.Left + ((workingArea.Width * (main - FrameZoom.Left)) / FrameZoom.Width);
                    list.Add(new Line(point, point2));
                    main += minor;
                }
                return list;
            }
        }

        private List<Line> GridLinesY
        {
            get
            {
                int minor;
                List<Line> list = new List<Line>();
                Point point = new Point(workingArea.Left, 0);
                Point point2 = new Point(workingArea.Right, 0);
                if (GridY.ShowMinor)
                {
                    minor = GridY.Minor;
                }
                else if (GridY.ShowMajor)
                {
                    minor = GridY.Major;
                }
                else
                {
                    return list;
                }
                int main = GridY.Main;
                while (main > FrameZoom.Bottom)
                {
                    main -= minor;
                }
                while (main < FrameZoom.Bottom)
                {
                    main += minor;
                }
                while (main < FrameZoom.Top)
                {
                    point.Y = point2.Y = workingArea.Bottom - ((workingArea.Height * (main - FrameZoom.Bottom)) / FrameZoom.Height);
                    list.Add(new Line(point, point2));
                    main += minor;
                }
                return list;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public GraphGrid GridX
        {
            get
            {
                return gridX;
            }
            set
            {
                gridX = value;
                updateData = true;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public GraphGrid GridY
        {
            get
            {
                return gridY;
            }
            set
            {
                gridY = value;
                updateData = true;
            }
        }

        [DefaultValue(100), Description("How many point to keep in hystory"), Category("Graph")]
        public int History
        {
            get
            {
                return history;
            }
            set
            {
                history = value;
                foreach (GraphData data in dataCollection)
                {
                    if (data.Value.Count > history)
                    {
                        data.Value.RemoveRange(0, data.Value.Count - history);
                    }
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public GraphScale ScaleX
        {
            get
            {
                return scaleX;
            }
            set
            {
                scaleX = value;
                updateData = true;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public GraphScale ScaleY
        {
            get
            {
                return scaleY;
            }
            set
            {
                scaleY = value;
                updateData = true;
            }
        }

        [DefaultValue(0), Category("Graph"), Description("Graph Type")]
        public eGraphType Type
        {
            get
            {
                return graphType;
            }
            set
            {
                graphType = value;
                updateData = true;
            }
        }

        [Description("Update Rate in milliseconds"), Category("Graph"), DefaultValue(0x19)]
        public int UpdateRate
        {
            get
            {
                return tmrRefresh.Interval;
            }
            set
            {
                tmrRefresh.Interval = value;
            }
        }

        [Description("Enable the Zoom features."), DefaultValue(false), Category("Graph")]
        public bool Zoom
        {
            get
            {
                return checkBoxAutoScale.Visible;
            }
            set
            {
                checkBoxAutoScale.Visible = value;
                checkBoxZoomIn.Visible = value;
                checkBoxZoomOut.Visible = value;
                checkBoxHand.Visible = value;
                if (value)
                {
                    rightOffset = checkBoxAutoScale.ClientRectangle.Width / 2;
                }
                else
                {
                    rightOffset = 0;
                }
                updateData = true;
            }
        }

        private Rectangle ZoomRect
        {
            get
            {
                Point location = new Point();
                location.X = workingArea.Left + ((int) (workingArea.Width * zoomLeft));
                location.Y = workingArea.Top + ((int) (workingArea.Height * zoomTop));
                Size size = new Size();
                size.Width = (workingArea.Width * FrameZoom.Width) / frameFull.Width;
                size.Height = (workingArea.Height * FrameZoom.Height) / frameFull.Height;
                return new Rectangle(location, size);
            }
        }

        public enum eGraphType
        {
            Dot,
            Line,
            Bar
        }

        public enum eZoomOption
        {
            None,
            ZoomIn,
            ZoomOut,
            AutoScale,
            Hand,
            FullScale
        }

        public class GraphData
        {
            protected System.Drawing.Color _color;
            protected System.Drawing.Color _colorGradient;
            protected GraphCtrl.eGraphType _graphType;
            protected int _maxWidth;
            protected string _text;
            protected List<int> _value;

            public GraphData()
            {
                _value = new List<int>();
                _maxWidth = 15;
                _graphType = GraphCtrl.eGraphType.Line;
            }

            public GraphData(List<int> value, System.Drawing.Color color, GraphCtrl.eGraphType graphType)
            {
                _value = new List<int>();
                _maxWidth = 15;
                _graphType = GraphCtrl.eGraphType.Line;
                _value = value;
                _color = color;
                _graphType = graphType;
            }

            public GraphData(List<int> value, System.Drawing.Color color, System.Drawing.Color colorGradient, GraphCtrl.eGraphType graphType)
            {
                _value = new List<int>();
                _maxWidth = 15;
                _graphType = GraphCtrl.eGraphType.Line;
                _value = value;
                _color = color;
                _colorGradient = colorGradient;
                _graphType = graphType;
            }

            public GraphData(List<int> value, System.Drawing.Color color, string text, GraphCtrl.eGraphType graphType)
            {
                _value = new List<int>();
                _maxWidth = 15;
                _graphType = GraphCtrl.eGraphType.Line;
                _value = value;
                _color = color;
                _text = text;
                _graphType = graphType;
            }

            public GraphData(List<int> value, System.Drawing.Color color, System.Drawing.Color colorGradient, string text, GraphCtrl.eGraphType graphType)
            {
                _value = new List<int>();
                _maxWidth = 15;
                _graphType = GraphCtrl.eGraphType.Line;
                _value = value;
                _color = color;
                _text = text;
                _colorGradient = colorGradient;
                _graphType = graphType;
            }

            public System.Drawing.Color Color
            {
                get
                {
                    return _color;
                }
                set
                {
                    _color = value;
                }
            }

            public System.Drawing.Color ColorGradient
            {
                get
                {
                    if (_colorGradient == System.Drawing.Color.Empty)
                    {
                        return _color;
                    }
                    return _colorGradient;
                }
                set
                {
                    _colorGradient = value;
                }
            }

            public GraphCtrl.eGraphType GraphType
            {
                get
                {
                    return _graphType;
                }
                set
                {
                    _graphType = value;
                }
            }

            public int MaxWidth
            {
                get
                {
                    return _maxWidth;
                }
                set
                {
                    _maxWidth = value;
                }
            }

            public string Text
            {
                get
                {
                    return _text;
                }
                set
                {
                    _text = value;
                }
            }

            public List<int> Value
            {
                get
                {
                    return _value;
                }
                set
                {
                    _value = value;
                }
            }
        }

        [Serializable]
        public class GraphDataCollection : CollectionBase
        {
            public GraphDataCollection()
            {
            }

            public GraphDataCollection(GraphCtrl.GraphDataCollection value)
            {
                AddRange(value);
            }

            public GraphDataCollection(GraphCtrl.GraphData[] value)
            {
                AddRange(value);
            }

            public int Add(GraphCtrl.GraphData value)
            {
                return base.List.Add(value);
            }

            public void AddRange(GraphCtrl.GraphData[] value)
            {
                for (int i = 0; i < value.Length; i++)
                {
                    Add(value[i]);
                }
            }

            public void AddRange(GraphCtrl.GraphDataCollection value)
            {
                for (int i = 0; i < value.Count; i++)
                {
                    Add(value[i]);
                }
            }

            public bool Contains(GraphCtrl.GraphData value)
            {
                return base.List.Contains(value);
            }

            public void CopyTo(GraphCtrl.GraphData[] array, int index)
            {
                base.List.CopyTo(array, index);
            }

            public new GraphDataEnumerator GetEnumerator()
            {
                return new GraphDataEnumerator(this);
            }

            public int IndexOf(GraphCtrl.GraphData value)
            {
                return base.List.IndexOf(value);
            }

            public void Insert(int index, GraphCtrl.GraphData value)
            {
                base.List.Insert(index, value);
            }

            public void Remove(GraphCtrl.GraphData value)
            {
                base.Capacity--;
                base.List.Remove(value);
            }

            public GraphCtrl.GraphData this[int index]
            {
                get
                {
                    return (GraphCtrl.GraphData) base.List[index];
                }
                set
                {
                    base.List[index] = value;
                }
            }

            public class GraphDataEnumerator : IEnumerator
            {
                private IEnumerator baseEnumerator;
                private IEnumerable temp;

                public GraphDataEnumerator(GraphCtrl.GraphDataCollection mappings)
                {
                    temp = mappings;
                    baseEnumerator = temp.GetEnumerator();
                }

                public bool MoveNext()
                {
                    return baseEnumerator.MoveNext();
                }

                public void Reset()
                {
                    baseEnumerator.Reset();
                }

                bool IEnumerator.MoveNext()
                {
                    return baseEnumerator.MoveNext();
                }

                void IEnumerator.Reset()
                {
                    baseEnumerator.Reset();
                }

                public GraphCtrl.GraphData Current
                {
                    get
                    {
                        return (GraphCtrl.GraphData) baseEnumerator.Current;
                    }
                }

                object IEnumerator.Current
                {
                    get
                    {
                        return baseEnumerator.Current;
                    }
                }
            }
        }

        private class GraphFrame
        {
            private int bottom;
            private int left;
            private int right;
            private int top;

            public static GraphCtrl.GraphFrame operator *(GraphCtrl.GraphFrame gf, float times)
            {
                GraphCtrl.GraphFrame frame = new GraphCtrl.GraphFrame();
                frame.Right = (gf.Right - (gf.Width / 2)) + ((int) ((times * gf.Width) / 2f));
                frame.Top = (gf.Top - (gf.Height / 2)) + ((int) ((times * gf.Height) / 2f));
                frame.Left = gf.Left + ((int) ((gf.Width * (1f - times)) / 2f));
                frame.Bottom = gf.Bottom + ((int) ((gf.Height * (1f - times)) / 2f));
                return frame;
            }

            public int Bottom
            {
                get
                {
                    return bottom;
                }
                set
                {
                    bottom = value;
                }
            }

            public int Height
            {
                get
                {
                    return (Top - Bottom);
                }
            }

            public int Left
            {
                get
                {
                    return left;
                }
                set
                {
                    left = value;
                }
            }

            public int Right
            {
                get
                {
                    return right;
                }
                set
                {
                    right = value;
                }
            }

            public int Top
            {
                get
                {
                    return top;
                }
                set
                {
                    top = value;
                }
            }

            public int Width
            {
                get
                {
                    return (Right - Left);
                }
            }
        }

        [Description("Graph Grid."), Category("Graph"), TypeConverter(typeof(GraphCtrl.GraphGridTypeConverter))]
        public class GraphGrid
        {
            private int main;
            private int major;
            private int minor;
            private bool showMajor;
            private bool showMinor;

            public event PropertyChangedEventHandler PropertyChanged;

            public override string ToString()
            {
                return string.Concat(new object[] { main, "; ", major, "; ", minor, "; ", showMajor, "; ", showMinor });
            }

            [Description("Main axe value from which the grid will depends.")]
            public int Main
            {
                get
                {
                    return main;
                }
                set
                {
                    main = value;
                    PropertyChanged();
                }
            }

            [Description("Major Grid Unit.")]
            public int Major
            {
                get
                {
                    return major;
                }
                set
                {
                    major = value;
                    PropertyChanged();
                }
            }

            [Description("Minor Grid Unit.")]
            public int Minor
            {
                get
                {
                    return minor;
                }
                set
                {
                    minor = value;
                    PropertyChanged();
                }
            }

            [Description("Show Major Grid.")]
            public bool ShowMajor
            {
                get
                {
                    return showMajor;
                }
                set
                {
                    showMajor = value;
                    PropertyChanged();
                }
            }

            [Description("Show Minor Grid.")]
            public bool ShowMinor
            {
                get
                {
                    return showMinor;
                }
                set
                {
                    showMinor = value;
                    PropertyChanged();
                }
            }

            public delegate void PropertyChangedEventHandler();
        }

        private class GraphGridTypeConverter : TypeConverter
        {
            public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
            {
                return TypeDescriptor.GetProperties(typeof(GraphCtrl.GraphGrid));
            }

            public override bool GetPropertiesSupported(ITypeDescriptorContext context)
            {
                return true;
            }
        }

        [Description("Graph Scale."), TypeConverter(typeof(GraphCtrl.GraphScaleTypeConverter)), Category("Graph")]
        public class GraphScale
        {
            private int max;
            private int min;
            private bool show;

            public event PropertyChangedEventHandler PropertyChanged;

            public override string ToString()
            {
                return string.Concat(new object[] { max, "; ", min, "; ", show });
            }

            [Description("Maximum Scale value.")]
            public int Max
            {
                get
                {
                    return max;
                }
                set
                {
                    max = value;
                    PropertyChanged();
                }
            }

            [Description("Minimum Scale value.")]
            public int Min
            {
                get
                {
                    return min;
                }
                set
                {
                    min = value;
                    PropertyChanged();
                }
            }

            [Description("If true, the scale will be show on the Graph.")]
            public bool Show
            {
                get
                {
                    return show;
                }
                set
                {
                    show = value;
                    PropertyChanged();
                }
            }

            public delegate void PropertyChangedEventHandler();
        }

        public class GraphScaleTypeConverter : TypeConverter
        {
            public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
            {
                return TypeDescriptor.GetProperties(typeof(GraphCtrl.GraphScale));
            }

            public override bool GetPropertiesSupported(ITypeDescriptorContext context)
            {
                return true;
            }
        }

        private class Line
        {
            private Point pt1 = new Point();
            private Point pt2 = new Point();

            public Line(Point pt1, Point pt2)
            {
                this.pt1 = pt1;
                this.pt2 = pt2;
            }

            public Point Pt1
            {
                get
                {
                    return pt1;
                }
                set
                {
                    pt1 = value;
                }
            }

            public Point Pt2
            {
                get
                {
                    return pt2;
                }
                set
                {
                    pt2 = value;
                }
            }
        }
    }
}

