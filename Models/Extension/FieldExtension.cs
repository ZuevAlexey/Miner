using System.Linq;

namespace Models.Extension {
    public static class FieldExtension {
        public static byte GetMinesAroundCount(this Field field, Cell cell) {
            return (byte) field.GetNeighbors(cell).Count(e => e.IsMineHere);
        }
    }
}