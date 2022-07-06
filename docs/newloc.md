# Add new location

Adds new location to the database.

**URL** : `/admin/locations`

**Method** : `POST`

**Auth required** : YES

**Data constraints**

```json
{
    "Name": "[Location name]"
}
```

It is possible to specify `Id` and `Desks` field but not recommended to and not supported.

## Success Response

**Code** : `201 CREATED`

**Content examples** : Response will reflect back new location that has been added to database.

```json
{
    "id": 1,
    "name": "Warszawa",
    "desks": []
}
```

## Error Response

**Condition** : If there is no `login-token` provided in header.

**Code** : `401 UNAUTHORIZED`

## Notes

* To get this result one must include `login-token` in request header with value that matches one set in main app.  
* There can be many locations with the same name.