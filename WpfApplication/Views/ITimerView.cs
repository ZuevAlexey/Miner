namespace WpfApplication.Views {
    /// <summary>
    /// Представление таймера
    /// </summary>
    public interface ITimerView {
        /// <summary>
        /// Запустить таймер
        /// </summary>
        void Start();
        
        /// <summary>
        /// Отановить таймер
        /// </summary>
        void Stop();
        
        /// <summary>
        /// Сбросить таймер
        /// </summary>
        void Reset();
    }
}
