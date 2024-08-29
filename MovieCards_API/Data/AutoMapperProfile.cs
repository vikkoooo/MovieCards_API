using AutoMapper;
using MovieCards_API.Model.DTO;
using MovieCards_API.Model.Entities;

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
		}
	}
}
