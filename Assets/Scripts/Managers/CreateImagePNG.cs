using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using Image = UnityEngine.UI.Image;

public class CreateImagePNG : MonoBehaviour
{

    public enum TypePlane
    {
        floor, plane, furniture
    }
    
    public Image ImageTest;
    public string PlaneFloor = "//Plans//PlanFloors//";
    public string PlaneApartment = "//Plans//PlanApartments//";
    public string PlaneApartmentFurn = "//Plans//PlanApartmentFurns//";
    
    public string PlaneParking = "//Plans//PlanParking//";

    private GameManager _manager;
    
    private HttpClient _httpClient = new HttpClient();
    
    public IEnumerator Init()
    {
        Debug.Log("Create ImagePNG");
        _manager = GameManager.instance;
        //StartCoroutine(LoadFileFromUrl(_domen+_path));
        //StartCoroutine(LoadPNG(_nameFile));
        if(!Directory.Exists(Directory.GetCurrentDirectory()+PlaneFloor))
            CreateFolder(Directory.GetCurrentDirectory()+PlaneFloor);
        if(!Directory.Exists(Directory.GetCurrentDirectory()+PlaneApartment))
            CreateFolder(Directory.GetCurrentDirectory()+PlaneApartment);
        if(!Directory.Exists(Directory.GetCurrentDirectory()+PlaneApartmentFurn))
            CreateFolder(Directory.GetCurrentDirectory()+PlaneApartmentFurn);
        
        if(!Directory.Exists(Directory.GetCurrentDirectory()+PlaneParking))
            CreateFolder(Directory.GetCurrentDirectory()+PlaneParking);
       
        yield return StartCoroutine(CheckPlane());
        yield return null;
    }

    private void CreateFolder(string url)
    {
        Directory.CreateDirectory(url);
    }

    //Проверка на наличие схем и изменение
    IEnumerator CheckPlane()
    {
        Debug.Log("Checking plane");
        
        int count = 0;
        foreach (var builder in _manager.MyData.MyBuilders)
        {
            count+=builder.myRealtyobjects.Count;
        }

        int count2 = 0;
        foreach (var builder in _manager.MyData.MyBuilders)
        {
            //Debug.Log(builder.Name);
            foreach (var realtyobject in builder.myRealtyobjects)
            {
                count2++;
                _manager.InfoStartPanel.text = count2.ToString()+"/"+count.ToString();
                //Debug.Log(realtyobject.Id);
                string idObject = builder.id.ToString() + realtyobject.RealtyObject.number.ToString();
                //Зашружаем схемы с мебелью
                if (!File.Exists(realtyobject.PathPlaneFurniture) &&
                    realtyobject.RealtyObject.furnlayouturl != "")
                    yield return StartCoroutine(LoadFileFromUrl(realtyobject.RealtyObject.furnlayouturl,
                        realtyobject.PathPlaneFurniture));
                //Загружаем схему квартиры
                if (!File.Exists(realtyobject.PathPlane) &&
                    realtyobject.RealtyObject.layouturl != "")
                    yield return StartCoroutine(LoadFileFromUrl(realtyobject.RealtyObject.layouturl,
                        realtyobject.PathPlane));
                //Загружаем схему этажа
                if (!File.Exists(realtyobject.PathFloor) &&
                    realtyobject.RealtyObject.planurl != "")
                    yield return StartCoroutine(LoadFileFromUrl(realtyobject.RealtyObject.planurl,
                        realtyobject.PathFloor));
                yield return null;
            }
        }
        Debug.Log("Checking plane END");
    }



    //Загрузили с сервера картинку
    IEnumerator LoadFileFromUrl(string url, string pathPlane) 
    {
        Debug.Log("LoadFileFromUrl");
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();
        // if (www.isHttpError || www.isNetworkError)
        // {
        //     Debug.Log("Error while Receiving: " + www.error);
        // }
        // else
        // {
            //_nameFile = Директория + папка + id квартиры
            Debug.Log("Изображение загружено");
            yield return StartCoroutine(CreatePNG(pathPlane, www.downloadHandler.data));
        //}
    }
    
    // //Загрузили с сервера картинку
    // IEnumerator LoadGifFileFromUrl(string url, string pathPlane) 
    // {
    //     try
    //     {
    //         await DownloadImageAsync(url, pathPlane);
    //         Console.WriteLine("Изображение успешно загружено.");
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine($"Ошибка: {ex.Message}");
    //     }
    //     Debug.Log("Изображение загружено");
    //     yield return StartCoroutine(CreatePNG(pathPlane, www.downloadHandler.data));
    //     //}
    // }
    
    //Сохранили картинку у нас
    public IEnumerator CreatePNG(string path, byte[] data)
    {
        // запись в файл
        using (FileStream fstream = new FileStream(path, FileMode.OpenOrCreate))
        {
            // запись массива байтов в файл
            yield return fstream.WriteAsync(data, 0, data.Length);
            Debug.Log("Картинка создана записан в файл");
        }
    }

    public IEnumerator LoadPNG(string urlPlane, Image image)
    {
        string url = "file://" + urlPlane;
        
        using (WWW www = new WWW(url))
        {
            yield return www;
            //Debug.Log(www.texture);
            Texture2D texture2D = www.texture;
            Sprite sprite = Sprite.Create(texture2D, new Rect(0.0f, 0.0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f), 100.0f);
            //_sprites.Add(sprite);
            if (image != null)
                image.sprite = sprite;
        }
    }

    public IEnumerator LoadSpritePNG(string urlPlane, Sprite sprite)
    {
        string url = "file://" + urlPlane;
        
        using (WWW www = new WWW(url))
        {
            yield return www;
            Texture2D texture2D = www.texture;
            Sprite _sprite = Sprite.Create(texture2D, new Rect(0.0f, 0.0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f), 100.0f);
            sprite = _sprite;
            sprite.name = urlPlane;
        }
    }
    
    public IEnumerator LoadSpriteFlatPNG(MyRealtyObject myObject)
    {
        string url = "file://" + myObject.PathPlaneFurniture;
        
        using (WWW www = new WWW(url))
        {
            yield return www;
            Texture2D texture2D = www.texture;
            Sprite _sprite = Sprite.Create(texture2D, new Rect(0.0f, 0.0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f), 100.0f);
            myObject.FlatSprite = _sprite;
        }
        
        url = "file://" + myObject.PathFloor;
        using (WWW www = new WWW(url))
        {
            yield return www;
            Texture2D texture2D = www.texture;
            Sprite _sprite = Sprite.Create(texture2D, new Rect(0.0f, 0.0f, texture2D.width, texture2D.height),
                new Vector2(0.5f, 0.5f), 100.0f);
            myObject.FloorSprite = _sprite;
        }
        
    }

}
