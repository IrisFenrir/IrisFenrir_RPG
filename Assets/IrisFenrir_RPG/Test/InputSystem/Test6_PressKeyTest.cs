using IrisFenrir.InputSystem;
using UnityEngine;

public class Test6_PressKeyTest : MonoBehaviour
{
    public bool defaultInit;

    private PressKey m_key;

    private void Start()
    {
        m_key = new PressKey();

        if(defaultInit)
        {
            m_key.SetKeyCode(KeyCode.Space);
            m_key.SetEnable(true);
        }
        else
        {
            m_key.Load(SaveHelper.Load(Application.dataPath + "/Test.fenrir"));
            Debug.Log("Load");
        }
    }

    private void Update()
    {
        m_key.Update(Time.deltaTime);

        if (m_key.isDown)
            Debug.Log("Down");
        if (m_key.isPressing)
            Debug.Log("Pressing");
        if (m_key.isUp)
            Debug.Log("Up");

        if(Input.GetKeyDown(KeyCode.J))
        {
            SaveHelper.Save(m_key.Save(), Application.dataPath + "/Test.fenrir");
            Debug.Log("Save");
        }    
    }
}
