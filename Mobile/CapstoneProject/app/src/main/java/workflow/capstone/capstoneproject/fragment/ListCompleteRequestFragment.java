package workflow.capstone.capstoneproject.fragment;


import android.content.Intent;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentTransaction;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.ImageView;
import android.widget.ListView;

import com.squareup.picasso.Picasso;

import java.util.List;

import de.hdodenhof.circleimageview.CircleImageView;
import workflow.capstone.capstoneproject.R;
import workflow.capstone.capstoneproject.activity.MainActivity;
import workflow.capstone.capstoneproject.activity.ProfileActivity;
import workflow.capstone.capstoneproject.adapter.NotificationAdapter;
import workflow.capstone.capstoneproject.entities.Profile;
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
    private ImageView imgMenu;
    private CircleImageView imgAvatar;

    public ListCompleteRequestFragment() {
        // Required empty public constructor
    }


    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        View view = inflater.inflate(R.layout.fragment_list_complete_request, container, false);
        listView = view.findViewById(R.id.list_request_history);
        imgAvatar = view.findViewById(R.id.img_avatar);

        Profile profile = DynamicWorkflowSharedPreferences.getStoredData(getContext(), ConstantDataManager.PROFILE_KEY, ConstantDataManager.PROFILE_NAME);
        Picasso.get()
                .load(ConstantDataManager.IMAGE_URL + profile.getImagePath())
                .error(getResources().getDrawable(R.drawable.avatar))
                .into(imgAvatar);

        imgAvatar.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(getActivity(), ProfileActivity.class);
                startActivity(intent);
                getActivity().overridePendingTransition(R.anim.slide_in_right, R.anim.slide_out_left);
            }
        });

//        imgMenu = view.findViewById(R.id.img_menu);
//        imgMenu.setOnClickListener(new View.OnClickListener() {
//            @Override
//            public void onClick(View v) {
//                Intent intent = new Intent(getActivity(), ProfileActivity.class);
//                startActivity(intent);
//                getActivity().overridePendingTransition(R.anim.slide_in_right, R.anim.slide_out_left);
//            }
//        });

        token = DynamicWorkflowSharedPreferences.getStoreJWT(getContext(), ConstantDataManager.AUTHORIZATION_TOKEN);
        loadCompleteRequest();
        return view;
    }

    private void loadCompleteRequest() {
        String token = DynamicWorkflowSharedPreferences.getStoreJWT(getActivity(), ConstantDataManager.AUTHORIZATION_TOKEN);
        capstoneRepository = new CapstoneRepositoryImpl();
        capstoneRepository.getNotification(token, new CallBackData<List<UserNotification>>() {
            @Override
            public void onSuccess(List<UserNotification> userNotifications) {
                notificationList = userNotifications;
                if (getActivity() != null) {
                    notificationAdapter = new NotificationAdapter(notificationList, getActivity(), true);
                    listView.setAdapter(notificationAdapter);
                }
//                onItemClick(listView);
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
