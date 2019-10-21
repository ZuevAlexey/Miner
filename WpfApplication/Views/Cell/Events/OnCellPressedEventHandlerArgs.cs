using System;
using System.Windows.Input;
using Models;

namespace WpfApplication.Views.Cell.Events {
    /// <summary>
    /// Аргументы события нажатия на ячейку
    /// </summary>
    public class OnCellPressedEventHandlerArgs: EventArgs {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="position">Позиция нажатой ячейки</param>
        /// <param name="button">Кнопка мыши, которая была нажата</param>
        public OnCellPressedEventHandlerArgs(Position position, MouseButton button) {
            Position = position;
            Button = button;
        }

        
        /// <summary>
        /// Позиция нажатой ячейки
        /// </summary>
        public Position Position { get; }
        
        /// <summary>
        /// Кнопка мыши, которая была нажата
        /// </summary>
        public MouseButton Button { get; }
    }
}
