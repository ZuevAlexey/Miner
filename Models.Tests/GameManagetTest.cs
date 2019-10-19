using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Models.Extension;
using Moq;
using NUnit.Framework;

namespace Models.Test {
    [TestFixture]
    public class GameManagerTest {
        private static IEnumerable RaiseGameFinishedOnceTestCaseSource {
            get {
                yield return new TestCaseData(GetWinStepsFieldOne(), CreateFieldOne(), true)
                    .SetName("Win game");

                var losingMoves = GetWinStepsFieldOne()
                                  .Take(4)
                                  //add lose step
                                  .Union(new[] {new Position(1, 1)});
                yield return new TestCaseData(losingMoves, CreateFieldOne(), false)
                    .SetName("Lose on last move");
            }
        }

        [TestCaseSource(nameof(RaiseGameFinishedOnceTestCaseSource))]
        public void RaiseGameFinishedOnceAtRightTimeWithRightResult(IEnumerable<Position> moves,
                                                                    Field field, bool gameResult) {
            var factory = CreateFactory(field);
            var manager = new GameManager(factory.Object);
            var gameFinishedCount = 0;
            var lastGameResult = false;
            manager.OnGameFinished += (sender, args) => {
                gameFinishedCount++;
                lastGameResult = args.IsVictory;
            };

            manager.StartGame(CreateUnusedSettings());
            foreach (var move in moves) {
                Assert.That(gameFinishedCount == 0);
                manager.TryOpen(move);
            }

            Assert.That(gameFinishedCount == 1);
            Assert.That(lastGameResult == gameResult);
        }

        [Test]
        public void GameFinishedException() {
            var factory = new Mock<IFieldFactory>();
            factory.Setup(e => e.Create(It.IsAny<GameSettings>()))
                   .Returns(new Field(2, 2));
            var manager = new GameManager(new StageFieldFactory(new SimpleMiningAlgorithm()));
            manager.StartGame(CreateUnusedSettings());
            manager.TryOpen(new Position(0, 0));

            Assert.Throws<InvalidOperationException>(() => manager.TryOpen(new Position(0, 0)));
        }

        [Test]
        public void GameStartedRaisedOnceAtRightTime() {
            var factory = CreateFactory(CreateFieldOne());
            var manager = new GameManager(factory.Object);
            var gameStartedCount = 0;
            manager.OnGameStarted += (sender, args) => { gameStartedCount++; };

            manager.StartGame(CreateUnusedSettings());
            foreach (var position in GetWinStepsFieldOne()) {
                manager.TryOpen(position);
                Assert.That(gameStartedCount == 1);
            }
        }

