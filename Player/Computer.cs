using System;
using System.Threading;

namespace SoftwareDesignEksamen {
    class Computer : Player {

        public Computer(int playerId) : base(playerId) {}

        public override int GetSwapHandIndex() {
            Thread.Sleep(Config.Instance.ComputerBaseDelay);
            return new Random().Next(1, 4);
        }

        public override int GetSwapTableIndex() {
            Thread.Sleep(Config.Instance.ComputerBaseDelay + Config.Instance.ComputerSwapDelay);
            return new Random().Next(1, 4);
        }

        public override int GetPlayOption(int numOptions) {
            Thread.Sleep(Config.Instance.ComputerBaseDelay + Config.Instance.ComputerActionDelay);
            return new Random().Next(1, numOptions + 1);
        }

        public override bool AskPlayDuplicateCard() {
            Thread.Sleep(Config.Instance.ComputerBaseDelay);
            return (new Random().Next(0, 1) == 0);
        }
    }
}
