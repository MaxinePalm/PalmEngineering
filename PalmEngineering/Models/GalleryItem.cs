namespace PalmEngineering.Models
{
    public class GalleryItem
    {
        public string Slug { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Alt { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public int Width { get; set; }
        public int Height { get; set; }
        public string BlurHash { get; set; } = string.Empty;
    }
}
