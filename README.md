# DCXAir - Backend

DCXAir is a backend API developed using C# with .NET, designed to provide flight search services. It allows users to search for flights by specifying the origin, destination, journey type (one-way or round-trip), and currency. The API returns flight details corresponding to the requested parameters. The backend fetches data from a JSON file, which is then loaded into a database using SQLite and Entity Framework to generate the API.

The backend follows a layered architecture, ensuring clean separation of concerns between different application layers: the presentation layer (API controllers), the service layer, and the data access layer (repositories).

## Features

- **Flight Search API**: Allows users to search for available flights by providing the origin, destination, journey type (one-way or round-trip), and currency. The API returns detailed information about the flights, including flight number, airline, origin, destination, and price.

- **Data Storage**: Flight data is loaded from a JSON file into an SQLite database, where it is then queried and served through the API.

- **Journey Type**: The API supports both one-way and round-trip flight searches, returning relevant flight options based on the specified type.

- **Currency Support**: The API can handle different currencies by accepting a currency parameter, making it easier for users to view flight prices in their preferred currency.

- **Error Handling**: The backend handles errors gracefully, ensuring proper responses when invalid parameters are provided or when no flights are found.

## Prerequisites

- .NET 6.0 or higher
- SQLite or other compatible databases (e.g., SQL Server, PostgreSQL)
- Visual Studio or Visual Studio Code

## Installation

Follow these steps to set up the backend on your local machine:

1. Clone this repository:
   "git clone https://github.com/DanielFAdarve/DCXAir.Backend"

2. Navigate to the project directory:
   "cd <project-directory>"

3. Install dependencies:
   "dotnet restore"

4. Build the project:
   "dotnet build"

5. Apply database migrations to create tables:
   "dotnet ef migrations add InitialCreate"
   "dotnet ef database update"

## How to Start

1. Run the application using the following command:
   "dotnet run"

2. Open your browser and go to "http://localhost:7156/" (or the URL specified in the terminal).

3. The API will be live and ready to handle flight search requests.

## Endpoints

### 1. **GET /api/Flight**

Search for flights based on origin, destination, journey type, and currency.

**Parameters:**
- "origin" (string): The origin airport code (e.g., "BOG").
- "destination" (string): The destination airport code (e.g., "MZL").
- "type" (string): The journey type ("one-way" or "round-trip").
- "currency" (string): The currency in which the flight prices will be displayed (e.g., "USD").

**Response Example:**
"{
  "origin": "BOG",
  "destination": "MZL",
  "currency": "USD",
  "totalRoutes": 2,
  "journeys": [
    {
      "type": "One-way direct",
      "flights": [
        {
          "origin": "BOG",
          "destination": "MZL",
          "price": 1000,
          "currency": "USD",
          "transport": [
          {
              "flightCarrier": "AV",
              "flightNumber": "8080"
            }
          ]
        }
      ],
      "totalPrice": 1000
    },
    {
      "type": "Connecting",
      "flights": [
        {
          "origin": "BOG",
          "destination": "PEI",
          "price": 1000,
          "currency": "USD",
          "transport": [
            {
              "flightCarrier": "AV",
              "flightNumber": "8090"
            }
          ]
        },
        {
          "origin": "PEI",
          "destination": "MZL",
          "price": 1000,
          "currency": "USD",
          "transport": [
            {
              "flightCarrier": "AV",
              "flightNumber": "8050"
            }
          ]
        }
      ],
      "totalPrice": 2000
    }
  ]
}"

## Architecture

The backend is designed using a layered architecture:

1. **Controllers (Presentation Layer)**: The API controllers handle incoming requests and return appropriate responses.
2. **Services (Business Logic Layer)**: The services contain the business logic for processing flight data and handling user queries.
3. **Repositories (Data Access Layer)**: The repositories interact with the database to retrieve flight data, load JSON data into the database, and handle CRUD operations.
