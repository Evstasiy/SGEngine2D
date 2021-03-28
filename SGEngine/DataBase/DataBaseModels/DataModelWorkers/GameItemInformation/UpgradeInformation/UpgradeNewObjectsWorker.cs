using Assets.Scripts.SGEngine.DataBase.DataBaseModels.DataModelWorkers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeNewObjectsWorker {
    List<IDataBaseWorker> databaseNewWorkers = new List<IDataBaseWorker>();

    #region UpgradeNewObjects
    public UpgradeNewItems upgradeNewItems { get; private set; }
    #endregion UpgradeNewObjects

    public UpgradeNewObjectsWorker(GameParameters xmlGameInformation, SaveGameInformation saveGameInformation) {
        upgradeNewItems = new UpgradeNewItems(xmlGameInformation.Upgrades, saveGameInformation);

        databaseNewWorkers.Add(upgradeNewItems);
    }

}
