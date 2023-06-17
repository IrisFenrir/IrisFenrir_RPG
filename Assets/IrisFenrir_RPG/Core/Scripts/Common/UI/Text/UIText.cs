using UnityEngine.UI;

namespace IrisFenrir
{
    public class UIText : ITextAdapter
    {
        public Text text;

        public UIText(Text text)
        {
            this.text = text;
        }

        public override string GetText()
        {
            return text.text;
        }

        public override void SetText(string content)
        {
            text.text = content;
        }
    }
}
