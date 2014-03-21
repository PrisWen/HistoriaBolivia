using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;

namespace BackGroundTask3
{
    public sealed class TileUpdate3 : IBackgroundTask
    {
        private List<string> myLista3;
        private List<string> myLista4;
        public TileUpdate3()
        {
            myLista3 = new List<string>();
            myLista3.Add("LA PAZ");
            myLista3.Add("ORURO");
            myLista3.Add("POTOSI");
            myLista3.Add("CHUQUISACA");
            myLista3.Add("TARIJA");
            myLista3.Add("COCHABAMBA");
            myLista3.Add("BENI");
            myLista3.Add("PANDO");
            myLista3.Add("SANTA CRUZ");

            myLista4 = new List<string>();
            myLista4.Add("Fecha cívica: 16 de Julio de (1809)");
            myLista4.Add("Fecha cívica: 10 de Febrero (1781)");
            myLista4.Add("Fecha cívica: 10 de Noviembre (1810");
            myLista4.Add("Fecha cívica: 25 de Mayo (1809)");
            myLista4.Add("Fecha cívica: 15 de Abril (1817)");
            myLista4.Add("Fecha cívica: 14 de Septiembre (1810)");
            myLista4.Add("Fecha cívica: 18 de Noviembre (1842)");
            myLista4.Add("Fecha cívica: 24 de Septiembre (1938)");
            myLista4.Add("Fecha cívica: 24 de Septiembre (1810)");
        }

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            var defferal = taskInstance.GetDeferral();

            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.EnableNotificationQueue(true);

            updater.Clear();

            for (int i = 0; i <9; i++)
            {
                var tile = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWideText09);
                tile.GetElementsByTagName("text")[0].InnerText = "" + myLista3[i];
                tile.GetElementsByTagName("text")[1].InnerText = ""+myLista4[i];

                updater.Update(new TileNotification(tile));
            }

            defferal.Complete();
        }
    }
}
