namespace SoftwareDesignEksamen {

    class Human : Player {

        private readonly ReadInput _input = new();

        public Human(int playerId) : base(playerId) {}

        public override int GetSwapHandIndex() {
            return _input.AskForNumber(1, 4) - 1;
        }

        public override int GetSwapTableIndex() {
            return _input.AskForNumber(1, 4) - 1;
        }

        public override int GetPlayOption(int numOptions) {
            return _input.AskForNumber(1, numOptions);
        }

        public override bool AskPlayDuplicateCard() {
            PrintMessages.Instance.PrintPlayDuplicateCardQuestion();
            return _input.AskForChar("", "yn") == 'y';
        }
    }
}
