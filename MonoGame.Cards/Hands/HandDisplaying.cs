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
            entities.Collect<Hand, FanOut>().ForEach(x => FanCards(entities, x.Get<Transform2>(), x.Get<Hand>(), x.Get<FanOut>()));
        }

        private void FanCards(IEntities entities, Transform2 transform, Hand hand, FanOut fanout)
        {
            if (hand.Count() == fanout.PreviousCardCount)
                return;
            fanout.PreviousCardCount = hand.Count();
            if (hand.Count() == 0)
                return;
            ResizeHand(transform, hand, fanout);
            RepositionCards(entities, hand, CalculateCardPositions(GetCardSize(entities), transform, hand.Count()));
            FaceCardsCorrectly(entities, hand);
        }

        private void ResizeHand(Transform2 transform, Hand hand, FanOut fanout)
        {
            var center = transform.Center;
            transform.Size = new Size2(fanout.WidthPerCard(hand.Count()), fanout.HeightPerCard(hand.Count()));
            transform.Center = center;
        }

        private static void RepositionCards(IEntities entities, Hand hand, List<Vector2> positions)
        {
            hand.Cards().ForEachIndex((cardId, i) => entities.With<Transform2>(cardId, (obj, CardTransform) =>
            {
                CardTransform.Center = positions[i];
                CardTransform.ZIndex = i;
            }));
        }

        private Size2 GetCardSize(IEntities entities)
        {
            return entities.Collect<Card>().First().Transform.Size;
        }

        private List<Vector2> CalculateCardPositions(Size2 cardSize, Transform2 transform, int cardCount)
        {
            return CalculateCardPositions(transform, 
                CalculateXPoints(cardSize, transform, cardCount),
                CalculateYPoints(cardSize, transform, cardCount));
        }

        private List<Vector2> CalculateCardPositions(Transform2 transform, List<int> xPoints, List<int> yPoints)
        {
            var cardPositions = new List<Vector2>();
            xPoints.ForEachIndex((point, i) => cardPositions.Add(new Vector2(point + transform.Location.X, yPoints[i] + transform.Location.Y)));
            return cardPositions;
        }

        private List<int> CalculateXPoints(Size2 cardSize, Transform2 transform, int cardCount)
        {
            return cardCount == 1 
                ? new List<int> { transform.Size.Width / 2 }
                : Enumerable.Range(0, cardCount)
                    .Select(i => (cardSize.Width / 2) + ((transform.Size.Width - cardSize.Width) / (cardCount - 1) * i)).ToList();
        }

        private List<int> CalculateYPoints(Size2 cardSize, Transform2 transform, int cardCount)
        {
            return OrderYPoints(cardCount <= 2
                ? new List<int> { transform.Size.Height / 2 } 
                : Enumerable.Range(0, (int)Math.Ceiling((double)cardCount / 2))
                    .Select(i => (cardSize.Height / 2) + ((transform.Size.Height - cardSize.Height) / (cardCount - 1) * i)).ToList(), 
                cardCount);
        }

        private List<int> OrderYPoints(List<int> yPoints, int cardCount)
        {
            var orderedYPoints = new List<int>(yPoints.ToList());
            orderedYPoints.Reverse();
            return orderedYPoints.Take(cardCount - yPoints.Count).Concat(yPoints).ToList();
        }

        private void FaceCardsCorrectly(IEntities entities, Hand hand)
        {
            hand.Cards().ForEachIndex((cardId, i) => entities.With<Card>(cardId, (obj, card) =>
            {
                //TODO make this not concrete
                if ((hand.Player().Equals("bob") && !card.FaceUp) 
                    || (!hand.Player().Equals("bob") && card.FaceUp))
                    card.Flip();
            }));
        }
    }
}
