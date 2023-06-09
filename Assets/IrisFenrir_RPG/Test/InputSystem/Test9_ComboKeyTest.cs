using IrisFenrir.InputSystem;
using UnityEngine;

public class Test9_ComboKeyTest : MonoBehaviour
{
    public int combo;
    public bool load;

    private ComboKey m_key;
    private string m_path;

    private void Start()
    {
        m_key = new ComboKey(3);
        m_path = Application.dataPath + "/IrisFenrir_RPG/Test/InputSystem/Save/ComboKeyTest.fenrir";

        if (!load)
        {
            m_key.SetKeyCode(KeyCode.B, 0);
            m_key.SetKeyCode(KeyCode.N, 1);
            m_key.SetKeyCode(KeyCode.M, 2);
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

        combo = m_key.combo;
        if (m_key.isTriggered)
            Debug.Log("Combo Key!!!");

        if(Input.GetKeyDown(KeyCode.J))
        {
            SaveHelper.Save(m_key, m_path);
            Debug.Log("Save");
        }
    }
}

