namespace SemtechLib.Controls.HexBoxCtrl.Design
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    internal class HexFontEditor : FontEditor
    {
        private object value;

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            this.value = value;
            if ((provider != null) && (((IWindowsFormsEditorService) provider.GetService(typeof(IWindowsFormsEditorService))) != null))
            {
                FontDialog dialog = new FontDialog();
                dialog.ShowApply = false;
                dialog.ShowColor = false;
                dialog.AllowVerticalFonts = false;
                dialog.AllowScriptChange = false;
                dialog.FixedPitchOnly = true;
                dialog.ShowEffects = false;
                dialog.ShowHelp = false;
                Font font = value as Font;
                if (font != null)
                {
                    dialog.Font = font;
                }
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    this.value = dialog.Font;
                }
                dialog.Dispose();
            }
            value = this.value;
            this.value = null;
            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
    }
}

