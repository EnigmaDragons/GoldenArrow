
namespace MonoGame.Cards.Cards
{
    public sealed class CardData
    {
        public string Name { get; set; }
        public string Front { get; set; }
        public string Back { get; set; }

        public CardData WithDir(string dir)
        {
            return new CardData {Name = Name, Back = $"{dir}/{Back}", Front = $"{dir}/{Front}"};
        }
    }
}
