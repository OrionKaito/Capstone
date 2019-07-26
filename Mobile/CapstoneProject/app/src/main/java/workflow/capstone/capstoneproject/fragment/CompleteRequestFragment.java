package workflow.capstone.capstoneproject.fragment;


import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.ListView;
import android.widget.TextView;

import java.util.ArrayList;
import java.util.List;

import workflow.capstone.capstoneproject.R;
import workflow.capstone.capstoneproject.activity.MainActivity;
import workflow.capstone.capstoneproject.adapter.RequestResultAdapter;
import workflow.capstone.capstoneproject.entities.RequestResult;
import workflow.capstone.capstoneproject.entities.StaffResult;
import workflow.capstone.capstoneproject.repository.CapstoneRepository;
import workflow.capstone.capstoneproject.repository.CapstoneRepositoryImpl;
import workflow.capstone.capstoneproject.utils.CallBackData;
import workflow.capstone.capstoneproject.utils.ConstantDataManager;
import workflow.capstone.capstoneproject.utils.DynamicWorkflowSharedPreferences;
import workflow.capstone.capstoneproject.utils.DynamicWorkflowUtils;
import workflow.capstone.capstoneproject.utils.FragmentUtils;

public class CompleteRequestFragment extends Fragment {

    private ImageView imgBack;
    private TextView tvRequestStatus;
    private TextView tvWorkFlowName;
    private String token;
    private CapstoneRepository capstoneRepository;
    private ListView listViewStatusStaffHandle;
    private RequestResultAdapter requestResultAdapter;

    public CompleteRequestFragment() {
        // Required empty public constructor
    }


    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        View view = inflater.inflate(R.layout.fragment_complete_request, container, false);
        initView(view);

        imgBack.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                FragmentUtils.back(getActivity());
            }
        });

        token = DynamicWorkflowSharedPreferences.getStoreJWT(getContext(), ConstantDataManager.AUTHORIZATION_TOKEN);

        final Bundle bundle = getArguments();
        getRequestResult(bundle.getString("requestActionID"), bundle.getString("userNotificationID"));

        return view;
    }

    private void initView(View view) {
        imgBack = view.findViewById(R.id.img_Back);
        tvRequestStatus = view.findViewById(R.id.tv_request_status);
        tvWorkFlowName = view.findViewById(R.id.tv_workflow_name);
        listViewStatusStaffHandle = view.findViewById(R.id.list_status_staff_handle);
    }

    private void getRequestResult(String requestActionID, String userNotificationID) {
        capstoneRepository = new CapstoneRepositoryImpl();
        capstoneRepository.getRequestResult(token, requestActionID, userNotificationID, new CallBackData<RequestResult>() {
            @Override
            public void onSuccess(RequestResult requestResult) {
                tvWorkFlowName.setText(requestResult.getWorkFlowTemplateName());
                tvRequestStatus.setText(requestResult.getStatus());
                configListView(listViewStatusStaffHandle, requestResult.getStaffResult());

                capstoneRepository = new CapstoneRepositoryImpl();
                capstoneRepository.getNumberOfNotification(token, new CallBackData<String>() {
                    @Override
                    public void onSuccess(String s) {
                        if (Integer.parseInt(s) > 0) {
                            MainActivity.notificationBadge.setText(s);
                            MainActivity.notificationBadge.setVisibility(View.VISIBLE);
                        } else {
                            MainActivity.notificationBadge.setVisibility(View.INVISIBLE);
                        }
                    }

                    @Override
                    public void onFail(String message) {

                    }
                });
            }

            @Override
            public void onFail(String message) {

            }
        });
    }

    private void configListView(ListView listView, List<StaffResult> staffResultList) {
        if (getActivity() != null) {
            requestResultAdapter = new RequestResultAdapter(getActivity(), staffResultList);
            listView.setAdapter(requestResultAdapter);
            DynamicWorkflowUtils.setListViewHeightBasedOnChildren(listView);
        }
    }

}
