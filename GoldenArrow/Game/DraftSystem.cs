using System;
using System.Linq;
using MonoDragons.Core.Entities;

namespace GoldenArrow.Game
{
    public class DraftSystem : ISystem
    {
        public void Update(IEntities entities, TimeSpan delta)
        {
            var draftPacks = entities.Collect<DraftPack>();
            PlaceCards(draftPacks.First());
        }

        private void PlaceCards(GameObject pack)
        {
            
        }

        private void HoverCards()
        {
            
        }

        private void SelectCards()
        {
            
        }
    }
}
