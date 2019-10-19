using System;
using System.Collections;
using System.Linq;
using NUnit.Framework;

namespace Models.Test {
    [TestFixture]
    public class MiningAlgorithmTest {
        private static IEnumerable CorrectMineCountCaseSource {
            get {
                var miningAlgo = new SimpleMiningAlgorithm();

                for (byte row = 5; row < 10; row++) {
                    for (byte col = 5; col < 10; col++) {
                        var step = Math.Min(row, col);
                        for (var mineCount = 0; mineCount <= row * col; mineCount += step) {
                            yield return new TestCaseData(new GameSettings(row, col, mineCount, false), miningAlgo)
                                .SetName($"row = {row}; col = {col}; mineCount = {mineCount}");
                        }
                    }
                }
            }
        }

        [TestCaseSource(nameof(CorrectMineCountCaseSource))]
        public void CorrectMineCount(GameSettings settings, IMiningAlgorithm algo) {
            var field = new Field(settings.Rows, settings.Columns);
            algo.DropMines(field, settings.MinesCount);

            Assert.That(field.AllCells.Count(e => e.IsMineHere) == settings.MinesCount);
        }

        private static IEnumerable BadSettingsExceptionCaseSource {
            get {
                var miningAlgo = new SimpleMiningAlgorithm();

                yield return new TestCaseData(new GameSettings(5, 5, 26, false), miningAlgo).SetName(
                    "minesCount > field.Size");

                yield return new TestCaseData(new GameSettings(0, 5, 0, false), miningAlgo).SetName("Columns = 0");

                yield return new TestCaseData(new GameSettings(5, 0, 0, false), miningAlgo).SetName("Rows = 0");

                yield return
                    new TestCaseData(new GameSettings(0, 0, 0, false), miningAlgo).SetName("Colums = Rows = 0");
            }
        }

        [TestCaseSource(nameof(BadSettingsExceptionCaseSource))]
        public void BadSettingsException(GameSettings settings, IMiningAlgorithm algo) {
            var field = new Field(settings.Rows, settings.Columns);

            Assert.Throws<ArgumentException>(() => algo.DropMines(field, settings.MinesCount));
        }
    }
}