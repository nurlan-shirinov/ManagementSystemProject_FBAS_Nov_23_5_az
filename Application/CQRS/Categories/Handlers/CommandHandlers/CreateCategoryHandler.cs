﻿using Application.CQRS.Categories.Commands.Requests;
using Application.CQRS.Categories.Commands.Responses;
using Application.Security;
using Common.Exceptions;
using Common.GlobalResponses.Generics;
using Domain.Entities;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Categories.Handlers.CommandHandlers;

public class CreateCategoryHandler(IUnitOfWork unitOfWork , IUserContext userContext) : IRequestHandler<CreateCategoryRequest, Result<CreateCategoryResponse>>
{


    public async Task<Result<CreateCategoryResponse>> Handle(CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        Category category = new()
        {
            Name = request.Name
        };

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            //return new Result<CreateCategoryResponse>
            //{
            //    Data = null,
            //    Errors = ["Categry Name bosh olmamalidir"],
            //    IsSuccess = false
            //};

            throw new BadRequestException("Name null ola bilmez");
        }

        category.CreatedBy= userContext.MustGetUserId();

        await unitOfWork.CategoryRepository.AddAsync(category);

        CreateCategoryResponse response  = new()
        {
            Id = category.Id,
            Name = request.Name
        };

        return new Result<CreateCategoryResponse> { Data = response , Errors = [] , IsSuccess=true };

    }
}