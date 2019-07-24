package workflow.capstone.capstoneproject.activity;

import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentTransaction;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.ImageView;
import android.widget.TextView;

import workflow.capstone.capstoneproject.R;
import workflow.capstone.capstoneproject.fragment.ProfileFragment;

public class ProfileActivity extends AppCompatActivity {

    private FragmentManager fragmentManager;
    private FragmentTransaction fragmentTransaction;
    public static TextView tvProfileTitle;
    public static ImageView imgBack;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_profile);
        initView();

        imgBack.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                onBackPressed();
            }
        });

        replaceFragment(new ProfileFragment());
    }

    @Override
    protected void onResume() {
        super.onResume();
    }

    private void replaceFragment(Fragment fragment) {
        FragmentManager fm = ProfileActivity.this.getSupportFragmentManager();
        for(int i = 0; i < fm.getBackStackEntryCount(); ++i) {
            fm.popBackStack();
        }
        fragmentManager = ProfileActivity.this.getSupportFragmentManager();
        fragmentTransaction = fragmentManager.beginTransaction();
        fragmentTransaction.replace(R.id.profile_frame, fragment);
        fragmentTransaction.commit();
    }

    private void initView() {
        tvProfileTitle = findViewById(R.id.tv_profile_title);
        imgBack = findViewById(R.id.img_Back);
    }

    @Override
    public void onBackPressed() {
        super.onBackPressed();
        overridePendingTransition(R.anim.slide_in_left, R.anim.slide_out_right);
    }
}
