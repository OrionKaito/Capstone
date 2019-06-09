package workflow.capstone.capstoneproject.fragment;


import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ListView;

import java.util.List;

import workflow.capstone.capstoneproject.R;
import workflow.capstone.capstoneproject.adapter.NotificationAdapter;
import workflow.capstone.capstoneproject.entities.Notification;
import workflow.capstone.capstoneproject.entities.Workflow;
import workflow.capstone.capstoneproject.repository.CapstoneRepository;
import workflow.capstone.capstoneproject.repository.CapstoneRepositoryImpl;
import workflow.capstone.capstoneproject.utils.CallBackData;
import workflow.capstone.capstoneproject.utils.ConstantDataManager;
import workflow.capstone.capstoneproject.utils.DynamicWorkflowSharedPreferences;

public class NotificationFragment extends Fragment {

    private CapstoneRepository capstoneRepository;
    private NotificationAdapter notificationAdapter;
    private List<Notification> notificationList;
    private ListView listView;

    public NotificationFragment() {
        // Required empty public constructor
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        View view = inflater.inflate(R.layout.fragment_workflow, container, false);
        loadNotifications(view);
        return view;
    }

    private void loadNotifications(final View view) {
        String token = DynamicWorkflowSharedPreferences.getStoreJWT(getContext(), ConstantDataManager.AUTHORIZATION_TOKEN);
        capstoneRepository = new CapstoneRepositoryImpl();
        capstoneRepository.getNotification(token, new CallBackData<List<Notification>>() {
            @Override
            public void onSuccess(List<Notification> notifications) {
                listView = view.findViewById(R.id.list);
                notificationList = notifications;
                notificationAdapter = new NotificationAdapter(notificationList, getContext());
                listView.setAdapter(notificationAdapter);
            }

            @Override
            public void onFail(String message) {

            }
        });
    }

}
