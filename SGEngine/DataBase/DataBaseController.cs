using Assets.Scripts.SGEngine.DataBase;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq; //fix (or not)
using System.Xml.Serialization;
using UnityEngine;

class DataBaseComponent
{
    private string PATH_SAVE_FILE;

    /// <summary>
    /// Проверяет целостность всех начальных файлов, испоьзуемых в работе 
    /// </summary>
    /// <returns></returns>
    public bool CheckBaseFiles()
    {
        PATH_SAVE_FILE = Application.persistentDataPath + @"/Save/UserSaveFile";
        try
        {
            CheckAndCreateSaveFile();
            return true;
        }
        catch (Exception ex) {
            Debug.LogError("CheckBaseFiles - " + ex.Message);
            return false;
        }
    }

    /// <summary>
    /// Проверяет файл сохранения на существование и создает его при отсутсвии
    /// </summary>
    /// <returns>Результат приверки и создания файла сохранения</returns>
    private bool CheckAndCreateSaveFile()
    {
        string pathSaveFile = PATH_SAVE_FILE + ".xml";
        if (!File.Exists(pathSaveFile))
        {
            try
            {
                Directory.CreateDirectory(Application.persistentDataPath + @"/Save");
                TextAsset BaseSaveFile = Resources.Load<TextAsset>("Date/DataBase/UserSaveFileBase");
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(BaseSaveFile.text);
                doc.Save(pathSaveFile);
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError("Не удалось создать файл сохранения. " + ex.Message);
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Записывает в файл сохранения измения из объекта с данными сохранения
    /// </summary>
    /// <param name="saveInformation">Объект с данными сохранения</param>
    /// <returns></returns>
    public bool WriteInSaveFile(SaveGameInformation saveInformation)
    {
        try
        {
            System.Xml.Serialization.XmlSerializer writer =
            new System.Xml.Serialization.XmlSerializer(typeof(SaveGameInformation));
            System.IO.FileStream file = System.IO.File.Open(PATH_SAVE_FILE + ".xml", FileMode.Open);
            file.SetLength(0);
            writer.Serialize(file, saveInformation);
            file.Close();

            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError("WriteInSaveFile - " + ex.Message);
            return false;
        }
    }

    /// <summary>
    /// Получает файл локализации в зависимости от параметра 
    /// </summary>
    /// <param name="nameLocalization">Необходимый язык</param>
    /// <returns></returns>
    public GameLocalization GetGlobalLocalizationFile(LocalizationOption.LocalizationRegion nameLocalization)
    {
        XmlDocument xml = new XmlDocument();
        TextAsset notSerializeXml = Resources.Load<TextAsset>("Date/DataBase/Localization/" + nameLocalization.ToString());
        xml.LoadXml(notSerializeXml.text);
        XmlNodeReader nod1 = new XmlNodeReader(xml);

        XmlRootAttribute root = new XmlRootAttribute();
        root.ElementName = "GameLocalization";
        root.IsNullable = true;

        XmlSerializer ser = new XmlSerializer(typeof(GameLocalization), root);
        var XMLInformation = (GameLocalization)ser.Deserialize(nod1);

        return XMLInformation;
    }

    /// <summary>
    /// Возвращает пакет с root и nod для дальнейшей десериализации 
    /// </summary>
    /// <param name="pathFile">Путь к файлу</param>
    /// <param name="elementName">Начальный элемент файла</param>
    /// <param name="isResourceFile">Брать ли файл из папки ресурсов</param>
    /// <returns></returns>
    private SerializerPacket GetSerializerPacket(string pathFile, string elementName, bool isResourceFile = true) {
        XmlDocument xml = new XmlDocument();
        string fileText = "";
        if (isResourceFile) {
            fileText = Resources.Load<TextAsset>(pathFile).text;
        }
        else {
            using (StreamReader sr = new StreamReader(pathFile + ".xml")) {
                fileText = sr.ReadToEnd();
            }
        }
        xml.LoadXml(fileText);
        XmlNodeReader nod1 = new XmlNodeReader(xml);

        XmlRootAttribute root = new XmlRootAttribute();
        root.ElementName = elementName;
        root.IsNullable = true;

        return new SerializerPacket(root, nod1);
    }

    /// <summary>
    /// Сериализует данные из файла сохранения PATH_SAVE_FILE в SaveGameInformation элементы
    /// </summary>
    /// <returns></returns>
    public SaveGameInformation GetXMLSaveGameInformation() {
        string elementName = "SaveGameInformation";
        var serializerPacket = GetSerializerPacket(PATH_SAVE_FILE, elementName, false);
        XmlSerializer ser = new XmlSerializer(typeof(SaveGameInformation), serializerPacket.root);
        var XMLInformation = (SaveGameInformation)ser.Deserialize(serializerPacket.node);

        return XMLInformation;
    }

    /// <summary>
    /// Сериализует информацию из Date/DataBase/TestBase.xml в GameParameters элементы
    /// </summary>
    /// <returns></returns>
    public GameParameters GetXMLGameParameters() {
        string path = "Date/DataBase/TestBase";
        string elementName = "GameParameters";
        var serializerPacket = GetSerializerPacket(path, elementName);
        XmlSerializer ser = new XmlSerializer(typeof(GameParameters), serializerPacket.root);
        var XMLInformation = (GameParameters)ser.Deserialize(serializerPacket.node);

        return XMLInformation;
    }

}


class SerializerPacket {
    public XmlRootAttribute root { get; private set; }
    public XmlNodeReader node { get; private set; }
    public SerializerPacket(XmlRootAttribute root, XmlNodeReader node) {
        this.root = root;
        this.node = node;
    }
}
