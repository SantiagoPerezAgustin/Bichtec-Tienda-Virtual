using Application.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace Infrastructure.Services;

public sealed class ImageWebpEncoder : IImageWebpEncoder
{
    private const int DefaultQuality = 80;

    public async Task<byte[]> EncodeToWebpAsync(
        Stream imageStream,
        int quality = DefaultQuality,
        CancellationToken cancellationToken = default,
        int maxEdgePixels = 0)
    {
        ArgumentNullException.ThrowIfNull(imageStream);
        quality = Math.Clamp(quality, 1, 100);
        maxEdgePixels = Math.Clamp(maxEdgePixels, 0, 8192);

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

        if (maxEdgePixels > 0 && (image.Width > maxEdgePixels || image.Height > maxEdgePixels))
        {
            image.Mutate(ctx => ctx.Resize(new ResizeOptions
            {
                Size = new Size(maxEdgePixels, maxEdgePixels),
                Mode = ResizeMode.Max,
            }));
        }

        await using var output = new MemoryStream();
        await image.SaveAsWebpAsync(output, new WebpEncoder { Quality = quality }, cancellationToken)
            .ConfigureAwait(false);

        return output.ToArray();
    }
}
