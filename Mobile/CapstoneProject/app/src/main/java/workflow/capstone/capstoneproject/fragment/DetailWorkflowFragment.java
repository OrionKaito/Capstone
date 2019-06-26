package workflow.capstone.capstoneproject.fragment;


import android.Manifest;
import android.app.Activity;
import android.app.AlertDialog;
import android.content.ClipData;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.net.Uri;
import android.os.Build;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.annotation.RequiresApi;
import android.support.v4.app.ActivityCompat;
import android.support.v4.app.Fragment;
import android.support.v4.content.ContextCompat;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.EditText;
import android.widget.GridView;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import com.kaopiz.kprogresshud.KProgressHUD;

import java.io.File;
import java.util.ArrayList;
import java.util.List;

import es.dmoral.toasty.Toasty;
import okhttp3.MediaType;
import okhttp3.MultipartBody;
import okhttp3.RequestBody;
import workflow.capstone.capstoneproject.R;
import workflow.capstone.capstoneproject.adapter.ListUploadImageAdapter;
import workflow.capstone.capstoneproject.api.ActionValue;
import workflow.capstone.capstoneproject.api.Request;
import workflow.capstone.capstoneproject.repository.CapstoneRepository;
import workflow.capstone.capstoneproject.repository.CapstoneRepositoryImpl;
import workflow.capstone.capstoneproject.utils.CallBackData;
import workflow.capstone.capstoneproject.utils.ConstantDataManager;
import workflow.capstone.capstoneproject.utils.DynamicWorkflowSharedPreferences;
import workflow.capstone.capstoneproject.utils.FragmentUtils;
import workflow.capstone.capstoneproject.utils.GetRealPathFromURI;
import workflow.capstone.capstoneproject.utils.KProgressHUDManager;

public class DetailWorkflowFragment extends Fragment {

    private ImageView imgBack;
    private ImageView imgUploadFile;
    private ImageView imgUploadImage;
    private EditText edtReason;
    private Button btnSend;
    private TextView tvNameOfWorkFlow;
    private TextView tvUploadSuccess;
    private GridView gridViewImage;
    private CapstoneRepository capstoneRepository;
    private String token = null;
    private List<String> listImageAbsolutePath = new ArrayList<>();
    private ListUploadImageAdapter listUploadImageAdapter;
    private List<String> listPath = new ArrayList<>();
    private List<Uri> uriList = new ArrayList<>();
    private MultipartBody.Part[] fileParts;

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

