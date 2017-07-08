using System;
using System.Collections.Generic;
using System.Linq;
using GoldenArrow.Game;
using Microsoft.Xna.Framework;
using MonoDragons.Core.Common;
using MonoDragons.Core.Engine;
using MonoDragons.Core.Entities;
using MonoDragons.Core.MouseControls;
using MonoDragons.Core.PhysicsEngine;
using MonoDragons.Core.Render;
using MonoDragons.Core.Scenes;
using MonoGame.Cards.Cards;
using MonoGame.Cards.Decks;
using MonoGame.Cards.Hands;
using MonoGame.Cards;

namespace GoldenArrow.Scenes
{
    public class InGame : EcsScene
    {
        protected override IEnumerable<GameObject> CreateObjs()
        {
            List<Card> cards = new List<Card>();
            cards.AddRange(Enumerable.Range(0, 9).Select(x => new Card(new CardData { Name = "Stone", Front = "Cards/stone", Back = "Cards/back-basic" })));
            cards.AddRange(Enumerable.Range(0, 9).Select(x => new Card(new CardData { Name = "Stone", Front = "Cards/gold", Back = "Cards/back-basic" })));
            cards.AddRange(Enumerable.Range(0, 9).Select(x => new Card(new CardData { Name = "Stone", Front = "Cards/food", Back = "Cards/back-basic" })));
            cards.AddRange(Enumerable.Range(0, 9).Select(x => new Card(new CardData { Name = "Stone", Front = "Cards/wood", Back = "Cards/back-basic" })));
            cards.Shuffle();
            var deck = new Deck(UIFactory.CreateCard, cards);
            return CreateTable()
                .Concat(CreatePlayerResourceBar(new Vector2(100, 100), new PlayerState(1)))
                .Concat(CreatePlayerResourceBar(new Vector2(100, 200), new PlayerState(2)))
                .Concat(CreatePlayerResourceBar(new Vector2(100, 300), new PlayerState(3)))
                .Concat(CreatePlayerResourceBar(new Vector2(100, 400), new PlayerState(4)))
                .Concat(CreateHand(new Vector2(800, 700), deck, "bob"))
                .Concat(CreateHand(new Vector2(200, 450), deck, "not bob"))
                .Concat(CreateHand(new Vector2(1400, 450), deck, "not bob"))
                .Concat(CreateHand(new Vector2(800, 100), deck, "not bob"));
        }

        private static List<GameObject> CreateHand(Vector2 position, Deck deck, string player)
        {
            return Entity.Create(new Transform2(position, 1))
                .Add(new Hand(new HandData {Cards = Enumerable.Range(0, 9).Select(x => deck.Draw().Id).ToList(), Player = player }))
                .Add(new FanOut())
                .AsList();
        }

        private static List<GameObject> CreateCard(Card card, Vector2 location)
        {
            return Entity.Create(new Transform2(location, Sizes.Card))
                .Add(card)
                .Add(card.Sprite)
                .Add(new MouseDrag())
                .Add(x => new MouseStateActions { OnReleased = () => card.Flip() }).AsList();
        }

        private static List<GameObject> CreateTable()
        {
            return Entity.Create(new Transform2 { Size = new Size2(1920, 1080), ZIndex = Layers.TableZIndex })
                .Add(new Sprite("Images/Table/table-wood")).AsList();
        }

        private static List<GameObject> CreatePlayerResourceBar(Vector2 position, PlayerState state)
        {
            var x = position.X;
            var y = position.Y;

            var resourceData = new Map<string, Func<string>>
            {
                ["wood"] = () => state.Wood.ToString(),
                ["food"] = () => state.Food.ToString(),
                ["gold"] = () => state.Gold.ToString(),
                ["stone"] = () => state.Stone.ToString(),
                ["population"] = () => state.Population.ToString(),
                ["happiness"] = () => state.Happiness.ToString()
            };

            var resourceIcons = new List<GameObject>();
            foreach (KeyValuePair<string, Func<string>> entry in resourceData)
            {
                resourceIcons.Add(CreateResourceIcon(new Vector2(x, y), entry.Key, entry.Value));
                x += IconSize + 5;
            }
            return resourceIcons;
        }

        private static GameObject CreateResourceIcon(Vector2 position, string iconName, Func<string> value)
        {
            return Entity
                .Create(new Transform2 { Location = position, Size = new Size2(IconSize, IconSize), ZIndex = Layers.PlayerResourcesZIndex })
                .Add(new Sprite("Images/Icons/", iconName))
                .Add(new TextDisplay { Text = value });
        }

        private const int IconSize = 40;
    }
}
