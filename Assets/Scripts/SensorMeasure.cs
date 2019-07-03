using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.IO.Ports;
using UnityEngine;
using UnityEditor;


public class SensorMeasure : MonoBehaviour
{
    SerialPort port = new SerialPort("COM9", 115200);
    Thread thread;
    bool running = false;
    string logPath;
    StreamWriter logWriter;

    public float bias = 2.0f;
    public float factor = 1.0f;
    public float display_value = 0.0f;
    public bool logging = false;
    public System.DateTime start;


    // Start is called before the first frame update
    void Start()
    {
        start = System.DateTime.Now;
        //logPath = "Assets/Logs/sensor_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".csv";
        logPath = "Assets/Logs/sensor_log.csv";
        logWriter = new StreamWriter(logPath, false);
        display_value = 0.0f;
        port.Open();
        running = true;
        thread = new Thread(LoopingRead);
        thread.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    void LoopingRead() {
        while (running) {
            string read_value = port.ReadLine();
            float sensor_value;
            if (float.TryParse(read_value,out sensor_value)) {
                display_value = (sensor_value * factor)-bias;
                if(logging) {
                    float time = (System.DateTime.Now - start).Ticks * 0.0000001f;
                    logWriter.WriteLine(time + "," + display_value);
                }
            }
        }
    }

    private void OnApplicationQuit() {
        running = false;
        logWriter.Close();
        AssetDatabase.ImportAsset(logPath);
    }
}
