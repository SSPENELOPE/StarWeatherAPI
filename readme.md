# StarWeatherAPI

# What this is
- This is the backend for the StarWeatherApplication that will be wrapped up with electron

- The code has been slightly modified from the orignal react-weatherapp to work purely as a backend server

- The instructions provided in this repo are generic and assume you have the basic understanding of ASP.NET and Database connection. 

# Installation
- Clone the repostiory, you will then need to add your own appsettings.json to the project. In this repo is a file called temp-appsettings.json that will have the basic items needed. You should not need much more unless you are storing connection strings and other configurations in it like I do

* Ensure you have all the nuget dependencies installed
    * BCrypt.Net-Core
    * Microsoft.AspNetCore.Authentication.JwtBearer
    * Microsoft.AspNetCore.Mvc.NewtonsoftJson
    * Microsoft.Data.SqlClient
    * Microsoft.EntityFrameworkCore
    * Microsoft.EntityFrameworkCore.SqlServer
    * Microsoft.EntityFrameworkCore.Tools
    * Microsoft.Identity.Web.UI
    * Microsoft.Identity.Web
    * System.Data.SqlClient 

- Run the `dotnet build` command in your terminal, this should produce most of the files I have included in the .gitignore as they contain secrets

- NOTE you may need to tweak the .json files throughout the project to work with your own build/configuration. Also my code is not perfect so somethings may be redundant as I am still learning ASP.NET

# Usage 
- I recommend running the application in Visual Studio, you can right click the "StarWeatherAPI.sln" in the solution explorer and and open it. At the top of Visual Studio you will see the "play/start" green triangle and that is how I recommend running it. You can also select the drop down and use the IIS Express server if that is your preference.

* You will need to configure your own DB to run with this application. There are plenty of youtube videos on how to do that. I found these ones helpful: 
    * https://www.youtube.com/watch?v=wZfCIw5y5j8&t=38s
    * https://www.youtube.com/watch?v=ON-Z1iD6Y-c

- Routes can be tested directely in your browser or with Postman, Insomnia, etc..

## Questions/Support 
- You can email me at SSPENELOPE23@gmail.com with any questions. If you find anything code breaking or have suggestions feel free to send that over as well


