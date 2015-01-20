using System;
using System.Linq;
using System.Windows.Forms;

namespace factor10.VisionQuest.Forms
{
    public partial class BaseForm : Form, IMessageFilter
    {
        public BaseForm()
        {
            InitializeComponent();
            Application.AddMessageFilter(this);
        }

        protected override void OnClosed(EventArgs e)
        {
            Application.RemoveMessageFilter(this);
            base.OnClosed(e);
        }

        private static Control findFocusedControl(Control ctrl)
        {
            if (ctrl == null || !ctrl.ContainsFocus)
                return null;
            return ctrl.Focused
                ? ctrl
                : (from Control ctrlChild in ctrl.Controls select findFocusedControl(ctrlChild)).FirstOrDefault(ctrlTest => ctrlTest != null);
        }

        public virtual bool OnEnterKey()
        {
            var focusedControl = findFocusedControl(this);
            if (focusedControl == null)
                return false;

            var button = focusedControl as Button;
            if (button != null)
            {
                button.PerformClick();
                return true;
            }
            if (AcceptButton == null)
                return false;
            AcceptButton.PerformClick();
            return true;
        }

        bool IMessageFilter.PreFilterMessage(ref Message m)
        {
            if (!ContainsFocus)
                return false;

            if ((m.Msg == 256 || m.Msg == 257) && m.WParam.ToInt32() == (int)Keys.Tab)
            {
                    if (m.Msg == 256 && (ModifierKeys == Keys.None || ModifierKeys == Keys.Shift))
                    SelectNextControl(ActiveControl, ModifierKeys == Keys.None, true, true, true);
                return true;
            }

            if (m.Msg == 256 && m.WParam.ToInt32() == (int)Keys.Enter)
            {
                if(OnEnterKey())
                    return true;
            }

            if (m.Msg == 256) // && m.WParam.ToInt32() == (int) Keys.F4)
            {
                //Close();
                //return true;
            }
            return false;
        }

    }
}
