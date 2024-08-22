Movie Cards API

The primary objective is to build a robust .NET API using EF Core to
manage movies, directors, actors, and genres, with a strict focus on
exposing data only through DTOs.

Required Entities: **Movie**:

> Id, Title, Rating, ReleaseDate, Description
>
> **Relationships**:
>
> o
>
> o
>
> o

**Director**:

One-to-Many with Director.

Many-to-Many with Actor.

Many-to-Many with Genre.

> Id, Name, DateOfBirth
>
> • **Relationship**:
>
> o One-to-One with ContactInformation.

**Actor**:

> Id, Name, DateOfBirth

**Genre**:

> Id, Name

**ContactInformation**:

> Id, Email, PhoneNumber

These endpoints are a minimum of what you should implement **Movie**:

> • GET /api/movies: Return a list of MovieDTO.
>
> • GET /api/movies/{id}: Return a single MovieDTO by ID.
>
> • POST /api/movies: Create a new movie using a MovieForCreationDTO.
>
> • PUT /api/movies/{id}: Update an existing movie with a
> MovieForUpdateDTO.
>
> • DELETE /api/movies/{id}: Delete a movie by ID

**Get** **Detailed** **Movie** **Information**:

> • GET /api/movies/{id}/details: Return a MovieDetailsDTO that combines
> data from Movie, Director, ContactInformation, Genre, and Actor.

Validation and setup

All tables should be seeded with random testdata.

Add Validation Creating and Update an Movie.

API Should return correct status codes!

Extra

Integrate the new MovieDetails Endpoint to existing movie card exercise.
Just for display from API.

Implement more endpoints for more of the other entities.

Implement add new Movie from React app.
