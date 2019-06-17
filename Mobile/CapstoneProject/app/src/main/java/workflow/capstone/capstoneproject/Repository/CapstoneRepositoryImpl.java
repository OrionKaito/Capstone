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

import okhttp3.ResponseBody;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import workflow.capstone.capstoneproject.api.RequestAPI;
import workflow.capstone.capstoneproject.api.RequestActionAPI;
import workflow.capstone.capstoneproject.api.RequestValueAPI;
import workflow.capstone.capstoneproject.entities.DynamicForm.DynamicForm;
import workflow.capstone.capstoneproject.entities.Notification;
import workflow.capstone.capstoneproject.entities.Profile;
import workflow.capstone.capstoneproject.entities.RequestAction;
import workflow.capstone.capstoneproject.entities.WorkflowTemplate;
import workflow.capstone.capstoneproject.retrofit.ClientApi;
import workflow.capstone.capstoneproject.utils.CallBackData;
import workflow.capstone.capstoneproject.utils.KProgressHUDManager;

public class CapstoneRepositoryImpl implements CapstoneRepository {
    ClientApi clientApi = new ClientApi();

    @Override
    public void login(final Context context, Map<String, String> fields, final CallBackData<String> callBackData) {
        Call<ResponseBody> serviceCall = clientApi.getDynamicWorkflowServices().login(fields);
        Log.e("URL=", clientApi.getDynamicWorkflowServices().login(fields).request().url().toString());
        //show progress bar
        final KProgressHUD khub = KProgressHUDManager.showProgressBar(context);

        serviceCall.enqueue(new Callback<ResponseBody>() {
            @Override
            public void onResponse(Call<ResponseBody> call, Response<ResponseBody> response) {

                //close progress bar
                KProgressHUDManager.dismiss(context, khub);
                if (response != null && response.body() != null) {
                    if (response.code() == 200) {
                        try {
                            String result = response.body().string();
                            Type type = new TypeToken<String>() {}.getType();
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
                        try {
                            callBackData.onFail(response.errorBody().string());
                        } catch (IOException e) {
                            e.printStackTrace();
                        }
                    }
                } else if(response.code() == 400) {
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
    public void getProfile(String token, final CallBackData<Profile> callBackData) {
        Call<ResponseBody> serviceCall = clientApi.getDWServices(token).getProfile();
        Log.e("URL=", clientApi.getDWServices(token).getProfile().request().url().toString());
        serviceCall.enqueue(new Callback<ResponseBody>() {
            @Override
            public void onResponse(Call<ResponseBody> call, Response<ResponseBody> response) {
                if (response != null && response.body() != null) {
                    if (response.code() == 200) {
                        try {
                            String result = response.body().string();
                            Type type = new TypeToken<Profile>() {
                            }.getType();
                            Profile responseResult = new Gson().fromJson(result, type);
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
                } else if(response.code() == 400) {
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
    public void getWorkflows(final CallBackData<List<WorkflowTemplate>> callBackData) {
        Call<ResponseBody> serviceCall = clientApi.getDynamicWorkflowServices().getWorkflows();
        Log.e("URL=", clientApi.getDynamicWorkflowServices().getWorkflows().request().url().toString());
        serviceCall.enqueue(new Callback<ResponseBody>() {
            @Override
            public void onResponse(Call<ResponseBody> call, Response<ResponseBody> response) {
                if (response != null && response.body() != null) {
                    if (response.code() == 200) {
                        try {
                            String result = response.body().string();
                            Type type = new TypeToken<List<WorkflowTemplate>>() {
                            }.getType();
                            List<WorkflowTemplate> responseResult = new Gson().fromJson(result, type);
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
                } else if(response.code() == 400) {
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
                } else if(response.code() == 400) {
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
    public void getNotification(String token, final CallBackData<List<Notification>> callBackData) {
        Call<ResponseBody> serviceCall = clientApi.getDWServices(token).getNotification();
        Log.e("URL=", clientApi.getDWServices(token).getNotification().request().url().toString());
        serviceCall.enqueue(new Callback<ResponseBody>() {
            @Override
            public void onResponse(Call<ResponseBody> call, Response<ResponseBody> response) {
                if (response != null && response.body() != null) {
                    if (response.code() == 200) {
                        try {
                            String result = response.body().string();
                            Type type = new TypeToken<List<Notification>>() {
                            }.getType();
                            List<Notification> responseResult = new Gson().fromJson(result, type);
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
                } else if(response.code() == 400) {
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
    public void postRequest(String description, String workFlowTemplateID, String token, final CallBackData<String> callBackData) {
        RequestAPI requestAPI = new RequestAPI(description, workFlowTemplateID);
        Call<ResponseBody> serviceCall = clientApi.getDWServices(token).postRequest(requestAPI);
        Log.e("URL=", clientApi.getDWServices(token).postRequest(requestAPI).request().url().toString());
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
                } else if(response.code() == 400) {
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
    public void postRequestAction(RequestAction requestAction, String token, final CallBackData<String> callBackData) {
        RequestActionAPI requestActionAPI = new RequestActionAPI(requestAction.getStatus(), requestAction.getRequestID(), requestAction.getNextStepID());
        Call<ResponseBody> serviceCall = clientApi.getDWServices(token).postRequestAction(requestActionAPI);
        Log.e("URL=", clientApi.getDWServices(token).postRequestAction(requestActionAPI).request().url().toString());
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
                } else if(response.code() == 400) {
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
    public void postRequestValue(String data, String requestActionID, String token, final CallBackData<String> callBackData) {
        RequestValueAPI requestValueAPI = new RequestValueAPI(data, requestActionID);
        Call<ResponseBody> serviceCall = clientApi.getDWServices(token).postRequestValue(requestValueAPI);
        Log.e("URL=", clientApi.getDWServices(token).postRequestValue(requestValueAPI).request().url().toString());
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
                } else if(response.code() == 400) {
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
    public void getForm(final CallBackData<DynamicForm> callBackData) {
        Call<ResponseBody> serviceCall = clientApi.getDynamicWorkflowServices().getForm();
        Log.e("URL=", clientApi.getDynamicWorkflowServices().getForm().request().url().toString());
        serviceCall.enqueue(new Callback<ResponseBody>() {
            @Override
            public void onResponse(Call<ResponseBody> call, Response<ResponseBody> response) {
                if (response != null && response.body() != null) {
                    if (response.code() == 200) {
                        try {
                            String result = response.body().string();
                            Type type = new TypeToken<DynamicForm>() {
                            }.getType();
                            DynamicForm responseResult = new Gson().fromJson(result, type);
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
                } else if(response.code() == 400) {
                    callBackData.onFail(response.message());
                }
            }

            @Override
            public void onFailure(Call<ResponseBody> call, Throwable t) {
                callBackData.onFail(call.toString());
            }
        });
    }
}
