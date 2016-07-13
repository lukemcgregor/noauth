using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using By = OpenQA.Selenium.Extensions.By;

namespace NoAuth.Selenium
{
	public static class NoAuthHelpers
	{
		private static string NoAuthBaseUrl = ConfigurationManager.AppSettings["noauth.baseurl"] ?? "noauth.azurewebsites.net";
		/// <summary>
		/// Signs in with noauth populating some default claims information for a random user.
		/// </summary>
		/// <param name="driver">the driver which has the page.</param>
		public static void SignInWithNoAuth(this IWebDriver driver)
		{
			driver.SignInWithNoAuth((d) => {
				d.FindElement(By.JQuerySelector("#populate-random-user")).Click();
			});
		}

		/// <summary>
		/// Signs in with noauth populating some default claims information for a random user, but using a set claimed identifier (so you can ensure its the same user in your application)
		/// </summary>
		/// <param name="driver">the driver which has the page.</param>
		/// <param name="claimedIdentifier">The claimed identifer for the user sent to your application.</param>
		public static void SignInWithNoAuth(this IWebDriver driver, string claimedIdentifier)
		{
			driver.SignInWithNoAuth((d) => {
				d.FindElement(By.JQuerySelector("#claimed-identifier")).Clear();
				d.FindElement(By.JQuerySelector("#claimed-identifier")).SendKeys(claimedIdentifier);
				d.FindElement(By.JQuerySelector("#populate-random-user")).Click();
			});
		}

		/// <summary>
		/// Signs in with noauth using a set claimed identifier (so you can ensure its the same user in your application), and a predefined list of claims (which overrides the default data).
		/// </summary>
		/// <param name="driver">the driver which has the page.</param>
		/// <param name="claimedIdentifier">The claimed identifer for the user sent to your application.</param>
		/// <param name="claims">A list of claims to make to your application.</param>
		public static void SignInWithNoAuth(this IWebDriver driver, string claimedIdentifier, List<Claim> claims)
		{
			driver.SignInWithNoAuth((d) => {
				d.FindElement(By.JQuerySelector("#claimed-identifier")).Clear();
				d.FindElement(By.JQuerySelector("#claimed-identifier")).SendKeys(claimedIdentifier);
				for (int i = 0; i < claims.Count(); i++)
				{
					d.FindElement(By.JQuerySelector("#add-claim")).Click();
					d.FindElement(By.JQuerySelector($"[name=\"Claims[{i}].Type\"")).SendKeys(claims[i].Type);
					d.FindElement(By.JQuerySelector($"[name=\"Claims[{i}].Value\"")).SendKeys(claims[i].Value);
				}
			});
		}

		private static void SignInWithNoAuth(this IWebDriver driver, Action<IWebDriver> extraStuff)
		{
			if (!driver.Url.Contains(NoAuthBaseUrl))
			{
				throw new Exception($"The current url ({driver.Url}) didn't contain { NoAuthBaseUrl } are you sure you clicked the login button?");
			}
			driver.WaitUntilNotLoading();
			extraStuff(driver);
			driver.WaitUntilNotLoading();
			driver.FindElement(By.JQuerySelector("#login")).Click();
		}

		private static void WaitUntilNotLoading(this IWebDriver driver, int milliseconds = 20000)
		{
			driver.WaitUntil(() =>
			{
				return !driver.IsLoading();
			}, milliseconds);
		}
		private static void WaitUntil(this IWebDriver driver, Func<bool> x, int milliseconds)
		{
			DateTime finishAt = DateTime.Now.AddMilliseconds(milliseconds);
			while (!x.Invoke())
			{
				if (DateTime.Now > finishAt)
				{
					throw new ArgumentException(String.Format("The condition did not become true in the timeframe {0}ms", milliseconds));
				}
				Thread.Sleep(Math.Max(10, milliseconds / 10));
			}
		}
		private static bool IsLoading(this IWebDriver driver)
		{
			return driver.FindElements(By.JQuerySelector(".loading")).Count > 0;
		}
	}
}
