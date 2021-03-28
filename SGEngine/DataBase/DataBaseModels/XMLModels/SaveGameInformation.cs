using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[XmlRoot(ElementName = "SaveItem")]
public class SaveItem
{
    [XmlAttribute(AttributeName = "id")]
    public int Id { get; set; }

    [XmlAttribute(AttributeName = "isLock")]
    public bool isLock { get; set; }

}

[XmlRoot(ElementName = "SaveItems")]
public class Save_Items
{
    [XmlElement(ElementName = "SaveItem")]
    public List<SaveItem> SaveItemList { get; set; }
}
[XmlRoot(ElementName = "Save_WorldObjects")]
public class Save_WorldObjects
{
    [XmlElement(ElementName = "Save_Items")]
    public Save_Items Save_Items { get; set; }
}

#region Update
[XmlRoot(ElementName = "Save_UpdateNewObject")]
public class Save_UpdateNewObject
{
    [XmlAttribute(AttributeName = "id")]
    public int Id { get; set; }
}

[XmlRoot(ElementName = "Save_UpdateBoostObject")]
public class Save_UpdateBoostObject {
    [XmlAttribute(AttributeName = "id")]
    public int Id { get; set; }
}

[XmlRoot(ElementName = "Save_New_Items")]
public class Save_New_Items
{
    [XmlElement(ElementName = "Save_UpdateNewObject")]
    public List<Save_UpdateNewObject> Save_UpdateNewOjectItem { get; set; }
}

[XmlRoot(ElementName = "Save_Boost_Items")]
public class Save_Boost_Items
{
    [XmlElement(ElementName = "Save_UpdateBoostObject")]
    public List<Save_UpdateBoostObject> Save_UpdateBoostObjectItem { get; set; }
}

[XmlRoot(ElementName = "Save_Upgrade_Items")]
public class Save_Upgrade_Items
{
    [XmlElement(ElementName = "Save_New_Items")]
    public Save_New_Items Save_New_Items { get; set; }
    
    [XmlElement(ElementName = "Save_Boost_Items")]
    public Save_Boost_Items Save_Boost_Items { get; set; }
}

[XmlRoot(ElementName = "Save_Upgrades")]
public class Save_Upgrades
{
    [XmlElement(ElementName = "Save_Upgrade_Items")]
    public Save_Upgrade_Items Save_Upgrade_Items { get; set; }
}
#endregion Update

[XmlRoot(ElementName = "SaveGameInformation")]
public class SaveGameInformation
{
    [XmlElement(ElementName = "Save_WorldObjects")]
    public Save_WorldObjects Save_WorldObjects { get; set; }
    
    [XmlElement(ElementName = "Save_Upgrades")]
    public Save_Upgrades Save_Upgrades { get; set; }
}