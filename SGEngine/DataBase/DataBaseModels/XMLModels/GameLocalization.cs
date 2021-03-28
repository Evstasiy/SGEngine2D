using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[XmlRoot(ElementName = "item")]
public class DescriptionItem
{
    [XmlAttribute(AttributeName = "id")]
    public int Id { get; set; }

    [XmlAttribute(AttributeName = "mainDescription")]
    public string MainDescription { get; set; }
    
    [XmlAttribute(AttributeName = "secondaryDescription")]
    public string SecondaryDescription { get; set; }

}
[XmlRoot(ElementName = "Items_Localization")]
public class Items_Localization
{
    [XmlElement(ElementName = "item")]
    public List<DescriptionItem> descriptionItems { get; set; }
}

[XmlRoot(ElementName = "WorldObjects_Localization")]
public class WorldObjects_Localization
{
    [XmlElement(ElementName = "Items_Localization")]
    public Items_Localization Items_Localization { get; set; }
}

[XmlRoot(ElementName = "New_Items_Localization")]
public class New_Items_Localization {
    [XmlElement(ElementName = "item")]
    public List<DescriptionItem> descriptionItems { get; set; }
}

[XmlRoot(ElementName = "Boost_Items_Localization")]
public class Boost_Items_Localization {
    [XmlElement(ElementName = "item")]
    public List<DescriptionItem> descriptionItems { get; set; }
}


[XmlRoot(ElementName = "Upgrade_Items_Localization")]
public class Upgrade_Items_Localization {
    [XmlElement(ElementName = "New_Items_Localization")]
    public New_Items_Localization New_Items_Localization { get; set; }

    [XmlElement(ElementName = "Boost_Items_Localization")]
    public Boost_Items_Localization Boost_Items_Localization { get; set; }
}

[XmlRoot(ElementName = "Upgrades_Localization")]
public class Upgrades_Localization {
    [XmlElement(ElementName = "Upgrade_Items_Localization")]
    public Upgrade_Items_Localization Upgrade_Items_Localization { get; set; }
}

[XmlRoot(ElementName = "GameParameters")]
public class GameLocalization
{
    [XmlElement(ElementName = "WorldObjects_Localization")]
    public WorldObjects_Localization WorldObjects_Localization { get; set; }
    
    [XmlElement(ElementName = "Upgrades_Localization")]
    public Upgrades_Localization Upgrades_Localization { get; set; }
}
