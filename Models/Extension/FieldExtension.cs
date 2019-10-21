using System.Linq;

namespace Models.Extension {
    /// <summary>
    ///     Расширение для класса <see cref="Field" />
    /// </summary>
    public static class FieldExtension {
        /// <summary>
        ///     Посчитать количество мин возле ячейки
        /// </summary>
        /// <param name="field">Поле</param>
        /// <param name="cell">Ячейка</param>
        /// <returns>Количество мин возле ячейки</returns>
        public static byte GetMinesAroundCount(this Field field, Cell cell) {
            return (byte) field.GetNeighbors(cell).Count(e => e.IsMineHere);
        }

        /// <summary>
        ///     Пересчитать для всего поля количество мин возле ячеек
        /// </summary>
        /// <param name="field">Поле</param>
        public static void RecalculateMinesAroundCount(this Field field) {
            foreach(var cell in field.AllCells) {
                cell.MineAroundCount = field.GetMinesAroundCount(cell);
            }
        }
    }
}
