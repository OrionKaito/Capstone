package workflow.capstone.capstoneproject.utils;

import android.content.Context;
import android.content.SharedPreferences;

import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import java.lang.reflect.Type;

import workflow.capstone.capstoneproject.entities.Profile;

public class DynamicWorkflowSharedPreferences {
    public static void storeJWT(Context context, String key, String value) {
        SharedPreferences preferences = context.getSharedPreferences("AUTHORIZE",Context.MODE_PRIVATE);
        SharedPreferences.Editor editor = preferences.edit();
        editor.putString(key, value);
        editor.commit();
    }

    public static String getStoreJWT(Context context, String key) {
        SharedPreferences preferences = context.getSharedPreferences("AUTHORIZE",Context.MODE_PRIVATE);
        String jwt = preferences.getString(key,"");
        return jwt;
    }

    public static void removeJWT(Context context) {
        SharedPreferences preferences = context.getSharedPreferences("AUTHORIZE",Context.MODE_PRIVATE);
        SharedPreferences.Editor editor = preferences.edit();
        editor.clear();
        editor.commit();
    }

    public static <T> void storeData(Context context, String key, String name, T object) {
        SharedPreferences preferences = context.getSharedPreferences(name, Context.MODE_PRIVATE);
        SharedPreferences.Editor editor = preferences.edit();
        Gson gson = new Gson();
        String json = gson.toJson(object);
        editor.putString(key, json);
        editor.apply();
    }

    public static Profile getStoredData(Context context, String key, String name) {
        SharedPreferences preferences = context.getSharedPreferences(name,Context.MODE_PRIVATE);
        Gson gson = new Gson();
        String json = preferences.getString(key, "");
        Type type = new TypeToken<Profile>() {}.getType();
        Profile obj = gson.fromJson(json, type);
        return obj;
    }
}
