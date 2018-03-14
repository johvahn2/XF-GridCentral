using GridCentral.Helpers;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GridCentral.Services
{
    public static class OfflineService
    {
        //private static OfflineService instance;

        //public static OfflineService Instance
        //{
        //    get
        //    {
        //        if (instance == null)
        //        {
        //            instance = new OfflineService();
        //        }

        //        return instance;
        //    }
        //}


        public static async Task Write<T>(T result, string fileName, IFolder folder)
        {
            string Itemcreds = Newtonsoft.Json.JsonConvert.SerializeObject(result);

            await PCLHelper.CreateFile(fileName, folder);
            await PCLHelper.WriteTextAllAsync(fileName, Itemcreds,folder);

        }

        public static async Task<T> Read<T>(string fileName, IFolder folder)
        {
            if (await PCLHelper.IsFileExistAsync(fileName))
            {
                string content = await PCLHelper.ReadAllTextAsync(fileName,folder);

                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(content);

            }

            return default(T);
        }


    }
}
