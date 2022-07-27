using System;
using System.IO;
using System.Reflection;

namespace SoftwareDesignEksamen
{
    public sealed class Config {

        public int DeckSize { get; private set; }

        public ConsoleColor[] PlayerColors { get; private set; }
        public ConsoleColor HandCardsForegroundColor { get; private set; }
        public ConsoleColor HandCardsBackgroundColor { get; private set; }
        public ConsoleColor FaceupCardsForegroundColor { get; private set; }
        public ConsoleColor FaceupCardsBackgroundColor { get; private set; }
        public ConsoleColor FacedownCardsForegroundColor { get; private set; }
        public ConsoleColor FacedownCardsBackgroundColor { get; private set; }

        public int ComputerBaseDelay { get; private set; }
        public int ComputerSwapDelay { get; private set; }
        public int ComputerActionDelay { get; private set; }

        public bool ShowComputerOptions { get; private set; }

        private static volatile Config _instance;
        private static readonly object _syncRoot = new();

        private Config() {
            PlayerColors = new ConsoleColor[4];
        }

        public static Config Instance {
            get {
                if (_instance == null) {
                    lock (_syncRoot) {
                        if (_instance == null)
                            _instance = new Config();
                    }
                }

                return _instance;
            }
        }

        public void Init() {
            ParseFile("config.txt");
        }

        private void ParseFile(string fileName) {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (!File.Exists(path + "\\" + fileName))
                path = path.Substring(0, path.Length - 17);

            string[] lines = System.IO.File.ReadAllLines(path + "\\" + fileName);

            foreach (string line in lines) {
                string[] words = line.Split(':');
                if (words.Length < 2) {
                    Console.WriteLine($"Config failed splitting the following line: {line}");
                    continue;
                }
                string key = words[0];
                string value = words[1];

                switch (key) {
                    case "deck_size":
                        DeckSize = Int32.Parse(value);
                        break;
                    case "hand_cards_foreground_color":
                        HandCardsForegroundColor = ParseConsoleColor(value);
                        break;
                    case "player_color_1":
                        PlayerColors[0] = ParseConsoleColor(value);
                        break;
                    case "player_color_2":
                        PlayerColors[1] = ParseConsoleColor(value);
                        break;
                    case "player_color_3":
                        PlayerColors[2] = ParseConsoleColor(value);
                        break;
                    case "player_color_4":
                        PlayerColors[3] = ParseConsoleColor(value);
                        break;
                    case "hand_cards_background_color":
                        HandCardsBackgroundColor = ParseConsoleColor(value);
                        break;
                    case "faceup_cards_foreground_color":
                        FaceupCardsForegroundColor = ParseConsoleColor(value);
                        break;
                    case "faceup_cards_background_color":
                        FaceupCardsBackgroundColor = ParseConsoleColor(value);
                        break;
                    case "facedown_cards_foreground_color":
                        FacedownCardsForegroundColor = ParseConsoleColor(value);
                        break;
                    case "facedown_cards_background_color":
                        FacedownCardsBackgroundColor = ParseConsoleColor(value);
                        break;
                    case "computer_base_delay":
                        ComputerBaseDelay = Int32.Parse(value);
                        break;
                    case "computer_swap_delay":
                        ComputerSwapDelay = Int32.Parse(value);
                        break;
                    case "computer_action_delay":
                        ComputerActionDelay = Int32.Parse(value);
                        break;
                    case "show_computer_options":
                        ShowComputerOptions = Boolean.Parse(value);
                        break;
                    default:
                        Console.WriteLine($"Config failed to identify key: {key}");
                        break;
                }
            }
        }

        private static ConsoleColor ParseConsoleColor(string value) {
            ConsoleColor c;
            Enum.TryParse(value, out c);
            return c;
        }
    }
}
