package workflow.capstone.capstoneproject.repository;

import android.content.Context;
import android.util.Log;

import com.google.gson.Gson;
import com.google.gson.JsonSyntaxException;
import com.google.gson.reflect.TypeToken;
import com.kaopiz.kprogresshud.KProgressHUD;

import java.io.IOException;
import java.lang.reflect.Type;
import java.util.List;
import java.util.Map;

import okhttp3.MultipartBody;
import okhttp3.ResponseBody;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import workflow.capstone.capstoneproject.api.LoginModel;
import workflow.capstone.capstoneproject.api.RequestApproveModel;
import workflow.capstone.capstoneproject.api.RequestModel;
import workflow.capstone.capstoneproject.api.UpdateAvatarModel;
import workflow.capstone.capstoneproject.api.UpdateProfileModel;
import workflow.capstone.capstoneproject.entities.HandleRequestForm;
import workflow.capstone.capstoneproject.entities.Login;
import workflow.capstone.capstoneproject.entities.MyRequest;
import workflow.capstone.capstoneproject.entities.MyRequestPaging;
import workflow.capstone.capstoneproject.entities.Profile;
import workflow.capstone.capstoneproject.entities.RequestForm;
import workflow.capstone.capstoneproject.entities.RequestResult;
import workflow.capstone.capstoneproject.entities.RequestToHandle;
import workflow.capstone.capstoneproject.entities.RequestToHandlePaging;
import workflow.capstone.capstoneproject.entities.UserNotification;
import workflow.capstone.capstoneproject.entities.UserNotificationPaging;
import workflow.capstone.capstoneproject.entities.WorkflowTemplatePaging;
import workflow.capstone.capstoneproject.retrofit.ClientApi;
import workflow.capstone.capstoneproject.utils.CallBackData;
import workflow.capstone.capstoneproject.utils.KProgressHUDManager;

public class CapstoneRepositoryImpl implements CapstoneRepository {
    ClientApi clientApi = new ClientApi();

    @Override
    public void login(final Context context, Map<String, String> fields, final CallBackData<Login> callBackData) {
        Call<ResponseBody> serviceCall = clientApi.getDynamicWorkflowServices().login(fields);
        Log.e("URL=", clientApi.getDynamicWorkflowServices().login(fields).request().url().toString());
        //show progress bar
        final KProgressHUD progressHUD = KProgressHUDManager.showProgressBar(context);

        serviceCall.enqueue(new Callback<ResponseBody>() {
            @Override
            public void onResponse(Call<ResponseBody> call, Response<ResponseBody> response) {
                //close progress bar
                progressHUD.dismiss();
                if (response != null && response.body() != null) {
                    if (response.code() == 200) {
                        try {
                            String result = response.body().string();
                            Type type = new TypeToken<Login>() {
                            }.getType();
                            Login responseResult = new Gson().fromJson(result, type);
                            if (responseResult == null) {
                                callBackData.onFail(response.message());
                            }
                            callBackData.onSuccess(responseResult);
                        } catch (JsonSyntaxException jsonError) {
                            jsonError.printStackTrace();
                        } catch (IOException e) {
                            e.printStackTrace();
                        }
                    } else {
                        try {
                            callBackData.onFail(response.errorBody().string());
                        } catch (IOException e) {
                            e.printStackTrace();
                        }
                    }
                } else if (response.code() == 400) {
                    try {
                        callBackData.onFail(response.errorBody().string());
                    } catch (IOException e) {
                        e.printStackTrace();
                    }
//                    callBackData.onFail(response.message());
                }
            }

            @Override
            public void onFailure(Call<ResponseBody> call, Throwable t) {
                callBackData.onFail(t.getMessage());
            }
        });
    }

