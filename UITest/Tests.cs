using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace UITest
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class Tests
    {
        IApp app;
        Platform platform;

        public Tests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
        }

        [Test]
        public void AppLaunches()
        {
            //app.Repl();
            app.Screenshot("First screen.");
        }

        [Test]
        public void Login_Go_Dashboard()
        {
            //Arrange
            app.Screenshot("App Launched");

            app.WaitForElement(c => c.Marked("Sign In"), "No Sign In Prompt Showed");

            app.Tap(c => c.Marked("Not Now"));

            app.Tap("Cart_Btn");

            app.WaitForElement(c => c.Marked("Login"), "No Login Prompt Showed");

            app.Tap(c => c.Marked("Login"));

            app.WaitForElement(c => c.Marked("Welcome to Grid Central"), "Not at login");

            app.Screenshot("Login Page");

            app.EnterText("Email_Entry", "test@grid.com");
            app.EnterText("Password_Entry", "fold!test1243");

            //Act
            app.Tap("Login_Btn");

            //Assert

            var result = app.WaitForElement("Your Interests");

            app.Screenshot("Logged In");


        }

        [Test]
        public void Add_Item_To_Cart()
        {

            Login_Go_Dashboard();

            app.Tap("Interest_Product");

            app.Screenshot("Product View");

            app.ScrollDown();

            app.Tap("Add_To_Cart_Btn");

            app.WaitForElement(c => c.Marked("Item Added To Cart"), "Item did not add");

            app.Tap(c => c.Id("NoResourceEntry-0")); // ToolBar Cart Button


            app.Screenshot("Cart View");

        }
    }
}

