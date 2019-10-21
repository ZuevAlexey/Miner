namespace Models {
    /// <summary>
    ///     Фабрика для создания полей
    /// </summary>
    public interface IFieldFactory {
        /// <summary>
        ///     Создать поле для заданных настроек
        /// </summary>
        /// <param name="settings">Настройки</param>
        /// <returns>Поле</returns>
        Field Create(GameSettings settings);
    }
}
