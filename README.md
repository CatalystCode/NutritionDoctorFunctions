# Ping An Nutrition Doctor - Functions

During the Microsoft One Week Hackathon, we [Microsoft] collaborated with Ping An to build Nutrition Doctor. Nutrition Doctor is an application that allows users to take a photo of a food item whereupon the app will identify the food item and present the user with nutritional information such as calories, fat, etc. 

## Architecture

![architecture](https://raw.githubusercontent.com/CatalystCode/NutritionDoctorApi/master/docs/architecture.png)

----

![call graph](https://raw.githubusercontent.com/CatalystCode/NutritionDoctorApi/master/docs/call_graph.png)

Each component is separated into its own Git repository:

* [Mobile Client](https://github.com/CatalystCode/NutritionDoctor)
* [Web Api](https://github.com/CatalystCode/NutritionDoctorApi) 
* [Functions](https://github.com/CatalystCode/NutritionDoctorFunctions)
* [Image Classifier](https://github.com/CatalystCode/NutritionDoctorImageClassifier)

## Getting Started

The Azure Function is triggered by queue messages. For each message, a call is made to Azure ML and Custom Vision to identify the image. The data is then persisted to a MySQL database.

1. `git clone git@github.com:CatalystCode/NutritionDoctorApi.git`
2. Open `NutritionDoctorApi.sln` in Visual Studio 2017
3. Update `local.settings.json` with the respective secrets for Storage Account, Custom Vision, and Azure ML.
4. `F5` to Debug