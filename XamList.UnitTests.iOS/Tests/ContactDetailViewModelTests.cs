using System;
using System.Linq;
using System.Threading.Tasks;

using NUnit.Framework;

using XamList.Shared;

namespace XamList.UnitTests.iOS
{
    [TestFixture]
    public class ContactDetailViewModelTests : BaseUnitTest
    {
        [Test]
        public void ContactTest()
        {
            //Arrange
            var contactDetailViewModel = new ContactDetailViewModel();

            //Act
            contactDetailViewModel.Contact = new ContactModel
            {
                FirstName = TestConstants.TestFirstName,
                LastName = TestConstants.TestLastName,
                PhoneNumber = TestConstants.TestPhoneNumber
            };

            //Assert
            Assert.AreEqual(TestConstants.TestFirstName, contactDetailViewModel.FirstNameText);
            Assert.AreEqual(TestConstants.TestLastName, contactDetailViewModel.LastNameText);
            Assert.AreEqual(TestConstants.TestPhoneNumber, contactDetailViewModel.PhoneNumberText);
        }

        [Test]
        public async Task CreateNewContactTest()
        {
            //Arrange
            var contactDetailViewModel = new ContactDetailViewModel();

            contactDetailViewModel.Contact = new ContactModel
            {
                FirstName = TestConstants.TestFirstName,
                LastName = TestConstants.TestLastName,
                PhoneNumber = TestConstants.TestPhoneNumber
            };

            //Act 
            contactDetailViewModel.SaveContactCompleted += HandleSaveContactCompleted;
            contactDetailViewModel.SaveButtonTappedCommand?.Execute(true);

            //Assert
            bool isHandleSaveContactCompletedFinished = false;
            await Task.Delay(TimeSpan.FromMinutes(1));
            Assert.IsTrue(isHandleSaveContactCompletedFinished);

            //Assert
            async void HandleSaveContactCompleted(object sender, EventArgs e)
            {
                contactDetailViewModel.SaveContactCompleted -= HandleSaveContactCompleted;

                var allContactsInDatabase = await ContactDatabase.GetAllContacts().ConfigureAwait(false);
                var testContact = GetTestContacts(allContactsInDatabase)?.FirstOrDefault();

                Assert.IsNotNull(testContact);

                var allContactsInBackend = await APIService.GetContactModel(testContact.Id).ConfigureAwait(false);
                Assert.IsNotNull(allContactsInBackend);

                isHandleSaveContactCompletedFinished = true;
            }
        }
    }
}
