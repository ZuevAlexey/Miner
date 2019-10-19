namespace Models {
    public interface IFieldFactory {
        Field Create(GameSettings settings);
    }
}