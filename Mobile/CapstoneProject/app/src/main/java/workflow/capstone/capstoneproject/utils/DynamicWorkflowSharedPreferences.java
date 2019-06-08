package workflow.capstone.capstoneproject.utils;

import android.content.Context;
import android.content.SharedPreferences;

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

    public static void removeJWT(Context context, String key) {
        SharedPreferences preferences = context.getSharedPreferences("AUTHORIZE",Context.MODE_PRIVATE);
        SharedPreferences.Editor editor = preferences.edit();
        editor.clear();
        editor.commit();
    }
}
