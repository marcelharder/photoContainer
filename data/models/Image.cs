namespace photoContainer.data.models;

public class Image
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public required string ImageUrl { get; set; }
    public required int YearTaken { get; set; }
    public required string Location { get; set; }
    public string? Familie { get; set; }
    public int Category { get; set; }
    public string? Quality { get; set; }
    public string? Series { get; set; }
    public string? Spare1 { get; set; }
    public string? Spare2 { get; set; }
    public string? Spare3 { get; set; }

}