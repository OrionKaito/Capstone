package workflow.capstone.capstoneproject.fragment;


import android.content.Intent;
import android.graphics.Color;
import android.os.Bundle;
import android.os.Handler;
import android.os.Message;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentTransaction;
import android.support.v4.widget.SwipeRefreshLayout;
import android.text.Editable;
import android.text.TextWatcher;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AbsListView;
import android.widget.AdapterView;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.ListView;
import android.widget.ProgressBar;
import android.widget.TextView;

import com.squareup.picasso.Picasso;

import java.util.ArrayList;
import java.util.List;

import de.hdodenhof.circleimageview.CircleImageView;
import workflow.capstone.capstoneproject.R;
import workflow.capstone.capstoneproject.activity.MainActivity;
import workflow.capstone.capstoneproject.activity.ProfileActivity;
import workflow.capstone.capstoneproject.adapter.WorkflowAdapter;
import workflow.capstone.capstoneproject.entities.Profile;
import workflow.capstone.capstoneproject.entities.WorkflowTemplate;
import workflow.capstone.capstoneproject.entities.WorkflowTemplatePaging;
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
    private CircleImageView imgAvatar;
    private int numberOfPage = 1;
    private boolean isLoading = false;
    private String token;
    private MyHandler handler;
    private int totalRecord;

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

        Profile profile = DynamicWorkflowSharedPreferences.getStoredData(getContext(), ConstantDataManager.PROFILE_KEY, ConstantDataManager.PROFILE_NAME);

        if (profile.getImagePath().isEmpty()) {
            Picasso.get()
                    .load("https://scontent.fsgn5-7.fna.fbcdn.net/v/t31.0-1/c282.0.960.960a/p960x960/10506738_10150004552801856_220367501106153455_o.jpg?_nc_cat=1&_nc_oc=AQk4xtBQZvCEIqEbATAtZAKtIaeJ7hZd_YGsEKB0TF9590ta9lE-02XOAw_SNgh0KVY&_nc_ht=scontent.fsgn5-7.fna&oh=216ae69582d7e89f6c1b5de5b6e8a2d9&oe=5DDF5069")
                    .into(imgAvatar);
        } else {
            Picasso.get()
                    .load(ConstantDataManager.IMAGE_URL + profile.getImagePath())
                    .into(imgAvatar);
        }

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
                loadWorkflows();
            }
        });

        //load workflow when start app
        loadWorkflows();

        //swipe to refresh list workflow
        swipeRefresh(view);

        //search view
        initSearchView(view);
        return view;
    }

    private void initView(View view) {
        handler = new MyHandler();
        lnOpenSearchTab = view.findViewById(R.id.linear_layout_open_search_tab);
        tvCancelSearch = view.findViewById(R.id.text_view_cancel_search);
        imgSearch = view.findViewById(R.id.img_search);
        imgAvatar = view.findViewById(R.id.img_avatar);
        listView = view.findViewById(R.id.list_workflow);
    }

    private void loadWorkflows() {
        numberOfPage = 1;
        token = DynamicWorkflowSharedPreferences.getStoreJWT(getActivity(), ConstantDataManager.AUTHORIZATION_TOKEN);
        capstoneRepository = new CapstoneRepositoryImpl();
        capstoneRepository.getWorkflow(token, numberOfPage, ConstantDataManager.NUMBER_OF_RECORD, new CallBackData<WorkflowTemplatePaging>() {
            @Override
            public void onSuccess(WorkflowTemplatePaging workflowTemplatePaging) {
                totalRecord = workflowTemplatePaging.getTotalRecord();
                workflowList = workflowTemplatePaging.getWorkFlowTemplates();
                if (getActivity() != null) {
                    workflowAdapter = new WorkflowAdapter(workflowList, getActivity());
                    listView.setAdapter(workflowAdapter);
                }
                DynamicWorkflowUtils.setListViewHeightBasedOnChildren(listView);
                itemOnClick(listView);
                if (workflowTemplatePaging.getTotalRecord() > 10) {
                    loadMoreItems(listView);
                }
            }

            @Override
            public void onFail(String message) {

            }
        });
    }

    private void loadMoreItems(final ListView listView) {
//        listView.setOnScrollListener(new AbsListView.OnScrollListener() {
//            @Override
//            public void onScrollStateChanged(AbsListView view, int scrollState) {
//
//            }
//
//            @Override
//            public void onScroll(AbsListView view, int firstVisibleItem, int visibleItemCount, int totalItemCount) {
//                if (firstVisibleItem + visibleItemCount == totalItemCount && totalItemCount != 0) {
//                    if (flag_loading == false) {
//                        flag_loading = true;
//                        addItems();
//                    }
//                }
//            }
//        });

        listView.setOnScrollListener(new AbsListView.OnScrollListener() {
            @Override
            public void onScrollStateChanged(AbsListView view, int scrollState) {

            }

            @Override
            public void onScroll(AbsListView absListView, int firstVisibleItem, int visibleItemCount, int totalItemCount) {
                if (absListView.getLastVisiblePosition() == totalItemCount - 1 && isLoading == false) {
                    isLoading = true;
                    Thread thread = new ThreadData();
                    thread.start();
                }
            }
        });
    }

    private class MyHandler extends Handler {
        @Override
        public void handleMessage(Message msg) {
            switch (msg.what) {
                case 0:
                    DynamicWorkflowUtils.addListViewFooter(getContext(), listView);
                    break;
                case 1:
                    DynamicWorkflowUtils.removeListViewFooter(getContext(), listView);
                    workflowAdapter.AddListItemAdapter((List<WorkflowTemplate>) msg.obj);
                    isLoading = false;
                    break;
            }
        }
    }

    private class ThreadData extends Thread {
        @Override
        public void run() {
            numberOfPage += 1;
            handler.sendEmptyMessage(0);
            final List<WorkflowTemplate> workflowTemplateList = new ArrayList<>();
            capstoneRepository = new CapstoneRepositoryImpl();
            capstoneRepository.getWorkflow(token, numberOfPage, ConstantDataManager.NUMBER_OF_RECORD, new CallBackData<WorkflowTemplatePaging>() {
                @Override
                public void onSuccess(WorkflowTemplatePaging workflowTemplatePaging) {
                    workflowTemplateList.addAll(workflowTemplatePaging.getWorkFlowTemplates());
                    try {
                        Thread.sleep(3000);
                    } catch (InterruptedException e) {
                        e.printStackTrace();
                    }

                    Message message = handler.obtainMessage(1, workflowTemplateList);
                    handler.sendMessage(message);
                }

                @Override
                public void onFail(String message) {

                }
            });
        }
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
                        loadWorkflows();
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
                    loadWorkflows();
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
