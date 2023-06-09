
using IrisFenrir.InputSystem;
using UnityEngine;

public class Test2_DelegatedKeyTest : MonoBehaviour
{
    public Test2_Button button;

    private DelegatedKey m_key;

    private void Start()
    {
        m_key = new DelegatedKey();
        m_key.getKeyDown = () => button.down;
        m_key.getKeyPressing = () => button.pressing;
        m_key.getKeyUp = () => button.up;
        m_key.SetEnable(true);
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

