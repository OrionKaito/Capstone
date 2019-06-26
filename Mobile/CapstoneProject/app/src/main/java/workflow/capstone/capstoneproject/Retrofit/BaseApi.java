package workflow.capstone.capstoneproject.retrofit;

import java.io.IOException;
import java.util.concurrent.TimeUnit;

import okhttp3.Interceptor;
import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.Response;
import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;

public class BaseApi {
    private Retrofit retrofitAdapter;

    public Retrofit getRetrofitAdapter() {
        return retrofitAdapter;
    }

    public <T> T getService(Class<T> tClass, String url) {
        if (getRetrofitAdapter() == null) {
            OkHttpClient client = new OkHttpClient.Builder()
                    .connectTimeout(60, TimeUnit.SECONDS)
                    .readTimeout(80, TimeUnit.SECONDS).build();
            retrofitAdapter = new Retrofit.Builder().baseUrl(url)
                    .addConverterFactory(GsonConverterFactory.create()).client(client).build();
        }
        return getRetrofitAdapter().create(tClass);
    }

    public <T> T getServiceWithAuthorization(Class<T> tClass, String url, final String token) {
        if (getRetrofitAdapter() == null) {
            OkHttpClient client = new OkHttpClient.Builder()
                    .addInterceptor(new Interceptor() {
                        @Override
                        public Response intercept(Chain chain) throws IOException {
                            Request newRequest  = chain.request().newBuilder()
                                    .addHeader("Authorization", "Bearer " + token)
                                    .build();
                            return chain.proceed(newRequest);
                        }
                    })
                    .connectTimeout(60, TimeUnit.SECONDS)
                    .readTimeout(80, TimeUnit.SECONDS).build();

            retrofitAdapter = new Retrofit.Builder().baseUrl(url)
                    .addConverterFactory(GsonConverterFactory.create()).client(client).build();
        }
        return getRetrofitAdapter().create(tClass);
    }
}

