package workflow.capstone.capstoneproject.fragment;


import android.Manifest;
import android.annotation.SuppressLint;
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
import android.view.Gravity;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.ListView;
import android.widget.Scroller;
import android.widget.Spinner;
import android.widget.TextView;

import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;
import com.kaopiz.kprogresshud.KProgressHUD;

import java.io.File;
import java.lang.reflect.Type;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import es.dmoral.toasty.Toasty;
import okhttp3.MediaType;
import okhttp3.MultipartBody;
import okhttp3.RequestBody;
import workflow.capstone.capstoneproject.R;
import workflow.capstone.capstoneproject.adapter.ListFileNameAdapter;
import workflow.capstone.capstoneproject.api.ActionValueModel;
import workflow.capstone.capstoneproject.api.RequestModel;
import workflow.capstone.capstoneproject.entities.Connection;
import workflow.capstone.capstoneproject.entities.DynamicForm.DynamicForm;
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
    private LinearLayout lnDynamicForm;
    private TextView tvActionName;
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
    private Map<String, View> idsMap = new HashMap<>();

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

        buildForm(bundle.getString("workFlowTemplateID"));

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
        tvActionName = view.findViewById(R.id.tv_action_title);
        lnDynamicForm = view.findViewById(R.id.ln_dynamic_form);
        lnButton = view.findViewById(R.id.ln_button);
        imgBack = view.findViewById(R.id.img_Back);
        edtReason = view.findViewById(R.id.edt_Reason);
        tvNameOfWorkFlow = view.findViewById(R.id.tv_name_of_workflow);
        imgUploadFile = view.findViewById(R.id.img_upload_file);
        imgUploadImage = view.findViewById(R.id.img_upload_image);
        listView = view.findViewById(R.id.list_file_name);
        tvAttachment = view.findViewById(R.id.tv_attachment);
    }

    private void buildForm(final String workFlowTemplateID) {
        capstoneRepository = new CapstoneRepositoryImpl();
        capstoneRepository.getRequestForm(token, workFlowTemplateID, new CallBackData<RequestForm>() {
            @Override
            public void onSuccess(final RequestForm requestForm) {
                //set title
                tvActionName.setText(requestForm.getWorkFlowTemplateActionName());

                //Build Dynamic form
                buildDynamicForm(requestForm.getActionType().getData());

                final List<Connection> connectionList = requestForm.getConnections();
                for (final Connection connection : connectionList) {
                    Button btn = new Button(getActivity());
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
                                    sendRequest(workFlowTemplateID, connection.getNextStepID(), requestForm.getWorkFlowTemplateActionID(), requestForm.getActionType().getData());
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

    @SuppressLint("ResourceType")
    private void buildDynamicForm(String data) {
        Type type = new TypeToken<List<DynamicForm>>() {
        }.getType();
        List<DynamicForm> dynamicFormList = new Gson().fromJson(data, type);

        for (int i = 0; i < dynamicFormList.size(); i++) {
            if (!dynamicFormList.get(i).getTextOnly().getName().isEmpty()) {
                LinearLayout linearLayoutLabel = new LinearLayout(getActivity());
                LinearLayout.LayoutParams params = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.WRAP_CONTENT);
                params.setMargins(0,5,0,5);
                linearLayoutLabel.setLayoutParams(params);
                linearLayoutLabel.setOrientation(LinearLayout.VERTICAL);
                linearLayoutLabel.setBackgroundResource(R.color.white);

                TextView textView = new TextView(getActivity());
                textView.setText(dynamicFormList.get(i).getTextOnly().getName());
                textView.setTextSize(17.0f);
                textView.setTextColor(getResources().getColor(R.color.colortext));

                putIdToMap(textView, i);

                linearLayoutLabel.addView(textView);
                lnDynamicForm.addView(linearLayoutLabel);

//                View lineView = new View(getActivity());
//                LinearLayout.LayoutParams viewParams = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MATCH_PARENT, 1);
//                viewParams.setMargins(10,10,10,10);
//                lineView.setLayoutParams(viewParams);
//                lineView.setBackgroundColor(getResources().getColor(R.color.colortext));
//                lnDynamicForm.addView(lineView);

            } else if (!dynamicFormList.get(i).getShortText().getName().isEmpty()) {
                EditText editText = new EditText(getActivity());
                //set layout_weight cho edittext
                editText.setLayoutParams(new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.WRAP_CONTENT, 3.0f));
                editText.setBackgroundColor(getResources().getColor(R.color.edit_text_background));
                editText.setBackgroundResource(R.drawable.border_radius);

                putIdToMap(editText, i);

                configView(dynamicFormList.get(i).getShortText().getName(), editText);
            } else if (!dynamicFormList.get(i).getLongText().getName().isEmpty()) {
                EditText editText = new EditText(getActivity());
                //set layout_weight cho edittext
                editText.setLayoutParams(new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.WRAP_CONTENT, 3.0f));
                editText.setScroller(new Scroller(getContext()));
                editText.setVerticalScrollBarEnabled(true);
                editText.setMinLines(5);
                editText.setMaxLines(5);
                editText.setGravity(Gravity.TOP);
                editText.setBackgroundColor(getResources().getColor(R.color.edit_text_background));
                editText.setBackgroundResource(R.drawable.border_radius);
                putIdToMap(editText, i);

                configView(dynamicFormList.get(i).getLongText().getName(), editText);
            } else if (!dynamicFormList.get(i).getInputCheckbox().getName().isEmpty()) {
                CheckBox checkBox = new CheckBox(getActivity());
                //set layout_weight cho checkBox
                checkBox.setLayoutParams(new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.WRAP_CONTENT, 3.0f));

                putIdToMap(checkBox, i);

                configView(dynamicFormList.get(i).getInputCheckbox().getName(), checkBox);
            } else if (!dynamicFormList.get(i).getComboBox().getName().isEmpty()) {
                List<String> listOption = dynamicFormList.get(i).getComboBox().getValueOfProper();
                Spinner spinner = new Spinner(getActivity());
                spinner.setLayoutParams(new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.WRAP_CONTENT, 3.0f));
                ArrayAdapter<String> adapter = new ArrayAdapter<>(getActivity(), android.R.layout.simple_spinner_dropdown_item, listOption);
                spinner.setAdapter(adapter);
                putIdToMap(spinner, i);

                configView(dynamicFormList.get(i).getComboBox().getName(), spinner);
            }
        }
    }

    private void putIdToMap(View view, Integer i) {
        String name = "task" + i;
        view.setId(i);
        idsMap.put(name, view);
    }

    private void configView(String textViewName, View view2) {
        LinearLayout linearLayout = new LinearLayout(getActivity());
        LinearLayout.LayoutParams params = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.WRAP_CONTENT);
        params.setMargins(0,5,0,5);
        linearLayout.setLayoutParams(params);
        linearLayout.setOrientation(LinearLayout.HORIZONTAL);
        linearLayout.setBackgroundResource(R.color.white);
        linearLayout.setWeightSum(10);

        TextView textView = new TextView(getActivity());
        //set layout_weight cho textview
        textView.setLayoutParams(new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.WRAP_CONTENT, 7.0f));
        textView.setText(textViewName);
        textView.setTextSize(17.0f);
        textView.setTextColor(getResources().getColor(R.color.colorAccent));

        //add child view to linear layout
        linearLayout.addView(textView);
        linearLayout.addView(view2);
        lnDynamicForm.addView(linearLayout);

        //add line view underline linear layout
