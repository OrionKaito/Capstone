package workflow.capstone.capstoneproject.activity;

import android.content.Context;
import android.os.Bundle;
import android.support.design.widget.TabLayout;
import android.support.v4.view.ViewPager;
import android.support.v7.app.AppCompatActivity;
import android.view.View;
import android.widget.ImageView;
import android.widget.TextView;

import net.yslibrary.android.keyboardvisibilityevent.KeyboardVisibilityEvent;
import net.yslibrary.android.keyboardvisibilityevent.KeyboardVisibilityEventListener;

import workflow.capstone.capstoneproject.R;
import workflow.capstone.capstoneproject.adapter.TabAdapter;
import workflow.capstone.capstoneproject.fragment.WorkflowFragment;
import workflow.capstone.capstoneproject.repository.CapstoneRepository;
import workflow.capstone.capstoneproject.repository.CapstoneRepositoryImpl;
import workflow.capstone.capstoneproject.utils.CallBackData;
import workflow.capstone.capstoneproject.utils.ConstantDataManager;
import workflow.capstone.capstoneproject.utils.DynamicWorkflowSharedPreferences;
import workflow.capstone.capstoneproject.utils.FragmentUtils;

public class MainActivity extends AppCompatActivity {

    private Context context = this;
    private CapstoneRepository capstoneRepository;

    private TabLayout tabLayout;
    private ViewPager viewPager;
    private TextView badge;
    private ImageView imageView;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        initTabLayout();
        KeyboardVisibilityEvent.setEventListener(this,
                new KeyboardVisibilityEventListener() {
                    @Override
                    public void onVisibilityChanged(boolean isOpen) {
                        tabLayout.setVisibility(isOpen ? View.GONE : View.VISIBLE);
                    }
                });
    }

    private void initTabLayout() {
        viewPager = findViewById(R.id.view_Pager);
        tabLayout = findViewById(R.id.tab_Layout);
        setupViewPager(viewPager);
        tabLayout.setupWithViewPager(viewPager);
        setupTabIcons();
        setOnChangeTab();
    }

    private void setupViewPager(ViewPager viewPager) {
        TabAdapter adapter = new TabAdapter(getSupportFragmentManager());
        viewPager.setAdapter(adapter);
        viewPager.setOffscreenPageLimit(4);
    }

    private void setupTabIcons() {
        //workflow tab
        tabLayout.getTabAt(0).setIcon(tabIcons[1]);

        //history tab
        tabLayout.getTabAt(1).setIcon(tabIcons[0]);

        //notification tab
        tabLayout.getTabAt(2).setCustomView(R.layout.notification_icon_grey);
        View view = tabLayout.getTabAt(2).getCustomView();
        imageView = view.findViewById(R.id.notification_icon);
        imageView.setImageResource(R.drawable.ic_notification_grey);
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
    }

    private int[] tabIcons = {
            R.drawable.ic_history_grey,
            R.drawable.ic_history_blue,
            R.drawable.ic_notification_grey,
            R.drawable.ic_notification_blue,
            R.drawable.ic_menu_grey,
            R.drawable.ic_menu_blue
    };

    private void setOnChangeTab() {
        tabLayout.addOnTabSelectedListener(new TabLayout.OnTabSelectedListener() {
            @Override
            public void onTabSelected(TabLayout.Tab tab) {
                int tabIndex = tab.getPosition();
                switch (tab.getPosition()) {
                    case 0:
                        tabLayout.getTabAt(tabIndex).setIcon(tabIcons[1]);
                        break;
                    case 1:
                        tabLayout.getTabAt(tabIndex).setIcon(tabIcons[1]);
                        break;
                    case 2:
                        imageView.setImageResource(R.drawable.ic_notification_blue);
                        badge.setVisibility(View.INVISIBLE);
                        break;
                    case 3:
                        tabLayout.getTabAt(tabIndex).setIcon(tabIcons[5]);
                        break;
                    default:
                        break;
                }
            }

            @Override
            public void onTabUnselected(TabLayout.Tab tab) {
                int tabIndex = tab.getPosition();
                switch (tab.getPosition()) {
                    case 0:
                        tabLayout.getTabAt(tabIndex).setIcon(tabIcons[0]);
                        FragmentUtils.changeFragment(MainActivity.this, R.id.page_one_fragment, new WorkflowFragment());
                        break;
                    case 1:
                        tabLayout.getTabAt(tabIndex).setIcon(tabIcons[0]);
                        break;
                    case 2:
                        imageView.setImageResource(R.drawable.ic_notification_grey);
                        break;
                    case 3:
                        tabLayout.getTabAt(tabIndex).setIcon(tabIcons[4]);
                        break;
                    default:
                        break;
                }
            }

            @Override
            public void onTabReselected(TabLayout.Tab tab) {

            }
        });
    }
}
