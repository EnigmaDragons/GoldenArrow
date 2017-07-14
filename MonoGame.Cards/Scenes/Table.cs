using System;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoDragons.Core.Common;
using MonoDragons.Core.Entities;
using MonoDragons.Core.MouseControls;
using MonoDragons.Core.PhysicsEngine;
using MonoDragons.Core.Scenes;
using MonoGame.Cards.Cards;
using MonoGame.Cards.Decks;

namespace MonoGame.Cards.Scenes
{
    public sealed class Table : IScene
    {
        public void Init()
        {
            Entity.Create(new Transform2(new Size2(1920, 1080)))
                .Add(new Sprite("Images/Table/casino-felt"));

            var deckData = DeckData.LoadFromDir("Content/Decks/Poker");
            var cards = new Items(deckData.GetAllCards().Select(x => Create(new Card(x))));

            var deck = new Deck(cards);
            Entity.Create(new Transform2(new Vector2(200, 200), Sizes.Card))
                .Add(cards)
                .Add(deck.Sprite)
                .Add(new ZGravity())
                .Add(x => new MouseDropTarget {
                    OnDrop = o => deck.PutFacedownOnTop(o)
                })
                .Add(x => new MouseStateActions
                {
                    OnPressed = () => deck.If(deck.Count > 0, () =>
                    {
                        var drawnCard = deck.Draw();
                        drawnCard.Transform.Location = x.Transform.Location;
                    })
                });
        }

        private GameObject Create(Card card)
        {
            return Entity.Create(new Transform2(Sizes.Card))
                .Add(card)
                .Add(card.Sprite)
                .Add(new ZGravity())
                .Add(new MouseDrag())
                .Add(x => new MouseStateActions
                {
                    OnPressed = () => x.Transform.ZIndex = 100,
                    OnReleased = () => card.Flip()
                });
        }

        public void Update(TimeSpan delta)
        {
        }

        public void Draw()
        {
        }
    }
}
