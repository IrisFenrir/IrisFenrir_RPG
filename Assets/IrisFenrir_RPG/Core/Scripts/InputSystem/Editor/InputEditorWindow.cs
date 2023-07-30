using IrisFenrir;
using IrisFenrir.InputSystem;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class InputEditorWindow : EditorWindow
{
    public class KeySetting
    {
        public Color titleColor;
    }

    [OnOpenAsset]
    public static bool OpenEditorWindow(int instanceID, int lineNumber)
    {
        InputAsset asset = EditorUtility.InstanceIDToObject(instanceID) as InputAsset;
        OpenWindow(asset);
        return false;
    }

    public static void OpenWindow(InputAsset asset)
    {
        var window = GetWindow<InputEditorWindow>();
        window.inputAsset = asset;
    }

    public InputAsset inputAsset;

    private Color m_textColor;
    private Color m_keyHeaderColor;
    private Color m_keyBackgroundColor;
    private Color m_axisHeaderColor;
    private GUIStyle m_headerStyle;
    private GUIStyle m_titleStyle;
    private GUIStyle m_buttonTextStyle;
    private GUIStyle m_keyNameStyle;

    private float m_currentHeight;
    private Dictionary<string, KeySetting> m_settings;


    private void OnEnable()
    {
        m_textColor = Color.white * (210f / 255f);
        m_textColor.a = 1f;

        m_keyHeaderColor = new Color(30f / 255f, 30f / 255f, 30f / 255f);
        m_keyBackgroundColor = new Color(80f / 255f, 80f / 255f, 80f / 255f);

        m_axisHeaderColor = new Color(50f / 255f, 50f / 255f, 50f / 255f);

        m_headerStyle = new GUIStyle();
        m_headerStyle.normal.textColor = m_textColor;
        m_headerStyle.fontSize = 20;
        m_headerStyle.fontStyle = FontStyle.Bold;

        m_titleStyle = new GUIStyle();
        m_titleStyle.normal.textColor = m_textColor;
        m_titleStyle.fontStyle = FontStyle.Bold;
        m_titleStyle.alignment = TextAnchor.MiddleLeft;

        m_buttonTextStyle = new GUIStyle();
        m_buttonTextStyle.normal.textColor = m_textColor;
        m_buttonTextStyle.fontSize = 20;
        m_buttonTextStyle.fontStyle = FontStyle.Bold;
        m_buttonTextStyle.alignment = TextAnchor.MiddleCenter;

        m_keyNameStyle = new GUIStyle();
        m_keyNameStyle.normal.textColor = m_textColor;
        m_keyNameStyle.fontStyle = FontStyle.Bold;
        m_keyNameStyle.alignment = TextAnchor.MiddleLeft;

        m_settings ??= new Dictionary<string, KeySetting>
        {
            { "Unknown", new KeySetting(){titleColor = Color.gray} },
            { "TapKey", new KeySetting() { titleColor = new Color(239f/255f, 148f/255f, 158f/255f) } },
            { "ValueKey", new KeySetting() {titleColor = new Color(242f/255f, 186f/255f, 2f/255f)} },
            { "PressKey", new KeySetting() {titleColor = new Color(117f/255f, 189f/255f, 66f/255f)} },
            { "AxisKey", new KeySetting() {titleColor = new Color(0,1,1)} },
            { "MultiKey", new KeySetting() {titleColor = new Color(34f/255f,167f/255f,242f/255f) } },
            { "ComboKey", new KeySetting() {titleColor = new Color(121f/155f, 52f/255f, 174f/255f)} }
        };
    }

    private void OnGUI()
    {
        CheckSelection();
        Draw();
    }

    private void CheckSelection()
    {
        InputAsset asset = Selection.activeObject as InputAsset;
        if (asset != null && asset != inputAsset)
            inputAsset = asset;
    }

    private void Draw()
    {
        DrawHeader();
        DrawKeys();
    }

    private void DrawHeader()
    {
        EditorGUI.LabelField(new Rect(10, 5, 300, 20), "Input System", m_headerStyle);
        EditorGUI.DrawRect(new Rect(10, 35, position.width - 20, 2), m_textColor);
        EditorGUI.LabelField(new Rect(10, 40, 100, 25), "Keys", m_titleStyle);
        GUI.enabled = false;
        int count = inputAsset == null ? 0 : inputAsset.keys.FindAll(k => k.GetProperty<bool>("canDraw")).Count;
        EditorGUI.IntField(new Rect(60, 43, 50, 20), inputAsset != null ? count : 0);
        GUI.enabled = true;
        bool addKey = GUI.Button(new Rect(120, 43, 20, 20), string.Empty);
        EditorGUI.LabelField(new Rect(121, 42, 20, 20), "+", m_buttonTextStyle);
        bool removeKey = GUI.Button(new Rect(145, 43, 20, 20), string.Empty);
        EditorGUI.LabelField(new Rect(146, 42, 20, 20), "-", m_buttonTextStyle);
        bool save = GUI.Button(new Rect(170, 43, 50, 20), "Save");

        if (inputAsset == null) return;
        if(addKey)
        {
            inputAsset.AddUnknownKey();
        }

        if(removeKey)
        {
            inputAsset.RemoveAt(inputAsset.keys.Count - 1);
        }

        if(save)
        {
            EditorUtility.SetDirty(inputAsset);
            AssetDatabase.SaveAssetIfDirty(inputAsset);
        }
    }

    private Vector2 m_scrollPosition;
    private void DrawKeys()
    {
        if (inputAsset == null) return;

        m_scrollPosition = GUI.BeginScrollView(new Rect(0, 70, position.width, position.height - 70), m_scrollPosition, new Rect(0, 0, position.width - 20, m_currentHeight), false, m_currentHeight > position.height - 70);

        m_currentHeight = 0;
        for (int i = 0; i < inputAsset.keys.Count; i++)
        {
            DrawKey(i);
        }

        GUI.EndScrollView();
    }

    private void DrawKey(int index)
    {
        ObjectHandle key = inputAsset.keys[index];

        bool canDraw = key.GetProperty<bool>("canDraw");
        if (!canDraw) return;

        string keyName = key.GetProperty<string>("name");
        string keyType = key.GetProperty<string>("type");
        bool isFoldout = key.GetProperty<bool>("isFoldout");

        var setting = m_settings[keyType];
        Color titleColor = setting != null ? setting.titleColor : Color.gray;

        EditorGUI.DrawRect(new Rect(10, m_currentHeight, position.width - 20, 20), m_keyHeaderColor);
        EditorGUI.DrawRect(new Rect(11, m_currentHeight + 1, 3, 18), titleColor);
        Rect keyNameArea = new Rect(18, m_currentHeight, 100, 20);
        EditorGUI.LabelField(keyNameArea, keyName, m_keyNameStyle);
        GUI.color = titleColor;
        Rect keyTypeArea = new Rect(120, m_currentHeight, 100, 20);
        EditorGUI.LabelField(keyTypeArea, keyType);
        GUI.color = Color.white;
        isFoldout = EditorGUI.Foldout(new Rect(220, m_currentHeight, 20, 20), isFoldout, string.Empty);

        if(IsRightClickArea(keyNameArea))
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Delete"), false, () => inputAsset.RemoveAt(index));
            menu.ShowAsContext();
        }
        if(IsRightClickArea(keyTypeArea))
        {
            GenericMenu menu = new GenericMenu();
            foreach (var type in m_settings.Keys)
            {
                if (type == "Unknown") continue;
                menu.AddItem(new GUIContent(type), false, () => inputAsset.ConvertTo(index, type));
            }
            menu.ShowAsContext();
        }

        key.SetProperty("isFoldout", isFoldout);

        m_currentHeight += 20f;

        if (isFoldout)
        {
            switch (keyType)
            {
                case "TapKey":
                    DrawTapKey(key);
                    break;
                case "ValueKey":
                    DrawValueKey(key);
                    break;
                case "PressKey":
                    DrawPressKey(key);
                    break;
                case "AxisKey":
                    DrawAxisKey(key);
                    break;
                case "MultiKey":
                    DrawMultiKey(key);
                    break;
                case "ComboKey":
                    DrawComboKey(key);
                    break;
            }
        }
    }

    private void DrawTapKey(ObjectHandle key)
    {
        DrawStringProperty(key, "name", "Name", 30);
        DrawIntProperty(key, "clickCount", "Click Count", 30);
        DrawFloatProperty(key, "clickInterval", "Click Interval", 30);
        DrawKeyCodeProperty(key, "key", "Key Code", 30);
    }

    private void DrawValueKey(ObjectHandle key)
    {
        DrawStringProperty(key, "name", "Name", 30);
        DrawVector2Property(key, "range", "Range", 30);
        DrawFloatProperty(key, "start", "Start", 30);
        DrawVector2Property(key, "speed", "Speed", 30);
        DrawKeyCodeProperty(key, "key", "Key Code", 30);
    }

    private void DrawPressKey(ObjectHandle key)
    {
        DrawStringProperty(key, "name", "Name", 30);
        DrawKeyCodeProperty(key, "key", "Key Code", 30);
    }

    private void DrawAxisKey(ObjectHandle key)
    {
        DrawStringProperty(key, "name", "Name", 30);
        DrawAxes(key);
    }

    private void DrawMultiKey(ObjectHandle key)
    {
        DrawStringProperty(key, "name", "Name", 30);
        DrawFloatProperty(key, "interval", "Interval", 30);
        DrawKeyCodes(key, "key", "Keys", 30);
    }

    private void DrawComboKey(ObjectHandle key)
    {
        DrawStringProperty(key, "name", "Name", 30);
        DrawFloatProperty(key, "interval", "Interval", 30);
        DrawKeyCodes(key, "key", "Keys", 30);
    }

    private bool IsRightClickArea(Rect area)
    {
        return Event.current.type == EventType.MouseDown && Event.current.button == 1 && area.Contains(Event.current.mousePosition);
    }

    private void DrawStringProperty(ObjectHandle key, string propertyName, string displayName, float leftPadding)
    {
        string value = key.GetProperty<string>(propertyName);
        EditorGUI.DrawRect(new Rect(10, m_currentHeight, position.width - 20, 20), m_keyBackgroundColor);
        EditorGUI.LabelField(new Rect(leftPadding, m_currentHeight, 100, 20), displayName);
        value = EditorGUI.TextField(new Rect(leftPadding + 100, m_currentHeight + 1, 100, 18), value);
        key.SetProperty(propertyName, value);
        m_currentHeight += 20f;
    }

    private void DrawIntProperty(ObjectHandle key, string propertyName, string displayName, float leftPadding)
    {
        int value = key.GetProperty<int>(propertyName);
        EditorGUI.DrawRect(new Rect(10, m_currentHeight, position.width - 20, 20), m_keyBackgroundColor);
        EditorGUI.LabelField(new Rect(leftPadding, m_currentHeight, 100, 20), displayName);
        value = EditorGUI.IntField(new Rect(leftPadding + 100, m_currentHeight + 1, 100, 18), value);
        key.SetProperty(propertyName, value);
        m_currentHeight += 20f;
    }

    private void DrawFloatProperty(ObjectHandle key, string propertyName, string displayName, float leftPadding)
    {
        float value = key.GetProperty<float>(propertyName);
        EditorGUI.DrawRect(new Rect(10, m_currentHeight, position.width - 20, 20), m_keyBackgroundColor);
        EditorGUI.LabelField(new Rect(leftPadding, m_currentHeight, 100, 20), displayName);
        value = EditorGUI.FloatField(new Rect(leftPadding + 100, m_currentHeight + 1, 100, 18), value);
        key.SetProperty(propertyName, value);
        m_currentHeight += 20f;
    }

    private void DrawVector2Property(ObjectHandle key, string propertyName, string displayName, float leftPadding)
    {
        Vector2 value = key.GetProperty<Vector2>(propertyName);
        EditorGUI.DrawRect(new Rect(10, m_currentHeight, position.width - 20, 20), m_keyBackgroundColor);
        EditorGUI.LabelField(new Rect(leftPadding, m_currentHeight, 100, 20), displayName);
        value = EditorGUI.Vector2Field(new Rect(leftPadding + 100, m_currentHeight + 1, 100, 18), string.Empty, value);
        key.SetProperty(propertyName, value);
        m_currentHeight += 20f;
    }

    private void DrawKeyCodeProperty(ObjectHandle key, string propertyName, string displayName, float leftPadding)
    {
        EditorGUI.DrawRect(new Rect(10, m_currentHeight, position.width - 20, 20), m_keyBackgroundColor);
        EditorGUI.LabelField(new Rect(leftPadding, m_currentHeight, 100, 20), displayName);

        int keyCount = key.GetProperty<int>(propertyName + "Count");

 
        for (int i = 0; i < keyCount; i++)
        {
            string keyCodeName = propertyName + i.ToString();
            KeyCode value = key.GetProperty<KeyCode>(keyCodeName);
            value = (KeyCode)EditorGUI.EnumPopup(new Rect(leftPadding + 100 + i * 105, m_currentHeight + 1, 100, 18), value);
            key.SetProperty(keyCodeName, value);
        }

        bool addKey = GUI.Button(new Rect(leftPadding + 100 + keyCount * 105, m_currentHeight + 1, 18, 18), string.Empty);
        EditorGUI.LabelField(new Rect(leftPadding + 100 + keyCount * 105, m_currentHeight - 1, 20, 20), "+", m_buttonTextStyle);
        bool removeKey = GUI.Button(new Rect(leftPadding + 123 + keyCount * 105, m_currentHeight + 1, 18, 18), string.Empty);
        EditorGUI.LabelField(new Rect(leftPadding + 123 + keyCount * 105, m_currentHeight - 1, 20, 20), "-", m_buttonTextStyle);

        m_currentHeight += 20f;

        if (addKey)
        {
            key.AddProperty(propertyName + keyCount.ToString(), KeyCode.None);
            key.SetProperty(propertyName + "Count", keyCount + 1);
        }
        if (removeKey)
        {
            if (keyCount == 1) return;
            key.RemoveProperty(propertyName + (keyCount - 1).ToString());
            keyCount = Mathf.Max(keyCount - 1, 0);
            key.SetProperty(propertyName + "Count", keyCount);
        }
    }

    private void DrawAxes(ObjectHandle key)
    {
        int axesCount = key.GetProperty<int>("axesCount");
        string name = key.GetProperty<string>("name");

        EditorGUI.DrawRect(new Rect(10, m_currentHeight, position.width - 20, 20), m_keyBackgroundColor);
        EditorGUI.LabelField(new Rect(30, m_currentHeight, 100, 20), "Axes");
        GUI.enabled = false;
        EditorGUI.IntField(new Rect(130, m_currentHeight + 1, 100, 18), axesCount);
        GUI.enabled = true;
        bool addAxis = GUI.Button(new Rect(235, m_currentHeight + 1, 18, 18), string.Empty);
        EditorGUI.LabelField(new Rect(236, m_currentHeight, 18, 18), "+", m_buttonTextStyle);
        bool removeAxis = GUI.Button(new Rect(258, m_currentHeight + 1, 18, 18), string.Empty);
        EditorGUI.LabelField(new Rect(259, m_currentHeight, 18, 18), "-", m_buttonTextStyle);

        m_currentHeight += 20f;

        for (int i = 0; i < axesCount; i++)
        {
            DrawAxis(name, i);
        }

        if (addAxis)
        {
            key.SetProperty("axesCount", axesCount + 1);
            string axixName = name + axesCount.ToString();
            inputAsset.AddAxis(axixName);
        }

        if(removeAxis)
        {
            if (axesCount == 0) return;
            key.SetProperty("axesCount", axesCount - 1);
            string axisName = name + (axesCount - 1).ToString();
            inputAsset.RemoveAxis(axisName);
        }
    }

    private void DrawAxis(string axisKeyName, int index)
    {
        string axisName = axisKeyName + index.ToString();
        ObjectHandle axis = inputAsset.keys.Find(k => k.GetProperty<string>("name") == axisName);
        if (axis == null) return;

        bool isFoldout = axis.GetProperty<bool>("isFoldout");

        EditorGUI.DrawRect(new Rect(10, m_currentHeight, position.width - 20, 20), m_axisHeaderColor);
        EditorGUI.LabelField(new Rect(30, m_currentHeight, 100, 20), "Axis" + index.ToString());
        isFoldout = EditorGUI.Foldout(new Rect(220, m_currentHeight, 20, 20), isFoldout, string.Empty);

        m_currentHeight += 20f;

        if(isFoldout)
            DrawAxisContent(axis);

        axis.SetProperty("isFoldout", isFoldout);
    }

    private void DrawAxisContent(ObjectHandle axis)
    {
        DrawVector2Property(axis, "range", "Range", 30);
        DrawFloatProperty(axis, "start", "Start", 30);
        DrawVector2Property(axis, "posSpeed", "Pos Speed", 30);
        DrawVector2Property(axis, "negSpeed", "Neg Speed", 30);
        DrawKeyCodeProperty(axis, "posKey", "Pos Key", 30);
        DrawKeyCodeProperty(axis, "negKey", "Neg Key", 30);
    }

    private void DrawKeyCodes(ObjectHandle key, string propertyName, string displayName, float leftPadding)
    {
        int keysCount = key.GetProperty<int>(propertyName + "Count");

        EditorGUI.DrawRect(new Rect(10, m_currentHeight, position.width - 20, 20), m_keyBackgroundColor);
        EditorGUI.LabelField(new Rect(leftPadding, m_currentHeight, 100, 20), displayName);
        GUI.enabled = false;
        EditorGUI.IntField(new Rect(130, m_currentHeight + 1, 100, 18), keysCount);
        GUI.enabled = true;
        bool addKey = GUI.Button(new Rect(235, m_currentHeight + 1, 18, 18), string.Empty);
        EditorGUI.LabelField(new Rect(236, m_currentHeight, 18, 18), "+", m_buttonTextStyle);
        bool removeKey = GUI.Button(new Rect(258, m_currentHeight + 1, 18, 18), string.Empty);
        EditorGUI.LabelField(new Rect(259, m_currentHeight, 18, 18), "-", m_buttonTextStyle);

        m_currentHeight += 20f;

        if(addKey)
        {
            key.SetProperty(propertyName + "Count", keysCount + 1);
            key.AddProperty(propertyName + keysCount.ToString() + "_0" , KeyCode.None);
            key.AddProperty(propertyName + keysCount.ToString() + "Count", 1);
        }
        if(removeKey && keysCount > 2)
        {
            key.SetProperty(propertyName + "Count", keysCount - 1);
            int keyCount = key.GetProperty<int>(propertyName + (keysCount - 1).ToString() + "Count");
            for (int i = 0; i < keyCount; i++)
            {
                key.RemoveProperty(propertyName + (keysCount - 1).ToString() + "_" + i.ToString());
            }
            key.RemoveProperty(propertyName + (keysCount - 1).ToString() + "Count");
        }

        for (int i = 0; i < keysCount; i++)
        {
            string keyName = propertyName + i.ToString();
            int keyCount = key.GetProperty<int>(keyName + "Count");
            EditorGUI.DrawRect(new Rect(10, m_currentHeight, position.width - 20, 20), m_keyBackgroundColor);
            for (int j = 0; j < keyCount; j++)
            {
                KeyCode keyCode = key.GetProperty<KeyCode>(keyName + "_" + j.ToString());
                keyCode = (KeyCode)EditorGUI.EnumPopup(new Rect(130 + 105 * j, m_currentHeight + 1, 100, 18), keyCode);
                key.SetProperty(keyName + "_" + j.ToString(), keyCode);
            }
            bool addKeyCode = GUI.Button(new Rect(130 + keyCount * 105, m_currentHeight + 1, 18, 18), string.Empty);
            EditorGUI.LabelField(new Rect(131 + keyCount * 105, m_currentHeight, 18, 18), "+", m_buttonTextStyle);
            bool removeKeyCode = GUI.Button(new Rect(153 + keyCount * 105, m_currentHeight + 1, 18, 18), string.Empty);
            EditorGUI.LabelField(new Rect(154 + keyCount * 105, m_currentHeight, 18, 18), "-", m_buttonTextStyle);

            if(addKeyCode)
            {
                key.SetProperty(keyName + "Count", keyCount + 1);
                key.AddProperty(keyName + "_" + keyCount.ToString(), KeyCode.None);
            }
            if(removeKeyCode && keyCount > 1)
            {
                key.SetProperty(keyName + "Count", keyCount - 1);
                key.RemoveProperty(keyName + "_" + (keyCount - 1).ToString());
            }

            m_currentHeight += 20f;
        }
    }
}
