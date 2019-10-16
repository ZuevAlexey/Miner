using Models.Events;

namespace Models {
    public interface IGameManager {
        Field GenerateField();
        void ChangeState(byte row, byte column, CellState newState);
        event CellStateChangedEventHandler OnCellStateChanged;
    }
}