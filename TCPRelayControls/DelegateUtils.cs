using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TCPRelayControls
{
    public static class DelegateUtils
    {
        private delegate void AsyncCallback<C>(Control invoker, C control, Action<C> action) where C : Control;
        private delegate void AsyncCallback<C, P>(Control invoker, C control, P parameter, Action<C, P> action) where C : Control;
        private delegate void AsyncCallback<C, P1, P2>(Control invoker, C control, P1 parameter1, P2 parameter2, Action<C, P1, P2> action) where C : Control;

        public static void DoAction<C>(Control invoker, C control, Action<C> action)
            where C : Control
        {
            if (control.InvokeRequired)
            {
                AsyncCallback<C> d = new AsyncCallback<C>(DoAction);
                try
                {
                    invoker.Invoke(d, new object[] { invoker, control, action });
                }
                catch (ObjectDisposedException)
                {
                    // ignore this
                }
            }
            else
            {
                action.Invoke(control);
            }
        }

        public static void DoAction<C, P>(Control invoker, C control, P parameter, Action<C, P> action)
            where C : Control
        {
            if (control.InvokeRequired)
            {
                AsyncCallback<C, P> d = new AsyncCallback<C, P>(DoAction);
                try
                {
                    invoker.Invoke(d, new object[] { invoker, control, parameter, action });
                }
                catch (ObjectDisposedException)
                {
                    // ignore this
                }
            }
            else
            {
                action.Invoke(control, parameter);
            }
        }

        public static void DoAction<C, P1, P2>(Control invoker, C control, P1 parameter1, P2 parameter2, Action<C, P1, P2> action)
            where C : Control
        {
            if (control.InvokeRequired)
            {
                AsyncCallback<C, P1, P2> d = new AsyncCallback<C, P1, P2>(DoAction);
                try
                {
                    invoker.Invoke(d, new object[] { invoker, control, parameter1, parameter2, action });
                }
                catch (ObjectDisposedException)
                {
                    // ignore this
                }
            }
            else
            {
                action.Invoke(control, parameter1, parameter2);
            }
        }

        public static void SetEnabled(Control invoker, Control control, bool enabled)
        {
            DoAction(invoker, control, enabled, (ctl, en) =>
            {
                ctl.Enabled = en;
            });
        }

        public static void SetVisible(Control invoker, Control control, bool visible)
        {
            DoAction(invoker, control, visible, (ctl, vs) =>
            {
                ctl.Visible = vs;
            });
        }

        public static void SetText(Control invoker, Control control, string text)
        {
            DoAction(invoker, control, text, (ctl, tx) =>
            {
                ctl.Text = tx;
            });
        }

        public static void SetToolTip(Control invoker, ToolTip tooltip, Control target, string text)
        {
            DoAction(invoker, target, tooltip, text, (tgt, tt, tx) =>
            {
                tt.SetToolTip(tgt, tx);
            });
        }

        public static void Refresh(Control invoker, Control control)
        {
            DoAction(invoker, control, (ctl) =>
            {
                ctl.Refresh();
            });
        }
    }
}
