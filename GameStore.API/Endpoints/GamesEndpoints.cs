using System;
using GameStore.API.Dtos;

namespace GameStore.API.Endpoints;

public static class GamesEndpoints
{
    const string getGameEndpointName = "getGame"; 

    private static readonly List<GameDTO> games = [
        new GameDTO(1, "The Witcher 3: Wild Hunt", "RPG", 39.99m, new DateOnly(2015, 5, 19)),
        new GameDTO(2, "Cyberpunk 2077", "Action RPG", 59.99m, new DateOnly(2020, 12, 10)),
        new GameDTO(3, "Minecraft", "Sandbox", 29.99m, new DateOnly(2011, 11, 18)),
        new GameDTO(4, "God of War", "Action", 49.99m, new DateOnly(2018, 4, 20)),
        new GameDTO(5, "Red Dead Redemption 2", "Adventure", 59.99m, new DateOnly(2018, 10, 26)),
        new GameDTO(6, "Hades", "Rogue-like", 24.99m, new DateOnly(2020, 9, 17)),
        new GameDTO(7, "Valorant", "First-Person Shooter", 0.00m, new DateOnly(2020, 6, 2)),
        new GameDTO(8, "Elden Ring", "Action RPG", 59.99m, new DateOnly(2022, 2, 25)),
        new GameDTO(9, "Stardew Valley", "Simulation", 14.99m, new DateOnly(2016, 2, 26)),
        new GameDTO(10, "Fortnite", "Battle Royale", 0.00m, new DateOnly(2017, 7, 21))
    ];

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app) 
    {

        var gamesGroup = app.MapGroup("games")
                            .WithParameterValidation();  

        // Get all games
        gamesGroup.MapGet("/", () => games); 

        // Get games with id
        gamesGroup.MapGet("/{id}", (int id) => {
            
            GameDTO? game = games.Find(game => game.Id == id); 

            return game is null ? Results.NotFound() : Results.Ok(game); 
        })
        .WithName(getGameEndpointName);  

        // Create games
        gamesGroup.MapPost("/", (CreateGameDTO newGame) => {
            GameDTO game = new (
                games.Count + 1, 
                newGame.Name, 
                newGame.Genre, 
                newGame.Price, 
                newGame.ReleaseDate
            ); 

            games.Add(game); 

            return Results.CreatedAtRoute(getGameEndpointName, new {id = game.Id}, game); 
        });

        // Update games
        gamesGroup.MapPut("/{id}", (int id, UpdateGameDTO updatedGame) => {
            var index = games.FindIndex(game => game.Id == id); 

            if (index == -1) 
            {
                return Results.NotFound(); 
            }

            games[index] = new GameDTO(
                id, 
                updatedGame.Name, 
                updatedGame.Genre, 
                updatedGame.Price, 
                updatedGame.releaseDate
            );

            return Results.NoContent();  
        }); 

        // Delete games
        gamesGroup.MapDelete("/{id}", (int id) => {
            games.RemoveAll(game => game.Id == id); 

            return Results.NoContent(); 
        });

        app.MapGet("/", () => "Hello World!");

        return gamesGroup; 
    }
}
