using System;
using System.Collections;
using System.Drawing;
using UnityEngine;
using System.Drawing.Printing;
using System.IO;
using TMPro;
using Image = System.Drawing.Image;


public class PrintingManager : MonoBehaviour
{
    
    public RenderTexture renderTexture;
    public Camera Camera;
    public TMP_Text Price;
    public TMP_Text PriceMetr;
    public TMP_Text Apartment;
    public TMP_Text Queue;
    public UnityEngine.UI.Image Image_Furniture;
    public UnityEngine.UI.Image Image_Floor;
    
    private string result;
    private GameManager _manager;
    private MyRealtyObject _realtyObject;
    private string _pathFolder = "//PrintImage//printing";
    private string _path;
    private bool _isPrinting = false;

    public void Init()
    {
        _manager = GameManager.instance;
        _path = Directory.GetCurrentDirectory() + _pathFolder;
        if (!Directory.Exists(_path))
            Directory.CreateDirectory(_path);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !_isPrinting)
        {
            StartCoroutine(PrintDoc(_manager.MyData.MyBuilders[0].myRealtyobjects[0]));
        }
    }

    public void Print(MyRealtyObject realtyObject)
    {
        if(_isPrinting) return;
        StartCoroutine(PrintDoc(realtyObject));
    }

    public IEnumerator PrintDoc(MyRealtyObject realtyObject)
    {
        _isPrinting = true;
        _realtyObject = realtyObject;
        Debug.Log("Printing start");
        // Формируем image со всеми данными
        Price.text = _manager.GetSplitPrice(_realtyObject.RealtyObject.price) + " " + GameManager.instance.SymvolRuble;
        PriceMetr.text =
            _manager.GetSplitPrice(((int) (_realtyObject.RealtyObject.price / _realtyObject.RealtyObject.area))) + " " +
            GameManager.instance.SymvolRuble + "/м" + GameManager.instance.SymvolQuadro;
        if (_realtyObject.RealtyObject.rooms == 0)
        {
            Apartment.text = "Студия №" + _realtyObject.RealtyObject.floorIndex + ", " +
                             _realtyObject.RealtyObject.area + " м" + GameManager.instance.SymvolQuadro;
        }
        else
        {
            Apartment.text = _realtyObject.RealtyObject.rooms.ToString() + "-комн. квартира №" +
                             _realtyObject.RealtyObject.floorIndex + ", " + _realtyObject.RealtyObject.area + " м" +
                             GameManager.instance.SymvolQuadro;
        }
        
        Queue.text = "Корпус "+_realtyObject.Korpus+ "| " + "Секция " +_realtyObject.Section+ "| " + "Этаж "+_realtyObject.RealtyObject.floor;
        
        Debug.Log(realtyObject.PathFloor);
        Debug.Log(realtyObject.PathPlaneFurniture.ToString());
        Image_Furniture.sprite = realtyObject.FlatSprite;
        Image_Floor.sprite = realtyObject.FloorSprite;
        Image_Floor.preserveAspect = true;
        Image_Furniture.preserveAspect = true;
        
        
        
        yield return new WaitForSeconds(0.1f);
        // Сохраняем картинку на диске
        yield return StartCoroutine(SaveImage());
        //Начинаем печать картинки на диске
        
        // объект для печати
        PrintDocument printDocument = new PrintDocument();

        // обработчик события печати
        printDocument.PrintPage += PrintPageHandler;
        Debug.Log("Print End");
        printDocument.Print();
        _isPrinting = false;
    }
    
    

    // обработчик события печати
    void PrintPageHandler(object sender, PrintPageEventArgs e)
    {
        Point[] destinationPoints = {
            new Point(0, 0),   // destination for upper-left point of
            // original
            new Point(e.PageSettings.PaperSize.Width-60, 0),  // destination for upper-right point of
            // original
            new Point(0, e.PageSettings.PaperSize.Height - 50),}; 
        Debug.Log(e.PageSettings.PaperSize.Width+" " + e.PageSettings.PaperSize.Height);
        // печать строки result
        Debug.Log("PrintPageHandler");
        //e.Graphics.DrawString(result, new Font("Arial", 14), Brushes.Black, 0, 0);
        Image image = Image.FromFile(_path+".png");
        e.Graphics.DrawImage(image, destinationPoints);
        //e.Graphics.DrawImage(image, new Point(50, 50));
    }
    
    IEnumerator SaveImage()
    {
        if (renderTexture == null)
        {
            Debug.LogError("RenderTexture не назначен!");
            yield break;
        }

        // 1. Создаем Texture2D с размерами RenderTexture
        Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);

        // 2. Делаем RenderTexture активным, чтобы ReadPixels мог прочитать из него
        RenderTexture.active = renderTexture;

        // Копируем данные из активного RenderTexture в Texture2D
        texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture.Apply();

        // Сбрасываем активный RenderTexture
        RenderTexture.active = null;

        // 3. Кодируем Texture2D в формат изображения
        byte[] bytes;
        bytes = texture.EncodeToPNG();
        
        // 4. Записываем массив байтов в файл
       yield return StartCoroutine(_manager.createImagePNG.CreatePNG(_path + ".png", bytes));

        // Уничтожаем временную текстуру
        Destroy(texture);
    }
}