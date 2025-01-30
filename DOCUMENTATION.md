# Car Booking Service API Documentation

## Overview

- **Base URL:** `/api`
- **Common Response Structure**

     All responses follow this structure:

    ```json
    {
      "status": "string",
      "code": integer,
      "data": object or array of returned data,
      "message": "string"
    }
    
    //and this for paginated one

    {
      "status": "string",
      "code": integer,
      "data": object or array of returned data,
      "message": "string",
      "page_index": integer,
      "page_size": integer,
      "total_data": integer,
      "total_pages": integer
    }

---

## Endpoints

### 1. Booking

#### **GET** `/api/Booking`
Retrieve all bookings.

- **Responses:**
  - `200 OK`: List of bookings retrieved.
    ```json
      {
      "status": "string",
      "code": 0,
      "data": [
        {
          "booking_id": 0,
          "car_id": 0,
          "car_model_brand": "string",
          "car_model_name": "string",
          "car_model_year": 0,
          "booking_date_time": "2025-01-16T19:04:32.347Z",
          "customer_name": "string",
          "customer_email": "string",
          "customer_phone": "string",
          "created_by": "string",
          "created_at": "2025-01-16T19:04:32.347Z",
          "updated_by": "string",
          "updated_at": "2025-01-16T19:04:32.347Z"
        }
      ],
      "message": "string"
    }
    ```

#### **GET** `/api/Booking/{bookingId}`
Retrieve a specific booking by ID.

- **Path Parameters:**
  - `booking_id` (integer, required): The ID of the booking.
- **Responses:**
  - `200 OK`: Booking retrieved successfully.
    ```json
    {
      "status": "string",
      "code": 200,
      "data": {
        "booking_id": 1,
        "car_id": 2,
        "car_model_brand": "string",
        "car_model_name": "string",
        "car_model_year": 2022,
        "booking_date_time": "2023-01-01T00:00:00Z",
        "customer_name": "string",
        "customer_email": "string",
        "customer_phone": "string",
        "created_by": "string",
        "created_at": "2023-01-01T00:00:00Z",
        "updated_by": "string",
        "updated_at": "2023-01-01T00:00:00Z"
      },
      "message": "string"
    }
    ```
#### **GET** `/api/Booking/list`
Retrieve a filtered list of bookings.

- **Query Parameters:**
  - `start_date` (string, required): Start date filter (ISO 8601 format).
  - `end_date` (string, required): End date filter (ISO 8601 format).
  - `customer_name` (string, optional): Customer name filter.
  - `customer_phone` (string, optional): Customer phone filter.
  - `customer_email` (string, optional): Customer email filter.
  - `car_id` (integer, optional): Car ID filter.
  - `car_brand` (string, optional): Car brand filter.
  - `car_model` (string, optional): Car model filter.
  - `car_year` (integer, optional): Car year filter.
- **Responses:**
  - `200 OK`: Filtered bookings retrieved successfully.
    ```json
    {
      "status": "string",
      "code": 200,
      "data": {
        "booking_id": 1,
        "car_id": 2,
        "car_model_brand": "string",
        "car_model_name": "string",
        "car_model_year": 2022,
        "booking_date_time": "2023-01-01T00:00:00Z",
        "customer_name": "string",
        "customer_email": "string",
        "customer_phone": "string",
        "created_by": "string",
        "created_at": "2023-01-01T00:00:00Z",
        "updated_by": "string",
        "updated_at": "2023-01-01T00:00:00Z"
      },
      "message": "string"
    }
    ```
#### **GET** `/api/Booking/list`
Retrieve a filtered list of bookings.

- **Query Parameters:**
  - `page` (integer, required): n page to be shown of all total pages.
  - `page_size` (integer, required): Amount of data to retrieve in one page.
  - `start_date` (string, required): Start date filter (ISO 8601 format).
  - `end_date` (string, required): End date filter (ISO 8601 format).
  - `customer_name` (string, optional): Customer name filter.
  - `customer_phone` (string, optional): Customer phone filter.
  - `customer_email` (string, optional): Customer email filter.
  - `car_id` (integer, optional): Car ID filter.
  - `car_brand` (string, optional): Car brand filter.
  - `car_model` (string, optional): Car model filter.
  - `car_year` (integer, optional): Car year filter.
- **Responses:**
  - `200 OK`: Filtered bookings retrieved successfully.
    ```json
    {
      "status": "string",
      "code": 0,
      "data": [
        {
          "booking_id": 0,
          "car_id": 0,
          "car_model_brand": "string",
          "car_model_name": "string",
          "car_model_year": 0,
          "booking_date_time": "2025-01-17T03:28:06.461Z",
          "customer_name": "string",
          "customer_email": "string",
          "customer_phone": "string",
          "created_by": "string",
          "created_at": "2025-01-17T03:28:06.461Z",
          "updated_by": "string",
          "updated_at": "2025-01-17T03:28:06.461Z"
        }
      ],
      "message": "string",
      "page_index": 0,
      "page_size": 0,
      "total_data": 0,
      "total_pages": 0
    }
    ```


#### **POST** `/api/Booking`
Create a new booking.

- **Request Body:**
  ```json
  {
    "car_id": 1,
    "customer_name": "string",
    "customer_phone": "string",
    "customer_email": "string",
    "booking_date_time": "2023-01-01T00:00:00Z"
  }
  ```
- **Responses:**
  - `201 Created`: Booking created successfully.
    ```json
      {
      "status": "string",
      "code": 0,
      "data": [
        {
          "booking_id": 0,
          "car_id": 0,
          "car_model_brand": "string",
          "car_model_name": "string",
          "car_model_year": 0,
          "booking_date_time": "2025-01-16T19:04:32.347Z",
          "customer_name": "string",
          "customer_email": "string",
          "customer_phone": "string",
          "created_by": "string",
          "created_at": "2025-01-16T19:04:32.347Z",
          "updated_by": "string",
          "updated_at": "2025-01-16T19:04:32.347Z"
        }
      ],
      "message": "string"
    }
    ```

#### **PUT** `/api/Booking`
Update an existing booking.

- **Request Body:**
  ```json
  {
    "booking_id": 1,
    "car_id": 2,
    "customer_name": "string",
    "customer_phone": "string",
    "customer_email": "string",
    "booking_date_time": "2023-01-01T00:00:00Z"
  }
  ```
- **Responses:**
  - `200 OK`: Booking updated successfully.
    ```json
      {
      "status": "string",
      "code": 0,
      "data": [
        {
          "booking_id": 0,
          "car_id": 0,
          "car_model_brand": "string",
          "car_model_name": "string",
          "car_model_year": 0,
          "booking_date_time": "2025-01-16T19:04:32.347Z",
          "customer_name": "string",
          "customer_email": "string",
          "customer_phone": "string",
          "created_by": "string",
          "created_at": "2025-01-16T19:04:32.347Z",
          "updated_by": "string",
          "updated_at": "2025-01-16T19:04:32.347Z"
        }
      ],
      "message": "string"
    }
    ```


#### **DELETE** `/api/Booking/{bookingId}`
Delete a booking by ID.

- **Path Parameters:**
  - `booking_id` (integer, required): The ID of the booking.
- **Responses:**
  - `204 No Content`: Booking deleted successfully.

---

### 2. Car Models

#### **GET** `/api/CarModels`
Retrieve all car models.

- **Responses:**
  - `200 OK`: List of car models retrieved.
  ```json
    {
      "status": "string",
      "code": 0,
      "data": [
        {
          "car_id": 0,
          "brand": "string",
          "model": "string",
          "year": 0,
          "image_url": "string",
          "description": "string",
          "is_available_for_test_drive": true,
          "created_by": "string",
          "created_at": "2025-01-16T19:12:22.340Z",
          "updated_by": "string",
          "updated_at": "2025-01-16T19:12:22.340Z"
        }
      ],
      "message": "string"
    }
    ```
    

#### **GET** `/api/CarModels/list`
Retrieve a filtered list of car models.

- **Query Parameters:**
  - `brand` (string, optional): Car brand filter.
  - `model` (string, optional): Car model filter.
  - `year` (integer, optional): Car year filter.
  - `description` (string, optional): Description filter.
  - `is_available` (boolean, optional): Availability filter.
- **Responses:**
  - `200 OK`: Filtered car models retrieved successfully.
  ```json
    {
      "status": "string",
      "code": 0,
      "data": [
        {
          "car_id": 0,
          "brand": "string",
          "model": "string",
          "year": 0,
          "image_url": "string",
          "description": "string",
          "is_available_for_test_drive": true,
          "created_by": "string",
          "created_at": "2025-01-16T19:12:22.340Z",
          "updated_by": "string",
          "updated_at": "2025-01-16T19:12:22.340Z"
        }
      ],
      "message": "string"
    }
    ```
  
#### **GET** `/api/CarModels/paging`
Retrieve a filtered list of car models.

- **Query Parameters:**
  - `page` (integer, required): n Page of total page.
  - `page_size` (integer, required): Amount of data show in one page.
  - `brand` (string, optional): Car brand filter.
  - `model` (string, optional): Car model filter.
  - `year` (integer, optional): Car year filter.
  - `description` (string, optional): Description filter.
  - `is_available` (boolean, optional): Availability filter.
- **Responses:**
  - `200 OK`: Filtered car models retrieved successfully.
  ```json
      {
        "status": "string",
        "code": 0,
        "data": [
          {
            "car_id": 0,
            "brand": "string",
            "model": "string",
            "year": 0,
            "image_url": "string",
            "description": "string",
            "is_available_for_test_drive": true,
            "created_by": "string",
            "created_at": "2025-01-17T03:28:06.468Z",
            "updated_by": "string",
            "updated_at": "2025-01-17T03:28:06.468Z"
          }
        ],
        "message": "string",
        "page_index": 0,
        "page_size": 0,
        "total_data": 0,
        "total_pages": 0
      }
    ```

#### **GET** `/api/CarModels/{carId}`
Retrieve a specific car model by ID.

- **Path Parameters:**
  - `car_id` (integer, required): The ID of the car model.
- **Responses:**
  - `200 OK`: Car model retrieved successfully.
  ```json
    {
      "status": "string",
      "code": 0,
      "data": {
        "car_id": 0,
        "brand": "string",
        "model": "string",
        "year": 0,
        "image_url": "string",
        "description": "string",
        "is_available_for_test_drive": true,
        "created_by": "string",
        "created_at": "2025-01-16T19:14:21.060Z",
        "updated_by": "string",
        "updated_at": "2025-01-16T19:14:21.060Z"
      },
      "message": "string"
    }
    ```

#### **POST** `/api/CarModels`
Create a new car model.

- **Request Body:**
  ```json
  {
    "brand": "string",
    "model": "string",
    "year": 2022,
    "image_url": "string",
    "description": "string"
  }
  ```
- **Responses:**
  - `201 Created`: Car model created successfully.
  ```json
    {
      "status": "string",
      "code": 0,
      "data": {
        "car_id": 0,
        "brand": "string",
        "model": "string",
        "year": 0,
        "image_url": "string",
        "description": "string",
        "is_available_for_test_drive": true,
        "created_by": "string",
        "created_at": "2025-01-16T19:14:21.060Z",
        "updated_by": "string",
        "updated_at": "2025-01-16T19:14:21.060Z"
      },
      "message": "string"
    }
    ```

#### **PUT** `/api/CarModels`
Update an existing car model.

- **Request Body:**
  ```json
  {
    "car_id": 1,
    "brand": "string",
    "model": "string",
    "year": 2022,
    "image_url": "string",
    "description": "string",
    "is_available_for_test_drive": true
  }
  ```
- **Responses:**
  - `200 OK`: Car model updated successfully.

#### **DELETE** `/api/CarModels/{carId}`
Delete a car model by ID.

- **Path Parameters:**
  - `car_id` (integer, required): The ID of the car model.
- **Responses:**
  - `204 No Content`: Car model deleted successfully.
---