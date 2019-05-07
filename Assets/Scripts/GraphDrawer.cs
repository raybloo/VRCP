using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphDrawer : MonoBehaviour
{

    // Public Fields
    
    public Vector3 origin = new Vector3(-2.5f, 1.5f, -0.01f);
    public float widthFactor = 2.0f;
    public float widthLimit = 6.0f;
    public float heightFactor = 0.6f;
    public float thresholdValue;
    public LineRenderer threshold;

    // Private Fields
    private LineRenderer lineRenderer;
    private int maxSize = 1000;
    private int size;
    private Vector3[] pos;
    
    // Start is called before the first frame update
    void Start()
    {
        size = 0;
        pos = new Vector3[maxSize];
        lineRenderer = GetComponent<LineRenderer>();
        if(threshold) {
            Vector3[] t_pos = new Vector3[] { new Vector3(origin.x, origin.y - thresholdValue, origin.z), new Vector3(origin.x + widthLimit, origin.y - thresholdValue, origin.z) };
            threshold.positionCount = 2;
            threshold.SetPositions(t_pos);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateGraph(Mathf.Sin(Time.time),Time.deltaTime);
    }

    void AddPoint(Vector3 point) 
    {
        if(size >= maxSize) {
            for(int i = 1; i < maxSize; i++) {
                pos[i - 1] = pos[i];
            }
        } else {
            size++;
        }
        pos[size - 1] = point;
    }

    void MoveGraph(float offset) 
    {
        //size = Mathf.Min(maxSize, size);
        for(int i = 0; i < size; i++) {
            pos[i].x += offset*widthFactor;
            if(pos[i].x > origin.x+widthLimit) {
                maxSize = size;
            }
        }
        size = Mathf.Min(maxSize, size);
        lineRenderer.positionCount = size;
        lineRenderer.SetPositions(pos);
    }

    public void UpdateGraph(float y, float x) 
    {
        MoveGraph(x);
        AddPoint(new Vector3(origin.x,origin.y-(y*heightFactor),origin.z));
    }

    public void UpdateThreshold(float newVal) 
    {
        thresholdValue = newVal;
        if (threshold) {
            Vector3[] t_pos = new Vector3[] { new Vector3(origin.x, origin.y - (thresholdValue * heightFactor), origin.z), new Vector3(origin.x + widthLimit, origin.y - (thresholdValue * heightFactor), origin.z) };
            threshold.positionCount = 2;
            threshold.SetPositions(t_pos);
        }
    }

}
