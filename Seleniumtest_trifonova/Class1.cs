using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace Seleniun_trifonova;

public class Seleniumtest
{

public ChromeDriver driver;

public string staff = "https://staff-testing.testkontur.ru";

[SetUp]
public void SetUp()

{ var options = new ChromeOptions();
    options.AddArguments("--no-sandbox", "--start-maximized", "--disable-extensions");
    driver = new ChromeDriver(options);
    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
}
[TearDown]
public void TearDown()
{
    driver.Close();
    driver.Quit();
}

public void Auth(string user, string password)
{
    driver.Navigate().GoToUrl(staff);
    driver.FindElement(By.Id("Username")).SendKeys(user);
    driver.FindElement(By.Name("Password")).SendKeys(password);
    driver.FindElement(By.Name("button")).Click();
    Assert.That(driver.FindElement(By.CssSelector("h1[data-tid='Title']")).Displayed, "Не удалось авторизоваться");
}

[Test]
public void Authorization()
{
    driver.Navigate().GoToUrl(staff);
    
    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5)); 
    wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("Username")));
    
    var login = driver.FindElement(By.Id("Username"));
    login.SendKeys("natalia.o.g@mail.ru");
    
    var password = driver.FindElement(By.Name("Password"));
    password.SendKeys("Rbgfhbc1987!!!@%");
    
    var enter = driver.FindElement(By.Name("button"));
    enter.Click();
    
    Assert.That(driver.FindElement(By.CssSelector("h1[data-tid='Title']")).Displayed, "Не получилось авторизироваться");

}

[Test]
public void TestSearch()
{
    Auth("natalia.o.g@mail.ru", "Rbgfhbc1987!!!@%");
    driver.Navigate().GoToUrl(staff);
    driver.FindElement(By.CssSelector("span[data-tid='SearchBar']")).Click();
    driver.FindElement(By.CssSelector("span[data-tid='SearchBar'] input")).SendKeys("Трифонова");
    driver.FindElement(By.CssSelector("div[title='Наталья Трифонова']")).Click();
    var employeeName = driver.FindElement(By.CssSelector("div[data-tid='EmployeeName']")).Text;
    var expectedEmployeeName = "Наталья Трифонова";
    Assert.That(employeeName == expectedEmployeeName,"Ожидаемый результат {employeeName} отличается от ожидаемого {expectedEmployeeName}");
}
[Test]
public void Logout()
{
    Auth("natalia.o.g@mail.ru", "Rbgfhbc1987!!!@%");
    driver.Navigate().GoToUrl(staff);
    driver.FindElement(By.CssSelector("[data-tid='Avatar']")).Click();     
    driver.FindElement(By.CssSelector("[data-tid='Logout']")).Click(); 
    var logoutText = driver.FindElement(By.CssSelector("h3")).Text;
    var expectedLogoutText = "Вы вышли из учетной записи";
    Assert.That(logoutText == expectedLogoutText, "Фактический текст {logoutText} отличается от ожидаемого {expectedLogoutText}");
}

[Test]
public void newCommunity()
{
    Auth("natalia.o.g@mail.ru", "Rbgfhbc1987!!!@%");
    driver.Navigate().GoToUrl(staff + "/communities");
    driver.FindElement(By.XPath("//*[@id=\"root\"]/section/section[2]/section/div[2]/span/button")).Click();
    driver.FindElement(By.CssSelector("textarea[placeholder='Название сообщества']")).SendKeys("Cats");
    driver.FindElement(By.CssSelector("textarea[placeholder='Описание сообщества']")).SendKeys("Cute Cats");
    driver.FindElement(By.XPath("//span[contains(text(), 'Создать')]")).Click();
    var CommunityName = driver.FindElement(By.CssSelector("[data-tid=\"Name\"]")).Text;
    var expectedCommunityName = "Cats";
    Assert.That(CommunityName == expectedCommunityName, "Сообщество не добавлено" );
    driver.FindElement(By.CssSelector("button[data-tid='DeleteButton']")).Click();
    driver.FindElement(By.XPath("//span[contains(text(), 'Удалить')]")).Click();
}
[Test]
public void addAddress()
{
    Auth("natalia.o.g@mail.ru", "Rbgfhbc1987!!!@%");
    driver.FindElement(By.CssSelector("[data-tid='Avatar']")).Click();
    driver.FindElement(By.CssSelector("[data-tid='ProfileEdit']")).Click();
    driver.FindElement(By.XPath("//*[@id=\"root\"]/section/section[2]/section[3]/div[1]/div[6]/label[2]/div/div/textarea")).SendKeys("Санкт-Петербург");
    driver.FindElement(By.XPath("//*[@id=\"root\"]/section/section[2]/section[1]/div[2]/button[1]")).Click();
    var AdressText = driver
        .FindElement(By.XPath("//*[@id=\"root\"]/section/section[2]/section[2]/div[2]/div[2]/div/div[2]")).Text;
    var expectedAdressText = "Санкт-Петербург";
    Assert.That(AdressText == expectedAdressText, "Адрес отличается от ожидаемого или не добавлен");
}

}


