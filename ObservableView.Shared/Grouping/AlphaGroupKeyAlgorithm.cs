
using ObservableView.Extensions;

namespace ObservableView.Grouping
{
    public class AlphaGroupKeyAlgorithm : IGroupKeyAlgorithm
    {
        private readonly bool upperCase;

        public AlphaGroupKeyAlgorithm(bool upperCase = true)
        {
            this.upperCase = upperCase;
        }

        public string GetGroupKey(string inputString)
        {
            char firstChar = inputString[0];
            if (char.IsNumber(firstChar))
            {
                return "#";
            }

            if (upperCase)
            {
                return firstChar.ToString().ToUpper();
            }

            return firstChar.ToString().ToLower();
        }
    }
}
