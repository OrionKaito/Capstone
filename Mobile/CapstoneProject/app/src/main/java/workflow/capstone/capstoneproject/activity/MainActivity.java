package workflow.capstone.capstoneproject.activity;

import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.support.design.widget.TabLayout;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentTransaction;
import android.support.v7.app.AppCompatActivity;
import android.view.View;
import android.widget.ImageView;
import android.widget.TextView;

import net.yslibrary.android.keyboardvisibilityevent.KeyboardVisibilityEvent;
import net.yslibrary.android.keyboardvisibilityevent.KeyboardVisibilityEventListener;

import java.util.List;

import workflow.capstone.capstoneproject.R;
import workflow.capstone.capstoneproject.entities.Profile;
import workflow.capstone.capstoneproject.fragment.ListHandleRequestFragment;
import workflow.capstone.capstoneproject.fragment.MyRequestFragment;
import workflow.capstone.capstoneproject.fragment.ProfileFragment;
import workflow.capstone.capstoneproject.fragment.ListNotificationFragment;
import workflow.capstone.capstoneproject.fragment.WorkflowFragment;
import workflow.capstone.capstoneproject.repository.CapstoneRepository;
import workflow.capstone.capstoneproject.repository.CapstoneRepositoryImpl;
import workflow.capstone.capstoneproject.utils.CallBackData;
import workflow.capstone.capstoneproject.utils.ConstantDataManager;
import workflow.capstone.capstoneproject.utils.DynamicWorkflowSharedPreferences;

public class MainActivity extends AppCompatActivity {

    private Context context = this;
    private CapstoneRepository capstoneRepository;
    public static TabLayout tabLayout;
    public static TextView notificationBadge;
    public static TextView taskBadge;
    public static ImageView imageViewNotification;
    public static ImageView imageViewTask;
    private FragmentManager fragmentManager;
    private FragmentTransaction fragmentTransaction;

    private WorkflowFragment workflowFragment;
    private ListNotificationFragment listCompleteRequestFragment;
    private ListHandleRequestFragment listHandleRequestFragment;
    private MyRequestFragment myRequestFragment;
    private ProfileFragment profileFragment;
    private String token;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        KeyboardVisibilityEvent.setEventListener(this,
                new KeyboardVisibilityEventListener() {
                    @Override
                    public void onVisibilityChanged(boolean isOpen) {
                        tabLayout.setVisibility(isOpen ? View.GONE : View.VISIBLE);
                    }
                });

        token = DynamicWorkflowSharedPreferences.getStoreJWT(context, ConstantDataManager.AUTHORIZATION_TOKEN);

