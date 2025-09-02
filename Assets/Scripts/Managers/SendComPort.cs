using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Management;
using System.Threading.Tasks;

public class SendComPort : MonoBehaviour
{

    public TMP_InputField InputPort;
    public Button b_Start;
    public TMP_Text Text;

    public Button b_TestChar;
    //public Button b_TestByte;
    private Queue<string> _queue = new Queue<string>();
    
    private SerialPort mySerialPort;
    private bool _isSend;
    private string _read;
    private Coroutine _coroutine;

    public void Init()
    {
        //b_TestChar.onClick.AddListener(()=>MySendMessage("0064010000000000"));
//#if !UNITY_EDITOR
        CreatPort();
        _isSend = true;
//#endif
        //b_Start.onClick.AddListener(CreatPort);
    }

    private void OnDestroy()
    {
//#if !UNITY_EDITOR
        mySerialPort.Close();
//#endif
    }
    
    private void Update()
    {
// #if UNITY_EDITOR
//         return;
// #endif
        if (_isSend && _queue.Count > 0)
        {
            if (mySerialPort == null || !mySerialPort.IsOpen)
            {
                if (mySerialPort == null)
                {
                    CreatPort();
                }
                else
                {
                    mySerialPort.Open();
                }
            }
            else
            {
                MySendMessage(_queue.Dequeue());
            }
        }
    }

    private void CreatPort() //Открываем порт
    {
        mySerialPort = new SerialPort();
        //Debug.Log(mySerialPort.PortName + " // " + InputField.text);

// #if UNITY_EDITOR
//         mySerialPort.PortName ="XXX"+ CheckPorts(); //Устанавливаем номер порта, который будем открывать.
//         Debug.LogError("Com port Editor");
// #endif
//#if !UNITY_EDITOR
        mySerialPort.PortName =CheckPorts(); //Устанавливаем номер порта, который будем открывать.  
//#endif
        mySerialPort.BaudRate = 115200;
        mySerialPort.Parity = Parity.None;
        mySerialPort.StopBits = StopBits.One;
        mySerialPort.DataBits = 8;
        mySerialPort.Handshake = Handshake.None;
        mySerialPort.RtsEnable = true;
        //mySerialPort.ReadBufferSize = 100;
        //mySerialPort.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);
        try
        {
            mySerialPort.Open(); //Открываем порт
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        mySerialPort.ReadTimeout = 100;
    }
    
    private string CheckPorts() //Проверяем есть ли COM-порт с подключённым устройством.
    {
        string[] ports = SerialPort.GetPortNames();

        Debug.Log(ports.Length);
        
        foreach(string port in ports)
        {
            Debug.Log(port);
            return port; //Берём первый существующий порт
        }
        return "COM1";
    }

    private async Task MySendMessage(string str)
    {
        _isSend = false;
        Debug.Log(str);
        string mail = str + "\r\n";

        try
        {
            mySerialPort.Write(mail);
        }
        catch (Exception e)
        {
            Debug.Log(e);
            throw;
        }

        await Task.Delay(50);
        _isSend = true;
        //mySerialPort.DiscardInBuffer();
        //mySerialPort.DiscardOutBuffer();
    }

    public void AddMessage(string message)
    {
        _queue.Enqueue(message);
    }

}