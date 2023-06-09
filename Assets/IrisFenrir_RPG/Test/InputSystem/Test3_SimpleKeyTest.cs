using IrisFenrir.InputSystem;
using UnityEngine;

public class Test3_SimpleKeyTest : MonoBehaviour
{
    public Test2_Button button;

    private SimpleKey m_key;

    private void Start()
    {
        m_key = new SimpleKey();

        RealKey key1 = new RealKey();
        key1.keyCode = KeyCode.Space;
        RealKey key2 = new RealKey();
        key2.keyCode = KeyCode.N;
        DelegatedKey key3 = new DelegatedKey();
        key3.getKeyDown = () => button.down;
        key3.getKeyPressing = () => button.pressing;
        key3.getKeyUp = () => button.up;
        m_key.AddKey(key1);
        m_key.AddKey(key2);
        m_key.AddKey(key3);

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

