# DogBreedsMvc
MVC Website to manage dog breeds

# Prerequisites

- Visual Studio version 2017. 
- .NET Core 2.2 SDK
- SQL Server Express

# Running the Application

## Locally
- Clone the repo from https://github.com/dttw/DogBreedsMvc
- Open the DogBreeds.Mvc.sln file with Visual Studio 2017
- Run the Initial Migrations 
    - Open the Package Manager Console (Tools > NuGet Package Manager > Package Manager Console)
    - Run the command EntityFrameworkCore\Update-Database
- Run the application using IIS Express as the hosting platform
- The database is populated with initial breeds from the json file stored as an embedded resource in the application
- You will be present with the home page of the application

## Remotely
- Navigate to http://whittenhamg-001-site1.htempurl.com/ in our chosen web browser.
- You will be presented with the home page of the application

# Using the Application

## Managing Breeds 
- Click on Breeds tab 
### Creating 
- Select 'Create'
- Enter the name of the breed
- If the breed is a type of another breed (for example, 'Yorkshire' and 'Jack Russell' are types of 'Terrier') you may select the "parent" breed from the dropdown
### Editing 
- Select 'Edit' 
- You can change the name of the breed
- You can change the type of the breed 
### Deleting 
- Select 'Delete' 
- You will be prompted for confirmation (You cannot delete if there are any individuals of that breed, or if any other breeds are of the type of breed being deleted) 
### Viewing 
- Select 'Details' 
- Shows any individuals of that breed
- Shows any breeds which are subtypes of that breed (for example 'Terrier' could show the subtypes of 'Yorkshire' and 'Jack Russell')

## Managing Individuals 
- Click on Individuals tab 
### Creating 
- Select 'Create'
- Enter the name of the individual
- Select the Breed for the individual, an individual must be associated with a breed.
### Editing 
- Select 'Edit' 
- You can change the name of the individual
- You can change the breed of the individual 
### Deleting 
- Select 'Delete' 
- You will be prompted for confirmation
### Viewing 
- Select 'Details'

# Tests

## Run C# tests using the Visual Studio test runner
- Open the Test Explorer in Visual Studio (Test menu > Windows > Test Explorer)
- In the default view you will see a single entry for the tests in the DogBreeds.Test project.
- Expanding this entry will show the Controller level tests, which can again be expanded to show the BreedControllerTests and IndividualControllerTests
- The Controller Tests hold the individual tests which can be run against each controller.
- To run tests right click in the Test Explorer at the chosen level (e.g. DogBreeds.Test.Controllers) and all tests under that level will be run. 
- Selecting an individual test will run only that test.
