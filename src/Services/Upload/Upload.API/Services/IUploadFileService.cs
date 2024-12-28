using Upload.API.Dtos;

namespace Upload.API.Services;

public interface IUploadFileService
{
    Task<FileResponseDto> UploadFileAsync(IFormFile file, CancellationToken cancellationToken = default!);
    Task<IEnumerable<FileResponseDto>> UploadMultipleFileAsync(IReadOnlyList<IFormFile> files, CancellationToken cancellationToken = default!);
    bool DeleteMultipleFile(IEnumerable<string> fileUrls);
}