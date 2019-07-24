package workflow.capstone.capstoneproject.customdialog;

import android.app.Activity;
import android.app.Dialog;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Build;
import android.os.Bundle;
import android.support.annotation.RequiresApi;
import android.view.View;
import android.view.Window;
import android.widget.Button;
import android.widget.EditText;

import java.util.LinkedHashMap;
import java.util.Map;

import es.dmoral.toasty.Toasty;
import workflow.capstone.capstoneproject.R;
import workflow.capstone.capstoneproject.activity.ConfirmForgotPasswordActivity;
import workflow.capstone.capstoneproject.activity.MainActivity;
import workflow.capstone.capstoneproject.entities.Login;
import workflow.capstone.capstoneproject.repository.CapstoneRepository;
import workflow.capstone.capstoneproject.repository.CapstoneRepositoryImpl;
import workflow.capstone.capstoneproject.utils.CallBackData;
import workflow.capstone.capstoneproject.utils.ConstantDataManager;
import workflow.capstone.capstoneproject.utils.DynamicWorkflowSharedPreferences;
import workflow.capstone.capstoneproject.utils.DynamicWorkflowUtils;

public class VerifyAccountDialog extends Dialog {

    private EditText edtVerifyCode;
    private Button cancelButton;
    private Button confirmButton;
    private Context mContext;
    private CapstoneRepository capstoneRepository;
    private String verifyCode;
    private String username;
    private String password;

    public VerifyAccountDialog(Context context) {
        super(context);
        mContext = context;
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        requestWindowFeature(Window.FEATURE_NO_TITLE);
        setContentView(R.layout.verify_account_dialog);

        edtVerifyCode = findViewById(R.id.edt_verify_code);

        cancelButton = findViewById(R.id.cancel_button);
        cancelButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                dismiss();
            }
        });

        confirmButton = findViewById(R.id.confirm_button);
        confirmButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                verifyCode = edtVerifyCode.getText().toString();

                boolean checkConnection = DynamicWorkflowUtils.isConnectingToInternet(mContext);

                if (!checkConnection) {
                    configCustomDialog("Connection", mContext.getResources().getString(R.string.no_network), false);
                } else if (verifyCode.isEmpty()) {
                    configCustomDialog("Input Verify Code", mContext.getResources().getString(R.string.input_all_fields), false);
                } else {
                    SharedPreferences preferences = mContext.getSharedPreferences("VERIFYACCOUNT", Context.MODE_PRIVATE);
                    username = preferences.getString("username", "");
                    password = preferences.getString("password", "");

                    capstoneRepository = new CapstoneRepositoryImpl();
                    capstoneRepository.verifyAccount(mContext, verifyCode, username, new CallBackData<String>() {
                        @Override
                        public void onSuccess(String s) {
                            dismiss();
                            configCustomDialog(mContext.getResources().getString(R.string.title_verify_success), mContext.getResources().getString(R.string.message_verify_success), true);
                        }

                        @Override
                        public void onFail(String message) {
                            dismiss();
                            configCustomDialog(mContext.getResources().getString(R.string.title_verify_fail), message, false);
                        }
                    });
                }
            }
        });
    }

    private void configCustomDialog(String title, String message, final boolean check) {
        new CustomDialog(mContext)
                .setTitleText(title)
                .setContentText(message)
                .setConfirmText("OK")
                .setConfirmClickListener(new CustomDialog.OnCustomClickListener() {
                    @Override
                    public void onClick(CustomDialog customDialog) {
                        if (check) {
                            customDialog.dismiss();
                            Map<String, String> fields = new LinkedHashMap<>();
                            fields.put("userName", username);
                            fields.put("password", password);
                            capstoneRepository = new CapstoneRepositoryImpl();
                            capstoneRepository.login(mContext, fields, new CallBackData<Login>() {
                                @Override
                                public void onSuccess(Login login) {
                                    DynamicWorkflowSharedPreferences.storeJWT(mContext, ConstantDataManager.AUTHORIZATION_TOKEN, login.getToken());
                                    Intent intent = new Intent(mContext, MainActivity.class);
                                    mContext.startActivity(intent);
                                    ((Activity) mContext).finish();
                                }

                                @RequiresApi(api = Build.VERSION_CODES.LOLLIPOP)
                                @Override
                                public void onFail(String message) {
                                    Toasty.error(mContext, "Fail login in verify account dialog", Toasty.LENGTH_SHORT).show();
                                }
                            });
                        } else {
                            customDialog.dismiss();
                        }
                    }
                })
                .show();
    }
}
