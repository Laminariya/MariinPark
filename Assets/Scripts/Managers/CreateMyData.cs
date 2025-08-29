using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class CreateMyData : MonoBehaviour
{
    private GameManager _manager;

    private MyData _myData;
    private string PlaneFloor = "//Plans//PlanFloors//";
    private string PlaneApartment = "//Plans//PlanFlat//";

    public IEnumerator Init()
    {
        Debug.Log("Create My Data");
        _manager = GameManager.instance;
        CreateData();
        yield return null;
    }

    private void CreateData()
    {
        _myData = new MyData();
        // foreach (var building in _manager.serializeJson.JsonData.buildings) 
        // {
        //     MyBuilding myBuilding = new MyBuilding(building);
        //     _myData.MyBuilders.Add(myBuilding);
        // }
        
        Building building = new Building();
        building.realtyobjects = new List<RealtyObject>();
        building.maxfloor = _manager.serializeJson.JsonData.buildings[0].maxfloor;
        building.releasedate = _manager.serializeJson.JsonData.buildings[0].releasedate;
        building.name = "1";
        building.id = _manager.serializeJson.JsonData.buildings[0].id;
        Building building2 = new Building();
        building2.realtyobjects = new List<RealtyObject>();
        building2.maxfloor = _manager.serializeJson.JsonData.buildings[0].maxfloor;
        building2.releasedate = _manager.serializeJson.JsonData.buildings[0].releasedate;
        building2.name = "2";;
        building2.id = _manager.serializeJson.JsonData.buildings[0].id;

        foreach (var realtyobject in _manager.serializeJson.JsonData.buildings[0].realtyobjects)
        {
            if (realtyobject.section == "Секция 1.1")
            {
                building.realtyobjects.Add(realtyobject);
            }
            if (realtyobject.section == "Секция 1.2")
            {
                building2.realtyobjects.Add(realtyobject);
            }
        }
        
        
        MyBuilding myBuilding = new MyBuilding(building);
        MyBuilding myBuilding2 = new MyBuilding(building2);
        _myData.MyBuilders.Add(myBuilding);
        _myData.MyBuilders.Add(myBuilding2);
        
        _manager.MyData = _myData;

    }
    
    public IEnumerator CreateSprites()
    {
        int count = 0;
        foreach (var building in _myData.MyBuilders) 
        {
            foreach (var myObject in building.myRealtyobjects)
            {
                count++;
            }
        }
       
        string str = GameManager.instance.InfoStartPanel.text;
        int count2 = 0;
        foreach (var building in _myData.MyBuilders)
        {
            foreach (var myObject in building.myRealtyobjects)
            {
                yield return StartCoroutine(_manager.createImagePNG.LoadSpriteFlatPNG(myObject));
                count2++;
                GameManager.instance.InfoStartPanel.text = str + "\r\n" + "Load Image: " +count2 + "/" + count;
            }
        }
    }

}

[Serializable]
public class MyData
{
    public List<MyBuilding> MyBuilders = new List<MyBuilding>();
}

[Serializable]
public class MyBuilding
{
    public int id;
    public string Name;
    public int maxfloor;
    public string releasedate;
    public List<MyRealtyObject> myRealtyobjects = new List<MyRealtyObject>();
    public List<MySection> mySections = new List<MySection>();
    public int MaxPrice;
    public int MinPrice;
    public float MaxArea;
    public float MinArea;
    public int MaxFloor;
    public int MinFloor;
    public int Korpus;
    public List<int> MaxPrices = new List<int>();
    public List<int> MinPrices = new List<int>();
    public List<float> MaxAreas = new List<float>();
    public List<float> MinAreas = new List<float>();
    public List<int> MaxFloors = new List<int>();
    public List<int> MinFloors = new List<int>();
    public List<int> Floors = new List<int>();
    
