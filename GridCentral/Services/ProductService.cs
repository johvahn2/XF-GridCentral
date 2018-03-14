using GridCentral.Helpers;
using GridCentral.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GridCentral.Services
{
    public class ProductService
    {
        private static ProductService instance;

        public static ProductService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProductService();
                }

                return instance;
            }
        }


        private ObservableCollection<Product> CalculatePercentages(ObservableCollection<Product> products)
        {
            for (int i = 0; i < products.Count; i++)
            {
                if(products[i].IsDeal == "True")
                {
                    var offset = (Convert.ToDouble(products[i].DealPrice) / Convert.ToDouble(products[i].Price)) * 100;


                    products[i].DealPercentage = (100 - Convert.ToInt16(offset)).ToString();
                }
            }

            return products;
        }

        private Product CalculatePercentage(Product product)
        {
            if (product.IsDeal == "true")
            {
                var offset = (Convert.ToDouble(product.DealPrice) / Convert.ToDouble(product.Price)) * 100;

                product.DealPercentage = (100 - Convert.ToInt16(offset)).ToString();
            }

            return product;

        }


        public async Task<ObservableCollection<Product>> FetchUserInterets(string UserEmail)
        {
            try
            {
                var httpClient = new HttpClient();

                var response = await httpClient.GetAsync(Keys.Url_Main + "product/interests/" + UserEmail);//product/interests/

                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();

                mServerCallback callback = Newtonsoft.Json.JsonConvert.DeserializeObject<mServerCallback>(content);

                if (callback.Status == "true")
                {
                    ObservableCollection<Product> newitems = new ObservableCollection<Product>();

                    newitems = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<Product>>(callback.Data.ToString());


                    if (newitems.Count < 1) return null;

                    newitems = CalculatePercentages(newitems);

                    return newitems;
                }
                else
                {

                    DialogService.ShowError(Strings.ServerFailed);
                    return null;
                }


            }
            catch (Exception ex)
            {
                DialogService.ShowErrorToast(Strings.HttpFailed);
                Debug.WriteLine(Keys.TAG + ex);
                return null;
            }
        }


        public async Task<ObservableCollection<Product>> FetchRandom(int amount, string category, string by,int len)
        {
            var link = Keys.Url_Main + "product/random?amount=" + amount.ToString()+"&len=" + len.ToString();
            if (!String.IsNullOrEmpty(category))
            {
                link += "&category=" + category;
            }
            if (!String.IsNullOrEmpty(by))
            {
                link += "&by=" + by;
            }

            try
            {
                var httpClient = new HttpClient();


                var response = await httpClient.GetAsync(link);

                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();


                mServerCallback callback = Newtonsoft.Json.JsonConvert.DeserializeObject<mServerCallback>(content);

                if (callback.Status == "true")
                {
                    ObservableCollection<Product> newitems = new ObservableCollection<Product>();
                    newitems = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<Product>>(callback.Data.ToString());
                    if (newitems.Count < 1) return null;

                    newitems = CalculatePercentages(newitems);

                    return newitems;

                }

                DialogService.ShowError(callback.Mess);
                return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
                DialogService.ShowErrorToast(Strings.HttpFailed);
                return null;
            }
        }

        public async Task<ObservableCollection<Product>> FetchRecent(int amount, string category, string by, int len)
        {
            var link = Keys.Url_Main + "product/recent?amount=" + amount.ToString() + "&len=" + len.ToString();
            if (!String.IsNullOrEmpty(category))
            {
                link += "&category=" + category;
            }
            if (!String.IsNullOrEmpty(by))
            {
                link += "&by=" + by;
            }

            try
            {
                var httpClient = new HttpClient();


                var response = await httpClient.GetAsync(link);

                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();


                mServerCallback callback = Newtonsoft.Json.JsonConvert.DeserializeObject<mServerCallback>(content);

                if (callback.Status == "true")
                {
                    ObservableCollection<Product> newitems = new ObservableCollection<Product>();
                    newitems = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<Product>>(callback.Data.ToString());
                    if (newitems.Count < 1) return null;

                    newitems = CalculatePercentages(newitems);


                    return newitems;

                }

                DialogService.ShowError(callback.Mess);
                return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
                DialogService.ShowErrorToast(Strings.HttpFailed);
                return null;
            }
        }

        public async Task<ObservableCollection<Product>> FetchSearch(string link)
        {
            try
            {
                var httpClient = new HttpClient();


                var response = await httpClient.GetAsync(link);

                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();

                mServerCallback callback = Newtonsoft.Json.JsonConvert.DeserializeObject<mServerCallback>(content);

                if (callback.Status == "true")
                {
                    ObservableCollection<Product> newitems = new ObservableCollection<Product>();
                    newitems = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<Product>>(callback.Data.ToString());
                    if (newitems.Count < 1) return null;

                    newitems = CalculatePercentages(newitems);


                    return newitems;

                }

                DialogService.ShowError(callback.Mess);
                return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
                DialogService.ShowError(Strings.HttpFailed);
                return null;
            }
        }


        public async Task<ObservableCollection<Product>> FetchCategory(string category,int amount, int len)
        {
            try
            {
                var link = Keys.Url_Main + "product/"+category + "/list/?amount=" + amount.ToString() + "&len=" + len.ToString();

                var httpClient = new HttpClient();


                var response = await httpClient.GetAsync(link);

                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();

                mServerCallback callback = Newtonsoft.Json.JsonConvert.DeserializeObject<mServerCallback>(content);

                if (callback.Status == "true")
                {
                    ObservableCollection<Product> newitems = new ObservableCollection<Product>();
                    newitems = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<Product>>(callback.Data.ToString());
                    if (newitems.Count < 1) return null;

                    newitems = CalculatePercentages(newitems);


                    return newitems;

                }

                DialogService.ShowError(callback.Mess);
                return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
                DialogService.ShowError(Strings.HttpFailed);
                return null;
            }
        }

        public async Task<ObservableCollection<Product>> FetchDeals(int amount, string category, string by, int len)
        {
            var link = Keys.Url_Main + "product/deals?amount=" + amount.ToString() + "&len=" + len.ToString();
            if (!String.IsNullOrEmpty(category))
            {
                link += "&category=" + category;
            }
            if (!String.IsNullOrEmpty(by))
            {
                link += "&by=" + by;
            }

            try
            {
                var httpClient = new HttpClient();


                var response = await httpClient.GetAsync(link);

                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();


                mServerCallback callback = Newtonsoft.Json.JsonConvert.DeserializeObject<mServerCallback>(content);

                if (callback.Status == "true")
                {
                    ObservableCollection<Product> newitems = new ObservableCollection<Product>();
                    newitems = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<Product>>(callback.Data.ToString());
                    if (newitems.Count < 1) return null;

                    newitems = CalculatePercentages(newitems);


                    return newitems;

                }

                DialogService.ShowError(callback.Mess);
                return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
                DialogService.ShowError(Strings.HttpFailed);
                return null;
            }
        }

        public async Task<Product> FetchProduct(string Id)
        {
            try
            {
                var httpClient = new HttpClient();

                var response = await httpClient.GetAsync(Keys.Url_Main + "product/get/" + Id);

                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();

                mServerCallback callback = Newtonsoft.Json.JsonConvert.DeserializeObject<mServerCallback>(content);

                if (callback.Status == "true")
                {
                    Product newitem = new Product();

                    newitem = Newtonsoft.Json.JsonConvert.DeserializeObject<Product>(callback.Data.ToString());


                    if (newitem == null) return null;

                    newitem = CalculatePercentage(newitem);

                    return newitem;
                }
                else
                {

                    DialogService.ShowError(Strings.ServerFailed);
                    return null;
                }


            }
            catch (Exception ex)
            {
                DialogService.ShowError(Strings.HttpFailed);
                Debug.WriteLine(Keys.TAG + ex);
                return null;
            }
        }

        public async Task<string> RateProduct(mRate rate)
        {
            try
            {
                var Itemcreds = Newtonsoft.Json.JsonConvert.SerializeObject(rate);

                HttpContent ItemContent = new StringContent(Itemcreds, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {

                    HttpResponseMessage response = await client.PutAsync(Keys.Url_Main + "product/rate/post", ItemContent);

                    using (HttpContent spawn = response.Content)
                    {
                        string content = await spawn.ReadAsStringAsync();

                        mServerCallback callback = Newtonsoft.Json.JsonConvert.DeserializeObject<mServerCallback>(content);

                        if (callback.Status == "true")
                        {
                            return "true";
                        }

                        DialogService.ShowError(Strings.ServerFailed);
                        return callback.Mess;
                    }

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
                return Strings.ServerFailed;
            }
        }
    }
}
