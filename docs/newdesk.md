# Add new desk to the location

Adds new desk at specific location to the database.

**URL** : `/admin/locations/{locid}/desks` where `{locid}` is the Id of the location we want our desk to be added to.

**Method** : `POST`

**Auth required** : YES

**Data constraints**

```json
{
    "description": "[Description of the desk]",
    "isAvailable": [True or False]
}
```

It is possible to specify `Id` and `Reservations` field but not recommended to and not supported.

## Success Response

**Code** : `201 CREATED`

**Content examples** : Response will reflect back new desk that has been added to database.

```json
{
    "id": 1,
    "description": "Some desk",
    "reservations": [],
    "isAvailable": true
}
```

## Error Response

**Condition** : If there is no `login-token` provided in header.

**Code** : `401 UNAUTHORIZED`

## Notes

* To get this result one must include `login-token` in request header with value that matches one set in main app.  
* There can be many desks with the same description.
* The desks ID are globally unique, which means that there cannot be two (or more) desks with the same ID but in different locations.