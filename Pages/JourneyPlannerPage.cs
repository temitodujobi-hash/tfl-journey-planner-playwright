using Microsoft.Playwright;

namespace UIAutomationTests.Pages
{
    public class JourneyPlannerPage
    {
        private readonly IPage _page;

        public JourneyPlannerPage(IPage page)
        {
            _page = page;
        }

        private ILocator AcceptCookiesButton =>
            _page.Locator("#CybotCookiebotDialogBodyLevelButtonLevelOptinAllowAll");

        private ILocator StartField =>
            _page.Locator("#InputFrom");

        private ILocator DestinationField =>
            _page.Locator("#InputTo");

        private ILocator PlanJourneyButton =>
            _page.Locator("#plan-journey-button");

        public async Task NavigateToJourneyPlanner()
        {
            var baseUrl = TestContext.Parameters["BaseUrl"];
            await _page.GotoAsync(baseUrl!);
            await AcceptCookies();
        }

        public async Task AcceptCookies()
        {
            try
            {
                await AcceptCookiesButton.WaitForAsync(new()
                {
                    State = WaitForSelectorState.Visible,
                    Timeout = 5000
                });

                await AcceptCookiesButton.ClickAsync();
            }
            catch (TimeoutException)
            {
                // Cookie banner not displayed
            }
        }

        public async Task EnterFromLocation(string from)
        {
            await StartField.FillAsync(from);
        }

        public async Task EnterToLocation(string to)
        {
            await DestinationField.FillAsync(to);
        }

        public async Task ClickPlanJourney()
        {
            await PlanJourneyButton.ClickAsync();
        }

        public async Task PlanJourney(string from, string to)
        {
            // Partial search for start field
            await StartField.FillAsync(from.Substring(0, 6));
            await _page
                .GetByText(from)
                .ClickAsync();

            // Partial search for destination field
            await DestinationField.FillAsync(to.Substring(0, 6));

            await _page
                .GetByText(to)
                .ClickAsync();

            await ClickPlanJourney();
        }

        private ILocator JourneyResultsHeading => _page.Locator("h1");

        public async Task<bool> IsJourneyResultDisplayed()
        {
            return await JourneyResultsHeading.IsVisibleAsync();
        }

        private ILocator CyclingTime =>
            _page.Locator("a.journey-box.cycling .col2.journey-info");

        private ILocator WalkingTime =>
            _page.Locator("a.journey-box.walking .col2.journey-info");

        public async Task<string> GetCyclingTime()
        {
            return await CyclingTime.InnerTextAsync();
        }

        public async Task<string> GetWalkingTime()
        {
            return await WalkingTime.InnerTextAsync();
        }

        private ILocator EditPreferencesButton =>
            _page.Locator("button.toggle-options");

        private ILocator LeastWalkingOption =>
            _page.Locator("[for='JourneyPreference_2']");

        private ILocator UpdateJourneyButton =>
            _page.Locator("#more-journey-options input.primary-button");

        private ILocator JourneyTime =>
              _page.Locator("#option-1-heading .journey-time.no-map");

        public async Task SelectLeastWalkingRoute()
        {
            await EditPreferencesButton.ClickAsync();
            await LeastWalkingOption.WaitForAsync();
            await LeastWalkingOption.ClickAsync();
            await UpdateJourneyButton.ScrollIntoViewIfNeededAsync();
            await UpdateJourneyButton.ClickAsync();
        }

        public async Task<string> GetUpdatedJourneyTime()
        {
            var time = await JourneyTime.InnerTextAsync();
            return time.Trim();
        }

        public async Task<bool> IsAccessInformationDisplayed(string accessInfo)
        {
            return await _page
                .Locator($"#option-1-content a.{accessInfo.ToLower().Trim().Replace(" ", "-")}.tooltip-container")
                .IsVisibleAsync();
        }

        private ILocator ViewDetailsButton =>
            _page.Locator("#option-1-content button.secondary-button.show-detailed-results.view-hide-details");

        public async Task ViewCompleteAccess()
        {
            await ViewDetailsButton.ScrollIntoViewIfNeededAsync();
            await ViewDetailsButton.ClickAsync();
        }

        public async Task PlanInvalidJourney()
        {
            await StartField.FillAsync("Invalid Location");
            await DestinationField.FillAsync("Covent Garden");
            await ClickPlanJourney();
        }

        private ILocator JourneyErrorMessage =>
            _page.GetByText("Sorry, we can't find a journey matching your criteria");

        public async Task<bool> IsJourneyErrorDisplayed()
        {
            return await JourneyErrorMessage.IsVisibleAsync();
        }

        public async Task PlansAnInvalidJourney()
        {
            await StartField.FillAsync("0000");
            await DestinationField.FillAsync("1111");
            await ClickPlanJourney();
        }

        public async Task<string> GetJourneyErrorMessage()
        {
            return await JourneyErrorMessage.InnerTextAsync();
        }

        public async Task PlanJourneyWithoutLocations()
        {
            await ClickPlanJourney();
        }

        private ILocator FromErrorMessage => _page.Locator("#InputFrom-error");

        private ILocator ToErrorMessage => _page.Locator("#InputTo-error");

        public async Task<string> GetFromErrorMessage()
        {
            return await FromErrorMessage.InnerTextAsync();
        }

        public async Task<string> GetToErrorMessage()
        {
            return await ToErrorMessage.InnerTextAsync();
        }
    }
}