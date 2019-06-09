package workflow.capstone.capstoneproject.activity;

import android.content.Context;
import android.os.Bundle;
import android.support.design.widget.TabLayout;
import android.support.v4.view.ViewPager;
import android.support.v7.app.AppCompatActivity;
import android.view.View;
import android.widget.TextView;
import android.widget.Toast;

import workflow.capstone.capstoneproject.R;
import workflow.capstone.capstoneproject.adapter.TabAdapter;
import workflow.capstone.capstoneproject.entities.Profile;
import workflow.capstone.capstoneproject.fragment.NotificationFragment;
import workflow.capstone.capstoneproject.fragment.ProfileFragment;
import workflow.capstone.capstoneproject.fragment.RequestHistoryFragment;
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
    private TextView badge;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
//        String tokenAuthorize = DynamicWorkflowSharedPreferences.getStoreJWT(context, ConstantDataManager.AUTHORIZATION_TOKEN);
//        getProfile(tokenAuthorize);
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

        initTabLayout();
    }

    private void initTabLayout() {
        viewPager = findViewById(R.id.viewPager);
        tabLayout = findViewById(R.id.tabLayout);
        tabAdapter = new TabAdapter(getSupportFragmentManager());
        tabAdapter.addFragment(new WorkflowFragment());
        tabAdapter.addFragment(new RequestHistoryFragment());
        tabAdapter.addFragment(new NotificationFragment());
        tabAdapter.addFragment(new ProfileFragment());
        viewPager.setAdapter(tabAdapter);
        tabLayout.setupWithViewPager(viewPager);

        //workflow tab
        tabLayout.getTabAt(0).setIcon(tabIcons[1]);

        //history tab
        tabLayout.getTabAt(1).setIcon(tabIcons[0]);

        //notification tab
        tabLayout.getTabAt(2).setCustomView(R.layout.workflow_icon);
        View view = tabLayout.getTabAt(2).getCustomView();
        badge = view.findViewById(R.id.notification_badge);
        String tokenAuthorize = DynamicWorkflowSharedPreferences.getStoreJWT(context, ConstantDataManager.AUTHORIZATION_TOKEN);
        capstoneRepository = new CapstoneRepositoryImpl();
        capstoneRepository.getNumberOfNotification(tokenAuthorize, new CallBackData<String>() {
            @Override
            public void onSuccess(String s) {
                if (Integer.parseInt(s) > 0) {
                    badge.setText(s);
                    badge.setVisibility(View.VISIBLE);
                }
            }

            @Override
            public void onFail(String message) {

            }
        });

        //profile tab
        tabLayout.getTabAt(3).setIcon(tabIcons[4]);

        tabLayout.addOnTabSelectedListener(new TabLayout.OnTabSelectedListener() {
            @Override
            public void onTabSelected(TabLayout.Tab tab) {
                if (tab.getPosition() == 0) {
                    tabLayout.getTabAt(0).setIcon(tabIcons[1]);
                    Toast.makeText(context, "0", Toast.LENGTH_SHORT).show();
                } else if (tab.getPosition() == 1) {
                    tabLayout.getTabAt(1).setIcon(tabIcons[1]);
                    Toast.makeText(context, "1", Toast.LENGTH_SHORT).show();
                } else if (tab.getPosition() == 2) {
                    tabLayout.getTabAt(2).setIcon(tabIcons[3]);
                    badge.setVisibility(View.GONE);
                    Toast.makeText(context, "2", Toast.LENGTH_SHORT).show();
                } else if (tab.getPosition() == 3) {
                    tabLayout.getTabAt(3).setIcon(tabIcons[5]);
                    Toast.makeText(context, "3", Toast.LENGTH_SHORT).show();
                }
            }

            @Override
            public void onTabUnselected(TabLayout.Tab tab) {
                if (tab.getPosition() == 0) {
                    tabLayout.getTabAt(0).setIcon(tabIcons[0]);
                    Toast.makeText(context, "unselected 0", Toast.LENGTH_SHORT).show();
                } else if (tab.getPosition() == 1) {
                    tabLayout.getTabAt(1).setIcon(tabIcons[0]);
                    Toast.makeText(context, "unselected 1", Toast.LENGTH_SHORT).show();
                } else if (tab.getPosition() == 2) {
                    tabLayout.getTabAt(2).setIcon(tabIcons[2]);
                    Toast.makeText(context, "unselected 2", Toast.LENGTH_SHORT).show();
                } else if (tab.getPosition() == 3) {
                    tabLayout.getTabAt(3).setIcon(tabIcons[4]);
                    Toast.makeText(context, "unselected 3", Toast.LENGTH_SHORT).show();
                }
            }

            @Override
            public void onTabReselected(TabLayout.Tab tab) {

            }
        });
    }

    private int[] tabIcons = {
            R.drawable.ic_history_grey,
            R.drawable.ic_history_blue,
            R.drawable.ic_notification_grey,
            R.drawable.ic_notification_blue,
            R.drawable.ic_menu_grey,
            R.drawable.ic_menu_blue
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

}
