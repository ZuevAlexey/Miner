using System;
using System.Collections;
using System.Linq;
using Moq;
using NUnit.Framework;

namespace Models.Test {
    [TestFixture]
    public class StageFieldFactoryTest {
        private static IEnumerable CorrectFieldCaseSource {
            get {
                var factory = new StageFieldFactory(new SimpleMiningAlgorithm());

                for (byte row = 5; row < 10; row++) {
                    for (byte col = 5; col < 10; col++) {
                        var step = Math.Min(row, col);
                        for (var mineCount = 0; mineCount <= row * col; mineCount += step) {
                            yield return
                                new TestCaseData(new GameSettings(row, col, mineCount, false), factory).SetName(
                                    $"row = {row}; col = {col}; mineCount = {mineCount}");
                        }
                    }
                }
            }
        }

        [TestCaseSource(nameof(CorrectFieldCaseSource))]
        public void CorrectField(GameSettings settings, IFieldFactory factory) {
            var field = factory.Create(settings);

            Assert.That(field.AllCells.Count(e => e.IsMineHere) == settings.MinesCount);
            Assert.That(field.AllCells.All(e => e.MineAroundCount == field.GetNeighbors(e).Count(m => m.IsMineHere)));
        }

        [Test]
        public void UsingMiningAlgorithm() {
            var algo = new Mock<IMiningAlgorithm>();
            var factory = new StageFieldFactory(algo.Object);
            var settings = new GameSettings(5, 2, 2, false);

            var field = factory.Create(settings);

            algo.Verify(e => e.DropMines(It.Is<Field>(f => f == field), It.Is<int>(m => m == settings.MinesCount)),
                Times.Once);
        }
    }
}