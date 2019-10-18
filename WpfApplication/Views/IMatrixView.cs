using Models;
using WpfApplication.Views.Events;

namespace WpfApplication.Views {
    public interface IMatrixView {
        event OnCellPressedEventHandler OnCellPressed;
        void ChangeCellState(Position position, CellState newState, bool isMineHere, byte minesAroundCount);
        void CreateField(byte rows, byte columns);
        CellState GetCellState(Position position);
    }
}