namespace SemtechLib.Controls.HexBoxCtrl
{
    using SemtechLib.Controls.HexBoxCtrl.Design;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;
    using System.Drawing.Drawing2D;
    using System.Globalization;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Security.Permissions;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;
    using System.Windows.Forms.VisualStyles;

    [ToolboxBitmap(typeof(HexBox), "HexBox.bmp")]
    public class HexBox : Control
    {
        private bool _abortFind;
        private Color _backColorDisabled = Color.FromName("WhiteSmoke");
        private System.Windows.Forms.BorderStyle _borderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
        private int _byteCharacterPos;
        private long _bytePos = -1L;
        private IByteProvider _byteProvider;
        private int _bytesPerLine = 0x10;
        private bool _caretVisible;
        private SizeF _charSize;
        private long _currentLine;
        private int _currentPositionInLine;
        private EmptyKeyInterpreter _eki;
        private long _endByte;
        private long _findingPos;
        private string _hexStringFormat = "X";
        private int _iHexMaxBytes;
        private int _iHexMaxHBytes;
        private int _iHexMaxVBytes;
        private bool _insertActive;
        private IKeyInterpreter _keyInterpreter;
        private KeyInterpreter _ki;
        private int _lastThumbtrack;
        private Color _lineInfoForeColor = Color.Empty;
        private bool _lineInfoVisible;
        private bool _readOnly;
        private int _recBorderBottom = SystemInformation.Border3DSize.Height;
        private int _recBorderLeft = SystemInformation.Border3DSize.Width;
        private int _recBorderRight = SystemInformation.Border3DSize.Width;
        private int _recBorderTop = SystemInformation.Border3DSize.Height;
        private Rectangle _recContent;
        private Rectangle _recHex;
        private Rectangle _recLineInfo;
        private Rectangle _recStringView;
        private long _scrollVmax;
        private long _scrollVmin;
        private long _scrollVpos;
        private Color _selectionBackColor = Color.Blue;
        private Color _selectionForeColor = Color.White;
        private long _selectionLength;
        private Color _shadowSelectionColor = Color.FromArgb(100, 60, 0xbc, 0xff);
        private bool _shadowSelectionVisible = true;
        private StringKeyInterpreter _ski;
        private long _startByte;
        private StringFormat _stringFormat;
        private bool _stringViewVisible;
        private long _thumbTrackPosition;
        private System.Windows.Forms.Timer _thumbTrackTimer = new System.Windows.Forms.Timer();
        private bool _useFixedBytesPerLine;
        private VScrollBar _vScrollBar = new VScrollBar();
        private bool _vScrollBarVisible;
        private byte lineInfoDigits = 2;
        private const int THUMPTRACKDELAY = 50;

        [Description("Occurs, when the value of BorderStyle property has changed.")]
        public event EventHandler BorderStyleChanged;

        [Description("Occurs, when the value of ByteProvider property has changed.")]
        public event EventHandler ByteProviderChanged;

        [Description("Occurs, when the value of BytesPerLine property has changed.")]
        public event EventHandler BytesPerLineChanged;

        [Description("Occurs, when Copy method was invoked and ClipBoardData changed.")]
        public event EventHandler Copied;

        [Description("Occurs, when CopyHex method was invoked and ClipBoardData changed.")]
        public event EventHandler CopiedHex;

        [Description("Occurs, when the value of CurrentLine property has changed.")]
        public event EventHandler CurrentLineChanged;

        [Description("Occurs, when the value of CurrentPositionInLine property has changed.")]
        public event EventHandler CurrentPositionInLineChanged;

        [Description("Occurs, when the value of HexCasing property has changed.")]
        public event EventHandler HexCasingChanged;

        [Description("Occurs, when the value of HorizontalByteCount property has changed.")]
        public event EventHandler HorizontalByteCountChanged;

        [Description("Occurs, when the value of InsertActive property has changed.")]
        public event EventHandler InsertActiveChanged;

        [Description("Occurs, when the value of LineInfoVisible property has changed.")]
        public event EventHandler LineInfoVisibleChanged;

        [Description("Occurs, when the value of ReadOnly property has changed.")]
        public event EventHandler ReadOnlyChanged;

        [Description("Occurs, when the value of SelectionLength property has changed.")]
        public event EventHandler SelectionLengthChanged;

        [Description("Occurs, when the value of SelectionStart property has changed.")]
        public event EventHandler SelectionStartChanged;

        [Description("Occurs, when the value of StringViewVisible property has changed.")]
        public event EventHandler StringViewVisibleChanged;

        [Description("Occurs, when the value of UseFixedBytesPerLine property has changed.")]
        public event EventHandler UseFixedBytesPerLineChanged;

        [Description("Occurs, when the value of VerticalByteCount property has changed.")]
        public event EventHandler VerticalByteCountChanged;

        [Description("Occurs, when the value of VScrollBarVisible property has changed.")]
        public event EventHandler VScrollBarVisibleChanged;

