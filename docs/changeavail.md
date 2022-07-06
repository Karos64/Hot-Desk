# Change desk availability

**URL** : `/admin/locations/{locid}/desks/{deskid}` where `{locid}` is the ID of the location where desk with ID `{deskid}` (which we want to delete) is.

**Method** : `PUT`

**Auth required** : YES

**Data constraints**

```json
{
    "isAvailable": [True or False]
}
```

It is possible to specify `Id`, `Reservations` or `Description` fields but not recommended to and not supported.


## Success Response

**Code** : `200 OK`

**Content examples** : Response will reflect back the desk we changed.

When we change availability of desk with ID = 1 we get the following result.
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

**Condition** : When there is no location/deks with provided ID.

**Code** : `404 NOT FOUND`

## Notes

* To get this result one must include `login-token` in request header with value that matches one set in main app.  