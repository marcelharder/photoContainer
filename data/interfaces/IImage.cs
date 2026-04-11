namespace photoContainer.data.interfaces;

public interface IImage
{

    Task<ActionResult<CarouselDto>> getCarouselData(int id);
    Task<ImageDto[]> getAllImages();
    Task<ImageDto[]?> GetImagesByCategory(ImageParams ip);
    Task<ImageDto?> getMainImageOfCategory(int categoryId);
    Task<ImageDto[]> findImagesByUser(CategoryParams cp);


    Task<int> createImage(ImageDto imdto);
    Task<ImageDto> ReadImage(int Id);
    Task<int> updateImage(ImageDto image);
    Task<int> deleteImage(int id);

    Task<bool> SaveChangesAsync();

}
