using IrisFenrir.InputSystem;
using UnityEngine;

public class Test5_ValueKeyTest : MonoBehaviour
{
    public float value;
    public bool load;

    private ValueKey m_key;
    private string m_path;

    private void Start()
    {
        m_key = new ValueKey();
        m_path = Application.dataPath + "/IrisFenrir_RPG/Test/InputSystem/Save/ValueKeyTest.fenrir";

        if (!load)
        {
            m_key.SetKeyCode(KeyCode.Space);
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

        value = m_key.value;

        if (Input.GetKeyDown(KeyCode.J))
        {
            SaveHelper.Save(m_key, m_path);
            Debug.Log("Save");
        }
    }
}

