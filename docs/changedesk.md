# Change a desk in reservation

Changes a desk reserved in reservation specified by its ID `{resid}` in location specified by `{locid}`

**URL** : `/locations/{locid}/reservation/{resid}`

**Method** : `POST`

**Auth required** : NO

**Data constraints**

```json
{
    "newdeskid": [Positive integer]
}
```

## Success Response

**Code** : `200 OK`

**Content examples** : Response will reflect back changed reservation.

```json
{
    "id": 1,
    "startDate": "2022-07-06T16:37:45.0920894+02:00",
    "endDate": "2022-07-06T17:37:45.0920964+02:00",
    "reservedBy": "John Smith"
}
```

## Error Response

**Condition** : When there is no location/desk with provided ID. Or new desk ID is invalid.

**Code** : `404 NOT FOUND`

### Or

**Condition** : Reservation date is longer than 7 days, picked desk isn't available or there are >=1 reservation (at new desk) overlapping the one we try to make.

**Code** : `409 CONFLICT`