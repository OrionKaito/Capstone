package workflow.capstone.capstoneproject.fragment;


import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentTransaction;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.ListView;

import java.util.List;

import workflow.capstone.capstoneproject.R;
import workflow.capstone.capstoneproject.adapter.NotificationAdapter;
import workflow.capstone.capstoneproject.entities.UserNotification;
import workflow.capstone.capstoneproject.repository.CapstoneRepository;
import workflow.capstone.capstoneproject.repository.CapstoneRepositoryImpl;
import workflow.capstone.capstoneproject.utils.CallBackData;
import workflow.capstone.capstoneproject.utils.ConstantDataManager;
import workflow.capstone.capstoneproject.utils.DynamicWorkflowSharedPreferences;

public class ListCompleteRequestFragment extends Fragment {

    private CapstoneRepository capstoneRepository;
    private NotificationAdapter notificationAdapter;
    private List<UserNotification> notificationList;
    private ListView listView;
    private String token = null;

    public ListCompleteRequestFragment() {
        // Required empty public constructor
    }


    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        View view = inflater.inflate(R.layout.fragment_list_complete_request, container, false);

        listView = view.findViewById(R.id.list_request_history);
        token = DynamicWorkflowSharedPreferences.getStoreJWT(getContext(), ConstantDataManager.AUTHORIZATION_TOKEN);
        loadCompleteRequest();
        return view;
    }

    private void loadCompleteRequest() {
        String token = DynamicWorkflowSharedPreferences.getStoreJWT(getActivity(), ConstantDataManager.AUTHORIZATION_TOKEN);
        capstoneRepository = new CapstoneRepositoryImpl();
        capstoneRepository.getNotificationByType(token, 3, new CallBackData<List<UserNotification>>() {
            @Override
            public void onSuccess(List<UserNotification> userNotifications) {
                notificationList = userNotifications;
                if (getActivity() != null) {
                    notificationAdapter = new NotificationAdapter(notificationList, getActivity(), true);
                    listView.setAdapter(notificationAdapter);
                    onItemClick(listView);
                }
            }

            @Override
            public void onFail(String message) {

            }
        });
    }

    private void onItemClick(final ListView listView) {
        listView.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> adapterView, View view, int position, long id) {
                Fragment fragment = new CompleteRequestFragment();
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
