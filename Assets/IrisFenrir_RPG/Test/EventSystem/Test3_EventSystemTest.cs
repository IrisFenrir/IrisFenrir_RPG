using IrisFenrir.EventSystem;
using UnityEngine;

public class Test3_EventSystemTest : MonoBehaviour
{
    public bool condition;
    public int parameter1;
    public string parameter2;

    private void Start()
    {
        EventSystem.CreateEventGroup("Root", "A")
            .CreateEvent("B")
            .Register(() => condition)
            .Subscribe(() => Debug.Log("B"));

        EventSystem.CreateEvent<int>("A", "C")
            .Register(() => condition, () => parameter1)
            .Subscribe(p => Debug.Log($"C: {p}"));

        EventSystem.CreateEvent<(int, string)>("A", "D")
            .Register(() => condition, () => (parameter1, parameter2))
            .Subscribe(p => Debug.Log($"D: {p.Item1} {p.Item2}"));

        EventSystem.instance.SetEnable(true);
    }

    private void Update()
    {
        EventSystem.instance.Update(Time.deltaTime);
    }
}
