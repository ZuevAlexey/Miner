namespace Models.Events {
    /// <summary>
    ///     Метод, обрабатывающий событие открытия кнопки
    /// </summary>
    /// <param name="sender">Источник-события</param>
    /// <param name="args">Аргументы события открытия кнопки</param>
    public delegate void CellOpenedEventHandler(object sender, CellOpenedEventHandlerArgs args);
}
