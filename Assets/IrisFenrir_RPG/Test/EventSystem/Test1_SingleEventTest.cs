using IrisFenrir.EventSystem;
using UnityEngine;

public class Test1_SingleEventTest : MonoBehaviour
{
    public bool condition;
    public int parameter1;
    public string parameter2;

    private SingleEvent m_event1;
    private SingleEvent<int> m_event2;
    private SingleEvent<(int, string)> m_event3;

    private void Start()
    {
        m_event1 = new SingleEvent("A");
        m_event1.Register(() => condition)
            .Subscribe(
                () => Debug.Log("A1"), 
                () => Debug.Log("A2")
            );
        m_event1.SetEnable(true);

        m_event2 = new SingleEvent<int>("B");
        m_event2.Register(() => condition, () => parameter1)
            .Subscribe(
                i => Debug.Log($"B1 {i}"),
                i => Debug.Log($"B2 {i}")
            );
        m_event2.SetEnable(true);

        m_event3 = new SingleEvent<(int, string)>("C");
        m_event3.Register(() => condition, () => (parameter1, parameter2))
            .Subscribe(
                t => Debug.Log($"C1 {t.Item1} {t.Item2}")
            );
        m_event3.SetEnable(true);
    }

    private void Update()
    {
        m_event1.Update(Time.deltaTime);
        m_event2.Update(Time.deltaTime);
        m_event3.Update(Time.deltaTime);
    }
}
