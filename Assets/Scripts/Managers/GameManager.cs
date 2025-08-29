using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;
using System.Threading.Tasks;
using TMPro;
    

public class GameManager : MonoBehaviour
{
    
    public static GameManager instance;

    public Color ActiveColor;
    public GameObject StartPanel;
    public TMP_Text InfoStartPanel;
    
    [HideInInspector] public MainPanel mainPanel;
    [HideInInspector] public SerializeJson serializeJson;
    [HideInInspector] public CreateMyData createMyData;
    [HideInInspector] public CreateImagePNG createImagePNG;
    [HideInInspector] public KorpusPanel korpusPanel;
    [HideInInspector] public CartFlatPanel cartFlatPanel;
    [HideInInspector] public PrintingManager printingManager;
    [HideInInspector] public EmailSender emailSender;
    [HideInInspector] public SendEmailPanel sendEmailPanel;
    [HideInInspector] public ChoseFlatOnParameter choseFlatOnParameter;
    [HideInInspector] public SendComPort sendComPort;
    [HideInInspector] public FloorPanel floorPanel;
    
    public MyData MyData;

    [HideInInspector] public string SymvolQuadro = "<sup>2</sup>";
    [HideInInspector] public string SymvolRuble = "\u20BD";

    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

    void Start()
    {
        StartPanel.SetActive(true);
        LoadJson();
    }

    private async Task LoadJson()
    {
        serializeJson = FindObjectOfType<SerializeJson>();
        await serializeJson.Init();
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        mainPanel = FindObjectOfType<MainPanel>(true);
        createMyData = FindObjectOfType<CreateMyData>(true);
        createImagePNG = FindObjectOfType<CreateImagePNG>(true);
        //korpusPanel = FindObjectOfType<KorpusPanel>(true);
        cartFlatPanel = FindObjectOfType<CartFlatPanel>(true);
        printingManager = FindObjectOfType<PrintingManager>(true);
        emailSender = FindObjectOfType<EmailSender>(true);
        sendEmailPanel = FindObjectOfType<SendEmailPanel>(true);
        choseFlatOnParameter = FindObjectOfType<ChoseFlatOnParameter>(true);
        sendComPort = FindObjectOfType<SendComPort>(true);
        floorPanel = FindObjectOfType<FloorPanel>(true);
        
        mainPanel.Init();
        //korpusPanel.Init();
        cartFlatPanel.Init();
        printingManager.Init();
        emailSender.Init();
        sendEmailPanel.Init();
        choseFlatOnParameter.Init();
        sendComPort.Init();        
        floorPanel.Init();

        yield return StartCoroutine(createMyData.Init());
        yield return StartCoroutine(createImagePNG.Init());
        yield return StartCoroutine(createMyData.CreateSprites());
        
        
        StartPanel.SetActive(false);
        MessageOnDemo();
        yield return null;
    }

    public string GetTypeFlat(int rooms)
    {
        switch (rooms)
        {
            case 0:
            {
                return "Студия";
            }
            case 1:
            {
                return "Однокомнатная";
            }
            case 2:
            {
                return "Двухкомнатная";
            }
            case 3:
            {
                return "Трёхкомнатная";
            }
            case 4:
            {
                return "Четырёхкомнатная";
            }
            case 5:
            {
                return "Пятикомнатная";
            }
            default:
            {
                return "";
            }
        }
    }

    public string GetSplitPrice(int price)
    {
        string result = price.ToString();
        int count = result.Length;

        if (count > 3)
            result = result.Insert(result.Length - 3, " ");
        if(count > 6)
            result = result.Insert(result.Length - 7, " ");
        if(count > 9)
            result = result.Insert(result.Length - 11, " ");
        return result;
    }

    public string GetShortPrice(int price)
    {
        string p = (price / 1000000f).ToString();
        if(p.Length>=4)
            p = p.Substring(0, 4);
        return p;
    }

    public void MessageOnHouse(int house, int porch, bool isOn = true)
    {
        //Debug.Log(house+" " + porch);
        //HH02PP0300000000
        string str = house.ToString("X");
        if(str.Length==1) str = "0" + str;
        str += "02";
        string por = porch.ToString("X");
        if(por.Length==1) por = "0" + por;
        str += por;
        if (isOn) str += "0300000000";
        else str += "0000000000";
        Debug.Log("Mess House");
        sendComPort.AddMessage(str);
    }

    public void MessageOnFlat(int house, int porch, int flat, bool isOn = true)
    {
        //HH01FFFF03000000
        string str = house.ToString("X");
        if(str.Length==1) str = "0" + str;
        str += "01";
        string f = flat.ToString("X");
        if (f.Length == 1) f = "000" + f;
        else if (f.Length == 2) f = "00" + f;
        else if (f.Length == 3) f = "0" + f;
        if (isOn) f += "03000000";
        else f += "00000000";
        str += f;
        Debug.Log("Mess Flat");
        sendComPort.AddMessage(str);
    }

    public void MessageOnFloor(int house, int porch, int floor)
    {
        //HH03SSXX03000000
        string str = house.ToString("X");
        if(str.Length==1) str = "0" + str;
        str += "03";
        string f = floor.ToString("X");
        if (f.Length == 1) f = "0" + f;
        str += f;
        string s = porch.ToString("X");
        if (s.Length == 1) s = "0" + s;
        str += s + "03000000";
        Debug.Log("Mess Floor");
        sendComPort.AddMessage(str);
    }

    public void MessageOffAllLight()
    {
        Debug.Log("Mess OffAll");
        sendComPort.AddMessage("007F060100000000"); //Погасить всё!!!
    }

    public void MessageOnDemo()
    {
        Debug.Log("Mess Demo");
        sendComPort.AddMessage("0064010000000000"); //Включить демо!
    }

    public void OnLogoClicked()
    {
        mainPanel.OnBackMainPage();
        floorPanel.Hide();
        choseFlatOnParameter.OnClose();
        cartFlatPanel.Hide();
    }

}
