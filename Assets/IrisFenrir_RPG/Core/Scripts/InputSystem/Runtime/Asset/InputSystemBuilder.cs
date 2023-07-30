using UnityEngine;

namespace IrisFenrir.InputSystem
{
    public static class InputSystemBuilder
    {
        public static void Build(InputAsset asset)
        {
            foreach (ObjectHandle key in asset.keys)
            {
                string type = key.GetProperty<string>("type");
                switch(type)
                {
                    case "TapKey":
                        TapKey tapKey = new TapKey();
                        tapKey.Name = key.GetProperty<string>("name");
                        tapKey.clickCount = key.GetProperty<int>("clickCount");
                        tapKey.clickInterval = key.GetProperty<float>("clickInterval");
                        ReadKeyCode(tapKey, 0, key, "key");
                        InputSystem.AddKey(tapKey);
                        break;
                    case "ValueKey":
                        ValueKey valueKey = new ValueKey();
                        valueKey.Name = key.GetProperty<string>("name");
                        valueKey.range = key.GetProperty<Vector2>("range");
                        valueKey.start = key.GetProperty<float>("start");
                        valueKey.speed = key.GetProperty<Vector2>("speed");
                        ReadKeyCode(valueKey, 0, key, "key");
                        InputSystem.AddKey(valueKey);
                        break;
                    case "PressKey":
                        PressKey pressKey = new PressKey();
                        pressKey.Name = key.GetProperty<string>("name");
                        ReadKeyCode(pressKey, 0, key, "key");
                        InputSystem.AddKey(pressKey);
                        break;
                    case "AxisKey":
                        AxisKey axisKey = new AxisKey();
                        axisKey.Name = key.GetProperty<string>("name");
                        int axesCount = key.GetProperty<int>("axesCount");
                        for (int i = 0; i < axesCount; i++)
                        {
                            ObjectHandle axis = asset.keys.Find(k => k.GetProperty<string>("name") == axisKey.Name + i.ToString());
                            if (axis == null) continue;
                            axisKey.AddAxis(axis.GetProperty<KeyCode>("posKey0"), axis.GetProperty<KeyCode>("negKey0"),
                                            axis.GetProperty<Vector2>("range"), axis.GetProperty<float>("start"),
                                            axis.GetProperty<Vector2>("posSpeed"), axis.GetProperty<Vector2>("negSpeed"));
                            ReadKeyCode(axisKey.axes[i], 0, axis, "posKey");
                            ReadKeyCode(axisKey.axes[i], 1, axis, "negKey");
                        }
                        InputSystem.AddKey(axisKey);
                        break;
                    case "MultiKey":
                        int keyCount = key.GetProperty<int>("keyCount");
                        MultiKey multiKey = new MultiKey(keyCount);
                        multiKey.Name = key.GetProperty<string>("name");
                        int count = key.GetProperty<int>("keyCount");
                        for (int i = 0; i < count; i++)
                        {
                            int keyCodeCount = key.GetProperty<int>("key" + i.ToString() + "Count");
                            multiKey.SetKeyCode(key.GetProperty<KeyCode>("key" + i.ToString() + "_0"), i, 0);
                            for (int j = 1; j < keyCodeCount; j++)
                            {
                                multiKey.keys[i].AddKey(new RealKey() { keyCode = key.GetProperty<KeyCode>("key" + i.ToString() + "_" + j.ToString()) });
                            }
                        }
                        InputSystem.AddKey(multiKey);
                        break;
                    case "ComboKey":
                        keyCount = key.GetProperty<int>("keyCount");
                        ComboKey comboKey = new ComboKey(keyCount);
                        comboKey.Name = key.GetProperty<string>("name");
                        count = key.GetProperty<int>("keyCount");
                        for (int i = 0; i < count; i++)
                        {
                            int keyCodeCount = key.GetProperty<int>("key" + i.ToString() + "Count");
                            comboKey.SetKeyCode(key.GetProperty<KeyCode>("key" + i.ToString() + "_0"), i, 0);
                            for (int j = 1; j < keyCodeCount; j++)
                            {
                                comboKey.keys[i].AddKey(new RealKey() { keyCode = key.GetProperty<KeyCode>("key" + i.ToString() + "_" + j.ToString()) });
                            }
                        }
                        InputSystem.AddKey(comboKey);
                        break;
                }
            }
        }

        private static void ReadKeyCode(IVirutalKey key, int index, ObjectHandle handle, string propertyName)
        {
            int keyCodeCount = handle.GetProperty<int>(propertyName + "Count");
            key.SetKeyCode(handle.GetProperty<KeyCode>(propertyName + "0"), index);
            for (int i = 1; i < keyCodeCount; i++)
            {
                key.AddKey(new RealKey() { keyCode = handle.GetProperty<KeyCode>(propertyName + i.ToString()) }, index);
            }
        }
    }
}
