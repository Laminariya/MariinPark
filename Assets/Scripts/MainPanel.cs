using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour
{
    
    public Sprite s_MainPage;
    public Sprite s_Korpus1;
    public Sprite s_Korpus2;
    public Sprite s_ArhitekOblik;
    public Sprite s_AkcentPodsvetka;
    public Sprite s_Blagoustroistvo;
    public Sprite s_Lobby;
    public Sprite s_Kvartirigraf;
    public Sprite s_RedkieFormati;
    public Sprite s_Infrastruktura;

    public Button b_Back;
    public Button b_Korpus1;
    public Button b_Korpus2;
    public Button b_Korpus1_OnPanel;
    public Button b_Korpus2_OnPanel;
    public Button b_ArhitekOblik;
    public Button b_AkcentPodsvetka;
    public Button b_Blagoustroistvo;
    public Button b_Lobby;
    public Button b_Kvartirigraf;
    public Button b_Okruzhenie;
    public Button b_ChoseFlatOnParametrs;
    public Button b_Infrastruktura;

    public GameObject ChoseFloorKorpus1;
    public GameObject ChoseFloorKorpus2;
    public Transform FloorsParent1;
    public Transform FloorsParent2;
    public GameObject FloorButtonPrefab;

    public Button b_ShowFlatKorpus1;
    public Button b_ShowFlatKorpus2;
    
    public List<GameObject> LightFloors1 = new List<GameObject>();
    public List<GameObject> LightFloors2 = new List<GameObject>();

    private Image _image;
    private List<FloorButtonPrefab> _floors = new List<FloorButtonPrefab>();
    private MyBuilding _currentBuilding;
    private int _currentFloor;
    
    public void Init()
    {
        _image = GetComponent<Image>();
       b_Back.onClick.AddListener(OnBackMainPage);
       b_Korpus1.onClick.AddListener(OnKorpus1);
       b_Korpus2.onClick.AddListener(OnKorpus2);
       b_Korpus1_OnPanel.onClick.AddListener(OnKorpus1);
       b_Korpus2_OnPanel.onClick.AddListener(OnKorpus2);
       b_ArhitekOblik.onClick.AddListener(OnArhitekOblik);
       b_Blagoustroistvo.onClick.AddListener(OnBlagoustroistvo);
       b_Kvartirigraf.onClick.AddListener(OnKvartirigraf);
       b_Okruzhenie.onClick.AddListener(OnOkruzhenie);
       b_ChoseFlatOnParametrs.onClick.AddListener(OnChoseFlatOnParametrs);
       b_AkcentPodsvetka.onClick.AddListener(OnAkcentPodsvetka);
       b_Lobby.onClick.AddListener(OnLobby);
       b_Infrastruktura.onClick.AddListener(OnInfrastruktura);
       b_ShowFlatKorpus1.onClick.AddListener(OnShowFlatKorpus1);
       b_ShowFlatKorpus2.onClick.AddListener(OnShowFlatKorpus2);
       OffLight();
       OnBackMainPage();
       
    }

    public void OnBackMainPage()
    {
        _image.sprite = s_MainPage;
        ChoseFloorKorpus1.SetActive(false);
        ChoseFloorKorpus2.SetActive(false);
        b_Korpus1_OnPanel.gameObject.SetActive(true);
        b_Korpus2_OnPanel.gameObject.SetActive(true);
        foreach (var light in LightFloors2)
        {
            light.SetActive(false);   
        }
        foreach (var light in LightFloors1)
        {
            light.SetActive(false);   
        }
        GameManager.instance.MessageOffAllLight();
        GameManager.instance.MessageOnDemo();
    }

    public void OnKorpus1()
    {
        OffLight();
        _image.sprite = s_Korpus1;
        ChoseFloorKorpus2.SetActive(false);
        ChoseFloorKorpus1.SetActive(true);
        b_Korpus1_OnPanel.gameObject.SetActive(false);
        b_Korpus2_OnPanel.gameObject.SetActive(false);

        for (int i = 0; i < _floors.Count; i++)
        {
            Destroy(_floors[i].gameObject);
        }
        _floors.Clear();
        
        foreach (var myBuilder in GameManager.instance.MyData.MyBuilders)
        {
            if (myBuilder.Korpus == 1)
            {
                _currentBuilding = myBuilder;
                foreach (var floor in myBuilder.Floors)
                {
                    FloorButtonPrefab newFloor =
                        Instantiate(FloorButtonPrefab, FloorsParent1).GetComponent<FloorButtonPrefab>();
                    newFloor.Init(floor);
                    newFloor.Button.onClick.AddListener(() => OnClickFloor1(floor, newFloor));
                    _floors.Add(newFloor);
                }
            }
        }
        b_ShowFlatKorpus1.gameObject.SetActive(false);
        b_ShowFlatKorpus2.gameObject.SetActive(false);
        
        GameManager.instance.MessageOffAllLight();
        GameManager.instance.MessageOnHouse(1,1);
    }

    public void OnKorpus2()
    {
        OffLight();
        _image.sprite = s_Korpus2;
        ChoseFloorKorpus1.SetActive(false);
        ChoseFloorKorpus2.SetActive(true);
        b_Korpus1_OnPanel.gameObject.SetActive(false);
        b_Korpus2_OnPanel.gameObject.SetActive(false);
        
        for (int i = 0; i < _floors.Count; i++)
        {
            Destroy(_floors[i].gameObject);
        }
        _floors.Clear();
        
        foreach (var myBuilder in GameManager.instance.MyData.MyBuilders)
        {
            //Debug.Log(myBuilder.Korpus);
            if (myBuilder.Korpus == 2)
            {
                _currentBuilding = myBuilder;
                foreach (var floor in myBuilder.Floors)
                {
                    FloorButtonPrefab newFloor =
                        Instantiate(FloorButtonPrefab, FloorsParent2).GetComponent<FloorButtonPrefab>();
                    newFloor.Init(floor);
                    newFloor.Button.onClick.AddListener(() => OnClickFloor2(floor, newFloor));
                    _floors.Add(newFloor);
                }
            }
        }
        b_ShowFlatKorpus1.gameObject.SetActive(false);
        b_ShowFlatKorpus2.gameObject.SetActive(false);
        
        GameManager.instance.MessageOffAllLight();
        GameManager.instance.MessageOnHouse(2,1);
    }

    private void OnClickFloor1(int floor, FloorButtonPrefab floorButtonPrefab)
    {
        _currentFloor = floor;
        foreach (var floorButton in _floors)
        {
            floorButton.Deactive();
        }
        floorButtonPrefab.Active();
        foreach (var light in LightFloors1)
        {
            light.SetActive(false);   
        }
        LightFloors1[LightFloors1.Count+1-floor].SetActive(true);
        b_ShowFlatKorpus1.gameObject.SetActive(true);
        
        GameManager.instance.MessageOffAllLight();
        GameManager.instance.MessageOnFloor(1,1,_currentFloor);
    }
    
    private void OnClickFloor2(int floor, FloorButtonPrefab floorButtonPrefab)
    {
        _currentFloor = floor;
        foreach (var floorButton in _floors)
        {
            floorButton.Deactive();
        }
        floorButtonPrefab.Active();
        foreach (var light in LightFloors2)
        {
            light.SetActive(false);   
        }
        LightFloors2[LightFloors2.Count+1-floor].SetActive(true);
        b_ShowFlatKorpus2.gameObject.SetActive(true);
        
        GameManager.instance.MessageOffAllLight();
        GameManager.instance.MessageOnFloor(2,1,_currentFloor);
    }

    private void OffLight()
    {
        foreach (var light in LightFloors1)
        {
            light.SetActive(false);
        }
        foreach (var light in LightFloors2)
        {
            light.SetActive(false);
        }
    }

    private void OnArhitekOblik()
    {
        _image.sprite = s_ArhitekOblik;
        CloseLightFloorPanel();
        //GameManager.instance.MessageOffAllLight();
        GameManager.instance.MessageOnDemo();
    }

    private void OnAkcentPodsvetka()
    {
        _image.sprite = s_AkcentPodsvetka;
        CloseLightFloorPanel();
        //GameManager.instance.MessageOffAllLight();
        GameManager.instance.MessageOnDemo();
    }

    private void OnBlagoustroistvo()
    {
        _image.sprite = s_Blagoustroistvo;
        CloseLightFloorPanel();
        //GameManager.instance.MessageOffAllLight();
        GameManager.instance.MessageOnDemo();
    }

    private void OnKvartirigraf()
    {
        _image.sprite = s_Kvartirigraf;
        CloseLightFloorPanel();
        //GameManager.instance.MessageOffAllLight();
        GameManager.instance.MessageOnDemo();
    }

    private void OnOkruzhenie()
    {
        _image.sprite = s_RedkieFormati;
        CloseLightFloorPanel();
        //GameManager.instance.MessageOffAllLight();
        GameManager.instance.MessageOnDemo();
    }
    
    private void OnInfrastruktura()
    {
        _image.sprite = s_Infrastruktura;
        CloseLightFloorPanel();
        //GameManager.instance.MessageOffAllLight();
        GameManager.instance.MessageOnDemo();
    }

    private void OnChoseFlatOnParametrs()
    {
        Debug.Log("OnChoseFlatOnParametrs");
    }

    private void OnLobby()
    {
        _image.sprite = s_Lobby;
        CloseLightFloorPanel();
        //GameManager.instance.MessageOffAllLight();
        GameManager.instance.MessageOnDemo();
    }

    private void CloseLightFloorPanel()
    {
        ChoseFloorKorpus1.SetActive(false);
        ChoseFloorKorpus2.SetActive(false);
        OffLight();
    }

    private void OnShowFlatKorpus1()
    {
        GameManager.instance.floorPanel.Show(_currentBuilding, _currentFloor);
    }

    private void OnShowFlatKorpus2()
    {
        GameManager.instance.floorPanel.Show(_currentBuilding, _currentFloor);
    }

}
