package workflow.capstone.capstoneproject.fragment;


import android.content.Intent;
import android.graphics.Color;
import android.os.Bundle;
import android.os.Handler;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentTransaction;
import android.support.v4.widget.SwipeRefreshLayout;
import android.text.Editable;
import android.text.TextWatcher;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.ListView;
import android.widget.TextView;

import com.squareup.picasso.Picasso;

import java.util.ArrayList;
import java.util.List;

import de.hdodenhof.circleimageview.CircleImageView;
import workflow.capstone.capstoneproject.R;
import workflow.capstone.capstoneproject.activity.MainActivity;
import workflow.capstone.capstoneproject.activity.ProfileActivity;
import workflow.capstone.capstoneproject.adapter.WorkflowAdapter;
import workflow.capstone.capstoneproject.entities.WorkflowTemplate;
import workflow.capstone.capstoneproject.repository.CapstoneRepository;
import workflow.capstone.capstoneproject.repository.CapstoneRepositoryImpl;
import workflow.capstone.capstoneproject.utils.CallBackData;
import workflow.capstone.capstoneproject.utils.ConstantDataManager;
import workflow.capstone.capstoneproject.utils.DynamicWorkflowSharedPreferences;
import workflow.capstone.capstoneproject.utils.DynamicWorkflowUtils;

public class WorkflowFragment extends Fragment {

    private CapstoneRepository capstoneRepository;
    private WorkflowAdapter workflowAdapter;
    private List<WorkflowTemplate> workflowList;
    private ListView listView;
    private SwipeRefreshLayout swipeRefreshLayout;
    private EditText mEdtSearch;
    private LinearLayout lnOpenSearchTab;
    private TextView tvCancelSearch;
    private ImageView imgSearch;
    private ImageView imgMenu;
    private CircleImageView imgAvatar;

    public WorkflowFragment() {
        // Required empty public constructor
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        final View view = inflater.inflate(R.layout.fragment_workflow, container, false);
        initView(view);
        imgSearch.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                lnOpenSearchTab.setVisibility(View.VISIBLE);
                imgSearch.setVisibility(View.GONE);
            }
        });

        Picasso.get().load("https://scontent.fsgn2-4.fna.fbcdn.net/v/t1.0-1/p160x160/51544827_1431384577001877_5331970394951778304_n.jpg?_nc_cat=109&_nc_oc=AQnco7rDhQqwfiIMn0yb5w1T_XbHhK4H7VHH2OkcvvJwPffe8ztui6o1jgmD0HV70sM_obUhA5ESdSz-trY9uwGu&_nc_ht=scontent.fsgn2-4.fna&oh=efcc572eee6bb9b41bc554297c98a4d6&oe=5DAD14A0")
                .into(imgAvatar);

//        imgMenu.setOnClickListener(new View.OnClickListener() {
//            @Override
//            public void onClick(View v) {
//                Intent intent = new Intent(getActivity(), ProfileActivity.class);
//                startActivity(intent);
//                getActivity().overridePendingTransition(R.anim.slide_in_right, R.anim.slide_out_left);
//            }
//        });
        imgAvatar.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(getActivity(), ProfileActivity.class);
                startActivity(intent);
                getActivity().overridePendingTransition(R.anim.slide_in_right, R.anim.slide_out_left);
            }
        });

        tvCancelSearch.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                lnOpenSearchTab.setVisibility(View.GONE);
                imgSearch.setVisibility(View.VISIBLE);
                loadWorkflows(view);
            }
        });

        //load workflow when start app
        loadWorkflows(view);

        //swipe to refresh list workflow
        swipeRefresh(view);

        //search view
        initSearchView(view);
        return view;
    }

    private void initView(View view) {
        lnOpenSearchTab = view.findViewById(R.id.linear_layout_open_search_tab);
        tvCancelSearch = view.findViewById(R.id.text_view_cancel_search);
        imgSearch = view.findViewById(R.id.img_search);
//        imgMenu = view.findViewById(R.id.img_menu);
        imgAvatar = view.findViewById(R.id.img_avatar);
    }

    private void loadWorkflows(final View view) {
        String token = DynamicWorkflowSharedPreferences.getStoreJWT(getActivity(), ConstantDataManager.AUTHORIZATION_TOKEN);
        capstoneRepository = new CapstoneRepositoryImpl();
        capstoneRepository.getWorkflow(token, new CallBackData<List<WorkflowTemplate>>() {
            @Override
            public void onSuccess(List<WorkflowTemplate> workflowTemplates) {
                listView = view.findViewById(R.id.list_workflow);
                workflowList = workflowTemplates;
                if (getActivity() != null) {
                    workflowAdapter = new WorkflowAdapter(workflowList, getActivity());
                    listView.setAdapter(workflowAdapter);
                }
                DynamicWorkflowUtils.setListViewHeightBasedOnChildren(listView);
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
                    @Override
                    public void run() {
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
        if (getActivity() != null) {
            workflowAdapter = new WorkflowAdapter(listWorkflow, getActivity());
            listView.setAdapter(workflowAdapter);
            itemOnClick(listView);
        }
    }

    private void itemOnClick(final ListView listView) {

        listView.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> adapterView, View view, int position, long id) {
                Fragment fragment = new SendRequestFragment();
                Bundle bundle = new Bundle();
                TextView tvName = view.findViewById(R.id.tv_workflow_name);
                String nameOfWorkflow = tvName.getText().toString();
                WorkflowTemplate workflowTemplate = (WorkflowTemplate) adapterView.getItemAtPosition(position);
                String workFlowTemplateID = workflowTemplate.getId();
                bundle.putString("nameOfWorkflow", nameOfWorkflow);
                bundle.putString("workFlowTemplateID", workFlowTemplateID);
                fragment.setArguments(bundle);

                FragmentManager fragmentManager = getActivity().getSupportFragmentManager();
                FragmentTransaction fragmentTransaction = fragmentManager.beginTransaction();
                fragmentTransaction.setCustomAnimations(R.anim.slide_in_right, R.anim.slide_out_left, R.anim.slide_in_left, R.anim.slide_out_right);
                fragmentTransaction.replace(R.id.main_frame, fragment);
                fragmentTransaction.addToBackStack(null);
                fragmentTransaction.commit();
            }
        });
    }

}
