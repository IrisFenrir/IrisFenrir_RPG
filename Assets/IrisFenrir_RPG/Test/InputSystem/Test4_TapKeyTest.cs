using IrisFenrir.InputSystem;
using UnityEngine;

public class Test4_TapKeyTest : MonoBehaviour
{
    public Test2_Button button;
    public bool load;

    private TapKey m_key;
    private string m_path;

    private void Start()
    {
        m_key = new TapKey();
        m_path = Application.dataPath + "/IrisFenrir_RPG/Test/InputSystem/Save/TapKeyTest.fenrir";

        DelegatedKey key = new DelegatedKey();
        key.getKeyDown = () => button.down;
        key.getKeyPressing = () => button.pressing;
        key.getKeyUp = () => button.up;

        m_key.AddKey(key);

        if(!load)
        {
            m_key.SetKeyCode(KeyCode.Space);
            m_key.clickCount = 3;
            m_key.SetEnable(true, true);
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
            Debug.Log("Tap Key");

        if(Input.GetKeyDown(KeyCode.K))
        {
            m_key.key.SetKeyEnable(true, true, 1);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            m_key.key.SetKeyEnable(false, true, 1);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            SaveHelper.Save(m_key, m_path);
            Debug.Log("Save");
        }
    }
}
