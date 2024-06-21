using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class AgentVision : MonoBehaviour
{
    [SerializeField]
    private float angleDegrees;
    private float angleRadians;
    [SerializeField]
    private int visionRange;


    [SerializeField]
    private Vector2[] points;
    [SerializeField]
    private Material material;
    [SerializeField]
    private Color fillColor = new Color(1, 1, 1, 0.5f); // semi-transparent white

    [SerializeField]
    private int rayCastResolution = 10;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        angleRadians = angleDegrees * Mathf.PI / 180;
        performRayCasts(rayCastResolution);
        renderVision();
    }

    void performRayCasts(int x)
    {
        Vector3[] rayDirections = new Vector3[x];
        Vector3 forward = transform.right;

        float angleStep = angleRadians / (x - 1);

        for (int i = 0; i < x; i++)
        {
            float currentAngle = -angleRadians / 2 + i * angleStep;
            float x_pos = forward.x * Mathf.Cos(currentAngle) - forward.y * Mathf.Sin(currentAngle);
            float y_pos = forward.x * Mathf.Sin(currentAngle) + forward.y * Mathf.Cos(currentAngle);
            rayDirections[i] = new Vector2(x_pos, y_pos);
        }

        points = new Vector2[x + 1];
        points[0] = new Vector2(transform.position.x, transform.position.y);

        for (int i = 0; i < x; i++)
        {
            RaycastHit2D hit;
            if (hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), new Vector2(rayDirections[i].x, rayDirections[i].y), visionRange))
            {
                points[i + 1] = hit.point;
                Debug.Log("Hit at direction index: " + i);
                if (hit.collider.gameObject.tag == "Player") {
                    playerSpotted();
                }
            }
            else
            {
                points[i + 1] = transform.position + rayDirections[i] * visionRange;
            }
        }
    }

    void renderVision() {
        // Create the outline using LineRenderer
        if (points.Length < 3)
        {
            Debug.LogError("At least 3 points are required to form a shape.");
            return;
        }

        // Create the outline using LineRenderer
        lineRenderer.positionCount = points.Length;
        lineRenderer.loop = true;
        lineRenderer.useWorldSpace = true;

        for (int i = 0; i < points.Length; i++)
        {
            lineRenderer.SetPosition(i, new Vector3(points[i].x, points[i].y, 0));
        }

    }

    void playerSpotted()
    {

    }
}
