﻿using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace Seleniun_trifonova;

public class Seleniumtest
{
    public ChromeDriver driver;

    // Убираем домен в переменную
    public string StaffTestingCity = "https://staff-testing.testkontur.ru";

    [SetUp]
    public void SetUp()

    {
        var options = new ChromeOptions();
        options.AddArguments("--no-sandbox", "--start-maximized", "--disable-extensions");
        driver = new ChromeDriver(options);
        //Добавляем неявное ожидание
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
    }

    [TearDown]
    public void TearDown()
    {
        // Закрывае браузер и все процессы
        driver.Close();
        driver.Quit();
    }

    public void Auth(string user, string password)
    {
        driver.Navigate().GoToUrl(StaffTestingCity);
        //Добавляем неявное ожидание
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        driver.FindElement(By.Id("Username")).SendKeys(user);
        driver.FindElement(By.Name("Password")).SendKeys(password);
        driver.FindElement(By.Name("button")).Click();
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
    }

    [Test]
    public void Authorization()
    {
        Auth("natalia.o.g@mail.ru", "Rbgfhbc1987!!!@%");
        
        // Проверяем, что удалось авторизоваться
        Assert.That(driver.FindElement(By.CssSelector("h1[data-tid='Title']")).Displayed,
            "Авторизироваться не удалось");
    }

    [Test]
    public void TestSearch()
    {
        Auth("natalia.o.g@mail.ru", "Rbgfhbc1987!!!@%");
        
        //Находим элемент поиска
        var queryInput = driver.FindElement(By.CssSelector("span[data-tid='SearchBar']"));
        queryInput.Click();
        
        //Вводим имя и фамилию пользователя, которого хотим найти
        var searchField = driver.FindElement(By.CssSelector("span[data-tid='SearchBar'] input"));
        searchField.SendKeys("Наталья Трифонова");
        
        var profileLink = driver.FindElement(By.CssSelector("div[title='Наталья Трифонова']"));
        profileLink.Click();
        
        var employeeName = driver.FindElement(By.CssSelector("div[data-tid='EmployeeName']")).Text;
        var expectedEmployeeName = "Наталья Трифонова";
        Assert.That(employeeName == expectedEmployeeName,
            $"Фактическое имя {employeeName} отличается от ожидаемого {expectedEmployeeName}");

    }

    [Test]
    public void Logout()
    {
        Auth("natalia.o.g@mail.ru", "Rbgfhbc1987!!!@%");
        
        //Кликааем на профиль пользователя
        var avatarElement = driver.FindElement(By.CssSelector("[data-tid='Avatar']"));
        avatarElement.Click();
        
        //Нажимаем кнопку выйти
        var logoutButton = driver.FindElement(By.CssSelector("[data-tid='Logout']"));
        logoutButton.Click();
        
        var logoutText = driver.FindElement(By.CssSelector("h3"));
        var expectedLogoutText = "Вы вышли из учетной записи";
        // Проверяем, что вышли из учетной записи
        Assert.That(logoutText.Text, Is.EqualTo(expectedLogoutText), $"Фактический текст {logoutText} отличается от ожидаемого {expectedLogoutText}");
    }

    [Test]
    public void newCommunity()
    {
        Auth("natalia.o.g@mail.ru", "Rbgfhbc1987!!!@%");
        driver.Navigate().GoToUrl(StaffTestingCity + "/communities");
        
        //Нажамаем на кнопку "Создать" 
        var createButton = driver.FindElement(By.XPath("//button[contains(text(), 'СОЗДАТЬ')]"));
        createButton.Click();
        
        //Заполняем название сообщеста
        var communityNameField = driver.FindElement(By.CssSelector("textarea[placeholder='Название сообщества']"));
        communityNameField.SendKeys("Cats");
        
        //Заполняем описание сообщества
        var communityDescriptionField = driver.FindElement(By.CssSelector("textarea[placeholder='Описание сообщества']"));
        communityDescriptionField.SendKeys("Cute Cats");
        
        //Нажимаем на кнопку "Создать"
        var createCommunityButton = driver.FindElement(By.XPath("//span[contains(text(), 'Создать')]"));
        createCommunityButton.Click();

        var communityNameElement = driver.FindElement(By.CssSelector("[data-tid=\"Name\"]"));
        string communityName = communityNameElement.Text;
        string expectedCommunityName = "Cats";

        //Проверяем, что сообщество создалось
        Assert.That(communityName, Is.EqualTo(expectedCommunityName), $"Фактический текст {communityName} отличается от ожидаемого {expectedCommunityName}");

        //Удаляем сообщество
        var deleteButton = driver.FindElement(By.CssSelector("button[data-tid='DeleteButton']"));
        deleteButton.Click();
        var deleteConfirmButton = driver.FindElement(By.XPath("//span[contains(text(), 'Удалить')]"));
        deleteConfirmButton.Click();
    }

    [Test]
    public void addAddress()
    {
        Auth("natalia.o.g@mail.ru", "Rbgfhbc1987!!!@%");
        
        //Кликаем на профиль пользователя
        var avatarElement = driver.FindElement(By.CssSelector("[data-tid='Avatar']"));
        avatarElement.Click();
        
        //Нажимаем на кнопку "Редактировать"
        var editProfileButton = driver.FindElement(By.CssSelector("[data-tid='ProfileEdit']"));
        editProfileButton.Click();

        //Заполняем поле "Адрес рабочего места"
        var addressField = driver.FindElement(By.XPath("// * [contains(@class, 'react-ui-r3t2bi')]\n"));
        addressField.SendKeys("Санкт-Петербург");
        
        //Нажимаем на кнопку "Сохранить"
        var saveButton = driver.FindElement(By.XPath("// * [contains(@class, 'sc-juXuNZ kVHSha')]\n"));
        saveButton.Click();

        var addressTextElement = driver.FindElement(By.XPath("//div[@class=\"sc-bdnxRM sc-jcwpoC sc-iTVJFM kSMcXF iUFNtI\"]"));
        string addressText = addressTextElement.Text;
        string expectedAddressText = "Санкт-Петербург";

        //Проверяем, что адрес добавлем
        Assert.That(addressText, Is.EqualTo(expectedAddressText), $"Фактический адрес {addressText} отличается от ожидаемого {expectedAddressText}");
    }
    
}