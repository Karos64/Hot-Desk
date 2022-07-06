# Make an reservation

Make a new reservation at specific desk `{deskid}` in specific location `{locid}`

**URL** : `/locations/{locid}/desks/{deskid}/book`

**Method** : `POST`

**Auth required** : NO

**Data constraints**

```json
{
    "startDate": "2022-07-06T16:37:45.0920894+02:00",
    "endDate": "2022-07-06T17:37:45.0920964+02:00",
    "reservedBy": "[Name & Surname]"
}
```

## Success Response

**Code** : `200 OK`

**Content examples** : Response will reflect back new reservation that has been added to database.

```json
{
    "id": 1,
    "startDate": "2022-07-06T16:37:45.0920894+02:00",
    "endDate": "2022-07-06T17:37:45.0920964+02:00",
    "reservedBy": "John Smith"
}
```

## Error Response

**Condition** : When there is no location/desk with provided ID.

**Code** : `404 NOT FOUND`

### Or

**Condition** : Reservation date is longer than 7 days, picked desk isn't available or there are >=1 reservation overlapping the one we try to make.

**Code** : `409 CONFLICT`