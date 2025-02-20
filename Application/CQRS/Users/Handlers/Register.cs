using Application.CQRS.Users.ResponseDtos;
using AutoMapper;
using Common.Exceptions;
using Common.GlobalResponses.Generics;
using Common.Security;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Users.Handlers;

public class Register
{
    public class Command : IRequest<Result<RegisterDto>>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
    }

    public sealed class Handler(IUnitOfWork unitOfWork, IMapper mapper, IValidator<Command> validator) : IRequestHandler<Command, Result<RegisterDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IValidator<Command> _validator=validator;

        public async Task<Result<RegisterDto>> Handle(Command request, CancellationToken cancellationToken)
        {

            var validationResult = await _validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var currentUser = await _unitOfWork.UserRepository.GetUserByEmailAsync(request.Email);
            if (currentUser != null) throw new BadRequestException("User is already exist with provided mail");

            var user = _mapper.Map<User>(request);

            var hashPassword = PasswordHasher.ComputeStringToSha256Hash(request.Password);
            user.PasswordHash = hashPassword;
            user.CreatedBy = 1;
            await _unitOfWork.UserRepository.RegisterAsync(user);

            var response = _mapper.Map<RegisterDto>(user);

            return new Result<RegisterDto>
            {
                Data = response,
                Errors = [],
                IsSuccess = true
            };
        }
    }
}