        public HexBox()
        {
            this._vScrollBar.Scroll += new ScrollEventHandler(this._vScrollBar_Scroll);
            this.BackColor = Color.White;
            this.Font = new System.Drawing.Font("Courier New", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this._stringFormat = new StringFormat(StringFormat.GenericTypographic);
            this._stringFormat.FormatFlags = StringFormatFlags.MeasureTrailingSpaces;
            this.ActivateEmptyKeyInterpreter();
            base.SetStyle(ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.DoubleBuffer, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            this._thumbTrackTimer.Interval = 50;
            this._thumbTrackTimer.Tick += new EventHandler(this.PerformScrollThumbTrack);
        }

        private void _byteProvider_LengthChanged(object sender, EventArgs e)
        {
            this.UpdateScrollSize();
        }

        private void _vScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            switch (e.Type)
            {
                case ScrollEventType.SmallDecrement:
                    this.PerformScrollLineUp();
                    break;

                case ScrollEventType.SmallIncrement:
                    this.PerformScrollLineDown();
                    break;

                case ScrollEventType.LargeDecrement:
                    this.PerformScrollPageUp();
                    break;

                case ScrollEventType.LargeIncrement:
                    this.PerformScrollPageDown();
                    break;

                case ScrollEventType.ThumbPosition:
                {
                    long pos = this.FromScrollPos(e.NewValue);
                    this.PerformScrollThumpPosition(pos);
                    break;
                }
                case ScrollEventType.ThumbTrack:
                {
                    if (this._thumbTrackTimer.Enabled)
                    {
                        this._thumbTrackTimer.Enabled = false;
                    }
                    int tickCount = Environment.TickCount;
                    if ((tickCount - this._lastThumbtrack) > 50)
                    {
                        this.PerformScrollThumbTrack(null, null);
                        this._lastThumbtrack = tickCount;
                    }
                    else
                    {
                        this._thumbTrackPosition = this.FromScrollPos(e.NewValue);
                        this._thumbTrackTimer.Enabled = true;
                    }
                    break;
                }
            }
            e.NewValue = this.ToScrollPos(this._scrollVpos);
        }

        public void AbortFind()
        {
            this._abortFind = true;
        }

        private void ActivateEmptyKeyInterpreter()
        {
            if (this._eki == null)
            {
                this._eki = new EmptyKeyInterpreter(this);
            }
            if (this._eki != this._keyInterpreter)
            {
                if (this._keyInterpreter != null)
                {
                    this._keyInterpreter.Deactivate();
                }
                this._keyInterpreter = this._eki;
                this._keyInterpreter.Activate();
            }
        }

        private void ActivateKeyInterpreter()
        {
            if (this._ki == null)
            {
                this._ki = new KeyInterpreter(this);
            }
            if (this._ki != this._keyInterpreter)
            {
                if (this._keyInterpreter != null)
                {
                    this._keyInterpreter.Deactivate();
                }
                this._keyInterpreter = this._ki;
                this._keyInterpreter.Activate();
            }
        }

        private void ActivateStringKeyInterpreter()
        {
            if (this._ski == null)
            {
                this._ski = new StringKeyInterpreter(this);
            }
            if (this._ski != this._keyInterpreter)
            {
                if (this._keyInterpreter != null)
                {
                    this._keyInterpreter.Deactivate();
                }
                this._keyInterpreter = this._ski;
                this._keyInterpreter.Activate();
            }
        }

        private bool BasePreProcessMessage(ref Message m)
        {
            return base.PreProcessMessage(ref m);
        }

        public bool CanCopy()
        {
            return ((this._selectionLength >= 1L) && (this._byteProvider != null));
        }

        public bool CanCut()
        {
            if (this.ReadOnly || !base.Enabled)
            {
                return false;
            }
            if (this._byteProvider == null)
            {
                return false;
            }
            return ((this._selectionLength >= 1L) && this._byteProvider.SupportsDeleteBytes());
        }

        public bool CanPaste()
        {
            if (this.ReadOnly || !base.Enabled)
            {
                return false;
            }
            if ((this._byteProvider == null) || !this._byteProvider.SupportsInsertBytes())
            {
                return false;
            }
            if (!this._byteProvider.SupportsDeleteBytes() && (this._selectionLength > 0L))
            {
                return false;
            }
            IDataObject dataObject = Clipboard.GetDataObject();
            return (dataObject.GetDataPresent("BinaryData") || dataObject.GetDataPresent(typeof(string)));
        }

        public bool CanPasteHex()
        {
            if (this.CanPaste())
            {
                IDataObject dataObject = Clipboard.GetDataObject();
                if (dataObject.GetDataPresent(typeof(string)))
                {
                    string data = (string) dataObject.GetData(typeof(string));
                    return (this.ConvertHexToBytes(data) != null);
                }
            }
            return false;
        }

        private void CheckCurrentLineChanged()
        {
            long num = ((long) Math.Floor((double) (((double) this._bytePos) / ((double) this._iHexMaxHBytes)))) + 1L;
            if ((this._byteProvider == null) && (this._currentLine != 0L))
            {
                this._currentLine = 0L;
                this.OnCurrentLineChanged(EventArgs.Empty);
            }
            else if (num != this._currentLine)
            {
                this._currentLine = num;
                this.OnCurrentLineChanged(EventArgs.Empty);
            }
        }

        private void CheckCurrentPositionInLineChanged()
        {
            int num = this.GetGridBytePoint(this._bytePos).X + 1;
            if ((this._byteProvider == null) && (this._currentPositionInLine != 0))
            {
                this._currentPositionInLine = 0;
                this.OnCurrentPositionInLineChanged(EventArgs.Empty);
            }
            else if (num != this._currentPositionInLine)
            {
                this._currentPositionInLine = num;
                this.OnCurrentPositionInLineChanged(EventArgs.Empty);
            }
        }

        private string ConvertBytesToHex(byte[] data)
        {
            StringBuilder builder = new StringBuilder();
            foreach (byte num in data)
            {
                string str = this.ConvertByteToHex(num);
                builder.Append(str);
                builder.Append(" ");
            }
            if (builder.Length > 0)
            {
                builder.Remove(builder.Length - 1, 1);
            }
            return builder.ToString();
        }

        private string ConvertByteToHex(byte b)
        {
            string str = b.ToString(this._hexStringFormat, Thread.CurrentThread.CurrentCulture);
            if (str.Length == 1)
            {
                str = "0" + str;
            }
            return str;
        }

        private bool ConvertHexToByte(string hex, out byte b)
        {
            return byte.TryParse(hex, NumberStyles.HexNumber, Thread.CurrentThread.CurrentCulture, out b);
        }

        private byte[] ConvertHexToBytes(string hex)
        {
            if (string.IsNullOrEmpty(hex))
            {
                return null;
            }
            hex = hex.Trim();
            string[] strArray = hex.Split(new char[] { ' ' });
            byte[] buffer = new byte[strArray.Length];
            for (int i = 0; i < strArray.Length; i++)
            {
                byte num2;
                string str = strArray[i];
                if (!this.ConvertHexToByte(str, out num2))
                {
                    return null;
                }
                buffer[i] = num2;
            }
            return buffer;
        }

        public void Copy()
        {
            if (this.CanCopy())
            {
                byte[] copyData = this.GetCopyData();
                DataObject data = new DataObject();
                string str = Encoding.ASCII.GetString(copyData, 0, copyData.Length);
                data.SetData(typeof(string), str);
                MemoryStream stream = new MemoryStream(copyData, 0, copyData.Length, false, true);
                data.SetData("BinaryData", stream);
                Clipboard.SetDataObject(data, true);
                this.UpdateCaret();
                this.ScrollByteIntoView();
                base.Invalidate();
                this.OnCopied(EventArgs.Empty);
            }
        }

        public void CopyHex()
        {
            if (this.CanCopy())
            {
                byte[] copyData = this.GetCopyData();
                DataObject data = new DataObject();
                string str = this.ConvertBytesToHex(copyData);
                data.SetData(typeof(string), str);
                MemoryStream stream = new MemoryStream(copyData, 0, copyData.Length, false, true);
                data.SetData("BinaryData", stream);
                Clipboard.SetDataObject(data, true);
                this.UpdateCaret();
                this.ScrollByteIntoView();
                base.Invalidate();
                this.OnCopiedHex(EventArgs.Empty);
            }
        }

        private void CreateCaret()
        {
            if (((this._byteProvider != null) && (this._keyInterpreter != null)) && (!this._caretVisible && this.Focused))
            {
                int nWidth = this.InsertActive ? 1 : ((int) this._charSize.Width);
                int height = (int) this._charSize.Height;
                SemtechLib.Controls.HexBoxCtrl.NativeMethods.CreateCaret(base.Handle, IntPtr.Zero, nWidth, height);
                this.UpdateCaret();
                SemtechLib.Controls.HexBoxCtrl.NativeMethods.ShowCaret(base.Handle);
                this._caretVisible = true;
            }
        }

        public void Cut()
        {
            if (this.CanCut())
            {
                this.Copy();
                this._byteProvider.DeleteBytes(this._bytePos, this._selectionLength);
                this._byteCharacterPos = 0;
                this.UpdateCaret();
                this.ScrollByteIntoView();
                this.ReleaseSelection();
                base.Invalidate();
                this.Refresh();
            }
        }

        private void DestroyCaret()
        {
            if (this._caretVisible)
            {
                SemtechLib.Controls.HexBoxCtrl.NativeMethods.DestroyCaret();
                this._caretVisible = false;
            }
        }

        public long Find(byte[] bytes, long startIndex)
        {
            int index = 0;
            int length = bytes.Length;
            this._abortFind = false;
            for (long i = startIndex; i < this._byteProvider.Length; i += 1L)
            {
                if (this._abortFind)
                {
                    return -2L;
                }
                if ((i % 0x3e8L) == 0L)
                {
                    Application.DoEvents();
                }
                if (this._byteProvider.ReadByte(i) != bytes[index])
                {
                    i -= index;
                    index = 0;
                    this._findingPos = i;
                }
                else
                {
                    index++;
                    if (index == length)
                    {
                        long start = (i - length) + 1L;
                        this.Select(start, (long) length);
                        this.ScrollByteIntoView(this._bytePos + this._selectionLength);
                        this.ScrollByteIntoView(this._bytePos);
                        return start;
                    }
                }
            }
            return -1L;
        }

        private long FromScrollPos(int value)
        {
            int num = 0xffff;
            if (this._scrollVmax < num)
            {
                return (long) value;
            }
            double num2 = (((double) value) / ((double) num)) * 100.0;
            return (long) ((int) Math.Floor((double) ((((double) this._scrollVmax) / 100.0) * num2)));
        }

        private PointF GetBytePointF(Point gp)
        {
            float x = ((3f * this._charSize.Width) * gp.X) + this._recHex.X;
            return new PointF(x, (((gp.Y + 1) * this._charSize.Height) - this._charSize.Height) + this._recHex.Y);
        }

        private PointF GetBytePointF(long byteIndex)
        {
            Point gridBytePoint = this.GetGridBytePoint(byteIndex);
            return this.GetBytePointF(gridBytePoint);
        }

        private PointF GetByteStringPointF(Point gp)
        {
            float x = (this._charSize.Width * gp.X) + this._recStringView.X;
            return new PointF(x, (((gp.Y + 1) * this._charSize.Height) - this._charSize.Height) + this._recStringView.Y);
        }

        private byte[] GetCopyData()
        {
            if (!this.CanCopy())
            {
                return new byte[0];
            }
            byte[] buffer = new byte[this._selectionLength];
            int index = -1;
            for (long i = this._bytePos; i < (this._bytePos + this._selectionLength); i += 1L)
            {
                index++;
                buffer[index] = this._byteProvider.ReadByte(i);
            }
            return buffer;
        }

        private Color GetDefaultForeColor()
        {
            if (base.Enabled)
            {
                return this.ForeColor;
            }
            return Color.Gray;
        }

        private Point GetGridBytePoint(long byteIndex)
        {
            int y = (int) Math.Floor((double) (((double) byteIndex) / ((double) this._iHexMaxHBytes)));
            return new Point((((int) byteIndex) + this._iHexMaxHBytes) - (this._iHexMaxHBytes * (y + 1)), y);
        }

        private BytePositionInfo GetHexBytePositionInfo(Point p)
        {
            float num3 = ((float) (p.X - this._recHex.X)) / this._charSize.Width;
            float num4 = ((float) (p.Y - this._recHex.Y)) / this._charSize.Height;
            int num5 = (int) num3;
            int num6 = (int) num4;
            int num7 = (num5 / 3) + 1;
            long index = Math.Min(this._byteProvider.Length, ((this._startByte + ((this._iHexMaxHBytes * (num6 + 1)) - this._iHexMaxHBytes)) + num7) - 1L);
            int characterPosition = num5 % 3;
            if (characterPosition > 1)
            {
                characterPosition = 1;
            }
            if (index == this._byteProvider.Length)
            {
                characterPosition = 0;
            }
            if (index < 0L)
            {
                return new BytePositionInfo(0L, 0);
            }
            return new BytePositionInfo(index, characterPosition);
        }

        private BytePositionInfo GetStringBytePositionInfo(Point p)
        {
            float num3 = ((float) (p.X - this._recStringView.X)) / this._charSize.Width;
            float num4 = ((float) (p.Y - this._recStringView.Y)) / this._charSize.Height;
            int num5 = (int) num3;
            int num6 = (int) num4;
            int num7 = num5 + 1;
            long index = Math.Min(this._byteProvider.Length, ((this._startByte + ((this._iHexMaxHBytes * (num6 + 1)) - this._iHexMaxHBytes)) + num7) - 1L);
            int characterPosition = 0;
            if (index < 0L)
            {
                return new BytePositionInfo(0L, 0);
            }
            return new BytePositionInfo(index, characterPosition);
        }

        private void InternalSelect(long start, long length)
        {
            long bytePos = start;
            long selectionLength = length;
            int byteCharacterPos = 0;
            if ((selectionLength > 0L) && this._caretVisible)
            {
                this.DestroyCaret();
            }
            else if ((selectionLength == 0L) && !this._caretVisible)
            {
                this.CreateCaret();
            }
            this.SetPosition(bytePos, byteCharacterPos);
            this.SetSelectionLength(selectionLength);
            this.UpdateCaret();
            base.Invalidate();
        }

        protected virtual void OnBorderStyleChanged(EventArgs e)
        {
            if (this.BorderStyleChanged != null)
            {
                this.BorderStyleChanged(this, e);
            }
        }

        protected virtual void OnByteProviderChanged(EventArgs e)
        {
            if (this.ByteProviderChanged != null)
            {
                this.ByteProviderChanged(this, e);
            }
        }

        protected virtual void OnBytesPerLineChanged(EventArgs e)
        {
            if (this.BytesPerLineChanged != null)
            {
                this.BytesPerLineChanged(this, e);
            }
        }

        protected virtual void OnCopied(EventArgs e)
        {
            if (this.Copied != null)
            {
                this.Copied(this, e);
            }
        }

        protected virtual void OnCopiedHex(EventArgs e)
        {
            if (this.CopiedHex != null)
            {
                this.CopiedHex(this, e);
            }
        }

        protected virtual void OnCurrentLineChanged(EventArgs e)
        {
            if (this.CurrentLineChanged != null)
            {
                this.CurrentLineChanged(this, e);
            }
        }

        protected virtual void OnCurrentPositionInLineChanged(EventArgs e)
        {
            if (this.CurrentPositionInLineChanged != null)
            {
                this.CurrentPositionInLineChanged(this, e);
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            this.CreateCaret();
        }

        protected virtual void OnHexCasingChanged(EventArgs e)
        {
            if (this.HexCasingChanged != null)
            {
                this.HexCasingChanged(this, e);
            }
        }

        protected virtual void OnHorizontalByteCountChanged(EventArgs e)
        {
            if (this.HorizontalByteCountChanged != null)
            {
                this.HorizontalByteCountChanged(this, e);
            }
        }

        protected virtual void OnInsertActiveChanged(EventArgs e)
        {
            if (this.InsertActiveChanged != null)
            {
                this.InsertActiveChanged(this, e);
            }
        }

        protected virtual void OnLineInfoVisibleChanged(EventArgs e)
        {
            if (this.LineInfoVisibleChanged != null)
            {
                this.LineInfoVisibleChanged(this, e);
            }
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            this.DestroyCaret();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (!this.Focused)
            {
                base.Focus();
            }
            this.SetCaretPosition(new Point(e.X, e.Y));
            base.OnMouseDown(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            int lines = -((e.Delta * SystemInformation.MouseWheelScrollLines) / 120);
            this.PerformScrollLines(lines);
            base.OnMouseWheel(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (this._byteProvider != null)
            {
                Region region = new Region(base.ClientRectangle);
                region.Exclude(this._recContent);
                e.Graphics.ExcludeClip(region);
                this.UpdateVisibilityBytes();
                if (this._lineInfoVisible)
                {
                    this.PaintLineInfo(e.Graphics, this._startByte, this._endByte);
                }
                if (!this._stringViewVisible)
                {
                    this.PaintHex(e.Graphics, this._startByte, this._endByte);
                }
                else
                {
                    this.PaintHexAndStringView(e.Graphics, this._startByte, this._endByte);
                    if (this._shadowSelectionVisible)
                    {
                        this.PaintCurrentBytesSign(e.Graphics);
                    }
                }
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            VisualStyleElement normal;
            Color backColor;
            switch (this._borderStyle)
            {
                case System.Windows.Forms.BorderStyle.FixedSingle:
                    e.Graphics.FillRectangle(new SolidBrush(this.BackColor), base.ClientRectangle);
                    ControlPaint.DrawBorder(e.Graphics, base.ClientRectangle, Color.Black, ButtonBorderStyle.Solid);
                    return;

                case System.Windows.Forms.BorderStyle.Fixed3D:
                    if (!TextBoxRenderer.IsSupported)
                    {
                        e.Graphics.FillRectangle(new SolidBrush(this.BackColor), base.ClientRectangle);
                        ControlPaint.DrawBorder3D(e.Graphics, base.ClientRectangle, Border3DStyle.Sunken);
                        return;
                    }
                    normal = VisualStyleElement.TextBox.TextEdit.Normal;
                    backColor = this.BackColor;
                    if (!base.Enabled)
                    {
                        normal = VisualStyleElement.TextBox.TextEdit.Disabled;
                        backColor = this.BackColorDisabled;
                        break;
                    }
                    if (!this.ReadOnly)
                    {
                        if (this.Focused)
                        {
                            normal = VisualStyleElement.TextBox.TextEdit.Focused;
                        }
                        break;
                    }
                    normal = VisualStyleElement.TextBox.TextEdit.ReadOnly;
                    break;

                default:
                    return;
            }
            VisualStyleRenderer renderer = new VisualStyleRenderer(normal);
            renderer.DrawBackground(e.Graphics, base.ClientRectangle);
            Rectangle backgroundContentRectangle = renderer.GetBackgroundContentRectangle(e.Graphics, base.ClientRectangle);
            e.Graphics.FillRectangle(new SolidBrush(backColor), backgroundContentRectangle);
        }

        protected virtual void OnReadOnlyChanged(EventArgs e)
        {
            if (this.ReadOnlyChanged != null)
            {
                this.ReadOnlyChanged(this, e);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.UpdateRectanglePositioning();
        }

        protected virtual void OnSelectionLengthChanged(EventArgs e)
        {
            if (this.SelectionLengthChanged != null)
            {
                this.SelectionLengthChanged(this, e);
            }
        }

        protected virtual void OnSelectionStartChanged(EventArgs e)
        {
            if (this.SelectionStartChanged != null)
            {
                this.SelectionStartChanged(this, e);
            }
        }

        protected virtual void OnStringViewVisibleChanged(EventArgs e)
        {
            if (this.StringViewVisibleChanged != null)
            {
                this.StringViewVisibleChanged(this, e);
            }
        }

        protected virtual void OnUseFixedBytesPerLineChanged(EventArgs e)
        {
            if (this.UseFixedBytesPerLineChanged != null)
            {
                this.UseFixedBytesPerLineChanged(this, e);
            }
        }

        protected virtual void OnVerticalByteCountChanged(EventArgs e)
        {
            if (this.VerticalByteCountChanged != null)
            {
                this.VerticalByteCountChanged(this, e);
            }
        }

        protected virtual void OnVScrollBarVisibleChanged(EventArgs e)
        {
            if (this.VScrollBarVisibleChanged != null)
            {
                this.VScrollBarVisibleChanged(this, e);
            }
        }

        private void PaintCurrentByteSign(Graphics g, Rectangle rec)
        {
            if (((rec.Top >= 0) && (rec.Left >= 0)) && ((rec.Width > 0) && (rec.Height > 0)))
            {
                Bitmap image = new Bitmap(rec.Width, rec.Height);
                Graphics graphics = Graphics.FromImage(image);
                SolidBrush brush = new SolidBrush(this._shadowSelectionColor);
                graphics.FillRectangle(brush, 0, 0, rec.Width, rec.Height);
                g.CompositingQuality = CompositingQuality.GammaCorrected;
                g.DrawImage(image, rec.Left, rec.Top);
            }
        }

        private void PaintCurrentBytesSign(Graphics g)
        {
            if (((this._keyInterpreter != null) && this.Focused) && ((this._bytePos != -1L) && base.Enabled))
            {
                if (this._keyInterpreter.GetType() == typeof(KeyInterpreter))
                {
                    if (this._selectionLength != 0L)
                    {
                        int num = this._recStringView.Width - ((int) this._charSize.Width);
                        Point gridBytePoint = this.GetGridBytePoint(this._bytePos - this._startByte);
                        PointF byteStringPointF = this.GetByteStringPointF(gridBytePoint);
                        Point gp = this.GetGridBytePoint(((this._bytePos - this._startByte) + this._selectionLength) - 1L);
                        PointF tf3 = this.GetByteStringPointF(gp);
                        int num2 = gp.Y - gridBytePoint.Y;
                        if (num2 != 0)
                        {
                            Rectangle rec = new Rectangle((int) byteStringPointF.X, (int) byteStringPointF.Y, (int) (((this._recStringView.X + num) - byteStringPointF.X) + this._charSize.Width), (int) this._charSize.Height);
                            if (rec.IntersectsWith(this._recStringView))
                            {
                                rec.Intersect(this._recStringView);
                                this.PaintCurrentByteSign(g, rec);
                            }
                            if (num2 > 1)
                            {
                                Rectangle rectangle4 = new Rectangle(this._recStringView.X, (int) (byteStringPointF.Y + this._charSize.Height), this._recStringView.Width, (int) (this._charSize.Height * (num2 - 1)));
                                if (rectangle4.IntersectsWith(this._recStringView))
                                {
                                    rectangle4.Intersect(this._recStringView);
                                    this.PaintCurrentByteSign(g, rectangle4);
                                }
                            }
                            Rectangle rectangle5 = new Rectangle(this._recStringView.X, (int) tf3.Y, (int) ((tf3.X - this._recStringView.X) + this._charSize.Width), (int) this._charSize.Height);
                            if (rectangle5.IntersectsWith(this._recStringView))
                            {
                                rectangle5.Intersect(this._recStringView);
                                this.PaintCurrentByteSign(g, rectangle5);
                            }
                        }
                        else
                        {
                            Rectangle rectangle2 = new Rectangle((int) byteStringPointF.X, (int) byteStringPointF.Y, (int) ((tf3.X - byteStringPointF.X) + this._charSize.Width), (int) this._charSize.Height);
                            if (rectangle2.IntersectsWith(this._recStringView))
                            {
                                rectangle2.Intersect(this._recStringView);
                                this.PaintCurrentByteSign(g, rectangle2);
                            }
                        }
                    }
                    else
                    {
                        Point point = this.GetGridBytePoint(this._bytePos - this._startByte);
                        PointF tf = this.GetByteStringPointF(point);
                        Size size = new Size((int) this._charSize.Width, (int) this._charSize.Height);
                        Rectangle rectangle = new Rectangle((int) tf.X, (int) tf.Y, size.Width, size.Height);
                        if (rectangle.IntersectsWith(this._recStringView))
                        {
                            rectangle.Intersect(this._recStringView);
                            this.PaintCurrentByteSign(g, rectangle);
                        }
                    }
                }
                else if (this._selectionLength == 0L)
                {
                    Point point4 = this.GetGridBytePoint(this._bytePos - this._startByte);
                    PointF bytePointF = this.GetBytePointF(point4);
                    Size size2 = new Size(((int) this._charSize.Width) * 2, (int) this._charSize.Height);
                    Rectangle rectangle6 = new Rectangle((int) bytePointF.X, (int) bytePointF.Y, size2.Width, size2.Height);
                    this.PaintCurrentByteSign(g, rectangle6);
                }
                else
                {
                    int num3 = this._recHex.Width - ((int) (this._charSize.Width * 5f));
                    Point point5 = this.GetGridBytePoint(this._bytePos - this._startByte);
                    PointF tf5 = this.GetBytePointF(point5);
                    Point point6 = this.GetGridBytePoint(((this._bytePos - this._startByte) + this._selectionLength) - 1L);
                    PointF tf6 = this.GetBytePointF(point6);
                    int num4 = point6.Y - point5.Y;
                    if (num4 == 0)
                    {
                        Rectangle rectangle7 = new Rectangle((int) tf5.X, (int) tf5.Y, (int) ((tf6.X - tf5.X) + (this._charSize.Width * 2f)), (int) this._charSize.Height);
                        if (rectangle7.IntersectsWith(this._recHex))
                        {
                            rectangle7.Intersect(this._recHex);
                            this.PaintCurrentByteSign(g, rectangle7);
                        }
                    }
                    else
                    {
                        Rectangle rectangle8 = new Rectangle((int) tf5.X, (int) tf5.Y, (int) (((this._recHex.X + num3) - tf5.X) + (this._charSize.Width * 2f)), (int) this._charSize.Height);
                        if (rectangle8.IntersectsWith(this._recHex))
                        {
                            rectangle8.Intersect(this._recHex);
                            this.PaintCurrentByteSign(g, rectangle8);
                        }
                        if (num4 > 1)
                        {
                            Rectangle rectangle9 = new Rectangle(this._recHex.X, (int) (tf5.Y + this._charSize.Height), num3 + ((int) (this._charSize.Width * 2f)), (int) (this._charSize.Height * (num4 - 1)));
                            if (rectangle9.IntersectsWith(this._recHex))
                            {
                                rectangle9.Intersect(this._recHex);
                                this.PaintCurrentByteSign(g, rectangle9);
                            }
                        }
                        Rectangle rectangle10 = new Rectangle(this._recHex.X, (int) tf6.Y, (int) ((tf6.X - this._recHex.X) + (this._charSize.Width * 2f)), (int) this._charSize.Height);
                        if (rectangle10.IntersectsWith(this._recHex))
                        {
                            rectangle10.Intersect(this._recHex);
                            this.PaintCurrentByteSign(g, rectangle10);
                        }
                    }
                }
            }
        }

        private void PaintHex(Graphics g, long startByte, long endByte)
        {
            Brush brush = new SolidBrush(this.GetDefaultForeColor());
            Brush brush2 = new SolidBrush(this._selectionForeColor);
            Brush brushBack = new SolidBrush(this._selectionBackColor);
            int num = -1;
            long num2 = Math.Min((long) (this._byteProvider.Length - 1L), (long) (endByte + this._iHexMaxHBytes));
            bool flag = (this._keyInterpreter == null) || (this._keyInterpreter.GetType() == typeof(KeyInterpreter));
            for (long i = startByte; i < (num2 + 1L); i += 1L)
            {
                num++;
                Point gridBytePoint = this.GetGridBytePoint((long) num);
                byte b = this._byteProvider.ReadByte(i);
                if ((((i >= this._bytePos) && (i <= ((this._bytePos + this._selectionLength) - 1L))) && (this._selectionLength != 0L)) && flag)
                {
                    this.PaintHexStringSelected(g, b, brush2, brushBack, gridBytePoint);
                }
                else
                {
                    this.PaintHexString(g, b, brush, gridBytePoint);
                }
            }
        }

        private void PaintHexAndStringView(Graphics g, long startByte, long endByte)
        {
            Brush brush = new SolidBrush(this.GetDefaultForeColor());
            Brush brush2 = new SolidBrush(this._selectionForeColor);
            Brush brushBack = new SolidBrush(this._selectionBackColor);
            int num = -1;
            long num2 = Math.Min((long) (this._byteProvider.Length - 1L), (long) (endByte + this._iHexMaxHBytes));
            bool flag = (this._keyInterpreter == null) || (this._keyInterpreter.GetType() == typeof(KeyInterpreter));
            bool flag2 = (this._keyInterpreter != null) && (this._keyInterpreter.GetType() == typeof(StringKeyInterpreter));
            for (long i = startByte; i < (num2 + 1L); i += 1L)
            {
                string str;
                num++;
                Point gridBytePoint = this.GetGridBytePoint((long) num);
                PointF byteStringPointF = this.GetByteStringPointF(gridBytePoint);
                byte b = this._byteProvider.ReadByte(i);
                bool flag3 = ((i >= this._bytePos) && (i <= ((this._bytePos + this._selectionLength) - 1L))) && (this._selectionLength != 0L);
                if (flag3 && flag)
                {
                    this.PaintHexStringSelected(g, b, brush2, brushBack, gridBytePoint);
                }
                else
                {
                    this.PaintHexString(g, b, brush, gridBytePoint);
                }
                if ((b > 0x1f) && ((b <= 0x7e) || (b >= 160)))
                {
                    str = ((char) b).ToString();
                }
                else
                {
                    str = ".";
                }
                if (flag3 && flag2)
                {
                    g.FillRectangle(brushBack, byteStringPointF.X, byteStringPointF.Y, this._charSize.Width, this._charSize.Height);
                    g.DrawString(str, this.Font, brush2, byteStringPointF, this._stringFormat);
                }
                else
                {
                    g.DrawString(str, this.Font, brush, byteStringPointF, this._stringFormat);
                }
            }
        }

        private void PaintHexString(Graphics g, byte b, Brush brush, Point gridPoint)
        {
            PointF bytePointF = this.GetBytePointF(gridPoint);
            string str = this.ConvertByteToHex(b);
            g.DrawString(str.Substring(0, 1), this.Font, brush, bytePointF, this._stringFormat);
            bytePointF.X += this._charSize.Width;
            g.DrawString(str.Substring(1, 1), this.Font, brush, bytePointF, this._stringFormat);
        }

        private void PaintHexStringSelected(Graphics g, byte b, Brush brush, Brush brushBack, Point gridPoint)
        {
            string str = b.ToString(this._hexStringFormat, Thread.CurrentThread.CurrentCulture);
            if (str.Length == 1)
            {
                str = "0" + str;
            }
            PointF bytePointF = this.GetBytePointF(gridPoint);
            float width = ((gridPoint.X + 1) == this._iHexMaxHBytes) ? (this._charSize.Width * 2f) : (this._charSize.Width * 3f);
            g.FillRectangle(brushBack, bytePointF.X, bytePointF.Y, width, this._charSize.Height);
            g.DrawString(str.Substring(0, 1), this.Font, brush, bytePointF, this._stringFormat);
            bytePointF.X += this._charSize.Width;
            g.DrawString(str.Substring(1, 1), this.Font, brush, bytePointF, this._stringFormat);
        }

        private void PaintLineInfo(Graphics g, long startByte, long endByte)
        {
            endByte = Math.Min(this._byteProvider.Length - 1L, endByte);
            Color color = (this.LineInfoForeColor != Color.Empty) ? this.LineInfoForeColor : this.ForeColor;
            Brush brush = new SolidBrush(color);
            int num = this.GetGridBytePoint(endByte - startByte).Y + 1;
            for (int i = 0; i < num; i++)
            {
                string str2;
                long num3 = startByte + (this._iHexMaxHBytes * i);
                PointF bytePointF = this.GetBytePointF(new Point(0, i));
                string str = num3.ToString(this._hexStringFormat, Thread.CurrentThread.CurrentCulture);
                int num4 = 8 - str.Length;
                if (num4 > -1)
                {
                    str2 = new string('0', this.lineInfoDigits - str.Length) + str;
                }
                else
                {
                    str2 = new string('~', this.lineInfoDigits);
                }
                g.DrawString(str2, this.Font, brush, new PointF((float) this._recLineInfo.X, bytePointF.Y), this._stringFormat);
            }
        }

        public void Paste()
        {
            byte[] bytes;
            if (this.CanPaste())
            {
                if (this._selectionLength > 0L)
                {
                    this._byteProvider.DeleteBytes(this._bytePos, this._selectionLength);
                }
                bytes = null;
                IDataObject dataObject = Clipboard.GetDataObject();
                if (dataObject.GetDataPresent("BinaryData"))
                {
                    MemoryStream data = (MemoryStream) dataObject.GetData("BinaryData");
                    bytes = new byte[data.Length];
                    data.Read(bytes, 0, bytes.Length);
                    goto Label_00A2;
                }
                if (dataObject.GetDataPresent(typeof(string)))
                {
                    string s = (string) dataObject.GetData(typeof(string));
                    bytes = Encoding.ASCII.GetBytes(s);
                    goto Label_00A2;
                }
            }
            return;
        Label_00A2:
            this._byteProvider.InsertBytes(this._bytePos, bytes);
            this.SetPosition(this._bytePos + bytes.Length, 0);
            this.ReleaseSelection();
            this.ScrollByteIntoView();
            this.UpdateCaret();
            base.Invalidate();
        }

        public void PasteHex()
        {
            if (this.CanPaste())
            {
                byte[] bs = null;
                IDataObject dataObject = Clipboard.GetDataObject();
                if (dataObject.GetDataPresent(typeof(string)))
                {
                    string data = (string) dataObject.GetData(typeof(string));
                    bs = this.ConvertHexToBytes(data);
                    if (bs != null)
                    {
                        if (this._selectionLength > 0L)
                        {
                            this._byteProvider.DeleteBytes(this._bytePos, this._selectionLength);
                        }
                        this._byteProvider.InsertBytes(this._bytePos, bs);
                        this.SetPosition(this._bytePos + bs.Length, 0);
                        this.ReleaseSelection();
                        this.ScrollByteIntoView();
                        this.UpdateCaret();
                        base.Invalidate();
                    }
                }
            }
        }

        private void PerformScrollLineDown()
        {
            this.PerformScrollLines(1);
        }

        private void PerformScrollLines(int lines)
        {
            long num;
            if (lines > 0)
            {
                num = Math.Min(this._scrollVmax, this._scrollVpos + lines);
            }
            else if (lines < 0)
            {
                num = Math.Max(this._scrollVmin, this._scrollVpos + lines);
            }
            else
            {
                return;
            }
            this.PerformScrollToLine(num);
        }

        private void PerformScrollLineUp()
        {
            this.PerformScrollLines(-1);
        }

        private void PerformScrollPageDown()
        {
            this.PerformScrollLines(this._iHexMaxVBytes);
        }

        private void PerformScrollPageUp()
        {
            this.PerformScrollLines(-this._iHexMaxVBytes);
        }

        private void PerformScrollThumbTrack(object sender, EventArgs e)
        {
            this._thumbTrackTimer.Enabled = false;
            this.PerformScrollThumpPosition(this._thumbTrackPosition);
            this._lastThumbtrack = Environment.TickCount;
        }

        private void PerformScrollThumpPosition(long pos)
        {
            int num = (this._scrollVmax > 0xffffL) ? 10 : 9;
            if (this.ToScrollPos(pos) == (this.ToScrollMax(this._scrollVmax) - num))
            {
                pos = this._scrollVmax;
            }
            this.PerformScrollToLine(pos);
        }

        private void PerformScrollToLine(long pos)
        {
            if (((pos >= this._scrollVmin) && (pos <= this._scrollVmax)) && (pos != this._scrollVpos))
            {
                this._scrollVpos = pos;
                this.UpdateVScroll();
                this.UpdateVisibilityBytes();
                this.UpdateCaret();
                base.Invalidate();
            }
        }

        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode=true), SecurityPermission(SecurityAction.InheritanceDemand, UnmanagedCode=true)]
        public override bool PreProcessMessage(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x100:
                    return this._keyInterpreter.PreProcessWmKeyDown(ref m);

                case 0x101:
                    return this._keyInterpreter.PreProcessWmKeyUp(ref m);

                case 0x102:
                    return this._keyInterpreter.PreProcessWmChar(ref m);
            }
            return base.PreProcessMessage(ref m);
        }

        private void ReleaseSelection()
        {
            if (this._selectionLength != 0L)
            {
                this._selectionLength = 0L;
                this.OnSelectionLengthChanged(EventArgs.Empty);
                if (!this._caretVisible)
                {
                    this.CreateCaret();
                }
                else
                {
                    this.UpdateCaret();
                }
                base.Invalidate();
            }
        }

        public void ScrollByteIntoView()
        {
            this.ScrollByteIntoView(this._bytePos);
        }

        public void ScrollByteIntoView(long index)
        {
            if ((this._byteProvider != null) && (this._keyInterpreter != null))
            {
                if (index < this._startByte)
                {
                    long pos = (long) Math.Floor((double) (((double) index) / ((double) this._iHexMaxHBytes)));
                    this.PerformScrollThumpPosition(pos);
                }
                else if (index > this._endByte)
                {
                    long num2 = (long) Math.Floor((double) (((double) index) / ((double) this._iHexMaxHBytes)));
                    num2 -= this._iHexMaxVBytes - 1;
                    this.PerformScrollThumpPosition(num2);
                }
            }
        }

        public void Select(long start, long length)
        {
            this.InternalSelect(start, length);
            this.ScrollByteIntoView();
        }

        private void SetCaretPosition(Point p)
        {
            if ((this._byteProvider != null) && (this._keyInterpreter != null))
            {
                long bytePos = this._bytePos;
                int byteCharacterPos = this._byteCharacterPos;
                if (this._recHex.Contains(p))
                {
                    BytePositionInfo hexBytePositionInfo = this.GetHexBytePositionInfo(p);
                    bytePos = hexBytePositionInfo.Index;
                    byteCharacterPos = hexBytePositionInfo.CharacterPosition;
                    this.SetPosition(bytePos, byteCharacterPos);
                    this.ActivateKeyInterpreter();
                    this.UpdateCaret();
                    base.Invalidate();
                }
                else if (this._recStringView.Contains(p))
                {
                    BytePositionInfo stringBytePositionInfo = this.GetStringBytePositionInfo(p);
                    bytePos = stringBytePositionInfo.Index;
                    byteCharacterPos = stringBytePositionInfo.CharacterPosition;
                    this.SetPosition(bytePos, byteCharacterPos);
                    this.ActivateStringKeyInterpreter();
                    this.UpdateCaret();
                    base.Invalidate();
                }
            }
        }

        private void SetHorizontalByteCount(int value)
        {
            if (this._iHexMaxHBytes != value)
            {
                this._iHexMaxHBytes = value;
                this.OnHorizontalByteCountChanged(EventArgs.Empty);
            }
        }

        private void SetPosition(long bytePos)
        {
            this.SetPosition(bytePos, this._byteCharacterPos);
        }

        public void SetPosition(long bytePos, int byteCharacterPos)
        {
            if (this._byteCharacterPos != byteCharacterPos)
            {
                this._byteCharacterPos = byteCharacterPos;
            }
            if (bytePos != this._bytePos)
            {
                this._bytePos = bytePos;
                this.CheckCurrentLineChanged();
                this.CheckCurrentPositionInLineChanged();
                this.OnSelectionStartChanged(EventArgs.Empty);
            }
        }

        private void SetSelectionLength(long selectionLength)
        {
            if (selectionLength != this._selectionLength)
            {
                this._selectionLength = selectionLength;
                this.OnSelectionLengthChanged(EventArgs.Empty);
            }
        }

        private void SetVerticalByteCount(int value)
        {
            if (this._iHexMaxVBytes != value)
            {
                this._iHexMaxVBytes = value;
                this.OnVerticalByteCountChanged(EventArgs.Empty);
            }
        }

        private int ToScrollMax(long value)
        {
            long num = 0xffffL;
            if (value > num)
            {
                return (int) num;
            }
            return (int) value;
        }

        private int ToScrollPos(long value)
        {
            int num = 0xffff;
            if (this._scrollVmax < num)
            {
                return (int) value;
            }
            double num2 = (((double) value) / ((double) this._scrollVmax)) * 100.0;
            int num3 = (int) Math.Floor((double) ((((double) num) / 100.0) * num2));
            num3 = (int) Math.Max(this._scrollVmin, (long) num3);
            return (int) Math.Min(this._scrollVmax, (long) num3);
        }

        private void UpdateCaret()
        {
            if ((this._byteProvider != null) && (this._keyInterpreter != null))
            {
                long byteIndex = this._bytePos - this._startByte;
                PointF caretPointF = this._keyInterpreter.GetCaretPointF(byteIndex);
                caretPointF.X += this._byteCharacterPos * this._charSize.Width;
                SemtechLib.Controls.HexBoxCtrl.NativeMethods.SetCaretPos((int) caretPointF.X, (int) caretPointF.Y);
            }
        }

        private void UpdateRectanglePositioning()
        {
            SizeF ef = base.CreateGraphics().MeasureString("A", this.Font, 100, this._stringFormat);
            this._charSize = new SizeF((float) Math.Ceiling((double) ef.Width), (float) Math.Ceiling((double) ef.Height));
            this._recContent = base.ClientRectangle;
            this._recContent.X += this._recBorderLeft;
            this._recContent.Y += this._recBorderTop;
            this._recContent.Width -= this._recBorderRight + this._recBorderLeft;
            this._recContent.Height -= this._recBorderBottom + this._recBorderTop;
            if (this._vScrollBarVisible)
            {
                this._recContent.Width -= this._vScrollBar.Width;
                this._vScrollBar.Left = this._recContent.X + this._recContent.Width;
                this._vScrollBar.Top = this._recContent.Y;
                this._vScrollBar.Height = this._recContent.Height;
            }
            int num = 4;
            if (this._lineInfoVisible)
            {
                this._recLineInfo = new Rectangle(this._recContent.X + num, this._recContent.Y, (int) (this._charSize.Width * (this.lineInfoDigits + 2)), this._recContent.Height);
            }
            else
            {
                this._recLineInfo = Rectangle.Empty;
                this._recLineInfo.X = num;
            }
            this._recHex = new Rectangle(this._recLineInfo.X + this._recLineInfo.Width, this._recLineInfo.Y, this._recContent.Width - this._recLineInfo.Width, this._recContent.Height);
            if (this.UseFixedBytesPerLine)
            {
                this.SetHorizontalByteCount(this._bytesPerLine);
                this._recHex.Width = (int) Math.Floor((double) (((this._iHexMaxHBytes * this._charSize.Width) * 3.0) + (2f * this._charSize.Width)));
            }
            else
            {
                int num2 = (int) Math.Floor((double) (((double) this._recHex.Width) / ((double) this._charSize.Width)));
                if (num2 > 1)
                {
                    this.SetHorizontalByteCount((int) Math.Floor((double) (((double) num2) / 3.0)));
                }
                else
                {
                    this.SetHorizontalByteCount(num2);
                }
            }
            if (this._stringViewVisible)
            {
                this._recStringView = new Rectangle(this._recHex.X + this._recHex.Width, this._recHex.Y, (int) (this._charSize.Width * this._iHexMaxHBytes), this._recHex.Height);
            }
            else
            {
                this._recStringView = Rectangle.Empty;
            }
            int num3 = (int) Math.Floor((double) (((double) this._recHex.Height) / ((double) this._charSize.Height)));
            this.SetVerticalByteCount(num3);
            this._iHexMaxBytes = this._iHexMaxHBytes * this._iHexMaxVBytes;
            this.UpdateScrollSize();
        }

        private void UpdateScrollSize()
        {
            if ((this.VScrollBarVisible && (this._byteProvider != null)) && ((this._byteProvider.Length > 0L) && (this._iHexMaxHBytes != 0)))
            {
                long num = (long) Math.Ceiling((double) ((((double) (this._byteProvider.Length + 1L)) / ((double) this._iHexMaxHBytes)) - this._iHexMaxVBytes));
                num = Math.Max(0L, num);
                long num2 = this._startByte / ((long) this._iHexMaxHBytes);
                if ((num < this._scrollVmax) && (this._scrollVpos == this._scrollVmax))
                {
                    this.PerformScrollLineUp();
                }
                if ((num != this._scrollVmax) || (num2 != this._scrollVpos))
                {
                    this._scrollVmin = 0L;
                    this._scrollVmax = num;
                    this._scrollVpos = Math.Min(num2, num);
                    this.UpdateVScroll();
                }
            }
            else if (this.VScrollBarVisible)
            {
                this._scrollVmin = 0L;
                this._scrollVmax = 0L;
                this._scrollVpos = 0L;
                this.UpdateVScroll();
            }
        }

        private void UpdateVisibilityBytes()
        {
            if ((this._byteProvider != null) && (this._byteProvider.Length != 0L))
            {
                this._startByte = ((this._scrollVpos + 1L) * this._iHexMaxHBytes) - this._iHexMaxHBytes;
                this._endByte = Math.Min((long) (this._byteProvider.Length - 1L), (long) (this._startByte + this._iHexMaxBytes));
            }
        }

        private void UpdateVScroll()
        {
            int num = this.ToScrollMax(this._scrollVmax);
            if (num > 0)
            {
                this._vScrollBar.Minimum = 0;
                this._vScrollBar.Maximum = num;
                this._vScrollBar.Value = this.ToScrollPos(this._scrollVpos);
                this._vScrollBar.Enabled = true;
            }
            else
            {
                this._vScrollBar.Enabled = false;
            }
        }

        [DefaultValue(typeof(Color), "White")]
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
            }
        }

        [Category("Appearance"), DefaultValue(typeof(Color), "WhiteSmoke")]
        public Color BackColorDisabled
        {
            get
            {
                return this._backColorDisabled;
            }
            set
            {
                this._backColorDisabled = value;
            }
        }

        [Description("Gets or sets the hex box\x00b4s border style."), DefaultValue(typeof(System.Windows.Forms.BorderStyle), "Fixed3D"), Category("Hex")]
        public System.Windows.Forms.BorderStyle BorderStyle
        {
            get
            {
                return this._borderStyle;
            }
            set
            {
                if (this._borderStyle != value)
                {
                    this._borderStyle = value;
                    switch (this._borderStyle)
                    {
                        case System.Windows.Forms.BorderStyle.None:
                            this._recBorderLeft = this._recBorderTop = this._recBorderRight = this._recBorderBottom = 0;
                            break;

                        case System.Windows.Forms.BorderStyle.FixedSingle:
                            this._recBorderLeft = this._recBorderTop = this._recBorderRight = this._recBorderBottom = 1;
                            break;

                        case System.Windows.Forms.BorderStyle.Fixed3D:
                            this._recBorderLeft = this._recBorderRight = SystemInformation.Border3DSize.Width;
                            this._recBorderTop = this._recBorderBottom = SystemInformation.Border3DSize.Height;
                            break;
                    }
                    this.UpdateRectanglePositioning();
                    this.OnBorderStyleChanged(EventArgs.Empty);
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public IByteProvider ByteProvider
        {
            get
            {
                return this._byteProvider;
            }
            set
            {
                if (this._byteProvider != value)
                {
                    if (value == null)
                    {
                        this.ActivateEmptyKeyInterpreter();
                    }
                    else
                    {
                        this.ActivateKeyInterpreter();
                    }
                    if (this._byteProvider != null)
                    {
                        this._byteProvider.LengthChanged -= new EventHandler(this._byteProvider_LengthChanged);
                    }
                    this._byteProvider = value;
                    if (this._byteProvider != null)
                    {
                        this._byteProvider.LengthChanged += new EventHandler(this._byteProvider_LengthChanged);
                    }
                    this.OnByteProviderChanged(EventArgs.Empty);
                    if (value == null)
                    {
                        this._bytePos = -1L;
                        this._byteCharacterPos = 0;
                        this._selectionLength = 0L;
                        this.DestroyCaret();
                    }
                    else
                    {
                        this.SetPosition(0L, 0);
                        this.SetSelectionLength(0L);
                        if (this._caretVisible && this.Focused)
                        {
                            this.UpdateCaret();
                        }
                        else
                        {
                            this.CreateCaret();
                        }
                    }
                    this.CheckCurrentLineChanged();
                    this.CheckCurrentPositionInLineChanged();
                    this._scrollVpos = 0L;
                    this.UpdateVisibilityBytes();
                    this.UpdateRectanglePositioning();
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(0x10), Description("Gets or sets the maximum count of bytes in one line."), Category("Hex")]
        public int BytesPerLine
        {
            get
            {
                return this._bytesPerLine;
            }
            set
            {
                if (this._bytesPerLine != value)
                {
                    this._bytesPerLine = value;
                    this.OnByteProviderChanged(EventArgs.Empty);
                    this.UpdateRectanglePositioning();
                    base.Invalidate();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public long CurrentFindingPosition
        {
            get
            {
                return this._findingPos;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public long CurrentLine
        {
            get
            {
                return this._currentLine;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public long CurrentPositionInLine
        {
            get
            {
                return (long) this._currentPositionInLine;
            }
        }

        [Editor(typeof(HexFontEditor), typeof(UITypeEditor))]
        public override System.Drawing.Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
            }
        }

        [DefaultValue(typeof(SemtechLib.Controls.HexBoxCtrl.HexCasing), "Upper"), Description("Gets or sets whether the HexBox control displays the hex characters in upper or lower case."), Category("Hex")]
        public SemtechLib.Controls.HexBoxCtrl.HexCasing HexCasing
        {
            get
            {
                if (this._hexStringFormat == "X")
                {
                    return SemtechLib.Controls.HexBoxCtrl.HexCasing.Upper;
                }
                return SemtechLib.Controls.HexBoxCtrl.HexCasing.Lower;
            }
            set
            {
                string str;
                if (value == SemtechLib.Controls.HexBoxCtrl.HexCasing.Upper)
                {
                    str = "X";
                }
                else
                {
                    str = "x";
                }
                if (this._hexStringFormat != str)
                {
                    this._hexStringFormat = str;
                    this.OnHexCasingChanged(EventArgs.Empty);
                    base.Invalidate();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public int HorizontalByteCount
        {
            get
            {
                return this._iHexMaxHBytes;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool InsertActive
        {
            get
            {
                return this._insertActive;
            }
            set
            {
                if (this._insertActive != value)
                {
                    this._insertActive = value;
                    this.DestroyCaret();
                    this.CreateCaret();
                    this.OnInsertActiveChanged(EventArgs.Empty);
                }
            }
        }

        public byte LineInfoDigits
        {
            get
            {
                return this.lineInfoDigits;
            }
            set
            {
                this.lineInfoDigits = value;
            }
        }

        [Description("Gets or sets the line info color. When this property is null, then ForeColor property is used."), DefaultValue(typeof(Color), "Empty"), Category("Hex")]
        public Color LineInfoForeColor
        {
            get
            {
                return this._lineInfoForeColor;
            }
            set
            {
                this._lineInfoForeColor = value;
                base.Invalidate();
            }
        }

        [DefaultValue(false), Category("Hex"), Description("Gets or sets the visibility of a line info.")]
        public bool LineInfoVisible
        {
            get
            {
                return this._lineInfoVisible;
            }
            set
            {
                if (this._lineInfoVisible != value)
                {
                    this._lineInfoVisible = value;
                    this.OnLineInfoVisibleChanged(EventArgs.Empty);
                    this.UpdateRectanglePositioning();
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(false), Category("Hex"), Description("Gets or sets if the count of bytes in one line is fix.")]
        public bool ReadOnly
        {
            get
            {
                return this._readOnly;
            }
            set
            {
                if (this._readOnly != value)
                {
                    this._readOnly = value;
                    this.OnReadOnlyChanged(EventArgs.Empty);
                    base.Invalidate();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), EditorBrowsable(EditorBrowsableState.Never), Bindable(false)]
        public override System.Windows.Forms.RightToLeft RightToLeft
        {
            get
            {
                return base.RightToLeft;
            }
            set
            {
                base.RightToLeft = value;
            }
        }

        [DefaultValue(typeof(Color), "Blue"), Category("Hex"), Description("Gets or sets the background color for the selected bytes.")]
        public Color SelectionBackColor
        {
            get
            {
                return this._selectionBackColor;
            }
            set
            {
                this._selectionBackColor = value;
                base.Invalidate();
            }
        }

        [Description("Gets or sets the foreground color for the selected bytes."), Category("Hex"), DefaultValue(typeof(Color), "White")]
        public Color SelectionForeColor
        {
            get
            {
                return this._selectionForeColor;
            }
            set
            {
                this._selectionForeColor = value;
                base.Invalidate();
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public long SelectionLength
        {
            get
            {
                return this._selectionLength;
            }
            set
            {
                this.SetSelectionLength(value);
                this.ScrollByteIntoView();
                base.Invalidate();
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public long SelectionStart
        {
            get
            {
                return this._bytePos;
            }
            set
            {
                this.SetPosition(value, 0);
                this.ScrollByteIntoView();
                base.Invalidate();
            }
        }

        [Category("Hex"), Description("Gets or sets the color of the shadow selection.")]
        public Color ShadowSelectionColor
        {
            get
            {
                return this._shadowSelectionColor;
            }
            set
            {
                this._shadowSelectionColor = value;
                base.Invalidate();
            }
        }

        [DefaultValue(true), Description("Gets or sets the visibility of a shadow selection."), Category("Hex")]
        public bool ShadowSelectionVisible
        {
            get
            {
                return this._shadowSelectionVisible;
            }
            set
            {
                if (this._shadowSelectionVisible != value)
                {
                    this._shadowSelectionVisible = value;
                    base.Invalidate();
                }
            }
        }

        [Description("Gets or sets the visibility of the string view."), DefaultValue(false), Category("Hex")]
        public bool StringViewVisible
        {
            get
            {
                return this._stringViewVisible;
            }
            set
            {
                if (this._stringViewVisible != value)
                {
                    this._stringViewVisible = value;
                    this.OnStringViewVisibleChanged(EventArgs.Empty);
                    this.UpdateRectanglePositioning();
                    base.Invalidate();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never), Bindable(false)]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }

        [DefaultValue(false), Category("Hex"), Description("Gets or sets if the count of bytes in one line is fix.")]
        public bool UseFixedBytesPerLine
        {
            get
            {
                return this._useFixedBytesPerLine;
            }
            set
            {
                if (this._useFixedBytesPerLine != value)
                {
                    this._useFixedBytesPerLine = value;
                    this.OnUseFixedBytesPerLineChanged(EventArgs.Empty);
                    this.UpdateRectanglePositioning();
                    base.Invalidate();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public int VerticalByteCount
        {
            get
            {
                return this._iHexMaxVBytes;
            }
        }

        [DefaultValue(false), Category("Hex"), Description("Gets or sets the visibility of a vertical scroll bar.")]
        public bool VScrollBarVisible
        {
            get
            {
                return this._vScrollBarVisible;
            }
            set
            {
                if (this._vScrollBarVisible != value)
                {
                    this._vScrollBarVisible = value;
                    if (this._vScrollBarVisible)
                    {
                        base.Controls.Add(this._vScrollBar);
                    }
                    else
                    {
                        base.Controls.Remove(this._vScrollBar);
                    }
                    this.UpdateRectanglePositioning();
                    this.UpdateScrollSize();
                    this.OnVScrollBarVisibleChanged(EventArgs.Empty);
                }
            }
        }

        private class EmptyKeyInterpreter : HexBox.IKeyInterpreter
        {
            private HexBox _hexBox;

            public EmptyKeyInterpreter(HexBox hexBox)
            {
                this._hexBox = hexBox;
            }

            public void Activate()
            {
            }

            public void Deactivate()
            {
            }

            public PointF GetCaretPointF(long byteIndex)
            {
                return new PointF();
            }

            public bool PreProcessWmChar(ref Message m)
            {
                return this._hexBox.BasePreProcessMessage(ref m);
            }

            public bool PreProcessWmKeyDown(ref Message m)
            {
                return this._hexBox.BasePreProcessMessage(ref m);
            }

            public bool PreProcessWmKeyUp(ref Message m)
            {
                return this._hexBox.BasePreProcessMessage(ref m);
            }
        }

        private interface IKeyInterpreter
        {
            void Activate();
            void Deactivate();
            PointF GetCaretPointF(long byteIndex);
            bool PreProcessWmChar(ref Message m);
            bool PreProcessWmKeyDown(ref Message m);
            bool PreProcessWmKeyUp(ref Message m);
        }

        private class KeyInterpreter : HexBox.IKeyInterpreter
        {
            private BytePositionInfo _bpi;
            private BytePositionInfo _bpiStart;
            protected HexBox _hexBox;
            private bool _mouseDown;
            protected bool _shiftDown;

            public KeyInterpreter(HexBox hexBox)
            {
                this._hexBox = hexBox;
            }

            public virtual void Activate()
            {
                this._hexBox.MouseDown += new MouseEventHandler(this.BeginMouseSelection);
                this._hexBox.MouseMove += new MouseEventHandler(this.UpdateMouseSelection);
                this._hexBox.MouseUp += new MouseEventHandler(this.EndMouseSelection);
            }

            private void BeginMouseSelection(object sender, MouseEventArgs e)
            {
                this._mouseDown = true;
                if (!this._shiftDown)
                {
                    this._bpiStart = new BytePositionInfo(this._hexBox._bytePos, this._hexBox._byteCharacterPos);
                    this._hexBox.ReleaseSelection();
                }
                else
                {
                    this.UpdateMouseSelection(this, e);
                }
            }

            public virtual void Deactivate()
            {
                this._hexBox.MouseDown -= new MouseEventHandler(this.BeginMouseSelection);
                this._hexBox.MouseMove -= new MouseEventHandler(this.UpdateMouseSelection);
                this._hexBox.MouseUp -= new MouseEventHandler(this.EndMouseSelection);
            }

            private void EndMouseSelection(object sender, MouseEventArgs e)
            {
                this._mouseDown = false;
            }

            protected virtual BytePositionInfo GetBytePositionInfo(Point p)
            {
                return this._hexBox.GetHexBytePositionInfo(p);
            }

            public virtual PointF GetCaretPointF(long byteIndex)
            {
                return this._hexBox.GetBytePointF(byteIndex);
            }

            protected virtual bool PerformPosMoveLeft()
            {
                long bytePos = this._hexBox._bytePos;
                long num2 = this._hexBox._selectionLength;
                int byteCharacterPos = this._hexBox._byteCharacterPos;
                if (num2 != 0L)
                {
                    byteCharacterPos = 0;
                    this._hexBox.SetPosition(bytePos, byteCharacterPos);
                    this._hexBox.ReleaseSelection();
                }
                else
                {
                    if ((bytePos == 0L) && (byteCharacterPos == 0))
                    {
                        return true;
                    }
                    if (byteCharacterPos > 0)
                    {
                        byteCharacterPos--;
                    }
                    else
                    {
                        bytePos = Math.Max((long) 0L, (long) (bytePos - 1L));
                        byteCharacterPos++;
                    }
                    this._hexBox.SetPosition(bytePos, byteCharacterPos);
                    if (bytePos < this._hexBox._startByte)
                    {
                        this._hexBox.PerformScrollLineUp();
                    }
                    this._hexBox.UpdateCaret();
                    this._hexBox.Invalidate();
                }
                this._hexBox.ScrollByteIntoView();
                return true;
            }

            protected virtual bool PerformPosMoveLeftByte()
            {
                long bytePos = this._hexBox._bytePos;
                int byteCharacterPos = this._hexBox._byteCharacterPos;
                if (bytePos != 0L)
                {
                    bytePos = Math.Max((long) 0L, (long) (bytePos - 1L));
                    byteCharacterPos = 0;
                    this._hexBox.SetPosition(bytePos, byteCharacterPos);
                    if (bytePos < this._hexBox._startByte)
                    {
                        this._hexBox.PerformScrollLineUp();
                    }
                    this._hexBox.UpdateCaret();
                    this._hexBox.ScrollByteIntoView();
                    this._hexBox.Invalidate();
                }
                return true;
            }

            protected virtual bool PerformPosMoveRight()
            {
                long bytePos = this._hexBox._bytePos;
                int byteCharacterPos = this._hexBox._byteCharacterPos;
                long num3 = this._hexBox._selectionLength;
                if (num3 != 0L)
                {
                    bytePos += num3;
                    byteCharacterPos = 0;
                    this._hexBox.SetPosition(bytePos, byteCharacterPos);
                    this._hexBox.ReleaseSelection();
                }
                else if ((bytePos != this._hexBox._byteProvider.Length) || (byteCharacterPos != 0))
                {
                    if (byteCharacterPos > 0)
                    {
                        bytePos = Math.Min(this._hexBox._byteProvider.Length, bytePos + 1L);
                        byteCharacterPos = 0;
                    }
                    else
                    {
                        byteCharacterPos++;
                    }
                    this._hexBox.SetPosition(bytePos, byteCharacterPos);
                    if (bytePos > (this._hexBox._endByte - 1L))
                    {
                        this._hexBox.PerformScrollLineDown();
                    }
                    this._hexBox.UpdateCaret();
                    this._hexBox.Invalidate();
                }
                this._hexBox.ScrollByteIntoView();
                return true;
            }

            protected virtual bool PerformPosMoveRightByte()
            {
                long bytePos = this._hexBox._bytePos;
                int byteCharacterPos = this._hexBox._byteCharacterPos;
                if (bytePos != this._hexBox._byteProvider.Length)
                {
                    bytePos = Math.Min(this._hexBox._byteProvider.Length, bytePos + 1L);
                    byteCharacterPos = 0;
                    this._hexBox.SetPosition(bytePos, byteCharacterPos);
                    if (bytePos > (this._hexBox._endByte - 1L))
                    {
                        this._hexBox.PerformScrollLineDown();
                    }
                    this._hexBox.UpdateCaret();
                    this._hexBox.ScrollByteIntoView();
                    this._hexBox.Invalidate();
                }
                return true;
            }

            public virtual bool PreProcessWmChar(ref Message m)
            {
                if (Control.ModifierKeys == Keys.Control)
                {
                    return this._hexBox.BasePreProcessMessage(ref m);
                }
                bool flag = this._hexBox._byteProvider.SupportsWriteByte();
                bool flag2 = this._hexBox._byteProvider.SupportsInsertBytes();
                bool flag3 = this._hexBox._byteProvider.SupportsDeleteBytes();
                long index = this._hexBox._bytePos;
                long length = this._hexBox._selectionLength;
                int byteCharacterPos = this._hexBox._byteCharacterPos;
                if ((!flag && (index != this._hexBox._byteProvider.Length)) || (!flag2 && (index == this._hexBox._byteProvider.Length)))
                {
                    return this._hexBox.BasePreProcessMessage(ref m);
                }
                char character = (char) m.WParam.ToInt32();
                if (!Uri.IsHexDigit(character))
                {
                    return this._hexBox.BasePreProcessMessage(ref m);
                }
                if (!this.RaiseKeyPress(character))
                {
                    byte num4;
                    if (this._hexBox.ReadOnly)
                    {
                        return true;
                    }
                    bool flag4 = index == this._hexBox._byteProvider.Length;
                    if ((!flag4 && flag2) && (this._hexBox.InsertActive && (byteCharacterPos == 0)))
                    {
                        flag4 = true;
                    }
                    if ((flag3 && flag2) && (length > 0L))
                    {
                        this._hexBox._byteProvider.DeleteBytes(index, length);
                        flag4 = true;
                        byteCharacterPos = 0;
                        this._hexBox.SetPosition(index, byteCharacterPos);
                    }
                    this._hexBox.ReleaseSelection();
                    if (flag4)
                    {
                        num4 = 0;
                    }
                    else
                    {
                        num4 = this._hexBox._byteProvider.ReadByte(index);
                    }
                    string str = num4.ToString("X", Thread.CurrentThread.CurrentCulture);
                    if (str.Length == 1)
                    {
                        str = "0" + str;
                    }
                    string s = character.ToString();
                    if (byteCharacterPos == 0)
                    {
                        s = s + str.Substring(1, 1);
                    }
                    else
                    {
                        s = str.Substring(0, 1) + s;
                    }
                    byte num5 = byte.Parse(s, NumberStyles.AllowHexSpecifier, Thread.CurrentThread.CurrentCulture);
                    if (flag4)
                    {
                        this._hexBox._byteProvider.InsertBytes(index, new byte[] { num5 });
                    }
                    else
                    {
                        this._hexBox._byteProvider.WriteByte(index, num5);
                    }
                    this.PerformPosMoveRight();
                    this._hexBox.Invalidate();
                }
                return true;
            }

            public virtual bool PreProcessWmKeyDown(ref Message m)
            {
                Keys keyData = ((Keys) m.WParam.ToInt32()) | Control.ModifierKeys;
                switch (keyData)
                {
                    case Keys.Back:
                    case Keys.Tab:
                    case Keys.PageUp:
                    case Keys.Next:
                    case Keys.End:
                    case Keys.Home:
                    case Keys.Left:
                    case Keys.Up:
                    case Keys.Right:
                    case Keys.Down:
                    case Keys.Delete:
                    case (Keys.Shift | Keys.ShiftKey):
                    case (Keys.Shift | Keys.Left):
                    case (Keys.Shift | Keys.Up):
                    case (Keys.Shift | Keys.Right):
                    case (Keys.Shift | Keys.Down):
                    case (Keys.Control | Keys.C):
                    case (Keys.Control | Keys.V):
                    case (Keys.Control | Keys.X):
                        if (this.RaiseKeyDown(keyData))
                        {
                            return true;
                        }
                        break;
                }
                switch (keyData)
                {
                    case Keys.Back:
                        return this.PreProcessWmKeyDown_Back(ref m);

                    case Keys.Tab:
                        return this.PreProcessWmKeyDown_Tab(ref m);

                    case Keys.PageUp:
                        return this.PreProcessWmKeyDown_PageUp(ref m);

                    case Keys.Next:
                        return this.PreProcessWmKeyDown_PageDown(ref m);

                    case Keys.End:
                        return this.PreProcessWmKeyDown_End(ref m);

                    case Keys.Home:
                        return this.PreProcessWmKeyDown_Home(ref m);

                    case Keys.Left:
                        return this.PreProcessWmKeyDown_Left(ref m);

                    case Keys.Up:
                        return this.PreProcessWmKeyDown_Up(ref m);

                    case Keys.Right:
                        return this.PreProcessWmKeyDown_Right(ref m);

                    case Keys.Down:
                        return this.PreProcessWmKeyDown_Down(ref m);

                    case Keys.Delete:
                        return this.PreProcessWmKeyDown_Delete(ref m);

                    case (Keys.Shift | Keys.ShiftKey):
                        return this.PreProcessWmKeyDown_ShiftShiftKey(ref m);

                    case (Keys.Shift | Keys.Left):
                        return this.PreProcessWmKeyDown_ShiftLeft(ref m);

                    case (Keys.Shift | Keys.Up):
                        return this.PreProcessWmKeyDown_ShiftUp(ref m);

                    case (Keys.Shift | Keys.Right):
                        return this.PreProcessWmKeyDown_ShiftRight(ref m);

                    case (Keys.Shift | Keys.Down):
                        return this.PreProcessWmKeyDown_ShiftDown(ref m);

                    case (Keys.Control | Keys.C):
                        return this.PreProcessWmKeyDown_ControlC(ref m);

                    case (Keys.Control | Keys.V):
                        return this.PreProcessWmKeyDown_ControlV(ref m);

                    case (Keys.Control | Keys.X):
                        return this.PreProcessWmKeyDown_ControlX(ref m);
                }
                this._hexBox.ScrollByteIntoView();
                return this._hexBox.BasePreProcessMessage(ref m);
            }

            protected virtual bool PreProcessWmKeyDown_Back(ref Message m)
            {
                if (this._hexBox._byteProvider.SupportsDeleteBytes())
                {
                    long num = this._hexBox._bytePos;
                    long num2 = this._hexBox._selectionLength;
                    long num4 = ((this._hexBox._byteCharacterPos == 0) && (num2 == 0L)) ? (num - 1L) : num;
                    if ((num4 < 0L) && (num2 < 1L))
                    {
                        return true;
                    }
                    long length = (num2 > 0L) ? num2 : 1L;
                    this._hexBox._byteProvider.DeleteBytes(Math.Max(0L, num4), length);
                    this._hexBox.UpdateScrollSize();
                    if (num2 == 0L)
                    {
                        this.PerformPosMoveLeftByte();
                    }
                    this._hexBox.ReleaseSelection();
                    this._hexBox.Invalidate();
                }
                return true;
            }

            protected virtual bool PreProcessWmKeyDown_ControlC(ref Message m)
            {
                this._hexBox.Copy();
                return true;
            }

            protected virtual bool PreProcessWmKeyDown_ControlV(ref Message m)
            {
                this._hexBox.Paste();
                return true;
            }

            protected virtual bool PreProcessWmKeyDown_ControlX(ref Message m)
            {
                this._hexBox.Cut();
                return true;
            }

            protected virtual bool PreProcessWmKeyDown_Delete(ref Message m)
            {
                if (this._hexBox._byteProvider.SupportsDeleteBytes())
                {
                    long index = this._hexBox._bytePos;
                    long num2 = this._hexBox._selectionLength;
                    if (index >= this._hexBox._byteProvider.Length)
                    {
                        return true;
                    }
                    long length = (num2 > 0L) ? num2 : 1L;
                    this._hexBox._byteProvider.DeleteBytes(index, length);
                    this._hexBox.UpdateScrollSize();
                    this._hexBox.ReleaseSelection();
                    this._hexBox.Invalidate();
                }
                return true;
            }

            protected virtual bool PreProcessWmKeyDown_Down(ref Message m)
            {
                long bytePos = this._hexBox._bytePos;
                int byteCharacterPos = this._hexBox._byteCharacterPos;
                if ((bytePos != this._hexBox._byteProvider.Length) || (byteCharacterPos != 0))
                {
                    bytePos = Math.Min(this._hexBox._byteProvider.Length, bytePos + this._hexBox._iHexMaxHBytes);
                    if (bytePos == this._hexBox._byteProvider.Length)
                    {
                        byteCharacterPos = 0;
                    }
                    this._hexBox.SetPosition(bytePos, byteCharacterPos);
                    if (bytePos > (this._hexBox._endByte - 1L))
                    {
                        this._hexBox.PerformScrollLineDown();
                    }
                    this._hexBox.UpdateCaret();
                    this._hexBox.ScrollByteIntoView();
                    this._hexBox.ReleaseSelection();
                    this._hexBox.Invalidate();
                }
                return true;
            }

            protected virtual bool PreProcessWmKeyDown_End(ref Message m)
            {
                long bytePos = this._hexBox._bytePos;
                int byteCharacterPos = this._hexBox._byteCharacterPos;
                if (bytePos < (this._hexBox._byteProvider.Length - 1L))
                {
                    bytePos = this._hexBox._byteProvider.Length;
                    byteCharacterPos = 0;
                    this._hexBox.SetPosition(bytePos, byteCharacterPos);
                    this._hexBox.ScrollByteIntoView();
                    this._hexBox.UpdateCaret();
                    this._hexBox.ReleaseSelection();
                }
                return true;
            }

            protected virtual bool PreProcessWmKeyDown_Home(ref Message m)
            {
                long bytePos = this._hexBox._bytePos;
                int byteCharacterPos = this._hexBox._byteCharacterPos;
                if (bytePos >= 1L)
                {
                    bytePos = 0L;
                    byteCharacterPos = 0;
                    this._hexBox.SetPosition(bytePos, byteCharacterPos);
                    this._hexBox.ScrollByteIntoView();
                    this._hexBox.UpdateCaret();
                    this._hexBox.ReleaseSelection();
                }
                return true;
            }

            protected virtual bool PreProcessWmKeyDown_Left(ref Message m)
            {
                return this.PerformPosMoveLeft();
            }

            protected virtual bool PreProcessWmKeyDown_PageDown(ref Message m)
            {
                long bytePos = this._hexBox._bytePos;
                int byteCharacterPos = this._hexBox._byteCharacterPos;
                if ((bytePos != this._hexBox._byteProvider.Length) || (byteCharacterPos != 0))
                {
                    bytePos = Math.Min(this._hexBox._byteProvider.Length, bytePos + this._hexBox._iHexMaxBytes);
                    if (bytePos == this._hexBox._byteProvider.Length)
                    {
                        byteCharacterPos = 0;
                    }
                    this._hexBox.SetPosition(bytePos, byteCharacterPos);
                    if (bytePos > (this._hexBox._endByte - 1L))
                    {
                        this._hexBox.PerformScrollPageDown();
                    }
                    this._hexBox.ReleaseSelection();
                    this._hexBox.UpdateCaret();
                    this._hexBox.Invalidate();
                }
                return true;
            }

            protected virtual bool PreProcessWmKeyDown_PageUp(ref Message m)
            {
                long bytePos = this._hexBox._bytePos;
                int num2 = this._hexBox._byteCharacterPos;
                if ((bytePos != 0L) || (num2 != 0))
                {
                    bytePos = Math.Max((long) 0L, (long) (bytePos - this._hexBox._iHexMaxBytes));
                    if (bytePos == 0L)
                    {
                        return true;
                    }
                    this._hexBox.SetPosition(bytePos);
                    if (bytePos < this._hexBox._startByte)
                    {
                        this._hexBox.PerformScrollPageUp();
                    }
                    this._hexBox.ReleaseSelection();
                    this._hexBox.UpdateCaret();
                    this._hexBox.Invalidate();
                }
                return true;
            }

            protected virtual bool PreProcessWmKeyDown_Right(ref Message m)
            {
                return this.PerformPosMoveRight();
            }

            protected virtual bool PreProcessWmKeyDown_ShiftDown(ref Message m)
            {
                long start = this._hexBox._bytePos;
                long length = this._hexBox._selectionLength;
                long num3 = this._hexBox._byteProvider.Length;
                if (((start + length) + this._hexBox._iHexMaxHBytes) <= num3)
                {
                    if (this._bpiStart.Index <= start)
                    {
                        length += this._hexBox._iHexMaxHBytes;
                        this._hexBox.InternalSelect(start, length);
                        this._hexBox.ScrollByteIntoView(start + length);
                    }
                    else
                    {
                        length -= this._hexBox._iHexMaxHBytes;
                        if (length < 0L)
                        {
                            start = this._bpiStart.Index;
                            length = -length;
                        }
                        else
                        {
                            start += this._hexBox._iHexMaxHBytes;
                        }
                        this._hexBox.InternalSelect(start, length);
                        this._hexBox.ScrollByteIntoView();
                    }
                }
                return true;
            }

            protected virtual bool PreProcessWmKeyDown_ShiftLeft(ref Message m)
            {
                long start = this._hexBox._bytePos;
                long length = this._hexBox._selectionLength;
                if ((start + length) >= 1L)
                {
                    if ((start + length) <= this._bpiStart.Index)
                    {
                        if (start == 0L)
                        {
                            return true;
                        }
                        start -= 1L;
                        length += 1L;
                    }
                    else
                    {
                        length = Math.Max((long) 0L, (long) (length - 1L));
                    }
                    this._hexBox.ScrollByteIntoView();
                    this._hexBox.InternalSelect(start, length);
                }
                return true;
            }

            protected virtual bool PreProcessWmKeyDown_ShiftRight(ref Message m)
            {
                long start = this._hexBox._bytePos;
                long length = this._hexBox._selectionLength;
                if ((start + length) < this._hexBox._byteProvider.Length)
                {
                    if (this._bpiStart.Index <= start)
                    {
                        length += 1L;
                        this._hexBox.InternalSelect(start, length);
                        this._hexBox.ScrollByteIntoView(start + length);
                    }
                    else
                    {
                        start += 1L;
                        length = Math.Max((long) 0L, (long) (length - 1L));
                        this._hexBox.InternalSelect(start, length);
                        this._hexBox.ScrollByteIntoView();
                    }
                }
                return true;
            }

            protected virtual bool PreProcessWmKeyDown_ShiftShiftKey(ref Message m)
            {
                if (!this._mouseDown)
                {
                    if (this._shiftDown)
                    {
                        return true;
                    }
                    this._shiftDown = true;
                    if (this._hexBox._selectionLength > 0L)
                    {
                        return true;
                    }
                    this._bpiStart = new BytePositionInfo(this._hexBox._bytePos, this._hexBox._byteCharacterPos);
                }
                return true;
            }

            protected virtual bool PreProcessWmKeyDown_ShiftTab(ref Message m)
            {
                if (this._hexBox._keyInterpreter is HexBox.StringKeyInterpreter)
                {
                    this._shiftDown = false;
                    this._hexBox.ActivateKeyInterpreter();
                    this._hexBox.ScrollByteIntoView();
                    this._hexBox.ReleaseSelection();
                    this._hexBox.UpdateCaret();
                    this._hexBox.Invalidate();
                    return true;
                }
                if (this._hexBox.Parent != null)
                {
                    this._hexBox.Parent.SelectNextControl(this._hexBox, false, true, true, true);
                }
                return true;
            }

            protected virtual bool PreProcessWmKeyDown_ShiftUp(ref Message m)
            {
                long start = this._hexBox._bytePos;
                long length = this._hexBox._selectionLength;
                if (((start - this._hexBox._iHexMaxHBytes) >= 0L) || (start > this._bpiStart.Index))
                {
                    if (this._bpiStart.Index >= (start + length))
                    {
                        start -= this._hexBox._iHexMaxHBytes;
                        length += this._hexBox._iHexMaxHBytes;
                        this._hexBox.InternalSelect(start, length);
                        this._hexBox.ScrollByteIntoView();
                    }
                    else
                    {
                        length -= this._hexBox._iHexMaxHBytes;
                        if (length < 0L)
                        {
                            start = this._bpiStart.Index + length;
                            length = -length;
                            this._hexBox.InternalSelect(start, length);
                            this._hexBox.ScrollByteIntoView();
                        }
                        else
                        {
                            length -= this._hexBox._iHexMaxHBytes;
                            this._hexBox.InternalSelect(start, length);
                            this._hexBox.ScrollByteIntoView(start + length);
                        }
                    }
                }
                return true;
            }

            protected virtual bool PreProcessWmKeyDown_Tab(ref Message m)
            {
                if (this._hexBox._stringViewVisible && (this._hexBox._keyInterpreter.GetType() == typeof(HexBox.KeyInterpreter)))
                {
                    this._hexBox.ActivateStringKeyInterpreter();
                    this._hexBox.ScrollByteIntoView();
                    this._hexBox.ReleaseSelection();
                    this._hexBox.UpdateCaret();
                    this._hexBox.Invalidate();
                    return true;
                }
                if (this._hexBox.Parent != null)
                {
                    this._hexBox.Parent.SelectNextControl(this._hexBox, true, true, true, true);
                }
                return true;
            }

            protected virtual bool PreProcessWmKeyDown_Up(ref Message m)
            {
                long bytePos = this._hexBox._bytePos;
                int num2 = this._hexBox._byteCharacterPos;
                if ((bytePos != 0L) || (num2 != 0))
                {
                    bytePos = Math.Max((long) (-1L), (long) (bytePos - this._hexBox._iHexMaxHBytes));
                    if (bytePos == -1L)
                    {
                        return true;
                    }
                    this._hexBox.SetPosition(bytePos);
                    if (bytePos < this._hexBox._startByte)
                    {
                        this._hexBox.PerformScrollLineUp();
                    }
                    this._hexBox.UpdateCaret();
                    this._hexBox.Invalidate();
                }
                this._hexBox.ScrollByteIntoView();
                this._hexBox.ReleaseSelection();
                return true;
            }

            public virtual bool PreProcessWmKeyUp(ref Message m)
            {
                Keys keyData = ((Keys) m.WParam.ToInt32()) | Control.ModifierKeys;
                Keys keys3 = keyData;
                if (((keys3 != Keys.ShiftKey) && (keys3 != Keys.Insert)) || !this.RaiseKeyUp(keyData))
                {
                    Keys keys4 = keyData;
                    if (keys4 != Keys.ShiftKey)
                    {
                        if (keys4 == Keys.Insert)
                        {
                            return this.PreProcessWmKeyUp_Insert(ref m);
                        }
                        return this._hexBox.BasePreProcessMessage(ref m);
                    }
                    this._shiftDown = false;
                }
                return true;
            }

            protected virtual bool PreProcessWmKeyUp_Insert(ref Message m)
            {
                this._hexBox.InsertActive = !this._hexBox.InsertActive;
                return true;
            }

            protected bool RaiseKeyDown(Keys keyData)
            {
                KeyEventArgs e = new KeyEventArgs(keyData);
                this._hexBox.OnKeyDown(e);
                return e.Handled;
            }

            protected bool RaiseKeyPress(char keyChar)
            {
                KeyPressEventArgs e = new KeyPressEventArgs(keyChar);
                this._hexBox.OnKeyPress(e);
                return e.Handled;
            }

            protected bool RaiseKeyUp(Keys keyData)
            {
                KeyEventArgs e = new KeyEventArgs(keyData);
                this._hexBox.OnKeyUp(e);
                return e.Handled;
            }

            private void UpdateMouseSelection(object sender, MouseEventArgs e)
            {
                if (this._mouseDown)
                {
                    long num2;
                    long num3;
                    this._bpi = this.GetBytePositionInfo(new Point(e.X, e.Y));
                    long index = this._bpi.Index;
                    if (index < this._bpiStart.Index)
                    {
                        num2 = index;
                        num3 = this._bpiStart.Index - index;
                    }
                    else if (index > this._bpiStart.Index)
                    {
                        num2 = this._bpiStart.Index;
                        num3 = index - num2;
                    }
                    else
                    {
                        num2 = this._hexBox._bytePos;
                        num3 = 0L;
                    }
                    if ((num2 != this._hexBox._bytePos) || (num3 != this._hexBox._selectionLength))
                    {
                        this._hexBox.InternalSelect(num2, num3);
                        this._hexBox.ScrollByteIntoView(this._bpi.Index);
                    }
                    long num4 = this._hexBox._bytePos;
                    long num5 = this._hexBox._selectionLength;
                    if (this._bpiStart.Index <= num4)
                    {
                        num5 += this._hexBox._iHexMaxHBytes;
                        this._hexBox.ScrollByteIntoView(num4 + num5);
                    }
                    else
                    {
                        num5 -= this._hexBox._iHexMaxHBytes;
                        if (num5 < 0L)
                        {
                            num4 = this._bpiStart.Index;
                            num5 = -num5;
                        }
                        else
                        {
                            num4 += this._hexBox._iHexMaxHBytes;
                            num5 -= this._hexBox._iHexMaxHBytes;
                        }
                        this._hexBox.ScrollByteIntoView();
                    }
                }
            }
        }

        private class StringKeyInterpreter : HexBox.KeyInterpreter
        {
            public StringKeyInterpreter(HexBox hexBox) : base(hexBox)
            {
                base._hexBox._byteCharacterPos = 0;
            }

            protected override BytePositionInfo GetBytePositionInfo(Point p)
            {
                return base._hexBox.GetStringBytePositionInfo(p);
            }

            public override PointF GetCaretPointF(long byteIndex)
            {
                Point gridBytePoint = base._hexBox.GetGridBytePoint(byteIndex);
                return base._hexBox.GetByteStringPointF(gridBytePoint);
            }

            public override bool PreProcessWmChar(ref Message m)
            {
                if (Control.ModifierKeys == Keys.Control)
                {
                    return base._hexBox.BasePreProcessMessage(ref m);
                }
                bool flag = base._hexBox._byteProvider.SupportsWriteByte();
                bool flag2 = base._hexBox._byteProvider.SupportsInsertBytes();
                bool flag3 = base._hexBox._byteProvider.SupportsDeleteBytes();
                long index = base._hexBox._bytePos;
                long length = base._hexBox._selectionLength;
                int byteCharacterPos = base._hexBox._byteCharacterPos;
                if ((!flag && (index != base._hexBox._byteProvider.Length)) || (!flag2 && (index == base._hexBox._byteProvider.Length)))
                {
                    return base._hexBox.BasePreProcessMessage(ref m);
                }
                char keyChar = (char) m.WParam.ToInt32();
                if (!base.RaiseKeyPress(keyChar))
                {
                    if (base._hexBox.ReadOnly)
                    {
                        return true;
                    }
                    bool flag4 = index == base._hexBox._byteProvider.Length;
                    if ((!flag4 && flag2) && base._hexBox.InsertActive)
                    {
                        flag4 = true;
                    }
                    if ((flag3 && flag2) && (length > 0L))
                    {
                        base._hexBox._byteProvider.DeleteBytes(index, length);
                        flag4 = true;
                        byteCharacterPos = 0;
                        base._hexBox.SetPosition(index, byteCharacterPos);
                    }
                    base._hexBox.ReleaseSelection();
                    if (flag4)
                    {
                        base._hexBox._byteProvider.InsertBytes(index, new byte[] { (byte) keyChar });
                    }
                    else
                    {
                        base._hexBox._byteProvider.WriteByte(index, (byte) keyChar);
                    }
                    this.PerformPosMoveRightByte();
                    base._hexBox.Invalidate();
                }
                return true;
            }

            public override bool PreProcessWmKeyDown(ref Message m)
            {
                Keys keyData = ((Keys) m.WParam.ToInt32()) | Control.ModifierKeys;
                Keys keys3 = keyData;
                if (((keys3 == Keys.Tab) || (keys3 == (Keys.Shift | Keys.Tab))) && base.RaiseKeyDown(keyData))
                {
                    return true;
                }
                Keys keys4 = keyData;
                if (keys4 != Keys.Tab)
                {
                    if (keys4 == (Keys.Shift | Keys.Tab))
                    {
                        return this.PreProcessWmKeyDown_ShiftTab(ref m);
                    }
                    return base.PreProcessWmKeyDown(ref m);
                }
                return this.PreProcessWmKeyDown_Tab(ref m);
            }

            protected override bool PreProcessWmKeyDown_Left(ref Message m)
            {
                return this.PerformPosMoveLeftByte();
            }

            protected override bool PreProcessWmKeyDown_Right(ref Message m)
            {
                return this.PerformPosMoveRightByte();
            }
        }
    }
}

