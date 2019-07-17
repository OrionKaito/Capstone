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
import android.support.v4.app.Fragment;
import android.support.v4.content.ContextCompat;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.ListView;
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
import workflow.capstone.capstoneproject.adapter.ListFileNameAdapter;
import workflow.capstone.capstoneproject.api.ActionValue;
import workflow.capstone.capstoneproject.api.Request;
import workflow.capstone.capstoneproject.entities.Connection;
import workflow.capstone.capstoneproject.entities.RequestForm;
import workflow.capstone.capstoneproject.repository.CapstoneRepository;
import workflow.capstone.capstoneproject.repository.CapstoneRepositoryImpl;
import workflow.capstone.capstoneproject.utils.CallBackData;
import workflow.capstone.capstoneproject.utils.ConstantDataManager;
import workflow.capstone.capstoneproject.utils.DynamicWorkflowSharedPreferences;
import workflow.capstone.capstoneproject.utils.DynamicWorkflowUtils;
import workflow.capstone.capstoneproject.utils.FragmentUtils;
import workflow.capstone.capstoneproject.utils.GetRealPathFromURI;
import workflow.capstone.capstoneproject.utils.KProgressHUDManager;

public class SendRequestFragment extends Fragment {

    private LinearLayout lnButton;
    private ImageView imgBack;
    private ImageView imgUploadFile;
    private ImageView imgUploadImage;
    private EditText edtReason;
    private TextView tvNameOfWorkFlow;
    private TextView tvAttachment;
    private ListView listView;
    private CapstoneRepository capstoneRepository;
    private String token = null;
    private List<String> listName = new ArrayList<>();
    private ListFileNameAdapter listFileNameAdapter;
    private List<String> listPath = new ArrayList<>();
    private List<Uri> uriList = new ArrayList<>();
    private MultipartBody.Part[] fileParts;
    private int checkToGo = 0;

