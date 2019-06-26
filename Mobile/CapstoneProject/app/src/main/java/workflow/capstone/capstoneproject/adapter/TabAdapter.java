package workflow.capstone.capstoneproject.adapter;

import android.support.annotation.NonNull;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentPagerAdapter;

import java.util.ArrayList;
import java.util.List;

import workflow.capstone.capstoneproject.fragment.NotificationFragment;
import workflow.capstone.capstoneproject.fragment.PageOneFragment;
import workflow.capstone.capstoneproject.fragment.ProfileFragment;
import workflow.capstone.capstoneproject.fragment.RequestHistoryFragment;

public class TabAdapter extends FragmentPagerAdapter {

    private static final int NUM_ITEMS = 4;

    private List<Fragment> fragmentList = new ArrayList<>();

    public TabAdapter(FragmentManager fm) {
        super(fm);
    }

    @Override
    public Fragment getItem(int position) {
        if (position == 0) {
            return new PageOneFragment();
        } else if (position == 1) {
            return new RequestHistoryFragment();
        } else if (position == 2) {
            return new NotificationFragment();
        } else {
            return new ProfileFragment();
        }
    }

    @Override
    public int getCount() {
        return NUM_ITEMS;
    }

    @Override
    public int getItemPosition(@NonNull Object object) {
        notifyDataSetChanged();
        return POSITION_NONE;
    }

    public void addFragment(Fragment fragment) {
        fragmentList.add(fragment);
        notifyDataSetChanged();
    }
}
