### API Call Tables
Base URL = localhost:[PORT]/api/

1. Get All Todos     : (GET METHOD) ex : localhost:8080/api/todo
```
Expected Result :
    {
    "id": 1,
    "title": "First Todo Task",
    "descriptions": "First Todo Task",
    "percentageOfCompletion": 0,
    "expirationDate": "2022-04-01T21:31:18.027Z"
    },
    {
    "id": 2,
    "title": "Second Todo Task",
    "descriptions": "Second Todo Task",
    "percentageOfCompletion": 0,
    "expirationDate": "2022-04-01T21:31:18.027Z"
    }

    HttpStatus.OK
```
2. Get Todos By Id  : (GET METHOD) ex : localhost:8080/api/todo/1
```
    Expected Result :
        {
        "id": 1,
        "title": "First Todo Task",
        "descriptions": "First Todo Task",
        "percentageOfCompletion": 0,
        "expirationDate": "2022-04-01T21:31:18.027Z"
        }

    HttpStatus.OK
``` 

3. Create Todos     : (POST METHOD) ex : localhost:8080/api/todo
```
    Example Body and Expected Result :
        {
        "id": 1,
        "title": "First Todo Task",
        "descriptions": "First Todo Task",
        "percentageOfCompletion": 0,
        "expirationDate": "2022-04-01T21:31:18.027Z"
        }

    HttpStatus.CREATED
``` 
4. Update todos : (PUT METHOD) ex : localhost:8080/api/todo/1
 ```
    Example Body and Expected Result :
        {
        "id": 1,
        "title": "First Todo Task",
        "descriptions": "First Todo Task",
        "percentageOfCompletion": 0,
        "expirationDate": "2022-04-01T21:31:18.027Z"
        }

    HttpStatus.OK
``` 
5. Delete todos : (DELETE METHOD) ex : localhost:8080/api/todo/1
```
    Expected Result : HttpStatus.OK
``` 
6. Mark Todo as Done : (GET METHOD) ex : localhost:8080/api/todo/mark_done/1
 ```
    Expected Result :
        {
        "id": 1,
        "title": "First Todo Task",
        "descriptions": "First Todo Task",
        "percentageOfCompletion": 100,
        "expirationDate": "2022-04-01T21:31:18.027Z"
        }
    
    HttpStatus.OK
``` 
7. Change Todo Percentage : (GET METHOD) ex : localhost:8080/api/todo/1/percentage=20
 ```
    Expected Result :
        {
        "id": 1,
        "title": "First Todo Task",
        "descriptions": "First Todo Task",
        "percentageOfCompletion": 20,
        "expirationDate": "2022-04-01T21:31:18.027Z"
        }
    
    HttpStatus.OK
``` 
8. Get Incoming Todos : (GET METHOD) ex : localhost:8080/api/todo/incoming/1
 ```
    Expected Result :
        {
        "id": 3,
        "title": "Third Todo Task",
        "descriptions": "Third Todo Task",
        "percentageOfCompletion": 10,
        "expirationDate": "2022-04-02T21:31:18.027Z"
        },
        {
        "id": 4,
        "title": "Fifth Todo Task",
        "descriptions": "Fifth Todo Task",
        "percentageOfCompletion": 50,
        "expirationDate": "2022-04-02T21:31:18.027Z"
        },
    
    HttpStatus.OK
``` 