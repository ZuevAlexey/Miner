using System;

namespace Models.Events {
    /// <summary>
    ///     Метод, обрабатывающий событие начала игры
    /// </summary>
    /// <param name="sender">Источник события</param>
    /// <param name="args">Аргументы события</param>
    public delegate void GameStartedEventHandler(object sender, EventArgs args);
}