    @Override
    public void newLogin(Context context, LoginModel loginModel, final CallBackData<Login> callBackData) {
        Call<ResponseBody> serviceCall = clientApi.getDynamicWorkflowServices().newLogin(loginModel);
        Log.e("URL=", clientApi.getDynamicWorkflowServices().newLogin(loginModel).request().url().toString());
        //show progress bar
        final KProgressHUD progressHUD = KProgressHUDManager.showProgressBar(context);

        serviceCall.enqueue(new Callback<ResponseBody>() {
            @Override
            public void onResponse(Call<ResponseBody> call, Response<ResponseBody> response) {
                //close progress bar
                progressHUD.dismiss();
                if (response != null && response.body() != null) {
                    if (response.code() == 200) {
                        try {
                            String result = response.body().string();
                            Type type = new TypeToken<Login>() {
                            }.getType();
                            Login responseResult = new Gson().fromJson(result, type);
                            if (responseResult == null) {
                                callBackData.onFail(response.message());
                            }
                            callBackData.onSuccess(responseResult);
                        } catch (JsonSyntaxException jsonError) {
                            jsonError.printStackTrace();
                        } catch (IOException e) {
                            e.printStackTrace();
                        }
                    } else {
                        try {
                            callBackData.onFail(response.errorBody().string());
                        } catch (IOException e) {
                            e.printStackTrace();
                        }
                    }
                } else if (response.code() == 400) {
                    try {
                        callBackData.onFail(response.errorBody().string());
                    } catch (IOException e) {
                        e.printStackTrace();
                    }
//                    callBackData.onFail(response.message());
                }
            }

            @Override
            public void onFailure(Call<ResponseBody> call, Throwable t) {
                callBackData.onFail(t.getMessage());
            }
        });
    }

    @Override
    public void logout(String token, String deviceToken, final CallBackData<String> callBackData) {
        Call<ResponseBody> serviceCall = clientApi.getDWServices(token).logout(deviceToken);
        Log.e("URL=", clientApi.getDWServices(token).logout(deviceToken).request().url().toString());

        serviceCall.enqueue(new Callback<ResponseBody>() {
            @Override
            public void onResponse(Call<ResponseBody> call, Response<ResponseBody> response) {
                if (response != null && response.body() != null) {
                    if (response.code() == 201) {
                        try {
                            String result = response.body().string();
                            String responseResult = result;
                            if (responseResult == null) {
                                callBackData.onFail(response.message());
                            }
                            callBackData.onSuccess(responseResult);
                        } catch (JsonSyntaxException jsonError) {
                            jsonError.printStackTrace();
                        } catch (IOException e) {
                            e.printStackTrace();
                        }
                    } else {
                        try {
                            callBackData.onFail(response.errorBody().string());
                        } catch (IOException e) {
                            e.printStackTrace();
                        }
                    }
                } else if (response.code() == 400) {
                    try {
                        callBackData.onFail(response.errorBody().string());
                    } catch (IOException e) {
                        e.printStackTrace();
                    }
//                    callBackData.onFail(response.message());
                }
            }

            @Override
            public void onFailure(Call<ResponseBody> call, Throwable t) {
                callBackData.onFail(t.getMessage());
            }
        });
    }

    @Override
    public void getProfile(String token, final CallBackData<List<Profile>> callBackData) {
        Call<ResponseBody> serviceCall = clientApi.getDWServices(token).getProfile();
        Log.e("URL=", clientApi.getDWServices(token).getProfile().request().url().toString());
        serviceCall.enqueue(new Callback<ResponseBody>() {
            @Override
            public void onResponse(Call<ResponseBody> call, Response<ResponseBody> response) {
                if (response != null && response.body() != null) {
                    if (response.code() == 200) {
                        try {
                            String result = response.body().string();
                            Type type = new TypeToken<List<Profile>>() {
                            }.getType();
                            List<Profile> responseResult = new Gson().fromJson(result, type);
                            if (responseResult == null) {
                                callBackData.onFail(response.message());
                            }
                            callBackData.onSuccess(responseResult);
                        } catch (JsonSyntaxException jsonError) {
                            jsonError.printStackTrace();
                        } catch (IOException e) {
                            e.printStackTrace();
                        }
                    } else {
                        callBackData.onFail(response.message());
                    }
                } else if (response.code() == 400) {
                    callBackData.onFail(response.message());
                }
            }

            @Override
            public void onFailure(Call<ResponseBody> call, Throwable t) {
                callBackData.onFail(call.toString());
            }
        });
    }

