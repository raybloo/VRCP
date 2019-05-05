using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphDrawer : MonoBehaviour
{

    // Public Fields
    public LineRenderer lineRenderer;

    // Private Fields
    private int maxSize = 1000;
    private int size;
    private Vector3[] pos;
    private float widthFactor = 1.0f;
    private float widthLimit = 2.0f;
    private Vector3 origin = new Vector3(1.0f,0.0f,0.0f);

    // Start is called before the first frame update
    void Start()
    {
        size = 0;
        pos = new Vector3[maxSize];
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGraph(Mathf.Sin(Time.time),Time.deltaTime);
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
        size = Mathf.Min(maxSize, size);
        for(int i = 0; i < size; i++) {
            pos[i].x -= offset;
            if(pos[i].x < origin.x-widthLimit) {
                size = i;
            }
        }
        lineRenderer.positionCount = size;
        lineRenderer.SetPositions(pos);
    }

    void UpdateGraph(float y, float x) 
    {
        MoveGraph(x);
        AddPoint(new Vector3(origin.x,origin.y+y,origin.z));
    }

}
