using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.SGEngine.DataBase.DataBaseModels.DataModelWorkers
{
    interface IDataBaseWorker
    {
        /// <summary>
        /// Метод перезаписи игровых элементов с новой локализацией
        /// </summary>
        /// <param name="gameLocalization">Файл локализации</param>
        /// <returns></returns>
        bool ReloadObjectsLanguage(GameLocalization gameLocalization);

        /// <summary>
        /// Обновлает объекты из файла сохранения
        /// </summary>
        /// <param name="saveGameInformation">Файл сохранения</param>
        void UpdateSaveObjects(SaveGameInformation saveGameInformation);
    }
}
