# Hot Desk minimal API by Seweryn Jarco

Hot desk booking system is a system which should designed to automate the reservation of desk in offices through an easy online booking system.
It is completely written in C# .NET with use of minimal API.

## Requirements

### Administration:

- Manage locations (add/remove, can't remove if desk exists in location).
- Manage desk in locations (add/remove if no pending reservation/make unavailable).
- Administrators can see who reserves a desk in location.

### Employees

- Determine which desks are available to book or unavailable.
- Filter desks based on location.
- Book a desk for the day.
- Allow reserving a desk for multiple days but now more than a week.
- Allow to change desk, but not later than the 24h before reservation.
- Employees can see only that specific desk is unavailable.

## Endpoints

### Admin authorization

To authorize request include "login-token" with value "1234567890" in header of your request.

### Endpoints that require authorization

* [Get all locations](docs/getlocs.md) : `GET /admin/locations`
* [Get all desks](docs/getdesks.md) : `GET /admin/desks`
* [Add new location](docs/newloc.md) : `POST /admin/locations`
* [Add new desk](docs/newdesk.md) : `POST /admin/locations/{locid}/desks`
* [Remove location](docs/remloc.md) : `DELETE /admin/locations/{locid}`
* [Remove desk](docs/remdesk.md) : `DELETE /admin/locations/{locid}/desks/{deskid}`
* [Change desk availability](docs/changeavail.md) : `PUT /admin/locations{locid}/desks/{deskid}`

### Endpoints that doesn't require authorization

* [Get data about specific location](docs/getloc.md) : `GET /locations/{locid}`
* [Get data about specific desk](docs/getdesk.md) : `GET /locations/{locid}/desks/{deskid}`
* [Filter desk in specific location](docs/filterdesk.md) : `GET /locations/{locid}/desks`
* [Check if desk is available](docs/checkdesk.md) : `GET /locations/{locid}/desks/available/{deskid}`
* [Get available desk at location](docs/getavail.md) : `GET /locations/{locid}/desks/available`
* [Make an reservation](docs/book.md) : `POST /locations/{locid}/desks/{deskid}/book`
* [Change a desk](docs/changedesk.md) : `POST /locations/{locid}/reservation/{resid}`

## Implementation

Whole API stores data in InMemory database so all data will be lost after restarting the program. This project contains 6 unit tests to check some functionality of the API.

* When resource was not found the API will usually return NotFound (`404`)
* When there was an conflict (e.g. trying to delete location with desk in it) the API will usually return Conflict (`409`)
* When request was successful the API will return either OK (`200`) or Created (`201`)
* Location can't be removed if there are any desks in it.
* Desk can't be removed if there is an **pending** reservation in it. Pending means that the desk will not be removed if there are any reservations with ending date after the time when user sends request.
* Reservation has a start & end date where end date is after start date (the API will return `409` if user try to do differently). These two dates can be up to 7 days apart. They are stored as DateTime format. If user doesn't specify them in request, the default will be applied: start date is set to the time user send request and end date is set to one hour after.
* There can't be two overlapping reservations at the same desk at the same location - the API will return `409`. At the same desk when trying to make a reservation the dates have to be chosen correctly so two reservations doesn't overlap.
* Some endpoints behave differently when user add `login-token` to the header, e.g. when user try to get data about specific location, without the `login-token` he wouldn't get info about reservations at this desk, but after including it in the header he will. 
