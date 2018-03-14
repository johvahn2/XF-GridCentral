using GridCentral.Services;
using GridCentral.ViewModels;
using Plugin.ImageResizer;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GridCentral.Views.Profile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddItem : ContentPage
    {
        public AddItem()
        {
            viewModel = new Profile_AddItem_ViewModel(new PageService(Navigation));
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
            Gestures();
        }

        public Profile_AddItem_ViewModel viewModel
        {
            get { return BindingContext as Profile_AddItem_ViewModel; }
            set { BindingContext = value; }
        }

        public void Gestures()
        {
            Img1.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async () =>
                {

                    byte[] arr = await GetImage();
                    if (arr == null)
                    {
                        viewModel.Img1 = null;
                        Img1.Source = "";
                        return;
                    }
                    if (arr.Length > 0)
                    {

                        var source = ImageSource.FromStream(() => new MemoryStream(arr));
                        Img1.Source = source;

                        viewModel.Img1 = arr;
                    }
                    else
                    {
                        viewModel.Img1 = null;
                    }

                })
            });

            Img2.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async () =>
                {
                    byte[] arr = await GetImage();
                    if (arr == null)
                    {
                        viewModel.Img2 = null;
                        Img2.Source = "";
                        return;
                    }
                    if (arr.Length > 0)
                    {

                        var source = ImageSource.FromStream(() => new MemoryStream(arr));
                        Img2.Source = source;

                        viewModel.Img2 = arr;
                    }
                    else
                    {
                        viewModel.Img2 = null;
                    }

                })
            });

            Img3.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async () =>
                {
                    byte[] arr = await GetImage();
                    if (arr == null)
                    {
                        viewModel.Img3 = null;
                        Img3.Source = "";
                        return;
                    }
                    if (arr.Length > 0)
                    {

                        var source = ImageSource.FromStream(() => new MemoryStream(arr));
                        Img3.Source = source;

                        viewModel.Img3 = arr;
                    }
                    else
                    {
                        viewModel.Img3 = null;
                    }

                })
            });

            Img4.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async () =>
                {

                    byte[] arr = await GetImage();
                    if (arr == null)
                    {
                        viewModel.Img4 = null;
                        Img4.Source = "";
                        return;
                    }
                    if (arr.Length > 0)
                    {

                        var source = ImageSource.FromStream(() => new MemoryStream(arr));
                        Img4.Source = source;

                        viewModel.Img4 = arr;
                    }
                    else
                    {
                        viewModel.Img4 = null;
                    }

                })
            });
        }


        private async Task<byte[]> GetImage()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return null;
            }

            MediaFile file = null;
            var action = await DisplayActionSheet("Image Options", "Cancel", null, "Selected Picture","Take Picture");

            if (action == "Selected Picture")
            {
                file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    CompressionQuality = 50,
                    PhotoSize = PhotoSize.Medium
                });

            }
            else if (action == "Take Picture")
            {
                file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    CompressionQuality = 100,
                    PhotoSize = PhotoSize.Full
                   
                });
                //DialogService.ShowErrorToast("Not Available");
                //return null;
            }

            if (file == null)
                return null;


            byte[] resizedImage = await CrossImageResizer.Current.ResizeImageWithAspectRatioAsync(file.GetStream(), 1000, 1000);
            return resizedImage;
            //var stream = file.GetStream();
            //byte[] buffer = new byte[stream.Length];
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    stream.CopyTo(ms);
            //    buffer = ms.ToArray();
            //    file.Dispose();

            //    return buffer;
            //}

        }
    }
}