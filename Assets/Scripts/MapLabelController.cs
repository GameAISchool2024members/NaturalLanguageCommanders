using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapLabelController : MonoBehaviour
{
    public static MapLabelController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Debug.Log(string.Join(",", GetLabels()));
    }

    public IEnumerable<string> GetLabels()
    {
        foreach (var child in GetComponentsInChildren<MapLabel>())
        {
            yield return child.name;
        }
    }

    public GameObject ObjectByLabel(string name)
    {
        return GetComponentsInChildren<MapLabel>().First(ml => ml.name == name)?.gameObject;
    }
}
