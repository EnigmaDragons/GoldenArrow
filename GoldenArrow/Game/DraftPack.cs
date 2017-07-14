using System.Collections.Generic;
using MonoDragons.Core.Common;
using MonoDragons.Core.Entities;
using MonoDragons.Core.MouseControls;
using MonoDragons.Core.PhysicsEngine;

namespace GoldenArrow.Game
{
    public class DraftPack
    {
        private readonly List<GameObject> _shadowCards = new List<GameObject>();

        public bool IsCardHovered { get; set; }
        public GameObject HoveredCard { get; set; }
        public bool IsCardSelected { get; set; }
        public GameObject SelectedCard { get; set; }

        public DraftPack(Items cards, Transform2 transform)
        {
            
        }

        private void HoverCard(GameObject card)
        {
            IsCardHovered = true;
            HoveredCard = card;
        }

        private void SelectCard(GameObject card)
        {
            IsCardSelected = SelectedCard != card;
            SelectedCard = card;
        }

        private void ExitCard(GameObject card)
        {
            IsCardHovered = false;
        }
    }
}
