using IrisFenrir.InputSystem;
using UnityEngine;

public class Test1_RealKeyTest : MonoBehaviour
{
    public KeyCode keyCode;
    public bool enable;

    private RealKey m_key;

    private void Start()
    {
        m_key = new RealKey();
        m_key.keyCode = keyCode;
        m_key.SetEnable(enable);
    }

    private void Update()
    {
        if (m_key.GetKeyDown())
            Debug.Log("Down");

        if (m_key.GetKeyPressing())
            Debug.Log("Pressing");

        if (m_key.GetKeyUp())
            Debug.Log("Up");
    }
}
