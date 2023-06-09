using IrisFenrir.InputSystem;
using UnityEngine;

public class Test7_AxisKeyTest : MonoBehaviour
{
    public Vector2 input;
    public bool defaultInit = true;

    private AxisKey m_key;

    private void Start()
    {
        m_key = new AxisKey(2);

        if(defaultInit)
        {
            m_key.AddAxis(0, KeyCode.W, KeyCode.S);
            m_key.AddAxis(1, KeyCode.D, KeyCode.A);
            m_key.SetEnable(true);
        }
        else
        {
            m_key.Load(SaveHelper.Load(Application.dataPath + "/AxisKeyTest.fenrir"));
            Debug.Log("Load");
        }

        //m_key.axes[0].SetEnable(false);
    }

    private void Update()
    {
        m_key.Update(Time.deltaTime);

        input = m_key.GetVector2();

        if(Input.GetKeyDown(KeyCode.J))
        {
            SaveHelper.Save(m_key.Save(), Application.dataPath + "/AxisKeyTest.fenrir");
            Debug.Log("Save");
        }
    }
}

