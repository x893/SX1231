namespace SemtechLib.Controls
{
    using System.Windows.Forms;

    internal class VProgressBar : ProgressBar
    {
        protected override System.Windows.Forms.CreateParams CreateParams
        {
            get
            {
                System.Windows.Forms.CreateParams createParams = base.CreateParams;
                createParams.Style |= 4;
                return createParams;
            }
        }
    }
}

