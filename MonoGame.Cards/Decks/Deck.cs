using System;
using MonoDragons.Core.Common;
using MonoDragons.Core.Entities;
using MonoDragons.Core.PhysicsEngine;
using MonoGame.Cards.Cards;

namespace MonoGame.Cards.Decks
{
    public sealed class Deck
    {
        private const string Empty = "Images/Decks/empty-black";
        private readonly Items _cards;

        public int Count => _cards.Count;
        public Sprite Sprite { get; } = new Sprite(Empty);

        public Deck(Items cards)
        {
            _cards = cards;
            _cards.ForEach(x => x.IsEnabled = false);
            UpdateSprite();
        }

        public void Shuffle()
        {
            _cards.Shuffle();
        }

        public GameObject Draw()
        {
            if (Count == 0)
                throw new InvalidOperationException("No cards to draw");

            var card = _cards[0];
            card.IsEnabled = true;
            _cards.RemoveAt(0);
            UpdateSprite();
            return card;
        }

        private void UpdateSprite()
        {
            Sprite.Name = Count > 0 ? _cards[0].Get<Sprite>().Name : Empty;
        }

        public void PutFacedownOnTop(GameObject card)
        {
            card.Get<Card>().FaceUp = false;
            _cards.Insert(0, card);
            card.IsEnabled = false;
            UpdateSprite();
        }
    }
}
