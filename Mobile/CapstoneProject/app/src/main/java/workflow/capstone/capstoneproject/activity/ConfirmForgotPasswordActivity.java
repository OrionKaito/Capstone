package workflow.capstone.capstoneproject.activity;

import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;

import workflow.capstone.capstoneproject.R;
import workflow.capstone.capstoneproject.customdialog.CustomDialog;
import workflow.capstone.capstoneproject.repository.CapstoneRepository;
import workflow.capstone.capstoneproject.repository.CapstoneRepositoryImpl;
import workflow.capstone.capstoneproject.utils.CallBackData;
import workflow.capstone.capstoneproject.utils.DynamicWorkflowUtils;

public class ConfirmForgotPasswordActivity extends AppCompatActivity {

    private EditText edtCode;
    private EditText edtPassword;
    private EditText edtConfirmPassword;
    private Button btnResetPassword;
    private ImageView imgBack;
    private CapstoneRepository capstoneRepository;
    private String code;
    private String password;
    private String confirmPassword;
    private String email;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_confirm_forgot_password);
        initView();

        imgBack.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                new CustomDialog(ConfirmForgotPasswordActivity.this)
                        .setContentText(getResources().getString(R.string.cancel_reset_password))
                        .setConfirmText("Ok")
                        .setConfirmClickListener(new CustomDialog.OnCustomClickListener() {
                            @Override
                            public void onClick(CustomDialog customDialog) {
                                customDialog.dismiss();
                                onBackPressed();
                            }
                        })
                        .setCancelText("Cancel")
                        .setCancelClickListener(new CustomDialog.OnCustomClickListener() {
                            @Override
                            public void onClick(CustomDialog customDialog) {
                                customDialog.dismiss();
                            }
                        })
                        .show();
            }
        });

        btnResetPassword.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                validateData();
            }
        });
    }

    private void initView() {
        edtCode = findViewById(R.id.edt_code);
        edtPassword = findViewById(R.id.edt_password);
        edtConfirmPassword = findViewById(R.id.edt_confirm_password);
        btnResetPassword = findViewById(R.id.btn_reset_password);
        imgBack = findViewById(R.id.img_Back);
    }

    private void validateData() {
        code = edtCode.getText().toString().trim();
        password = edtPassword.getText().toString();
        confirmPassword = edtConfirmPassword.getText().toString();
        email = getIntent().getStringExtra("email");

        boolean checkConnection = DynamicWorkflowUtils.isConnectingToInternet(ConfirmForgotPasswordActivity.this);

        if (!checkConnection) {
            configDialog(false, true, getResources().getString(R.string.no_network), false, getResources().getString(R.string.close));
        } else if (code.isEmpty() || password.isEmpty() || confirmPassword.isEmpty()) {
            configDialog(false, true, getResources().getString(R.string.input_all_fields), false, getResources().getString(R.string.close));
        } else if (!password.equals(confirmPassword)) {
            configDialog(false, true, getResources().getString(R.string.not_match_password), false, getResources().getString(R.string.close));
        } else {
            capstoneRepository = new CapstoneRepositoryImpl();
            capstoneRepository.confirmForgotPassword(ConfirmForgotPasswordActivity.this, code, email, password, new CallBackData<String>() {
                @Override
                public void onSuccess(String s) {
                    configDialog(true, false, getResources().getString(R.string.change_password_success), true, getResources().getString(R.string.ok));
                }

                @Override
                public void onFail(String message) {
                    configDialog(false, true, message, false, getResources().getString(R.string.close));
                }
            });
        }
    }

    private void configDialog(boolean success, boolean fail, String message, final boolean check, String nextStep) {
        new CustomDialog(ConfirmForgotPasswordActivity.this)
                .setImageSuccess(success)
                .setImageFail(fail)
                .setContentText(message)
                .setConfirmText(nextStep)
                .setConfirmClickListener(new CustomDialog.OnCustomClickListener() {
                    @Override
                    public void onClick(CustomDialog customDialog) {
                        if (check) {
                            customDialog.dismiss();
                            onBackPressed();
                        } else {
                            customDialog.dismiss();
                        }
                    }
                })
                .show();
    }

    @Override
    public void onBackPressed() {
        super.onBackPressed();
        overridePendingTransition(R.anim.slide_in_left, R.anim.slide_out_right);
    }
}
