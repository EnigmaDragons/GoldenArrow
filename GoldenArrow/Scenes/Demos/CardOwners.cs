using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoDragons.Core.Entities;
using MonoDragons.Core.MouseControls;
using MonoDragons.Core.PhysicsEngine;
using MonoDragons.Core.Players;
using MonoDragons.Core.Scenes;
using MonoGame.Cards;
using MonoGame.Cards.Cards;

namespace GoldenArrow.Scenes.Demos
{
    public class CardOwners : EcsScene
    {
        protected override IEnumerable<GameObject> CreateObjs()
        {
            var stone = new Card(new CardData {Name = "Stone", Front = "Cards/stone", Back = "Cards/back-basic"});
            var otherCard = new Card(new CardData {Name = "Stone", Front = "Cards/stone", Back = "Cards/back-basic"});
            yield return CreateCard(stone, Vector2.Zero, "bob", "bob");
            yield return CreateCard(otherCard, new Vector2(50, 50), "bob", "not bob");
        }

        private GameObject CreateCard(Card card, Vector2 location, string currentPlayer, string cardOwner)
        {
            Func<GameObject, bool> isCommonOrOwnedByPlayer = x => !x.Get<Owner>().IsOtherPlayer(currentPlayer);
            return Entity.Create(new Transform2(location, Sizes.Card))
                .Add(card)
                .Add(card.Sprite)
                .Add(new Owner {Id = cardOwner})
                .Add(x => new MouseDrag {IsEnabled = () => isCommonOrOwnedByPlayer(x)})
                .Add(x => new MouseStateActions
                {
                    IsEnabled = () => isCommonOrOwnedByPlayer(x),
                    OnReleased = () => card.Flip()
                });
        }
    }
}
