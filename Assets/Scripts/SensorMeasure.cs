using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.IO.Ports;
using UnityEngine;

public class SensorMeasure : MonoBehaviour
{
    SerialPort port = new SerialPort("COM9", 115200);
    Thread thread;
    bool running = false;
    public float bias = 2.0f;
    public float factor = 1.0f;
    public float display_value = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
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
        while(running) {
            string read_value = port.ReadLine();
            float sensor_value;
            if (float.TryParse(read_value,out sensor_value)) {
                display_value = (sensor_value * factor)-bias;
                //Debug.Log("Value of the sensor is: " + display_value);
            }
        }
    }

    private void OnApplicationQuit() {
        running = false;
    }
}
