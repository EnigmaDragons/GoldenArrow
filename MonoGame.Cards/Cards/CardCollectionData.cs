using System.Collections.Generic;

namespace MonoGame.Cards.Cards
{
    public class CardCollectionData
    {
        private string _filePath = "";

        public List<CardData> Cards { get; set; } = new List<CardData>();

        public void Add(CardData card)
        {
            Cards.Add(card);
        }
    }
}