    @Override
    public void updateProfile(Context context, String token, UpdateProfileModel model, final CallBackData<String> callBackData) {
        Call<ResponseBody> serviceCall = clientApi.getDWServices(token).updateProfile(model);
        Log.e("URL=", clientApi.getDWServices(token).updateProfile(model).request().url().toString());
        final KProgressHUD progressHUD = KProgressHUDManager.showProgressBar(context);

        serviceCall.enqueue(new Callback<ResponseBody>() {
            @Override
            public void onResponse(Call<ResponseBody> call, Response<ResponseBody> response) {
                progressHUD.dismiss();
                if (response != null && response.body() != null) {
                    if (response.code() == 200) {
                        try {
                            String result = response.body().string();
                            Type type = new TypeToken<String>() {
                            }.getType();
                            String responseResult = new Gson().fromJson(result, type);
                            if (responseResult == null) {
                                callBackData.onFail(response.message());
                            }
                            callBackData.onSuccess(responseResult);
                        } catch (JsonSyntaxException jsonError) {
                            jsonError.printStackTrace();
                        } catch (IOException e) {
                            e.printStackTrace();
                        }
                    } else {
                        callBackData.onFail(response.message());
                    }
                } else if (response.code() == 400) {
                    callBackData.onFail(response.message());
                }
            }

            @Override
            public void onFailure(Call<ResponseBody> call, Throwable t) {
                callBackData.onFail(call.toString());
            }
        });
    }

    @Override
    public void updateAvatar(Context context, String token, UpdateAvatarModel updateAvatarModel, final CallBackData<String> callBackData) {
        Call<ResponseBody> serviceCall = clientApi.getDWServices(token).updateAvatar(updateAvatarModel);
        Log.e("URL=", clientApi.getDWServices(token).updateAvatar(updateAvatarModel).request().url().toString());
        final KProgressHUD progressHUD = KProgressHUDManager.showProgressBar(context);
        serviceCall.enqueue(new Callback<ResponseBody>() {
            @Override
            public void onResponse(Call<ResponseBody> call, Response<ResponseBody> response) {
                progressHUD.dismiss();
                if (response != null && response.body() != null) {
                    if (response.code() == 200) {
                        try {
                            String result = response.body().string();
                            Type type = new TypeToken<String>() {
                            }.getType();
                            String responseResult = new Gson().fromJson(result, type);
                            if (responseResult == null) {
                                callBackData.onFail(response.message());
                            }
                            callBackData.onSuccess(responseResult);
                        } catch (JsonSyntaxException jsonError) {
                            jsonError.printStackTrace();
                        } catch (IOException e) {
                            e.printStackTrace();
                        }
                    } else {
                        callBackData.onFail(response.message());
                    }
                } else if (response.code() == 400) {
                    callBackData.onFail(response.message());
                }
            }

            @Override
            public void onFailure(Call<ResponseBody> call, Throwable t) {
                callBackData.onFail(call.toString());
            }
        });
    }

    @Override
    public void changePassword(Context context, String token, String oldPassword, String newPassword, final CallBackData<String> callBackData) {
        Call<ResponseBody> serviceCall = clientApi.getDWServices(token).changePassword(oldPassword, newPassword);
        Log.e("URL=", clientApi.getDWServices(token).changePassword(oldPassword, newPassword).request().url().toString());
        final KProgressHUD progressHUD = KProgressHUDManager.showProgressBar(context);

        serviceCall.enqueue(new Callback<ResponseBody>() {
            @Override
            public void onResponse(Call<ResponseBody> call, Response<ResponseBody> response) {
                progressHUD.dismiss();
                if (response != null && response.body() != null) {
                    if (response.code() == 200) {
                        try {
                            String result = response.body().string();
                            Type type = new TypeToken<String>() {
                            }.getType();
                            String responseResult = new Gson().fromJson(result, type);
                            if (responseResult == null) {
                                callBackData.onFail(response.message());
                            }
                            callBackData.onSuccess(responseResult);
                        } catch (JsonSyntaxException jsonError) {
                            jsonError.printStackTrace();
                        } catch (IOException e) {
                            e.printStackTrace();
                        }
                    } else {
                        callBackData.onFail(response.message());
                    }
                } else if (response.code() == 400) {
                    callBackData.onFail(response.message());
                }
            }

            @Override
            public void onFailure(Call<ResponseBody> call, Throwable t) {
                callBackData.onFail(call.toString());
            }
        });
    }

