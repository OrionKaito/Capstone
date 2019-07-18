package workflow.capstone.capstoneproject.entities;

public class Comment {

    private String comment;
    private String fullName;

    public Comment(String comment, String fullName) {
        this.comment = comment;
        this.fullName = fullName;
    }

    public String getComment() {
        return comment;
    }

    public void setComment(String comment) {
        this.comment = comment;
    }

    public String getFullName() {
        return fullName;
    }

    public void setFullName(String fullName) {
        this.fullName = fullName;
    }
}
