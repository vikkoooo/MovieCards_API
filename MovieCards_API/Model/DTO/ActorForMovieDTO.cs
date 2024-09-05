namespace MovieCards_API.Model.DTO
{
	public record ActorForMovieDTO
	{
		public int? Id { get; init; } // if adding by id other params skip
		public string Name { get; init; } = string.Empty; // use this if creating new actor, id nullable
		public DateTime DateOfBirth { get; init; } // use this if creating new actor, id nullable
	}
}
