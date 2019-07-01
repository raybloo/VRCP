using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class SensorMeasure : MonoBehaviour
{
    SerialPort port = new SerialPort("/dev/ttyACM0", 115200);
    public float factor = 1.0f;
    public float display_value = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        display_value = 0.0f;
        port.Open();
    }

    // Update is called once per frame
    void Update()
    {
        string read_value = port.ReadLine();
        float sensor_value = float.Parse(read_value);
        if (sensor_value != 0) {
            display_value = sensor_value * factor;
            Debug.Log("Value of the sensor is: " + display_value);
        }
    }
}
