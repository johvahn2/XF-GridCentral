using GridCentral.Models;
using GridCentral.Views;
using GridCentral.Views.Contact;
using GridCentral.Views.Deals;
using GridCentral.Views.Navigation.Home;
using GridCentral.Views.Navigation.Settings;
using GridCentral.Views.Notifaction;
using GridCentral.Views.Order;
using GridCentral.Views.Profile;
using GridCentral.Views.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GridCentral.ViewModels
{
    public class SampleDefinition
    {
        private static List<SampleCategory> _samplesCategoryList;
        private static Dictionary<string, SampleCategory> _samplesCategories;
        private static List<Sample> _allSamples;
        private static List<SampleGroup> _samplesGroupedByCategory;

        public static string[] _categoriesColors = {
            "#921243",
            "#B31250",
            "#CD195E",
            "#56329A",
            "#6A40B9",
            "#7C4ECD",
            "#525ABB",
            "#5F7DD4",
            "#7B96E5"
        };

        public static List<SampleCategory> SamplesCategoryList
        {
            get
            {
                if (_samplesCategoryList == null)
                {
                    InitializeSamples();
                }

                return _samplesCategoryList;
            }
        }

        public static Dictionary<string, SampleCategory> SamplesCategories
        {
            get
            {
                if (_samplesCategories == null)
                {
                    InitializeSamples();
                }

                return _samplesCategories;
            }
        }

        public static List<Sample> AllSamples
        {
            get
            {
                if (_allSamples == null)
                {
                    InitializeSamples();
                }
                return _allSamples;
            }
        }

        public static List<SampleGroup> SamplesGroupedByCategory
        {
            get
            {
                if (_samplesGroupedByCategory == null)
                {
                    InitializeSamples();
                }

                return _samplesGroupedByCategory;
            }
        }


        internal static Dictionary<string, SampleCategory> CreateSamples()
        {
            var categories = new Dictionary<string, SampleCategory>();

            categories.Add(
                "Main",
                new SampleCategory
                {
                    Name = "Main",
                    BackgroundColor = Color.FromHex(_categoriesColors[0]),
                    BackgroundImage = null,
                    Icon = GrialShapesFont.Person,
                    Badge = 2,
                    SamplesList = new List<Sample> {
                        new Sample("GridCentral", typeof(DashBoard), SampleData.DashboardImagesList[3], GrialShapesFont.Dashboard),
                        new Sample("Notifications", typeof(Notify),SampleData.DashboardImagesList[3], GrialShapesFont.Notifications),
                        new Sample("Buy & Sell", typeof(Views.BuySell.DashBoard), SampleData.DashboardImagesList[3], GrialShapesFont.Group),

                    }
                }
            );

            categories.Add(
                "My Grid Central",
                new SampleCategory
                {
                    Name = "My Grid Central",
                    BackgroundColor = Color.FromHex(_categoriesColors[2]),
                    BackgroundImage = null,
                    Badge = 5,
                    Icon = GrialShapesFont.Dashboard,
                    SamplesList = new List<Sample> {

                        new Sample("Profile", typeof(MyProfile), SampleData.DashboardImagesList[3], GrialShapesFont.AccountCircle),
                        new Sample("Orders", typeof(OrderList),SampleData.DashboardImagesList[3], GrialShapesFont.List),
                        new Sample("Selling", typeof(MyItems),SampleData.DashboardImagesList[3], GrialShapesFont.Label),

                    }
                }
            );

            categories.Add(
                "Others",
                new SampleCategory
                {
                    Name = "Others",
                    BackgroundColor = Color.FromHex(_categoriesColors[2]),
                    BackgroundImage = null,
                    Badge = 5,
                    Icon = GrialShapesFont.Dashboard,
                    SamplesList = new List<Sample> {

                        new Sample("Shop by Category", typeof(CategoryList), SampleData.DashboardImagesList[3], GrialShapesFont.Module),
                        new Sample("Deals", typeof(Deals), SampleData.DashboardImagesList[3], GrialShapesFont.Whatshot),

                    }
                }
            );

            categories.Add(
                "Settings",
                new SampleCategory
                {
                    Name = "Settings",
                    BackgroundColor = Color.FromHex(_categoriesColors[2]),
                    BackgroundImage = null,
                    Badge = 5,
                    Icon = GrialShapesFont.Dashboard,
                    SamplesList = new List<Sample> {
                        new Sample("Settings", typeof(Main_List),SampleData.DashboardImagesList[3], GrialShapesFont.SettingsApplications),
                        new Sample("Help & Contact", typeof(ContactUs),SampleData.DashboardImagesList[3], GrialShapesFont.Help),

                    }
                }
            );

            return categories;
        }



            internal static void InitializeSamples()
        {
            _samplesCategories = CreateSamples();

            _samplesCategoryList = new List<SampleCategory>();

            foreach (var sample in _samplesCategories.Values)
            {
                _samplesCategoryList.Add(sample);
            }

            _allSamples = new List<Sample>();

            _samplesGroupedByCategory = new List<SampleGroup>();

            foreach (var sampleCategory in SamplesCategories.Values)
            {

                var sampleItem = new SampleGroup(sampleCategory.Name.ToUpper());

                foreach (var sample in sampleCategory.SamplesList)
                {
                    _allSamples.Add(sample);
                    sampleItem.Add(sample);
                }

                _samplesGroupedByCategory.Add(sampleItem);
            }
        }

        private static void RootPageCustomNavigation(INavigation navigation)
        {
            SampleCoordinator.RaisePresentMainMenuOnAppearance();
            navigation.PopToRootAsync();
        }
    }

    public class SampleGroup : List<Sample>
    {
        private readonly string _name;

        public SampleGroup(string name)
        {
            _name = name;
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }
    }
}
