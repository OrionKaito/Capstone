package workflow.capstone.capstoneproject.entities.DynamicForm;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

import java.util.List;

public class DynamicForm {

    @SerializedName("fields")
    @Expose
    private List<Field> fields = null;
    @SerializedName("name")
    @Expose
    private String name;

    public List<Field> getFields() {
        return fields;
    }

    public void setFields(List<Field> fields) {
        this.fields = fields;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }
}