using GridCentral.Models;
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
    public partial class EditItem : ContentPage
    {
        public EditItem(mUserItem item)
        {
            viewModel = new Profile_EditItem_ViewModel(new PageService(Navigation), item);
            InitializeComponent();

            Gestures();
        }

        private void Gestures()
        {
            Img1.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async () =>
                {
                    var action = await DisplayActionSheet("Image Options", "Cancel", "REMOVE", "Selected Picture","Take Picture");
                    if (action == "Cancel")
                    {
                        return;
                    }
                    else if (action == "REMOVE")
                    {
                        viewModel.Img1 = "Remove";
                        Img1.Source = "";

                    }
                    else if (action == "Selected Picture")
                    {
                        byte[] arr = await GetImage();

                        if (arr == null) return;

                        if (arr.Length > 0)
                        {
                            var source = ImageSource.FromStream(() => new MemoryStream(arr));
                            Img1.Source = source;

                            viewModel.bImg1 = arr;
                        }
                        else
                        {
                            viewModel.bImg1 = null;
                        }
                    }
                    else if (action == "Take Picture")
                    {
                        //DialogService.ShowErrorToast("Not Available");
                        byte[] arr = await GetImage(true);

                        if (arr == null) return;

                        if (arr.Length > 0)
                        {
                            var source = ImageSource.FromStream(() => new MemoryStream(arr));
                            Img1.Source = source;

                            viewModel.bImg1 = arr;
                        }
                        else
                        {
                            viewModel.bImg1 = null;
                        }
                    }
                })
            });

            Img2.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async () =>
                {
                    var action = await DisplayActionSheet("Image Options", "Cancel", "REMOVE", "Selected Picture","Take Picture");
                    if (action == "Cancel")
                    {
                        return;
                    }
                    else if (action == "REMOVE")
                    {
                        viewModel.Img2 = "Remove";
                        Img2.Source = "";
                    }
                    else if (action == "Selected Picture")
                    {
                        byte[] arr = await GetImage();

                        if (arr == null) return;

                        if (arr.Length > 0)
                        {
                            var source = ImageSource.FromStream(() => new MemoryStream(arr));
                            Img2.Source = source;

                            viewModel.bImg2 = arr;
                        }
                        else
                        {
                            viewModel.bImg2 = null;
                        }
                    }
                    else if (action == "Take Picture")
                    {
                        //DialogService.ShowErrorToast("Not Available");
                        byte[] arr = await GetImage(true);

                        if (arr == null) return;

                        if (arr.Length > 0)
                        {
                            var source = ImageSource.FromStream(() => new MemoryStream(arr));
                            Img2.Source = source;

                            viewModel.bImg2 = arr;
                        }
                        else
                        {
                            viewModel.bImg2 = null;
                        }
                    }
                })
            });

            Img3.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async () =>
                {
                    var action = await DisplayActionSheet("Image Options", "Cancel", "REMOVE", "Selected Picture", "Take Picture");
                    if (action == "Cancel")
                    {
                        return;
                    }
                    else if (action == "REMOVE")
                    {
                        viewModel.Img3 = "Remove";
                        Img3.Source = "";
                    }
                    else if (action == "Selected Picture")
                    {
                        byte[] arr = await GetImage();

                        if (arr == null) return;

                        if (arr.Length > 0)
                        {
                            var source = ImageSource.FromStream(() => new MemoryStream(arr));
                            Img3.Source = source;

                            viewModel.bImg3 = arr;
                        }
                        else
                        {
                            viewModel.bImg3 = null;
                        }
                    }
                    else if (action == "Take Picture")
                    {
                        //DialogService.ShowErrorToast("Not Available");
                        byte[] arr = await GetImage(true);

                        if (arr == null) return;

                        if (arr.Length > 0)
                        {
                            var source = ImageSource.FromStream(() => new MemoryStream(arr));
                            Img3.Source = source;

                            viewModel.bImg3 = arr;
                        }
                        else
                        {
                            viewModel.bImg3 = null;
                        }
                    }
                })
            });

            Img4.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async () =>
                {
                    var action = await DisplayActionSheet("Image Options", "Cancel", "REMOVE", "Selected Picture","Take Picture");
                    if (action == "Cancel")
                    {
                        return;
                    }
                    else if (action == "REMOVE")
                    {
                        viewModel.Img4 = "Remove";
                        Img4.Source = "";
                    }
                    else if (action == "Selected Picture")
                    {
                        byte[] arr = await GetImage();

                        if (arr == null) return;

                        if (arr.Length > 0)
                        {
                            var source = ImageSource.FromStream(() => new MemoryStream(arr));
                            Img4.Source = source;

                            viewModel.bImg4 = arr;
                        }
                        else
                        {
                            viewModel.bImg4 = null;
                        }
                    }
                    else if (action == "Take Picture")
                    {
                        //DialogService.ShowErrorToast("Not Available");
                        byte[] arr = await GetImage(true);

                        if (arr == null) return;

                        if (arr.Length > 0)
                        {
                            var source = ImageSource.FromStream(() => new MemoryStream(arr));
                            Img4.Source = source;

                            viewModel.bImg4 = arr;
                        }
                        else
                        {
                            viewModel.bImg4 = null;
                        }
                    }
                })
            });

        }

        private async Task<byte[]> GetImage(bool takepick = false)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return null;
            }

            MediaFile file = null;
            if (takepick)
            {
                file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    CompressionQuality = 50,
                    PhotoSize = PhotoSize.Medium
                });
            }
            else
            {
                file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    CompressionQuality = 100,
                    PhotoSize = PhotoSize.Full
                });

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

        public Profile_EditItem_ViewModel viewModel
        {
            get { return BindingContext as Profile_EditItem_ViewModel; }
            set { BindingContext = value; }
        }

        private void Img1_DownloadProgress(object sender, FFImageLoading.Forms.CachedImageEvents.DownloadProgressEventArgs e)
        {

        }
    }
}