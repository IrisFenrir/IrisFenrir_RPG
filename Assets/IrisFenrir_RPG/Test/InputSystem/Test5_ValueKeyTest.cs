using IrisFenrir.InputSystem;
using UnityEngine;

public class Test5_ValueKeyTest : MonoBehaviour
{
    public float value;
    public bool defaultInit;

    private ValueKey m_key;

    private void Start()
    {
        m_key = new ValueKey();

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

        value = m_key.value;

        if (Input.GetKeyDown(KeyCode.J))
        {
            SaveHelper.Save(m_key.Save(), Application.dataPath + "/Test.fenrir");
            Debug.Log("Save");
        }
    }
}

