using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GridCentral.Models
{
    public static class SampleData
    {
        private static string[] _names;
        private static List<string> _socialImageGalleryItems;
        private static List<string> _articlesImagesList;
        private static List<string> _usersImagesList;
        private static List<string> _dashboardImagesList;
        private static List<string> _productsImagesList;
        private static List<Product> _products;


        private static List<Notification> _notifications;

        public static List<Notification> Notifications
        {
            get
            {
                if (_notifications == null)
                {
                    _notifications = InitNotifications();
                }

                return _notifications;
            }
        }



        public static string[] Names
        {
            get
            {
                if (_names == null)
                {
                    _names = InitNames();
                }

                return _names;
            }
        }



        public static List<string> DashboardImagesList
        {
            get
            {
                if (_dashboardImagesList == null)
                {
                    _dashboardImagesList = InitDashboardImagesList();
                }

                return _dashboardImagesList;
            }
        }

        public static List<string> ProductsImagesList
        {
            get
            {
                if (_productsImagesList == null)
                {
                    _productsImagesList = Test();//InitProductsImagesList();
                }

                return _productsImagesList;
            }
        }





        public static List<Product> Products
        {
            get
            {
                if (_products == null)
                {
                    _products = InitProducts();
                }

                return _products;
            }
        }



        private static List<Notification> InitNotifications()
        {
            return new List<Notification> {

                new Notification {
                    Title = "Confirmation",
                    Type = NotificationType.Confirmation,
                    Description = "Please confirm your email address"
                },

                new Notification {
                    Title = "Error",
                    Type = NotificationType.Error,
                    Description = "Please confirm your email address"
                },

                new Notification {
                    Title = "Warning",
                    Type = NotificationType.Warning,
                    Description = "Can't reach your current location"
                },

                new Notification {
                    Title = "Warning",
                    Type = NotificationType.Warning,
                    Description = "Please contact support center"
                },

                new Notification {
                    Title = "Notification",
                    Type = NotificationType.Notification,
                    Description = "You have new message"
                },

                new Notification {
                    Title = "Success",
                    Type = NotificationType.Success,
                    Description = "You have a new follower"
                },

            };

        }


        private static string[] InitNames()
        {
            return new[]{
                "Pat Davies",
                "Janis Spector",
                "Regina Joplin",
                "Jaco Morrison",
                "Margaret Whites",
                "Skyler Harrisson",
                "Al Pastorius",
            };
        }

        private static List<string> InitSocialImageGalleryItems()
        {
            return new List<string>() {
                "social_album_1.jpg",
                "social_album_2.jpg",
                "social_album_3.jpg",
                "social_album_4.jpg",
                "social_album_5.jpg",
                "social_album_6.jpg",
                "social_album_7.jpg",
                "social_album_8.jpg",
                "social_album_9.jpg",
            };
        }

        private static List<string> InitArticlesImagesList()
        {
            return new List<string>() {
                "article_image_0.jpg",
                "article_image_1.jpg",
                "article_image_2.jpg",
                "article_image_3.jpg",
                "article_image_4.jpg",
                "article_image_5.jpg"
            };
        }
        private static List<string> InitUsersImagesList()
        {
            return new List<string>() {
                "friend_thumbnail_27.jpg",
                "friend_thumbnail_31.jpg",
                "friend_thumbnail_34.jpg",
                "friend_thumbnail_55.jpg",
                "friend_thumbnail_71.jpg",
                "friend_thumbnail_75.jpg",
                "friend_thumbnail_93.jpg",
            };
        }

        private static List<string> InitDashboardImagesList()
        {
            return new List<string>() {
                "dashboard_thumbnail_0.jpg",
                "dashboard_thumbnail_1.jpg",
                "dashboard_thumbnail_2.jpg",
                "dashboard_thumbnail_3.jpg",
                "dashboard_thumbnail_4.jpg",
                "dashboard_thumbnail_5.jpg",
                "dashboard_thumbnail_6.jpg",
                "dashboard_thumbnail_7.jpg",
                "dashboard_thumbnail_8.jpg",
            };
        }
        private static List<string> InitProductsImagesList()
        {
            return new List<string>() {
                "product_item_0.jpg",
                "product_item_1.jpg",
                "product_item_2.jpg",
                "product_item_3.jpg",
                "product_item_4.jpg",
                "product_item_5.jpg",
                "product_item_6.jpg",
                "product_item_7.jpg",
            };
        }

        private static List<string> Test()
        {
            return new List<string>() {
                "user_profile_0.jpg",
                "user_profile_0.jpg",
                "user_profile_0.jpg",
                "user_profile_0.jpg",
                "user_profile_0.jpg",
                "user_profile_0.jpg",
                "user_profile_0.jpg",
                "user_profile_0.jpg",
                "user_profile_0.jpg",
            };
        }




        private static List<Product> InitProducts()
        {
            return new List<Product> {
                new Product {
                    Name            = "Flannel Shirt",
                    Description     = "Classic 90's Skateboarding style shirt. Feel like Pat Duffy or even flow like Edie from Pearl Jam. With that casual grunge style this is the shirt you need.",
                    Thumbnail           = SampleData.ProductsImagesList[0],
                    Price           = "$39.90",
                    ThumbnailHeight = "100",
                    RatingMax       = 5,
                    RatingValue     = 4.5,
                    Manufacturer    = "Johvahn"
                },

                new Product {
                    Name            = "Bomber Jacket",
                    Description     = "Top gun in every gentelman closet. This leather jacket will make you feel like Tom Cruise without that crazy look. Be a good boy make mom proud.",
                    Thumbnail           = SampleData.ProductsImagesList[1],
                    Price           = "$89.90",
                    ThumbnailHeight = "100",
                    RatingMax       = 5,
                    RatingValue     = 4.5,
                    Manufacturer    = "Johvahn"
                },

                new Product {
                    Name            = "Classic Black",
                    Description     = "Get that instant normal look that everybody wants. Blend with the humans, it will make you feel less strange. You know you are not normal",
                    Thumbnail          = SampleData.ProductsImagesList[2],
                    Price           = "$49.90",
                    ThumbnailHeight = "100",
                    RatingMax       = 5,
                    RatingValue     = 4.5,
                    Manufacturer    = "Johvahn"
                },

                new Product {
                    Name            = "Flowers Shirt",
                    Description     = "Our newest swim tees with a much looser fit than traditional rash guard for yet more comfort and versatility, is well known for great fit, function and colors.",
                    Thumbnail           = SampleData.ProductsImagesList[3],
                    Price           = "$29.90",
                    ThumbnailHeight = "100",
                    RatingMax       = 5,
                    RatingValue     = 4.5,
                    Manufacturer    = "Johvahn"
                },

                new Product {
                    Name            = "Sccotish Shirt",
                    Description     = "Not just another common shirt. Upgrade your sexappeal looking good. Rock and roll never gets old. Eric Burdon wears a shirt like this one when he wants to lood good. ",
                    Thumbnail           = SampleData.ProductsImagesList[4],
                    Price           = "$34.90",
                    ThumbnailHeight = "100",
                    RatingMax       = 5,
                    RatingValue     = 4.5,
                },

                new Product {
                    Name            = "Silk Shirt",
                    Description     = "Let's face it, this shirt does not look good on anybody. But how many times do you buy something that you don't need? Buy this one feel happy for a minute then dismiss it. ",
                    Thumbnail           = SampleData.ProductsImagesList[5],
                    Price           = "$39.90",
                    ThumbnailHeight = "100",
                    RatingMax       = 5,
                    RatingValue     = 4.5,
                    Manufacturer    = "Johvahn"
                },

                new Product {
                    Name            = "Entrepreneur Shirt",
                    Description     = "Do you have a meeting? Do you want to look good reliable and confident? This is the shirt that you need for those horrible meetings trying to find someone that lend you some money.",
                    Thumbnail           = SampleData.ProductsImagesList[6],
                    Price           = "$65.90",
                    ThumbnailHeight = "100",
                    RatingMax       = 5,
                    RatingValue     = 4.5,
                    Manufacturer    = "Johvahn"
                },

                new Product {
                    Name            = "Soldier Shirt",
                    Description     = "Now is your time. Wanna be the alpha male of your local bar? Common! Get this shirt now and feel like a sexy Rambo on your next date. Remember that girls loves peacefull soliders.",
                    Thumbnail           = SampleData.ProductsImagesList[7],
                    Price           = "$46.90",
                    ThumbnailHeight = "1000",
                    RatingMax       = 5,
                    RatingValue     = 4.5,
                    Manufacturer    = "Johvahn"
                }
            };
        }

    }


}
