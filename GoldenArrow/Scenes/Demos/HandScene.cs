using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoDragons.Core.Entities;
using MonoDragons.Core.MouseControls;
using MonoDragons.Core.PhysicsEngine;
using MonoDragons.Core.Scenes;
using MonoGame.Cards;
using MonoGame.Cards.Cards;
using MonoGame.Cards.Hands;

namespace GoldenArrow.Scenes.Demos
{
    public class HandScene : EcsScene
    {
        private readonly List<GameObject> _gameObjects = new List<GameObject>();

        protected override IEnumerable<GameObject> CreateObjs()
        {
            var obj1 = Create(new Card(new CardData {Back = "Cards/spiral-back", Front = "Decks/Poker/ace-of-diamonds"}));
            var obj2 = Create(new Card(new CardData { Back = "Cards/spiral-back", Front = "Decks/Poker/ace-of-diamonds" }));
            var obj3 = Create(new Card(new CardData { Back = "Cards/spiral-back", Front = "Decks/Poker/ace-of-diamonds" }));
            var obj4 = Create(new Card(new CardData { Back = "Cards/spiral-back", Front = "Decks/Poker/ace-of-diamonds" }));
            var obj5 = Create(new Card(new CardData { Back = "Cards/spiral-back", Front = "Decks/Poker/ace-of-diamonds" }));

            var hand = new Hand(new HandData { Cards = new List<int> { obj1.Id, obj2.Id, obj3.Id, obj4.Id, obj5.Id } });
            var handTransform = new Transform2(new Size2(0, 0));
            handTransform.Center = new Vector2(800, 350);
            GameObject handObj = Entity
                .Create(handTransform)
                .Add(hand)
                .Add(new FanOut());
            _gameObjects.Add(handObj);
            _gameObjects.Add(obj1);
            _gameObjects.Add(obj2);
            _gameObjects.Add(obj3);
            _gameObjects.Add(obj4);
            _gameObjects.Add(obj5);
            yield return obj1;
            yield return obj2;
            yield return obj3;
            yield return obj4;
            yield return obj5;
            yield return handObj;
            yield return UIFactory.CreateButton(new Vector2(550, 750), "Draw", () =>
            {
                var obj = Create(new Card(new CardData { Back = "Cards/spiral-back", Front = "Decks/Poker/ace-of-diamonds" }));
                _gameObjects.Add(obj);
                AddObj(obj);
                hand.Add(obj.Id);
            });
            yield return UIFactory.CreateButton(new Vector2(800, 750), "Discard", () =>
            {
                var card = _gameObjects[1];
                hand.Remove(card.Id);
                _gameObjects.Remove(card);
                Entity.Destroy(card);
            });
        }

        private GameObject Create(Card card)
        {
            return Entity.Create(new Transform2(Vector2.Zero, Sizes.Card))
                .Add(card)
                .Add(card.Sprite)
                .Add(new MouseDrag())
                .Add(x => new MouseStateActions { OnReleased = () => card.Flip() });
        }
    }
}
