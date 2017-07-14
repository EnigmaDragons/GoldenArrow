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
            yield return UIFactory.CreateCard(new Card(new CardData { Name = "Stone", Front = "Images/Cards/stone", Back = "Images/Cards/back-basic" }))
                .Add(new DurationTravel
                {
                    Duration = TimeSpan.FromMilliseconds(3000),
                    Target = new Transform2(new Vector2(500, 500), new Rotation2(180), new Size2(256, 354), 1.5f)
                });
        }
    }
}
