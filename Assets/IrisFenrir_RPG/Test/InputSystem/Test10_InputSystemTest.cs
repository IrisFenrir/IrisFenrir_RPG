using IrisFenrir.InputSystem;
using UnityEngine;

public class Test10_InputSystemTest : MonoBehaviour
{
    [Header("Output")]
    public float value;
    public Vector2 input;

    [Header("Load")]
    public bool load;

    private TapKey m_keyA;
    private ValueKey m_keyB;
    private AxisKey m_keyC;
    private string m_path;

    private void Awake()
    {
        TapKey tapKey = new TapKey();
        tapKey.SetKeyCode(KeyCode.Space);
        tapKey.clickCount = 3;
        tapKey.name = "KeyA";
        InputSystem.AddKey(tapKey);

        ValueKey valueKey = new ValueKey();
        valueKey.SetKeyCode(KeyCode.Space);
        valueKey.name = "KeyB";
        InputSystem.AddKey(valueKey);

        AxisKey axisKey = new AxisKey(2);
        axisKey.AddAxis(0, KeyCode.W, KeyCode.S);
        axisKey.AddAxis(1, KeyCode.D, KeyCode.A);
        axisKey.name = "KeyC";
        InputSystem.AddKey(axisKey);

        InputSystem.instance.SetEnable(true);
    }

    private void Start()
    {
        m_path = Application.dataPath + "/IrisFenrir_RPG/Test/InputSystem/Save/InputSystemTest.fenrir";

        if (load)
        {
            SaveHelper.Load(InputSystem.instance, m_path);
            Debug.Log("Load");
        }

        m_keyA = InputSystem.FindKey<TapKey>("KeyA");
        m_keyB = InputSystem.FindKey<ValueKey>("KeyB");
        m_keyC = InputSystem.FindKey<AxisKey>("KeyC");
    }

    private void Update()
    {
        InputSystem.instance.Update(Time.deltaTime);

        if (m_keyA.isTriggered)
            Debug.Log("TapKey");

        value = m_keyB.value;

        input = m_keyC.GetVector2();

        if(Input.GetKeyDown(KeyCode.C))
        {
            m_keyA.SetKeyCode(KeyCode.V);
            Debug.Log("已将KeyA键位更改为V键");
        }

        if(Input.GetKeyDown(KeyCode.J))
        {
            SaveHelper.Save(InputSystem.instance, m_path);
            Debug.Log("Save");
        }    
    }
}
