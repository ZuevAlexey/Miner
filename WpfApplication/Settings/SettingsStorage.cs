using System;
using System.Collections.Generic;
using System.Configuration;
using Models;

namespace WpfApplication.Settings {
    public class SettingsStorage {
        private static readonly Dictionary<Complexity, string> _complexityConfigKeys =
            new Dictionary<Complexity, string> {
                [Complexity.Easy] = "easySettings",
                [Complexity.Medium] = "mediumSettings",
                [Complexity.Hard] = "hardSettings"
            };

        private readonly Dictionary<Complexity, GameSettings> _gameSettings;

        private Complexity _complexity = Complexity.Easy;

        public Action OnComplexityChanged;

        public Complexity Complexity {
            set {
                if(_complexity == value) {
                    return;
                }

                _complexity = value;
                OnComplexityChanged?.Invoke();
            }
        }

        public GameSettings Current =>
            new GameSettings(_gameSettings[_complexity].Rows, _gameSettings[_complexity].Columns,
                _gameSettings[_complexity].MinesCount, _gameSettings[_complexity].CanOpenMineFirstTry);

        private SettingsStorage(Dictionary<Complexity, GameSettings> gameSettings) {
            _gameSettings = gameSettings;
        }
        
        public static SettingsStorage Load() {
           var gameSettings = new Dictionary<Complexity, GameSettings>();
            foreach(var pair in _complexityConfigKeys) {
                gameSettings[pair.Key] = ParseGameSettings(pair.Value);
            }
            
            return new SettingsStorage(gameSettings);
        }

        private static GameSettings ParseGameSettings(string key) {
            var value = ConfigurationManager.AppSettings[key];
            var arr = value.Split(';');
            return new GameSettings(byte.Parse(arr[0]), byte.Parse(arr[1]), int.Parse(arr[2]), false);
        }
    }
}
