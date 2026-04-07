namespace photoContainer.data.dtos;

public class CarouselDto
{
    public int? numberOfImages { get; set; }
    public int? category { get; set; }
    public bool ShowR { get; set; }
    public bool ShowL { get; set; }
    public int? nextImageIdR { get; set; }
    public int? nextImageIdL { get; set; }
}