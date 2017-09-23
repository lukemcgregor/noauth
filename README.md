# noauth

<img src="https://ci.appveyor.com/api/projects/status/u0nqgu4hdurn5ae8/branch/master?svg=true"/>

[Docs](http://noauth.azurewebsites.net/)

## What the security?

NoAuth makes no claims at security, in fact a user can make ANY claim via NoAuth with no checks at all. For this reason it's probably not OK to include NoAuth in your production applications.

## Whats the point?

I have written a number of OAuth based applications and am a big believer in UITesting. NoAuth makes it easy to test applications that require OAuth and look at how your application will respond to changing claims.

## Whats do I need to do?

### .NET

Its really easy to integrate NoAuth into your .NET application.

<div class="nuget-badge">
	<p>
		<code>
			Install-Package Owin.Security.Providers.NoAuth
		</code>
	</p>
</div>

In your Startup.Auth.cs file enable NoAuth with the fluent syntax, its also a good idea to put this behind a configuration toggle (like appsettings) so that you can turn NoAuth right off in production.

<pre class="prettyprint lang-csharp">
if (ConfigurationManager.AppSettings["auth.noauth"] == "enabled")
{
	app.UseNoAuthAuthentication();
}</pre>

Then add a new login button in your site (with a toggle to turn it off as well) which calls /account/external-login/NoAuth (or whatever your site configures as the external login target.

That’s it you’re done, you should now be able to sign in with NoAuth.

## What about UI Tests?

NoAuth is all about testing so I have also provided Selenium helpers to make using NoAuth in UI tests easier.

<div class="nuget-badge">
	<p>
		<code>
			Install-Package NoAuth.Selenium
		</code>
	</p>
</div>
	
After you click the NoAuth button in your site, you can use the NoAuth driver extensions (in using NoAuth.Selenium;) to do one of the following:
	
<pre class="prettyprint lang-csharp">
//Sign in with a brand new completely random user.
driver.SignInWithNoAuth();

//Sign in with a consistent user so you can sign in and out with the same account. 
//The deets for this account will be random on the first access but stay the same over multiple requests.
driver.SignInWithNoAuth(claimedIdentifier);

//Sign in with a user with an explicit set of claims, No data will be randomly generated in this case.
driver.SignInWithNoAuth(claimedIdentifier, new Claim[]{ });</pre>
