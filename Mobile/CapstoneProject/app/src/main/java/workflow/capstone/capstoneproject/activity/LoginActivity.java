package workflow.capstone.capstoneproject.activity;

import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Build;
import android.os.Bundle;
import android.support.annotation.RequiresApi;
import android.support.v7.app.AppCompatActivity;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;

import com.google.android.gms.tasks.OnCompleteListener;
import com.google.android.gms.tasks.OnSuccessListener;
import com.google.android.gms.tasks.Task;
import com.google.firebase.iid.FirebaseInstanceId;
import com.google.firebase.iid.InstanceIdResult;

import java.util.LinkedHashMap;
import java.util.Map;

import workflow.capstone.capstoneproject.R;
import workflow.capstone.capstoneproject.api.TestLogin;
import workflow.capstone.capstoneproject.customdialog.VerifyAccountDialog;
import workflow.capstone.capstoneproject.entities.Login;
import workflow.capstone.capstoneproject.repository.CapstoneRepository;
import workflow.capstone.capstoneproject.repository.CapstoneRepositoryImpl;
import workflow.capstone.capstoneproject.utils.CallBackData;
import workflow.capstone.capstoneproject.utils.ConstantDataManager;
import workflow.capstone.capstoneproject.utils.DynamicWorkflowSharedPreferences;
import workflow.capstone.capstoneproject.utils.DynamicWorkflowUtils;

public class LoginActivity extends AppCompatActivity {

    private EditText inputEmail;
    private EditText inputPassword;
    private Button btnLogin;
    private CapstoneRepository capstoneRepository;
    private Context context = this;
    private TextView tvErrorLogin;
    private TextView tvForgotPassword;
    private String deviceID;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login);
        initView();
        btnLogin.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                login();
            }
        });
        tvForgotPassword.setOnClickListener(new View.OnClickListener() {
            @RequiresApi(api = Build.VERSION_CODES.LOLLIPOP)
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(LoginActivity.this, SendCodeActivity.class);
                startActivity(intent);
                overridePendingTransition(R.anim.slide_in_right, R.anim.slide_out_left);
            }
        });
    }

    private void initView() {
        inputEmail = findViewById(R.id.input_email);
        inputPassword = findViewById(R.id.input_password);
        btnLogin = findViewById(R.id.btn_login);
        tvErrorLogin = findViewById(R.id.tv_Error_Login);
        tvForgotPassword = findViewById(R.id.tv_forgot_password);
    }

    private void login() {
        final String username = inputEmail.getText().toString();
        String password = inputPassword.getText().toString();

        FirebaseInstanceId.getInstance().getInstanceId()
                .addOnSuccessListener(new OnSuccessListener<InstanceIdResult>() {
                    @Override
                    public void onSuccess(InstanceIdResult instanceIdResult) {

                        // Get new Instance ID token
                        deviceID = instanceIdResult.getToken();
                    }
                });

        if (deviceID == null) {
            deviceID = FirebaseInstanceId.getInstance().getToken();
        }
        TestLogin testLogin = new TestLogin();
        testLogin.setUserName(username);
        testLogin.setPassword(password);
        testLogin.setDeviceID(deviceID);

        SharedPreferences preferences = context.getSharedPreferences("VERIFYACCOUNT", Context.MODE_PRIVATE);
        SharedPreferences.Editor editor = preferences.edit();
        editor.putString("username", username);
        editor.putString("password", password);
        editor.commit();

        Map<String, String> fields = new LinkedHashMap<>();
        fields.put("userName", username);
        fields.put("password", password);

        boolean checkConnection = DynamicWorkflowUtils.isConnectingToInternet(context);
        if (!checkConnection) {
            setTextError(R.string.no_network + "");
        } else if (username.trim().isEmpty() && password.trim().isEmpty()) {
            setTextError("Please input email and password!");
        } else if (username.trim().isEmpty()) {
            setTextError("Please input email!");
        } else if (password.trim().isEmpty()) {
            setTextError("Please input password!");
        } else {
            capstoneRepository = new CapstoneRepositoryImpl();
//            capstoneRepository.login(context, fields, new CallBackData<Login>() {
//                @Override
//                public void onSuccess(Login login) {
//                    DynamicWorkflowSharedPreferences.storeJWT(context, ConstantDataManager.AUTHORIZATION_TOKEN, login.getToken());
//                    Intent intent = new Intent(LoginActivity.this, MainActivity.class);
//                    startActivity(intent);
//                    finish();
//                }
//
//                @RequiresApi(api = Build.VERSION_CODES.LOLLIPOP)
//                @Override
//                public void onFail(String message) {
//                    if (message.contains("Invalid username or password!")) {
//                        setTextError(message);
//                    } else if (message.contains("Please verify your account first")) {
//                        VerifyAccountDialog dialog = new VerifyAccountDialog(LoginActivity.this);
//                        dialog.setCancelable(false);
//                        dialog.create();
//                        dialog.show();
//                    } else {
//                        setTextError(message);
//                    }
//                }
//            });
            capstoneRepository.testLogin(context, testLogin, new CallBackData<Login>() {
                @Override
                public void onSuccess(Login login) {
                    DynamicWorkflowSharedPreferences.storeJWT(context, ConstantDataManager.AUTHORIZATION_TOKEN, login.getToken());
                    Intent intent = new Intent(LoginActivity.this, MainActivity.class);
                    startActivity(intent);
                    finish();
                }

                @RequiresApi(api = Build.VERSION_CODES.LOLLIPOP)
                @Override
                public void onFail(String message) {
                    if (message.contains("Invalid username or password!")) {
                        setTextError(message);
                    } else if (message.contains("Please verify your account first")) {
                        VerifyAccountDialog dialog = new VerifyAccountDialog(LoginActivity.this);
                        dialog.setCancelable(false);
                        dialog.create();
                        dialog.show();
                    } else {
                        setTextError(message);
                    }
                }
            });
        }
    }

    private void setTextError(String message) {
        tvErrorLogin.setVisibility(View.VISIBLE);
        tvErrorLogin.setText(message);
    }

}
