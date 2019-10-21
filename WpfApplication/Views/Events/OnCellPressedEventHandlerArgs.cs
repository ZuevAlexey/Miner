using System;
using System.Windows.Input;
using Models;

namespace WpfApplication.Views.Events {
    public class OnCellPressedEventHandlerArgs: EventArgs {
        public OnCellPressedEventHandlerArgs(Position position, MouseButton button) {
            Position = position;
            Button = button;
        }

        public Position Position { get; }
        public MouseButton Button { get; }
    }
}
