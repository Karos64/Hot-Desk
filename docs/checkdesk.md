# Checks availability of desk

Checks wether the desk specified with `{deskid}` at location specified with `{locid}` is available to book or not.

**URL** : `/locations/{locid}/desks/available/{deskid}`

**Method** : `GET`

**Auth required** : NO

## Success Response

**Code** : `200 OK`

**Content examples** : When desk is available to book

```json
true
```

**Content examples** : When desk isn't available to book

```json
false
```


## Error Response

**Condition** : When there is no location/deks with provided ID.

**Code** : `404 NOT FOUND`