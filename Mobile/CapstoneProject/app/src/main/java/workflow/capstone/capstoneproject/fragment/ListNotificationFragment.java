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
import android.widget.ListView;

import com.squareup.picasso.Picasso;

import java.util.List;

import de.hdodenhof.circleimageview.CircleImageView;
import workflow.capstone.capstoneproject.R;
import workflow.capstone.capstoneproject.activity.ProfileActivity;
import workflow.capstone.capstoneproject.adapter.NotificationAdapter;
import workflow.capstone.capstoneproject.entities.Profile;
import workflow.capstone.capstoneproject.entities.UserNotification;
import workflow.capstone.capstoneproject.repository.CapstoneRepository;
import workflow.capstone.capstoneproject.repository.CapstoneRepositoryImpl;
import workflow.capstone.capstoneproject.utils.CallBackData;
import workflow.capstone.capstoneproject.utils.ConstantDataManager;
import workflow.capstone.capstoneproject.utils.DynamicWorkflowSharedPreferences;

public class ListNotificationFragment extends Fragment {

    private CapstoneRepository capstoneRepository;
    private NotificationAdapter notificationAdapter;
    private List<UserNotification> notificationList;
    private ListView listView;
    private String token = null;
    private CircleImageView imgAvatar;

    public ListNotificationFragment() {
        // Required empty public constructor
    }


    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        View view = inflater.inflate(R.layout.fragment_list_notification, container, false);
        listView = view.findViewById(R.id.list_request_history);
        imgAvatar = view.findViewById(R.id.img_avatar);

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

        token = DynamicWorkflowSharedPreferences.getStoreJWT(getContext(), ConstantDataManager.AUTHORIZATION_TOKEN);
        loadNotification();
        return view;
    }

    private void loadNotification() {
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
