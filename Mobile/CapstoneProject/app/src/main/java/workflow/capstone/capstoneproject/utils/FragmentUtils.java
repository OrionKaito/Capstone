package workflow.capstone.capstoneproject.utils;

import android.app.Activity;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentActivity;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentTransaction;

import workflow.capstone.capstoneproject.fragment.BottomSheetFragment;

public class FragmentUtils {

    public static void changeFragment(Activity activity, int layout, Fragment fragment) {
        FragmentManager fragmentManager = ((FragmentActivity) activity).getSupportFragmentManager();
        FragmentTransaction fragmentTransaction = fragmentManager.beginTransaction();
        fragmentTransaction.replace(layout, fragment);
        fragmentTransaction.addToBackStack(null);
        fragmentTransaction.commit();
    }

    public static void back(Activity activity) {
        ((FragmentActivity) activity).getSupportFragmentManager().popBackStack();
    }

    public static void showBottomSheetDialog(Activity activity) {
        BottomSheetFragment bottomSheetFragment = new BottomSheetFragment();
        bottomSheetFragment.show(((FragmentActivity) activity).getSupportFragmentManager(), bottomSheetFragment.getTag());
    }
}
