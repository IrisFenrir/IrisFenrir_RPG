using TMPro;

namespace IrisFenrir
{
    public class UITextMeshPro : ITextAdapter
    {
        public TextMeshProUGUI text;

        public UITextMeshPro(TextMeshProUGUI text)
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
