using System;
using System.Collections;
using System.Linq;
using Models.Extension;
using Moq;
using NUnit.Framework;

namespace Models.Test {
    [TestFixture]
    public class GameManagerTest {
        private static IEnumerable PlaySettings {
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

        [TestCaseSource(nameof(PlaySettings))]
        public void CheckField(GameSettings settings, IFieldFactory factory) {
            var field = factory.Create(settings);

            Assert.That(field.AllCells.Count(e => e.IsMineHere) == settings.MinesCount);
            Assert.That(field.AllCells.All(e => e.MineAroundCount == field.GetMinesAroundCount(e)));
        }

        [Test]
        public void GameFinishedException() {
            var factory = new Mock<IFieldFactory>();
            factory.Setup(e => e.Create(It.IsAny<GameSettings>())).Returns(new Field(2, 2));
            var manager = new GameManager(new StageFieldFactory(new SimpleMiningAlgorithm()));
            manager.StartGame(new GameSettings(1, 1, 1, false));
            manager.TryOpen(new Position(0, 0));

            Assert.Throws<InvalidOperationException>(() => manager.TryOpen(new Position(0, 0)));
        }

        [Test]
        public void NotStartedGameException() {
            var manager = new GameManager(new StageFieldFactory(new SimpleMiningAlgorithm()));


            Assert.Throws<InvalidOperationException>(() => manager.TryOpen(new Position(0, 0)));
        }

        [Test]
        public void UsingFieldFactory() {
            var factory = new Mock<IFieldFactory>();
            var manager = new GameManager(factory.Object);
            var settings = new GameSettings(1, 1, 1, false);

            manager.StartGame(settings);

            factory.Verify(e => e.Create(It.Is<GameSettings>(s => s == settings)), Times.Once);
        }
    }
}