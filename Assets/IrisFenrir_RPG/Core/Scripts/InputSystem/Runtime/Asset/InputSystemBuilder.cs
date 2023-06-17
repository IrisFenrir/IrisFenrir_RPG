namespace IrisFenrir.InputSystem
{
    public class InputSystemBuilder
    {
        public static void Build(InputSystemAsset asset)
        {
            foreach (var data in asset.keys)
            {
                switch(data.type)
                {
                    case KeyDataUnit.KeyType.TapKey:
                        TapKey tapKey = new TapKey();
                        tapKey.name = data.GetStringProperty("name");
                        tapKey.clickCount = data.GetIntProperty("clickCount");
                        tapKey.clickInterval = data.GetFloatProperty("clickInterval");
                        tapKey.SetKeyCode(data.GetKeyCodeProperty("keyCode"));
                        InputSystem.AddKey(tapKey);
                        break;
                    case KeyDataUnit.KeyType.ValueKey:
                        ValueKey valueKey = new ValueKey();
                        valueKey.name = data.GetStringProperty("name");
                        valueKey.range = data.GetVector2Property("range");
                        valueKey.start = data.GetFloatProperty("start");
                        valueKey.speed = data.GetVector2Property("speed");
                        valueKey.SetKeyCode(data.GetKeyCodeProperty("keyCode"));
                        InputSystem.AddKey(valueKey);
                        break;
                    case KeyDataUnit.KeyType.PressKey:
                        PressKey pressKey = new PressKey();
                        pressKey.name = data.GetStringProperty("name");
                        pressKey.SetKeyCode(data.GetKeyCodeProperty("keyCode"));
                        InputSystem.AddKey(pressKey);
                        break;
                    case KeyDataUnit.KeyType.AxisKey:
                        int axisCount = data.GetIntProperty("axisCount");
                        AxisKey axisKey = new AxisKey(axisCount);
                        axisKey.name = data.GetStringProperty("name");
                        for (int i = 0; i < axisCount; i++)
                        {
                            axisKey.AddAxis(i, data.GetKeyCodeProperty($"posKey{i}"), data.GetKeyCodeProperty($"negKey{i}"), data.GetVector2Property($"range{i}"), data.GetFloatProperty($"start{i}"), data.GetVector2Property($"posSpeed{i}"), data.GetVector2Property($"negSpeed{i}"));
                        }
                        InputSystem.AddKey(axisKey);
                        break;
                    case KeyDataUnit.KeyType.MultiKey:
                        int keyCount = data.GetIntProperty("keyCount");
                        MultiKey multiKey = new MultiKey(keyCount);
                        multiKey.name = data.GetStringProperty("name");
                        multiKey.interval = data.GetFloatProperty("interval");
                        for (int i = 0; i < keyCount; i++)
                        {
                            multiKey.SetKeyCode(data.GetKeyCodeProperty($"key{i}"), i);
                        }
                        InputSystem.AddKey(multiKey);
                        break;
                    case KeyDataUnit.KeyType.ComboKey:
                        keyCount = data.GetIntProperty("keyCount");
                        ComboKey comboKey = new ComboKey(keyCount);
                        comboKey.name = data.GetStringProperty("name");
                        comboKey.interval = data.GetFloatProperty("interval");
                        for (int i = 0; i < keyCount; i++)
                        {
                            comboKey.SetKeyCode(data.GetKeyCodeProperty($"key{i}"), i);
                        }
                        InputSystem.AddKey(comboKey);
                        break;
                }
            }
        }
    }
}
