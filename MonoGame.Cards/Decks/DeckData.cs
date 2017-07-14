using System.Collections.Generic;
using System.Linq;
using MonoDragons.Core.Common;
using MonoDragons.Core.IO;
using MonoGame.Cards.Cards;

namespace MonoGame.Cards.Decks
{
    public sealed class DeckData
    {
        private const string FileName = "deck.json";

        private Optional<string> _dataDir = new Optional<string>();

        public string DeckCardBack { get; set; } = "spiral-back";
        public List<CardQty> Cards { get; set; } = new List<CardQty>();

        public static DeckData LoadFromDir(string dir)
        {
            var deck = new JsonIo().Load<DeckData>($"{dir}/{FileName}");
            deck._dataDir = dir.Replace("Content/", "");
            return deck;
        }

        public void SaveToDir(string dir)
        {
            new JsonIo().Save(dir + FileName, this);
        }

        public void Add(CardData card, int num)
        {
            if (string.IsNullOrWhiteSpace(card.Back))
                card.Back = DeckCardBack;
            Cards.Add(new CardQty { Count = num, Name = card.Name });
        }

        public IEnumerable<CardData> GetAllCards()
        {
            var dir = _dataDir.HasValue ? _dataDir.Value : "";
            var cards = new List<CardData>();
            Cards.ForEach(x => Enumerable.Range(0, x.Count)
                .ForEach(i => cards.Add(
                    new CardData
                    {
                        Name = x.Name,
                        Front = $"{dir}/{x.Name.Replace(" ", "-").ToLowerInvariant()}",
                        Back = $"{dir}/{DeckCardBack}",
                    })));
            return cards;
        }
    }
}
