using AutoMapper;
using MovieCards.Domain.Entities;
using MovieCards_API.Model.DTO;

namespace MovieCards_API.Data
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<Movie, MovieDTO>()
				.ForMember(dest => dest.DirectorName, opt => opt.MapFrom(src => src.Director.Name))
				.ForMember(dest => dest.ActorNames, opt => opt.MapFrom(src => src.Actors.Select(a => a.Name)))
				.ForMember(dest => dest.GenreNames, opt => opt.MapFrom(src => src.Genres.Select(g => g.Name)));

			CreateMap<MovieForCreationDTO, Movie>();

			CreateMap<MovieForUpdateDTO, Movie>();

			CreateMap<Movie, MovieForPatchDTO>().ReverseMap();

			CreateMap<Movie, MovieDetailsDTO>()
				.ForMember(dest => dest.DirectorName, opt => opt.MapFrom(src => src.Director.Name))
				.ForMember(dest => dest.DirectorDateOfBirth, opt => opt.MapFrom(src => src.Director.DateOfBirth))
				.ForMember(dest => dest.DirectorEmail, opt => opt.MapFrom(src => src.Director.ContactInformation.Email))
				.ForMember(dest => dest.DirectorPhoneNumber, opt => opt.MapFrom(src => src.Director.ContactInformation.PhoneNumber))
				.ForMember(dest => dest.ActorNames, opt => opt.MapFrom(src => src.Actors.Select(a => a.Name)))
				.ForMember(dest => dest.GenreNames, opt => opt.MapFrom(src => src.Genres.Select(g => g.Name)));

			CreateMap<Director, DirectorDTO>();
			CreateMap<Actor, ActorDTO>();
			CreateMap<Genre, GenreDTO>();
		}
	}
}
