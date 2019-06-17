package workflow.capstone.capstoneproject.fragment;


import android.graphics.Color;
import android.os.Bundle;
import android.os.Handler;
import android.support.v4.app.Fragment;
import android.support.v4.widget.SwipeRefreshLayout;
import android.text.Editable;
import android.text.TextWatcher;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.EditText;
import android.widget.ListView;
import android.widget.TextView;

import java.util.ArrayList;
import java.util.List;

import workflow.capstone.capstoneproject.R;
import workflow.capstone.capstoneproject.adapter.WorkflowAdapter;
import workflow.capstone.capstoneproject.entities.Workflow;
import workflow.capstone.capstoneproject.entities.WorkflowTemplate;
import workflow.capstone.capstoneproject.repository.CapstoneRepository;
import workflow.capstone.capstoneproject.repository.CapstoneRepositoryImpl;
import workflow.capstone.capstoneproject.utils.CallBackData;
import workflow.capstone.capstoneproject.utils.ConstantDataManager;
import workflow.capstone.capstoneproject.utils.DynamicWorkflowSharedPreferences;
import workflow.capstone.capstoneproject.utils.FragmentUtils;

public class WorkflowFragment extends Fragment {

    private CapstoneRepository capstoneRepository;
    private WorkflowAdapter workflowAdapter;
    private List<WorkflowTemplate> workflowList;
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
        String token = DynamicWorkflowSharedPreferences.getStoreJWT(getActivity(), ConstantDataManager.AUTHORIZATION_TOKEN);
        capstoneRepository = new CapstoneRepositoryImpl();
        capstoneRepository.getWorkflows(new CallBackData<List<WorkflowTemplate>>() {
            @Override
            public void onSuccess(List<WorkflowTemplate> workflowTemplates) {
                listView = view.findViewById(R.id.list);
                workflowList = workflowTemplates;
//                workflowAdapter = new WorkflowAdapter(workflowList, getContext());
                if (getActivity()!=null){
                    workflowAdapter = new WorkflowAdapter(workflowList, getActivity());
                    listView.setAdapter(workflowAdapter);
                }
//                listView.setAdapter(workflowAdapter);
                listView.setBackgroundColor(Color.WHITE);
                itemOnClick(listView);
            }

            @Override
            public void onFail(String message) {

            }
        });
    }

    private void swipeRefresh(final View view) {
        swipeRefreshLayout = view.findViewById(R.id.swipe_Container);
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
                    List<WorkflowTemplate> listWorkflow = new ArrayList<>();
                    for (WorkflowTemplate workflowTemplate : workflowList) {
                        if (workflowTemplate.getName().toLowerCase().contains(searchText)) {
                            listWorkflow.add(workflowTemplate);
                        }
                    }
                    searchWorkflow(listWorkflow);
                } else {
                    loadWorkflows(view);
                }
            }
        });
    }

    private void searchWorkflow(List<WorkflowTemplate> listWorkflow) {
        workflowAdapter = new WorkflowAdapter(listWorkflow, getContext());
        listView.setAdapter(workflowAdapter);
        itemOnClick(listView);
    }

    private void itemOnClick(final ListView listView) {

        listView.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> adapterView, View view, int position, long id) {
                Fragment fragment = new DetailWorkflowFragment();
                Bundle bundle = new Bundle();
                TextView tvName = view.findViewById(R.id.tv_Workflow_Name);
                String nameOfWorkflow = tvName.getText().toString();
                WorkflowTemplate workflowTemplate = (WorkflowTemplate) adapterView.getItemAtPosition(position);
                String workFlowTemplateID = workflowTemplate.getId();
                bundle.putString("nameOfWorkflow", nameOfWorkflow);
                bundle.putString("workFlowTemplateID", workFlowTemplateID);
                fragment.setArguments(bundle);
                FragmentUtils.changeFragment(getActivity(), R.id.page_one_fragment, fragment);
            }
        });
    }

}
