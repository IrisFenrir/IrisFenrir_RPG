using IrisFenrir;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {
        string json = "123123e";
        Json js = JsonMapper.ToJsonObject(json);
    }
}
