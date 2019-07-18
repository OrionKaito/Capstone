package workflow.capstone.capstoneproject.activity;

import android.content.Intent;
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

public class SendCodeActivity extends AppCompatActivity {

    private EditText edtEmail;
    private ImageView imgBack;
    private Button btnSendEmail;
    private CapstoneRepository capstoneRepository;
    private String email;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_send_code);
        initView();
        imgBack.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                onBackPressed();
            }
        });

        btnSendEmail.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                email = edtEmail.getText().toString();
                boolean checkConnection = DynamicWorkflowUtils.isConnectingToInternet(SendCodeActivity.this);
                if (!checkConnection) {
                    configDialog(false, true, getResources().getString(R.string.no_network), false, getResources().getString(R.string.close));
                } else if (email.trim().isEmpty()) {
                    configDialog(false, true,"Please input email!", false, getResources().getString(R.string.close));
                } else {
                    capstoneRepository = new CapstoneRepositoryImpl();
                    capstoneRepository.forgotPassword(SendCodeActivity.this, email, new CallBackData<String>() {
                        @Override
                        public void onSuccess(String s) {
                            configDialog(true, false, getResources().getString(R.string.send_code_success), true, getResources().getString(R.string.continue_step));
                        }

                        @Override
                        public void onFail(String message) {
                            configDialog(false, true, getResources().getString(R.string.send_code_fail), false, getResources().getString(R.string.close));
                        }
                    });
                }
            }
        });
    }

    private void initView() {
        edtEmail = findViewById(R.id.edt_email);
        imgBack = findViewById(R.id.img_Back);
        btnSendEmail = findViewById(R.id.btn_send_email);
    }

    private void configDialog(boolean success, boolean fail, String message, final boolean check, String nextStep) {
        new CustomDialog(SendCodeActivity.this)
                .setContentText(message)
                .setImageSuccess(success)
                .setImageFail(fail)
                .setConfirmText(nextStep)
                .setConfirmClickListener(new CustomDialog.OnCustomClickListener() {
                    @Override
                    public void onClick(CustomDialog customDialog) {
                        if (check) {
                            customDialog.dismiss();
                            Intent intent = new Intent(SendCodeActivity.this, ConfirmForgotPasswordActivity.class);
                            intent.putExtra("email", email);
                            startActivity(intent);
                            overridePendingTransition(R.anim.slide_in_right,R.anim.slide_out_left);
                            finish();
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
