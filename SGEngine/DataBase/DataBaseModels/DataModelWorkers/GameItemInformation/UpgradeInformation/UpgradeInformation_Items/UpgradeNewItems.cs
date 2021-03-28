using Assets.Scripts.SGEngine.DataBase.DataBaseModels;
using Assets.Scripts.SGEngine.DataBase.DataBaseModels.DataModelWorkers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeNewItems : UpgradeBaseObjectWorker, IDataBaseWorker {

    public List<UpgradeObjectItem> allUpgradeObjectItems { get; private set; }
    public List<UpgradeObjectItem> saveUpgradeObjectItems { get; private set; }
    List<UpgradeBaseObject> baseUpgradeObjectItems;

    SaveGameInformation saveGameInformation;
    Upgrades xmlUpgrades;
    Save_Upgrades saveUpgrades;

    public UpgradeNewItems(Upgrades xmlUpgrades, SaveGameInformation saveGameInformation) {
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
    private List<UpgradeObjectItem> ConvertBaseObjectXmlToObjectList() {
        List<UpgradeObjectItem> allUpgradeObjectItems = new List<UpgradeObjectItem>();
        List<newAssignment_Item> xmlNewItemsAssignment = xmlUpgrades.Upgrade_Items.New_Items.UpgradeNewAssignment_Items.newAssignment_Items;

        foreach (var baseNewItem in baseUpgradeObjectItems) {
            try {
                UpgradeObjectItem upgradeObjectItem = UpgradeObjectItem.ConvertUpgradeBaseObject(baseNewItem);
                upgradeObjectItem.OpenIds = xmlNewItemsAssignment.Where(x => x.IdUpgrade == baseNewItem.Id).Select(x => x.IdNew).ToArray();
                allUpgradeObjectItems.Add(upgradeObjectItem);
            }
            catch (Exception ex) {
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
    private List<UpgradeObjectItem> ConverSaveXmlToObjectList() {
        var saveItemsXml = saveUpgrades.Save_Upgrade_Items.Save_New_Items.Save_UpdateNewOjectItem;
        saveItemsXml = saveItemsXml.Where(x => allUpgradeObjectItems.Any(y => y.Id == x.Id)).ToList();

        List<UpgradeObjectItem> saveGameItems = new List<UpgradeObjectItem>();
        foreach (var paramsItem in saveItemsXml) {
            try {
                var idItem = paramsItem.Id;
                var gameItemInAll = allUpgradeObjectItems.FirstOrDefault(x => x.Id == paramsItem.Id);
                UpgradeObjectItem gameItem = new UpgradeObjectItem() {
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
            baseUpgradeObjectItems = ConvertXmlToObjectList(xmlUpgrades.Upgrade_Items.New_Items.UpgradeNew_Items.updateObject, gameLocalization.Upgrades_Localization.Upgrade_Items_Localization.New_Items_Localization.descriptionItems);
            allUpgradeObjectItems = ConvertBaseObjectXmlToObjectList();
            saveUpgradeObjectItems = ConverSaveXmlToObjectList();
            return true;
        }
        catch (Exception ex) {
            Debug.LogError(ex.Message);
            return false;
        }
    }

    /// <summary>
    /// Добавляет запись в базу данных сохранения
    /// </summary>
    /// <param name="Item">Объект, который будут сохранен в БД</param>
    /// <returns></returns>
    public bool AddItemInSaveFile(UpgradeObjectItem Item) {
        try {
            var allSaveItems = saveGameInformation.Save_Upgrades.Save_Upgrade_Items.Save_New_Items.Save_UpdateNewOjectItem;
            allSaveItems.Add(Item);

            saveGameInformation.Save_Upgrades.Save_Upgrade_Items.Save_New_Items.Save_UpdateNewOjectItem = allSaveItems;
            DataBaseManager.dataBaseManager.SaveChanges(saveGameInformation);
        } catch (Exception ex) {
            Debug.LogError($"{ex.Message} \n {ex.StackTrace} \n MethodName: AddItemInSaveFile()");
        }
        return true;
    }

    public void UpdateSaveObjects(SaveGameInformation saveGameInformation) {
        this.saveGameInformation = saveGameInformation;
        saveUpgradeObjectItems = ConverSaveXmlToObjectList();
    }
}