        [Test]
        public void RaiseNotStartedGameException() {
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
        
        private static IEnumerable ChainOpeningEmptyCellsCaseSource {
            get {
                var firstEmptyCells = new List<Position> {
                    new Position(0, 0),
                    new Position(0, 1),
                    new Position(0, 2),
                    new Position(1, 2),
                    new Position(2, 2)
                };

                var fistChain = firstEmptyCells.Union(new List<Position> {
                    new Position(0, 3),
                    new Position(1, 0),
                    new Position(1, 1),
                    new Position(1, 3),
                    new Position(2, 1),
                    new Position(2, 3),
                    new Position(3, 1),
                    new Position(3, 2),
                    new Position(3, 3),
                }).ToList();

                foreach (var position in firstEmptyCells) {
                    yield return new TestCaseData(CreateFieldTwo(), position, new HashSet<Position>(fistChain))
                        .SetName($"Chain 1 - ({position.Row}, {position.Column})");
                }

                var secondEmptyCells = new List<Position> {
                    new Position(3, 4),
                    new Position(4, 4)
                };

                var secondChain = secondEmptyCells.Union(new List<Position> {
                    new Position(2, 3),
                    new Position(2, 4),
                    new Position(3, 3),
                    new Position(4, 3)
                }).ToList();
            
                foreach (var position in secondEmptyCells) {
                    yield return new TestCaseData(CreateFieldTwo(), position, new HashSet<Position>(secondChain))
                        .SetName($"Chain 2 - ({position.Row}, {position.Column})");
                }
            }
        }
        
        [TestCaseSource(nameof(ChainOpeningEmptyCellsCaseSource))]
        public void ChainOpeningEmptyCells(Field field, Position move, HashSet<Position> expectedOpenedCells) {
            var factory = CreateFactory(field);
            var manager = new GameManager(factory.Object);
            manager.OnCellOpened += (sender, args) => {
                Assert.That(expectedOpenedCells.Contains(args.Position));
                Assert.That(!args.IsMineHere);
                expectedOpenedCells.Remove(args.Position);
            };
            
            manager.StartGame(CreateUnusedSettings());
            manager.TryOpen(move);
            
            Assert.That(expectedOpenedCells.Count == 0);
        }
        
        private static IEnumerable FirstTryOpenMineCaseSource {
            get {
                var minePositions = new[] {
                    new Position(0, 2),
                    new Position(1, 1),
                    new Position(2, 0),
                    new Position(2, 1)
                };
                
                var canOpenMineFirstTrySettings = new GameSettings(3, 3, 4, true);
                var canNotOpenMineFirstTrySettings = new GameSettings(3, 3, 4, false);

                foreach (var minePosition in minePositions) {
                    yield return new TestCaseData(CreateFieldOne(), minePosition, canOpenMineFirstTrySettings, false)
                        .SetName($"Swap mine - ({minePosition.Row}, {minePosition.Column})");

                    yield return new TestCaseData(CreateFieldOne(), minePosition, canNotOpenMineFirstTrySettings, null)
                        .SetName($"Lose - ({minePosition.Row}, {minePosition.Column})");
                }
            }
        }
        
        [TestCaseSource(nameof(FirstTryOpenMineCaseSource))]
        public void FirstTryOpenMine(Field field, Position move, GameSettings settings, bool? expectedGameResult) {
            var factory = CreateFactory(field);
            var manager = new GameManager(factory.Object);

            bool? gameResult = null;
            manager.OnGameFinished += (sender, args) => { gameResult = args.IsVictory; };
            
            manager.StartGame(settings);
            manager.TryOpen(move);
            
            Assert.That(gameResult == expectedGameResult);
        }

        private static GameSettings CreateUnusedSettings() {
            return new GameSettings(1, 1, 1, false);
        }

        private static IEnumerable<Position> GetWinStepsFieldOne() {
            yield return new Position(0, 0);
            yield return new Position(0, 1);
            yield return new Position(1, 0);
            yield return new Position(1, 2);
            yield return new Position(2, 2);
        }

        /// <summary>
        ///     Create test field
        ///     1    2    M
        ///     3    M    3
        ///     M    M    2
        /// </summary>
        /// <returns></returns>
        private static Field CreateFieldOne() {
            var result = new Field(3, 3);
            result[new Position(0, 2)].TryDropMine();
            result[new Position(1, 1)].TryDropMine();
            result[new Position(2, 0)].TryDropMine();
            result[new Position(2, 1)].TryDropMine();

            result.RecalculateMinesAroundCount();
            return result;
        }
        
        /// <summary>
        ///     Create test field
        ///     0    0    0    2    M
        ///     1    1    0    2    M
        ///     M    2    0    1    1
        ///     M    3    1    1    0
        ///     1    2    M    1    0
        /// </summary>
        /// <returns></returns>
        private static Field CreateFieldTwo() {
            var result = new Field(5, 5);
            result[new Position(0, 4)].TryDropMine();
            result[new Position(1, 4)].TryDropMine();
            result[new Position(2, 0)].TryDropMine();
            result[new Position(3, 0)].TryDropMine();
            result[new Position(4, 2)].TryDropMine();

            result.RecalculateMinesAroundCount();
            return result;
        }
        
        private static Mock<IFieldFactory> CreateFactory(Field field) {
            var factory = new Mock<IFieldFactory>();
            factory.Setup(e => e.Create(It.IsAny<GameSettings>())).Returns(field);
            return factory;
        }
    }
}