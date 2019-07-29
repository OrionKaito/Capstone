package workflow.capstone.capstoneproject.entities.DynamicForm;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

import java.util.List;

public class DynamicForm {

    @SerializedName("textOnly")
    @Expose
    private TextOnly textOnly;

    @SerializedName("shortText")
    @Expose
    private ShortText shortText;

    @SerializedName("longText")
    @Expose
    private LongText longText;

    @SerializedName("comboBox")
    @Expose
    private ComboBox comboBox;

    @SerializedName("inputCheckbox")
    @Expose
    private InputCheckbox inputCheckbox;

    public TextOnly getTextOnly() {
        return textOnly;
    }

    public void setTextOnly(TextOnly textOnly) {
        this.textOnly = textOnly;
    }

    public ShortText getShortText() {
        return shortText;
    }

    public void setShortText(ShortText shortText) {
        this.shortText = shortText;
    }

    public LongText getLongText() {
        return longText;
    }

    public void setLongText(LongText longText) {
        this.longText = longText;
    }

    public ComboBox getComboBox() {
        return comboBox;
    }

    public void setComboBox(ComboBox comboBox) {
        this.comboBox = comboBox;
    }

    public InputCheckbox getInputCheckbox() {
        return inputCheckbox;
    }

    public void setInputCheckbox(InputCheckbox inputCheckbox) {
        this.inputCheckbox = inputCheckbox;
    }
}