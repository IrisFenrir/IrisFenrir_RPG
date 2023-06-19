using IrisFenrir.EventSystem;
using UnityEngine;

public class Test2_EventGroupTest : MonoBehaviour
{
    public bool enableB;
    public bool conditionB;
    public int parameterB;

    public bool enableC;
    public bool conditionC;
    public string parameterC;

    private EventGroup m_group;
    private SingleEvent<int> m_event1;
    private SingleEvent<string> m_event2;

    private void Start()
    {
        m_group = new EventGroup("A");

        m_event1 = new SingleEvent<int>("B");
        m_event1.Register(() => conditionB, () => parameterB)
            .Subscribe(p => Debug.Log($"B: {p}"));

        m_event2 = new SingleEvent<string>("C");
        m_event2.Register(() => conditionC, () => parameterC)
            .Subscribe(p => Debug.Log($"C: {p}"));

        m_group.AddEvent(m_event1);
        m_group.AddEvent(m_event2);
        m_group.SetEnable(true);
    }

    private void Update()
    {
        m_event1.SetEnable(enableB);
        m_event2.SetEnable(enableC);
        m_group.Update(Time.deltaTime);
    }
}
