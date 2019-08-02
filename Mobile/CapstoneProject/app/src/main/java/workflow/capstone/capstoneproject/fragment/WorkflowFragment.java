package workflow.capstone.capstoneproject.fragment;


import android.content.Context;
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
import es.dmoral.toasty.Toasty;
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
    private List<WorkflowTemplate> workflowList = new ArrayList<>();
    private ListView listView;
    private SwipeRefreshLayout swipeRefreshLayout;
    private EditText mEdtSearch;
    private LinearLayout lnOpenSearchTab;
    private TextView tvCancelSearch;
    private ImageView imgSearch;
    private CircleImageView imgAvatar;
    private int numberOfPage = 1;
    private boolean isLoading = false;
    private boolean isNoNewData = false;
    private String token;
    private MyHandler handler;
    private View footerView;
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

        token = DynamicWorkflowSharedPreferences.getStoreJWT(getActivity(), ConstantDataManager.AUTHORIZATION_TOKEN);

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
                loadWorkflow(1);
            }
        });

        workflowList.clear();
        isNoNewData = false;
        numberOfPage = 1;
        //load workflow when start app
        loadWorkflow(numberOfPage);

        //load more workflow
        if (totalRecord > 10) {
            loadMoreItems();
        }

        //swipe to refresh list workflow
        swipeRefresh(view);

        //search view
//        initSearchView(view);
        return view;
    }

    private void initView(View view) {
        handler = new MyHandler();
        lnOpenSearchTab = view.findViewById(R.id.linear_layout_open_search_tab);
        tvCancelSearch = view.findViewById(R.id.text_view_cancel_search);
        imgSearch = view.findViewById(R.id.img_search);
        imgAvatar = view.findViewById(R.id.img_avatar);
        listView = view.findViewById(R.id.list_workflow);
        LayoutInflater inflater = (LayoutInflater) getActivity().getSystemService(Context.LAYOUT_INFLATER_SERVICE);
        footerView = inflater.inflate(R.layout.footer_listview_progressbar, null);
    }

    private void loadWorkflow(final int page) {
        capstoneRepository = new CapstoneRepositoryImpl();
        capstoneRepository.getWorkflow(token, page, ConstantDataManager.NUMBER_OF_RECORD, new CallBackData<WorkflowTemplatePaging>() {
            @Override
            public void onSuccess(WorkflowTemplatePaging workflowTemplatePaging) {
                totalRecord = workflowTemplatePaging.getTotalRecord();
                if (workflowTemplatePaging.getWorkFlowTemplates().size() == 0) {
                    isNoNewData = true;
                    listView.removeFooterView(footerView);
                    Toasty.success(getActivity(), "No new data", Toasty.LENGTH_SHORT).show();
                } else if (page == 1) {
                    listView.removeFooterView(footerView);
                    workflowList = workflowTemplatePaging.getWorkFlowTemplates();
                    if (getActivity() != null) {
                        workflowAdapter = new WorkflowAdapter(workflowList, getActivity());
                        listView.setAdapter(workflowAdapter);
                    }
                    DynamicWorkflowUtils.setListViewHeightBasedOnChildren(listView);
                    itemOnClick();
                } else if (page != 1) {
                    listView.removeFooterView(footerView);
                    workflowAdapter.AddListItemAdapter(workflowTemplatePaging.getWorkFlowTemplates());
                    DynamicWorkflowUtils.setListViewHeightBasedOnChildren(listView);
                    itemOnClick();
                }
            }

            @Override
            public void onFail(String message) {

            }
        });
    }

    private void loadMoreItems() {
        itemOnClick();
        listView.setOnScrollListener(new AbsListView.OnScrollListener() {
            //scroll list view tới 1 vị trí nào đó
            @Override
            public void onScrollStateChanged(AbsListView view, int scrollState) {

            }

            //scroll list view
            @Override
            public void onScroll(AbsListView absListView, int firstItem, int visibleItem, int totalItem) {
                //firstItem: item đầu tiên
                //visibleItem: các item có thể nhìn thấy trong view này
                //totalItem: tổng số lượng item trong listview
                if (firstItem + visibleItem == totalItem && totalItem != 0 && isLoading == false && isNoNewData == false) {
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
                    listView.addFooterView(footerView);
                    break;
                case 1:
                    loadWorkflow(++numberOfPage);
                    isLoading = false;
                    break;
            }
        }
    }

    private class ThreadData extends Thread {
        @Override
        public void run() {
            handler.sendEmptyMessage(0);
            try {
                Thread.sleep(3000);
            } catch (InterruptedException e) {
                e.printStackTrace();
            }

            //liên kết giữa các thread với handler
            Message message = handler.obtainMessage(1);
            handler.sendMessage(message);
            super.run();
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
                        workflowList = new ArrayList<>();
                        loadWorkflow(1);
                    }
                }, 1500);
            }
        });
    }

    private void initSearchView(View view) {
        mEdtSearch = view.findViewById(R.id.edit_text_search);
        addTextWatcher();
    }

    private void addTextWatcher() {
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
                    loadWorkflow(1);
                }
            }
        });
    }

    private void searchWorkflow(List<WorkflowTemplate> listWorkflow) {
        if (getActivity() != null) {
            workflowAdapter = new WorkflowAdapter(listWorkflow, getActivity());
            listView.setAdapter(workflowAdapter);
            itemOnClick();
        }
    }

    private void itemOnClick() {

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
