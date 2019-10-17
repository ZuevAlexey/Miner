using Models;

namespace WpfApplication.Views {
    public interface IMatrixView {
        event OnCellPressedEventHandler OnCellPressed;
        void ChangeCellState(byte row, byte column, CellState newState, string displayString);
        void CreateField(byte rows, byte columns);
    }
}