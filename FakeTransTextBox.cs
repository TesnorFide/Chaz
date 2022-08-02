using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;

namespace WindowsFormsApplication1
{
    public class FakeTransTextBox : System.Windows.Forms.TextBox
    {
        public FakeTransTextBox()
        {
            SetStyle(System.Windows.Forms.ControlStyles.SupportsTransparentBackColor | System.Windows.Forms.ControlStyles.UserPaint | System.Windows.Forms.ControlStyles.AllPaintingInWmPaint, true);
            this.BackColor = Color.Transparent;
            BorderStyle = System.Windows.Forms.BorderStyle.None;
        }
    }
}