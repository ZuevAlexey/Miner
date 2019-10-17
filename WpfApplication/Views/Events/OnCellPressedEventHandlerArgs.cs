using System;
using System.Windows.Input;

namespace WpfApplication.Views {
    public class OnCellPressedEventHandlerArgs : EventArgs {
        public OnCellPressedEventHandlerArgs(byte row, byte column, MouseButton button) {
            Row = row;
            Column = column;
            Button = button;
        }

        public byte Row { get; }
        public byte Column { get; }
        public MouseButton Button { get; }
    }
}