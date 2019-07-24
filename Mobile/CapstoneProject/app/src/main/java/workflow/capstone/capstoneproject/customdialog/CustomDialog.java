package workflow.capstone.capstoneproject.customdialog;

import android.app.Dialog;
import android.content.Context;
import android.os.Bundle;
import android.view.View;
import android.view.Window;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.TextView;

import workflow.capstone.capstoneproject.R;
import workflow.capstone.capstoneproject.repository.CapstoneRepository;

public class CustomDialog extends Dialog implements View.OnClickListener {

    private Context mContext;
    private TextView titleTextView;
    private TextView contentTextView;
    private Button cancelButton;
    private Button confirmButton;
    private ImageView imgSuccess;
    private ImageView imgFail;
    private CustomDialog.OnCustomClickListener mCancelClickListener;
    private CustomDialog.OnCustomClickListener mConfirmClickListener;
    private String mTitleText;
    private String mContentText;
    private String mCancelText;
    private String mConfirmText;
    private boolean mShowCancel;
    private boolean mSuccess;
    private boolean mFail;

    public CustomDialog(Context context) {
        super(context);
        mContext = context;
        this.setCancelable(true);
        this.setCanceledOnTouchOutside(false);
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        requestWindowFeature(Window.FEATURE_NO_TITLE);
        setContentView(R.layout.custom_dialog);
        initView();
    }

    private void initView() {
        this.titleTextView = findViewById(R.id.title_text);
        this.contentTextView = findViewById(R.id.content_text);
        this.cancelButton = findViewById(R.id.cancel_button);
        this.confirmButton = findViewById(R.id.confirm_button);
        this.imgSuccess = findViewById(R.id.img_success);
        this.imgFail = findViewById(R.id.img_fail);
        this.confirmButton.setOnClickListener(this);
        this.cancelButton.setOnClickListener(this);
        this.setImageSuccess(mSuccess);
        this.setImageFail(mFail);
        this.setTitleText(this.mTitleText);
        this.setContentText(this.mContentText);
        this.setCancelText(this.mCancelText);
        this.setConfirmText(this.mConfirmText);
    }

    public CustomDialog setCancelClickListener(CustomDialog.OnCustomClickListener listener) {
        this.mCancelClickListener = listener;
        return this;
    }

    public CustomDialog setConfirmClickListener(CustomDialog.OnCustomClickListener listener) {
        this.mConfirmClickListener = listener;
        return this;
    }

    public void onClick(View v) {
        if (v.getId() == R.id.cancel_button) {
            if (this.mCancelClickListener != null) {
                this.mCancelClickListener.onClick(this);
            } else {
                this.dismiss();
            }
        } else if (v.getId() == R.id.confirm_button) {
            if (this.mConfirmClickListener != null) {
                this.mConfirmClickListener.onClick(this);
            } else {
                this.dismiss();
            }
        }

    }

    public CustomDialog setImageSuccess(boolean success) {
        mSuccess = success;
        if (this.imgSuccess != null && mSuccess) {
            this.imgSuccess.setVisibility(View.VISIBLE);
        }

        return this;
    }

    public CustomDialog setImageFail(boolean success) {
        this.mFail = success;
        if (this.imgFail != null && this.mFail) {
            this.imgFail.setVisibility(View.VISIBLE);
        }

        return this;
    }

    public String getTitleText() {
        return this.mTitleText;
    }

    public CustomDialog setTitleText(String text) {
        this.mTitleText = text;
        if (this.titleTextView != null && this.mTitleText != null) {
            this.titleTextView.setVisibility(View.VISIBLE);
            this.titleTextView.setText(this.mTitleText);
        }

        return this;
    }

    public String getContentText() {
        return this.mContentText;
    }

    public CustomDialog setContentText(String text) {
        this.mContentText = text;
        if (this.contentTextView != null && this.mContentText != null) {
            this.contentTextView.setVisibility(View.VISIBLE);
            this.contentTextView.setText(this.mContentText);
        }

        return this;
    }

    public String getCancelText() {
        return this.mCancelText;
    }

    public CustomDialog setCancelText(String text) {
        this.mCancelText = text;
        if (this.cancelButton != null && this.mCancelText != null) {
            this.showCancelButton(true);
            this.cancelButton.setText(this.mCancelText);
        }

        return this;
    }

    public String getConfirmText() {
        return this.mConfirmText;
    }

    public CustomDialog setConfirmText(String text) {
        this.mConfirmText = text;
        if (this.confirmButton != null && this.mConfirmText != null) {
            this.confirmButton.setText(this.mConfirmText);
        }

        return this;
    }

    public boolean isShowCancelButton() {
        return this.mShowCancel;
    }

    public CustomDialog showCancelButton(boolean isShow) {
        this.mShowCancel = isShow;
        if (this.cancelButton != null) {
            this.cancelButton.setVisibility(this.mShowCancel ? View.VISIBLE : View.GONE);
        }

        return this;
    }

    public interface OnCustomClickListener {
        void onClick(CustomDialog customDialog);
    }
}
