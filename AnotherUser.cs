using Assets.Scripts.SGEngine.DataBase.DataBaseModels;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnotherUser : MonoBehaviour
{
    public DataBaseManager dataBaseManager;
    void Start() {
        dataBaseManager = DataBaseManager.dataBaseManager;
        var allItems = dataBaseManager.UpgradeNewObjectsWorker.upgradeNewItems.allUpgradeObjectItems;
        var save = dataBaseManager.UpgradeNewObjectsWorker.upgradeNewItems.saveUpgradeObjectItems;

        
        /*var g = dataBaseManager.GameItemWorker;
        var all = g.allGameItems;
        var allSave = g.allGameItemsSave;
        GameItem gam = new GameItem() { Id = 99, Name = "Kek" };
        GameItem gam1 = new GameItem() { Id = 7, Name = "Kektr5", IsLock = true };
        GameItem gam3 = new GameItem() { Id = 9, Name = "Kek5", IsLock = true };
        GameItem gam4 = new GameItem() { Id = 4, Name = "Ke98k5", IsLock = false };
        allSave.Add(gam);
        allSave.Add(gam1);
        allSave.Remove(allSave.First(x=>x.Id == 4));
        allSave.Add(gam4);

        g.SaveChangesInSaveFile(allSave);*/
    }

    public void AnotherClick() {
    }
}
