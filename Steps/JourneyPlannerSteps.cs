using FluentAssertions;
using Reqnroll;
using UIAutomationTests.Hooks;
using UIAutomationTests.Pages;

namespace UIAutomationTests.Steps
{
    [Binding]
    public class JourneyPlannerSteps
    {
        private readonly JourneyPlannerPage _journeyPlannerPage;
        private readonly string? _originalJourneyTime;

        public JourneyPlannerSteps()
        {
            _journeyPlannerPage = 
                new JourneyPlannerPage(JourneyPlannerHooks.Page!);
        }

        [Given(@"user is on the TFL home page")]
        public async Task GivenUserIsOnTheTFLHomePage()
        {
            await _journeyPlannerPage.NavigateToJourneyPlanner();
        }

        [When(@"user plans a journey from '(.*)' to '(.*)'")]
        public async Task WhenUserPlansAJourneyFromTo(string from, string to)
        {
            await _journeyPlannerPage.PlanJourney(from, to);
        }

        [Then(@"user should be presented with the Journey Results")]
        public async Task ThenUserShouldBePresentedWithTheJourneyResults()
        {
            var result =
       await _journeyPlannerPage.IsJourneyResultDisplayed();

            result.Should().BeTrue();
        }

        [Then(@"'(.*)' time should be be displayed")]
        public async Task ThenTimeShouldBeBeDisplayed(string journeyType)
        {
            string time = string.Empty;

            switch (journeyType.ToLower())
            {
                case "cycling":
                    time = await _journeyPlannerPage.GetCyclingTime();
                    break;

                case "walking":
                    time = await _journeyPlannerPage.GetWalkingTime();
                    break;
            }
            TestContext.WriteLine($"{journeyType} journey time = {time}");

            time.Should().NotBeNullOrWhiteSpace();
        }

        [When(@"user updates journey using least walking preference")]
        public async Task WhenUserUpdatesJourneyUsingLeastWalkingPreference()
        {
            await _journeyPlannerPage.SelectLeastWalkingRoute();
        }

        [Then(@"journey time should be displayed")]
        public async Task ThenJourneyTimeShouldBeDisplayed()
        {
            var journeyTime = await _journeyPlannerPage.GetUpdatedJourneyTime();
            TestContext.WriteLine($"Original Journey Time = {_originalJourneyTime}");
            var updatedJourneyTime = await _journeyPlannerPage.GetUpdatedJourneyTime();
            TestContext.WriteLine($"Updated Journey Time = {updatedJourneyTime}");
            journeyTime.Should().NotBe(_originalJourneyTime);
        }

        [Given(@"a journey contains least walking routes between '(.*)' to '(.*)'")]
        public async Task GivenAJourneyContainsLeastWalkingRoutesBetweenTo(
            string from,
            string to)
        {
            await _journeyPlannerPage.NavigateToJourneyPlanner();
            await _journeyPlannerPage.PlanJourney(from, to);
            await _journeyPlannerPage.SelectLeastWalkingRoute();
        }

        [When(@"user views the complete access information")]
        public async Task WhenUserViewsTheCompleteAccessInformation()
        {
            await _journeyPlannerPage.ViewCompleteAccess();
        }

        [Then(@"user should be presented with the following access information")]
        public async Task ThenUserShouldBePresentedWithTheFollowingAccessInformation(Table table)
        {
            foreach (var row in table.Rows)
            {
                var accessInformation =
                    row["AccessInformation"];

                var expectedResult =
                    bool.Parse(row["IsDisplayed"]);

                var actualResult =
                    await _journeyPlannerPage
                        .IsAccessInformationDisplayed(accessInformation);

                actualResult.Should().Be(expectedResult);
            }
        }

        [When(@"user plans an invalid journey")]
        public async Task WhenUserPlansAnInvalidJourney()
        {
            await _journeyPlannerPage.PlansAnInvalidJourney();
        }

        [Then(@"user should be presented with the Journey Results page with an error message")]
        public async Task ThenUserShouldBePresentedWithTheJourneyResultsPageWithAnErrorMessage()
        {
            var errorMessage =
                await _journeyPlannerPage.GetJourneyErrorMessage();

            errorMessage.Should().Contain(
                "Sorry, we can't find a journey matching your criteria");
        }

        [When(@"user tries to plan a journey without locations")]
        public async Task WhenUserTriesToPlanAJourneyWithoutLocations()
        {
            await _journeyPlannerPage.PlanJourneyWithoutLocations();
        }

        [Then(@"user should be presented with required fields error messages")]
        public async Task ThenUserShouldBePresentedWithRequiredFieldsErrorMessages(
    Table table)
        {
            var expectedFrom =
                table.Rows[0]["From"];

            var expectedTo =
                table.Rows[0]["To"];

            var actualFrom =
                await _journeyPlannerPage.GetFromErrorMessage();

            var actualTo =
                await _journeyPlannerPage.GetToErrorMessage();

            actualFrom.Should().Be(expectedFrom);

            actualTo.Should().Be(expectedTo);
        }
    }
}