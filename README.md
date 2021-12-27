# Movies Microservice
**Version: 2.0**

## Description

OrleansMovies is a movies indexing application that has high volumes of traffic and thus is fast, efficient and robust while being secure at the same time.

## Features

- **Home**
  - List top 5 highest rated movies

- **Movies List**
  - List Movies
  - Search
  - Filter by Genre

- **Movie detail**
  - Display selected movie detail information

- **Create Movie**
  - Create a new movie that can be retrieved in the movies list

- **Update Movie**
  - Update movies data.  

### Technologies

- [ASP.NET (AspNetCore)](https://dotnet.microsoft.com/apps/aspnet)
- [Microsoft Orleans](https://dotnet.github.io/orleans/)
- [GraphQL](https://github.com/graphql-dotnet/graphql-dotnet)

### Running the application

- Open the Solution
- Make sure the startup project is set to `Movies.Server`
- Run the application by pressing F5 or by pressing the button "Silo Local" in Visual Studio
- You can start querying the API by using the provided postman collection OR by using GraphQL playground available at: http://localhost:6600/ui/playground

### GraphQL queries 

- GraphQL query to get all movies:
```
query
{
  movies {
      id
      key
      name
      description
      img
      length
      rate
  }
}
```

- GraphQL query to get the 5 most rated movies:
```
query
{
  mostratedmovies {
      id
      key
      name
      description
      img
      length
      rate
  }
}
```

- GraphQL query to get movie details:
```
query($id: String)
{
  movie(id: $id) {
    id
    key
    name
    description
    img
    length
    rate
  }
}
```
with query variables 

```
{"id": "1"}
```

- GraphQL query to get movie details:
```
query($query: String)
{
  searchmovie(query: $query) {
    id
    key
    name
    description
    img
    length
    rate
  }
}
```
with query variables 
```
{"query": "gang"}
```

- GraphQL query to get movie by genre ID:
```
query($genreid: String)
{
  getallbygenre(genreid: $genreid) {
    id
    key
    name
    description
    img
    length
    rate
  }
}
```
with query variables 
```
{"genreid": "3"}
```

- GraphQL query to update a movie:
```
mutation ($movie: UpdateMovie!) {
  updateMovie(movie: $movie) {
    id
    key
    name
    description
    img
    length
    rate
  }
}
```
with query variables 
```
{
  "movie": 
  {
  	"id": "1",
    "key": "deadpool2",
    "name": "Deadpool 2",
    "description": "A former Special Forces operative turned mercenary is subjected to a rogue experiment that leaves him with accelerated healing powers, adopting the alter ego Deadpool.",
    "img": "deadpool.jpg",
    "length": "1hr 48mins",
    "rate": 8.6
	}
}
```

- GraphQL query to create a movie:
```
mutation ($movie: InputMovie!) {
  addMovie(movie: $movie) {
    key
    name
    description
    img
    length
    rate
  }
}
```
with query variables 
```
{
  "movie": 
  {
    "key": "fast-and-furious-7",
    "name": "Fast & Furious 7",
    "description": "Deckard Shaw seeks revenge against Dominic Toretto and his family for his comatose brother.",
    "img": "fast-and-furious-7.jpg",
    "length": "2hr 17mins",
    "rate": 7.3
  }
}
```