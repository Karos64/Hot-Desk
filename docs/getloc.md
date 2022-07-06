# Get data about specific location

Returns data about location specified with `{id}`

**URL** : `/locations/{id}`

**Method** : `GET`

**Auth required** : NO

## Success Response

**Code** : `200 OK`

**Content examples** : Without including `login-token` in header:
```json
{
    "id": 1,
    "name": "Poznañ",
    "desks": [
        {
            "id": 2,
            "description": "Another desk",
            "reservations": [],
            "isAvailable": true
        }
    ]
}
```

**Content examples** : Including `login-token` in header:
```json

{
    "id": 1,
    "name": "Poznañ",
    "desks": [
        {
            "id": 2,
            "description": "Another desk",
            "reservations": [
                {
                    "id": 1,
                    "startDate": "2022-07-06T16:31:13.8754298+02:00",
                    "endDate": "2022-07-06T17:31:13.8754356+02:00",
                    "reservedBy": "John Smith"
                }
            ],
            "isAvailable": true
        }
    ]
}
```


## Error Response

**Condition** : When there is no location with provided ID.

**Code** : `404 NOT FOUND`

## Notes

* It does not require `login-token` in header. But when `login-token` is included, the endpoint will return **all** the data about this location, including desks and reservations info. Without it though only basic information will be returned.