    @Override
    public void verifyAccount(Context context, String code, String email, final CallBackData<String> callBackData) {
        Call<ResponseBody> serviceCall = clientApi.getDynamicWorkflowServices().verifyAccount(code, email);
        Log.e("URL=", clientApi.getDynamicWorkflowServices().verifyAccount(code, email).request().url().toString());
        final KProgressHUD progressHUD = KProgressHUDManager.showProgressBar(context);

        serviceCall.enqueue(new Callback<ResponseBody>() {
            @Override
            public void onResponse(Call<ResponseBody> call, Response<ResponseBody> response) {
                progressHUD.dismiss();
                if (response != null && response.body() != null) {
                    if (response.code() == 200) {
                        try {
                            String result = response.body().string();
                            Type type = new TypeToken<String>() {
                            }.getType();
                            String responseResult = new Gson().fromJson(result, type);
                            if (responseResult == null) {
                                callBackData.onFail(response.message());
                            }
                            callBackData.onSuccess(responseResult);
                        } catch (JsonSyntaxException jsonError) {
                            jsonError.printStackTrace();
                        } catch (IOException e) {
                            e.printStackTrace();
                        }
                    } else {
                        callBackData.onFail(response.message());
                    }
                } else if (response.code() == 400) {
                    try {
                        callBackData.onFail(response.errorBody().string());
                    } catch (IOException e) {
                        e.printStackTrace();
                    }
                }
            }

            @Override
            public void onFailure(Call<ResponseBody> call, Throwable t) {
                callBackData.onFail(t.getMessage());
            }
        });
    }

    @Override
    public void getWorkflow(String token, int numberOfPage, int numberOfRecord, final CallBackData<WorkflowTemplatePaging> callBackData) {
        Call<ResponseBody> serviceCall = clientApi.getDWServices(token).getWorkflow(numberOfPage, numberOfRecord);
        Log.e("URL=", clientApi.getDWServices(token).getWorkflow(numberOfPage, numberOfRecord).request().url().toString());
        serviceCall.enqueue(new Callback<ResponseBody>() {
            @Override
            public void onResponse(Call<ResponseBody> call, Response<ResponseBody> response) {
                if (response != null && response.body() != null) {
                    if (response.code() == 200) {
                        try {
                            String result = response.body().string();
                            Type type = new TypeToken<WorkflowTemplatePaging>() {
                            }.getType();
                            WorkflowTemplatePaging responseResult = new Gson().fromJson(result, type);
                            if (responseResult == null) {
                                callBackData.onFail(response.message());
                            }
                            callBackData.onSuccess(responseResult);
                        } catch (JsonSyntaxException jsonError) {
                            jsonError.printStackTrace();
                        } catch (IOException e) {
                            e.printStackTrace();
                        }
                    } else {
                        callBackData.onFail(response.message());
                    }
                } else if (response.code() == 400) {
                    callBackData.onFail(response.message());
                }
            }

            @Override
            public void onFailure(Call<ResponseBody> call, Throwable t) {
                callBackData.onFail(call.toString());
            }
        });
    }

    @Override
    public void getNumberOfNotification(String token, final CallBackData<String> callBackData) {
        Call<ResponseBody> serviceCall = clientApi.getDWServices(token).getNumberNotification();
        Log.e("URL=", clientApi.getDWServices(token).getNumberNotification().request().url().toString());
        serviceCall.enqueue(new Callback<ResponseBody>() {
            @Override
            public void onResponse(Call<ResponseBody> call, Response<ResponseBody> response) {
                if (response != null && response.body() != null) {
                    if (response.code() == 200) {
                        try {
                            String result = response.body().string();
                            Type type = new TypeToken<String>() {
                            }.getType();
                            String responseResult = new Gson().fromJson(result, type);
                            if (responseResult == null) {
                                callBackData.onFail(response.message());
                            }
                            callBackData.onSuccess(responseResult);
                        } catch (JsonSyntaxException jsonError) {
                            jsonError.printStackTrace();
                        } catch (IOException e) {
                            e.printStackTrace();
                        }
                    } else {
                        callBackData.onFail(response.message());
                    }
                } else if (response.code() == 400) {
                    callBackData.onFail(response.message());
                }
            }

            @Override
            public void onFailure(Call<ResponseBody> call, Throwable t) {
                callBackData.onFail(call.toString());
            }
        });
    }

