# Remove desk at specific location

**URL** : `/admin/locations/{locid}/desks/{deskid}` where `{locid}` is the ID of the location where desk with ID `{deskid}` (which we want to delete) is.

**Method** : `DELETE`

**Auth required** : YES


## Success Response

**Code** : `200 OK`

**Content examples** : Response will reflect back the desk we deleted.

When we delete desk with ID = 1 we get the following result.
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

### Or

**Condition** : If there are >= 1 pending (not yet finished) reservations binded to desk we try to delete.

**Code** : `409 CONFLICT`

### Or

**Condition** : When there is no location/desk with provided ID.

**Code** : `404 NOT FOUND`

## Notes

* To get this result one must include `login-token` in request header with value that matches one set in main app.  