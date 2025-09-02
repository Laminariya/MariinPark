using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CartFlatPanel : MonoBehaviour
{
    public Button b_Back;
    public Button b_Email;
    public Button b_Printing;
    public Button b_ChoseFlatOnParametr;
    public TMP_Text TypeNumberArea;
    public TMP_Text Price;
    public TMP_Text OldPrice;
    public TMP_Text PriceOnMetr;
    public TMP_Text Korpus;
    public Image Image;
    public Button b_PlaneFlat;
    public Button b_PlaneFloor;
    public Image SwitchButton;
    public Sprite FlatButtonImage;
    public Sprite FloorButtonImage;

    private GameManager _manager;
    private MyRealtyObject _myRealtyObject;
    private ISendMessageOnComPort _sendMessageOnComPort;
    
    public void Init()
    {
        _manager = GameManager.instance;
        b_Back.onClick.AddListener(OnBack);
        b_Email.onClick.AddListener(OnEmail);
        b_Printing.onClick.AddListener(OnPrinting);
        b_PlaneFlat.onClick.AddListener(OnPlaneFlat);
        b_PlaneFloor.onClick.AddListener(OnPlaneFloor);
        b_ChoseFlatOnParametr.onClick.AddListener(OnChoseFlatOnParametr);
        Hide();
    }

    public void Show(MyRealtyObject myRealtyObject, ISendMessageOnComPort sendMessageOnComPort)
    {
        _sendMessageOnComPort = sendMessageOnComPort;
        _myRealtyObject = myRealtyObject;

        TypeNumberArea.text = _manager.GetTypeFlat(_myRealtyObject.RealtyObject.rooms) + " №" +
                              _myRealtyObject.RealtyObject.number + ", " + _myRealtyObject.RealtyObject.area + " м" +
                              _manager.SymvolQuadro;
        Price.text = _manager.GetSplitPrice(_myRealtyObject.RealtyObject.price) + " " + _manager.SymvolRuble;
        OldPrice.text = "";
        int priceOnMeter = (int) (_myRealtyObject.RealtyObject.price / _myRealtyObject.RealtyObject.area);
        PriceOnMetr.text = _manager.GetSplitPrice(priceOnMeter) + " " + _manager.SymvolRuble + "/м" +
                           _manager.SymvolQuadro;
        Korpus.text = "Корпус " + _myRealtyObject.Korpus + " | Секция " + _myRealtyObject.Section + " | Этаж " +
                      _myRealtyObject.RealtyObject.floor;
        Image.sprite = _myRealtyObject.FlatSprite;
        Image.preserveAspect = true;
        OnPlaneFlat();
        gameObject.SetActive(true);
        
        GameManager.instance.MessageOffAllLight();
        GameManager.instance.MessageOnFlat(_myRealtyObject.Korpus,1,_myRealtyObject.RealtyObject.number);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnBack()
    {
        Hide();
       _sendMessageOnComPort.SendMessageOnComPort();
    }

    private void OnEmail()
    {
        GameManager.instance.sendEmailPanel.Activate(_myRealtyObject);
        Debug.Log("Email");
    }

    private void OnPrinting()
    {
        GameManager.instance.printingManager.Print(_myRealtyObject);
        Debug.Log("Printing");
    }

    private void OnPlaneFlat()
    {
        Image.sprite = _myRealtyObject.FlatSprite;
        SwitchButton.sprite = FlatButtonImage;
    }

    private void OnPlaneFloor()
    {
        Image.sprite = _myRealtyObject.FloorSprite;
        SwitchButton.sprite = FloorButtonImage;
    }

    private void OnChoseFlatOnParametr()
    {
        GameManager.instance.choseFlatOnParameter.Show();
        Hide();
    }

}
