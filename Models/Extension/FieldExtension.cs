using System.Linq;

namespace Models.Extension {
    public static class FieldExtension {
        public static byte GetMinesAroundCount(this Field field, Cell cell) {
            return (byte) field.GetNeighbors(cell).Count(e => e.IsMineHere);
        }
        
        public static void RecalculateMinesAroundCount(this Field field) {
            foreach (var cell in field.AllCells) {
                cell.MineAroundCount = field.GetMinesAroundCount(cell);
            }
        }
    }
}