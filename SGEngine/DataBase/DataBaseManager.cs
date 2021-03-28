using Assets.Scripts.SGEngine.DataBase.DataBaseModels.DataModelWorkers;
using Assets.Scripts.SGEngine.DataBase.DataBaseModels.DataModelWorkers.GameItemInformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.SGEngine.DataBase.DataBaseModels
{
    public class DataBaseManager : MonoBehaviour
    {
        public static DataBaseManager dataBaseManager = null;
        private DataBaseComponent dataBaseComponent;

        private string PATH_SAVE_FILE;

        
        private SaveGameInformation saveGameInformation;
        private GameParameters xmlGameInformation;

        #region Workers
        List<IDataBaseWorker> dataBaseWorkers = new List<IDataBaseWorker>();
        public GameItemWorker GameItemWorker { get; private set; }
        public UpgradeNewObjectsWorker UpgradeNewObjectsWorker { get; private set; }
        public UpgradBoostObjectsWorker UpgradBoostObjectsWorker { get; private set; }
        #endregion

        public delegate void UpdateInfoInSaveFile(SaveGameInformation saveGameInformation);
        public event UpdateInfoInSaveFile isChangeSaveFile;

        private void Awake() {
            if (dataBaseManager == null)
                dataBaseManager = this;
            else if (dataBaseManager == this)
                Destroy(gameObject);

            PATH_SAVE_FILE = Application.persistentDataPath + @"/Save/UserSaveFile";
            DontDestroyOnLoad(gameObject);
            Debug.Log(PATH_SAVE_FILE);
            dataBaseComponent = new DataBaseComponent();
            dataBaseComponent.CheckBaseFiles();
            SetupSettings();
            Test();
        }
        private void Test()
        {

        }
         
        /// <summary>
        /// Начальная настройка менеджера, где запрашивается вся информация из базы
        /// </summary>
        private void SetupSettings() {
            saveGameInformation = dataBaseComponent.GetXMLSaveGameInformation();
            xmlGameInformation = dataBaseComponent.GetXMLGameParameters();


            GameItemWorker = new GameItemWorker(xmlGameInformation, saveGameInformation);
            UpgradeNewObjectsWorker = new UpgradeNewObjectsWorker(xmlGameInformation, saveGameInformation);
            UpgradBoostObjectsWorker = new UpgradBoostObjectsWorker(xmlGameInformation, saveGameInformation);

            dataBaseWorkers.Add(GameItemWorker);
            dataBaseWorkers.Add(UpgradeNewObjectsWorker.upgradeNewItems);
            dataBaseWorkers.Add(UpgradBoostObjectsWorker.upgradeBoostItems);

            foreach (var dataBaseWorker in dataBaseWorkers) {
                isChangeSaveFile += dataBaseWorker.UpdateSaveObjects;
            }

            ReloadLocalization(LocalizationOption.LocalizationRegion.Eng);
        }

        /// <summary>
        /// Перезагружает локализацию в игровые объекты воркеров
        /// </summary>
        /// <param name="localizationRegion"></param>
        public void ReloadLocalization(LocalizationOption.LocalizationRegion localizationRegion) {
            GameLocalization gameLocalization = dataBaseComponent.GetGlobalLocalizationFile(localizationRegion);
            foreach (var dataBaseWorker in dataBaseWorkers) {
                dataBaseWorker.ReloadObjectsLanguage(gameLocalization);
            }
        }

        /// <summary>
        /// Сохраняет изменения в БД
        /// </summary>
        /// <param name="saveGameInformation">Полная информация с изменениями</param>
        /// <returns>Вернет true, если операция успешна</returns>
        public bool SaveChanges(SaveGameInformation saveGameInformation) {
            try {
                dataBaseComponent.WriteInSaveFile(saveGameInformation);
                isChangeSaveFile(saveGameInformation);
                return true;
            }
            catch (Exception ex) {
                Debug.LogError(ex.Message + "  " + ex.StackTrace);
                return false;
            }
        }
    }

}
