# Get all desks

Returns all desks that are stored in the database right now. 
This endpoint does not provide data about reservations. 

**URL** : `/admin/desks`

**Method** : `GET`

**Auth required** : YES


## Success Response

**Code** : `200 OK`

**Content examples**

When there are only three desks stored in the database in some specified location we get following result.

```json
[
    {
        "id": 1,
        "description": "Some desk",
        "reservations": [],
        "isAvailable": true
    },
    {
        "id": 2,
        "description": "Another desk",
        "reservations": [],
        "isAvailable": true
    },
    {
        "id": 3,
        "description": "Yet another desk",
        "reservations": [],
        "isAvailable": true
    }
]
```

## Error Response

**Condition** : If there is no `login-token` provided in header.

**Code** : `401 UNAUTHORIZED`

## Notes

* To get this result one must include `login-token` in request header with value that matches one set in main app.  
* This endpoint provide data about all desks stored in database so there is no way of filtering it by location or sth.