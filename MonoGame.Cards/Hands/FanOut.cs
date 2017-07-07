using System;

namespace MonoGame.Cards.Hands
{
    public class FanOut
    {
        public Func<int, int> WidthPerCard { get; set; } = x =>
        {
            var result = 150;
            for (int i = 2; i < x + 1; i++)
                result = result + 300 / i;
            return result;
        };
        public Func<int, int> HeightPerCard { get; set; } = x =>
        {
            var result = 209;
            for (int i = 0; i < x; i++)
                result = result + i;
            return result;
        };
    }
}