//        View lineView = new View(getActivity());
//        LinearLayout.LayoutParams viewParams = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MATCH_PARENT, 2);
//        viewParams.setMargins(10,10,10,10);
//        lineView.setLayoutParams(viewParams);
//        lineView.setBackgroundColor(getResources().getColor(R.color.colortext));
//        lnDynamicForm.addView(lineView);
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

    private void sendRequest(String workFlowTemplateID, String nextStepID, String workFlowTemplateActionID, String data) {
        if (token != null) {
            Type type = new TypeToken<List<DynamicForm>>() {
            }.getType();
            List<DynamicForm> dynamicFormList = new Gson().fromJson(data, type);
            List<ActionValueModel> actionValueModelList = new ArrayList<>();
            for (int i = 0; i < dynamicFormList.size(); i++) {
                String name = "task" + i;
                if (!dynamicFormList.get(i).getTextOnly().getName().isEmpty()) {
                    TextView textView = (TextView) idsMap.get(name);
                    ActionValueModel actionValueModel = new ActionValueModel(dynamicFormList.get(i).getTextOnly().getName(), textView.getText().toString());
                    actionValueModelList.add(actionValueModel);
                } else if (!dynamicFormList.get(i).getShortText().getName().isEmpty()) {
                    EditText editText = (EditText) idsMap.get(name);
                    ActionValueModel actionValueModel = new ActionValueModel(dynamicFormList.get(i).getShortText().getName(), editText.getText().toString());
                    actionValueModelList.add(actionValueModel);
                } else if (!dynamicFormList.get(i).getLongText().getName().isEmpty()) {
                    EditText editText = (EditText) idsMap.get(name);
                    ActionValueModel actionValueModel = new ActionValueModel(dynamicFormList.get(i).getLongText().getName(), editText.getText().toString());
                    actionValueModelList.add(actionValueModel);
                } else if (!dynamicFormList.get(i).getInputCheckbox().getName().isEmpty()) {
                    CheckBox checkBox = (CheckBox) idsMap.get(name);
                    ActionValueModel actionValueModel = new ActionValueModel(dynamicFormList.get(i).getInputCheckbox().getName(), checkBox.isChecked() + "");
                    actionValueModelList.add(actionValueModel);
                } else if (!dynamicFormList.get(i).getComboBox().getName().isEmpty()) {
                    Spinner spinner = (Spinner) idsMap.get(name);
                    ActionValueModel actionValueModel = new ActionValueModel(dynamicFormList.get(i).getComboBox().getName(), spinner.getSelectedItem().toString());
                    actionValueModelList.add(actionValueModel);
                }
            }

            RequestModel requestModel = new RequestModel();
            requestModel.setDescription("");
            requestModel.setWorkFlowTemplateID(workFlowTemplateID);
            requestModel.setWorkFlowTemplateActionID(workFlowTemplateActionID);
            requestModel.setNextStepID(nextStepID);
            requestModel.setActionValues(actionValueModelList);
            requestModel.setImagePaths(listPath);

            final KProgressHUD progressHUD = KProgressHUDManager.showProgressBar(getContext());
            capstoneRepository = new CapstoneRepositoryImpl();
            capstoneRepository.postRequest(token, requestModel, new CallBackData<String>() {
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
