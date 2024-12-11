# CSharpUnitTests
A simple project to practice Unit testing skills

## Setup
You will need [Visual Studio](https://visualstudio.microsoft.com/vs/community/) or similar IDE  
You will also need the [DotNet 7 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)  

1. Extract the application to your local machine
2. Open the solution with your editor of choice and build
3. Open Test Explorer, One test should be discovered called "ExampleTest"
4. Run "ExampleTest" and if everything is working, the test should pass

The Application project contains the code for the app under test.

The Tests project contains the example integration test of the
app under test using MSTest


## Requirements

As a user working in a remote team  
I want to see the current time in the UK and Canada  
So I know what time it is for my colleagues  

**Acceptance Criteria**

* Must get the current date and time from https://worldtimeapi.org/
* Must display the current time for the UK and Canada
* Date and time must be displayed in the format `Monday 1 January 2023 17:00:00`
* Must display the difference in time between the UK and Canada
* Must show temperature for current location

