using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloorPanel : MonoBehaviour
{

    public Button b_Back;
    public Button b_ShowFloor;
    public Button b_ShowListFlat;
    public Transform ListButtonFloorParent;
    public GameObject ButtonFloorPrefab;
    
    public GameObject ListPanel;
    public Transform ListFlatParent;
    public GameObject ListFlatPrefab;

    public Button b_ChoseFlatOnParameterDown;
    public Button b_ChoseFlayOnParameterUp;
    
    private List<FloorButtonOnFloorPanelPrefab> _floorButtonPrefabs = new List<FloorButtonOnFloorPanelPrefab>();
    private List<FlatOnFloorPrefab> _flatButtonPrefabs = new List<FlatOnFloorPrefab>();
    private FloorPlanesClass _floorPlanesClass;
    private MyBuilding _currentBuilding;
    private int _currentFloor;
    
    public void Init()
    {
        _floorPlanesClass = GetComponentInChildren<FloorPlanesClass>(true);
        _floorPlanesClass.Init();
        b_ShowFloor.onClick.AddListener(OnShowFloorPlanes);
        b_ShowListFlat.onClick.AddListener(OnShowListFlat);
        b_ChoseFlatOnParameterDown.onClick.AddListener(GameManager.instance.choseFlatOnParameter.Show);
        b_ChoseFlayOnParameterUp.onClick.AddListener(GameManager.instance.choseFlatOnParameter.Show);
        b_Back.onClick.AddListener(OnBack);
        
        Hide();
    }

    public void Show(MyBuilding building, int floor)
    {
        _currentBuilding = building;
        _currentFloor = floor;
        gameObject.SetActive(true);
        CreateFloorButtonPrefab();
        OnShowFloorPlanes();
        foreach (var buttonPrefab in _floorButtonPrefabs)
        {
            if(buttonPrefab.Floor == floor)
                OnShowFloor(buttonPrefab);
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        _floorPlanesClass.Hide();
    }

    private void OnBack()
    {
        Hide();
    }

    private void OnShowFloorPlanes()
    {
        Debug.Log("OnShowFloorPlanes");
        b_ShowListFlat.gameObject.SetActive(true);
        _floorPlanesClass.gameObject.SetActive(true);
        ListPanel.gameObject.SetActive(false);
        b_ChoseFlayOnParameterUp.gameObject.SetActive(false);
        b_ChoseFlatOnParameterDown.gameObject.SetActive(true);
    }

    private void OnShowListFlat()
    {
        Debug.Log("OnShowListFlat");
        b_ShowListFlat.gameObject.SetActive(false);
        _floorPlanesClass.gameObject.SetActive(false);
        ListPanel.gameObject.SetActive(true);
        b_ChoseFlayOnParameterUp.gameObject.SetActive(true);
        b_ChoseFlatOnParameterDown.gameObject.SetActive(false);
    }

    private void CreateFloorButtonPrefab()
    {
        for (int i = 0; i < _floorButtonPrefabs.Count; i++)
        {
            Destroy(_floorButtonPrefabs[i].gameObject);
        }
        _floorButtonPrefabs.Clear();
        
        foreach (var floor in _currentBuilding.Floors)
        {
            FloorButtonOnFloorPanelPrefab button = Instantiate(ButtonFloorPrefab, ListButtonFloorParent)
                .GetComponent<FloorButtonOnFloorPanelPrefab>();
            button.Init(floor);
            button.Button.onClick.AddListener(() => OnShowFloor(button));
            _floorButtonPrefabs.Add(button);
        }
    }

    private void OnShowFloor(FloorButtonOnFloorPanelPrefab floorButtonPrefab)
    {
        foreach (var buttonPrefab in _floorButtonPrefabs)
        {
            buttonPrefab.Deactive();
        }
        floorButtonPrefab.Active();
        _currentFloor = floorButtonPrefab.Floor;
        _floorPlanesClass.CreateFloor(_currentBuilding.Korpus, _currentFloor);


        foreach (var onFloorPrefab in _flatButtonPrefabs)
        {
            Destroy(onFloorPrefab.gameObject);
        }
        _flatButtonPrefabs.Clear();
        foreach (var myRealtyobject in _currentBuilding.myRealtyobjects)  
        {
            if (myRealtyobject.RealtyObject.floor == _currentFloor)
            {
                FlatOnFloorPrefab prefab = Instantiate(ListFlatPrefab, ListFlatParent).GetComponent<FlatOnFloorPrefab>();
                prefab.Init(myRealtyobject);
                _flatButtonPrefabs.Add(prefab);
            }
        }
        
        GameManager.instance.MessageOffAllLight();
        GameManager.instance.MessageOnFloor(1,1,_currentFloor);
    }

}
