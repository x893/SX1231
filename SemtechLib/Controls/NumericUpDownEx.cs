namespace SemtechLib.Controls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Reflection;
    using System.Threading;
    using System.Windows.Forms;

    [DesignerCategory("code")]
    public class NumericUpDownEx : NumericUpDown
    {
        private bool mouseOver;
        private TextBox tBox;
        private Control udBtn;

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), Category("Mouse")]
        public new event EventHandler MouseEnter;

        [EditorBrowsable(EditorBrowsableState.Always), Category("Mouse"), Browsable(true)]
        public new event EventHandler MouseLeave;

        public NumericUpDownEx()
        {
            this.tBox = (TextBox) this.GetPrivateField("upDownEdit");
            if (this.tBox == null)
            {
                throw new ArgumentNullException(base.GetType().FullName + ": Can't find internal TextBox field.");
            }
            this.udBtn = this.GetPrivateField("upDownButtons");
            if (this.udBtn == null)
            {
                throw new ArgumentNullException(base.GetType().FullName + ": Can't find internal UpDown buttons field.");
            }
            this.tBox.MouseEnter += new EventHandler(this.MouseEnterLeave);
            this.tBox.MouseLeave += new EventHandler(this.MouseEnterLeave);
            this.udBtn.MouseEnter += new EventHandler(this.MouseEnterLeave);
            this.udBtn.MouseLeave += new EventHandler(this.MouseEnterLeave);
            base.MouseEnter += new EventHandler(this.MouseEnterLeave);
            base.MouseLeave += new EventHandler(this.MouseEnterLeave);
        }

        protected Control GetPrivateField(string name)
        {
            return (Control) base.GetType().GetField(name, BindingFlags.FlattenHierarchy | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this);
        }

        private void MouseEnterLeave(object sender, EventArgs e)
        {
            Rectangle rectangle = base.RectangleToScreen(base.ClientRectangle);
            Point mousePosition = Control.MousePosition;
            bool flag = rectangle.Contains(mousePosition);
            if (this.mouseOver ^ flag)
            {
                this.mouseOver = flag;
                if (this.mouseOver)
                {
                    if (this.MouseEnter != null)
                    {
                        this.MouseEnter(this, EventArgs.Empty);
                    }
                }
                else if (this.MouseLeave != null)
                {
                    this.MouseLeave(this, EventArgs.Empty);
                }
            }
        }
    }
}

