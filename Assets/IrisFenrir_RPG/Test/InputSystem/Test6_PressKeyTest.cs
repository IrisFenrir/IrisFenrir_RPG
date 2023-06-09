using IrisFenrir.InputSystem;
using UnityEngine;

public class Test6_PressKeyTest : MonoBehaviour
{
    public float pressingTime;
    public bool load;

    private PressKey m_key;
    private string m_path;

    private void Start()
    {
        m_key = new PressKey();
        m_path = Application.dataPath + "/IrisFenrir_RPG/Test/InputSystem/Save/PressKeyTest.fenrir";

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

        pressingTime = m_key.pressTime;
        if (m_key.isDown)
            Debug.Log("Down");
        if (m_key.isPressing)
            Debug.Log("Pressing");
        if (m_key.isUp)
            Debug.Log("Up");

        if(Input.GetKeyDown(KeyCode.J))
        {
            SaveHelper.Save(m_key, m_path);
            Debug.Log("Save");
        }    
    }
}
