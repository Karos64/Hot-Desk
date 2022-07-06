# Filter only available desks at specific location

Returns all desk that are available to book at location specified with `{locid}`

**URL** : `/locations/{locid}/desks/available`

**Method** : `GET`

**Auth required** : NO

## Success Response

**Code** : `200 OK`

**Content examples** 

```json
[
    {
        "id": 1,
        "description": "Some desk",
        "reservations": [],
        "isAvailable": true
    }
]
```

## Error Response

**Condition** : When there is no location with provided ID.

**Code** : `404 NOT FOUND`