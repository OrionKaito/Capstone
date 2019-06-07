package workflow.capstone.capstoneproject.activity;

import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.os.Bundle;
import android.support.design.widget.TabLayout;
import android.support.v4.view.ViewPager;
import android.support.v7.app.AlertDialog;
import android.support.v7.app.AppCompatActivity;

import workflow.capstone.capstoneproject.R;
import workflow.capstone.capstoneproject.adapter.TabAdapter;
import workflow.capstone.capstoneproject.entities.Profile;
import workflow.capstone.capstoneproject.fragment.NotificationFragment;
import workflow.capstone.capstoneproject.fragment.ProfileFragment;
import workflow.capstone.capstoneproject.fragment.WorkflowFragment;
import workflow.capstone.capstoneproject.repository.CapstoneRepository;
import workflow.capstone.capstoneproject.repository.CapstoneRepositoryImpl;
import workflow.capstone.capstoneproject.utils.CallBackData;
import workflow.capstone.capstoneproject.utils.ConstantDataManager;
import workflow.capstone.capstoneproject.utils.DynamicWorkflowSharedPreferences;

public class MainActivity extends AppCompatActivity {

    private Context context = this;
    private CapstoneRepository capstoneRepository;

    private TabAdapter tabAdapter;
    private TabLayout tabLayout;
    private ViewPager viewPager;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        String tokenAuthorize = DynamicWorkflowSharedPreferences.getStoreJWT(context, ConstantDataManager.AUTHORIZATION_TOKEN);
        getProfile(tokenAuthorize);
//        btnLogout = findViewById(R.id.btn_logout);
//        btnLogout.setOnClickListener(new View.OnClickListener() {
//            @Override
//            public void onClick(View view) {
//                DynamicWorkflowSharedPreferences.removeJWT(context, ConstantDataManager.AUTHORIZATION_TOKEN);
//                Intent intent = new Intent(MainActivity.this, LoginActivity.class);
//                startActivity(intent);
//                finish();
//            }
//        });

        initView();
    }

    private void initView() {
        viewPager = findViewById(R.id.viewPager);
        tabLayout = findViewById(R.id.tabLayout);
        tabAdapter = new TabAdapter(getSupportFragmentManager());
        tabAdapter.addFragment(new WorkflowFragment());
        tabAdapter.addFragment(new NotificationFragment());
        tabAdapter.addFragment(new ProfileFragment());
        viewPager.setAdapter(tabAdapter);
        tabLayout.setupWithViewPager(viewPager);
        tabLayout.getTabAt(0).setIcon(tabIcons[0]);
        tabLayout.getTabAt(1).setIcon(tabIcons[0]);
        tabLayout.getTabAt(2).setIcon(tabIcons[0]);
    }

    private int[] tabIcons = {
            R.drawable.ic_notification
    };

    private void getProfile(String token) {
        capstoneRepository = new CapstoneRepositoryImpl();
        capstoneRepository.getProfile(token, new CallBackData<Profile>() {
            @Override
            public void onSuccess(Profile profile) {
//                textJWT.setText("Email: " + profile.getEmail() + "\nFullname: " + profile.getFullName() + "\nDateOfBirth: " + profile.getDateOfBirth());
            }

            @Override
            public void onFail(String message) {
//                textJWT.setText(message);
            }
        });
    }

    private void logout() {
        AlertDialog.Builder builder = new AlertDialog.Builder(this);
        builder.setCancelable(false);
        builder.setMessage(R.string.logout_confirm)
                .setPositiveButton(R.string.logout, new DialogInterface.OnClickListener() {
                    public void onClick(DialogInterface dialog, int id) {
                        DynamicWorkflowSharedPreferences.removeJWT(context, ConstantDataManager.AUTHORIZATION_TOKEN);
                        Intent intent = new Intent(MainActivity.this, LoginActivity.class);
                        startActivity(intent);
                        finish();
                    }
                })
                .setNegativeButton(R.string.cancel, new DialogInterface.OnClickListener() {
                    public void onClick(DialogInterface dialog, int id) {
                        dialog.dismiss();
                    }
                });
        AlertDialog dialog = builder.create();
        dialog.show();
    }
}
