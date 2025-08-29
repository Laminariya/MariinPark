using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SendEmailPanel : MonoBehaviour
{
    
    public Button CloseButton;
    public TMP_InputField EmailInputField;

    private GameManager _manager;
    private MyRealtyObject _myRealtyObject;
    private KeyboardPanel KeyboardPanel;
    
    public void Init()
    {
        _manager = GameManager.instance;
        CloseButton.onClick.AddListener(OnClose);
        KeyboardPanel = GetComponentInChildren<KeyboardPanel>(true);
        KeyboardPanel.Init();
        OnClose();
    }

    public void Activate(MyRealtyObject myRealtyObject)
    {
        _myRealtyObject = myRealtyObject;
        gameObject.SetActive(true);
        EmailInputField.text = "";
        KeyboardPanel.Show();
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void OnSend()
    {
        _manager.emailSender.Send(_myRealtyObject, EmailInputField.text);
        Deactivate();
    }

    private void OnClose()
    {
        gameObject.SetActive(false);   
    }

}
