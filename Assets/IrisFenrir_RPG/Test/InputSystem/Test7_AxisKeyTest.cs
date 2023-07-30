using IrisFenrir.InputSystem;
using UnityEngine;

public class Test7_AxisKeyTest : MonoBehaviour
{
    public Vector2 input;
    public bool load;

    private AxisKey m_key;
    private string m_path;

    private void Start()
    {
        m_key = new AxisKey();
        m_path = Application.dataPath + "/IrisFenrir_RPG/Test/InputSystem/Save/AxisKeyTest.fenrir";

        if (!load)
        {
            m_key.AddAxis(KeyCode.D, KeyCode.A);
            m_key.AddAxis(KeyCode.W, KeyCode.S);
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

        input = m_key.GetVector2();

        if(Input.GetKeyDown(KeyCode.J))
        {
            SaveHelper.Save(m_key, m_path);
            Debug.Log("Save");
        }
    }
}

