using TMPro;
using UnityEngine;

public class MapLabel : MonoBehaviour
{
    private void Start()
    {
        transform.GetComponentInChildren<TextMeshPro>().text = name;
    }
}
