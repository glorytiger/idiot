namespace SoftwareDesignEksamen
{
    class TableSlot {
        public int NameRow { get; private set; }
        public int NameCol { get; private set; }
        public int[] CardRow { get; private set; }
        public int[] CardCol { get; private set; }

        public TableSlot(int nameRow, int nameCol, int[] cardRow, int[] cardCol) {
            NameRow = nameRow;
            NameCol = nameCol;
            CardRow = cardRow;
            CardCol = cardCol;
        }
    }
}
