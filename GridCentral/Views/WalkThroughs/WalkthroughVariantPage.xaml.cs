using GridCentral.Services;
using GridCentral.ViewModels;
using GridCentral.Views.Navigation.Home;
using GridCentral.Views.Navigation.WalkThroughs.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GridCentral.Views.Navigation.WalkThroughs
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WalkthroughVariantPage : CarouselPage
    {

        PageService _pageService = new PageService();

        public WalkthroughVariantPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            this.BindingContext = new WalkthroughViewModel(Close, GoToStep);

        }

        private async Task GoToStep()
        {
            var index = Children.IndexOf(CurrentPage);
            var moveToIndex = 0;
            if (index < Children.Count - 1)
            {
                moveToIndex = index + 1;

                SelectedItem = Children[moveToIndex];
            }
            else
            {
                await Close();
            }
        }

        private async Task Close()
        {
            //await Navigation.PopModalAsync();
            //await _pageService.PushAsync(new DashBoard());
            _pageService.ShowMain(new DashBoard());
        }

        protected async override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
            var currentStep = (WalkthroughVariantStepItemTemplate)CurrentPage;

            await currentStep.AnimateIn();
        }
    }
}
