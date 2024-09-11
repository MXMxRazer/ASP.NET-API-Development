namespace GameStore.API.Dtos;

public record class UpdateGameDTO(
    string Name, 
    string Genre, 
    decimal Price, 
    DateOnly releaseDate 
); 
