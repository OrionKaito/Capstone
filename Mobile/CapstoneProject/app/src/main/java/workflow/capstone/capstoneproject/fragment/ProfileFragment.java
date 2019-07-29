package workflow.capstone.capstoneproject.fragment;


import android.Manifest;
import android.app.Activity;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.net.Uri;
import android.os.Build;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.annotation.RequiresApi;
import android.support.v4.app.Fragment;
import android.support.v4.content.ContextCompat;
import android.support.v7.app.AlertDialog;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.TextView;
import android.widget.Toast;

import com.google.android.gms.tasks.OnSuccessListener;
import com.google.firebase.iid.FirebaseInstanceId;
import com.google.firebase.iid.InstanceIdResult;
import com.squareup.picasso.Picasso;

import java.io.File;
import java.util.List;

import es.dmoral.toasty.Toasty;
import okhttp3.MediaType;
import okhttp3.MultipartBody;
import okhttp3.RequestBody;
import workflow.capstone.capstoneproject.R;
import workflow.capstone.capstoneproject.activity.LoginActivity;
import workflow.capstone.capstoneproject.activity.MainActivity;
import workflow.capstone.capstoneproject.activity.ProfileActivity;
import workflow.capstone.capstoneproject.entities.Profile;
import workflow.capstone.capstoneproject.repository.CapstoneRepository;
import workflow.capstone.capstoneproject.repository.CapstoneRepositoryImpl;
import workflow.capstone.capstoneproject.utils.CallBackData;
import workflow.capstone.capstoneproject.utils.ConstantDataManager;
import workflow.capstone.capstoneproject.utils.DynamicWorkflowSharedPreferences;
import workflow.capstone.capstoneproject.utils.FragmentUtils;
import workflow.capstone.capstoneproject.utils.GetRealPathFromURI;

public class ProfileFragment extends Fragment {

    private LinearLayout lnViewProfile;
    private LinearLayout lnChangePassword;
    private LinearLayout lnSignOut;
    private TextView tvFullName;
    private CapstoneRepository capstoneRepository;
    private String token;
    private String deviceToken;
    private ImageView imgAvatar;
    private ImageView imgChangeAvatar;
    private Profile profile;

    public ProfileFragment() {
        // Required empty public constructor
    }


    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        View view = inflater.inflate(R.layout.fragment_profile, container, false);

        profile = DynamicWorkflowSharedPreferences.getStoredData(getContext(), ConstantDataManager.PROFILE_KEY, ConstantDataManager.PROFILE_NAME);

        initView(view);

        Picasso.get()
                .load(ConstantDataManager.IMAGE_URL + profile.getImagePath())
                .error(getResources().getDrawable(R.drawable.avatar))
                .into(imgAvatar);

        ProfileActivity.tvProfileTitle.setText("Profile");

        tvFullName.setText(profile.getFullName());

