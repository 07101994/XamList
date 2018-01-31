using System.Threading.Tasks;

using NUnit.Framework;

using Xamarin.UITest;

using XamList.Tests.Shared;

namespace XamList.UITests
{
    [TestFixture(Platform.iOS)]
    [TestFixture(Platform.Android)]
    public abstract class BaseUITest : BaseTest
    {
        #region Constructors
        protected BaseUITest(Platform platform) => Platform = platform;
        #endregion

        #region Properties
        protected Platform Platform { get; }

        protected ContactsListPage ContactsListPage { get; private set; }
        protected ContactDetailsPage ContactDetailsPage { get; private set; }
        protected IApp App { get; private set; }


        #endregion

        #region Methods
        [SetUp]
        protected virtual void BeforeEachTest()
        {
            App = AppInitializer.StartApp(Platform);
            ContactsListPage = new ContactsListPage(App, Platform);
            ContactDetailsPage = new ContactDetailsPage(App, Platform);

            RevoveTestContactsFromDatabases(App).GetAwaiter().GetResult();

            ContactsListPage.WaitForPageToLoad();
            ContactsListPage.WaitForNoPullToRefreshActivityIndicatorAsync().GetAwaiter().GetResult();
        }

        [TearDown]
        protected virtual void AfterEachTest() => RevoveTestContactsFromDatabases(App).GetAwaiter().GetResult();

        Task RevoveTestContactsFromDatabases(IApp app)
        {
            var removeTestContactsFromRemoteDatabaseTask = RemoveTestContactsFromRemoteDatabase();
            var removeTestContactsFromLocalDatabaseTask = Task.Run(() => BackdoorMethodHelpers.RemoveTestContactsFromLocalDatabase(app));

            return Task.WhenAll(removeTestContactsFromLocalDatabaseTask, removeTestContactsFromRemoteDatabaseTask);
        }
        #endregion
    }
}

