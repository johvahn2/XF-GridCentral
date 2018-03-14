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
    public class QuestionService
    {
        private static QuestionService instance;

        public static QuestionService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new QuestionService();
                }

                return instance;
            }
        }

        public async Task<string> PostQuestion(mQuestion question)//server error
        {
            try
            {
                var Questioncreds = Newtonsoft.Json.JsonConvert.SerializeObject(question);

                HttpContent ItemContent = new StringContent(Questioncreds, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = await client.PostAsync(Keys.Url_Main + "item-question/post", ItemContent);

                    using (HttpContent spawn = response.Content)
                    {
                        string content = await spawn.ReadAsStringAsync();

                        mServerCallback callback = Newtonsoft.Json.JsonConvert.DeserializeObject<mServerCallback>(content);

                        if (callback.Status == "true")
                        {
                            return "true";
                        }

                        DialogService.ShowError(callback.Mess);
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
                return Strings.HttpFailed;
            }
        }

        public async Task<ObservableCollection<mQuestion>> FetchQuestions(string link,int len,int amount,string txt=null)
        {

            try
            {
                if (txt == null)
                {
                    link += "?len=" + len + "&amount=" + amount;
                }
                else
                {
                    link +="?Search=" + txt + "&len=" + len + "&amount=" + amount;
                }
                var httpClient = new HttpClient();


                var response = await httpClient.GetAsync(link);

                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();

                mServerCallback callback = Newtonsoft.Json.JsonConvert.DeserializeObject<mServerCallback>(content);

                if (callback.Status == "true")
                {
                    ObservableCollection<mQuestion> newitems = new ObservableCollection<mQuestion>();
                    newitems = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<mQuestion>>(callback.Data.ToString());
                    if (newitems.Count < 1)
                    {
                        return null;
                    }

                    for (var i = 0; i < newitems.Count; i++)
                    {
                        if (newitems[i].Answer == null)
                        {
                            newitems[i].Answer = "*No Answer Yet*";
                        }
                    }

                    return newitems;
                }
                else
                {
                    DialogService.ShowError(callback.Mess);
                    return null;

                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
                DialogService.ShowErrorToast(Strings.HttpFailed);
                return null;
            }
        }

        public async Task<mQuestion> FetchQuestion(string id)
        {
            try
            {

                var httpClient = new HttpClient();

                var response = await httpClient.GetAsync(Keys.Url_Main+"item-question/question/"+id);

                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();

                mServerCallback callback = Newtonsoft.Json.JsonConvert.DeserializeObject<mServerCallback>(content);

                if (callback.Status == "true")
                {
                    mQuestion newitems = new mQuestion();
                    newitems = Newtonsoft.Json.JsonConvert.DeserializeObject<mQuestion>(callback.Data.ToString());
                    if (newitems == null)
                    {
                        return null;
                    }

                    if (newitems.Answer == null)
                    {
                        newitems.Answer = "*No Answer Yet*";
                    }
                    
                    return newitems;
                }
                else
                {
                    DialogService.ShowError(callback.Mess);
                    return null;

                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
                DialogService.ShowErrorToast(Strings.HttpFailed);
                return null;
            }
        }

        public async Task<string> EditQuestion(mQuestion question)
        {
            try
            {
                var Questioncreds = Newtonsoft.Json.JsonConvert.SerializeObject(question);

                HttpContent ItemContent = new StringContent(Questioncreds, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = await client.PutAsync(Keys.Url_Main + "item-question/edit-question", ItemContent);

                    using (HttpContent spawn = response.Content)
                    {
                        string content = await spawn.ReadAsStringAsync();

                        mServerCallback callback = Newtonsoft.Json.JsonConvert.DeserializeObject<mServerCallback>(content);

                        if (callback.Status == "true")
                        {
                            return "true";
                        }

                        DialogService.ShowError(callback.Mess);
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
                return Strings.HttpFailed;
            }
        }

        public async Task<string> EditAnswer(mQuestion question)
        {
            try
            {
                var Questioncreds = Newtonsoft.Json.JsonConvert.SerializeObject(question);

                HttpContent ItemContent = new StringContent(Questioncreds, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = await client.PutAsync(Keys.Url_Main + "item-question/edit-answer", ItemContent);

                    using (HttpContent spawn = response.Content)
                    {
                        string content = await spawn.ReadAsStringAsync();

                        mServerCallback callback = Newtonsoft.Json.JsonConvert.DeserializeObject<mServerCallback>(content);

                        if (callback.Status == "true")
                        {
                            return "true";
                        }

                        DialogService.ShowError(callback.Mess);
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
                return Strings.HttpFailed;
            }
        }

        public async Task<string> AnswerQuestion(mQuestion question)
        {
            try
            {
                var Questioncreds = Newtonsoft.Json.JsonConvert.SerializeObject(question);

                HttpContent ItemContent = new StringContent(Questioncreds, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = await client.PutAsync(Keys.Url_Main + "item-question/answer", ItemContent);

                    using (HttpContent spawn = response.Content)
                    {
                        string content = await spawn.ReadAsStringAsync();

                        mServerCallback callback = Newtonsoft.Json.JsonConvert.DeserializeObject<mServerCallback>(content);

                        if (callback.Status == "true")
                        {
                            return "true";
                        }

                        DialogService.ShowError(callback.Mess);
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(Keys.TAG + ex);
                return Strings.HttpFailed;
            }
        }
    }
}
