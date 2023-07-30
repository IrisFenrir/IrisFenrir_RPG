using IrisFenrir.InputSystem;
using UnityEngine;
using UnityEngine.UI;

public class Test12_UIKeyTest : MonoBehaviour
{
    public InputAsset asset;
    public GraphicRaycaster raycaster;

    [Header("Output")]
    public float value;
    public Vector2 vec;

    private UIKeyManager m_uiKeyManager;

    private TapKey m_tapKey;
    private ValueKey m_valueKey;
    private PressKey m_pressKey;
    private AxisKey m_axisKey;
    private MultiKey m_multiKey;
    private ComboKey m_comboKey;

    private void Start()
    {
        InputSystemBuilder.Build(asset);

        m_tapKey = InputSystem.FindKey<TapKey>("TapTest");
        m_tapKey.AddKey(new RealKey() { keyCode = KeyCode.T });
        m_valueKey = InputSystem.FindKey<ValueKey>("ValueTest");
        m_pressKey = InputSystem.FindKey<PressKey>("PressTest");
        m_axisKey = InputSystem.FindKey<AxisKey>("AxisTest");
        m_multiKey = InputSystem.FindKey<MultiKey>("MultiTest");
        m_comboKey = InputSystem.FindKey<ComboKey>("ComboTest");

        m_uiKeyManager = new UIKeyManager(raycaster, "UI/InputWindow");
        m_uiKeyManager.SetEnable(true);

        InputSystem.Instance.SetEnable(true);
    }

    private void Update()
    {
        InputSystem.Instance.Update(Time.deltaTime);
        m_uiKeyManager.Update(Time.deltaTime);

        
        if (m_tapKey.isTriggered)
            Debug.Log("Tap Key!!!");

        value = m_valueKey.value;

        if (m_pressKey.isDown)
            Debug.Log("Down");
        if (m_pressKey.isPressing)
            Debug.Log("Pressing");
        if (m_pressKey.isUp)
            Debug.Log("Up");

        vec = m_axisKey.GetVector2();

        if (m_multiKey.isTriggered)
            Debug.Log("Multi Key!!!");

        if (m_comboKey.isTriggered)
            Debug.Log("Combo Key!!!");
    }
}
