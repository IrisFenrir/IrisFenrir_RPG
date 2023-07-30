using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IrisFenrir.InputSystem
{
    public class UIKeyManager : IUpdater, IEnable
    {
        public bool Enable { get; private set; }

        private List<UIKey> m_uiKeys;
        private GraphicRaycaster m_raycaster;
        private Dictionary<GameObject, UIKey> m_map;
        private UIKey m_activeKey;

        public UIKeyManager(GraphicRaycaster raycaster, string inputWindowName)
        {
            m_uiKeys = new List<UIKey>();
            m_map = new Dictionary<GameObject, UIKey>();

            m_raycaster = raycaster;
            CreateUIKeys(inputWindowName);
        }

        private void CreateUIKeys(string windowName)
        {
            GameObject windowGO = GameObject.Find(windowName);
            if (windowGO == null) return;
            for (int i = 0; i < windowGO.transform.childCount; i++)
            {
                string uiName = windowGO.transform.GetChild(i).name;
                Match match = Regex.Match(uiName, @"^([A-Za-z]+)(\d+)?(?:_(\d+))?$");
                if (match.Success)
                {
                    string keyName = match.Groups[1].Value;
                    int keyIndex = match.Groups[2].Success ? int.Parse(match.Groups[2].Value) : 0;
                    int subKeyIndex = match.Groups[3].Success ? int.Parse(match.Groups[3].Value) : 0;
                    IVirutalKey key = InputSystem.FindKey(keyName);
                    if (key == null) continue;

                    GameObject uiText = GameObject.Find($"{windowName}/{uiName}/KeyCode");
                    if (uiText == null) continue;
                    UIKey uiKey = new UIKey(key, keyIndex, subKeyIndex, new UITextMeshPro(uiText.GetComponent<TextMeshProUGUI>()));
                    m_map.Add(uiText, uiKey);
                    m_uiKeys.Add(uiKey);

                    uiKey.LoadKey();
                }
            }
        }


        public void SetEnable(bool enable, bool includeChildren = true)
        {
            Enable = enable;
            m_activeKey = null;
        }

        public bool GetEnable()
        {
            return Enable;
        }

        public void Init() { }

        public void Update(float deltaTime)
        {
            if (!Enable) return;

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
