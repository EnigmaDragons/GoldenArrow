namespace MonoDragons.Core.KeyboardControls
{
    public class TypingInput
    {
        public bool IsActive { get; set; }
        public string Value { get; set; } = "";

        public void Append(string val)
        {
            Value += val;
        }

        public void Backspace()
        {
            if (Value.Length > 0)
                Value = Value.Remove(Value.Length - 1);
        }
    }
}
