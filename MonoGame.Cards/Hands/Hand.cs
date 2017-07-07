
using System.Collections.Generic;
using System.Linq;

namespace MonoGame.Cards.Hands
{
    public class Hand
    {
        private readonly HandData _data;

        public Hand(HandData data)
        {
            _data = data;
        }

        public void Add(int card)
        {
            _data.Cards.Add(card);
        }

        public void Remove(int card)
        {
            _data.Cards.Remove(card);
        }

        public int Count()
        {
            return _data.Cards.Count;
        }

        public List<int> Cards()
        {
            return _data.Cards.ToList();
        }
    }
}
