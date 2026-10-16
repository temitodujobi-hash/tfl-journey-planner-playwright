# TfL Journey Planner Automation Framework

## Overview

This project automates the Transport for London (TfL) Journey Planner application using:

- C#
- Playwright
- Reqnroll (SpecFlow)
- FluentAssertions
- NUnit
- Page Object Model (POM)

The framework validates key user journeys, accessibility information and input validation behaviours within the TfL Journey Planner application.

---

## Framework Design

The solution follows the Page Object Model design pattern to separate test logic from page interactions.

```text
Feature Files
        ↓
Step Definitions
        ↓
Page Objects
        ↓
Playwright
        ↓
Browser
```

### Project Structure

```text
UIAutomationTests
│
├── Features
│   └── JourneyPlanner.feature
│
├── Hooks
│   └── JourneyPlannerHooks.cs
│
├── Pages
│   └── JourneyPlannerPage.cs
│
├── Steps
│   └── JourneyPlannerSteps.cs
│
├── test.runsettings
│
└── UIAutomationTests.csproj
```

---

## Configuration

The application URL is externalised into a Run Settings file rather than being hard-coded within the automation framework.

This approach provides several benefits:

- Supports environment-specific execution
- Avoids hard-coded URLs
- Improves maintainability
- Allows configuration changes without source code modifications

Example:

```xml
<TestRunParameters>
  <Parameter name="BaseUrl" value="https://tfl.gov.uk" />
</TestRunParameters>
```

The framework reads the `BaseUrl` value at runtime and uses it when navigating to the application.



```

### Features

Contains the business-readable BDD scenarios.

### Step Definitions

Responsible for translating business language into automation actions and assertions.

### Page Objects

Responsible for page locators and reusable page actions.

### Hooks

Responsible for browser and Playwright setup and teardown.

---

## Development Decisions

### Why Playwright?

Playwright was selected because it provides:

- Automatic waiting mechanisms
- Reliable browser interaction
- Reduced test flakiness
- Modern browser support

### Why Reqnroll (SpecFlow)?

Reqnroll was selected to support a Behaviour Driven Development (BDD) approach.

Benefits include:

- Business-readable scenarios
- Collaboration between technical and non-technical stakeholders
- Traceability between requirements and automated tests

### Why Page Object Model?

The Page Object Model pattern was used to:

- Improve maintainability
- Reduce code duplication
- Separate locators from test logic
- Promote reusable automation components

Example:

```csharp
private ILocator StartField =>
    _page.Locator("#InputFrom");
```

Locators remain within page objects and are never accessed directly from step definitions.

### Data-Driven Testing

ReqnRoll tables were used to validate accessibility information and error messages without duplicating test logic.

Example:

```gherkin
Then user should be presented with the following access information

| AccessInformation | IsDisplayed |
| Up Stairs         | true        |
| Up Lift           | true        |
| Level Walkway     | true        |
```

This allows new accessibility indicators to be added without modifying the automation code.

---

## Implemented Test Scenarios

### Scenario 1 - Plan a Valid Journey Between Two Stations

Validates:

- Journey planning
- Auto-complete station selection
- Journey results page
- Cycling journey time
- Walking journey time

### Scenario 2 - Update Journey with Least Walking Routes

Validates:

- Edit Preferences
- Least Walking route selection
- Update journey functionality
- Updated journey duration

### Scenario 3 - Verify Complete Access Information

Validates:

- View Details functionality
- Accessibility information

Including:

- Up Stairs
- Up Lift
- Level Walkway

### Scenario 4 - Plan an Invalid Journey

Validates error handling for invalid locations.

Expected result:

```text
Sorry, we can't find a journey matching your criteria
```

### Scenario 5 - Display Error Messages When No Locations Are Entered

Validates required field messages:

```text
The From field is required.
The To field is required.
```

---

## Additional Future Scenarios

The feature file also contains additional scenarios that have been intentionally marked with:

```gherkin
@ignore
```

These were identified as potential future enhancements, including:

- Editing destinations
- Recently planned journeys
- Future journey planning
- Favourite journeys
- Saved preferences
- Multi-user testing
- Response time validation
- SQL injection validation

---

## Challenges Encountered

### Cookie Consent Banner

The TfL application occasionally displayed a cookie consent overlay which prevented user interaction.

Example runtime issue:

```text
cb-cookieoverlay intercepts pointer events
```

The framework handles this by conditionally waiting for and accepting the cookie banner when it appears.

Example:

```csharp
await AcceptCookiesButton.WaitForAsync(new()
{
    State = WaitForSelectorState.Visible,
    Timeout = 5000
});
```

This avoids unnecessary hard-coded waits and improves test stability.

### Dynamic Journey Results

Journey durations can vary depending on departure time and route availability.

For this reason, validations focus on:

- Presence of journey results
- Presence of journey duration information

rather than hard-coded journey durations.

### Multiple Matching Elements

The TfL site contains repeated controls such as:

- View Details
- Journey Time
- Accessibility indicators

Locators were refined to target the appropriate journey card while maintaining Playwright's strict mode compliance.

---

## Prerequisites

- .NET 9
- Visual Studio 2022+
- Playwright

Install Playwright browsers:

```bash
playwright install
```



### Configure Test Settings

Ensure the Run Settings file is selected before executing the tests:

```text
Test
→ Configure Run Settings
→ Select Solution Wide RunSettings File
```

Select:

```text
test.runsettings
```

### Execute via Visual Studio

```text
Test Explorer
→ Run All Tests
```

### Execute via Command Line

Run all tests:

```bash
dotnet test
```

Or execute using the Run Settings file:

```bash
dotnet test --settings test.runsettings
```

## Use of AI

AI-assisted tooling was used during the development of this project to support:

- Playwright implementation guidance
- Troubleshooting and debugging activities
- Documentation generation

AI was used as a development aid and not as a substitute for engineering judgement. All automation design decisions, implementation, debugging, test execution, and final validation were performed by the project author.

All generated suggestions were reviewed, adapted where necessary, and validated through successful execution of the automated test suite.

---

## Author

Temi
Senior Test Engineer