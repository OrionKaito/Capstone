package workflow.capstone.capstoneproject.fragment;


import android.app.Activity;
import android.content.ClipData;
import android.content.Intent;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.net.Uri;
import android.os.Build;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.annotation.RequiresApi;
import android.support.design.widget.BottomSheetDialogFragment;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.LinearLayout;
import android.widget.Toast;

import java.io.File;
import java.util.List;

import okhttp3.MediaType;
import okhttp3.MultipartBody;
import okhttp3.RequestBody;
import workflow.capstone.capstoneproject.R;
import workflow.capstone.capstoneproject.entities.UploadFilePath;
import workflow.capstone.capstoneproject.repository.CapstoneRepository;
import workflow.capstone.capstoneproject.repository.CapstoneRepositoryImpl;
import workflow.capstone.capstoneproject.utils.CallBackData;
import workflow.capstone.capstoneproject.utils.ConstantDataManager;
import workflow.capstone.capstoneproject.utils.DynamicWorkflowSharedPreferences;
import workflow.capstone.capstoneproject.utils.GetRealPathFromURI;

public class BottomSheetFragment extends BottomSheetDialogFragment {

    private LinearLayout imageChoosen;
    private LinearLayout fileChoosen;
    private static final int PICK_FILE_REQUEST = 1;
    private static final int PICK_IMAGE_REQUEST = 2;
    private CapstoneRepository capstoneRepository;
    private String token = null;
    private List<Uri> uriList;

    public BottomSheetFragment() {
        // Required empty public constructor
    }

    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        View view = inflater.inflate(R.layout.fragment_bottem_sheet, container, false);
        imageChoosen = view.findViewById(R.id.image_choosen);
        fileChoosen = view.findViewById(R.id.file_choosen);

        token = DynamicWorkflowSharedPreferences.getStoreJWT(getContext(), ConstantDataManager.AUTHORIZATION_TOKEN);

        imageChoosen.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Intent intent = new Intent(Intent.ACTION_PICK);
                intent.setType("image/*");
                intent.putExtra(Intent.EXTRA_ALLOW_MULTIPLE, true);
                startActivityForResult(intent, PICK_IMAGE_REQUEST);
            }
        });

        fileChoosen.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Intent intent = new Intent(Intent.ACTION_GET_CONTENT);
                intent.setType("*/*");
                startActivityForResult(intent, PICK_FILE_REQUEST);
            }
        });
        return view;
    }

    @RequiresApi(api = Build.VERSION_CODES.JELLY_BEAN)
    @Override
    public void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        if (resultCode == Activity.RESULT_OK) {
            switch (requestCode) {
                case PICK_IMAGE_REQUEST:
                    if (data == null) {
                        Toast.makeText(getActivity(), "Data null!", Toast.LENGTH_SHORT).show();
                        return;
                    }

                    //multiple image
//                    ClipData clipData = data.getClipData();
//                    StringBuilder builder = new StringBuilder();
//                    for (int i = 0; i < clipData.getItemCount(); i++) {
//                        ClipData.Item item = clipData.getItemAt(i);
//                        Uri uri = item.getUri();
//                        uriList.add(uri);
//                        builder.append(i + "-")
//                                .append(GetRealPathFromURI.getPath(getActivity(), uri))
//                                .append("\n");
//                    }
//
//                    Uri selectedFileUri = data.getData();
//                    String realPath = GetRealPathFromURI.getPath(getActivity(), selectedFileUri);
//                    if (realPath != null && !realPath.isEmpty()) {
//                        final File file = new File(realPath);
//                        RequestBody requestFile = RequestBody.create(MediaType.parse(getActivity().getContentResolver().getType(selectedFileUri)), file);
//                        MultipartBody.Part multipartBody = MultipartBody.Part.createFormData("file", file.getName(), requestFile);
//
//                        capstoneRepository = new CapstoneRepositoryImpl();
//                        capstoneRepository.postRequestFile(token, multipartBody, new CallBackData<UploadFilePath>() {
//                            @Override
//                            public void onSuccess(UploadFilePath uploadFilePath) {
//                                Toast.makeText(getContext(), "Success", Toast.LENGTH_SHORT).show();
////                                tvUploadSuccess.setText(uploadFilePath.getDbPath());
////                                Bitmap myBitmap = BitmapFactory.decodeFile(file.getAbsolutePath());
////                                imgUploadFile.setImageBitmap(myBitmap);
//                            }
//
//                            @Override
//                            public void onFail(String message) {
//
//                            }
//                        });
//                    }
                    break;
                case PICK_FILE_REQUEST:
                    break;
            }
        }
    }
}
