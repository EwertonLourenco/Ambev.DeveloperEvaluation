using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Users.CreateUser;

public class CreateUserProfile : Profile
{
    public CreateUserProfile()
    {
        CreateMap<CreateUserCommand, User>()
            .ForMember(d => d.Username, o => o.MapFrom(s => s.Username));

        CreateMap<User, CreateUserResult>()
            .ForMember(d => d.Name, o => o.MapFrom(s => s.Username))
            .ForMember(d => d.Email, o => o.MapFrom(s => s.Email))
            .ForMember(d => d.Phone, o => o.MapFrom(s => s.Phone))
            .ForMember(d => d.Role, o => o.MapFrom(s => s.Role))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status));
    }
}