    @Override
    public void getNotification(String token, int numberOfPage, int numberOfRecord, final CallBackData<UserNotificationPaging> callBackData) {
        Call<ResponseBody> serviceCall = clientApi.getDWServices(token).getNotification(numberOfPage, numberOfRecord);
        Log.e("URL=", clientApi.getDWServices(token).getNotification(numberOfPage, numberOfRecord).request().url().toString());
        serviceCall.enqueue(new Callback<ResponseBody>() {
            @Override
            public void onResponse(Call<ResponseBody> call, Response<ResponseBody> response) {
                if (response != null && response.body() != null) {
                    if (response.code() == 200) {
                        try {
                            String result = response.body().string();
                            Type type = new TypeToken<UserNotificationPaging>() {
                            }.getType();
                            UserNotificationPaging responseResult = new Gson().fromJson(result, type);
                            if (responseResult == null) {
                                callBackData.onFail(response.message());
                            }
                            callBackData.onSuccess(responseResult);
                        } catch (JsonSyntaxException jsonError) {
                            jsonError.printStackTrace();
                        } catch (IOException e) {
                            e.printStackTrace();
                        }
                    } else {
                        callBackData.onFail(response.message());
                    }
                } else if (response.code() == 400) {
                    callBackData.onFail(response.message());
                }
            }

            @Override
            public void onFailure(Call<ResponseBody> call, Throwable t) {
                callBackData.onFail(call.toString());
            }
        });
    }

    @Override
    public void getRequestsToHandleByPermission(String token, int numberOfPage, int numberOfRecord, final CallBackData<RequestToHandlePaging> callBackData) {
        Call<ResponseBody> serviceCall = clientApi.getDWServices(token).getRequestsToHandleByPermission(numberOfPage, numberOfRecord);
        Log.e("URL=", clientApi.getDWServices(token).getRequestsToHandleByPermission(numberOfPage, numberOfRecord).request().url().toString());
        serviceCall.enqueue(new Callback<ResponseBody>() {
            @Override
            public void onResponse(Call<ResponseBody> call, Response<ResponseBody> response) {
                if (response != null && response.body() != null) {
                    if (response.code() == 200) {
                        try {
                            String result = response.body().string();
                            Type type = new TypeToken<RequestToHandlePaging>() {
                            }.getType();
                            RequestToHandlePaging responseResult = new Gson().fromJson(result, type);
                            if (responseResult == null) {
                                callBackData.onFail(response.message());
                            }
                            callBackData.onSuccess(responseResult);
                        } catch (JsonSyntaxException jsonError) {
                            jsonError.printStackTrace();
                        } catch (IOException e) {
                            e.printStackTrace();
                        }
                    } else {
                        callBackData.onFail(response.message());
                    }
                } else if (response.code() == 400) {
                    callBackData.onFail(response.message());
                }
            }

            @Override
            public void onFailure(Call<ResponseBody> call, Throwable t) {
                callBackData.onFail(call.toString());
            }
        });
    }

    @Override
    public void getMyRequest(String token, int numberOfPage, int numberOfRecord, final CallBackData<MyRequestPaging> callBackData) {
        Call<ResponseBody> serviceCall = clientApi.getDWServices(token).getMyRequest(numberOfPage, numberOfRecord);
        Log.e("URL=", clientApi.getDWServices(token).getMyRequest(numberOfPage, numberOfRecord).request().url().toString());
        serviceCall.enqueue(new Callback<ResponseBody>() {
            @Override
            public void onResponse(Call<ResponseBody> call, Response<ResponseBody> response) {
                if (response != null && response.body() != null) {
                    if (response.code() == 200) {
                        try {
                            String result = response.body().string();
                            Type type = new TypeToken<MyRequestPaging>() {
                            }.getType();
                            MyRequestPaging responseResult = new Gson().fromJson(result, type);
                            if (responseResult == null) {
                                callBackData.onFail(response.message());
                            }
                            callBackData.onSuccess(responseResult);
                        } catch (JsonSyntaxException jsonError) {
                            jsonError.printStackTrace();
                        } catch (IOException e) {
                            e.printStackTrace();
                        }
                    } else {
                        callBackData.onFail(response.message());
                    }
                } else if (response.code() == 400) {
                    callBackData.onFail(response.message());
                }
            }

            @Override
            public void onFailure(Call<ResponseBody> call, Throwable t) {
                callBackData.onFail(call.toString());
            }
        });
    }

