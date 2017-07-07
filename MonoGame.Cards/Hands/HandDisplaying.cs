using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoDragons.Core.Common;
using MonoDragons.Core.Entities;
using MonoDragons.Core.PhysicsEngine;
using MonoGame.Cards.Cards;

namespace MonoGame.Cards.Hands
{
    public class HandDisplaying : ISystem
    {
        public void Update(IEntities entities, TimeSpan delta)
        {
            entities.Collect<Hand, FanOut>().ForEach(x => FanCards(entities, x));
        }

        private void FanCards(IEntities entities, GameObject gameObj)
        {
            Hand hand = gameObj.Get<Hand>();
            FanOut fanout = gameObj.Get<FanOut>();

            if (hand.Count() == fanout.PreviousCardAmount)
                return;

            var center = gameObj.Transform.Center;
            int cardCount = hand.Count();
            fanout.PreviousCardAmount = cardCount;
            gameObj.Transform.Size = new Size2(fanout.WidthPerCard(cardCount), fanout.HeightPerCard(cardCount));
            gameObj.Transform.Center = center;

            if (cardCount == 0)
                return;

            List<int> xPoints = null;
            if (cardCount == 1)
            {
                xPoints = new List<int> { gameObj.Transform.Size.Width / 2 };
            }
            else
            {
                int cardWidth = 0;
                entities.With<Card>(hand.Cards().First(), (obj, card) => cardWidth = obj.Transform.Size.Width);
                int widthToDivide = gameObj.Transform.Size.Width - cardWidth;
                int widthBetweenCenters = widthToDivide / (cardCount - 1);
                xPoints = Enumerable.Range(0, cardCount)
                    .Select(i => (cardWidth / 2) + (widthBetweenCenters * i)).ToList();
            }

            List<int> yPoints = null;
            if (cardCount <= 2)
            {
                yPoints = new List<int> { gameObj.Transform.Size.Height / 2 };
            }
            else
            {
                int cardHeight = 0;
                entities.With<Card>(hand.Cards().First(), (obj, card) => cardHeight = obj.Transform.Size.Height);
                int heightToDivide = gameObj.Transform.Size.Height - cardHeight;
                int heightBetweenCenters = heightToDivide / (cardCount - 1);
                yPoints = Enumerable.Range(0, (int)Math.Ceiling((double)cardCount / 2))
                    .Select(i => (cardHeight / 2) + (heightBetweenCenters * i)).ToList();
            }


            List<int> orderedYPoints = new List<int>(yPoints.ToList());
            orderedYPoints.Reverse();
            orderedYPoints = orderedYPoints.Take(cardCount - yPoints.Count).Concat(yPoints).ToList();

            List<Vector2> cardPositions = new List<Vector2>();
            xPoints.ForEachIndex((point, i) => cardPositions.Add(new Vector2(point + gameObj.Transform.Location.X, orderedYPoints[i] + gameObj.Transform.Location.Y)));

            hand.Cards().ForEachIndex((cardId, i) => entities.With<Transform2>(cardId, (obj, CardTransform) =>
            {
                CardTransform.Center = cardPositions[i];
                CardTransform.ZIndex = i;
            }));
        }
    }
}
