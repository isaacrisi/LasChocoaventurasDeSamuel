using System;
using System.IO.Ports;
using UnityEngine;

public class FloatCommunication : MonoBehaviour
{
    SerialPort serialPort = new SerialPort("COM5", 115200);

    public GameObject cube;
    private float receivedFloat;

    void Start()
    {
        try
        {
            if (!serialPort.IsOpen)
            {
                serialPort.Open();
                serialPort.ReadTimeout = 1000;
                Debug.Log("Puerto serial abierto exitosamente.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error al abrir el puerto serial: " + e.Message);
        }
    }

    void Update()
    {
        if (serialPort.IsOpen)
        {
            try
            {
                // Envía el carácter 's' al Raspberry Pi Pico
                if (Time.frameCount % 60 == 0)
                {
                    serialPort.Write("s");
                    Debug.Log("Enviado 's' al microcontrolador.");
                }

                // Verifica si hay algún dato disponible
                if (serialPort.BytesToRead > 0)
                {
                    int data = serialPort.ReadByte();  // Lee un byte
                    Debug.Log("Byte recibido: " + data);  // Muestra el byte en consola
                }
            }
            catch (TimeoutException)
            {
                Debug.LogWarning("Tiempo de espera excedido al leer del puerto serial.");
            }
            catch (Exception e)
            {
                Debug.LogError("Error en la comunicación serial: " + e.Message);
            }
        }
    }

    private void OnApplicationQuit()
    {
        if (serialPort.IsOpen)
        {
            serialPort.Close();
            Debug.Log("Puerto serial cerrado.");
        }
    }
}
