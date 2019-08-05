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

import com.squareup.picasso.Picasso;

import java.util.ArrayList;
import java.util.List;

import de.hdodenhof.circleimageview.CircleImageView;
import workflow.capstone.capstoneproject.R;
import workflow.capstone.capstoneproject.activity.ProfileActivity;
import workflow.capstone.capstoneproject.adapter.NotificationAdapter;
import workflow.capstone.capstoneproject.entities.Profile;
import workflow.capstone.capstoneproject.entities.UserNotification;
import workflow.capstone.capstoneproject.entities.UserNotificationPaging;
import workflow.capstone.capstoneproject.repository.CapstoneRepository;
import workflow.capstone.capstoneproject.repository.CapstoneRepositoryImpl;
import workflow.capstone.capstoneproject.utils.CallBackData;
import workflow.capstone.capstoneproject.utils.ConstantDataManager;
import workflow.capstone.capstoneproject.utils.DynamicWorkflowSharedPreferences;

public class ListNotificationFragment extends Fragment {

    private CapstoneRepository capstoneRepository;
    private NotificationAdapter notificationAdapter;
    private List<UserNotification> notificationList = new ArrayList<>();
    private ListView listView;
    private String token;
    private CircleImageView imgAvatar;
    private int numberOfPage = 1;
    private boolean isLoading = false;
    private boolean isNoNewData = false;
    private MyHandler handler;
    private View footerView;
    private int totalRecord;

    public ListNotificationFragment() {
        // Required empty public constructor
    }


    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        View view = inflater.inflate(R.layout.fragment_list_notification, container, false);
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

        notificationList.clear();
        isNoNewData = false;
        numberOfPage = 1;
        //load notification when start app
        loadNotification(1);

        //load more notification
        if (totalRecord > 10) {
            loadMoreItems();
        }
        return view;
    }

    private void initView(View view) {
        handler = new MyHandler();
        listView = view.findViewById(R.id.list_request_history);
        imgAvatar = view.findViewById(R.id.img_avatar);
        LayoutInflater inflater = (LayoutInflater) getActivity().getSystemService(Context.LAYOUT_INFLATER_SERVICE);
        footerView = inflater.inflate(R.layout.footer_listview_progressbar, null);
    }

    private void loadNotification(final int page) {
        capstoneRepository = new CapstoneRepositoryImpl();
        capstoneRepository.getNotification(token, page, ConstantDataManager.NUMBER_OF_RECORD, new CallBackData<UserNotificationPaging>() {
            @Override
            public void onSuccess(UserNotificationPaging userNotificationPaging) {
                totalRecord = userNotificationPaging.getTotalRecord();
                if (userNotificationPaging.getUserNotifications().size() == 0) {
                    isNoNewData = true;
                    listView.removeFooterView(footerView);
                } else if (page == 1) {
                    listView.removeFooterView(footerView);
                    notificationList = userNotificationPaging.getUserNotifications();
                    if (getActivity() != null) {
                        notificationAdapter = new NotificationAdapter(notificationList, getActivity());
                        listView.setAdapter(notificationAdapter);
                    }
//                    DynamicWorkflowUtils.setListViewHeightBasedOnChildren(listView);
                    onItemClick();
                } else if (page != 1) {
                    listView.removeFooterView(footerView);
                    notificationAdapter.AddListItemAdapter(userNotificationPaging.getUserNotifications());
//                    DynamicWorkflowUtils.setListViewHeightBasedOnChildren(listView);
//                    onItemClick();
                }
            }

            @Override
            public void onFail(String message) {

            }
        });
    }

    private void loadMoreItems() {
//        onItemClick();
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
                    loadNotification(++numberOfPage);
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
                Fragment fragment = new DetailRequestFragment();
                Bundle bundle = new Bundle();
                UserNotification userNotification = (UserNotification) adapterView.getItemAtPosition(position);
                String requestActionID = userNotification.getEventID();
                String userNotificationID = userNotification.getUserNotificationID();
                bundle.putString("requestActionID", requestActionID);
                bundle.putString("userNotificationID", userNotificationID);
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