    public MyBuilding(Building building)
    {
        id = building.id;
        Name = building.name;
        maxfloor = building.maxfloor;
        releasedate = building.releasedate;

        //Счастье в Казани, корпус К4

        try
        {
            Korpus = int.Parse(Name);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Korpus = 0;
        }
        
        foreach (var realtyobject in building.realtyobjects)
        {
            MyRealtyObject myRealtyObject = new MyRealtyObject(realtyobject, building, Korpus);
            
            myRealtyobjects.Add(myRealtyObject);
        }
        
        List<string> sections = new List<string>();
        foreach (var realtyobject in myRealtyobjects)
        {
            if (!sections.Contains(realtyobject.Section))
            {
                sections.Add(realtyobject.Section);
            }
        }
        sections.Sort();

        for (int i = 0; i < sections.Count; i++)
        {
            MySection section = new MySection();
            section.Name = sections[i];
            foreach (var o in myRealtyobjects)
            {
                if (o.Section == sections[i])
                {
                    section.myRealtyobjects.Add(o);
                }
            }
        }
        
        // ищем максимум и минимум
        int price = 0;
        foreach (var realtyobject in myRealtyobjects)
        {
            if (realtyobject.RealtyObject.price > price) price = realtyobject.RealtyObject.price;
        }

        MaxPrice = price;

        price = int.MaxValue;
        foreach (var realtyobject in myRealtyobjects)
        {
            if (realtyobject.RealtyObject.price < price) price = realtyobject.RealtyObject.price;
        }

        MinPrice = price;


        float area = 0;
        foreach (var realtyobject in myRealtyobjects)
        {
            if (realtyobject.RealtyObject.area > area) area = realtyobject.RealtyObject.area;
        }

        MaxArea = area;

        area = 10000f;
        foreach (var realtyobject in myRealtyobjects)
        {
            if (realtyobject.RealtyObject.area < area) area = realtyobject.RealtyObject.area;
        }

        MinArea = area;
        
        int floor = 0;
        foreach (var realtyobject in myRealtyobjects)
        {
            if (realtyobject.RealtyObject.floor > floor) floor = realtyobject.RealtyObject.floor;
        }

        MaxFloor = floor;

        floor = 10000;
        foreach (var realtyobject in myRealtyobjects)
        {
            if (realtyobject.RealtyObject.floor < floor) floor = realtyobject.RealtyObject.floor;
        }

        MinFloor = floor;
        
        //Ищем максимум и минимум по количеству комнат
        AddMinMaxPrices(0);
        AddMinMaxPrices(1);
        AddMinMaxPrices(2);
        AddMinMaxPrices(3);
        AddMinMaxPrices(4);
        AddMinMaxPrices(5);
        
        AddMinMaxArea(0);
        AddMinMaxArea(1);
        AddMinMaxArea(2);
        AddMinMaxArea(3);
        AddMinMaxArea(4);
        AddMinMaxArea(5);
        
        AddMinMaxFloor(0);
        AddMinMaxFloor(1);
        AddMinMaxFloor(2);
        AddMinMaxFloor(3);
        AddMinMaxFloor(4);
        AddMinMaxFloor(5);
        
        foreach (var myRealtyObject in myRealtyobjects)
        {
            if(!Floors.Contains(myRealtyObject.RealtyObject.floor))
                Floors.Add(myRealtyObject.RealtyObject.floor);
        }
        Floors.Sort();
        
    }

    private void AddMinMaxPrices(int rooms)
    {
        int price = 0;
        foreach (var realtyobject in myRealtyobjects)
        {
            if (realtyobject.RealtyObject.rooms == rooms && realtyobject.RealtyObject.price > price)
                price = realtyobject.RealtyObject.price;
        }
        //Debug.Log("Max " + rooms + " " + price);
        MaxPrices.Add(price);
        price = int.MaxValue;
        foreach (var realtyobject in myRealtyobjects)
        {
            if (realtyobject.RealtyObject.rooms == rooms && realtyobject.RealtyObject.price < price)
                price = realtyobject.RealtyObject.price;
        }

        if (price > 1000000000) price = 0;
        //Debug.Log("Min " + rooms + " " + price);
        MinPrices.Add(price);
    }

    private void AddMinMaxArea(int rooms)
    {
        float area = 0;
        foreach (var realtyobject in myRealtyobjects)
        {
            if (realtyobject.RealtyObject.rooms == rooms && realtyobject.RealtyObject.area > area)
                area = realtyobject.RealtyObject.area;
        }
        //Debug.Log("Max " + Queue + " " + Stage + " " + rooms + " " + area + " Builder " + Name);
        MaxAreas.Add(area);

        area = 10000f;
        foreach (var realtyobject in myRealtyobjects)
        {
            if (realtyobject.RealtyObject.rooms == rooms && realtyobject.RealtyObject.area < area)
                area = realtyobject.RealtyObject.area;
        }

        if (area > 1000f) area = 0;
        //Debug.Log("Min " + Queue + " " + Stage + " " + rooms + " " + area);
        MinAreas.Add(area);
    }
    
    private void AddMinMaxFloor(int rooms)
    {
        int area = 0;
        foreach (var realtyobject in myRealtyobjects)
        {
            if (realtyobject.RealtyObject.rooms == rooms && realtyobject.RealtyObject.floor > area)
                area = realtyobject.RealtyObject.floor;
        }
        //Debug.Log("Max " + rooms + " " + area);
        MaxFloors.Add(area);

        area = 10000;
        foreach (var realtyobject in myRealtyobjects)
        {
            if (realtyobject.RealtyObject.rooms == rooms && realtyobject.RealtyObject.floor < area)
                area = realtyobject.RealtyObject.floor;
        }

        if (area > 1000) area = 0;
        //Debug.Log("Min " + rooms + " " + area);
        MinFloors.Add(area);
    }

}

[Serializable]
public class MySection
{
    public string Name;
    public List<MyRealtyObject> myRealtyobjects = new List<MyRealtyObject>();
}

[Serializable]
public class MyRealtyObject
{
    public string Id;
    public string PathFloor;
    public string PathPlane;
    public string PathPlaneFurniture;
    public RealtyObject RealtyObject;
    public int Korpus;
    public string Section;
    public Sprite FlatSprite;
    public Sprite FloorSprite;
    public int Number;

    public MyRealtyObject(RealtyObject realtyObject, Building building, int korpus)
    {
        RealtyObject = realtyObject;
        Korpus = korpus;
        Id = building.id.ToString() + RealtyObject.number.ToString();
        PathFloor = Directory.GetCurrentDirectory() + "//Plans//PlanFloors//" + Id.ToString() + ".png";
        PathPlane = Directory.GetCurrentDirectory() + "//Plans//PlanApartments//" + Id.ToString() + ".png";
        PathPlaneFurniture = Directory.GetCurrentDirectory() + "//Plans//PlanApartmentFurns//" + Id.ToString() + ".png";
        Section = realtyObject.section[realtyObject.section.Length - 1].ToString();
        Number = RealtyObject.number;
    }

}