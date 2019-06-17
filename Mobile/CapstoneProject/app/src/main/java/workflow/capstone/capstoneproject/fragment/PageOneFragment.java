package workflow.capstone.capstoneproject.fragment;


import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentTransaction;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import workflow.capstone.capstoneproject.R;

public class PageOneFragment extends Fragment {


    public PageOneFragment() {
        // Required empty public constructor
    }


    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_page_one, container, false);
        FragmentTransaction transaction = getFragmentManager()
                .beginTransaction();
        transaction.replace(R.id.page_one_fragment, new WorkflowFragment());
        transaction.commit();
        return view;
    }

}
