using System;
using System.Collections.Generic;

namespace SoftwareDesignEksamen
{
    public sealed class DrawTable {

        public HandFacade hf = new();
		readonly char[] playerStr = { 'P', 'l', 'a', 'y', 'e', 'r', ' ' };
		readonly TableSlot[] slots = {
            new TableSlot(7, 28, new int[] { 6, 6, 6 }, new int[] { 25, 31, 37 }),
            new TableSlot(1, 28, new int[] { 2, 2, 2 }, new int[] { 25, 31, 37 }),
            new TableSlot(4, 1,  new int[] { 3, 4, 5 }, new int[] { 11, 11, 11 }),
            new TableSlot(4, 57, new int[] { 3, 4, 5 }, new int[] { 50, 50, 50 })
        };
		readonly int playedRow = 4;
		readonly int playedCol = 31;

        private readonly char[,] _table = new char[7, 64];

        public DrawTable() {
            for (int row = 0; row < 7; row++) {
                for (int col = 0; col < 64; col++) {
                    _table[row, col] = ' ';
                }
            }
        }

        public void Draw(List<Player> players, List<Card> playedCards) {

            UpdateTable(players, playedCards);
            RenderTable();
        }

        private void UpdateTable(List<Player> players, List<Card> playedCards) {

            // Set player names and cards
            for (int i = 0; i < players.Count; i++) {
                SetPlayerName(i);
                SetPlayerCards(slots[i], players[players[i].PlayerId]);
            }

            // Set played card in the center
            char rankCh = ' ';
            char suitCh = ' ';
            if (playedCards.Count > 0) {
                rankCh = GetRank(playedCards[playedCards.Count - 1]);
                suitCh = GetSuit(playedCards[playedCards.Count - 1]);
            }
            SetCard(playedRow - 1, playedCol - 1, rankCh, suitCh);
        }

        private void SetPlayerCards(TableSlot slot, Player player) {

            for (int i = 0; i < 3; i++) {

                SetCard(slot.CardRow[i] - 1, slot.CardCol[i] - 1, GetPlayerCardRank(player, i), GetPlayerCardSuit(player, i));
            }
        }

        private void SetCard(int row, int col, char rankCh, char suitCh) {
            _table[row, col++] = '[';
            _table[row, col++] = (rankCh == ':') ? '1' : (rankCh == '?') ? ' ' : rankCh;
            _table[row, col++] = (rankCh == ':') ? '0' : (rankCh == '?') ? '?' : ' ';
            _table[row, col++] = suitCh;
            _table[row, col] = ']';
        }

        private static char GetPlayerCardRank(Player player, int slotIndex) {
            List<Card> downCards = player.hf.FaceDownCards.Cards;
            List<Card> upCards = player.hf.FaceUpCards.Cards;
            char c = ' ';

            foreach (Card card in downCards) {
                if (card.DeckSlotId == slotIndex) {
                    c = '?';
                    break;
                }
            }

            foreach (Card card in upCards) {
                if (card.DeckSlotId == slotIndex) {
                    c = GetRank(card);
                    break;
                }
            }

            return c;
        }

        private static char GetRank(Card card) {
			var c    = (int)card.Rank switch
			{
				11 => 'J',
				12 => 'Q',
				13 => 'K',
				14 => 'A',
				_ => (char)(card.Rank + 48),
			};
			return c;
        }

        private static char GetPlayerCardSuit(Player player, int slotIndex) {
            List<Card> upCards = player.hf.FaceUpCards.Cards;
            char c = ' ';

            foreach (Card card in upCards) {
                if (card.DeckSlotId == slotIndex) {
                    c = GetSuit(card);
                    break;
                }
            }

            return c;
        }

        private static char GetSuit(Card card) {
			var c = card.Suit switch
			{
				Suit.Hearts => '♥',
				Suit.Clubs => '♣',
				Suit.Diamonds => '♦',
				Suit.Spades => '♠',
				_ => ' ',
			};
			return c;
        }

        private void SetPlayerName(int slotIndex) {
            int row = slots[slotIndex].NameRow - 1;
            int col = slots[slotIndex].NameCol - 1;

            foreach (char c in playerStr)
                _table[row, col++] = c;
            _table[row, col] = ((char)((slotIndex + 1) + '0'));
        }

        private void RenderTable() {
            Console.WriteLine("\n                       Cards at the table\n");

            for (int row = 0; row < _table.GetLength(0); row++) {
                for (int col = 0; col < _table.GetLength(1); col++) {
                    ActivateColor(row, col);
                    Console.Write(_table[row, col]);
                    DeactivateColor(row, col);
                }
                Console.WriteLine("");
            }

            Console.WriteLine();
        }

        private static void ActivateColor(int row, int col) {

            if (row == 6 && col == 27)
                Console.ForegroundColor = Config.Instance.PlayerColors[0];
            else if (row == 0 && col == 27)
                Console.ForegroundColor = Config.Instance.PlayerColors[1];
            else if (row == 3 && col == 0)
                Console.ForegroundColor = Config.Instance.PlayerColors[2];
            else if (row == 3 && col == 56)
                Console.ForegroundColor = Config.Instance.PlayerColors[3];
        }

        private static void DeactivateColor(int row, int col) {

            if (row == 6 && col == 34 || row == 0 && col == 34 || row == 3 && col == 7 || row == 3 && col == 63)
                Console.ResetColor();
        }
    }
}
