# MyDriverAPI

## Overview
Simulation for cycle between drivers and passengers in real-world applications. when a passenger requests a trip 
from location to destination, all drivers that have this location in their favourite areas will be notified and 
offer price then it's the passenger's turn to choose the driver based on price, rate, info or car.

## Technologies Used
ASP.NET Core ,
C# ,
Entity Framework Core and
SQL Server 

## Key Features

### Authentication and Authorization
- API is secured using Identity, Jwt and refresh tokens 
- Define roles and permissions for users to ensure proper authorization

### CRUD Operations
- Create, Read, Update, and Delete operations for Drivers and Passengers
- Utilize RESTful principles to design consistent and predictable endpoints

### Design Patterns 
- (Repository pattern and Unit of work) is used to handle database connections 
- observer pattern is used to notify the drivers when there is an appropriate trip 

### Passenger Features  	
- the passenger can update his data 
- the passenger can request a trip and choose the location and destination 
- the passenger can show the offers he got from drivers and choose the best one for him
- the passenger can rate the driver after the trip ( 0 - 5)
- the passenger can show all his trip info 

### Driver Features 
- the driver can add his car photo to his profile
- the driver can add his favourite area to work in and he will be notified if there is any trip requested from these areas
- the driver can delete any area from his favourite areas 
- the driver can offer a price for the trip 
- the driver can check his status to see if he got any accepted offers 
- the driver can show all his trip info
  
## Getting Started
- to use this API you should have SQL server / SQL management studio 
- just in the package manager console add this " update-database "
