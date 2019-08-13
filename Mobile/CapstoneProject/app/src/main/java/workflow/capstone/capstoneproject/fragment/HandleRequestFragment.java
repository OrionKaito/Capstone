package workflow.capstone.capstoneproject.fragment;


import android.app.DownloadManager;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.IntentFilter;
import android.net.Uri;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v7.app.AlertDialog;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.Window;
import android.widget.AdapterView;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.ListView;
import android.widget.TextView;

import com.kaopiz.kprogresshud.KProgressHUD;
import com.squareup.picasso.Picasso;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Date;
import java.util.List;

import es.dmoral.toasty.Toasty;
import workflow.capstone.capstoneproject.R;
import workflow.capstone.capstoneproject.adapter.CommentAdapter;
import workflow.capstone.capstoneproject.adapter.HandleFileNameAdapter;
import workflow.capstone.capstoneproject.api.ActionValueModel;
import workflow.capstone.capstoneproject.api.RequestApproveModel;
import workflow.capstone.capstoneproject.entities.Comment;
import workflow.capstone.capstoneproject.entities.Connection;
import workflow.capstone.capstoneproject.entities.HandleRequestForm;
import workflow.capstone.capstoneproject.entities.Profile;
import workflow.capstone.capstoneproject.entities.RequestFile;
import workflow.capstone.capstoneproject.entities.RequestValue;
import workflow.capstone.capstoneproject.entities.StaffRequestAction;
import workflow.capstone.capstoneproject.repository.CapstoneRepository;
import workflow.capstone.capstoneproject.repository.CapstoneRepositoryImpl;
import workflow.capstone.capstoneproject.utils.CallBackData;
import workflow.capstone.capstoneproject.utils.ConstantDataManager;
import workflow.capstone.capstoneproject.utils.DynamicWorkflowSharedPreferences;
import workflow.capstone.capstoneproject.utils.DynamicWorkflowUtils;
import workflow.capstone.capstoneproject.utils.FragmentUtils;
import workflow.capstone.capstoneproject.utils.KProgressHUDManager;

public class HandleRequestFragment extends Fragment {

    private TextView tvActionName;
    private ImageView imgSendComment;
    private EditText edtComment;
    private CapstoneRepository capstoneRepository;
    private String stringComment;
    private String fullName;
    private ListView listViewComment;
    private List<Comment> commentList = new ArrayList<>();
    private List<String> stringCommentList = new ArrayList<>();
    private CommentAdapter commentAdapter;
    private ListView listViewFileName;
    private List<String> fileNameList = new ArrayList<>();
    private HandleFileNameAdapter handleFileNameAdapter;

    private List<String> fileUrl = new ArrayList<>();
    private ImageView imgBack;
    private LinearLayout lnButton;
    private String token = null;
    private TextView tvInitiatorName;
    private TextView tvWorkFlowName;
    private DownloadManager downloadManager;
    private String requestActionID;
    private LinearLayout lnRequestForm;

