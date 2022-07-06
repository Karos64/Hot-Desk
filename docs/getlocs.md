# Get all locations

Returns all locations that are stored in the database right now. 
This endpoint does not provide data about desks or reservations. 

**URL** : `/admin/locations`

**Method** : `GET`

**Auth required** : YES


## Success Response

**Code** : `200 OK`

**Content examples**

When there are only two locations stored in the database named 'Kraków' and 'Poznañ' we get following result.

```json
[
    {
        "id": 1,
        "name": "Kraków",
        "desks": []
    },
    {
        "id": 2,
        "name": "Poznañ",
        "desks": []
    }
]
```

## Error Response

**Condition** : If there is no `login-token` provided in header.

**Code** : `401 UNAUTHORIZED`

## Notes

* To get this result one must include `login-token` in request header with value that matches one set in main app.