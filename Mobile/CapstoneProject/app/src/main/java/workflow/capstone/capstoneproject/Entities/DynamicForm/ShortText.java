package workflow.capstone.capstoneproject.entities.DynamicForm;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class ShortText {

    @SerializedName("name")
    @Expose
    private String name;

    @SerializedName("value")
    @Expose
    private String value;

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public String getValue() {
        return value;
    }

    public void setValue(String value) {
        this.value = value;
    }

}
