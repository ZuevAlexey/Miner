using System;

namespace Models {
    /// <summary>
    ///     Simple mining algorithm based on the Random and retrying if failed
    /// </summary>
    public class SimpleMiningAlgorithm : IMiningAlgorithm {
        private static readonly Random _random = new Random();

        public void DropMines(Field field, int minesCount) {
            if (field.Size <= 0) {
                throw new ArgumentException("The field must not be empty");
            }

            if (minesCount > field.Size) {
                throw new ArgumentException("The number of mines must be less than the size of the field");
            }

            var currentMineCount = 0;
            while (currentMineCount < minesCount) {
                var newMineIndex = _random.Next(0, field.Size);
                if (field[newMineIndex].TryDropMine()) {
                    currentMineCount++;
                }
            }
        }
    }
}