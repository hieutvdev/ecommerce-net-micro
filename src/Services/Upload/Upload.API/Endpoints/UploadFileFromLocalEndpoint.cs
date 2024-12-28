using BuildingBlocks.Exceptions;
using Carter;
using MediatR;
using Upload.API.Dtos.Cloudinary.Requests;
using Upload.API.UseCase.V1.Commands.DeleteFileCloudinary;
using Upload.API.UseCase.V1.Commands.UploadFileFromCloudinary;
using Upload.API.UseCase.V1.Commands.UploadFileFromLocal;
using Upload.API.UseCase.V1.Commands.UploadFileS3;
using Upload.API.UseCase.V1.Commands.UploadMultipleFileCloudinary;


namespace Upload.API.Endpoints;

public class UploadFileFromLocalEndpoint : ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/upload";
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.NewVersionedApi("upload")
            .MapGroup(BaseUrl).HasApiVersion(1);
        
        group.MapPost("/uploadfilefromlocal", async (HttpContext context, ISender sender) =>
        {
            var formCollection = await context.Request.ReadFormAsync();
            var file = formCollection.Files.GetFile("file");

            if (file == null)
                return Results.BadRequest("File is missing");
            var result = await sender.Send(new UploadFileFromLocalCommand(context));
            return Results.Ok(result);
        });
        
        group.MapPost("/upload_file_from_cloudinary", async (HttpRequest request, ISender sender) =>
        {
            if (!request.HasFormContentType || request.Form.Files.Count == 0)
            {
                return Results.BadRequest("No file uploaded.");
            }

            var file = request.Form.Files[0];
            await using var stream = file.OpenReadStream();
    
            var result = await sender.Send(new UploadFileFromCloudinaryCommand(stream));
            return Results.Ok(result);
        });
        
        group.MapPost("/upload_multiple_file_from_cloudinary", async (HttpContext context, ISender sender) =>
        {
            var fromCollection = await context.Request.ReadFormAsync();
            var files = fromCollection.Files.GetFiles("files");
            if (!files.Any())
                throw new BadRequestException("File is missing");
            var result = await sender.Send(new UploadMultipleFileCloudinaryCommand(files));
            return Results.Ok(result);
        });
        
        
        group.MapPost("/delete_file_from_cloudinary", async (DeleteFileCloudinaryRequestDto request, ISender sender) =>
        {
            var result = await sender.Send(new DeleteFileCloudinaryCommand(request.Url));
            return Results.Ok(result);
        });

        group.MapPost("/upload_file_s3", async (HttpRequest request, ISender sender) =>
        {
            var file = request.Form.Files[0];
            await using var stream = file.OpenReadStream();
            var result = await sender.Send(new UploadFileS3Command(stream));
            return Results.Ok(result);
        });


    }
}