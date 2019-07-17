package workflow.capstone.capstoneproject.fragment;


import android.app.DatePickerDialog;
import android.content.Context;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.text.InputType;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.inputmethod.InputMethodManager;
import android.widget.Button;
import android.widget.DatePicker;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.TextView;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.Date;
import java.util.List;

import es.dmoral.toasty.Toasty;
import workflow.capstone.capstoneproject.R;
import workflow.capstone.capstoneproject.api.UpdateProfileModel;
import workflow.capstone.capstoneproject.customdialog.CustomDialog;
import workflow.capstone.capstoneproject.entities.Profile;
import workflow.capstone.capstoneproject.repository.CapstoneRepository;
import workflow.capstone.capstoneproject.repository.CapstoneRepositoryImpl;
import workflow.capstone.capstoneproject.utils.CallBackData;
import workflow.capstone.capstoneproject.utils.ConstantDataManager;
import workflow.capstone.capstoneproject.utils.DynamicWorkflowSharedPreferences;
import workflow.capstone.capstoneproject.utils.DynamicWorkflowUtils;
import workflow.capstone.capstoneproject.utils.FragmentUtils;

public class ViewProfileFragment extends Fragment {

    private ImageView imgBack;
    private ImageView imgEditFullname;
    private ImageView imgEditDob;
    private EditText edtFullName;
    private TextView tvEmail;
    private TextView tvBirthDay;
    private Button btnSave;
    private int dayOfBirth;
    private int monthOfBirth;
    private int yearOfBirth;
    private String strDateOfBirth;
    private CapstoneRepository capstoneRepository;
    private String token;
    private Profile profile;
    private String newDateOfBirth;

    public ViewProfileFragment() {
        // Required empty public constructor
    }


    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        View view = inflater.inflate(R.layout.fragment_view_profile, container, false);
        initView(view);
        token = DynamicWorkflowSharedPreferences.getStoreJWT(getContext(), ConstantDataManager.AUTHORIZATION_TOKEN);

        edtFullName.setInputType(InputType.TYPE_NULL);
        imgBack.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                FragmentUtils.back(getActivity());
            }
        });

        profile = DynamicWorkflowSharedPreferences.getStoredData(getContext(), ConstantDataManager.PROFILE_KEY, ConstantDataManager.PROFILE_NAME);
        edtFullName.setText(profile.getFullName());
        tvEmail.setText(profile.getEmail());
        try {
            Date date = new SimpleDateFormat("yyyy-MM-dd").parse(profile.getDateOfBirth());
            strDateOfBirth = new SimpleDateFormat("dd-MM-yyyy").format(date);

            dayOfBirth = date.getDate();
            monthOfBirth = date.getMonth();
            yearOfBirth = date.getYear() + 1900;
        } catch (ParseException e) {
            e.printStackTrace();
        }
        tvBirthDay.setText(strDateOfBirth);

        imgEditFullname.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                edtFullName.setInputType(InputType.TYPE_CLASS_TEXT);
                int pos = edtFullName.getText().length();
                edtFullName.setSelection(pos);
                edtFullName.requestFocus();
                InputMethodManager imm = (InputMethodManager) getContext().getSystemService(Context.INPUT_METHOD_SERVICE);
                imm.showSoftInput(edtFullName, InputMethodManager.SHOW_IMPLICIT);
            }
        });

        imgEditDob.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                DatePickerDialog datePickerDialog = new DatePickerDialog(getContext(), new DatePickerDialog.OnDateSetListener() {
                    @Override
                    public void onDateSet(DatePicker view, int year, int month, int dayOfMonth) {
                        Calendar calendar = Calendar.getInstance();
                        calendar.set(year, month, dayOfMonth);
                        SimpleDateFormat simpleDateFormat = new SimpleDateFormat("dd-MM-yyyy");
                        tvBirthDay.setText(simpleDateFormat.format(calendar.getTime()));
                    }
                }, yearOfBirth, monthOfBirth, dayOfBirth);
                datePickerDialog.show();
            }
        });

        btnSave.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

                boolean checkConnection = DynamicWorkflowUtils.isConnectingToInternet(getActivity());

                if (!checkConnection) {
                    configDialog(false, true, getResources().getString(R.string.no_network), false, getResources().getString(R.string.close));
                } else if (edtFullName.getText().toString().trim().isEmpty()) {
                    configDialog(false, true, getResources().getString(R.string.input_all_fields), false, getResources().getString(R.string.close));
                } else {

                    try {
                        Date date = new SimpleDateFormat("dd-MM-yyyy").parse(tvBirthDay.getText().toString());
                        newDateOfBirth = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss").format(date);
                    } catch (ParseException e) {
                        e.printStackTrace();
                    }

                    UpdateProfileModel model = new UpdateProfileModel(edtFullName.getText().toString(), newDateOfBirth);

                    capstoneRepository = new CapstoneRepositoryImpl();
                    capstoneRepository.updateProfile(getContext(), token, model, new CallBackData<String>() {
                        @Override
                        public void onSuccess(String s) {
                            profile.setFullName(edtFullName.getText().toString());

                            profile.setDateOfBirth(newDateOfBirth);
                            DynamicWorkflowSharedPreferences.storeData(getContext(), ConstantDataManager.PROFILE_KEY, ConstantDataManager.PROFILE_NAME, profile);
                            configDialog(true, false, getResources().getString(R.string.change_profile_success), true, getResources().getString(R.string.ok));
                        }

                        @Override
                        public void onFail(String message) {
                            Toasty.error(getContext(), message, Toasty.LENGTH_SHORT).show();
                        }
                    });
                }
            }
        });

        return view;
    }

    private void configDialog(boolean success, boolean fail, String message, final boolean check, String nextStep) {
        new CustomDialog(getActivity())
                .setImageSuccess(success)
                .setImageFail(fail)
                .setContentText(message)
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

    private void initView(View view) {
        imgBack = view.findViewById(R.id.img_Back);
        imgEditFullname = view.findViewById(R.id.img_edit_fullname);
        imgEditDob = view.findViewById(R.id.img_edit_dob);
        edtFullName = view.findViewById(R.id.edt_full_name);
        tvEmail = view.findViewById(R.id.tv_email);
        tvBirthDay = view.findViewById(R.id.tv_birth_day);
        btnSave = view.findViewById(R.id.btn_save);
    }

}
