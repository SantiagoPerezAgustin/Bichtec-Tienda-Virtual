using Application.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;

namespace Infrastructure.Services;

public sealed class ImageWebpEncoder : IImageWebpEncoder
{
    private const int DefaultQuality = 80;

    public async Task<byte[]> EncodeToWebpAsync(
        Stream imageStream,
        int quality = DefaultQuality,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(imageStream);
        quality = Math.Clamp(quality, 1, 100);

        Stream input = imageStream;
        if (!input.CanSeek)
        {
            var buffer = new MemoryStream();
            await input.CopyToAsync(buffer, cancellationToken).ConfigureAwait(false);
            buffer.Position = 0;
            input = buffer;
        }
        else if (input.Position != 0)
        {
            input.Position = 0;
        }

        using var image = await Image.LoadAsync(input, cancellationToken).ConfigureAwait(false);

        await using var output = new MemoryStream();
        await image.SaveAsWebpAsync(output, new WebpEncoder { Quality = quality }, cancellationToken)
            .ConfigureAwait(false);

        return output.ToArray();
    }
}
