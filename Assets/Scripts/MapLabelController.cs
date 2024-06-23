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

    public string ClosestLabel(Vector3 position)
    {
        var closest_distance = float.MaxValue;
        var closest_label = "";
        foreach (var child in GetComponentsInChildren<MapLabel>())
        {
            var distance = Vector3.Distance(position, child.transform.position);
            if (distance < closest_distance)
            {
                closest_distance = distance;
                closest_label = child.name;
            }
        }
        return closest_label;
    }
}
