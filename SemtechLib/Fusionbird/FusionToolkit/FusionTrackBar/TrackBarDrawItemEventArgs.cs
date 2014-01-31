using System;
using System.Drawing;

namespace Fusionbird.FusionToolkit.FusionTrackBar
{
	public class TrackBarDrawItemEventArgs : EventArgs
	{
		private Rectangle _bounds;
		private System.Drawing.Graphics _graphics;
		private TrackBarItemState _state;

		public TrackBarDrawItemEventArgs(System.Drawing.Graphics graphics, Rectangle bounds, TrackBarItemState state)
		{
			_graphics = graphics;
			_bounds = bounds;
			_state = state;
		}

		public Rectangle Bounds
		{
			get { return _bounds; }
		}

		public System.Drawing.Graphics Graphics
		{
			get { return _graphics; }
		}

		public TrackBarItemState State
		{
			get { return _state; }
		}
	}
}

