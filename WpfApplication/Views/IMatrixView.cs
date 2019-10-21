using Models;
using WpfApplication.Views.Cell;
using WpfApplication.Views.Cell.Events;

namespace WpfApplication.Views {
    /// <summary>
    /// Представление для отображения поля игры
    /// </summary>
    public interface IMatrixView {
        /// <summary>
        /// Событие, происходящее когда ячейка поля нажата
        /// </summary>
        event OnCellPressedEventHandler OnCellPressed;
        
        /// <summary>
        /// Изменить состояние ячейки
        /// </summary>
        /// <param name="position">Позиция изменяемой ячейки</param>
        /// <param name="newState">Новое состояние ячейки</param>
        void ChangeCellState(Position position, CellData newState);
        
        /// <summary>
        /// Создать поле
        /// </summary>
        /// <param name="rows">Количество строк</param>
        /// <param name="columns">Количество столюцов</param>
        void CreateField(byte rows, byte columns);
        
        /// <summary>
        /// Получить состояние ячейки
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        CellState GetCellState(Position position);
    }
}
