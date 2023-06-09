using IrisFenrir.InputSystem;
using UnityEngine;

public class Test8_MultiKeyTest : MonoBehaviour
{
    public bool load;

    private MultiKey m_key;
    private string m_path;

    private void Start()
    {
        m_key = new MultiKey(2);
        m_path = Application.dataPath + "/IrisFenrir_RPG/Test/InputSystem/Save/MultiKeyTest.fenrir";

        if (!load)
        {
            m_key.SetKeyCode(KeyCode.F, 0);
            m_key.SetKeyCode(KeyCode.G, 1);
            m_key.SetEnable(true);
        }
        else
        {
            SaveHelper.Load(m_key, m_path);
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
            SaveHelper.Save(m_key, m_path);
            Debug.Log("Save");
        }
    }
}

