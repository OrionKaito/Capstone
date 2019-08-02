package workflow.capstone.capstoneproject.adapter;

import android.support.annotation.NonNull;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentPagerAdapter;

import workflow.capstone.capstoneproject.fragment.ListHandleRequestFragment;
import workflow.capstone.capstoneproject.fragment.ProfileFragment;
import workflow.capstone.capstoneproject.fragment.ListNotificationFragment;
import workflow.capstone.capstoneproject.fragment.WorkflowFragment;

public class TabAdapter extends FragmentPagerAdapter {

    private static final int NUM_ITEMS = 4;

    public TabAdapter(FragmentManager fm) {
        super(fm);
    }

    @Override
    public Fragment getItem(int position) {
        if (position == 0) {
            return new WorkflowFragment();
        } else if (position == 1) {
            return new ListNotificationFragment();
        } else if (position == 2) {
            return new ListHandleRequestFragment();
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
}
