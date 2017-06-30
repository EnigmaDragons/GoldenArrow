
namespace MonoGame.Cards.Hands
{
    public class Hand
    {
        private readonly HandData _data;

        public Hand(HandData data)
        {
            _data = data;
        }

        public void Add(string card)
        {
            _data.Cards.Add(card);
        }

        public void Remove(string card)
        {
            _data.Cards.Remove(card);
        }
    }
}
