using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KorpusPanel : MonoBehaviour, ISendMessageOnComPort
{

    public Button b_Back;
    public Transform ButtonFloorParent;
    public GameObject FloorPrefab;
    public GameObject FloorPanel;
    public Transform FlatParent;
    public GameObject FlatPrefab;
    public Button b_FloorBack;
    public Scrollbar Scrollbar;
    public GridLayoutGroup GridLayoutGroup;
    
    public List<Sprite> Sprites = new List<Sprite>();

    private Image _image;
    private int _currentKorpus;
    private int _currentFloor;
    private List<FloorPrefab> _floorButtons = new List<FloorPrefab>();
    private List<FlatOnFloorPrefab> _flatOnFloorPrefabs = new List<FlatOnFloorPrefab>();

    public void Init()
    {
        _image = GetComponent<Image>();
        b_Back.onClick.AddListener(OnBack);
        b_FloorBack.onClick.AddListener(OnBackFloor);
        
        Hide();
    }

    public void Show(int korpus)
    {
        gameObject.SetActive(true);
        _image.sprite = Sprites[korpus - 1];
        _currentKorpus = korpus;
        FloorPanel.SetActive(false);
        LoadFloors();
        
        GameManager.instance.MessageOffAllLight();
        GameManager.instance.MessageOnHouse(korpus, 1);
    }

    public void ShowParking()
    {
        Debug.Log("Show Parking");
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnBack()
    {
        Hide();
        GameManager.instance.MessageOffAllLight();
        GameManager.instance.MessageOnDemo();
    }
    
    private void OnBackFloor()
    {
        FloorPanel.SetActive(false);
        GameManager.instance.MessageOffAllLight();
        GameManager.instance.MessageOnHouse(_currentKorpus,1);
    }

    private void LoadFloors()
    {

        for (int i = 0; i < _floorButtons.Count; i++)
        {
            Destroy(_floorButtons[i].gameObject);
        }
        _floorButtons.Clear();
        
        foreach (var myBuilder in GameManager.instance.MyData.MyBuilders)
        {
            if (myBuilder.Korpus == _currentKorpus)
            {
                foreach (var floor in myBuilder.Floors)
                {
                    FloorPrefab floorPrefab = Instantiate(FloorPrefab, ButtonFloorParent).GetComponent<FloorPrefab>();
                    floorPrefab.Init(floor);
                    floorPrefab.Button.onClick.AddListener(()=>OnClickFloor(floor));
                    _floorButtons.Add(floorPrefab);
                }
            }
        }
        
    }

    private void OnClickFloor(int floor)
    {
        _currentFloor = floor;
        FloorPanel.SetActive(true);
        foreach (var floorPrefab in _floorButtons)
        {
            if (floorPrefab.Floor != floor)
                floorPrefab.OnDeactivate();
        }

        for (int i = 0; i < _flatOnFloorPrefabs.Count; i++)
        {
            Destroy(_flatOnFloorPrefabs[i].gameObject);
        }
        _flatOnFloorPrefabs.Clear();

        foreach (var myBuilder in GameManager.instance.MyData.MyBuilders)
        {
            if (myBuilder.Korpus == _currentKorpus)
            {
                foreach (var myRealtyObject in myBuilder.myRealtyobjects)
                {
                    if (myRealtyObject.RealtyObject.floor == floor)
                    {
                        FlatOnFloorPrefab prefab =
                            Instantiate(FlatPrefab, FlatParent).GetComponent<FlatOnFloorPrefab>();
                        prefab.Init(myRealtyObject);
                        _flatOnFloorPrefabs.Add(prefab);
                    }
                }
            }
        }
        Scrollbar.value = 1f;
        GridLayoutGroup.spacing += Vector2.one;
        Canvas.ForceUpdateCanvases();
        GridLayoutGroup.spacing -= Vector2.one;
        
        GameManager.instance.MessageOffAllLight();
        GameManager.instance.MessageOnFloor(_currentKorpus,1,floor);
    }

    public void SendMessageOnComPort()
    {
        GameManager.instance.MessageOffAllLight();
        GameManager.instance.MessageOnFloor(_currentKorpus,1,_currentFloor);
    }
}
