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
        tapKey.Name = "KeyA";
        InputSystem.AddKey(tapKey);

        ValueKey valueKey = new ValueKey();
        valueKey.SetKeyCode(KeyCode.Space);
        valueKey.Name = "KeyB";
        InputSystem.AddKey(valueKey);

        AxisKey axisKey = new AxisKey();
        axisKey.AddAxis(KeyCode.W, KeyCode.S);
        axisKey.AddAxis(KeyCode.D, KeyCode.A);
        axisKey.Name = "KeyC";
        InputSystem.AddKey(axisKey);

        InputSystem.Instance.SetEnable(true);
    }

    private void Start()
    {
        m_path = Application.dataPath + "/IrisFenrir_RPG/Test/InputSystem/Save/InputSystemTest.fenrir";

        if (load)
        {
            SaveHelper.Load(InputSystem.Instance, m_path);
            Debug.Log("Load");
        }

        m_keyA = InputSystem.FindKey<TapKey>("KeyA");
        m_keyB = InputSystem.FindKey<ValueKey>("KeyB");
        m_keyC = InputSystem.FindKey<AxisKey>("KeyC");
    }

    private void Update()
    {
        InputSystem.Instance.Update(Time.deltaTime);

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
            SaveHelper.Save(InputSystem.Instance, m_path);
            Debug.Log("Save");
        }    
    }
}
