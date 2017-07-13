using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoDragons.Core.Entities;
using MonoDragons.Core.PhysicsEngine;
using MonoDragons.Core.Scenes;
using MonoGame.Cards.Cards;

namespace GoldenArrow.Scenes.Demos
{
    public sealed class CardAnimation : EcsScene
    {
        protected override IEnumerable<GameObject> CreateObjs()
        {
            yield return UIFactory.CreateCard(new Card(new CardData { Name = "Stone", Front = "Cards/stone", Back = "Cards/back-basic" }))
                .Add(new DurationTravel { Duration = TimeSpan.FromMilliseconds(1000), TargetLocation = new Vector2(500, 500)});
        }
    }
}