    @Override
    public void postRequest(String token, RequestModel requestModel, final CallBackData<String> callBackData) {
        Call<ResponseBody> serviceCall = clientApi.getDWServices(token).postRequest(requestModel);
        Log.e("URL=", clientApi.getDWServices(token).postRequest(requestModel).request().url().toString());
        serviceCall.enqueue(new Callback<ResponseBody>() {
            @Override
            public void onResponse(Call<ResponseBody> call, Response<ResponseBody> response) {
                if (response != null && response.body() != null) {
                    if (response.code() == 201) {
                        try {
                            String result = response.body().string();
                            Type type = new TypeToken<String>() {
                            }.getType();
                            String responseResult = new Gson().fromJson(result, type);
                            if (responseResult == null) {
                                callBackData.onFail(response.message());
                            }
                            callBackData.onSuccess(responseResult);
                        } catch (JsonSyntaxException jsonError) {
                            jsonError.printStackTrace();
                        } catch (IOException e) {
                            e.printStackTrace();
                        }
                    } else {
                        callBackData.onFail(response.message());
                    }
                } else if (response.code() == 400) {
                    callBackData.onFail(response.message());
                }
            }

            @Override
            public void onFailure(Call<ResponseBody> call, Throwable t) {
                callBackData.onFail(call.toString());
            }
        });
    }

    @Override
    public void postRequestFile(String token, MultipartBody.Part file, final CallBackData<String[]> callBackData) {
        Call<ResponseBody> serviceCall = clientApi.getDWServices(token).upFile(file);
        Log.e("URL=", clientApi.getDWServices(token).upFile(file).request().url().toString());
        serviceCall.enqueue(new Callback<ResponseBody>() {
            @Override
            public void onResponse(Call<ResponseBody> call, Response<ResponseBody> response) {
                if (response != null && response.body() != null) {
                    if (response.code() == 200) {
                        try {
                            String result = response.body().string();
                            Type type = new TypeToken<String[]>() {
                            }.getType();
                            String[] responseResult = new Gson().fromJson(result, type);
                            if (responseResult == null) {
                                callBackData.onFail(response.message());
                            }
                            callBackData.onSuccess(responseResult);
                        } catch (JsonSyntaxException jsonError) {
                            jsonError.printStackTrace();
                        } catch (IOException e) {
                            e.printStackTrace();
                        }
                    } else {
                        callBackData.onFail(response.message());
                    }
                } else if (response.code() == 400) {
                    callBackData.onFail(response.message());
                }
            }

            @Override
            public void onFailure(Call<ResponseBody> call, Throwable t) {
                callBackData.onFail(call.toString());
            }
        });
    }

    @Override
    public void postMultipleRequestFile(String token, MultipartBody.Part[] files, final CallBackData<String[]> callBackData) {
        Call<ResponseBody> serviceCall = clientApi.getDWServices(token).uploadMultipleFile(files);
        Log.e("URL=", clientApi.getDWServices(token).uploadMultipleFile(files).request().url().toString());
        serviceCall.enqueue(new Callback<ResponseBody>() {
            @Override
            public void onResponse(Call<ResponseBody> call, Response<ResponseBody> response) {
                if (response != null && response.body() != null) {
                    if (response.code() == 200) {
                        try {
                            String result = response.body().string();
                            Type type = new TypeToken<String[]>() {
                            }.getType();
                            String[] responseResult = new Gson().fromJson(result, type);
                            if (responseResult == null) {
                                callBackData.onFail(response.message());
                            }
                            callBackData.onSuccess(responseResult);
                        } catch (JsonSyntaxException jsonError) {
                            jsonError.printStackTrace();
                        } catch (IOException e) {
                            e.printStackTrace();
                        }
                    } else {
                        callBackData.onFail(response.message());
                    }
                } else if (response.code() == 400) {
                    callBackData.onFail(response.message());
                }
            }

            @Override
            public void onFailure(Call<ResponseBody> call, Throwable t) {
                callBackData.onFail(call.toString());
            }
        });
    }

