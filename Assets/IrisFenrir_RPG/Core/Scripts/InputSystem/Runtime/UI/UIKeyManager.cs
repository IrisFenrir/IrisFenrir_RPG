using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IrisFenrir.InputSystem
{
    public class UIKeyManager : IUpdater, IEnable
    {
        public bool enable { get; private set; }

        private List<UIKey> m_uiKeys;
        private GraphicRaycaster m_raycaster;
        private Dictionary<GameObject, UIKey> m_map;
        private UIKey m_activeKey;

        public UIKeyManager(GraphicRaycaster raycaster)
        {
            m_uiKeys = new List<UIKey>();
            m_map = new Dictionary<GameObject, UIKey>();
            InputSystem.instance.keys.ForEach(key => CreateUIKey(key));

            m_raycaster = raycaster;
            
        }

        private void CreateUIKey(IVirutalKey key)
        {
            for (int i = 0; i < key.keyCount; i++)
            {
                string uiName = key.name + (i == 0 ? string.Empty : i.ToString());
                GameObject go = GameObject.Find($"UI/InputWindow/{uiName}/KeyCode");
                if (go == null) return;
                ITextAdapter text = new UITextMeshPro(go.GetComponent<TextMeshProUGUI>());
                UIKey uiKey = new UIKey(key, i, text);
                uiKey.LoadKey();
                m_uiKeys.Add(uiKey);
                m_map.Add(go, uiKey);
            }
        }

        public void SetEnable(bool enable, bool includeChildren = true)
        {
            this.enable = enable;
            m_activeKey = null;
        }

        public bool GetEnable()
        {
            return enable;
        }

        public void Init() { }

        public void Update(float deltaTime)
        {
            if (!enable) return;

            if(m_activeKey == null)
            {
                if (UIRaycaster.RaycastWithClick(m_raycaster, out var results))
                {
                    if (m_map.TryGetValue(results[0].gameObject, out UIKey key))
                    {
                        key.Clear();
                        m_activeKey = key;
                    }
                }
            }
            else
            {
                if(InputHelper.GetInputKey(out KeyCode keyCode))
                {
                    m_activeKey.SaveKey(keyCode);
                    m_activeKey = null;
                }
            }
        }

        public void Stop() { }


    }
}
