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
import android.widget.TextView;

import com.squareup.picasso.Picasso;

import java.util.ArrayList;
import java.util.List;

import de.hdodenhof.circleimageview.CircleImageView;
import workflow.capstone.capstoneproject.R;
import workflow.capstone.capstoneproject.activity.ProfileActivity;
import workflow.capstone.capstoneproject.adapter.MyRequestAdapter;
import workflow.capstone.capstoneproject.entities.MyRequest;
import workflow.capstone.capstoneproject.entities.RequestToHandle;
import workflow.capstone.capstoneproject.entities.UserNotification;
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
    private TextView tvEmptyRequest;

    public MyRequestFragment() {
        // Required empty public constructor
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        View view = inflater.inflate(R.layout.fragment_my_request, container, false);

        imgAvatar = view.findViewById(R.id.img_avatar);
        Picasso.get().load("https://scontent.fsgn2-4.fna.fbcdn.net/v/t1.0-1/p160x160/51544827_1431384577001877_5331970394951778304_n.jpg?_nc_cat=109&_nc_oc=AQnco7rDhQqwfiIMn0yb5w1T_XbHhK4H7VHH2OkcvvJwPffe8ztui6o1jgmD0HV70sM_obUhA5ESdSz-trY9uwGu&_nc_ht=scontent.fsgn2-4.fna&oh=efcc572eee6bb9b41bc554297c98a4d6&oe=5DAD14A0")
                .into(imgAvatar);
        imgAvatar.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(getActivity(), ProfileActivity.class);
                startActivity(intent);
                getActivity().overridePendingTransition(R.anim.slide_in_right, R.anim.slide_out_left);
            }
        });
        loadMyRequest(view);
        return view;
    }

    private void loadMyRequest(View view) {
        listView = view.findViewById(R.id.list_my_request);
        tvEmptyRequest = view.findViewById(R.id.tv_empty_request);
        String token = DynamicWorkflowSharedPreferences.getStoreJWT(getActivity(), ConstantDataManager.AUTHORIZATION_TOKEN);
        capstoneRepository = new CapstoneRepositoryImpl();
        capstoneRepository.getMyRequest(token, new CallBackData<List<MyRequest>>() {
            @Override
            public void onSuccess(List<MyRequest> myRequests) {
                myRequestList = myRequests;
                myRequestAdapter = new MyRequestAdapter(myRequestList, getActivity());
                listView.setAdapter(myRequestAdapter);
//                DynamicWorkflowUtils.setListViewHeightBasedOnChildren(listView);
                onItemCLick(listView);
                if(myRequestList.isEmpty()) {
                    tvEmptyRequest.setVisibility(View.VISIBLE);
                }
            }

            @Override
            public void onFail(String message) {

            }
        });
    }

    private void onItemCLick(ListView listView) {
        listView.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> adapterView, View view, int position, long id) {
                Fragment fragment = new CompleteRequestFragment();
                Bundle bundle = new Bundle();
                MyRequest myRequest = (MyRequest) adapterView.getItemAtPosition(position);
                String requestActionID = myRequest.getCurrentRequestActionID();
                bundle.putString("requestActionID", requestActionID);
                bundle.putString("userNotificationID", null);
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
