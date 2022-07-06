# Remove location

**URL** : `/admin/locations/{locid}` where `{locid}` is the ID of the location we want to delete.

**Method** : `DELETE`

**Auth required** : YES


## Success Response

**Code** : `200 OK`

**Content examples** : Response will reflect back the location we deleted.

When we delete location with ID = 1 (which happens to be Kraków) we get the following result.
```json
{
    "id": 1,
    "name": "Kraków",
    "desks": []
}
```

## Error Response

**Condition** : If there is no `login-token` provided in header.

**Code** : `401 UNAUTHORIZED`

### Or

**Condition** : If there are >= 1 desks binded to location we try to delete.

**Code** : `409 CONFLICT`

### Or

**Condition** : When there is no location with provided ID.

**Code** : `404 NOT FOUND`

## Notes

* To get this result one must include `login-token` in request header with value that matches one set in main app.  