using System;

namespace Models {
    /// <summary>
    ///     Simple mining algorithm based on the Random and retrying if failed
    /// </summary>
    public class SimpleMiningAlgorithm : IMiningAlgorithm {
        private static readonly Random Random = new Random();

        public void DropMines(Field field, PlaySettings settings) {
            if (settings.MineCount > field.Size) {
                throw new ArgumentException("Count of mines must be a less than field.Size");
            }

            var currentMineCount = 0;
            while (currentMineCount < settings.MineCount) {
                var newMineIndex = Random.Next(0, field.Size);
                if (field[newMineIndex].TryDropMine()) {
                    currentMineCount++;
                }
            }
        }
    }
}