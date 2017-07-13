using System;
using Microsoft.Xna.Framework;

namespace MonoDragons.Core.PhysicsEngine
{
    public sealed class DurationTravel
    {
        private Vector2 _targetLocation;
        private TimeSpan _remainingDuration;

        public Vector2 TargetLocation
        {
            get { return _targetLocation; }
            set
            {
                _targetLocation = value;
                _remainingDuration = Duration;
            }
        }

        public TimeSpan Duration { get; set; } = TimeSpan.FromMilliseconds(250);

        public Vector2 GetNewPosition(Vector2 current, TimeSpan delta)
        {
            if (_remainingDuration.TotalMilliseconds <= 0)
                return current;

            var tripPercent = Convert.ToSingle(delta.TotalMilliseconds / _remainingDuration.TotalMilliseconds);
            var newLocation = Vector2.Lerp(current, TargetLocation, tripPercent);
            _remainingDuration -= delta;
            return newLocation;
        }
    }
}
