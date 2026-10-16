using Microsoft.Playwright;
using Reqnroll;

namespace UIAutomationTests.Hooks
{
    [Binding]
    public sealed class JourneyPlannerHooks
    {
        public static IPlaywright? Playwright { get; private set; }
        public static IBrowser? Browser { get; private set; }
        public static IBrowserContext? Context { get; private set; }
        public static IPage? Page { get; private set; }

        [BeforeScenario]
        public async Task BeforeScenario()
        {
            Playwright = await Microsoft.Playwright.Playwright.CreateAsync();

            Browser = await Playwright.Chromium.LaunchAsync(
                new BrowserTypeLaunchOptions
                {
                    Headless = false

                }
                );

            Context = await Browser.NewContextAsync();

            Page = await Context.NewPageAsync();
        }

        [AfterScenario]
        public async Task AfterScenario()
        {
            await Browser!.CloseAsync();
           
        }

    }
}
