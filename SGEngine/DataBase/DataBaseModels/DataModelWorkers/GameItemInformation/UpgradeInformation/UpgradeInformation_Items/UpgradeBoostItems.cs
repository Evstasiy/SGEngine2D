using Assets.Scripts.SGEngine.DataBase.DataBaseModels;
using Assets.Scripts.SGEngine.DataBase.DataBaseModels.DataModelWorkers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeBoostItems : UpgradeBaseObjectWorker, IDataBaseWorker {

    public List<UpgradeBoostItem> allUpgradeBoostItems { get; private set; }
    public List<UpgradeBoostItem> saveUpgradeBoostItems { get; private set; }

    List<UpgradeBaseObject> baseUpgradeObjectItems;

    Upgrades xmlUpgrades;
    Save_Upgrades saveUpgrades;
    SaveGameInformation saveGameInformation;

    public UpgradeBoostItems(Upgrades xmlUpgrades, SaveGameInformation saveGameInformation) {
        this.xmlUpgrades = xmlUpgrades;
        this.saveGameInformation = saveGameInformation;
        saveUpgrades = saveGameInformation.Save_Upgrades;
    }

    public bool BuildUpgrades() {

        return true;
    }

    /// <summary>
    /// Метод конвертации базовых объектов в игровые
    /// </summary>
    /// <returns>Игровые объекты</returns>
    private List<UpgradeBoostItem> ConvertBaseObjectXmlToObjectList() {
        List<UpgradeBoostItem> allUpgradeObjectItems = new List<UpgradeBoostItem>();
        var xmlBoostAssignmentItems = xmlUpgrades.Upgrade_Items.Boost_Items.UpgradeBoostAssignment_Items.boostAssignment_Item;

        foreach (var baseItem in baseUpgradeObjectItems) {
            try {
                UpgradeBoostItem upgradeObjectItem = UpgradeBoostItem.ConvertUpgradeBaseObject(baseItem);
                var xmlBoostAssignmentItemsForItem = xmlBoostAssignmentItems.Where(x => x.IdUpgrade == baseItem.Id).ToArray();
                UpgradeBoostInfoObject[] upgradeBoostInfoObjects = new UpgradeBoostInfoObject[xmlBoostAssignmentItemsForItem.Length];
                for (int i = 0; i < xmlBoostAssignmentItemsForItem.Length; i++) {
                    upgradeBoostInfoObjects[i] = new UpgradeBoostInfoObject() {
                        idBoostObject = xmlBoostAssignmentItemsForItem[i].idBoost_Item,
                        PercentBoost = xmlBoostAssignmentItemsForItem[i].PercentBoost
                    };
                }
                upgradeObjectItem.BoostObjectsInfo = upgradeBoostInfoObjects;
                allUpgradeObjectItems.Add(upgradeObjectItem);
            } catch (Exception ex) {
                Debug.LogError(ex.Message);
                continue;
            }
        }
        return allUpgradeObjectItems;
    }

    /// <summary>
    /// Метод конвертации информации из базы в игровые объекты
    /// </summary>
    /// <returns>Игровые объекты</returns>
    private List<UpgradeBoostItem> ConverSaveXmlToObjectList() {
        var saveItemsXml = saveUpgrades.Save_Upgrade_Items.Save_Boost_Items.Save_UpdateBoostObjectItem;
        saveItemsXml = saveItemsXml.Where(x => allUpgradeBoostItems.Any(y => y.Id == x.Id)).ToList();

        List<UpgradeBoostItem> saveGameItems = new List<UpgradeBoostItem>();
        foreach (var paramsItem in saveItemsXml) {
            try {
                var idItem = paramsItem.Id;
                var gameItemInAll = allUpgradeBoostItems.FirstOrDefault(x => x.Id == paramsItem.Id);
                UpgradeBoostItem gameItem = new UpgradeBoostItem() {
                    Id = idItem,
                    Name = gameItemInAll.Name,
                    Description = gameItemInAll.Description
                };
                saveGameItems.Add(gameItem);
            } catch (Exception ex) {
                Debug.LogError(ex.Message);
                continue;
            }
        }

        return saveGameItems;
    }

    public bool ReloadObjectsLanguage(GameLocalization gameLocalization) {
        try {
            baseUpgradeObjectItems = ConvertXmlToObjectList(xmlUpgrades.Upgrade_Items.Boost_Items.UpgradeBoost_Items.updateObject, gameLocalization.Upgrades_Localization.Upgrade_Items_Localization.Boost_Items_Localization.descriptionItems);
            allUpgradeBoostItems = ConvertBaseObjectXmlToObjectList();
            saveUpgradeBoostItems = ConverSaveXmlToObjectList();
            return true;
        } catch (Exception ex) {
            Debug.LogError(ex.Message);
            return false;
        }
    }

    /// <summary>
    /// Добавляет запись в базу данных сохранения
    /// </summary>
    /// <param name="Item">Объект, который будут сохранен в БД</param>
    /// <returns></returns>
    public bool AddItemInSaveFile(UpgradeBoostItem Item) {
        try {
            var allSaveItems = saveGameInformation.Save_Upgrades.Save_Upgrade_Items.Save_Boost_Items.Save_UpdateBoostObjectItem;
            allSaveItems.Add(Item);

            saveGameInformation.Save_Upgrades.Save_Upgrade_Items.Save_Boost_Items.Save_UpdateBoostObjectItem = allSaveItems;
            DataBaseManager.dataBaseManager.SaveChanges(saveGameInformation);
        } catch (Exception ex) {
            Debug.LogError($"{ex.Message} \n {ex.StackTrace} \n MethodName: AddItemInSaveFile()");
        }
        return true;
    }

    public void UpdateSaveObjects(SaveGameInformation saveGameInformation) {
        this.saveGameInformation = saveGameInformation;
        saveUpgradeBoostItems = ConverSaveXmlToObjectList();
    }
}

