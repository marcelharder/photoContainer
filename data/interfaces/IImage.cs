namespace photoContainer.data.interfaces;

using photoContainer.data.models;

public interface IImage
{

    Task<CarouselDto> getCarouselData(int id);
    Task<ImageDto[]> getAllImages();
    Task<ImageDto[]?> GetImagesByCategory(ImageParams ip);
    Task<ImageDto[]?> findImagesByUser(CategoryParams ip);
    Task<int> createImage(ImageDto imdto);
    Task<ImageDto> ReadImage(int Id);
    Task<int> updateImage(ImageDto image);
    Task<int> deleteImage(int id);
    Task<bool> SaveChangesAsync();

}
