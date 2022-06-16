# Blazor based Magic The Gathering lookup [![GitHub issues](https://img.shields.io/github/issues/theomenden/MTGView.Blazor.Server?style=plastic)](https://github.com/theomenden/MTGView.Blazor.Server/issues) [![GitHub license](https://img.shields.io/github/license/theomenden/MTGView.Blazor.Server)](https://github.com/theomenden/MTGView.Blazor.Server)
## This application is built using [Blazor](https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor)
  - Rationale for using blazor comes from:
    - The ability to use C# throughout the entire application with minimal javascript interaction/interop.
    - Easier Maintainence from a single developer team
  
  Thank you for understanding! I have no plans on moving this to Angular or React at this time.
 
## The UI Components are built using [Blazorise](https://www.blazorise.com) and [Bootstrap 5](https://www.getbootstrap.com)
 Blazorise is a free, and easy to use component library for blazor server and web applications, providing access to over 80 free components.
 
 Bootstrap is an independent CSS framework, aimed towards responsive design.
# To retrieve the most up to date card data, we use a combination of: MTGJson & Scryfall API
## [MtgJson](https://mtgjson.com/)
### **MtgJson** is where most of the underlying card information is imported, via a self-contained background running service
## [Scryfall](https://scryfall.com/)
### **Scryfall** is where the imagery, and latest pricing information is imported. 
