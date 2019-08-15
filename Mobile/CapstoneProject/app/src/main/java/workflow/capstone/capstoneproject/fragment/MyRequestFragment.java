package workflow.capstone.capstoneproject.fragment;


import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.os.Handler;
import android.os.Message;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentTransaction;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AbsListView;
import android.widget.AdapterView;
import android.widget.ListView;
import android.widget.TextView;

import com.squareup.picasso.Picasso;

import java.util.ArrayList;
import java.util.List;

import de.hdodenhof.circleimageview.CircleImageView;
import workflow.capstone.capstoneproject.R;
import workflow.capstone.capstoneproject.activity.ProfileActivity;
import workflow.capstone.capstoneproject.adapter.MyRequestAdapter;
import workflow.capstone.capstoneproject.entities.MyRequest;
import workflow.capstone.capstoneproject.entities.MyRequestPaging;
import workflow.capstone.capstoneproject.entities.Profile;
import workflow.capstone.capstoneproject.repository.CapstoneRepository;
import workflow.capstone.capstoneproject.repository.CapstoneRepositoryImpl;
import workflow.capstone.capstoneproject.utils.CallBackData;
import workflow.capstone.capstoneproject.utils.ConstantDataManager;
import workflow.capstone.capstoneproject.utils.DynamicWorkflowSharedPreferences;
import workflow.capstone.capstoneproject.utils.DynamicWorkflowUtils;

public class MyRequestFragment extends Fragment {

    private CapstoneRepository capstoneRepository;
    private CircleImageView imgAvatar;
    private MyRequestAdapter myRequestAdapter;
    private List<MyRequest> myRequestList = new ArrayList<>();
    private ListView listView;
    private int numberOfPage = 1;
    private boolean isLoading = false;
    private boolean isNoNewData = false;
    private MyHandler handler;
    private View footerView;
    private int totalRecord;

    public MyRequestFragment() {
        // Required empty public constructor
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        View view = inflater.inflate(R.layout.fragment_my_request, container, false);
        initView(view);

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

        myRequestList.clear();
        isNoNewData = false;
        numberOfPage = 1;
        //load my request when start app
        loadMyRequest(1);

        //load more my request
        if (totalRecord > 10) {
            loadMoreItems();
        }

        return view;
    }

    private void initView(View view) {
        handler = new MyHandler();
        listView = view.findViewById(R.id.list_my_request);
        imgAvatar = view.findViewById(R.id.img_avatar);
        LayoutInflater inflater = (LayoutInflater) getActivity().getSystemService(Context.LAYOUT_INFLATER_SERVICE);
        footerView = inflater.inflate(R.layout.footer_listview_progressbar, null);
    }

    private void loadMyRequest(final int page) {
        String token = DynamicWorkflowSharedPreferences.getStoreJWT(getActivity(), ConstantDataManager.AUTHORIZATION_TOKEN);
        capstoneRepository = new CapstoneRepositoryImpl();
        capstoneRepository.getMyRequest(token, page, ConstantDataManager.NUMBER_OF_RECORD, new CallBackData<MyRequestPaging>() {
            @Override
            public void onSuccess(MyRequestPaging myRequestPaging) {
                totalRecord = myRequestPaging.getTotalRecord();
                if (myRequestPaging.getMyRequests().size() == 0) {
                    isNoNewData = true;
                    listView.removeFooterView(footerView);
                } else if (page == 1) {
                    listView.removeFooterView(footerView);
                    myRequestList = myRequestPaging.getMyRequests();
                    if (getActivity() != null) {
                        myRequestAdapter = new MyRequestAdapter(myRequestList, getActivity());
                        listView.setAdapter(myRequestAdapter);
                    }
//                    DynamicWorkflowUtils.setListViewHeightBasedOnChildren(listView);
                    onItemCLick();
                } else if (page != 1) {
                    listView.removeFooterView(footerView);
                    myRequestAdapter.AddListItemAdapter(myRequestPaging.getMyRequests());
//                    DynamicWorkflowUtils.setListViewHeightBasedOnChildren(listView);
                    onItemCLick();
                }
            }

            @Override
            public void onFail(String message) {

            }
        });
    }

    private void loadMoreItems() {
        onItemCLick();
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
                    loadMyRequest(++numberOfPage);
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

    private void onItemCLick() {
        listView.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> adapterView, View view, int position, long id) {
                Fragment fragment = new DetailRequestFragment();
                Bundle bundle = new Bundle();
                MyRequest myRequest = (MyRequest) adapterView.getItemAtPosition(position);
                String requestActionID = myRequest.getCurrentRequestActionID();
                bundle.putString("requestActionID", requestActionID);
                bundle.putString("userNotificationID", null);
                bundle.putString("createDate", myRequest.getCreateDate());
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
