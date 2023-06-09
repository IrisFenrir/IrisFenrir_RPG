using IrisFenrir.InputSystem;
using UnityEngine;

public class Test8_MultiKeyTest : MonoBehaviour
{
    public bool defaultInit = true;

    private MultiKey m_key;
    private string m_path;

    private void Start()
    {
        m_key = new MultiKey(2);
        m_path = Application.dataPath + "/MultiKeyTest.fenrir";

        if(defaultInit)
        {
            m_key.SetKeyCode(KeyCode.F, 0);
            m_key.SetKeyCode(KeyCode.G, 1);
            m_key.SetEnable(true);
        }
        else
        {
            m_key.Load(SaveHelper.Load(m_path));
            Debug.Log("Load");
        }
    }

    private void Update()
    {
        m_key.Update(Time.deltaTime);

        if (m_key.isTriggered)
            Debug.Log("Multi Key");

        if(Input.GetKeyDown(KeyCode.J))
        {
            SaveHelper.Save(m_key.Save(), m_path);
            Debug.Log("Save");
        }
    }
}