        capstoneRepository = new CapstoneRepositoryImpl();
        capstoneRepository.getProfile(token, new CallBackData<List<Profile>>() {
            @Override
            public void onSuccess(List<Profile> listProfile) {
                Profile profile = listProfile.get(0);
                DynamicWorkflowSharedPreferences.storeData(context, ConstantDataManager.PROFILE_KEY, ConstantDataManager.PROFILE_NAME, profile);
                initTabLayout();
            }

            @Override
            public void onFail(String message) {

            }
        });
    }

    private void initTabLayout() {
        tabLayout = findViewById(R.id.tab_Layout);
        workflowFragment = new WorkflowFragment();
        listCompleteRequestFragment = new ListNotificationFragment();
        listHandleRequestFragment = new ListHandleRequestFragment();
        myRequestFragment = new MyRequestFragment();
        profileFragment = new ProfileFragment();
        for (int i = 0; i < 4; i++) {
            tabLayout.addTab(tabLayout.newTab());
        }

        setupTabIcons();
        setOnChangeTab();
        Intent intent = this.getIntent();

        if (intent.getStringExtra("pushNotification") != null &&intent.getStringExtra("pushNotification").equals("Notification")) {
            setCurrentTabFragment(3);
            setUnselectedTab(0);
            setUnselectedTab(1);
            setUnselectedTab(2);
        } else {
            setCurrentTabFragment(0);
        }
    }

    private void setupTabIcons() {
        //workflow tab
        tabLayout.getTabAt(0).setIcon(tabIcons[1]);

        //your request tab
        tabLayout.getTabAt(1).setIcon(tabIcons[4]);

        //handle request tab
        tabLayout.getTabAt(2).setCustomView(R.layout.task_icon);
        View taskView = tabLayout.getTabAt(2).getCustomView();
        imageViewTask = taskView.findViewById(R.id.task_icon);
        imageViewTask.setImageResource(R.drawable.ic_task_gray);

        //notification tab
        tabLayout.getTabAt(3).setCustomView(R.layout.notification_icon);
        View notificationView = tabLayout.getTabAt(3).getCustomView();
        imageViewNotification = notificationView.findViewById(R.id.notification_icon);
        imageViewNotification.setImageResource(R.drawable.ic_notification_grey);
        notificationBadge = notificationView.findViewById(R.id.notification_badge);
        capstoneRepository = new CapstoneRepositoryImpl();
        capstoneRepository.getNumberOfNotification(token, new CallBackData<String>() {
            @Override
            public void onSuccess(String s) {
                if (Integer.parseInt(s) > 0) {
                    notificationBadge.setText(s);
                    notificationBadge.setVisibility(View.VISIBLE);
                }
            }

            @Override
            public void onFail(String message) {

            }
        });
    }

    private int[] tabIcons = {
            R.drawable.ic_home_gray,
            R.drawable.ic_home_blue,
            R.drawable.ic_notification_grey,
            R.drawable.ic_notification_blue,
            R.drawable.ic_history_grey,
            R.drawable.ic_history_blue
    };

    private void setOnChangeTab() {
        tabLayout.addOnTabSelectedListener(new TabLayout.OnTabSelectedListener() {
            @Override
            public void onTabSelected(TabLayout.Tab tab) {
                setCurrentTabFragment(tab.getPosition());
            }

            @Override
            public void onTabUnselected(TabLayout.Tab tab) {
                setUnselectedTab(tab.getPosition());
            }

            @Override
            public void onTabReselected(TabLayout.Tab tab) {
                setCurrentTabFragment(tab.getPosition());
            }
        });
    }

    private void setCurrentTabFragment(int tabPosition)
    {
        switch (tabPosition) {
            case 0:
                tabLayout.getTabAt(tabPosition).setIcon(tabIcons[1]);
                replaceFragment(workflowFragment);
                break;
            case 1:
                tabLayout.getTabAt(tabPosition).setIcon(tabIcons[5]);
                replaceFragment(myRequestFragment);
                break;
            case 2:
                imageViewTask.setImageResource(R.drawable.ic_task_blue);
                replaceFragment(listHandleRequestFragment);
                break;
            case 3:
                imageViewNotification.setImageResource(R.drawable.ic_notification_blue);
                replaceFragment(listCompleteRequestFragment);
                break;
            default:
                break;
        }
    }

    private void setUnselectedTab(int tabPosition)
    {
        switch (tabPosition) {
            case 0:
                tabLayout.getTabAt(tabPosition).setIcon(tabIcons[0]);
                break;
            case 1:
                tabLayout.getTabAt(tabPosition).setIcon(tabIcons[4]);
                break;
            case 2:
                imageViewTask.setImageResource(R.drawable.ic_task_gray);
                break;
            case 3:
                imageViewNotification.setImageResource(R.drawable.ic_notification_grey);
                notificationBadge.setVisibility(View.INVISIBLE);
                break;
            default:
                break;
        }
    }

    private void replaceFragment(Fragment fragment) {
        FragmentManager fm = MainActivity.this.getSupportFragmentManager();
        for(int i = 0; i < fm.getBackStackEntryCount(); ++i) {
            fm.popBackStack();
        }
        fragmentManager = MainActivity.this.getSupportFragmentManager();
        fragmentTransaction = fragmentManager.beginTransaction();
        fragmentTransaction.replace(R.id.main_frame, fragment);
        fragmentTransaction.commit();
    }
}
