# Get data about specific desk in location

Returns data about desk specified with `{deskid}` at location specified with `{locid}`

**URL** : `/locations/{locid}/desks/{deskid}`

**Method** : `GET`

**Auth required** : NO

## Success Response

**Code** : `200 OK`

**Content examples** : Without including `login-token` in header:
```json
{
    "id": 1,
    "description": "Some desk",
    "reservations": [],
    "isAvailable": true
}
```

**Content examples** : Including `login-token` in header:
```json

{
    "id": 1,
    "description": "Some desk",
    "reservations": [
        {
            "id": 1,
            "startDate": "2022-07-06T16:37:45.0920894+02:00",
            "endDate": "2022-07-06T17:37:45.0920964+02:00",
            "reservedBy": "John Smith"
        }
    ],
    "isAvailable": true
}
```


## Error Response

**Condition** : When there is no location/deks with provided ID.

**Code** : `404 NOT FOUND`

## Notes

* It does not require `login-token` in header. But when `login-token` is included, the endpoint will return **all** the data  about this desk, including reservations info. Without it though only basic information will be returned.
