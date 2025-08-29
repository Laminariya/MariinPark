using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

public class SerializeJson : MonoBehaviour
{
    
    [HideInInspector] public JsonData JsonData;
    private string _nameJson = "note1.txt";
    private HttpClient Client = new HttpClient();
    private string _json;
    private GameManager _manager;
    
    //ссылка на json
    private string _url = "https://etalongroup.ru/upload/feed/Mariinnpark/mariinn-park/feed.json";
    
    public async Task Init()
    {
        _manager = GameManager.instance;
        await LoadJSON();
    }
    
    public void  LoadJsonFile()
    {
        _json = Resources.Load<TextAsset>("note1").text;

        if (_json != "")
        {
            //GetComponent<GameManager>().Json = JsonUtility.FromJson<JsonData>(_json);
        }
        Debug.Log("Json загружен");
    }

    private async Task LoadJSON()
    {
        var uri = new Uri(_url);

        var result = await Client.GetAsync(uri);
        string str = await result.Content.ReadAsStringAsync();
        
        //Debug.Log("JSON " + str);
        JsonData = JsonUtility.FromJson<JsonData>(str);
    }

}

[Serializable]
public class JsonData
{
    public List<Building> buildings = new List<Building>();
}

[Serializable]
public class Building
{
    public int id;
    public string name;
    public int maxfloor;
    public string releasedate;
    public List<RealtyObject> realtyobjects = new List<RealtyObject>();
}

[Serializable]
public class RealtyObject
{
    public string type;
    public float area;
    public int floor;
    public int floorIndex;
    public int number;
    public int price;
    public int rooms;
    public List<string> tags = new List<string>();
    public string section;
    public string layoutext;
    public string layouturl;
    public string furnlayoutext;
    public string furnlayouturl;
    public string planurlext;
    public string planurl;
}