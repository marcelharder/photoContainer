namespace photoContainer.data.interfaces;

public interface IImage
{
    Task<PagedList<ImageDto>?> GetFilesForUser(CategoryParams ip);
    Task<ActionResult<CarouselDto>> getCarouselData(int id);
    Task<PagedList<ImageDto>> getImages(ImageParams imgP);
    public Task<PagedList<ImageDto>?> GetImagesByCategory(ImageParams ip);

    Task<int> addImage(ImageDto imdto);
    Task<int> deleteImage(int id);
    //Task SeedImages();
    Task<int> updateImage(ImageDto image);
    Task<ImageDto> findImage(int Id);
    Task<ActionResult<List<ImageDto>>> findImagesByUser(List<int> catP);
    Task<bool> SaveChangesAsync();
   
}
