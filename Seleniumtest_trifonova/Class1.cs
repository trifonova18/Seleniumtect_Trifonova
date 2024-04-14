using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Seleniumtest_trifonova;

public class Seleniumtest
{
    [Test]
    public void Authorization()
    {
        var options = new ChromeOptions();
        options.AddArgument("--start-maximized");

        var driver = new ChromeDriver(options);
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru");
        Thread.Sleep(5000);

        var login = driver.FindElement(By.XPath("//*[@id=\"Username\"]"));
        login.SendKeys("natalia.o.g@mail.ru");
        
        var password = driver.FindElement(By.XPath("//*[@id=\"Password\"]"));
        password.SendKeys("Rbgfhbc1987!!!@%");

        var enter = driver.FindElement(By.XPath("//*[@id=\"login_form\"]/div[5]/button"));
        enter.Click();

        Thread.Sleep(3000);

        var currentUrl = driver.Url;
        Assert.That(currentUrl == "https://staff-testing.testkontur.ru/news");   
        
        driver.Quit();
    }
}