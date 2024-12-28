﻿namespace Upload.API.Services.S3Upload;

public interface IS3UploadService
{
    Task<string> S3UploadFileAsync(Stream fileStream, CancellationToken cancellationToken = default!);
    Task<string> S3UploadGenerateCloudFrontAsync(Stream fileStream, CancellationToken cancellationToken = default!);
    
}