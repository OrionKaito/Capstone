package workflow.capstone.capstoneproject.fragment;


import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.ListView;
import android.widget.TextView;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;
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

public class DetailRequestFragment extends Fragment {

    private ImageView imgBack;
    private TextView tvRequestStatus;
    private TextView tvWorkFlowName;
    private String token;
    private CapstoneRepository capstoneRepository;
    private ListView listViewStatusStaffHandle;
    private RequestResultAdapter requestResultAdapter;
    private TextView tvHistoryApprove;
    private TextView tvCreateDate;

    public DetailRequestFragment() {
        // Required empty public constructor
    }


    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        View view = inflater.inflate(R.layout.fragment_detail_request, container, false);
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

        String createDate = "";

        try {
            Date date = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss").parse(bundle.getString("createDate"));
            createDate = new SimpleDateFormat("MMM dd yyyy' at 'hh:mm a").format(date);
        } catch (ParseException e) {
            e.printStackTrace();
        }
        tvCreateDate.setText(createDate);

        return view;
    }

    private void initView(View view) {
        imgBack = view.findViewById(R.id.img_Back);
        tvRequestStatus = view.findViewById(R.id.tv_request_status);
        tvWorkFlowName = view.findViewById(R.id.tv_workflow_name_title);
        listViewStatusStaffHandle = view.findViewById(R.id.list_status_staff_handle);
        tvHistoryApprove = view.findViewById(R.id.tv_history_approve);
        tvCreateDate = view.findViewById(R.id.tv_create_date);
    }

    private void getRequestResult(String requestActionID, String userNotificationID) {
        capstoneRepository = new CapstoneRepositoryImpl();
        capstoneRepository.getRequestResult(token, requestActionID, userNotificationID, new CallBackData<RequestResult>() {
            @Override
            public void onSuccess(RequestResult requestResult) {
                tvWorkFlowName.setText(requestResult.getWorkFlowTemplateName());
                tvRequestStatus.setText(requestResult.getStatus());
//                if (requestResult.getStaffResult().isEmpty()) {
//                    tvHistoryApprove.setVisibility(View.GONE);
//                } else {
                configListView(requestResult.getStaffResult());
//                }
            }

            @Override
            public void onFail(String message) {

            }
        });
    }

    private void configListView(List<StaffResult> staffResultList) {
        if (getActivity() != null) {
            requestResultAdapter = new RequestResultAdapter(getActivity(), staffResultList);
            listViewStatusStaffHandle.setAdapter(requestResultAdapter);
            listViewStatusStaffHandle.setClickable(false);
//            DynamicWorkflowUtils.setListViewHeightBasedOnChildren(listViewStatusStaffHandle);
        }
    }

}