        lnViewProfile.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                viewProfile();
            }
        });

        lnChangePassword.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                changePassword();
            }
        });

        lnSignOut.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                logout();
            }
        });

        imgChangeAvatar.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                readStoragePermissionGranted();
            }
        });

        return view;
    }

    private void initView(View view) {
        tvFullName = view.findViewById(R.id.tv_full_name);
        lnViewProfile = view.findViewById(R.id.ln_view_profile);
        lnChangePassword = view.findViewById(R.id.ln_change_password);
        lnSignOut = view.findViewById(R.id.ln_sign_out);
        imgAvatar = view.findViewById(R.id.img_avatar);
        imgChangeAvatar = view.findViewById(R.id.img_changeAvatar);
    }

    private void logout() {
        token = DynamicWorkflowSharedPreferences.getStoreJWT(getContext(), ConstantDataManager.AUTHORIZATION_TOKEN);
        FirebaseInstanceId.getInstance().getInstanceId()
                .addOnSuccessListener(new OnSuccessListener<InstanceIdResult>() {
                    @Override
                    public void onSuccess(InstanceIdResult instanceIdResult) {

                        // Get new Instance ID token
                        deviceToken = instanceIdResult.getToken();
                    }
                });

        if (deviceToken == null) {
            deviceToken = FirebaseInstanceId.getInstance().getToken();
        }

        AlertDialog.Builder builder = new AlertDialog.Builder(getContext());
        builder.setCancelable(false);
        builder.setMessage(R.string.logout_confirm)
                .setPositiveButton(R.string.sign_out, new DialogInterface.OnClickListener() {
                    public void onClick(DialogInterface dialog, int id) {
                        capstoneRepository = new CapstoneRepositoryImpl();
                        capstoneRepository.logout(token, deviceToken, new CallBackData<String>() {
                            @Override
                            public void onSuccess(String s) {
                                DynamicWorkflowSharedPreferences.removeJWT(getContext());
                                Intent intent = new Intent(getActivity(), LoginActivity.class);
                                intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK | Intent.FLAG_ACTIVITY_CLEAR_TASK);
                                startActivity(intent);
                                getActivity().finish();
                            }

                            @Override
                            public void onFail(String message) {
                                Toasty.error(getActivity(), message, Toasty.LENGTH_SHORT).show();
                            }
                        });
                    }
                })
                .setNegativeButton(R.string.cancel, new DialogInterface.OnClickListener() {
                    public void onClick(DialogInterface dialog, int id) {
                        dialog.dismiss();
                    }
                });
        AlertDialog dialog = builder.create();
        dialog.show();
    }

    private void viewProfile() {
        FragmentUtils.changeFragment(getActivity(), R.id.profile_frame, new ViewProfileFragment());
    }

    private void changePassword() {
        FragmentUtils.changeFragment(getActivity(), R.id.profile_frame, new ChangePasswordFragment());
    }

    @RequiresApi(api = Build.VERSION_CODES.KITKAT)
    @Override
    public void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        if (resultCode == Activity.RESULT_OK) {
            switch (requestCode) {
                case ConstantDataManager.PICK_IMAGE_REQUEST:
                    if (data == null) {
                        Toasty.warning(getContext(), R.string.data_null, Toasty.LENGTH_SHORT).show();
                        return;
                    }

                    Uri selectedFileUri = data.getData();
                    String realPath = GetRealPathFromURI.getPath(getActivity(), selectedFileUri);
                    if (realPath != null && !realPath.isEmpty()) {
                        final File file = new File(realPath);

                        RequestBody requestFile = RequestBody.create(MediaType.parse(getActivity().getContentResolver().getType(selectedFileUri)), file);
                        MultipartBody.Part multipartBody = MultipartBody.Part.createFormData("picture", file.getName(), requestFile);

                        capstoneRepository = new CapstoneRepositoryImpl();
                        capstoneRepository.postRequestFile(token, multipartBody, new CallBackData<String[]>() {
                            @Override
                            public void onSuccess(final String[] strings) {
                                capstoneRepository = new CapstoneRepositoryImpl();
                                capstoneRepository.updateAvatar(getContext(), token, strings[0], new CallBackData<String>() {
                                    @Override
                                    public void onSuccess(String s) {
                                        profile.setImagePath(strings[0]);
                                        Toasty.success(getContext(), s, Toasty.LENGTH_SHORT).show();
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
                    break;
            }
        }
    }

    private void readStoragePermissionGranted() {
        if (Build.VERSION.SDK_INT >= 23) {
            if (ContextCompat.checkSelfPermission(getContext(), Manifest.permission.READ_EXTERNAL_STORAGE)
                    == PackageManager.PERMISSION_GRANTED) {
                Intent intent = new Intent(Intent.ACTION_PICK);
                intent.setType("image/*");
                startActivityForResult(intent, ConstantDataManager.PICK_IMAGE_REQUEST);
            } else {
                requestStoragePermission();
            }
        }
    }

    private void requestStoragePermission() {
        if (shouldShowRequestPermissionRationale(Manifest.permission.READ_EXTERNAL_STORAGE)) {
            new android.app.AlertDialog.Builder(getContext())
                    .setTitle(R.string.permission_needed)
                    .setMessage(R.string.permission_required_message)
                    .setPositiveButton(R.string.ok, new DialogInterface.OnClickListener() {
                        @Override
                        public void onClick(DialogInterface dialog, int which) {
                            requestPermissions(new String[]{Manifest.permission.READ_EXTERNAL_STORAGE}, ConstantDataManager.MY_PERMISSIONS_READ_EXTERNAL_STORAGE);
                        }
                    })
                    .setNegativeButton(R.string.cancel, new DialogInterface.OnClickListener() {
                        @Override
                        public void onClick(DialogInterface dialog, int which) {
                            dialog.dismiss();
                        }
                    }).create().show();
        } else {
            requestPermissions(new String[]{Manifest.permission.READ_EXTERNAL_STORAGE}, ConstantDataManager.MY_PERMISSIONS_READ_EXTERNAL_STORAGE);
        }
    }

    @Override
    public void onRequestPermissionsResult(int requestCode, @NonNull String[] permissions, @NonNull int[] grantResults) {
        super.onRequestPermissionsResult(requestCode, permissions, grantResults);
        if (requestCode == ConstantDataManager.MY_PERMISSIONS_READ_EXTERNAL_STORAGE) {
            if (grantResults.length > 0 && grantResults[0] == PackageManager.PERMISSION_GRANTED) {
                Toasty.success(getContext(), R.string.permission_granted, Toasty.LENGTH_SHORT).show();
            } else {
                Toasty.warning(getContext(), R.string.permission_not_granted, Toasty.LENGTH_SHORT).show();
            }
        }
    }

}