        imgUploadFile.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Intent intent = new Intent(Intent.ACTION_GET_CONTENT);
                intent.setType("*/*");
                startActivityForResult(intent, ConstantDataManager.PICK_FILE_REQUEST);
            }
        });

        imgUploadImage.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Intent intent = new Intent(Intent.ACTION_PICK);
                intent.setType("image/*");
                intent.putExtra(Intent.EXTRA_ALLOW_MULTIPLE, true);
                startActivityForResult(intent, ConstantDataManager.PICK_IMAGE_REQUEST);
            }
        });

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

    @RequiresApi(api = Build.VERSION_CODES.KITKAT)
    @Override
    public void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        if (resultCode == Activity.RESULT_OK) {
            switch (requestCode) {
                case ConstantDataManager.PICK_FILE_REQUEST:
                    if (data == null) {
                        Toasty.error(getContext(), R.string.data_null, Toasty.LENGTH_SHORT);
                        return;
                    }

                    Uri selectedFileUri = data.getData();
                    String realPath = GetRealPathFromURI.getPath(getActivity(), selectedFileUri);
                    if (realPath != null && !realPath.isEmpty()) {
                        final File file = new File(realPath);
                        RequestBody requestFile = RequestBody.create(MediaType.parse(getActivity().getContentResolver().getType(selectedFileUri)), file);
                        MultipartBody.Part multipartBody = MultipartBody.Part.createFormData("file", file.getName(), requestFile);

                        capstoneRepository = new CapstoneRepositoryImpl();
                        capstoneRepository.postRequestFile(token, multipartBody, new CallBackData<String[]>() {
                            @Override
                            public void onSuccess(String[] strings) {
                                Toast.makeText(getContext(), "File Success: " + file.getName(), Toast.LENGTH_SHORT).show();
                                for (int i = 0; i < strings.length; i++) {
                                    listPath.add(strings[i]);
                                }
                            }

                            @Override
                            public void onFail(String message) {

                            }
                        });
                    }
                    break;
                case ConstantDataManager.PICK_IMAGE_REQUEST:
                    if (data == null) {
                        Toasty.error(getContext(), R.string.data_null, Toasty.LENGTH_SHORT);
                        return;
                    }

                    //multiple image
                    handleMultipleImage(data);
                    break;
            }
        }
    }

    private void initView(View view) {
        imgBack = view.findViewById(R.id.img_Back);
        edtReason = view.findViewById(R.id.edt_Reason);
        btnSend = view.findViewById(R.id.btn_Send);
        tvNameOfWorkFlow = view.findViewById(R.id.tv_name_of_workflow);
        imgUploadFile = view.findViewById(R.id.img_upload_file);
        imgUploadImage = view.findViewById(R.id.img_upload_image);
        tvUploadSuccess = view.findViewById(R.id.tv_upload_success);
        gridViewImage = view.findViewById(R.id.grid_view_image);
    }

    @RequiresApi(api = Build.VERSION_CODES.JELLY_BEAN)
    private void handleMultipleImage(Intent data) {
        ClipData clipData = data.getClipData();
        for (int i = 0; i < clipData.getItemCount(); i++) {
            ClipData.Item item = clipData.getItemAt(i);
            Uri uri = item.getUri();
            uriList.add(uri);
        }

        fileParts = new MultipartBody.Part[uriList.size()];
        for (int i = 0; i < uriList.size(); i++) {
            Uri selectedFileUriImage = uriList.get(i);
            File file = new File(GetRealPathFromURI.getPath(getActivity(), selectedFileUriImage));
            if (!listImageAbsolutePath.contains(file.getAbsolutePath())) {
                listImageAbsolutePath.add(file.getAbsolutePath());
            }
            // Khởi tạo RequestBody từ những file đã được chọn
            RequestBody requestBody = RequestBody.create(MediaType.parse(getActivity().getContentResolver().getType(selectedFileUriImage)), file);
            // Add thêm request body vào trong builder
            MultipartBody.Part multipartBody = MultipartBody.Part.createFormData("picture" + i, file.getName(), requestBody);
            fileParts[i] = multipartBody;
        }

        capstoneRepository = new CapstoneRepositoryImpl();
        capstoneRepository.postMultipleRequestFile(token, fileParts, new CallBackData<String[]>() {
            @Override
            public void onSuccess(String[] strings) {
                for (int i = 0; i < strings.length; i++) {
                    listPath.add(strings[i]);
                }
                configGridView();
            }

            @Override
            public void onFail(String message) {
                Toasty.error(getContext(), message, Toasty.LENGTH_SHORT);
            }
        });
    }

    private void configGridView() {
        listUploadImageAdapter = new ListUploadImageAdapter(getContext(), listImageAbsolutePath);
        gridViewImage.setAdapter(listUploadImageAdapter);
        gridViewImage.setVisibility(View.VISIBLE);
    }

    private void sendRequest(String workFlowTemplateID) {
        if (token != null) {
            ActionValue actionValue = new ActionValue();
            actionValue.setKey("text");
            actionValue.setValue(edtReason.getText().toString());

            List<ActionValue> actionValues = new ArrayList<>();
            actionValues.add(actionValue);

            Request request = new Request();
            request.setDescription("Xin nghi hoc");
            request.setWorkFlowTemplateID(workFlowTemplateID);
            request.setNextStepID("5cdf2701-6af0-4a6b-8d79-08d6f8a56d9f");
            request.setStatus(1);
            request.setActionValues(actionValues);
            request.setImagePaths(listPath);

            final KProgressHUD khub = KProgressHUDManager.showProgressBar(getContext());
            capstoneRepository = new CapstoneRepositoryImpl();
            capstoneRepository.postRequest(token, request, new CallBackData<String>() {
                @Override
                public void onSuccess(String s) {
                    FragmentUtils.back(getActivity());
                    khub.dismiss();
                    Toasty.success(getContext(), R.string.request_sent, Toast.LENGTH_SHORT).show();
                }

                @Override
                public void onFail(String message) {
//                    Toasty.error(getContext(), message);
                }
            });
        }
    }

    public void readStoragePermissionGranted() {
        if (Build.VERSION.SDK_INT >= 23) {
            if (ContextCompat.checkSelfPermission(getContext(), Manifest.permission.READ_EXTERNAL_STORAGE)
                    == PackageManager.PERMISSION_GRANTED) {
            } else {
                requestStoragePermission();
            }
        }
    }

    private void requestStoragePermission() {
        if (ActivityCompat.shouldShowRequestPermissionRationale(getActivity(), Manifest.permission.READ_EXTERNAL_STORAGE)) {
            new AlertDialog.Builder(getContext())
                    .setTitle(R.string.permission_needed)
                    .setMessage(R.string.permission_required_message)
                    .setPositiveButton(R.string.ok, new DialogInterface.OnClickListener() {
                        @Override
                        public void onClick(DialogInterface dialog, int which) {
                            ActivityCompat.requestPermissions(getActivity(), new String[]{Manifest.permission.READ_EXTERNAL_STORAGE}, ConstantDataManager.MY_PERMISSIONS_READ_EXTERNAL_STORAGE);
                        }
                    })
                    .setNegativeButton(R.string.cancel, new DialogInterface.OnClickListener() {
                        @Override
                        public void onClick(DialogInterface dialog, int which) {
                            dialog.dismiss();
                        }
                    }).create().show();
        } else {
            ActivityCompat.requestPermissions(getActivity(), new String[]{Manifest.permission.READ_EXTERNAL_STORAGE}, ConstantDataManager.MY_PERMISSIONS_READ_EXTERNAL_STORAGE);
        }
    }

    @Override
    public void onRequestPermissionsResult(int requestCode, @NonNull String[] permissions, @NonNull int[] grantResults) {
        super.onRequestPermissionsResult(requestCode, permissions, grantResults);
        if (requestCode == ConstantDataManager.MY_PERMISSIONS_READ_EXTERNAL_STORAGE) {
            if (grantResults.length > 0 && grantResults[0] == PackageManager.PERMISSION_GRANTED) {
                Toasty.success(getContext(), R.string.permission_granted, Toasty.LENGTH_SHORT);
            } else {
                Toasty.warning(getContext(), R.string.permission_not_granted, Toasty.LENGTH_SHORT);
            }
        }
    }
}
