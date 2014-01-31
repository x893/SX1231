using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace SemtechLib.Controls
{
    [DesignerCategory("code")]
    public class GroupBoxEx : GroupBox
    {
        private bool mouseOver;

        [Category("Mouse"), Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public new event EventHandler MouseEnter;
        [Category("Mouse"), Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public new event EventHandler MouseLeave;

        public GroupBoxEx()
        {
            base.MouseEnter += new EventHandler(MouseEnterLeave);
            base.MouseLeave += new EventHandler(MouseEnterLeave);
        }

        private void MouseEnterLeave(object sender, EventArgs e)
        {
            Rectangle rectangle = base.RectangleToScreen(base.ClientRectangle);
            Point mousePosition = Control.MousePosition;
            bool flag = rectangle.Contains(mousePosition);
            if (mouseOver ^ flag)
            {
                mouseOver = flag;
                if (mouseOver)
                {
                    if (MouseEnter != null)
                        MouseEnter(this, EventArgs.Empty);
                }
                else if (MouseLeave != null)
                    MouseLeave(this, EventArgs.Empty);
            }
        }
    }
}
