using IrisFenrir.InputSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace IrisFenrir
{
    public class InputSystemSettingWindow : EditorWindow
    {
        private InputSystemAsset m_asset;

        private GUIStyle m_headerStyle;

        private float m_currentHeight;

        private bool m_keysFoldout;

        private Dictionary<KeyDataUnit, bool> m_keyFoldout;
        private Dictionary<KeyDataUnit, bool> m_axesFoldout;

        private float m_scroll;

        [OnOpenAsset]
        public static bool OpenWindow(int instanceID, int line)
        {
            var asset = EditorUtility.InstanceIDToObject(instanceID) as InputSystemAsset;
            if (asset != null)
            {
                InputSystemSettingWindow.Open(asset);
            }
            return false;
        }


        [MenuItem("IrisFenrir/Input System Window")]
        public static void Open()
        {
            GetWindow<InputSystemSettingWindow>().titleContent = new GUIContent("Input System Setting");
        }

        public static void Open(InputSystemAsset asset)
        {
            var window = GetWindow<InputSystemSettingWindow>();
            window.m_asset = asset;
            window.titleContent = new GUIContent("Input System Setting");
        }

        private void OnEnable()
        {
            m_headerStyle = new GUIStyle();
            m_headerStyle.fontSize = 25;
            m_headerStyle.fontStyle = FontStyle.Bold;
            m_headerStyle.normal.textColor = Color.white * 0.85f;

            m_keyFoldout = new Dictionary<KeyDataUnit, bool>();
            m_axesFoldout = new Dictionary<KeyDataUnit, bool>();
        }

        private void OnDisable()
        {
            EditorUtility.SetDirty(m_asset);
            AssetDatabase.SaveAssets();
        }

        private void OnGUI()
        {
            GetAsset();

            m_currentHeight = 75f;
            m_currentHeight -= m_scroll * (position.height - 75);
            DrawKeysContent();

            m_currentHeight += m_scroll * (position.height - 75);
            m_scroll = GUI.VerticalScrollbar(new Rect(0, 80, 20, position.height - 75), m_scroll, Mathf.Clamp01(position.height / m_currentHeight * 0.7f), 0, 1);

            m_currentHeight = 0f;
            EditorGUI.DrawRect(new Rect(0, 0, position.width, 80), new Color(0.22f, 0.22f, 0.22f));
            DrawHeader();
            DrawKeysHeader();
        }

        private void GetAsset()
        {
            if (m_asset == null)
            {
                m_asset = Selection.activeObject as InputSystemAsset;
            }

            if (m_asset.keys.Count > 0)
            {
                if (m_keyFoldout.Count == 0)
                {
                    foreach (var key in m_asset.keys)
                    {
                        m_keyFoldout.Add(key, false);
                        key.subKeys.ForEach(k => m_keyFoldout.Add(k, false));
                    }
                }

                if (m_axesFoldout.Count == 0)
                    m_asset.keys.ForEach(key => m_axesFoldout.Add(key, false));
            }
        }

        private void DrawHeader()
        {
            float leftMargin = 10;
            float topMargin = 5;
            float height = 40f;

            EditorGUI.LabelField(new Rect(leftMargin, topMargin, 500, height), "Input System Setting", m_headerStyle);
            EditorGUI.DrawRect(new Rect(leftMargin, topMargin + height, position.width - leftMargin * 2, 1), Color.white * 0.85f);

            m_currentHeight += topMargin + height;
        }

        private void DrawKeysHeader()
        {
            float topMargin = 10f;
            float leftMargin = 10f;
            float lineHeight = 20f;

            m_keysFoldout = EditorGUI.Foldout(new Rect(leftMargin, m_currentHeight + topMargin, 50, lineHeight), m_keysFoldout, "Keys");
            EditorGUI.IntField(new Rect(leftMargin + 60, m_currentHeight + topMargin, 50, lineHeight), m_asset.keys.Count);
            if (GUI.Button(new Rect(leftMargin + 60 * 2, m_currentHeight + topMargin, 20, lineHeight), "+"))
            {
                var key = new KeyDataUnit();
                key.AddStringProperty("name", "Custom Key");
                m_asset.keys.Add(key);
                m_keyFoldout.Add(key, false);
                m_axesFoldout.Add(key, false);
            }
            if (GUI.Button(new Rect(leftMargin + 60 * 2 + 30, m_currentHeight + topMargin, 20, lineHeight), "-"))
            {
                var key = m_asset.keys.Last();
                if (key != null)
                {
                    m_asset.keys.Remove(key);
                    m_keyFoldout.Remove(key);
                    m_axesFoldout.Remove(key);
                }
            }

            m_currentHeight += topMargin + lineHeight;
        }

        private void DrawKeysContent()
        {
            if (!m_keysFoldout) return;

            m_asset.keys.ForEach(key => DrawKey(key));
        }

        private void DrawKey(KeyDataUnit key)
        {
            float topMargin = 10f;
            float leftMargin = 30f;
            float lineHeight = 20f;

            Rect nameArea = new Rect(leftMargin, m_currentHeight + topMargin, 100, lineHeight);
            m_keyFoldout[key] = EditorGUI.Foldout(nameArea, m_keyFoldout[key], key.GetStringProperty("name"));

            Rect typeTextArea = new Rect(leftMargin + 110, m_currentHeight + topMargin, 100, lineHeight);
            GUI.color = GetTypeTextColor(key.type);
            EditorGUI.LabelField(typeTextArea, key.type.ToString());
            GUI.color = Color.white;

            m_currentHeight += topMargin + lineHeight;

            if (m_keyFoldout[key])
                DrawKeyProperty(key);

            if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
            {
                if (nameArea.Contains(Event.current.mousePosition))
                {
                    GenericMenu menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Delete"), false, () =>
                    {
                        m_asset.keys.Remove(key);
                        m_keyFoldout.Remove(key);
                    });
                    menu.ShowAsContext();
                }

                if (typeTextArea.Contains(Event.current.mousePosition))
                {
                    GenericMenu menu = new GenericMenu();
                    menu.AddItem(new GUIContent("TapKey"), false, () => SwitchType(key, KeyDataUnit.KeyType.TapKey));
                    menu.AddItem(new GUIContent("ValueKey"), false, () => SwitchType(key, KeyDataUnit.KeyType.ValueKey));
                    menu.AddItem(new GUIContent("PressKey"), false, () => SwitchType(key, KeyDataUnit.KeyType.PressKey));
                    menu.AddItem(new GUIContent("AxisKey"), false, () => SwitchType(key, KeyDataUnit.KeyType.AxisKey));
                    menu.AddItem(new GUIContent("MultiKey"), false, () => SwitchType(key, KeyDataUnit.KeyType.MultiKey));
                    menu.AddItem(new GUIContent("ComboKey"), false, () => SwitchType(key, KeyDataUnit.KeyType.ComboKey));
                    menu.ShowAsContext();
                }
            }
        }

        private Color GetTypeTextColor(KeyDataUnit.KeyType type)
        {
            return type switch
            {
                KeyDataUnit.KeyType.None => Color.white,
                KeyDataUnit.KeyType.TapKey => new Color(1, 0, 0),
                KeyDataUnit.KeyType.ValueKey => new Color(1, 1, 0),
                KeyDataUnit.KeyType.PressKey => new Color(0, 1, 0),
                KeyDataUnit.KeyType.AxisKey => new Color(0, 1, 1),
                KeyDataUnit.KeyType.MultiKey => new Color(0, 0, 1),
                KeyDataUnit.KeyType.ComboKey => new Color(1, 0, 1),
                _ => Color.white
            };
        }

        private void SwitchType(KeyDataUnit key, KeyDataUnit.KeyType type)
        {
            key.type = type;
            key.ClearAll();
            switch (type)
            {
                case KeyDataUnit.KeyType.TapKey:
                    key.AddStringProperty("name", "Custom Key");
                    key.AddIntProperty("clickCount", 1);
                    key.AddFloatProperty("clickInterval", 0.5f);
                    key.AddKeyCodeProperty("keyCode", KeyCode.None);
                    break;
                case KeyDataUnit.KeyType.ValueKey:
                    key.AddStringProperty("name", "Custom Key");
                    key.AddVector2Property("range", new Vector2(0, 1));
                    key.AddFloatProperty("start", 0);
                    key.AddVector2Property("speed", new Vector2(5, 5));
                    key.AddKeyCodeProperty("keyCode", KeyCode.None);
                    break;
                case KeyDataUnit.KeyType.PressKey:
                    key.AddStringProperty("name", "Custom Key");
                    key.AddKeyCodeProperty("keyCode", KeyCode.None);
                    break;
                case KeyDataUnit.KeyType.AxisKey:
                    key.AddStringProperty("name", "Custom Key");
                    break;
                case KeyDataUnit.KeyType.MultiKey:
                    key.AddStringProperty("name", "Custom Key");
                    key.AddFloatProperty("interval", 0.5f);
                    key.AddIntProperty("keyCount", 0);
                    break;
                case KeyDataUnit.KeyType.ComboKey:
                    key.AddStringProperty("name", "Custom Key");
                    key.AddFloatProperty("interval", 0.5f);
                    key.AddIntProperty("keyCount", 0);
                    break;
            }
        }

        private void DrawKeyProperty(KeyDataUnit key)
        {
            switch (key.type)
            {
                case KeyDataUnit.KeyType.TapKey:
                    DrawTextField(key, "Name", "name");
                    DrawIntField(key, "Click Count", "clickCount");
                    DrawFloatField(key, "Click Interval", "clickInterval");
                    DrawKeyCodeFiled(key, "Key Code", "keyCode");
                    break;
                case KeyDataUnit.KeyType.ValueKey:
                    DrawTextField(key, "Name", "name");
                    DrawVector2Filed(key, "Range", "range");
                    DrawFloatField(key, "Start", "start");
                    DrawVector2Filed(key, "Speed", "speed");
                    DrawKeyCodeFiled(key, "Key Code", "keyCode");
                    break;
                case KeyDataUnit.KeyType.PressKey:
                    DrawTextField(key, "Name", "name");
                    DrawKeyCodeFiled(key, "Key Code", "keyCode");
                    break;
                case KeyDataUnit.KeyType.AxisKey:
                    DrawTextField(key, "Name", "name");
                    DrawAxes(key);
                    break;
                case KeyDataUnit.KeyType.MultiKey:
                case KeyDataUnit.KeyType.ComboKey:
                    DrawTextField(key, "Name", "name");
                    DrawFloatField(key, "Interval", "interval");
                    DrawKeyCodes(key);
                    break;
            }
        }

        private void DrawTextField(KeyDataUnit key, string displayName, string propertyName, bool wrap = true, float leftMargin = 50f, float topMargin = 5f, float lineHeight = 20f)
        {
            EditorGUI.LabelField(new Rect(leftMargin, m_currentHeight + topMargin, 100, lineHeight), displayName);
            key.SetStringProperty(propertyName, EditorGUI.TextField(new Rect(leftMargin + 100, m_currentHeight + topMargin, 100, lineHeight), key.GetStringProperty(propertyName)));
            if (wrap)
                m_currentHeight += topMargin + lineHeight;
        }

        private void DrawIntField(KeyDataUnit key, string displayName, string propertyName, bool wrap = true, float leftMargin = 50f, float topMargin = 5f, float lineHeight = 20f)
        {
            EditorGUI.LabelField(new Rect(leftMargin, m_currentHeight + topMargin, 100, lineHeight), displayName);
            key.SetIntProperty(propertyName, EditorGUI.IntField(new Rect(leftMargin + 100, m_currentHeight + topMargin, 100, lineHeight), key.GetIntProperty(propertyName)));
            if (wrap)
                m_currentHeight += topMargin + lineHeight;
        }

        private void DrawFloatField(KeyDataUnit key, string displayName, string propertyName, bool wrap = true, float leftMargin = 50f, float topMargin = 5f, float lineHeight = 20f)
        {
            EditorGUI.LabelField(new Rect(leftMargin, m_currentHeight + topMargin, 100, lineHeight), displayName);
            key.SetFloatProperty(propertyName, EditorGUI.FloatField(new Rect(leftMargin + 100, m_currentHeight + topMargin, 100, lineHeight), key.GetFloatProperty(propertyName)));
            if (wrap)
                m_currentHeight += topMargin + lineHeight;
        }

        private void DrawKeyCodeFiled(KeyDataUnit key, string displayName, string propertyName, bool wrap = true, float leftMargin = 50f, float topMargin = 5f, float lineHeight = 20f)
        {
            EditorGUI.LabelField(new Rect(leftMargin, m_currentHeight + topMargin, 100, lineHeight), displayName);
            key.SetKeyCodeProperty(propertyName, (KeyCode)EditorGUI.EnumPopup(new Rect(leftMargin + 100, m_currentHeight + topMargin, 100, lineHeight), key.GetKeyCodeProperty(propertyName)));
            if (wrap)
                m_currentHeight += topMargin + lineHeight;
        }

        private void DrawVector2Filed(KeyDataUnit key, string displayName, string propertyName, bool wrap = true, float leftMargin = 50f, float topMargin = 5f, float lineHeight = 20f)
        {
            EditorGUI.LabelField(new Rect(leftMargin, m_currentHeight + topMargin, 100, lineHeight), displayName);
            key.SetVector2Property(propertyName, EditorGUI.Vector2Field(new Rect(leftMargin + 100, m_currentHeight + topMargin, 100, lineHeight), string.Empty, key.GetVector2Property(propertyName)));
            if (wrap)
                m_currentHeight += topMargin + lineHeight;
        }

        private void DrawAxes(KeyDataUnit key)
        {
            m_axesFoldout[key] = EditorGUI.Foldout(new Rect(50, m_currentHeight + 5, 100, 20), m_axesFoldout[key], "Axes");
            EditorGUI.IntField(new Rect(150, m_currentHeight + 5, 50, 20), key.subKeys.Count);
            if (GUI.Button(new Rect(205, m_currentHeight + 5, 20, 20), "+"))
            {
                var subKey = new KeyDataUnit();
                subKey.type = KeyDataUnit.KeyType.Axis;
                subKey.AddVector2Property("range", new Vector2(-1, 1));
                subKey.AddVector2Property("posSpeed", new Vector2(5, 5));
                subKey.AddVector2Property("negSpeed", new Vector2(5, 5));
                subKey.AddFloatProperty("start", 0);
                subKey.AddKeyCodeProperty("posKey", KeyCode.None);
                subKey.AddKeyCodeProperty("negKey", KeyCode.None);
                m_keyFoldout.Add(subKey, false);
                key.subKeys.Add(subKey);
            }
            if (GUI.Button(new Rect(230, m_currentHeight + 5, 20, 20), "-"))
            {
                var subKey = key.subKeys.Last();
                if (subKey != null)
                {
                    key.subKeys.Remove(subKey);
                }
            }
            m_currentHeight += 25;

            if (!m_axesFoldout[key]) return;

            for (int i = 0; i < key.subKeys.Count; i++)
            {
                DrawAxis(key, i);
            }
        }

        private void DrawAxis(KeyDataUnit key, int index)
        {
            var subKey = key.subKeys[index];
            Rect area = new Rect(70, m_currentHeight + 5, 100, 20);
            m_keyFoldout[subKey] = EditorGUI.Foldout(area, m_keyFoldout[subKey], $"Axis {index}");
            m_currentHeight += 25;

            if (Event.current.type == EventType.MouseDown && Event.current.button == 1 && area.Contains(Event.current.mousePosition))
            {
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("Delete"), false, () =>
                {
                    key.subKeys.Remove(subKey);
                });
                menu.ShowAsContext();
            }

            if (!m_keyFoldout[subKey]) return;

            float leftMargin = 90f;
            DrawVector2Filed(subKey, "Range", "range", true, leftMargin);
            DrawFloatField(subKey, "Start", "start", true, leftMargin);
            DrawVector2Filed(subKey, "Pos Speed", "posSpeed", false, leftMargin);
            DrawVector2Filed(subKey, "Neg Speed", "negSpeed", true, leftMargin + 220);
            DrawKeyCodeFiled(subKey, "Pos Key", "posKey", false, leftMargin);
            DrawKeyCodeFiled(subKey, "Neg Key", "negKey", true, leftMargin + 220);
        }

        private void DrawKeyCodes(KeyDataUnit key)
        {
            int keyCount;

            m_axesFoldout[key] = EditorGUI.Foldout(new Rect(50, m_currentHeight + 5, 100, 20), m_axesFoldout[key], "Keys");
            EditorGUI.IntField(new Rect(150, m_currentHeight + 5, 50, 20), key.GetIntProperty("keyCount"));
            if (GUI.Button(new Rect(205, m_currentHeight + 5, 20, 20), "+"))
            {
                keyCount = key.GetIntProperty("keyCount");
                key.SetIntProperty("keyCount", keyCount + 1);
                key.AddKeyCodeProperty($"key{keyCount}", KeyCode.None);
            }
            if (GUI.Button(new Rect(230, m_currentHeight + 5, 20, 20), "-"))
            {
                keyCount = key.GetIntProperty("keyCount");
                key.RemoveKeyCodeProperty($"key{keyCount - 1}");
                key.SetIntProperty("keyCount", keyCount - 1);
            }
            m_currentHeight += 25;

            if (!m_axesFoldout[key]) return;
            keyCount = key.GetIntProperty("keyCount");
            for (int i = 0; i < keyCount; i++)
            {
                key.SetKeyCodeProperty($"key{i}", (KeyCode)EditorGUI.EnumPopup(new Rect(70, m_currentHeight + 5, 100, 20), key.GetKeyCodeProperty($"key{i}")));
                m_currentHeight += 25f;
            }
        }
    }
}