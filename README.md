# Pokedex API 
 
 Pokedex REST API is built using ASP.NET Core, retrieves pokemon information from public API https://pokeapi.co/.
 
 ## Tech Stack:
 .NET Core, Refit, AutoMapper, XUnit, Moq

## Install & Run

 Clone master. Run the following commands, inside the application directory:
 
    cd Pokedex/Pokedex.WebAPI

    dotnet run

This will launch the app on https://localhost:44342

## Endpoints �

http://localhost:44342/pokemon/:name	

http://localhost:44342/pokemon/translated/:name


Response body

    {
      "name": "mewtwo",
      "description": "It was created by a scientist after years of horrific\fgene splicing and DNA engineering experiments.",
      "habitat": "rare",
      "isLegendary": true
    }

Swagger documentation is also configured. More information on the API endpoints and responses can be accessed at this endpoint:

https://localhost:44342/swagger/index.html

## Unit Tests 🧪

Pokedex.Core.Tests.Core.Test.Unit includes unit tests for the core business logic.

The tests can be run either through Visual Studio or dotnet CLI.

    cd AddressBook.UnitTests
    
    dotnet test






