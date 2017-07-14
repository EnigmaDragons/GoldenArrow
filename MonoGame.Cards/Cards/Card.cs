using MonoDragons.Core.PhysicsEngine;

namespace MonoGame.Cards.Cards
{
    public sealed class Card
    {
        private readonly CardData _data;
        private bool _faceup;

        public bool FaceUp
        {
            get { return _faceup; }
            set
            {
                _faceup = value;
                UpdateSprite();
            }
        }

        public Sprite Sprite { get; }

        public Card(CardData data)
        {
            _data = data;
            Sprite = new Sprite("", data.Back);
        }

        public void Flip()
        {
            FaceUp = !FaceUp;
            UpdateSprite();
        }

        private void UpdateSprite()
        {
            Sprite.Name = FaceUp ? _data.Front : _data.Back;
        }
    }
}
