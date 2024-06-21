using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapLabelController : MonoBehaviour
{
    public MapLabelController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
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
        return GetComponentsInChildren<MapLabel>().First(ml => ml.name == name).gameObject;
    }
}
