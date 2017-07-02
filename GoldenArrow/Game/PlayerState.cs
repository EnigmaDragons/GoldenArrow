namespace GoldenArrow.Game
{
    public sealed class PlayerState
    {
        public int Player { get; }

        public int Wood { get; set; }
        public int Food { get; set; }
        public int Gold { get; set; }
        public int Population { get; set; }
        public int Stone { get; set; }
        public int Happiness { get; set; }
        public int PointsFromBuildings { get; set; }

        public int TotalVictoryPoints => Population + Happiness + PointsFromBuildings;

        public PlayerState(int player)
        {
            Player = player;
        }
    }
}
