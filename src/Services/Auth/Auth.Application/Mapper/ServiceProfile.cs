using Auth.Application.Auths.Commands.AuthLogin;
using Auth.Application.Auths.Commands.AuthRegister;
using Auth.Application.Auths.Commands.CreateKey;
using Auth.Application.Auths.Commands.RefreshToken;
using Auth.Application.DTOs.Auth.Requests;
using Auth.Application.DTOs.Auth.Responses;
using Auth.Application.DTOs.Key;
using Auth.Application.DTOs.Key.Requests;
using Auth.Application.DTOs.Key.Responses;
using Auth.Domain.Models;
using AutoMapper;

namespace Auth.Application.Mapper;

public class ServiceProfile : Profile
{
    public ServiceProfile()
    {
        #region Authentication
        // User
        CreateMap<ApplicationUser, UserDto>().ReverseMap();
        CreateMap<UserDto, ApplicationUser>().ReverseMap();
        // Register 
        CreateMap<AuthRegisterCommand, RegisterRequestDto>().ReverseMap();
        CreateMap<RegisterRequestDto, AuthRegisterCommand>().ReverseMap();
        CreateMap<LoginResponseDto, AuthRegisterResult>().ReverseMap();
        CreateMap<AuthRegisterResult, LoginResponseDto>().ReverseMap();
        // Login
        CreateMap<AuthLoginCommand, LoginRequestDto>().ReverseMap();
        CreateMap<LoginRequestDto, AuthLoginCommand>().ReverseMap();
        CreateMap<LoginResponseDto, AuthLoginResult>().ReverseMap();
        CreateMap<AuthLoginResult, LoginResponseDto>().ReverseMap();
        #endregion


        #region Keys

        // Key
        CreateMap<Key, KeyDto>().ReverseMap();
        CreateMap<KeyDto, Key>().ReverseMap();
        
        // Create Key
        CreateMap<CreateKeyRequestDto, CreateKeyCommand>().ReverseMap();
        CreateMap<CreateKeyCommand, CreateKeyRequestDto>().ReverseMap();
        
        // Refresh Token Request
        CreateMap<RefreshTokenCommand, RefreshTokenByUserResponseDto>();
        CreateMap<RefreshTokenByUserResponseDto, RefreshTokenCommand>();
        // Refresh Token Response
        CreateMap<RefreshTokenResult, RefreshTokenByUserResponseDto>();
        CreateMap<RefreshTokenByUserResponseDto, RefreshTokenResult>();
        #endregion


    }
}