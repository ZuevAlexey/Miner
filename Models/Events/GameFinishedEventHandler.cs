namespace Models.Events {
    /// <summary>
    ///     Метод, обрабатывающий событие завершения игры
    /// </summary>
    /// <param name="sender">Источник события</param>
    /// <param name="args">Аргументы события завершения игры</param>
    public delegate void GameFinishedEventHandler(object sender, GameFinishedEventHandlerArgs args);
}