    @Override
    public void getRequestResult(String token, String requestActionID, String userNotificationID, final CallBackData<RequestResult> callBackData) {
        Call<ResponseBody> serviceCall = clientApi.getDWServices(token).getRequestResult(requestActionID, userNotificationID);
        Log.e("URL=", clientApi.getDWServices(token).getRequestResult(requestActionID, userNotificationID).request().url().toString());
        serviceCall.enqueue(new Callback<ResponseBody>() {
            @Override
            public void onResponse(Call<ResponseBody> call, Response<ResponseBody> response) {
                if (response != null && response.body() != null) {
                    if (response.code() == 200) {
                        try {
                            String result = response.body().string();
                            Type type = new TypeToken<RequestResult>() {
                            }.getType();
                            RequestResult responseResult = new Gson().fromJson(result, type);
                            if (responseResult == null) {
                                callBackData.onFail(response.message());
                            }
                            callBackData.onSuccess(responseResult);
                        } catch (JsonSyntaxException jsonError) {
                            jsonError.printStackTrace();
                        } catch (IOException e) {
                            e.printStackTrace();
                        }
                    } else {
                        callBackData.onFail(response.message());
                    }
                } else if (response.code() == 400) {
                    callBackData.onFail(response.message());
                }
            }

            @Override
            public void onFailure(Call<ResponseBody> call, Throwable t) {
                callBackData.onFail(call.toString());
            }
        });
    }

    @Override
    public void getRequestForm(String token, String workflowTemplateID, final CallBackData<RequestForm> callBackData) {
        Call<ResponseBody> serviceCall = clientApi.getDWServices(token).getRequestForm(workflowTemplateID);
        Log.e("URL=", clientApi.getDWServices(token).getRequestForm(workflowTemplateID).request().url().toString());
        serviceCall.enqueue(new Callback<ResponseBody>() {
            @Override
            public void onResponse(Call<ResponseBody> call, Response<ResponseBody> response) {
                if (response != null && response.body() != null) {
                    if (response.code() == 200) {
                        try {
                            String result = response.body().string();
                            Type type = new TypeToken<RequestForm>() {
                            }.getType();
                            RequestForm responseResult = new Gson().fromJson(result, type);
                            if (responseResult == null) {
                                callBackData.onFail(response.message());
                            }
                            callBackData.onSuccess(responseResult);
                        } catch (JsonSyntaxException jsonError) {
                            jsonError.printStackTrace();
                        } catch (IOException e) {
                            e.printStackTrace();
                        }
                    } else {
                        callBackData.onFail(response.message());
                    }
                } else if (response.code() == 400) {
                    callBackData.onFail(response.message());
                }
            }

            @Override
            public void onFailure(Call<ResponseBody> call, Throwable t) {
                callBackData.onFail(call.toString());
            }
        });
    }

    @Override
    public void getRequestHandleForm(String token, String requestActionID, final CallBackData<HandleRequestForm> callBackData) {
        Call<ResponseBody> serviceCall = clientApi.getDWServices(token).getRequestHandleForm(requestActionID);
        Log.e("URL=", clientApi.getDWServices(token).getRequestHandleForm(requestActionID).request().url().toString());
        serviceCall.enqueue(new Callback<ResponseBody>() {
            @Override
            public void onResponse(Call<ResponseBody> call, Response<ResponseBody> response) {
                if (response != null && response.body() != null) {
                    if (response.code() == 200) {
                        try {
                            String result = response.body().string();
                            Type type = new TypeToken<HandleRequestForm>() {
                            }.getType();
                            HandleRequestForm responseResult = new Gson().fromJson(result, type);
                            if (responseResult == null) {
                                callBackData.onFail(response.message());
                            }
                            callBackData.onSuccess(responseResult);
                        } catch (JsonSyntaxException jsonError) {
                            jsonError.printStackTrace();
                        } catch (IOException e) {
                            e.printStackTrace();
                        }
                    } else {
                        callBackData.onFail(response.message());
                    }
                } else if (response.code() == 400) {
                    callBackData.onFail(response.message());
                }
            }

            @Override
            public void onFailure(Call<ResponseBody> call, Throwable t) {
                callBackData.onFail(call.toString());
            }
        });
    }

