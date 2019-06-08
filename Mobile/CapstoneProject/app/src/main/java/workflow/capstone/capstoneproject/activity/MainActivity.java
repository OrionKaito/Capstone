package workflow.capstone.capstoneproject.activity;

import android.content.Context;
import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;

import workflow.capstone.capstoneproject.Entities.Profile;
import workflow.capstone.capstoneproject.R;
import workflow.capstone.capstoneproject.Repository.CapstoneRepository;
import workflow.capstone.capstoneproject.Repository.CapstoneRepositoryImpl;
import workflow.capstone.capstoneproject.utils.CallBackData;
import workflow.capstone.capstoneproject.utils.ConstantDataManager;
import workflow.capstone.capstoneproject.utils.DynamicWorkflowSharedPreferences;

public class MainActivity extends AppCompatActivity {

    private TextView textJWT;
    private Button btnLogout;
    private Context context = this;
    private CapstoneRepository capstoneRepository;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        textJWT = findViewById(R.id.tvJWT);
//        textJWT.setText(DynamicWorkflowSharedPreferences.getStoreJWT(MainActivity.this, ConstantDataManager.AUTHORIZATION_TOKEN));
        String tokenAuthorize = "Bearer " + DynamicWorkflowSharedPreferences.getStoreJWT(context, ConstantDataManager.AUTHORIZATION_TOKEN);
        getProfile(tokenAuthorize);
        btnLogout = findViewById(R.id.btn_logout);
        btnLogout.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                DynamicWorkflowSharedPreferences.removeJWT(context, ConstantDataManager.AUTHORIZATION_TOKEN);
                Intent intent = new Intent(MainActivity.this, LoginActivity.class);
                startActivity(intent);
                finish();
            }
        });
    }

    private void getProfile(String token) {
        capstoneRepository = new CapstoneRepositoryImpl();
        capstoneRepository.getProfile(token, new CallBackData<Profile>() {
            @Override
            public void onSuccess(Profile profile) {
                textJWT.setText("Email: " + profile.getEmail() + "\nFullname: " + profile.getFullName() + "\nDateOfBirth: " + profile.getDateOfBirth());
            }

            @Override
            public void onFail(String message) {
                textJWT.setText(message);
            }
        });
    }
}
