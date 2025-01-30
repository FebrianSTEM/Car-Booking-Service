# Setup Documentation for Running a .NET 8 Project

## Prerequisites

Before running the .NET 8 project, ensure you have the following installed on your machine:

1. **Operating System**:
   - Windows, macOS, or Linux

2. **.NET 8 SDK**:
   - Download and install the .NET 8 SDK from the official [Microsoft .NET website](https://dotnet.microsoft.com/).

3. **Code Editor**:
   - Visual Studio Code (free, cross-platform) or Visual Studio 2022 (Windows/macOS).
     - **Visual Studio Code**: Install the [C# Extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp).
     - **Visual Studio 2022**: Ensure the ".NET desktop development" workload is installed.

4. **Database System** (if applicable):
   - Install the required database system (e.g., SQL Server, PostgreSQL, MySQL, etc.) based on your project requirements.

5. **Node.js** (Optional):
   - If the project uses front-end frameworks (e.g., React or Angular), install [Node.js](https://nodejs.org/) to manage dependencies.

---

## Step-by-Step Setup

### 1. Install .NET 8 SDK
- **Download**: Visit [https://dotnet.microsoft.com/download/dotnet/8.0](https://dotnet.microsoft.com/download/dotnet/8.0).
- **Install**:
  - Run the installer and follow the instructions.
  - Verify the installation by running the following command in your terminal:
    ```bash
    dotnet --version
    ```
    Output should display a version like `8.x.x`.

### 2. Clone the Project Repository
- Clone the repository to your local machine:
  ```bash
  git clone <repository-url>
  cd <project-folder>
  ```

### 3. Restore Dependencies
- Use the following command to restore NuGet packages:
  ```bash
  dotnet restore
  ```

### 4. Build the Project
- Compile the project using the following command:
  ```bash
  dotnet build
  ```

### 5. Run the Project
#### From Visual Studio 2022
1. Open the project in **Visual Studio 2022**.
2. Set startup project:
   - Right-click on the **car-booking-service** project in the **Solution Explorer** and select **Set as Startup Project**.
3. Run the project:
   - Click the green **Start** button or press `F5` to run the application in Debug mode.
   - Alternatively, press `Ctrl + F5` to run the application without debugging.
4. Visual Studio will launch the application and open the default browser at the application URL http://localhost:5276/swagger or https://localhost:7125/swagger

#### From the Command Line
- Run the project in development mode:
  ```bash
  dotnet run --project car-booking-service
  ```

### 6. Access the Application
- Open a browser and navigate to the URL specified in the terminal or Visual Studio output (e.g., `https://localhost:5001`).
URL http://localhost:5276/swagger or https://localhost:7125/swagger
---

### 7. Logging
- To access logging data you could access it in hyundai-testDriveBooking-service\hyundai-testDriveBooking-service\Logs

---