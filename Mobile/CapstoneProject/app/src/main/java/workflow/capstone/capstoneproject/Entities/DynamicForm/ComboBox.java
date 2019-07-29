package workflow.capstone.capstoneproject.entities.DynamicForm;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

import java.util.List;

public class ComboBox {

    @SerializedName("name")
    @Expose
    private String name;

    @SerializedName("value")
    @Expose
    private String value;

    @SerializedName("valueOfProper")
    @Expose
    private List<String> valueOfProper;

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

    public List<String> getValueOfProper() {
        return valueOfProper;
    }

    public void setValueOfProper(List<String> valueOfProper) {
        this.valueOfProper = valueOfProper;
    }
}