    public HandleRequestFragment() {
        // Required empty public constructor
    }


    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_handle_request, container, false);

        initView(view);

        imgBack.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                FragmentUtils.back(getActivity());
            }
        });

        token = DynamicWorkflowSharedPreferences.getStoreJWT(getContext(), ConstantDataManager.AUTHORIZATION_TOKEN);

        final Bundle bundle = getArguments();
        requestActionID = bundle.getString("requestActionID");
        getHandleForm(requestActionID);

        imgSendComment.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                updateListComment();
            }
        });

        return view;
    }

    private void initView(View view) {
        tvActionName = view.findViewById(R.id.tv_action_title);
        imgSendComment = view.findViewById(R.id.img_send_comment);
        edtComment = view.findViewById(R.id.edt_comment);
        listViewComment = view.findViewById(R.id.list_comment);
        listViewFileName = view.findViewById(R.id.list_file_name);
        imgBack = view.findViewById(R.id.img_Back);
        lnButton = view.findViewById(R.id.ln_button);
        tvInitiatorName = view.findViewById(R.id.tv_initiator_name);
        tvWorkFlowName = view.findViewById(R.id.tv_name_of_workflow);
        lnRequestForm = view.findViewById(R.id.ln_request_form);
    }

    private void updateListComment() {
        stringComment = edtComment.getText().toString();
        stringCommentList.add(stringComment);

        Profile profile = DynamicWorkflowSharedPreferences.getStoredData(getContext(), ConstantDataManager.PROFILE_KEY, ConstantDataManager.PROFILE_NAME);
        fullName = profile.getFullName();

        //get current date
        Date currentTime = Calendar.getInstance().getTime();
        String date = new SimpleDateFormat("MMM dd yyyy' at 'hh:mm a").format(currentTime);
        Comment comment = new Comment(stringComment, fullName, date);

        edtComment.setText("");
        commentList.add(comment);
        if (getActivity() != null) {
            commentAdapter = new CommentAdapter(commentList, getActivity());
            commentAdapter.notifyDataSetChanged();
            listViewComment.setAdapter(commentAdapter);
            listViewComment.setVisibility(View.VISIBLE);
            DynamicWorkflowUtils.setListViewHeightBasedOnChildren(listViewComment);
            setListViewCommentOnLongClick(listViewComment);
        }
    }

    private void getListComment() {
        if (getActivity() != null) {
            commentAdapter = new CommentAdapter(commentList, getActivity());
            commentAdapter.notifyDataSetChanged();
            listViewComment.setAdapter(commentAdapter);
            listViewComment.setVisibility(View.VISIBLE);
            DynamicWorkflowUtils.setListViewHeightBasedOnChildren(listViewComment);
        }
    }

    private void setListViewCommentOnLongClick(ListView listView) {
        listView.setOnItemLongClickListener(new AdapterView.OnItemLongClickListener() {
            @Override
            public boolean onItemLongClick(AdapterView<?> parent, View view, int position, long id) {
                Toasty.success(getContext(), "This is long click", Toasty.LENGTH_SHORT).show();
                return true;
            }
        });
    }

    private void initFileNameListView() {
        if (getActivity() != null) {
            handleFileNameAdapter = new HandleFileNameAdapter(getActivity(), fileNameList);
            listViewFileName.setAdapter(handleFileNameAdapter);
            DynamicWorkflowUtils.setListViewHeightBasedOnChildren(listViewFileName);
            setOnClickListViewFileName(listViewFileName);
        }
    }

    private void setOnClickListViewFileName(final ListView listView) {
        listView.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
                final String fileNameUrl = fileUrl.get(position);
                String imageExtension = fileNameUrl.substring(fileNameUrl.lastIndexOf(".") + 1);
                boolean checkIsImage = DynamicWorkflowUtils.accept(imageExtension);

                //check nếu là image thì hiển thị, ngược lại download file
                if (checkIsImage) {
                    AlertDialog.Builder builder = new AlertDialog.Builder(getContext());
                    builder.setPositiveButton("Close", new DialogInterface.OnClickListener() {
                        @Override
                        public void onClick(DialogInterface dialog, int which) {
                            dialog.dismiss();
                        }
                    });
                    final AlertDialog dialog = builder.create();
                    LayoutInflater inflater = getLayoutInflater();
                    View dialogLayout = inflater.inflate(R.layout.image_dialog, null);
                    dialog.setView(dialogLayout);
                    dialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
                    dialog.setCancelable(false);
                    dialog.setOnShowListener(new DialogInterface.OnShowListener() {
                        @Override
                        public void onShow(DialogInterface d) {
                            ImageView image = dialog.findViewById(R.id.imageDialog);

                            Picasso.get()
                                    .load(fileNameUrl)
                                    .into(image);

                            LinearLayout.LayoutParams layoutParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WRAP_CONTENT, LinearLayout.LayoutParams.WRAP_CONTENT);
                            image.setLayoutParams(layoutParams);


                        }
                    });
                    dialog.show();
                } else {
                    new AlertDialog.Builder(getContext())
                            .setTitle("Download file.")
                            .setMessage("Are you sure you want to download this file?")
                            .setPositiveButton("Ok", new DialogInterface.OnClickListener() {
                                public void onClick(DialogInterface dialog, int which) {
                                    downloadFileWithDownloadManager(fileNameUrl);
                                    dialog.dismiss();
                                }
                            })
                            .setNegativeButton("Cancel", new DialogInterface.OnClickListener() {
                                @Override
                                public void onClick(DialogInterface dialog, int which) {
                                    dialog.dismiss();
                                }
                            })
                            .setCancelable(false)
                            .show();
                }
            }
        });
    }

    private void downloadFileWithDownloadManager(String fileNameUrl) {
        downloadManager = (DownloadManager) getContext().getSystemService(Context.DOWNLOAD_SERVICE);
        Uri uri = Uri.parse(fileNameUrl);
        DownloadManager.Request request = new DownloadManager.Request(uri);
        request.setNotificationVisibility(DownloadManager.Request.VISIBILITY_VISIBLE_NOTIFY_COMPLETED);
        downloadManager.enqueue(request);

        BroadcastReceiver onComplete = new BroadcastReceiver() {
            public void onReceive(Context ctxt, Intent intent) {
                Toasty.success(ctxt, "Download file successful!", Toasty.LENGTH_LONG).show();
            }
        };
        getContext().registerReceiver(onComplete, new IntentFilter(DownloadManager.ACTION_DOWNLOAD_COMPLETE));
    }

    private void getHandleForm(String requestActionID) {
        capstoneRepository = new CapstoneRepositoryImpl();
        capstoneRepository.getRequestHandleForm(token, requestActionID, new CallBackData<HandleRequestForm>() {
            @Override
            public void onSuccess(final HandleRequestForm handleRequestForm) {
                //set title
                tvActionName.setText(handleRequestForm.getWorkFlowTemplateActionName());

                final List<Connection> connectionList = handleRequestForm.getConnections();

                //set InitiatorName
                tvInitiatorName.setText(handleRequestForm.getInitiatorName());

                //set Workflow Name
                tvWorkFlowName.setText(handleRequestForm.getWorkFlowTemplateName());

                //get form
                List<RequestValue> requestValueUserList = handleRequestForm.getUserRequestAction().getRequestValues();
                for (RequestValue requestValue : requestValueUserList) {
                    LinearLayout linearLayout = new LinearLayout(getActivity());
                    LinearLayout.LayoutParams params = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.WRAP_CONTENT);
                    params.setMargins(0,5,0,5);
                    linearLayout.setLayoutParams(params);
                    linearLayout.setOrientation(LinearLayout.HORIZONTAL);
                    linearLayout.setBackgroundResource(R.color.white);
                    linearLayout.setWeightSum(10);

                    if (requestValue.getValue().isEmpty()) {
                        //label
                        TextView textViewKey = new TextView(getActivity());
                        textViewKey.setText(requestValue.getKey());
                        textViewKey.setTextSize(15.0f);
                        textViewKey.setTextColor(getResources().getColor(R.color.colortext));
                        textViewKey.setLayoutParams(new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.WRAP_CONTENT));
                        linearLayout.addView(textViewKey);
                    } else {
                        TextView textViewKey = new TextView(getActivity());
                        textViewKey.setText(requestValue.getKey());
                        textViewKey.setTextSize(15.0f);
                        textViewKey.setTextColor(getResources().getColor(R.color.colorAccent));
                        textViewKey.setLayoutParams(new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.WRAP_CONTENT, 7.0f));
                        linearLayout.addView(textViewKey);

                        TextView textViewName = new TextView(getActivity());
                        textViewName.setText(requestValue.getValue());
                        textViewName.setTextSize(18.0f);
                        textViewName.setTextColor(getResources().getColor(R.color.colortext));
                        textViewName.setLayoutParams(new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.WRAP_CONTENT, 3.0f));
                        linearLayout.addView(textViewName);
                    }

                    lnRequestForm.addView(linearLayout);
                }

                //get comment from api
                List<StaffRequestAction> staffRequestActionList = handleRequestForm.getStaffRequestActions();
                for (StaffRequestAction staffRequestAction : staffRequestActionList) {
                    List<RequestValue> requestValueStaffList = staffRequestAction.getRequestValues();
                    for (RequestValue requestValue : requestValueStaffList) {
                        String createDate = "";
                        try {
                            Date date = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss").parse(staffRequestAction.getCreateDate());
                            createDate = new SimpleDateFormat("MMM dd yyyy' at 'hh:mm a").format(date);
                        } catch (ParseException e) {
                            e.printStackTrace();
                        }
                        commentList.add(new Comment(requestValue.getValue(), staffRequestAction.getFullName(), createDate));
                    }
                }
                getListComment();


                //get file path
                List<RequestFile> requestFileList = handleRequestForm.getUserRequestAction().getRequestFiles();
                for (RequestFile requestFile : requestFileList) {
                    String fileName = requestFile.getPath().substring(requestFile.getPath().lastIndexOf("\\") + 1);
                    fileNameList.add(fileName);
                    fileUrl.add(ConstantDataManager.IMAGE_URL + requestFile.getPath().replace("\\", "/"));
                }

                initFileNameListView();

                //get dynamic button
                for (final Connection connection : connectionList) {
                    Button btn = new Button(getActivity());
                    btn.setText(connection.getConnectionTypeName());
                    btn.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            android.app.AlertDialog.Builder builder = new android.app.AlertDialog.Builder(getContext());
                            builder.setTitle(connection.getConnectionTypeName().toUpperCase());
                            builder.setMessage("Do you want to continue?");
                            builder.setCancelable(false);
                            builder.setPositiveButton("Ok", new DialogInterface.OnClickListener() {
                                @Override
                                public void onClick(DialogInterface dialog, int which) {
                                    handleRequest(handleRequestForm.getRequest().getId(), connection.getNextStepID());
                                    dialog.dismiss();
                                }
                            })
                                    .setNegativeButton("Cancel", new DialogInterface.OnClickListener() {
                                        @Override
                                        public void onClick(DialogInterface dialog, int which) {
                                            dialog.dismiss();
                                        }
                                    });
                            android.app.AlertDialog alertDialog = builder.create();
                            alertDialog.show();
                        }
                    });
                    lnButton.addView(btn);
                }
            }

            @Override
            public void onFail(String message) {

            }
        });
    }

    private void handleRequest(String requestID, String nextStepID) {
        List<ActionValueModel> actionValueModelList = new ArrayList<>();
        for (int i = 0; i < stringCommentList.size(); i++) {
            actionValueModelList.add(new ActionValueModel("comment " + i, stringCommentList.get(i)));
        }

        RequestApproveModel requestApproveModel = new RequestApproveModel();
        requestApproveModel.setRequestID(requestID);
        requestApproveModel.setRequestActionID(requestActionID);
        requestApproveModel.setNextStepID(nextStepID);
        requestApproveModel.setActionValueModels(actionValueModelList);

        final KProgressHUD progressHUD = KProgressHUDManager.showProgressBar(getContext());
        capstoneRepository = new CapstoneRepositoryImpl();
        capstoneRepository.approveRequest(token, requestApproveModel, new CallBackData<String>() {
            @Override
            public void onSuccess(String s) {
                FragmentUtils.back(getActivity());
                progressHUD.dismiss();
                Toasty.success(getContext(), R.string.handle_success, Toasty.LENGTH_SHORT).show();
            }

            @Override
            public void onFail(String message) {
                progressHUD.dismiss();
                Toasty.error(getContext(), message, Toasty.LENGTH_SHORT).show();
            }
        });
    }

}
