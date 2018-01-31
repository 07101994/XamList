using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using NUnit.Framework;

using Xamarin.Forms.Mocks;

using XamList.Shared;
using XamList.Tests.Shared;

namespace XamList.UnitTests.iOS
{
    public abstract class BaseUnitTest : BaseTest
    {
        #region Properties
        protected DependencyServiceStub DependencyService { get; private set; }
        #endregion

        #region Methods
        [SetUp]
        protected virtual void BeforeEachTest() => MockForms.Init();

        [TearDown]
        protected virtual void AfterEachTest() => RemoveTestContactFromDatabases().GetAwaiter().GetResult();

        Task RemoveTestContactFromDatabases()
        {
            var removeTestContactsFromRemoteDatabaseTask = RemoveTestContactsFromRemoteDatabase();
            var removeTestContactsFromLocalDatabaseTask = RemoveTestContactsFromLocalDatabase();

            return Task.WhenAll(removeTestContactsFromLocalDatabaseTask, removeTestContactsFromRemoteDatabaseTask);
        }

        async Task RemoveTestContactsFromLocalDatabase()
        {
            var allContactsInLocalDatabase = await ContactDatabase.GetAllContacts().ConfigureAwait(false);

            var allTestContactsInLocalDatabase = allContactsInLocalDatabase?.Where(x => x.FirstName.Equals(TestConstants.TestFirstName) &&
                                                                                        x.LastName.Equals(TestConstants.TestLastName) &&
                                                                                        x.PhoneNumber.Equals(TestConstants.TestPhoneNumber));

            var removeTestContactsInLocalDatabaseTaskList = new List<Task>();
            foreach (var testContact in allTestContactsInLocalDatabase)
                removeTestContactsInLocalDatabaseTaskList.Add(ContactDatabase.RemoveContact(testContact));

            await Task.WhenAll(removeTestContactsInLocalDatabaseTaskList);
        }
        #endregion
    }
}
