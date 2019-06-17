package workflow.capstone.capstoneproject.fragment;


import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import workflow.capstone.capstoneproject.R;
import workflow.capstone.capstoneproject.activity.MainActivity;
import workflow.capstone.capstoneproject.entities.Request;
import workflow.capstone.capstoneproject.entities.RequestAction;
import workflow.capstone.capstoneproject.repository.CapstoneRepository;
import workflow.capstone.capstoneproject.repository.CapstoneRepositoryImpl;
import workflow.capstone.capstoneproject.utils.CallBackData;
import workflow.capstone.capstoneproject.utils.ConstantDataManager;
import workflow.capstone.capstoneproject.utils.DynamicWorkflowSharedPreferences;
import workflow.capstone.capstoneproject.utils.FragmentUtils;

public class DetailWorkflowFragment extends Fragment {

    private ImageView imgBack;
    private EditText edtReason;
    private Button btnSend;
    private TextView tvNameOfWorkFlow;
    private CapstoneRepository capstoneRepository;
    private String token = null;

    public DetailWorkflowFragment() {
        // Required empty public constructor
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        final View view = inflater.inflate(R.layout.fragment_detail_workflow, container, false);
        initView(view);
        final Bundle bundle = getArguments();

        imgBack.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                FragmentUtils.back(getActivity());
            }
        });

        token = DynamicWorkflowSharedPreferences.getStoreJWT(getContext(), ConstantDataManager.AUTHORIZATION_TOKEN);

        btnSend.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                sendRequest(bundle.getString("workFlowTemplateID"));
//                Toast.makeText(getContext(), "workFlowTemplateID: " + bundle.getString("workFlowTemplateID"), Toast.LENGTH_SHORT).show();
            }
        });

        tvNameOfWorkFlow.setText(bundle.getString("nameOfWorkflow"));
        return view;
    }

    private void initView(View view) {
        imgBack = view.findViewById(R.id.img_Back);
        edtReason = view.findViewById(R.id.edt_Reason);
        btnSend = view.findViewById(R.id.btn_Send);
        tvNameOfWorkFlow = view.findViewById(R.id.tv_name_of_workflow);
    }

    private void sendRequest(String workFlowTemplateID) {
        if (token != null) {
            capstoneRepository = new CapstoneRepositoryImpl();
            capstoneRepository.postRequest("test", workFlowTemplateID, token, new CallBackData<String>() {
                @Override
                public void onSuccess(String id) {
                    postRequestAction(id);
                }

                @Override
                public void onFail(String message) {

                }
            });
        }
    }

    private void postRequestAction(String id) {
        if (token != null) {
            capstoneRepository = new CapstoneRepositoryImpl();
            RequestAction requestAction = new RequestAction();
            requestAction.setStatus("Status");
            requestAction.setRequestID(id);
            requestAction.setNextStepID("5e4de477-7ad1-4a30-ce3f-08d6f3400065");
            capstoneRepository.postRequestAction(requestAction, token, new CallBackData<String>() {
                @Override
                public void onSuccess(String id) {
                    postRequestValue(id);
                }

                @Override
                public void onFail(String message) {

                }
            });
        }
    }

    private void postRequestValue(String id) {
        if (token != null) {
            capstoneRepository = new CapstoneRepositoryImpl();
            capstoneRepository.postRequestValue(edtReason.getText().toString(), id, token, new CallBackData<String>() {
                @Override
                public void onSuccess(String s) {
                    Toast.makeText(getContext(), "Success", Toast.LENGTH_SHORT).show();
                    FragmentUtils.changeFragment(getActivity(), R.id.page_one_fragment, new WorkflowFragment());
                }

                @Override
                public void onFail(String message) {

                }
            });
        }
    }

}
