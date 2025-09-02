using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoseFlatOnParameter : MonoBehaviour, ISendMessageOnComPort
{

    public Color ActiveColor;
    
    public DubleSlider DubleSliderArea;
    public DubleSlider DubleSliderPrice;
    public DubleSlider DubleSliderFloor;
    public Button b_Close;
    public Button b_Reset;
    public Button b_ShowFlat;
    public Button b_St;
    public Button b_1;
    public Button b_2;
    public Button b_3;
    public Button b_4;
    public Button b_5;
    public TMP_Text MinArea;
    public TMP_Text MaxArea;
    public TMP_Text MinPrice;
    public TMP_Text MaxPrice;
    public TMP_Text MinFloor;
    public TMP_Text MaxFloor;

    public Transform ParentPrefabFlat;
    public GameObject PrefabFlat;

    private List<FlatOnFloorPrefab> _flatOnFloorPrefabs = new List<FlatOnFloorPrefab>();

    //private int _St;
    private int _1;
    private int _2;
    private int _3;
    private int _4;
    //private int _5;
    
    private float _minArea;
    private float _maxArea;
    private float _minPrice;
    private float _maxPrice;
    private int _minFloor;
    private int _maxFloor;

    public void Init()
    {
        b_St.onClick.AddListener(OnSt);
        b_1.onClick.AddListener(On1);
        b_2.onClick.AddListener(On2);
        b_3.onClick.AddListener(On3);
        b_4.onClick.AddListener(On4);
        b_5.onClick.AddListener(On5);
        b_Close.onClick.AddListener(OnClose);
        b_Reset.onClick.AddListener(OnReset);
        b_ShowFlat.onClick.AddListener(OnShowFlat);
        DubleSliderArea.Action += OnDoubleSliderArea;
        DubleSliderPrice.Action += OnDoubleSliderPrice;
        DubleSliderFloor.Action += OnDoubleSliderFloor;
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        
        for (int i = 0; i < _flatOnFloorPrefabs.Count; i++)
        {
            Destroy(_flatOnFloorPrefabs[i].gameObject);
        }
        _flatOnFloorPrefabs.Clear();
        
        OnReset();
    }

    public void OnClose()
    {
        gameObject.SetActive(false);
        // GameManager.instance.mainPanel.OnBackMainPage();
        // GameManager.instance.floorPanel.Hide();
        // GameManager.instance.cartFlatPanel.Hide();
    }

    public void OnReset()
    {
        //_St = 0;
        _1 = 1;
        _2 = 2;
        _3 = 3;
        _4 = 4;
        //_5 = 5;
        b_St.image.color = ActiveColor;
        b_1.image.color = ActiveColor;
        b_2.image.color = ActiveColor;
        b_3.image.color = ActiveColor;
        b_4.image.color = ActiveColor;
        b_5.image.color = ActiveColor;
        OnSt();
        On1();
        On2();
        On3();
        On4();
        On5();
        DubleSliderArea.LeftSlider.value = 0f;
        DubleSliderArea.RightSlider.value = 1f;
        DubleSliderFloor.LeftSlider.value = 0f;
        DubleSliderFloor.RightSlider.value = 1f;
        DubleSliderPrice.LeftSlider.value = 0f;
        DubleSliderPrice.RightSlider.value = 1f;
        ReloadSliders();
    }

    private void OnShowFlat()
    {
        GameManager.instance.MessageOffAllLight();
        
        for (int i = 0; i < _flatOnFloorPrefabs.Count; i++)
        {
            Destroy(_flatOnFloorPrefabs[i].gameObject);
        }
        _flatOnFloorPrefabs.Clear();
        
        foreach (var building in GameManager.instance.MyData.MyBuilders)
        {
            foreach (var myObject in building.myRealtyobjects)
            {
                if ((myObject.RealtyObject.rooms == _1 ||
                     myObject.RealtyObject.rooms == _2
                     || myObject.RealtyObject.rooms == _3 || myObject.RealtyObject.rooms == _4)
                    && myObject.RealtyObject.area <= _maxArea && myObject.RealtyObject.area >= _minArea &&
                    myObject.RealtyObject.price <= _maxPrice && myObject.RealtyObject.price >= _minPrice &&
                    myObject.RealtyObject.floor <= _maxFloor && myObject.RealtyObject.floor >= _minFloor)
                {
                    FlatOnFloorPrefab flat = Instantiate(PrefabFlat, ParentPrefabFlat)
                        .GetComponent<FlatOnFloorPrefab>();
                    flat.Init(myObject, this);
                    flat.OnSendMessageOnComPort();
                    _flatOnFloorPrefabs.Add(flat);
                }
            }
        }
    }

    private void OnSt()
    {
        // CheckResetButtons();
        // if (_St==0)
        // {
        //     b_St.image.color = Color.white;
        //     b_St.GetComponentInChildren<TMP_Text>().color = Color.black;
        //     _St = -1;
        // }
        // else
        // {
        //     b_St.image.color = ActiveColor;
        //     b_St.GetComponentInChildren<TMP_Text>().color = Color.white;
        //     _St = 0;
        // }
        // CheckAllOffButtons();
        // ReloadSliders();
    }

    private void On1()
    {
        CheckResetButtons();
        if (_1==1)
        {
            b_1.image.color = Color.white;
            b_1.GetComponentInChildren<TMP_Text>().color = Color.black;
            _1 = -1;
        }
        else
        {
            b_1.image.color = ActiveColor;
            b_1.GetComponentInChildren<TMP_Text>().color = Color.white;
            _1 = 1;
        }
        CheckAllOffButtons();
        ReloadSliders();
    }
    
    private void On2()
    {
        CheckResetButtons();
        if (_2==2)
        {
            b_2.image.color = Color.white;
            b_2.GetComponentInChildren<TMP_Text>().color = Color.black;
            _2 = -1;
        }
        else
        {
            b_2.image.color = ActiveColor;
            b_2.GetComponentInChildren<TMP_Text>().color = Color.white;
            _2 = 2;
        }
        CheckAllOffButtons();
        ReloadSliders();
    }
    
    private void On3()
    {
        CheckResetButtons();
        if (_3==3)
        {
            b_3.image.color = Color.white;
            b_3.GetComponentInChildren<TMP_Text>().color = Color.black;
            _3 = -1;
        }
        else
        {
            b_3.image.color = ActiveColor;
            b_3.GetComponentInChildren<TMP_Text>().color = Color.white;
            _3 = 3;
        }
        CheckAllOffButtons();
        ReloadSliders();
    }
    
    private void On4()
    {
        CheckResetButtons();
        if (_4==4)
        {
            b_4.image.color = Color.white;
            b_4.GetComponentInChildren<TMP_Text>().color = Color.black;
            _4 = -1;
        }
        else
        {
            b_4.image.color = ActiveColor;
            b_4.GetComponentInChildren<TMP_Text>().color = Color.white;
            _4 = 4;
        }
        CheckAllOffButtons();
        ReloadSliders();
    }
    
    private void On5()
    {
        // CheckResetButtons();
        // if (_5==5)
        // {
        //     b_5.image.color = Color.white;
        //     b_5.GetComponentInChildren<TMP_Text>().color = Color.black;
        //     _5 = -1;
        // }
        // else
        // {
        //     b_5.image.color = ActiveColor;
        //     b_5.GetComponentInChildren<TMP_Text>().color = Color.white;
        //     _5 = 5;
        // }
        // CheckAllOffButtons();
        // ReloadSliders();
    }
    
    private void OnDoubleSliderArea(float value)
    {
        float max = 0;
        float min = int.MaxValue;
        foreach (var building in GameManager.instance.MyData.MyBuilders)
        {
            foreach (var myObject in building.myRealtyobjects)
            {
                if ((myObject.RealtyObject.rooms == _1 || myObject.RealtyObject.rooms == _2
                     || myObject.RealtyObject.rooms == _3 || myObject.RealtyObject.rooms == _4)
                    && myObject.RealtyObject.area > max)
                {
                    max = myObject.RealtyObject.area;
                }
            }

            foreach (var myObject in building.myRealtyobjects)
            {
                if ((myObject.RealtyObject.rooms == _1 || myObject.RealtyObject.rooms == _2
                     || myObject.RealtyObject.rooms == _3 || myObject.RealtyObject.rooms == _4)
                    && myObject.RealtyObject.area < min)
                {
                    min = myObject.RealtyObject.area;
                }
            }
        }
        
        if (min > 1000000000) min = 0;

        float _delta = max - min;
        _minArea = min + DubleSliderArea.LeftSlider.value * _delta;
        _maxArea = max - (1 - DubleSliderArea.RightSlider.value) * _delta;
        string min1Str = Math.Round(min, 1).ToString();
        string max1Str = Math.Round(max, 1).ToString();
        string min2Str = Math.Round(_minArea, 1).ToString();
        string max2Str = Math.Round(_maxArea, 1).ToString();
        MinArea.text = min2Str;
        MaxArea.text = max2Str;

        if (min1Str != min2Str || max1Str != max2Str)
        {
            //FilterPointArea.Show(Math.Round(_minArea, 1) + "-" + Math.Round(_maxArea, 1) + "м2");
        }
        else
        {
            //FilterPointArea.Hide();
        }
        //ReloadCountFlat();
        
        //AreaRect.offsetMax+=Vector2.one;
        Canvas.ForceUpdateCanvases();
        //AreaRect.offsetMax-=Vector2.one;
    }

    private void OnDoubleSliderPrice(float value)
    {
        float max = 0;
        float min = int.MaxValue;
        foreach (var building in GameManager.instance.MyData.MyBuilders)
        {
            foreach (var myObject in building.myRealtyobjects)
            {
                if ((myObject.RealtyObject.rooms == _1 || myObject.RealtyObject.rooms == _2
                     || myObject.RealtyObject.rooms == _3 || myObject.RealtyObject.rooms == _4)
                    && myObject.RealtyObject.price > max)
                {
                    max = myObject.RealtyObject.price;
                }
            }

            foreach (var myObject in building.myRealtyobjects)
            {
                if ((myObject.RealtyObject.rooms == _1 || myObject.RealtyObject.rooms == _2
                     || myObject.RealtyObject.rooms == _3 || myObject.RealtyObject.rooms == _4)
                    && myObject.RealtyObject.price < min)
                {
                    min = myObject.RealtyObject.price;
                }
            }
        }
        
        if (min > 1000000000) min = 0;

        float _delta = max - min;
        _minPrice = min + DubleSliderPrice.LeftSlider.value * _delta;
        _maxPrice = max - (1 - DubleSliderPrice.RightSlider.value) * _delta;
        MinPrice.text = GameManager.instance.GetShortPrice((int)_minPrice); //Math.Round(_minPrice, 1).ToString(); //_manager.GetShortPrice()
        MaxPrice.text = GameManager.instance.GetShortPrice((int)_maxPrice); //Math.Round(_maxPrice, 1).ToString(); //_manager.GetShortPrice()
        
        string min1Str = Math.Round(min, 1).ToString();
        string max1Str = Math.Round(max, 1).ToString();
        string min2Str = Math.Round(_minPrice, 1).ToString();
        string max2Str = Math.Round(_maxPrice, 1).ToString();
        
        if(min1Str != min2Str || max1Str != max2Str)
        { 
            // FilterPointPrice.Show(GameManager.Instance.GetShortPrice((int)_minPrice) + "-" +
            //                     GameManager.Instance.GetShortPrice((int)_maxPrice)  + "Р");
        }
        else
        {
            //FilterPointPrice.Hide();
        }
        //PriceRect.offsetMax+=Vector2.one;
        Canvas.ForceUpdateCanvases();
        //PriceRect.offsetMax-=Vector2.one;
    }
    
    private void OnDoubleSliderFloor(float value)
    {
        float max = 0;
        float min = int.MaxValue;
        foreach (var building in GameManager.instance.MyData.MyBuilders)
        {
            foreach (var myObject in building.myRealtyobjects)
            {
                if ((myObject.RealtyObject.rooms == _1 || myObject.RealtyObject.rooms == _2
                     || myObject.RealtyObject.rooms == _3 || myObject.RealtyObject.rooms == _4)
                    && myObject.RealtyObject.floor > max)
                {
                    max = myObject.RealtyObject.floor;
                }
            }

            foreach (var myObject in building.myRealtyobjects)
            {
                if ((myObject.RealtyObject.rooms == _1 || myObject.RealtyObject.rooms == _2
                     || myObject.RealtyObject.rooms == _3 || myObject.RealtyObject.rooms == _4)
                    && myObject.RealtyObject.floor < min)
                {
                    min = myObject.RealtyObject.floor;
                }
            }
        }
        
        if (min > 1000000000) min = 0;

        float _delta = max - min;
        _minFloor = (int)(min + DubleSliderFloor.LeftSlider.value * _delta);
        _maxFloor = (int)(max - (1 - DubleSliderFloor.RightSlider.value) * _delta);
        MinFloor.text = _minFloor.ToString();
        MaxFloor.text = _maxFloor.ToString();
        
        if (_minFloor != (int)min || _maxFloor != (int)max)
        {
            //FilterPointFloor.Show(_minFloor + "-" + _maxFloor);
        }
        else
        {
            //FilterPointFloor.Hide();
        }
        
        //FloorRect.offsetMax+=Vector2.one;
        Canvas.ForceUpdateCanvases();
        //FloorRect.offsetMax-=Vector2.one;
    }
    
    public void ReloadSliders()
    {
        DubleSliderArea.Init();
        DubleSliderFloor.Init();
        DubleSliderPrice.Init();
        OnDoubleSliderArea(1f);
        OnDoubleSliderFloor(1f);
        OnDoubleSliderPrice(1f);
    }
    
    private void CheckAllOffButtons()
    {
        if (_1 == -1 && _2 == -1 && _3 == -1 && _4 == -1)
        {
            //_St = 9;
            _1 = 1;
            _2 = 2;
            _3 = 3;
            _4 = 4;
            //_5 = 4;
        }
    }
    
    private void CheckResetButtons()
    {
        if (_1 != -1 && _2 != -1 && _3 != -1 && _4 != -1)
        {
            if (b_St.image.color == ActiveColor && b_1.image.color == ActiveColor && b_2.image.color == ActiveColor &&
                b_3.image.color == ActiveColor && b_4.image.color == ActiveColor && b_5.image.color == ActiveColor) return;
            //_St = -1;
            _1 = -1;
            _2 = -1;
            _3 = -1;
            _4 = -1;
            //_5 = -1;
        }
    }

    public void SendMessageOnComPort()
    {
        GameManager.instance.MessageOffAllLight();
        foreach (var prefab in _flatOnFloorPrefabs)
        {
            prefab.OnSendMessageOnComPort();
        }
    }
}
