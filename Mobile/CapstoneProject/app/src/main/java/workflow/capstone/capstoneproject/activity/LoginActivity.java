package workflow.capstone.capstoneproject.activity;

import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.AppCompatButton;
import android.view.View;
import android.widget.EditText;
import android.widget.TextView;

import java.util.LinkedHashMap;
import java.util.Map;

import workflow.capstone.capstoneproject.R;
import workflow.capstone.capstoneproject.entities.Login;
import workflow.capstone.capstoneproject.repository.CapstoneRepository;
import workflow.capstone.capstoneproject.repository.CapstoneRepositoryImpl;
import workflow.capstone.capstoneproject.utils.CallBackData;
import workflow.capstone.capstoneproject.utils.ConstantDataManager;
import workflow.capstone.capstoneproject.utils.DynamicWorkflowSharedPreferences;

public class LoginActivity extends AppCompatActivity {

    private EditText inputEmail;
    private EditText inputPassword;
    private AppCompatButton btnLogin;
    private CapstoneRepository capstoneRepository;
    private Context context = this;
    private TextView tvErrorLogin;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login);
        getWindow().setBackgroundDrawableResource(R.drawable.blue_background);
        initView();
        btnLogin.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                login();
            }
        });
    }

    private void initView() {
        inputEmail = findViewById(R.id.input_email);
        inputPassword = findViewById(R.id.input_password);
        btnLogin = findViewById(R.id.btn_login);
        tvErrorLogin = findViewById(R.id.tv_Error_Login);
    }

    private void login() {
        String username = inputEmail.getText().toString();
        String password = inputPassword.getText().toString();
        Map<String, String> fields = new LinkedHashMap<>();
        fields.put("userName", username);
        fields.put("password", password);
        capstoneRepository = new CapstoneRepositoryImpl();
        capstoneRepository.login(context, fields, new CallBackData<Login>() {
            @Override
            public void onSuccess(Login login) {
                DynamicWorkflowSharedPreferences.storeJWT(context, ConstantDataManager.AUTHORIZATION_TOKEN, login.getToken());
                Intent intent = new Intent(LoginActivity.this, MainActivity.class);
                startActivity(intent);
                finish();
            }

            @Override
            public void onFail(String message) {
                tvErrorLogin.setVisibility(View.VISIBLE);
                tvErrorLogin.setText(message);
            }
        });
    }
}
