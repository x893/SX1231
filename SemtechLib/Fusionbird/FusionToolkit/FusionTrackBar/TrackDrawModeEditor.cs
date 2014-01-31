using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Fusionbird.FusionToolkit.FusionTrackBar
{
    public class TrackDrawModeEditor : UITypeEditor
    {
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            TrackBarOwnerDrawParts none = TrackBarOwnerDrawParts.None;
            if (!(value is TrackBarOwnerDrawParts) || (provider == null))
                return value;

			IWindowsFormsEditorService service = (IWindowsFormsEditorService) provider.GetService(typeof(IWindowsFormsEditorService));
            if (service == null)
                return value;
            
			CheckedListBox control = new CheckedListBox();
            control.BorderStyle = System.Windows.Forms.BorderStyle.None;
            control.CheckOnClick = true;
            control.Items.Add("Ticks", (((Fusionbird.FusionToolkit.FusionTrackBar.FusionTrackBar) context.Instance).OwnerDrawParts & TrackBarOwnerDrawParts.Ticks) == TrackBarOwnerDrawParts.Ticks);
            control.Items.Add("Thumb", (((Fusionbird.FusionToolkit.FusionTrackBar.FusionTrackBar) context.Instance).OwnerDrawParts & TrackBarOwnerDrawParts.Thumb) == TrackBarOwnerDrawParts.Thumb);
            control.Items.Add("Channel", (((Fusionbird.FusionToolkit.FusionTrackBar.FusionTrackBar) context.Instance).OwnerDrawParts & TrackBarOwnerDrawParts.Channel) == TrackBarOwnerDrawParts.Channel);
            service.DropDownControl(control);
            IEnumerator enumerator = control.CheckedItems.GetEnumerator();
            while (enumerator.MoveNext())
            {
                object objectValue = RuntimeHelpers.GetObjectValue(enumerator.Current);
                none |= (TrackBarOwnerDrawParts) Enum.Parse(typeof(TrackBarOwnerDrawParts), objectValue.ToString());
            }
            control.Dispose();
            service.CloseDropDown();
            return none;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }
    }
}

