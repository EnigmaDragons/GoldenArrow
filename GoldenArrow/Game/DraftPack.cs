using MonoDragons.Core.Common;
using MonoDragons.Core.Entities;
using MonoDragons.Core.MouseControls;

namespace GoldenArrow.Game
{
    public class DraftPack
    {
        private readonly Items _cards;

        public bool IsCardHovered { get; set; }
        public GameObject HoveredCard { get; set; }
        public bool IsCardSelected { get; set; }
        public GameObject SelectedCard { get; set; }

        public DraftPack(Items cards)
        {
            _cards = cards;
            _cards.ForEach(x => x.Add(new MouseStateActions
            {
                OnHover = () => HoverCard(x),
                OnPressed = () => SelectCard(x),
                OnExit = () => ExitCard(x)
            }));
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
