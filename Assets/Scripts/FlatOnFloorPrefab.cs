using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FlatOnFloorPrefab : MonoBehaviour
{

    public TMP_Text TypeNumber;
    public TMP_Text AreaFloor;
    public Image Plane;
    public TMP_Text OldPrice;
    public TMP_Text Price;
    
    private MyRealtyObject _myRealtyObject;
    private GameManager _manager;
    private ISendMessageOnComPort _sendMessageOnComPort;
    
    public void Init(MyRealtyObject myRealtyObject, ISendMessageOnComPort sendMessageOnComPort)
    {
        _sendMessageOnComPort = sendMessageOnComPort;
        GetComponent<Button>().onClick.AddListener(OnClick);
        _manager = GameManager.instance;
        _myRealtyObject = myRealtyObject;
        TypeNumber.text = _manager.GetTypeFlat(myRealtyObject.RealtyObject.rooms) + " №" +
                          _myRealtyObject.RealtyObject.number;
        AreaFloor.text = _myRealtyObject.RealtyObject.area + " м" + _manager.SymvolQuadro + " | " +
                         _myRealtyObject.RealtyObject.floor + " этаж";
        OldPrice.text = "";
        Price.text = _manager.GetSplitPrice(_myRealtyObject.RealtyObject.price) + " " + _manager.SymvolRuble;
        Plane.sprite = _myRealtyObject.FlatSprite;
        Plane.preserveAspect = true;
    }

    private void OnClick()
    {
        GameManager.instance.cartFlatPanel.Show(_myRealtyObject, _sendMessageOnComPort);
    }

    public void OnSendMessageOnComPort()
    {
        GameManager.instance.MessageOnFlat(_myRealtyObject.Korpus,1,_myRealtyObject.RealtyObject.number);
    }
    
}
