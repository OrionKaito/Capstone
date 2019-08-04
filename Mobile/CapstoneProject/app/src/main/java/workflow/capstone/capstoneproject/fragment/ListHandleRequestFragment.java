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
import android.widget.ImageView;
import android.widget.ListView;

import com.squareup.picasso.Picasso;

import java.util.ArrayList;
import java.util.List;

import de.hdodenhof.circleimageview.CircleImageView;
import workflow.capstone.capstoneproject.R;
import workflow.capstone.capstoneproject.activity.MainActivity;
import workflow.capstone.capstoneproject.activity.ProfileActivity;
import workflow.capstone.capstoneproject.adapter.NotificationAdapter;
import workflow.capstone.capstoneproject.adapter.RequestToHandleAdapter;
import workflow.capstone.capstoneproject.entities.Profile;
import workflow.capstone.capstoneproject.entities.RequestToHandle;
import workflow.capstone.capstoneproject.entities.RequestToHandlePaging;
import workflow.capstone.capstoneproject.entities.UserNotification;
import workflow.capstone.capstoneproject.repository.CapstoneRepository;
import workflow.capstone.capstoneproject.repository.CapstoneRepositoryImpl;
import workflow.capstone.capstoneproject.utils.CallBackData;
import workflow.capstone.capstoneproject.utils.ConstantDataManager;
import workflow.capstone.capstoneproject.utils.DynamicWorkflowSharedPreferences;
import workflow.capstone.capstoneproject.utils.DynamicWorkflowUtils;

public class ListHandleRequestFragment extends Fragment {

    private CapstoneRepository capstoneRepository;
    private RequestToHandleAdapter requestToHandleAdapter;
    private List<RequestToHandle> requestToHandleList = new ArrayList<>();
    private ListView listView;
    private CircleImageView imgAvatar;
    private String token;
    private int numberOfPage = 1;
    private boolean isLoading = false;
    private boolean isNoNewData = false;
    private MyHandler handler;
    private View footerView;
    private int totalRecord;

    public ListHandleRequestFragment() {
        // Required empty public constructor
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        View view = inflater.inflate(R.layout.fragment_list_handle_request, container, false);

        initView(view);

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

        requestToHandleList.clear();
        isNoNewData = false;
        numberOfPage = 1;
        //load handle request when start app
        loadHandleRequest(1);

        //load more handle request
        if (totalRecord > 10) {
            loadMoreItems();
        }
        return view;
    }

    private void initView(View view) {
        handler = new MyHandler();
        listView = view.findViewById(R.id.list_notification);
        imgAvatar = view.findViewById(R.id.img_avatar);
        LayoutInflater inflater = (LayoutInflater) getActivity().getSystemService(Context.LAYOUT_INFLATER_SERVICE);
        footerView = inflater.inflate(R.layout.footer_listview_progressbar, null);
    }

    private void loadHandleRequest(final int page) {
        capstoneRepository = new CapstoneRepositoryImpl();
        capstoneRepository.getRequestsToHandleByPermission(token, page, ConstantDataManager.NUMBER_OF_RECORD, new CallBackData<RequestToHandlePaging>() {
            @Override
            public void onSuccess(RequestToHandlePaging requestToHandlePaging) {
                totalRecord = requestToHandlePaging.getTotalRecord();
                if (requestToHandlePaging.getRequests().size() == 0) {
                    isNoNewData = true;
                    listView.removeFooterView(footerView);
                } else if (page == 1) {
                    listView.removeFooterView(footerView);
                    requestToHandleList = requestToHandlePaging.getRequests();
                    if (getActivity() != null) {
                        requestToHandleAdapter = new RequestToHandleAdapter(requestToHandleList, getActivity());
                        listView.setAdapter(requestToHandleAdapter);
                    }
                    onItemClick();
                } else if (page != 1) {
                    listView.removeFooterView(footerView);
                    requestToHandleAdapter.AddListItemAdapter(requestToHandlePaging.getRequests());
                    onItemClick();
                }
            }

            @Override
            public void onFail(String message) {

            }
        });
    }

    private void loadMoreItems() {
        onItemClick();
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
                    loadHandleRequest(++numberOfPage);
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

    private void onItemClick() {
        listView.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> adapterView, View view, int position, long id) {
                Fragment fragment = new HandleRequestFragment();
                Bundle bundle = new Bundle();
                RequestToHandle requestToHandle = (RequestToHandle) adapterView.getItemAtPosition(position);
                String requestID = requestToHandle.getId();
                String requestActionID = requestToHandle.getRequestActionID();
                bundle.putString("requestID", requestID);
                bundle.putString("requestActionID", requestActionID);
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
