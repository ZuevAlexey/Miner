namespace Models {
    public interface IMiningAlgorithm {
        void DropMines(Field field, PlaySettings settings);
    }
}