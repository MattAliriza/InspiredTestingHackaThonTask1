
using System.Collections;
using AventStack.ExtentReports;
using HackaThon.Models;
using HackaThon.Utilities;
using OpenQA.Selenium;

namespace HackaThon.PageObjects
{
    public class WebTablePage
    {
        private WebUtils _seleniumInstance;
        private ExtentTest _currentTest;

        public WebTablePage(WebUtils webUtil, ExtentTest _CurrentTest)
        {
            _seleniumInstance = webUtil;
            _currentTest = _CurrentTest;
        }

        private static string table = "//table[@class='smart-table table table-striped']";
        public static string addUserButton = "//button[@class='btn btn-link pull-right']";
        public static string addUserForm = "//div[@class='modal ng-scope']";
        public static string tableUsernameValues = "//tr[@ng-repeat='dataRow in displayedCollection']/td[3]";
        public static string tableFirstLineValues = "//tr[@ng-repeat='dataRow in displayedCollection'][1]/td[not(@class='smart-table-data-cell ng-hide')]";
        private string _firstNameInputField = "//td[text()='First Name']/..//input";
        private string _lastNameInputField = "//td[text()='Last Name']/..//input";
        private string _userNameInputField = "//td[text()='User Name']/..//input";
        private string _passwordInputField = "//td[text()='Password']/..//input";
        private string _customerRadioButton(string customerValue) => "//label[text()='" + customerValue + "']/input";
        private string _roleComboBox = "//select[@name='RoleId']";
        private string _emailInputField = "//td[text()='E-mail']/..//input";
        private string _cellPhoneInputField = "//td[text()='Cell Phone']/..//input";
        private string _saveButton = "//button[text()='Save']";

        public void NavigateToWebPage(string websiteUrl)
        {
            //Navigates to the URl and maximises the browser
            _seleniumInstance.GetDriver.Navigate().GoToUrl(websiteUrl);
            _seleniumInstance.GetDriver.Manage().Window.Maximize();

            //validates that the correct page is displayed
            _seleniumInstance.CheckElementIsPresent(WebTablePage.table, _currentTest, _seleniumInstance.GetDriver);
            Core.ExtentReport.StepPassedWithScreenShot(_currentTest, _seleniumInstance.GetDriver, "Successfully navigated to '" + websiteUrl + "'.");
        }

        public void AddUserToTable(User testData, bool softAssert = false)
        {
            //Stores usernames for later validation
            ArrayList currentlyStoredUserNames = new ArrayList(_seleniumInstance.GetAllValuesFrom(WebTablePage.tableUsernameValues));

            //Clicks the add user button
            _seleniumInstance.clickElement(WebTablePage.addUserButton, _currentTest, _seleniumInstance.GetDriver, softAssert);

            //Validates that the add user pop up appears
            _seleniumInstance.CheckElementIsPresent(WebTablePage.addUserForm, _currentTest, _seleniumInstance.GetDriver, softAssert);

            //logs a step passed
            Core.ExtentReport.StepPassedWithScreenShot(_currentTest, _seleniumInstance.GetDriver, "Successfully navigated to the 'add user' form.");

            //Inserts the firstname
            _seleniumInstance.SendKeysTo(_firstNameInputField, testData.fname, _currentTest, _seleniumInstance.GetDriver, softAssert);

            //Inserts the lastname
            _seleniumInstance.SendKeysTo(_lastNameInputField, testData.lname, _currentTest, _seleniumInstance.GetDriver, softAssert);

            //Inserts the username
            _seleniumInstance.SendKeysTo(_userNameInputField, testData.username, _currentTest, _seleniumInstance.GetDriver, softAssert);

            //Inserts the password
            _seleniumInstance.SendKeysTo(_passwordInputField, testData.password, _currentTest, _seleniumInstance.GetDriver, softAssert);

            //Selects the customer
            _seleniumInstance.clickElement(_customerRadioButton(testData.customer), _currentTest, _seleniumInstance.GetDriver, softAssert);

            //Selects the role
            _seleniumInstance.SelectValueFromComboBox(_roleComboBox, testData.role, _currentTest, _seleniumInstance.GetDriver, softAssert);

            //Inserts the email
            _seleniumInstance.SendKeysTo(_emailInputField, testData.email, _currentTest, _seleniumInstance.GetDriver, softAssert);

            //Inserts the cellphone
            _seleniumInstance.SendKeysTo(_cellPhoneInputField, testData.cellnumber, _currentTest, _seleniumInstance.GetDriver, softAssert);

            //logs a step passed
            Core.ExtentReport.StepPassedWithScreenShot(_currentTest, _seleniumInstance.GetDriver, "Successfully filled out the 'add user' form.");

            //Checks that the username is unique
            foreach (string existingUsernames in currentlyStoredUserNames)
            {
                if (existingUsernames.Equals(testData.username))
                    Core.ExtentReport.TestFailed(_currentTest, _seleniumInstance.GetDriver, "Failed due to the user name '" + testData.username + "' already existing in the application.", softAssert);
            }

            Core.ExtentReport.StepPassedWithScreenShot(_currentTest, _seleniumInstance.GetDriver, "Successfully validated that the username '" + testData.username + "' is unique.");

            //Submits the form
            _seleniumInstance.clickElement(_saveButton, _currentTest, _seleniumInstance.GetDriver, softAssert);
        }

