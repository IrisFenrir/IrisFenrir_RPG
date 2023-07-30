using IrisFenrir;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {
        PropertyHandle property = new PropertyHandle();
        property.SetProperty(new Vector3(1, 2, 3));
        Vector3 value = property.GetProperty<Vector3>();
    }
}