    @Override
    public void approveRequest(String token, RequestApproveModel requestApproveModel, final CallBackData<String> callBackData) {
        Call<ResponseBody> serviceCall = clientApi.getDWServices(token).approveRequest(requestApproveModel);
        Log.e("URL=", clientApi.getDWServices(token).approveRequest(requestApproveModel).request().url().toString());
        serviceCall.enqueue(new Callback<ResponseBody>() {
            @Override
            public void onResponse(Call<ResponseBody> call, Response<ResponseBody> response) {
                if (response != null && response.body() != null) {
                    if (response.code() == 201) {
                        try {
                            String result = response.body().string();
                            Type type = new TypeToken<String>() {
                            }.getType();
                            String responseResult = new Gson().fromJson(result, type);
                            if (responseResult == null) {
                                callBackData.onFail(response.message());
                            }
                            callBackData.onSuccess(responseResult);
                        } catch (JsonSyntaxException jsonError) {
                            jsonError.printStackTrace();
                        } catch (IOException e) {
                            e.printStackTrace();
                        }
                    } else {
                        callBackData.onFail(response.message());
                    }
                } else if (response.code() == 400) {
                    try {
                        callBackData.onFail(response.errorBody().string());
                    } catch (IOException e) {
                        e.printStackTrace();
                    }
                }
            }

            @Override
            public void onFailure(Call<ResponseBody> call, Throwable t) {
                callBackData.onFail(call.toString());
            }
        });
    }

    @Override
    public void forgotPassword(Context context, String email, final CallBackData<String> callBackData) {
        Call<ResponseBody> serviceCall = clientApi.getDynamicWorkflowServices().forgotPassword(email);
        Log.e("URL=", clientApi.getDynamicWorkflowServices().forgotPassword(email).request().url().toString());

        final KProgressHUD progressHUD = KProgressHUDManager.showProgressBar(context);
        serviceCall.enqueue(new Callback<ResponseBody>() {
            @Override
            public void onResponse(Call<ResponseBody> call, Response<ResponseBody> response) {
                progressHUD.dismiss();
                if (response != null && response.body() != null) {
                    if (response.code() == 200) {
                        try {
                            String result = response.body().string();
                            Type type = new TypeToken<String>() {
                            }.getType();
                            String responseResult = new Gson().fromJson(result, type);
                            if (responseResult == null) {
                                callBackData.onFail(response.message());
                            }
                            callBackData.onSuccess(responseResult);
                        } catch (JsonSyntaxException jsonError) {
                            jsonError.printStackTrace();
                        } catch (IOException e) {
                            e.printStackTrace();
                        }
                    } else {
                        callBackData.onFail(response.message());
                    }
                } else if (response.code() == 400) {
                    callBackData.onFail(response.message());
                }
            }

            @Override
            public void onFailure(Call<ResponseBody> call, Throwable t) {
                callBackData.onFail(call.toString());
            }
        });
    }

    @Override
    public void confirmForgotPassword(Context context, String code, String email, String newPassword, final CallBackData<String> callBackData) {
        Call<ResponseBody> serviceCall = clientApi.getDynamicWorkflowServices().confirmForgotPassword(code, email, newPassword);
        Log.e("URL=", clientApi.getDynamicWorkflowServices().confirmForgotPassword(code, email, newPassword).request().url().toString());
        final KProgressHUD progressHUD = KProgressHUDManager.showProgressBar(context);

        serviceCall.enqueue(new Callback<ResponseBody>() {
            @Override
            public void onResponse(Call<ResponseBody> call, Response<ResponseBody> response) {
                progressHUD.dismiss();
                if (response != null && response.body() != null) {
                    if (response.code() == 200) {
                        try {
                            String result = response.body().string();
                            Type type = new TypeToken<String>() {
                            }.getType();
                            String responseResult = new Gson().fromJson(result, type);
                            if (responseResult == null) {
                                callBackData.onFail(response.message());
                            }
                            callBackData.onSuccess(responseResult);
                        } catch (JsonSyntaxException jsonError) {
                            jsonError.printStackTrace();
                        } catch (IOException e) {
                            e.printStackTrace();
                        }
                    } else {
                        callBackData.onFail(response.message());
                    }
                } else if (response.code() == 400) {
                    try {
                        callBackData.onFail(response.errorBody().string());
                    } catch (IOException e) {
                        e.printStackTrace();
                    }
                }
            }

            @Override
            public void onFailure(Call<ResponseBody> call, Throwable t) {
                callBackData.onFail(t.getMessage());
            }
        });
    }
}
