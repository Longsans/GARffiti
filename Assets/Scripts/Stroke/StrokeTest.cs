using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrokeTest : MonoBehaviour
{
    public GameObject brushPrefab;
    public GameObject brush;
    public bool completed = false;

    private Vector3 oldPos;
    Stroke spaceStroke;

    private void Start()
    {
        brush = GameObject.Instantiate(brushPrefab, transform.position, Quaternion.identity);
        spaceStroke = new SpaceStroke(brush);
        oldPos = transform.position;
        spaceStroke.DrawTo(transform.position);
    }

    private void Update()
    {
        if ((transform.position - oldPos).magnitude > 0.3)
        {
            spaceStroke.DrawTo(transform.position);
            oldPos = transform.position;
        }

        if (completed)
        {
            completed = false;
            spaceStroke.Finished();
        }
    }
}
