package workflow.capstone.capstoneproject.fragment;


import android.os.Bundle;
import android.os.Handler;
import android.support.v4.app.Fragment;
import android.support.v4.app.ListFragment;
import android.support.v4.widget.SwipeRefreshLayout;
import android.text.Editable;
import android.text.TextWatcher;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.EditText;
import android.widget.LinearLayout;
import android.widget.ListView;

import java.util.ArrayList;
import java.util.List;

import workflow.capstone.capstoneproject.R;
import workflow.capstone.capstoneproject.adapter.WorkflowAdapter;
import workflow.capstone.capstoneproject.entities.Workflow;
import workflow.capstone.capstoneproject.repository.CapstoneRepository;
import workflow.capstone.capstoneproject.repository.CapstoneRepositoryImpl;
import workflow.capstone.capstoneproject.utils.CallBackData;
import workflow.capstone.capstoneproject.utils.ConstantDataManager;
import workflow.capstone.capstoneproject.utils.DynamicWorkflowSharedPreferences;

public class WorkflowFragment extends Fragment {

    private CapstoneRepository capstoneRepository;
    private WorkflowAdapter workflowAdapter;
    private List<Workflow> workflowList;
    private ListView listView;
    private SwipeRefreshLayout swipeRefreshLayout;
    private EditText mEdtSearch;

    public WorkflowFragment() {
        // Required empty public constructor
    }


    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        View view = inflater.inflate(R.layout.fragment_workflow, container, false);
        //load workflow when start app
        loadWorkflows(view);

        //swipe to refresh list workflow
        swipeRefresh(view);

        //search view
        initSearchView(view);
        return view;
    }

    private void loadWorkflows(final View view) {
        String token = DynamicWorkflowSharedPreferences.getStoreJWT(getContext(), ConstantDataManager.AUTHORIZATION_TOKEN);
        capstoneRepository = new CapstoneRepositoryImpl();
        capstoneRepository.getWorkflows(token, new CallBackData<List<Workflow>>() {
            @Override
            public void onSuccess(List<Workflow> workflows) {
                listView = view.findViewById(R.id.list);
                workflowList = workflows;
                workflowAdapter = new WorkflowAdapter(workflowList, getContext());
                listView.setAdapter(workflowAdapter);
                listView.setBackgroundColor(getResources().getColor(R.color.white));
            }

            @Override
            public void onFail(String message) {

            }
        });
    }

    private void swipeRefresh(final View view) {
        swipeRefreshLayout = view.findViewById(R.id.swipeContainer);
        swipeRefreshLayout.setOnRefreshListener(new SwipeRefreshLayout.OnRefreshListener() {
            @Override
            public void onRefresh() {
                new Handler().postDelayed(new Runnable() {
                    @Override public void run() {
                        swipeRefreshLayout.setRefreshing(false);
                        loadWorkflows(view);
                    }
                }, 1500);
            }
        });
    }

    private void initSearchView(View view) {
        mEdtSearch = view.findViewById(R.id.edit_text_search);
        addTextWatcher(view);
    }

    private void addTextWatcher(final View view) {
        mEdtSearch.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence charSequence, int i, int i1, int i2) {

            }

            @Override
            public void onTextChanged(CharSequence charSequence, int i, int i1, int i2) {
            }

            @Override
            public void afterTextChanged(Editable editable) {
                String searchText = mEdtSearch.getText().toString().trim().toLowerCase();

                if (!searchText.isEmpty()) {
                    List<Workflow> listWorkflow = new ArrayList<>();
                    for (Workflow workflow : workflowList) {
                        if (workflow.getName().toLowerCase().contains(searchText)) {
                            listWorkflow.add(workflow);
                        }
                    }
                    searchWorkflow(listWorkflow);
                } else {
                    loadWorkflows(view);
                }
            }
        });
    }

    private void searchWorkflow(List<Workflow> listWorkflow) {
        workflowAdapter = new WorkflowAdapter(listWorkflow, getContext());
        listView.setAdapter(workflowAdapter);
    }

}
