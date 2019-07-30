package workflow.capstone.capstoneproject.api;

public class UpdateAvatarModel {
    private String imagePath;

    public UpdateAvatarModel(String imagePath) {
        this.imagePath = imagePath;
    }

    public String getImagePath() {
        return imagePath;
    }

    public void setImagePath(String imagePath) {
        this.imagePath = imagePath;
    }
}
