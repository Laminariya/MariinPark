using System;
using System.Net;
using System.Net.Mail;
using System.Net.Mime; // Для MediaTypeNames
using System.IO;
using UnityEngine; // Для работы с файлами

public class EmailSender : MonoBehaviour
{

    // !!! ЗАМЕНИТЕ НА СВОИ НАСТРОЙКИ SMTP И ДАННЫЕ !!!
    string smtpHost = "smtp.mail.ru"; // Например, smtp.gmail.com (для Gmail)
    int smtpPort = 25; // Например, 587 (для TLS) или 465 (для SSL)
    string username = "happinessinkazan@mail.ru"; // Ваш email для отправки
    string password = "seOKnXYykca5qVLYawmH"; // Ваш пароль email

    string sender = "happinessinkazan@mail.ru";
    string recipient = "ran.stat@yandex.ru"; // Получатель
    string emailSubject = "ЖК Соколин парк";


    public string BodyEmail;
    private GameManager _manager;

    public void Init()
    {
        _manager = GameManager.instance;
    }

    // public void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.E))
    //     {
    //         Debug.Log("Sending email");
    //         SendHtmlEmailWithEmbeddedImage(
    //             smtpHost,
    //             smtpPort,
    //             username,
    //             password,
    //             sender,
    //             recipient,
    //             emailSubject,
    //             null,
    //             null);
    //     }
    // }

    public void Send(MyRealtyObject realtyObject, string email)
    {
        SendHtmlEmailWithEmbeddedImage(
            smtpHost,
            smtpPort,
            username,
            password,
            sender,
            email,
            emailSubject,
            realtyObject);
    }

    public void SendHtmlEmailWithEmbeddedImage(
        string smtpServerHost,
        int smtpServerPort,
        string smtpUsername,
        string smtpPassword,
        string senderEmail,
        string recipientEmail,
        string subject,
        MyRealtyObject realtyObject) // Путь к файлу изображения, которое нужно встроить
    {
        string body = BodyEmail;

        // 1. Создаем объект сообщения
        using (MailMessage mail = new MailMessage())
        {
            mail.From = new MailAddress(senderEmail);
            mail.To.Add(recipientEmail);
            mail.Subject = subject;
            // Мы будем использовать HTML тело
            mail.IsBodyHtml = true;

            // 2. Подготавливаем HTML тело письма
            // Важно: в теге <img> используем src="cid:ImageID",
            // где ImageID - это уникальный идентификатор, который мы назначим изображению.

            // 3. Создаем AlternateView для HTML тела
            // Это позволяет предоставить альтернативную версию письма (например, Plain Text)
            // Хотя здесь используется только для добавления LinkedResource к HTML

            //TODO Сформировать правильный Email
            Debug.Log("Sending email");
            if (realtyObject != null)
            {
                //1-комн. квартира №9, 45 м2
                string roomsNumberArray = realtyObject.RealtyObject.rooms + "-комн. квартира №" +
                                          realtyObject.RealtyObject.floorIndex + ", " + realtyObject.RealtyObject.area +
                                          " м" + _manager.SymvolQuadro;
                //2 очередь  | 1 этап | 4 корпус
                string queueStageKorpus = "Корпус " + realtyObject.Korpus + "| " + "Секция " + realtyObject.Section +
                                          "| " + "Этаж " + realtyObject.RealtyObject.floor;
                //1 секция | 10 этаж | №126
                string sectionFloorNumber = realtyObject.Section + " секция | " + realtyObject.RealtyObject.floor +
                                            " этаж | №" + realtyObject.RealtyObject.number;
                string price = _manager.GetSplitPrice(realtyObject.RealtyObject.price) + " " + _manager.SymvolRuble;
                body = body.Replace("###", roomsNumberArray);
                body = body.Replace("$$$1", queueStageKorpus);
                body = body.Replace("$$$2", "");
                body = body.Replace("&&&", price);
                body = body.Replace("imageOnFloor", realtyObject.RealtyObject.furnlayouturl);
                body = body.Replace("imageFloor", realtyObject.RealtyObject.planurl);
                Debug.Log(body);
            }


            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);

            // 4. Создаем LinkedResource для изображения
            // Используем путь к файлу изображения


            // Добавляем HTML представление к сообщению
            mail.AlternateViews.Add(htmlView);

            // (Опционально) Добавляем plain text представление
            // Это хорошая практика для клиентов, которые не поддерживают HTML
            // string plainTextBody = "Привет!\nЭто письмо с встроенным изображением (в HTML версии).\nС уважением, Отправитель";
            // AlternateView plainTextView = AlternateView.CreateAlternateViewFromString(plainTextBody, null, MediaTypeNames.Text.Plain);
            // mail.AlternateViews.Add(plainTextView);


            // 5. Создаем SmtpClient и настраиваем его
            using (SmtpClient smtpClient = new SmtpClient(smtpServerHost, smtpServerPort))
            {
                // Если SMTP-сервер требует аутентификации
                smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                // Если SMTP-сервер требует SSL
                smtpClient.EnableSsl = true;
                // (Опционально) Таймаут в секундах
                // smtpClient.Timeout = 10000;

                // 6. Отправляем письмо
                try
                {
                    smtpClient.Send(mail);
                    Debug.Log("Email отправлен успешно!");
                }
                catch (SmtpException ex)
                {
                    Debug.Log($"Ошибка SMTP: {ex.StatusCode}");
                    Debug.Log($"Ответ сервера: {ex.Message}");
                    // Дополнительная информация об ошибке
                    if (ex.InnerException != null)
                    {
                        Debug.Log($"Внутренняя ошибка: {ex.InnerException.Message}");
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log($"Произошла ошибка при отправке email: {ex.Message}");
                }
            } // SmtpClient автоматически Dispose()
        } // MailMessage автоматически Dispose()
    }
}