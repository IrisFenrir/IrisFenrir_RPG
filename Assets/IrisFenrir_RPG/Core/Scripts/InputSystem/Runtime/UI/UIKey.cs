using UnityEngine;

namespace IrisFenrir.InputSystem
{
    public class UIKey
    {
        private IVirutalKey m_key;
        private int m_index;
        private int m_subIndex;
        private ITextAdapter m_text;

        public UIKey(IVirutalKey key, int keyIndex, int subKeyIndex, ITextAdapter text)
        {
            m_key = key;
            m_index = keyIndex;
            m_subIndex = subKeyIndex;
            m_text = text;
        }

        public void LoadKey()
        {
            m_text.SetText(m_key.GetKeyCode(m_index, m_subIndex).ToString());
        }

        public void SaveKey(KeyCode keyCode)
        {
            m_text.SetText(keyCode.ToString());
            m_key.SetKeyCode(keyCode, m_index, m_subIndex);
        }

        public void Clear()
        {
            m_text.SetText("None");
        }
    }
}
