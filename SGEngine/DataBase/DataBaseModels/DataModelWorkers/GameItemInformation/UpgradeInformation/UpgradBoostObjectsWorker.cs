using Assets.Scripts.SGEngine.DataBase.DataBaseModels.DataModelWorkers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradBoostObjectsWorker {
    List<IDataBaseWorker> databaseNewWorkers = new List<IDataBaseWorker>();

    #region UpgradeBoostObjects
    public UpgradeBoostItems upgradeBoostItems { get; private set; }
    #endregion UpgradeBoostObjects

    public UpgradBoostObjectsWorker(GameParameters xmlGameInformation, SaveGameInformation saveGameInformation) {
        upgradeBoostItems = new UpgradeBoostItems(xmlGameInformation.Upgrades, saveGameInformation);

        databaseNewWorkers.Add(upgradeBoostItems);
    }
}
