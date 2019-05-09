﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
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
    public string LogPath = "Assets/Logs/massageLog.csv"; 

    // Private Fields
    private LineRenderer lineRenderer;
    private int maxSize = 10000;
    private int size;
    private Vector3[] pos;
    StreamWriter logWriter;

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
        logWriter = new StreamWriter(LogPath,false);
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateGraph(Mathf.Sin(Time.time),Time.deltaTime);
    }

    private void OnApplicationQuit() {
        logWriter.Close();
        AssetDatabase.ImportAsset(LogPath);
    }

    void AddPoint(Vector3 point,float offset) 
    {
        if (size < maxSize) {
            size++;
        }
        MoveGraph(offset);
        for (int i = size - 1; i > 0; i--) {
            pos[i] = pos[i - 1];
        }
        pos[0] = point;
    }

    void MoveGraph(float offset) 
    {
        //size = Mathf.Min(maxSize, size);
        for(int i = 0; i < size; i++) {
            pos[i].x -= offset*widthFactor;
            if(pos[i].x < origin.x-widthLimit) {
                size--;
            }
        }
    }

    public void UpdateGraph(float y, float x) 
    {
        //MoveGraph(x);
        AddPoint(new Vector3(origin.x,origin.y-(y*heightFactor),origin.z),x);
        //size = Mathf.Min(maxSize, size);
        lineRenderer.positionCount = size;
        lineRenderer.SetPositions(pos);
        Log(y);
    }

    public void UpdateThreshold(float newVal) 
    {
        thresholdValue = newVal;
        if (threshold) {
            Vector3[] t_pos = new Vector3[] { new Vector3(origin.x, origin.y - (thresholdValue * heightFactor), origin.z), new Vector3(origin.x - widthLimit, origin.y - (thresholdValue * heightFactor), origin.z) };
            threshold.positionCount = 2;
            threshold.SetPositions(t_pos);
        }
    }

    public void Log(float value) {
        logWriter.WriteLine(Time.time+","+value);
    }

}
