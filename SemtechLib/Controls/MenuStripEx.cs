using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace SemtechLib.Controls
{
	public class MenuStripEx : MenuStrip
	{
		private bool clickThrough;
		private bool suppressHighlighting = true;

		protected override void WndProc(ref Message m)
		{
			if (m.Msg != 0x200L || !suppressHighlighting || base.TopLevelControl.ContainsFocus)
			{
				base.WndProc(ref m);
				if (m.Msg == 0x21L && clickThrough && m.Result == (IntPtr)2)
					m.Result = (IntPtr)1;
			}
		}

		[DefaultValue("false"), Category("Extended")]
		public bool ClickThrough
		{
			get { return clickThrough; }
			set { clickThrough = value; }
		}

		[Category("Extended"), DefaultValue("true")]
		public bool SuppressHighlighting
		{
			get { return suppressHighlighting; }
			set { suppressHighlighting = value; }
		}
	}
}
