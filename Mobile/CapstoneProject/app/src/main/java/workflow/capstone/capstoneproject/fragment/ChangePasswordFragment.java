package workflow.capstone.capstoneproject.fragment;


import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;

import workflow.capstone.capstoneproject.R;
import workflow.capstone.capstoneproject.activity.ConfirmForgotPasswordActivity;
import workflow.capstone.capstoneproject.activity.ProfileActivity;
import workflow.capstone.capstoneproject.customdialog.CustomDialog;
import workflow.capstone.capstoneproject.repository.CapstoneRepository;
import workflow.capstone.capstoneproject.repository.CapstoneRepositoryImpl;
import workflow.capstone.capstoneproject.utils.CallBackData;
import workflow.capstone.capstoneproject.utils.ConstantDataManager;
import workflow.capstone.capstoneproject.utils.DynamicWorkflowSharedPreferences;
import workflow.capstone.capstoneproject.utils.FragmentUtils;

public class ChangePasswordFragment extends Fragment {

    private EditText edtPassword;
    private EditText edtConfirmPassword;
    private ImageView imgBack;
    private Button btnChangePassword;
    private CapstoneRepository capstoneRepository;
    private String token;

    public ChangePasswordFragment() {
        // Required empty public constructor
    }


    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        View view = inflater.inflate(R.layout.fragment_change_password, container, false);
        initView(view);
        token = DynamicWorkflowSharedPreferences.getStoreJWT(getContext(), ConstantDataManager.AUTHORIZATION_TOKEN);

        ProfileActivity.tvProfileTitle.setText("Change Password");
//        imgBack.setOnClickListener(new View.OnClickListener() {
//            @Override
//            public void onClick(View v) {
//                FragmentUtils.back(getActivity());
//            }
//        });

        btnChangePassword.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                String password = edtPassword.getText().toString();
                String confirmPassword = edtConfirmPassword.getText().toString();
                if(password.trim().isEmpty() || confirmPassword.trim().isEmpty()) {
                    configDialog(false, true, "Please input all of field!", false, getResources().getString(R.string.close));
                } else if(!password.equals(confirmPassword)) {
                    configDialog(false, true, "Confirm Password do not match with Password!", false, getResources().getString(R.string.close));
                } else {
                    capstoneRepository = new CapstoneRepositoryImpl();
                    capstoneRepository.changePassword(getContext(), token, password, new CallBackData<String>() {
                        @Override
                        public void onSuccess(String s) {
                            configDialog(true, false, getResources().getString(R.string.change_password_success), true, getResources().getString(R.string.ok));
                        }

                        @Override
                        public void onFail(String message) {

                        }
                    });
                }
            }
        });

        return view;
    }

    private void initView(View view) {
        edtPassword = view.findViewById(R.id.edt_password);
        edtConfirmPassword = view.findViewById(R.id.edt_confirm_password);
//        imgBack = view.findViewById(R.id.img_Back);
        btnChangePassword = view.findViewById(R.id.btn_change_pass);
    }

    private void configDialog(boolean success, boolean fail, String message, final boolean check, String nextStep) {
        new CustomDialog(getContext())
                .setContentText(message)
                .setImageSuccess(success)
                .setImageFail(fail)
                .setConfirmText(nextStep)
                .setConfirmClickListener(new CustomDialog.OnCustomClickListener() {
                    @Override
                    public void onClick(CustomDialog customDialog) {
                        if (check) {
                            customDialog.dismiss();
                            FragmentUtils.back(getActivity());
                        } else {
                            customDialog.dismiss();
                        }
                    }
                })
                .show();
    }

}
