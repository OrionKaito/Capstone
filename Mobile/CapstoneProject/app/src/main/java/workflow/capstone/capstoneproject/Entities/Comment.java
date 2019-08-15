package workflow.capstone.capstoneproject.entities;

public class Comment {

    private String comment;
    private String fullName;
    private String date;

    public Comment(String comment, String fullName, String date) {
        this.comment = comment;
        this.fullName = fullName;
        this.date = date;
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

    public String getDate() {
        return date;
    }

    public void setDate(String date) {
        this.date = date;
    }
}
