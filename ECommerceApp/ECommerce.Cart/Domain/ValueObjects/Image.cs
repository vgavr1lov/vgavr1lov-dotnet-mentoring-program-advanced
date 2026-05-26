namespace ECommerce.Cart.Domain.ValueObjects;

public sealed record Image
{
    public string Url { get; }
    public string? AltText { get; }

    public Image(string url, string? altText)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("Image URL is required!");

        if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            throw new ArgumentException("Invalid URL format!");

        Url = url;
        AltText = altText;
    }

}
