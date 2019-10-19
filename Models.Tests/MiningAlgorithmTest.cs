using System;
using System.Collections;
using System.Linq;
using NUnit.Framework;

namespace Models.Test {
    [TestFixture]
    public class MiningAlgorithmTest {
        private static IEnumerable PlaySettings {
            get {
                var miningAlgo = new SimpleMiningAlgorithm();

                for (byte row = 5; row < 10; row++) {
                    for (byte col = 5; col < 10; col++) {
                        for (var mineCount = 0; mineCount <= row * col; mineCount++) {
                            yield return new TestCaseData(new PlaySettings {
                                Columns = col,
                                Rows = row,
                                MinesCount = mineCount
                            }, miningAlgo).SetName($"row = {row}; col = {col}; mineCount = {mineCount}");
                        }
                    }
                }
            }
        }

        [TestCaseSource(nameof(PlaySettings))]
        public void CheckMineCount(PlaySettings settings, IMiningAlgorithm algo) {
            var field = new Field(settings.Rows, settings.Columns);
            Assert.That(field.AllCells.Count(e => e.IsMineHere) == 0);

            algo.DropMines(field, settings.MinesCount);

            Assert.That(field.AllCells.Count(e => e.IsMineHere) == settings.MinesCount);
        }

        private static IEnumerable BadSettings {
            get {
                var miningAlgo = new SimpleMiningAlgorithm();

                yield return new TestCaseData(new PlaySettings {
                    Columns = 5,
                    Rows = 5,
                    MinesCount = 26
                }, miningAlgo).SetName("minesCount > field.Size");

                yield return new TestCaseData(new PlaySettings {
                    Columns = 0,
                    Rows = 5,
                    MinesCount = 0
                }, miningAlgo).SetName("Columns = 0");
                
                yield return new TestCaseData(new PlaySettings {
                    Columns = 5,
                    Rows = 0,
                    MinesCount = 0
                }, miningAlgo).SetName("Rows = 0");
                
                yield return new TestCaseData(new PlaySettings {
                    Columns = 0,
                    Rows = 0,
                    MinesCount = 0
                }, miningAlgo).SetName("Colums = Rows = 0");
            }
        }

        [TestCaseSource(nameof(BadSettings))]
        public void CheckBadSettings(PlaySettings settings, IMiningAlgorithm algo) {
            var field = new Field(settings.Rows, settings.Columns);

            Assert.Throws<ArgumentException>(() => algo.DropMines(field, settings.MinesCount));
        }
    }
}