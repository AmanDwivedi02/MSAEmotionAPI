using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSAEmotions
{
    public class EasyTablesBackend
    {
        private static EasyTablesBackend instance;
        private MobileServiceClient client;
        private IMobileServiceTable<EmotionTable> emotionsTable;

        private EasyTablesBackend()
        {
            client = new MobileServiceClient("http://spellcheckmsa.azurewebsites.net");
            emotionsTable = client.GetTable<EmotionTable>();
        }

        public MobileServiceClient AzureClient
        {
            get { return client; }
        }

        public static EasyTablesBackend EasyTablesBackendInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EasyTablesBackend();
                }
                return instance;
            }
        }

        public async Task PostEmotion(EmotionTable emotionModel)
        {
            await emotionsTable.InsertAsync(emotionModel);
        }

        public async Task<List<EmotionTable>> GetEmotion()
        {
            return await emotionsTable.ToListAsync();
        }
    }
}