        public void ValidateUserWasAdded(User testData, bool softAssert = false)
        {
            //Ensures that the user has been added to the top of the list with the exact details
            //Stores usernames for later validation
            ArrayList newestAdditionToTheTable = new ArrayList(_seleniumInstance.GetAllValuesFrom(WebTablePage.tableFirstLineValues));

            //Validates firstname
            if (!newestAdditionToTheTable[0].Equals(testData.fname))
                Core.ExtentReport.TestFailedWithScreenShot(_currentTest, _seleniumInstance.GetDriver, "Failed due to the table column 'First Name' not matching '" + testData.fname + "'.", softAssert);

            //Validates lastname
            if (!newestAdditionToTheTable[1].Equals(testData.lname))
                Core.ExtentReport.TestFailedWithScreenShot(_currentTest, _seleniumInstance.GetDriver, "Failed due to the table column 'Last Name' not matching '" + testData.lname + "'.", softAssert);

            //Validates username
            if (!newestAdditionToTheTable[2].Equals(testData.username))
                Core.ExtentReport.TestFailedWithScreenShot(_currentTest, _seleniumInstance.GetDriver, "Failed due to the table column 'User Name' not matching '" + testData.username + "'.", softAssert);

            //Validates customer
            if (!newestAdditionToTheTable[3].Equals(testData.customer))
                Core.ExtentReport.TestFailedWithScreenShot(_currentTest, _seleniumInstance.GetDriver, "Failed due to the table column 'Customer' not matching '" + testData.customer + "'.", softAssert);

            //Validates role
            if (!newestAdditionToTheTable[4].Equals(testData.role))
                Core.ExtentReport.TestFailedWithScreenShot(_currentTest, _seleniumInstance.GetDriver, "Failed due to the table column 'Role' not matching '" + testData.role + "'.", softAssert);

            //Validates email
            if (!newestAdditionToTheTable[5].Equals(testData.email))
                Core.ExtentReport.TestFailedWithScreenShot(_currentTest, _seleniumInstance.GetDriver, "Failed due to the table column 'E-mail' not matching '" + testData.email + "'.", softAssert);

            //Validates cellnumber
            if (!newestAdditionToTheTable[6].Equals(testData.cellnumber))
                Core.ExtentReport.TestFailedWithScreenShot(_currentTest, _seleniumInstance.GetDriver, "Failed due to the table column 'Cell Number' not matching '" + testData.cellnumber + "'.", softAssert);

            //Logs a passing step to the report
            Core.ExtentReport.StepPassedWithScreenShot(_currentTest, _seleniumInstance.GetDriver, "Successfully validated that the user '" + testData.fname + "' was present");
        }
    }
}