    public SendRequestFragment() {
        // Required empty public constructor
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        final View view = inflater.inflate(R.layout.fragment_send_request, container, false);
        initView(view);
        final Bundle bundle = getArguments();

        imgBack.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                FragmentUtils.back(getActivity());
            }
        });

        token = DynamicWorkflowSharedPreferences.getStoreJWT(getContext(), ConstantDataManager.AUTHORIZATION_TOKEN);

        buildDynamicForm(bundle.getString("workFlowTemplateID"));

        imgUploadFile.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                checkToGo = 2;
                readStoragePermissionGranted();
            }
        });

        imgUploadImage.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                checkToGo = 1;
                readStoragePermissionGranted();
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
                        Toasty.warning(getContext(), R.string.data_null, Toasty.LENGTH_SHORT).show();
                        return;
                    }

                    Uri selectedFileUri = data.getData();
                    String realPath = GetRealPathFromURI.getPath(getActivity(), selectedFileUri);
                    if (realPath != null && !realPath.isEmpty()) {
                        final File file = new File(realPath);

                        if (!listName.contains(file.getName())) {
                            listName.add(file.getName());
                        }

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
                                configListView();
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
        lnButton = view.findViewById(R.id.ln_button);
        imgBack = view.findViewById(R.id.img_Back);
        edtReason = view.findViewById(R.id.edt_Reason);
        tvNameOfWorkFlow = view.findViewById(R.id.tv_name_of_workflow);
        imgUploadFile = view.findViewById(R.id.img_upload_file);
        imgUploadImage = view.findViewById(R.id.img_upload_image);
        listView = view.findViewById(R.id.list_file_name);
        tvAttachment = view.findViewById(R.id.tv_attachment);
    }

    private void buildDynamicForm(final String workFlowTemplateID) {
        capstoneRepository = new CapstoneRepositoryImpl();
        capstoneRepository.getRequestForm(token, workFlowTemplateID, new CallBackData<RequestForm>() {
            @Override
            public void onSuccess(RequestForm requestForm) {
                final List<Connection> connectionList = requestForm.getConnections();
                for (final Connection connection : connectionList) {
                    Button btn = new Button(getContext());
                    btn.setText(connection.getConnectionTypeName());
                    btn.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            AlertDialog.Builder builder = new AlertDialog.Builder(getContext());
                            builder.setTitle(connection.getConnectionTypeName().toUpperCase());
                            builder.setMessage("Do you want to continue?");
                            builder.setCancelable(false);
                            builder.setPositiveButton("Ok", new DialogInterface.OnClickListener() {
                                @Override
                                public void onClick(DialogInterface dialog, int which) {
                                    sendRequest(workFlowTemplateID, connection.getNextStepID());
                                    dialog.dismiss();
                                }
                            })
                                    .setNegativeButton("Cancel", new DialogInterface.OnClickListener() {
                                        @Override
                                        public void onClick(DialogInterface dialog, int which) {
                                            dialog.dismiss();
                                        }
                                    });
                            AlertDialog alertDialog = builder.create();
                            alertDialog.show();
                        }
                    });
                    lnButton.addView(btn);
                }
            }

            @Override
            public void onFail(String message) {
                Toasty.error(getContext(), message, Toasty.LENGTH_SHORT).show();
            }
        });
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
            if (!listName.contains(file.getName())) {
                listName.add(file.getName());
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
                configListView();
            }

            @Override
            public void onFail(String message) {
                Toasty.error(getContext(), message, Toasty.LENGTH_SHORT).show();
            }
        });
    }

    private void configListView() {
        if (listName.size() > 0) {
            tvAttachment.setVisibility(View.VISIBLE);
        }
        if (getActivity() != null) {
            listFileNameAdapter = new ListFileNameAdapter(getActivity(), listName);
            listView.setAdapter(listFileNameAdapter);
            listView.setVisibility(View.VISIBLE);
            DynamicWorkflowUtils.setListViewHeightBasedOnChildren(listView);
        }
    }

    private void sendRequest(String workFlowTemplateID, String nextStepID) {
        if (token != null) {
            ActionValue actionValue = new ActionValue("text", edtReason.getText().toString());

            List<ActionValue> actionValues = new ArrayList<>();
            actionValues.add(actionValue);

            Request request = new Request();
            request.setDescription("Xin nghi hoc");
            request.setWorkFlowTemplateID(workFlowTemplateID);
            request.setNextStepID(nextStepID);
            request.setStatus(1);
            request.setActionValues(actionValues);
            request.setImagePaths(listPath);

            final KProgressHUD progressHUD = KProgressHUDManager.showProgressBar(getContext());
            capstoneRepository = new CapstoneRepositoryImpl();
            capstoneRepository.postRequest(token, request, new CallBackData<String>() {
                @Override
                public void onSuccess(String s) {
                    FragmentUtils.back(getActivity());
                    progressHUD.dismiss();
                    Toasty.success(getContext(), R.string.request_sent, Toasty.LENGTH_SHORT).show();
                }

                @Override
                public void onFail(String message) {
                    progressHUD.dismiss();
                    Toasty.error(getContext(), message, Toasty.LENGTH_SHORT).show();
                }
            });
        }
    }

    public void readStoragePermissionGranted() {
        if (Build.VERSION.SDK_INT >= 23) {
            if (ContextCompat.checkSelfPermission(getContext(), Manifest.permission.READ_EXTERNAL_STORAGE)
                    == PackageManager.PERMISSION_GRANTED) {
                if (checkToGo == 1) {
                    Intent intent = new Intent(Intent.ACTION_PICK);
                    intent.setType("image/*");
                    intent.putExtra(Intent.EXTRA_ALLOW_MULTIPLE, true);
                    startActivityForResult(intent, ConstantDataManager.PICK_IMAGE_REQUEST);
                } else if (checkToGo == 2) {
                    Intent intent = new Intent(Intent.ACTION_GET_CONTENT);
                    intent.setType("*/*");
                    startActivityForResult(intent, ConstantDataManager.PICK_FILE_REQUEST);
                }
            } else {
                requestStoragePermission();
            }
        }
    }

    private void requestStoragePermission() {
        if (shouldShowRequestPermissionRationale(Manifest.permission.READ_EXTERNAL_STORAGE)) {
            new AlertDialog.Builder(getContext())
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
